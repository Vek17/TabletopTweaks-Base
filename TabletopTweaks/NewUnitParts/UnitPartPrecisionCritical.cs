using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartPrecisionCritical : UnitPart {

        public int GetMultiplier() {
            int multiplier = 0;
            multiplier += BaseMultipliers
                .Select(m => m.CriticalMultiplier)
                .Max();
            multiplier += AdditionalMultipliers
                .Select(m => m.CriticalMultiplier)
                .Sum();
            return Math.Max(multiplier, 1);
        }

        public void AddEntry(int increasedCritMultipler, bool additional, EntityFact source) {
            PrecisionCriticalEntry item = new PrecisionCriticalEntry {
                CriticalMultiplier = increasedCritMultipler,
                Additional = additional,
                Source = source
            };
            if (item.Additional) {
                AdditionalMultipliers.Add(item);
            } else {
                BaseMultipliers.Add(item);
            }

        }
        public void RemoveEntry(EntityFact source) {
            BaseMultipliers.RemoveAll((PrecisionCriticalEntry c) => c.Source == source);
            TryRemove();
        }
        private void TryRemove() {
            if (!BaseMultipliers.Any() && AdditionalMultipliers.Any()) { this.RemoveSelf(); }
        }

        private readonly List<PrecisionCriticalEntry> BaseMultipliers = new();
        private readonly List<PrecisionCriticalEntry> AdditionalMultipliers = new();
        public class PrecisionCriticalEntry {
            public int CriticalMultiplier;
            public bool Additional;
            public EntityFact Source;
        }
    }
}
