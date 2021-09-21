using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
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
        private readonly List<TTUnitPartDamageReduction.Immunity> m_Immunities = new List<TTUnitPartDamageReduction.Immunity>();

        public void AddPenaltyEntry(int penalty, EntityFact source) => this.PenaltyEntries.Add(new TTUnitPartDamageReduction.ReductionPenalty()
        {
            Penalty = penalty,
            Source = source
        });

        public void RemovePenaltyEntry(EntityFact source) => this.PenaltyEntries.RemoveAll((Predicate<TTUnitPartDamageReduction.ReductionPenalty>)(p => p.Source == source));

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

        public IEnumerable<TTAddDamageResistanceBase.ComponentRuntime> AllSources
        {
            get
            {
                this.TryInitialize();
                return this.m_Chunks.Select<TTUnitPartDamageReduction.Chunk, TTAddDamageResistanceBase.ComponentRuntime>((Func<TTUnitPartDamageReduction.Chunk, TTAddDamageResistanceBase.ComponentRuntime>)(c => c.DR));
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
        }

        public void Remove(EntityFact fact)
        {
            this.m_SourceFacts.Remove(fact);
            this.m_Chunks.RemoveAll((Predicate<TTUnitPartDamageReduction.Chunk>)(c => c.Source == fact));
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
            ChunkStack[] chunkStacks = CreateDRStackingGroups();
            foreach (DamageValue damage in evt.CalculatedDamage)
            {
                if (damage.FinalValue >= 1)
                {
                    ChunkStack bestDRHigh = FindBestDRWithPriority(chunkStacks, damage, evt.DamageBundle.Weapon, TTAddDamageResistanceBase.DRPriority.High);
                    ChunkStack bestDRNormal = FindBestDRWithPriority(chunkStacks, damage, evt.DamageBundle.Weapon, TTAddDamageResistanceBase.DRPriority.Normal);
                    ChunkStack bestDRLow = FindBestDRWithPriority(chunkStacks, damage, evt.DamageBundle.Weapon, TTAddDamageResistanceBase.DRPriority.Low);
                    this.ApplyReduction(damage, evt.DamageBundle.Weapon, clusteredAttack, bestDRHigh, bestDRNormal, bestDRLow);
                }
            }
            this.Cleanup(clusteredAttack);
        }

        private ChunkStack FindBestDRWithPriority(
            ChunkStack[] chunkStacks, 
            DamageValue damage,
            ItemEntityWeapon damageEventWeapon,
            TTAddDamageResistanceBase.DRPriority priority)
        {
            ChunkStack bestDR = null;
            foreach (ChunkStack chunkStack in chunkStacks.Where(cs => cs.Priority == priority))
            {
                chunkStack.CalculateReduction(damage, damageEventWeapon);
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
            if (DR == null)
            {
                return 0;
            }

            int reductionWithPenalties = DR.IsImmunity ? remainingDamage : DR.RemainReduction;
            int clusteredAttackPenalty = clusteredAttack != null ? clusteredAttack.Reduction : 0;
            if (!DR.IsImmunity && !DR.IsImmunityPool && DR.BaseChunk.DR.Settings is TTAddDamageResistancePhysical)
            {
                reductionWithPenalties -= clusteredAttackPenalty + this.CalculatePenalty();
            }

            int reduction = Math.Min(reductionWithPenalties, remainingDamage);
            if (reduction > 0)
            {
                DR.SpendPool(reduction, damage.Source, damageEventWeapon);
                DR.ApplyReduction(reduction, damage.Source, damageEventWeapon);
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

        private ChunkStack[] CreateDRStackingGroups()
        {
            ChunkStack[] result = m_Chunks.Select(c => new ChunkStack(c)).ToArray();
            BlueprintUnitFactReference[] unitFactsThatCanStack = result.Where(cs => cs.IsStacksWithUnitFacts).SelectMany(cs => cs.StacksWithFacts).ToArray();
            foreach (Chunk chunk in m_Chunks.Where(c => !c.IsImmunity && !c.IsImmunityPool))
            {
                if (chunk.DR.Settings.SourceIsArmor)
                {
                    result
                        .Where(cs => cs.IsStacksWithArmor && cs.BaseChunk != chunk && chunk.DR.Settings.IsSameDRTypeAs(cs.BaseChunk.DR.Settings))
                        .ForEach(cs => cs.StackingChunks.Add(chunk));
                }

                if (chunk.DR.Settings.SourceIsClassFeature)
                {
                    result
                        .Where(cs => cs.IsStacksWithClassFeatures && cs.BaseChunk != chunk && chunk.DR.Settings.IsSameDRTypeAs(cs.BaseChunk.DR.Settings))
                        .ForEach(cs => cs.StackingChunks.Add(chunk));
                }

                BlueprintUnitFactReference sourceFactReference = chunk.DR.Fact.Blueprint.ToReference<BlueprintUnitFactReference>();
                if (unitFactsThatCanStack.Contains(sourceFactReference))
                {
                    result
                        .Where(cs => cs.IsStacksWithUnitFacts && cs.BaseChunk != chunk && chunk.DR.Settings.StacksWithFacts.Contains(sourceFactReference) && chunk.DR.Settings.IsSameDRTypeAs(cs.BaseChunk.DR.Settings))
                        .ForEach(cs => cs.StackingChunks.Add(chunk));
                }
            }
            return result;
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
        }

        private class ChunkStack
        {
            public ChunkStack(Chunk baseChunk)
            {
                BaseChunk = baseChunk;
                IsStacksWithArmor = baseChunk.DR.Settings.StacksWithArmor;
                IsStacksWithClassFeatures = baseChunk.DR.Settings.StacksWithClassFeatures;
                IsStacksWithUnitFacts = baseChunk.DR.Settings.StacksWithFacts != null && baseChunk.DR.Settings.StacksWithFacts.Length > 0;
            }

            public Chunk BaseChunk { get; set; }

            public bool IsStacksWithArmor = false;
            public bool IsStacksWithClassFeatures = false;
            public bool IsStacksWithUnitFacts = false;

            public BlueprintUnitFactReference[] StacksWithFacts => BaseChunk.DR.Settings.StacksWithFacts;

            public HashSet<Chunk> StackingChunks { get; set; } = new HashSet<Chunk>();

            public int AppliedReduction => BaseChunk.AppliedReduction + StackingChunks.Sum(c => c.AppliedReduction);
            public void ApplyReduction(int reduction, BaseDamage damageSource, [CanBeNull] ItemEntityWeapon damageEventWeapon)
            {
                int remaining = reduction;
                if (BaseChunk.RemainReduction > 0 && !BaseChunk.Bypassed(damageSource, damageEventWeapon))
                {
                    int reduceBy = Math.Min(remaining, BaseChunk.RemainReduction);
                    BaseChunk.AppliedReduction += reduceBy;
                    remaining -= reduceBy;
                }
                if (remaining > 0)
                {
                    foreach (Chunk chunk in StackingChunks.Where(c => !c.Bypassed(damageSource, damageEventWeapon)))
                    {
                        int reduceBy = Math.Min(remaining, chunk.RemainReduction);
                        chunk.AppliedReduction += reduceBy;
                        remaining -= reduceBy;
                        if (remaining <= 0) break;
                    }
                }
            }

            public int Reduction { get; set; } = 0;

            public bool IsImmunity { get; set; } = false;

            public bool IsImmunityPool { get; set; } = false;

            public TTAddDamageResistanceBase.DRPriority Priority => BaseChunk.Priority;

            public int RemainReduction => Reduction - AppliedReduction;

            public int CalculateReduction(
                DamageValue damage, 
                [CanBeNull] ItemEntityWeapon damageEventWeapon)
            {
                if (BaseChunk.Bypassed(damage.Source, damageEventWeapon))
                {
                    Reduction = 0;
                    return 0;
                }

                IsImmunity = BaseChunk.IsImmunity;
                IsImmunityPool = BaseChunk.IsImmunityPool;

                int reduction = BaseChunk.Reduction;
                foreach (Chunk chunk in StackingChunks)
                {
                    if (!chunk.Bypassed(damage.Source, damageEventWeapon))
                    {
                        reduction += chunk.Reduction;
                    }
                }

                Reduction = reduction;
                return reduction;
            }

            public void SpendPool(
                int amount,
                BaseDamage damageSource,
                [CanBeNull] ItemEntityWeapon damageEventWeapon)
            {
                int remaining = amount;
                if (BaseChunk.DR.Settings.UsePool && !BaseChunk.Bypassed(damageSource, damageEventWeapon))
                {
                    int spendAmount = Math.Min(BaseChunk.DR.RemainPool, remaining);
                    BaseChunk.DR.SpendPool(spendAmount);
                    remaining -= spendAmount;
                }

                if (remaining > 0)
                {
                    foreach(Chunk chunk in StackingChunks.Where(c => !c.Bypassed(damageSource, damageEventWeapon)))
                    {
                        int spendAmount = Math.Min(chunk.DR.RemainPool, remaining);
                        chunk.DR.SpendPool(spendAmount);
                        remaining -= spendAmount;
                        if (remaining <= 0) break;
                    }
                }
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
