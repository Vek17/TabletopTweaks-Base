using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.NewUnitParts {
    public class UnitPartBuffSupressTTT : OldStyleUnitPart {

        public override void OnTurnOn() {
            SuppressionEntries.Remove(entry => entry.Source.Fact == null);
            ContinuousSuppressionEntries.Remove(entry => entry.Source.Fact == null);

            SuppressionEntries.ForEach(entry => entry.ActivateSuppression());
            ContinuousSuppressionEntries.ForEach(entry => entry.ActivateSuppression(base.Owner));
        }
        public void AddNormalEntry(EntityFact source,
            BlueprintBuffReference[] buffs,
            SpellSchool[] spellSchools,
            SpellDescriptor spellDescriptor) {
            if (SuppressionEntries.Any(entry => entry.Source.FactId == source.UniqueId && entry.Type == SuppresionType.Normal)) { return; }

            var suppressionEntry = new SuppressionEffectEntry(source, SuppresionType.Normal);
            foreach (Buff buff in base.Owner.Buffs) {
                bool shouldSuppress = buff.Context.SpellDescriptor.HasAnyFlag(spellDescriptor)
                    || spellSchools.Contains(buff.Context.SpellSchool)
                    || buffs.Any(reference => buff.Blueprint.AssetGuid == reference.Guid);

                if (shouldSuppress && !buff.IsSuppressed) {
                    suppressionEntry.Buffs.Add(buff);
                }
            }
            suppressionEntry.ActivateSuppression();
            SuppressionEntries.Add(suppressionEntry);
        }

        public void AddSizeEntry(EntityFact source) {
            if (SuppressionEntries.Any(entry => entry.Source.FactId == source.UniqueId && entry.Type == SuppresionType.Size)) { return; }
            var suppressionEntry = new SuppressionEffectEntry(source, SuppresionType.Size);
            foreach (Buff buff in base.Owner.Buffs) {
                bool shouldSuppress = buff != source
                    && !buff.Context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph)
                    && buff.GetComponent<ChangeUnitSize>();

                if (shouldSuppress) {
                    suppressionEntry.Buffs.Add(buff);
                }
            }
            suppressionEntry.ActivateSuppression();
            SuppressionEntries.Add(suppressionEntry);
        }

        public void AddContinuousEntry(EntityFact source,
            BlueprintBuffReference[] buffs,
            SpellSchool[] spellSchools,
            SpellDescriptor spellDescriptor) {
            if (ContinuousSuppressionEntries.Any(entry => entry.Source.FactId == source.UniqueId)) { return; }

            var suppressionEntry = new ContinuousSuppressionEffectEntry(source, buffs, spellSchools, spellDescriptor);
            suppressionEntry.ActivateSuppression(base.Owner);
            ContinuousSuppressionEntries.Add(suppressionEntry);
        }

        public void AddContinuousPolymorphEntry(EntityFact source) {
            if (ContinuousSuppressionEntries.Any(entry => entry.Source.FactId == source.UniqueId)) { return; }

            var suppressionEntry = new ContinuousSuppressionPolymorphEntry(source);
            suppressionEntry.ActivateSuppression(base.Owner);
            ContinuousSuppressionEntries.Add(suppressionEntry);
        }

        public bool IsSuppressedContinuously(Buff buff) {
            return ContinuousSuppressionEntries.Any(entry => entry.ShouldSuppress(buff));
        }

        public void RemoveEntry(EntityFact source) {
            SuppressionEntries
                .Where(entry => entry.Source.FactId == source.UniqueId)
                .ForEach(entry => entry.DeactivateSuppression());
            SuppressionEntries.Remove(entry => entry.Source.FactId == source.UniqueId);

            ContinuousSuppressionEntries
                .Where(entry => entry.Source.FactId == source.UniqueId)
                .ForEach(entry => entry.DeactivateSuppression(base.Owner));
            ContinuousSuppressionEntries.Remove(entry => entry.Source.FactId == source.UniqueId);

            SuppressionEntries.ForEach(entry => entry.ActivateSuppression());
            ContinuousSuppressionEntries.ForEach(entry => entry.ActivateSuppression(base.Owner));

            TryRemovePart();
        }

        private void TryRemovePart() {
            if (SuppressionEntries.Empty() && ContinuousSuppressionEntries.Empty()) {
                base.RemoveSelf();
            }
        }

        [JsonProperty]
        private readonly List<SuppressionEffectEntry> SuppressionEntries = new();
        [JsonProperty]
        private readonly List<ContinuousSuppressionEffectBase> ContinuousSuppressionEntries = new();
        public class SuppressionEffectEntry {
            public SuppressionEffectEntry() {
            }
            public SuppressionEffectEntry(EntityFact source, SuppresionType type, params Buff[] buffs) {
                this.Source = source;
                this.Type = type;
                Buffs = buffs.Select(buff => new EntityFactRef<Buff>(buff)).ToList();
            }
            [JsonProperty]
            public readonly List<EntityFactRef<Buff>> Buffs = new();
            [JsonProperty]
            public readonly EntityFactRef<EntityFact> Source;
            [JsonProperty]
            public readonly SuppresionType Type;

            public void ActivateSuppression() {
                Buffs.ForEach(buffRef => {
                    var buff = buffRef.Fact;
                    if (buff != null) {
                        buff.IsSuppressed = true;
                        if (buff.IsActive) {
                            buff.Deactivate();
                        }
                    }
                });
            }

            public void DeactivateSuppression() {
                Buffs.ForEach(buffRef => {
                    var buff = buffRef.Fact;
                    if (buff != null) {
                        buff.IsSuppressed = false;
                        if (!buff.IsActive) {
                            buff.Activate();
                        }
                    }
                });
            }
        }
        public enum SuppresionType {
            Normal,
            Size
        }
        public abstract class ContinuousSuppressionEffectBase {
            public ContinuousSuppressionEffectBase() {
            }
            public ContinuousSuppressionEffectBase(EntityFact source) {
                Source = source;
            }
            [JsonProperty]
            public readonly EntityFactRef<EntityFact> Source;
            public virtual void ActivateSuppression(UnitDescriptor owner) {
                foreach (Buff buff in owner.Buffs) {
                    bool shouldSuppress = ShouldSuppress(buff);

                    if (shouldSuppress && !buff.IsSuppressed) {
                        buff.IsSuppressed = true;
                        if (buff.IsActive) {
                            buff.Deactivate();
                        }
                    }
                }
            }
            public virtual void DeactivateSuppression(UnitDescriptor owner) {
                foreach (Buff buff in owner.Buffs) {
                    bool shouldSuppress = ShouldSuppress(buff);

                    if (shouldSuppress && buff.IsSuppressed) {
                        buff.IsSuppressed = false;
                        if (!buff.IsActive) {
                            buff.Activate();
                        }
                    }
                }
            }
            public abstract bool ShouldSuppress(Buff buff);
        }
        public class ContinuousSuppressionEffectEntry : ContinuousSuppressionEffectBase {
            public ContinuousSuppressionEffectEntry() { }
            public ContinuousSuppressionEffectEntry(EntityFact source,
                BlueprintBuffReference[] buffs,
                SpellSchool[] spellSchools,
                SpellDescriptor spellDescriptor) : base(source) {
                this.Buffs = buffs ?? new BlueprintBuffReference[0];
                this.Schools = spellSchools ?? new SpellSchool[0];
                this.Descriptor = spellDescriptor;
            }
            [JsonProperty]
            public readonly BlueprintBuffReference[] Buffs;
            [JsonProperty]
            public readonly SpellSchool[] Schools;
            [JsonProperty]
            public readonly SpellDescriptor Descriptor;

            public override bool ShouldSuppress(Buff buff) {
                return buff.Context.SpellDescriptor.HasAnyFlag(Descriptor)
                    || Schools.Contains(buff.Context.SpellSchool)
                    || Buffs.Any(reference => buff.Blueprint.AssetGuid == reference.Guid);
            }
        }
        public class ContinuousSuppressionPolymorphEntry : ContinuousSuppressionEffectBase {
            public ContinuousSuppressionPolymorphEntry() { }
            public ContinuousSuppressionPolymorphEntry(EntityFact source) : base(source) {
            }

            public override bool ShouldSuppress(Buff buff) {
                return !buff.Context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph)
                    && buff.GetComponent<ChangeUnitSize>();
            }
        }
        //Suppress new buffs on attach if they are flagged
        [HarmonyPatch(typeof(Buff), nameof(Buff.OnAttach))]
        private static class Buff_OnAttach_Suppression_Patch {
            static void Postfix(Buff __instance) {
                var unitPartBuffSuppress = __instance.Owner.Get<UnitPartBuffSupressTTT>();
                if (unitPartBuffSuppress != null && !__instance.IsSuppressed) {
                    __instance.IsSuppressed = unitPartBuffSuppress.IsSuppressedContinuously(__instance);
                }
            }
        }
    }
}
