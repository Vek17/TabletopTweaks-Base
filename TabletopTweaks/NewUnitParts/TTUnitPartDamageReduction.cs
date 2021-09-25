﻿using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewComponents;

namespace TabletopTweaks.NewUnitParts
{
    public class TTUnitPartDamageReduction :
        OldStyleUnitPart,
        IUnitApplyDamageReduction,
        IUnitSubscriber,
        ISubscriber
    {
        public List<TTUnitPartDamageReduction.ReductionPenalty> PenaltyEntries = new List<TTUnitPartDamageReduction.ReductionPenalty>();
        private static readonly List<TTUnitPartDamageReduction.Chunk> ChunksForRemove = new List<TTUnitPartDamageReduction.Chunk>();
        [JsonProperty]
        private readonly List<EntityFact> m_SourceFacts = new List<EntityFact>();
        private bool m_Initialized;
        private readonly List<TTUnitPartDamageReduction.Chunk> m_Chunks = new List<TTUnitPartDamageReduction.Chunk>();
        private ChunkStack[] m_ChunkStacks;
        private List<ReductionDisplay> m_ReductionDisplays = new List<ReductionDisplay>();
        private readonly List<TTUnitPartDamageReduction.Immunity> m_Immunities = new List<TTUnitPartDamageReduction.Immunity>();

        public void AddPenaltyEntry(int penalty, EntityFact source) => this.PenaltyEntries.Add(new TTUnitPartDamageReduction.ReductionPenalty()
        {
            Penalty = penalty,
            Source = source
        });

        public void RemovePenaltyEntry(EntityFact source) => this.PenaltyEntries.RemoveAll((Predicate<TTUnitPartDamageReduction.ReductionPenalty>)(p => p.Source == source));

        // I renamed this to CalculatePenalty (it was CalculateReduction, as in Damage Reduction reduction) to avoid confusion.
        public int CalculatePenalty()
        {
            int num = 0;
            List<BlueprintFact> blueprintFactList = new List<BlueprintFact>();
            foreach (TTUnitPartDamageReduction.ReductionPenalty penaltyEntry in this.PenaltyEntries)
            {
                if (!blueprintFactList.Contains(penaltyEntry.Source.Blueprint))
                {
                    num += penaltyEntry.Penalty;
                    blueprintFactList.Add(penaltyEntry.Source.Blueprint);
                }
            }
            return num;
        }

        public IEnumerable<ReductionDisplay> AllSources
        {
            get
            {
                this.TryInitialize();
                return this.m_ReductionDisplays;
            }
        }

        public bool HasAbsolutePhysicalDR
        {
            get
            {
                foreach (TTUnitPartDamageReduction.Chunk chunk in this.m_Chunks)
                {
                    if (chunk.DR.SourceBlueprintComponent is TTAddDamageResistancePhysical blueprintComponent1 && blueprintComponent1.IsAbsolute)
                        return true;
                }
                return false;
            }
        }

        private void RemovePartIfNecessary()
        {
            if (!this.m_Chunks.Empty<TTUnitPartDamageReduction.Chunk>() || !this.m_Immunities.Empty<TTUnitPartDamageReduction.Immunity>())
                return;
            this.Owner.Remove<TTUnitPartDamageReduction>();
        }

        public void Add(EntityFact fact)
        {
            this.TryInitialize();
            if (this.m_SourceFacts.HasItem<EntityFact>(fact))
                return;
            this.m_SourceFacts.Add(fact);
            foreach (BlueprintComponentAndRuntime<TTAddDamageResistanceBase> componentAndRuntime in fact.SelectComponentsWithRuntime<TTAddDamageResistanceBase>())
            {
                this.m_Chunks.Add(new TTUnitPartDamageReduction.Chunk(fact, (TTAddDamageResistanceBase.ComponentRuntime)componentAndRuntime.Runtime));
            }
            this.RecalculateChunkStacks();
        }

        public void Remove(EntityFact fact)
        {
            this.m_SourceFacts.Remove(fact);
            this.m_Chunks.RemoveAll((Predicate<TTUnitPartDamageReduction.Chunk>)(c => c.Source == fact));
            this.RecalculateChunkStacks();
            this.RemovePartIfNecessary();
        }

        public void AddImmunity(EntityFact fact, BlueprintComponent component, DamageEnergyType type)
        {
            if (this.m_Immunities.FirstItem<TTUnitPartDamageReduction.Immunity>((Func<TTUnitPartDamageReduction.Immunity, bool>)(i => i.SourceFact == fact && i.SourceComponent == component)) != null)
                PFLog.Default.Error("UnitPartDamageReduction.AddImmunity: can't add immunity twice {0}.{1}", (object)fact.Blueprint.name, (object)component.name);
            else
                this.m_Immunities.Add(new TTUnitPartDamageReduction.Immunity(fact, component, type));
        }

        public void RemoveImmunity(EntityFact fact, BlueprintComponent component)
        {
            this.m_Immunities.RemoveAll((Predicate<TTUnitPartDamageReduction.Immunity>)(i => i.SourceFact == fact && i.SourceComponent == component));
            this.RemovePartIfNecessary();
        }

        public void ApplyDamageReduction(RuleCalculateDamage evt)
        {
            this.TryInitialize();
            UnitPartClusteredAttack partClusteredAttack = evt.Initiator.Get<UnitPartClusteredAttack>();
            UnitPartClusteredAttack clusteredAttack = partClusteredAttack == null || !partClusteredAttack.IsSuitableForEvent((RulebookTargetEvent)evt) ? (UnitPartClusteredAttack)null : partClusteredAttack;
            foreach (DamageValue damage in evt.CalculatedDamage)
            {
                if (damage.FinalValue >= 1)
                {
                    // Reduction is calculated in three steps; first the best of all high priority resistances/immunities is applied, then the best of the normal
                    // priority ones, and then the best of the low priority ones.
                    // This is used for "spill-over" immunities and resistances. By default, only Protection From Energy has a High priority and only the Abjuration
                    // school's Energy Absorption feature has a Low priority. Everything else should have a Normal priority.
                    ChunkStack bestDRHigh = FindBestDRWithPriority(damage, evt.DamageBundle.Weapon, TTAddDamageResistanceBase.DRPriority.High);
                    ChunkStack bestDRNormal = FindBestDRWithPriority(damage, evt.DamageBundle.Weapon, TTAddDamageResistanceBase.DRPriority.Normal);
                    ChunkStack bestDRLow = FindBestDRWithPriority(damage, evt.DamageBundle.Weapon, TTAddDamageResistanceBase.DRPriority.Low);
                    this.ApplyReduction(damage, evt.DamageBundle.Weapon, clusteredAttack, bestDRHigh, bestDRNormal, bestDRLow);
                }
            }
            this.Cleanup(clusteredAttack);
        }

        private ChunkStack FindBestDRWithPriority(
            DamageValue damage,
            ItemEntityWeapon damageEventWeapon,
            TTAddDamageResistanceBase.DRPriority priority)
        {
            ChunkStack bestDR = null;
            foreach (ChunkStack chunkStack in m_ChunkStacks.Where(cs => cs.Priority == priority))
            {
                // CalculateReduction still returns the remainig pool size in the case of e.g. Protection From Energy. However, the "best" DR is now calculated
                // such that full immunity is always considered better than pool-based immunity, which is always considered better than "normal" resistance.
                // If there are no valid (pool-based) immunities, then Reduction is used to determine best resistance (as it is in vanilla)
                if (
                    (bestDR == null && (chunkStack.IsImmunity || chunkStack.IsImmunityPool || chunkStack.Reduction > 0))
                    || (bestDR != null && (!bestDR.IsImmunity && chunkStack.IsImmunity))
                    || (bestDR != null && (!bestDR.IsImmunity && !bestDR.IsImmunityPool && chunkStack.IsImmunityPool))
                    || (bestDR != null && (!bestDR.IsImmunity && !bestDR.IsImmunityPool && chunkStack.Reduction > bestDR.Reduction)))
                {
                    bestDR = chunkStack;
                }
            }
            return bestDR;
        } 

        private void ApplyReduction(
          DamageValue damage,
          ItemEntityWeapon damageEventWeapon,
          [CanBeNull] UnitPartClusteredAttack clusteredAttack,
          [CanBeNull] ChunkStack bestDRHigh,
          [CanBeNull] ChunkStack bestDRNormal,
          [CanBeNull] ChunkStack bestDRLow)
        {
            if (bestDRHigh == null && bestDRNormal == null && bestDRLow == null)
                return;

            // Again, the actual reduction is calculated in three steps, one per priority.
            int remainingDamage = damage.ValueWithoutReduction;
            int reductionHigh = ApplyReductionToRemainingDamage(damage, damageEventWeapon, clusteredAttack, bestDRHigh, ref remainingDamage);
            int reductionNormal = ApplyReductionToRemainingDamage(damage, damageEventWeapon, clusteredAttack, bestDRNormal, ref remainingDamage);
            int reductionLow = ApplyReductionToRemainingDamage(damage, damageEventWeapon, clusteredAttack, bestDRLow, ref remainingDamage);

            damage.Source.SetReduction(reductionHigh + reductionNormal + reductionLow);
        }

        private int ApplyReductionToRemainingDamage(
            DamageValue damage,
            ItemEntityWeapon damageEventWeapon,
            [CanBeNull] UnitPartClusteredAttack clusteredAttack,
            ChunkStack DR,
            ref int remainingDamage)
        {
            if (DR == null || remainingDamage <= 0)
            {
                return 0;
            }

            // Note that the IsImmunity function checks for absolute immunity, without a pool, and that in the case of a pool-based immunity, DR.RemainReduction will
            // eventually redirect to return the remaining pool size.
            int reductionWithPenalties = DR.IsImmunity ? remainingDamage : DR.RemainReduction;
            int clusteredAttackPenalty = clusteredAttack != null ? clusteredAttack.Reduction : 0;
            // Bugfix: in the base game, clustered shots would *not* apply if the resistance was AddDamageResistanceEnergy, which meant it applied in the case
            // of AddDamageResistancePhyiscal (correct) and AddDamageResistanceForce (incorrect). It has been fixed to *only* apply when the resistance
            // is (TT)AddDamageResistancePhysical (i.e. DR)
            // Bugfix: in the base game, penalties to DR would apply to all resistances. This *probably* caused things like the Aeon's Enforcing Gaze DR option
            // to also lower enemies' energy resistances, which was not the intent. It has been fixed to apply under the same conditions as clustered shots, 
            // i.e. only to DR.
            if (!DR.IsImmunity && !DR.IsImmunityPool && DR.ReferenceChunk.DR.Settings is TTAddDamageResistancePhysical)
            {
                reductionWithPenalties -= clusteredAttackPenalty + this.CalculatePenalty();
            }

            int reduction = Math.Min(reductionWithPenalties, remainingDamage);
            if (reduction > 0)
            {
                DR.ApplyReduction(reduction, damage.Source, damageEventWeapon);
                // Remember that remainingDamage is a ref argument
                remainingDamage -= reduction;
            }

            return Math.Max(reduction, 0);
        }

        public bool IsImmune(DamageEnergyType energy) => !this.Owner.State.HasCondition(UnitCondition.SuppressedEnergyImmunity) && this.m_Immunities.HasItem<TTUnitPartDamageReduction.Immunity>((Func<TTUnitPartDamageReduction.Immunity, bool>)(i => i.Type == energy));

        private void Cleanup([CanBeNull] UnitPartClusteredAttack clusteredAttack)
        {
            for (int index = 0; index < this.m_Chunks.Count; ++index)
            {
                if (this.m_Chunks[index].DR.Settings is TTAddDamageResistancePhysical && clusteredAttack != null)
                    clusteredAttack.AddReduction(this.m_Chunks[index].AppliedReduction);
                this.m_Chunks[index].AppliedReduction = 0;
                if (this.m_Chunks[index].ShouldBeRemoved)
                    TTUnitPartDamageReduction.ChunksForRemove.Add(this.m_Chunks[index]);
            }
            for (int index = 0; index < TTUnitPartDamageReduction.ChunksForRemove.Count; ++index)
            {
                if (!(TTUnitPartDamageReduction.ChunksForRemove[index].Source is Buff source1))
                {
                    this.m_Chunks.Remove(TTUnitPartDamageReduction.ChunksForRemove[index]);
                    if (this.m_Chunks.Empty<TTUnitPartDamageReduction.Chunk>())
                        this.Owner.Remove<TTUnitPartDamageReduction>();
                }
                else
                    source1.Remove();
            }
            TTUnitPartDamageReduction.ChunksForRemove.Clear();
        }

        private void RecalculateChunkStacks()
        {
#if DEBUG
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
#endif
            m_ChunkStacks = m_Chunks.Select((c, i) => new ChunkStack(m_Chunks, i)).ToArray();
            BlueprintUnitFactReference[] factsPresentInChunks = m_Chunks.Select(c => c.DR.Fact.Blueprint.ToReference<BlueprintUnitFactReference>()).ToArray();

            // Increases pass
            foreach (ChunkStack chunkStack in m_ChunkStacks.Where(cs => !cs.IsImmunity && !cs.IsImmunityPool))
            {
                if (chunkStack.IsIncreasedByArmor)
                {
                    m_ChunkStacks
                        .Where(other => chunkStack.IsCompatibleWith(other) && other.SourceIsArmor)
                        .ForEach(other => chunkStack.AddAsIncrease(other));
                }

                if (chunkStack.IsIncreasedByClassFeatures)
                {
                    m_ChunkStacks
                        .Where(other => chunkStack.IsCompatibleWith(other) && other.SourceIsClassFeature)
                        .ForEach(other => chunkStack.AddAsIncrease(other));
                }

                if (chunkStack.IsIncreasedByFacts && factsPresentInChunks.Intersect(chunkStack.IncreasedByFacts).Any())
                {
                    m_ChunkStacks
                        .Where(other => chunkStack.IsCompatibleWith(other) && chunkStack.IncreasedByFacts.Contains(other.ReferenceFact))
                        .ForEach(other => chunkStack.AddAsIncrease(other));
                }

                if (chunkStack.IsIncreasesFacts && factsPresentInChunks.Intersect(chunkStack.IncreasesFacts).Any())
                {
                    m_ChunkStacks
                        .Where(other => chunkStack.IsCompatibleWith(other) && chunkStack.IncreasesFacts.Contains(other.ReferenceFact))
                        .ForEach(other => other.AddAsIncrease(chunkStack));
                }
            }

            // Stacking pass
            foreach (ChunkStack chunkStack in m_ChunkStacks.Where(cs => !cs.IsImmunity && !cs.IsImmunityPool))
            {
                if (chunkStack.IsStacksWithArmor)
                {
                    m_ChunkStacks
                        .Where(other => chunkStack.IsCompatibleWith(other) && other.SourceIsArmor)
                        .ForEach(other => chunkStack.AddToStack(other));
                }

                if (chunkStack.IsStacksWithClassFeatures)
                {
                    m_ChunkStacks
                        .Where(other => chunkStack.IsCompatibleWith(other) && other.SourceIsClassFeature)
                        .ForEach(other => chunkStack.AddToStack(other));
                }

                if (chunkStack.IsStacksWithUnitFacts && factsPresentInChunks.Intersect(chunkStack.StacksWithFacts).Any())
                {
                    m_ChunkStacks
                        .Where(other => chunkStack.IsCompatibleWith(other) && chunkStack.StacksWithFacts.Contains(other.ReferenceFact))
                        .ForEach(other => chunkStack.AddToStack(other));
                }
            }

            // Populate sources display list
            List<ChunkStack> chunkStacksForDisplay = new List<ChunkStack>();
            for (int i = 0; i < m_ChunkStacks.Length; i++)
            {
                ChunkStack chunkStack = m_ChunkStacks[i];
                if (!chunkStacksForDisplay.Contains(cd =>
                    cd.ReferenceChunk.DR.Settings.IsSameDRTypeAs(chunkStack.ReferenceChunk.DR.Settings)
                    && cd.ReferenceChunk.DR.Settings.Priority == chunkStack.ReferenceChunk.DR.Settings.Priority))
                {
                    ChunkStack maxChunkStack = m_ChunkStacks
                            .Where(cs => cs.ReferenceChunk.DR.Settings.IsSameDRTypeAs(chunkStack.ReferenceChunk.DR.Settings)
                                && cs.ReferenceChunk.DR.Settings.Priority == chunkStack.ReferenceChunk.DR.Settings.Priority)
                            .MaxBy(cs => cs.Reduction);

                    chunkStacksForDisplay.Add(maxChunkStack);
                }
            }

            m_ReductionDisplays = chunkStacksForDisplay.Select(cs => new ReductionDisplay(cs.ReferenceChunk.DR, cs.Reduction)).ToList();
#if DEBUG
            watch.Stop();
            DebugLogChunkStacks();
            DebugLogReductionDisplays();
            Main.LogDebug($"Calculated DR stacking groups in {watch.ElapsedMilliseconds} ms");
#endif
        }

        private void DebugLogChunkStacks()
        {
            StringBuilder builder = new StringBuilder("ChunkStacks: \n");
            int indent = 1;
            for (int i = 0; i < m_ChunkStacks.Length; i++)
            {
                builder.Append(m_ChunkStacks[i].ToDebugString(indent));
            }
            Main.LogDebug(builder.ToString());
        }

        private void DebugLogReductionDisplays()
        {
            StringBuilder builder = new StringBuilder("Reduction Display Values: \n");
            int indent = 1;
            foreach (ReductionDisplay rd in m_ReductionDisplays)
            {
                builder.Append(new string(' ', indent * 2) + "- " + rd.ToDebugString() + "\n");
            }
            Main.LogDebug(builder.ToString());
        }

        public void TryInitialize()
        {
            if (this.m_Initialized)
                return;
            this.m_Initialized = true;
            foreach (EntityFact sourceFact in this.m_SourceFacts)
            {
                foreach (BlueprintComponentAndRuntime<TTAddDamageResistanceBase> componentAndRuntime in sourceFact.SelectComponentsWithRuntime<TTAddDamageResistanceBase>())
                    this.m_Chunks.Add(new TTUnitPartDamageReduction.Chunk(sourceFact, (TTAddDamageResistanceBase.ComponentRuntime)componentAndRuntime.Runtime));
            }
            this.RecalculateChunkStacks();
        }

        public override void OnPreSave()
        {
            base.OnPreSave();
            this.m_Chunks.RemoveAll((Predicate<TTUnitPartDamageReduction.Chunk>)(c =>
            {
                EntityFact source = c.Source;
                return source != null && !source.Active;
            }));
        }

        public float EstimateDamage(float value, BaseDamage damage, [CanBeNull] ItemEntityWeapon weapon)
        {
            int num = 0;
            foreach (TTUnitPartDamageReduction.Chunk chunk in this.m_Chunks)
            {
                if (!chunk.Bypassed(damage, weapon) && chunk.Reduction > num)
                    num = chunk.Reduction;
            }
            return Math.Max(0.0f, value - (float)num);
        }

        public bool CanBypass(BaseDamage damage, [CanBeNull] ItemEntityWeapon weapon)
        {
            foreach (TTUnitPartDamageReduction.Chunk chunk in this.m_Chunks)
            {
                if (!chunk.Bypassed(damage, weapon))
                    return false;
            }
            return true;
        }

        public class ReductionDisplay
        {
            public ReductionDisplay(TTAddDamageResistanceBase.ComponentRuntime referenceRuntime, int totalReduction)
            {
                ReferenceRuntime = referenceRuntime;
                TotalReduction = totalReduction;
            }

            public TTAddDamageResistanceBase.ComponentRuntime ReferenceRuntime { get; }
            public int TotalReduction { get; }

            public string ToDebugString()
            {
                string result = "";
                LocalizedTexts ls = Game.Instance.BlueprintRoot.LocalizedTexts;
                if (this.ReferenceRuntime.Settings is TTAddDamageResistancePhysical settings1)
                {
                    List<string> exceptions = new List<string>();
                    int value = this.TotalReduction;
                    if (settings1.BypassedByAlignment)
                        exceptions.Add(ls.DamageAlignment.GetTextFlags(settings1.Alignment));
                    if (settings1.BypassedByForm)
                        exceptions.AddRange(settings1.Form.Components().Select<PhysicalDamageForm, string>((Func<PhysicalDamageForm, string>)(f => ls.DamageForm.GetText(f))));
                    if (settings1.BypassedByMagic)
                        exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MagicDRDescriptor);
                    if (settings1.BypassedByMaterial)
                        exceptions.Add(ls.DamageMaterial.GetTextFlags(settings1.Material));
                    if (settings1.BypassedByReality)
                        exceptions.Add(ls.DamageReality.GetText(settings1.Reality));
                    if (settings1.BypassedByMeleeWeapon)
                        exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MeleeDRDescriptor);
                    if (settings1.BypassedByWeaponType)
                        exceptions.Add((string)settings1.WeaponType.TypeName);
                    if (exceptions.Count == 0)
                        exceptions.Add("-");

                    if (settings1.Or)
                    {
                        result = "DR " + value.ToString() + "/" + string.Join(" or ", exceptions);
                    }
                    else
                    {
                        result = "DR " + value.ToString() + "/" + string.Join(" and ", exceptions);
                    }
                }
                else if (this.ReferenceRuntime.Settings is TTProtectionFromEnergy settings2)
                {
                    result = "protection from " + ls.DamageEnergy.GetText(settings2.Type) + " (" + this.ReferenceRuntime.RemainPool + ")";
                }
                else if (this.ReferenceRuntime.Settings is TTWizardAbjurationResistance settings3)
                {
                    result = "resist " + ls.DamageEnergy.GetText(settings3.Type) + " " + this.TotalReduction + " (abjuration)";
                }
                else if (this.ReferenceRuntime.Settings is TTWizardEnergyAbsorption settings4)
                {
                    result = "(abjuration) " + ls.DamageEnergy.GetText(settings4.Type) + " absorption (" + this.ReferenceRuntime.RemainPool + ")";
                }
                else if (this.ReferenceRuntime.Settings is TTAddDamageResistanceEnergy settings5)
                {
                    result = "resist " + ls.DamageEnergy.GetText(settings5.Type) + " " + this.TotalReduction;
                }
                else if (this.ReferenceRuntime.Settings is TTAddDamageResistanceForce settings6)
                {
                    result = "resist force " + this.TotalReduction;
                }

                result += " <" + this.ReferenceRuntime.Fact.Blueprint.name + ":" + this.ReferenceRuntime.Fact.Blueprint.AssetGuid.ToString() + "> ";
                return result;
            }
        }

        private class Chunk
        {
            [CanBeNull]
            public EntityFact Source { get; }

            public TTAddDamageResistanceBase.ComponentRuntime DR { get; }

            public int AppliedReduction { get; set; }

            public int Reduction => this.DR.GetCurrentValue();

            public int RemainReduction => this.Reduction - this.AppliedReduction;

            public bool ShouldBeRemoved => this.DR.ShouldBeRemoved;

            public bool IsStackable => this.DR.Settings.IsStackable;

            public bool IsImmunity => this.DR.Settings.IsImmunity;

            public bool IsImmunityPool => this.DR.Settings.IsImmunityPool;

            public TTAddDamageResistanceBase.DRPriority Priority => this.DR.Settings.Priority;

            public Chunk(EntityFact src, TTAddDamageResistanceBase.ComponentRuntime dr)
            {
                this.Source = src;
                this.DR = dr;
            }

            public bool Bypassed(BaseDamage damage, ItemEntityWeapon weapon) => this.DR.Bypassed(damage, weapon);

            public string ToDebugString()
            {
                string result = "";
                LocalizedTexts ls = Game.Instance.BlueprintRoot.LocalizedTexts;
                if (this.DR.Settings is TTAddDamageResistancePhysical settings1)
                {
                    List<string> exceptions = new List<string>();
                    int value = this.Reduction;
                    if (settings1.BypassedByAlignment)
                        exceptions.Add(ls.DamageAlignment.GetTextFlags(settings1.Alignment));
                    if (settings1.BypassedByForm)
                        exceptions.AddRange(settings1.Form.Components().Select<PhysicalDamageForm, string>((Func<PhysicalDamageForm, string>)(f => ls.DamageForm.GetText(f))));
                    if (settings1.BypassedByMagic)
                        exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MagicDRDescriptor);
                    if (settings1.BypassedByMaterial)
                        exceptions.Add(ls.DamageMaterial.GetTextFlags(settings1.Material));
                    if (settings1.BypassedByReality)
                        exceptions.Add(ls.DamageReality.GetText(settings1.Reality));
                    if (settings1.BypassedByMeleeWeapon)
                        exceptions.Add((string)Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.MeleeDRDescriptor);
                    if (settings1.BypassedByWeaponType)
                        exceptions.Add((string)settings1.WeaponType.TypeName);
                    if (exceptions.Count == 0)
                        exceptions.Add("-");
                    
                    if (settings1.Or)
                    {
                        result = "DR " + value.ToString() + "/" + string.Join(" or ", exceptions);
                    }
                    else
                    {
                        result = "DR " + value.ToString() + "/" + string.Join(" and ", exceptions);
                    }
                }
                else if (this.DR.Settings is TTProtectionFromEnergy settings2)
                {
                    result = "protection from " + ls.DamageEnergy.GetText(settings2.Type) + " (" + this.DR.RemainPool + ")";
                }
                else if (this.DR.Settings is TTWizardAbjurationResistance settings3)
                {
                    result = "resist " + ls.DamageEnergy.GetText(settings3.Type) + " " + this.Reduction + " (abjuration)";
                }
                else if (this.DR.Settings is TTWizardEnergyAbsorption settings4)
                {
                    result = "(abjuration) " + ls.DamageEnergy.GetText(settings4.Type) + " absorption (" + this.DR.RemainPool + ")"; 
                }
                else if (this.DR.Settings is TTAddDamageResistanceEnergy settings5)
                {
                    result = "resist " + ls.DamageEnergy.GetText(settings5.Type) + " " + this.Reduction;
                }
                else if (this.DR.Settings is TTAddDamageResistanceForce settings6)
                {
                    result = "resist force " + this.Reduction;
                }

                result += " <" + this.Source.Blueprint.name + ":" + this.Source.Blueprint.AssetGuid.ToString() + "> ";
                return result;
            }
        }

        /// <summary>
        /// A ChunkStack consists of a BaseChunk and a list of StackingChunks, which are chunks that can stack with the BaseChunk
        /// </summary>
        private class ChunkStack
        {

            public ChunkStack(List<Chunk> chunks, int indexOfReferenceChunk)
            {
                m_chunkListReference = chunks;
                m_baseChunkIndices = new BitArray(chunks.Count);
                m_baseChunkIndices[indexOfReferenceChunk] = true;
                m_stackingChunkIndices = new BitArray(chunks.Count);
                m_referenceChunkIndex = indexOfReferenceChunk;
                ReferenceChunk = chunks[m_referenceChunkIndex];
                ReferenceFact = ReferenceChunk.DR.Fact.Blueprint.ToReference<BlueprintUnitFactReference>();
            }

            private readonly List<Chunk> m_chunkListReference;
            private BitArray m_baseChunkIndices;
            private BitArray m_stackingChunkIndices;

            private readonly int m_referenceChunkIndex;
            public Chunk ReferenceChunk { get; }
            public BlueprintUnitFactReference ReferenceFact { get; }
            public bool IsCompatibleWith(ChunkStack other)
            {
                return !this.IsImmunity && !other.IsImmunity
                    && !this.IsImmunityPool && !other.IsImmunityPool
                    && this.ReferenceChunk != other.ReferenceChunk
                    && this.ReferenceChunk.DR.Settings.IsSameDRTypeAs(other.ReferenceChunk.DR.Settings)
                    && this.ReferenceChunk.DR.Settings.Priority == other.ReferenceChunk.DR.Settings.Priority;
            }

            public bool IsCompatibleWith(Chunk chunk)
            {
                return !this.IsImmunity && !chunk.IsImmunity
                    && !this.IsImmunityPool && !chunk.IsImmunityPool
                    && this.ReferenceChunk != chunk
                    && this.ReferenceChunk.DR.Settings.IsSameDRTypeAs(chunk.DR.Settings)
                    && this.ReferenceChunk.DR.Settings.Priority == chunk.DR.Settings.Priority;
            }

            public bool SourceIsArmor => ReferenceChunk.DR.Settings.SourceIsArmor;

            public bool SourceIsClassFeature => ReferenceChunk.DR.Settings.SourceIsClassFeature;

            public void AddAsIncrease(ChunkStack other)
            {
                m_baseChunkIndices.Or(other.m_baseChunkIndices);
                other.m_baseChunkIndices.Or(m_baseChunkIndices);
            }

            public void AddToStack(ChunkStack other)
            {
                // Don't add a chunk as stacking if it is already included in the "base" increases package.
                if (m_baseChunkIndices[other.m_referenceChunkIndex]) { return; }
                m_stackingChunkIndices[other.m_referenceChunkIndex] = true;
            }

            public bool IsStacksWithArmor
            {
                get
                {
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i] && m_chunkListReference[i].DR.Settings.IsStacksWithArmor)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            public bool IsStacksWithClassFeatures
            {
                get
                {
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i] && m_chunkListReference[i].DR.Settings.IsStacksWithClassFeatures)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            public bool IsStacksWithUnitFacts
            {
                get
                {
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i]
                            && m_chunkListReference[i].DR.Settings.StacksWithFacts != null
                            && m_chunkListReference[i].DR.Settings.StacksWithFacts.Length > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public IEnumerable<BlueprintUnitFactReference> StacksWithFacts =>
                m_chunkListReference.Where((c, i) => m_baseChunkIndices[i]).SelectMany(c => c.DR.Settings.StacksWithFacts.EmptyIfNull());


            public bool IsIncreasedByArmor
            {
                get
                {
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i] && m_chunkListReference[i].DR.Settings.IsIncreasedByArmor)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public bool IsIncreasedByClassFeatures
            {
                get
                {
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i] && m_chunkListReference[i].DR.Settings.IsIncreasedByClassFeatures)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public bool IsIncreasesFacts
            {
                get
                {
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i] && m_chunkListReference[i].DR.Settings.IsIncreasesFacts)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public IEnumerable<BlueprintUnitFactReference> IncreasesFacts =>
                m_chunkListReference.Where((c, i) => m_baseChunkIndices[i]).SelectMany(c => c.DR.Settings.IncreasesFacts.EmptyIfNull());


            public bool IsIncreasedByFacts
            {
                get
                {
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i] && m_chunkListReference[i].DR.Settings.IsIncreasedByFacts)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            public IEnumerable<BlueprintUnitFactReference> IncreasedByFacts =>
                m_chunkListReference.Where((c, i) => m_baseChunkIndices[i]).SelectMany(c => c.DR.Settings.IncreasedByFacts.EmptyIfNull());



            public HashSet<Chunk> StackingChunks { get; set; } = new HashSet<Chunk>();

            // The applied reduction of the stack is the sum of all the chunks' applied reduction in this stack.
            public int AppliedReduction
            {
                get
                {
                    int sumReduction = 0;
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i] || m_stackingChunkIndices[i])
                        {
                            sumReduction += m_chunkListReference[i].AppliedReduction;
                        }
                    }
                    return sumReduction;
                }
            }

            // This applies a reduction to all the chunks in this stack, and spends points from the pool if necessary. 
            public void ApplyReduction(int reduction, BaseDamage damageSource, [CanBeNull] ItemEntityWeapon damageEventWeapon)
            {
                int remaining = reduction;

                if (!ReferenceChunk.Bypassed(damageSource, damageEventWeapon))
                {
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i] || m_stackingChunkIndices[i])
                        {
                            Chunk chunk = m_chunkListReference[i];
                            int reduceBy = Math.Min(remaining, chunk.RemainReduction);
                            chunk.DR.SpendPool(reduceBy);
                            chunk.AppliedReduction += reduceBy;
                            remaining -= reduceBy;
                            if (remaining <= 0) break;
                        }
                    }
                }
            }

            private int m_cachedReduction = -1;
            public int Reduction
            {
                get
                {
                    if (m_cachedReduction >= 0)
                    {
                        return m_cachedReduction;
                    }

                    int reduction = 0;
                    for (int i = 0; i < m_baseChunkIndices.Length; i++)
                    {
                        if (m_baseChunkIndices[i] || m_stackingChunkIndices[i])
                        {
                            reduction += m_chunkListReference[i].Reduction;
                        }
                    }
                    m_cachedReduction = reduction;
                    return reduction;
                }
            }

            public bool IsImmunity => ReferenceChunk.IsImmunity;

            public bool IsImmunityPool => ReferenceChunk.IsImmunityPool;

            public TTAddDamageResistanceBase.DRPriority Priority => ReferenceChunk.Priority;

            public int RemainReduction => Reduction - AppliedReduction;

            public string ToDebugString(int indent)
            {
                StringBuilder builder = new StringBuilder();
                int ind = indent;
                builder.Append(new string(' ', ind * 2) + "ChunkStack: \n");
                ind++;
                builder.Append(new string(' ', ind * 2) + "- Reference Chunk: ");
                builder.Append(this.ReferenceChunk.ToDebugString());
                builder.Append("\n");
                builder.Append(new string(' ', ind * 2) + "- Base Chunks: \n");
                builder.Append(new string(' ', ind * 2) + "[\n");
                ind++;
                for (int i = 0; i < m_baseChunkIndices.Length; i++)
                {
                    if (m_baseChunkIndices[i])
                    {
                        builder.Append(new string(' ', ind * 2) + "- " + m_chunkListReference[i].ToDebugString() + "\n");
                    }
                }
                ind--;
                builder.Append(new string(' ', ind * 2) + "]\n");
                builder.Append(new string(' ', ind * 2) + "- Stack Chunks: \n");
                builder.Append(new string(' ', ind * 2) + "[\n");
                ind++;
                for (int i = 0; i < m_stackingChunkIndices.Length; i++)
                {
                    if (m_stackingChunkIndices[i])
                    {
                        builder.Append(new string(' ', ind * 2) + "- " + m_chunkListReference[i].ToDebugString() + "\n");
                    }
                }
                ind--;
                builder.Append(new string(' ', ind * 2) + "]\n");
                return builder.ToString();
            }
        
        }

        public class ReductionPenalty
        {
            public int Penalty;
            public EntityFact Source;
        }

        public class Immunity
        {
            public readonly EntityFact SourceFact;
            public readonly BlueprintComponent SourceComponent;
            public readonly DamageEnergyType Type;

            public Immunity(
              EntityFact sourceFact,
              BlueprintComponent sourceComponent,
              DamageEnergyType type)
            {
                this.SourceFact = sourceFact;
                this.SourceComponent = sourceComponent;
                this.Type = type;
            }
        }
    }
}
