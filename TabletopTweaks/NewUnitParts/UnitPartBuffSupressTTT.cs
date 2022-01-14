using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartBuffSupressTTT : OldStyleUnitPart {

        public override void OnTurnOn() {
            SuppressionEntries.ForEach(entry => entry.ActivateSuppression());
        }

        public void AddEntry(EntityFact source, SpellSchool[] spellSchools) {
            if (SuppressionEntries.Any(entry => entry.Source.FactId == source.UniqueId && entry.Type == SuppresionType.School)) { return; }

            var suppressionEntry = new SuppressionEffectEntry(source, SuppresionType.School);
            foreach (Buff buff in base.Owner.Buffs) {
                bool shouldSuppress = spellSchools.Contains(buff.Context.SpellSchool);

                if (shouldSuppress && !buff.IsSuppressed) {
                    suppressionEntry.Buffs.Add(buff);
                }
            }
            suppressionEntry.ActivateSuppression();
            SuppressionEntries.Add(suppressionEntry);
        }

        public void AddEntry(EntityFact source, SpellDescriptor spellDescriptor) {
            if (SuppressionEntries.Any(entry => entry.Source.FactId == source.UniqueId && entry.Type == SuppresionType.Descriptor)) { return; }

            var suppressionEntry = new SuppressionEffectEntry(source, SuppresionType.Descriptor);
            foreach (Buff buff in base.Owner.Buffs) {
                bool shouldSuppress = buff.Context.SpellDescriptor.HasAnyFlag(spellDescriptor);

                if (shouldSuppress && !buff.IsSuppressed) {
                    suppressionEntry.Buffs.Add(buff);
                }
            }
            suppressionEntry.ActivateSuppression();
            SuppressionEntries.Add(suppressionEntry);
        }

        public void AddEntry(EntityFact source, BlueprintBuffReference[] buffs) {
            if (SuppressionEntries.Any(entry => entry.Source.FactId == source.UniqueId && entry.Type == SuppresionType.Specific)) { return; }

            var suppressionEntry = new SuppressionEffectEntry(source, SuppresionType.Specific);
            foreach (Buff buff in base.Owner.Buffs) {
                bool shouldSuppress = buffs.Any(reference => buff.Blueprint.AssetGuid == reference.Guid);

                if (shouldSuppress && !buff.IsSuppressed) {
                    suppressionEntry.Buffs.Add(buff);
                }
            }
            suppressionEntry.ActivateSuppression();
            SuppressionEntries.Add(suppressionEntry);
        }

        public void RemoveEntry(EntityFact source) {
            SuppressionEntries
                .Where(entry => entry.Source.FactId == source.UniqueId)
                .ForEach(entry => entry.DeactivateSuppression());
            SuppressionEntries.Remove(entry => entry.Source.FactId == source.UniqueId);
            TryRemovePart();
        }

        private void TryRemovePart() {
            if (SuppressionEntries.Empty()) {
                base.RemoveSelf();
            }
        }

        [JsonProperty]
        private readonly List<SuppressionEffectEntry> SuppressionEntries = new();
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
            Descriptor,
            School,
            Specific
        }
    }
}
