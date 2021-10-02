using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartFocusedWeapon : UnitPart {
        public void AddEntry(WeaponCategory? category, EntityFact source) {
            if (category == null) {
                return;
            }
            FocusedWeaponEntry item = new FocusedWeaponEntry {
                Category = category.Value,
                Source = source
            };
            this.Weapons.Add(item);
        }

        public void RemoveEntry(EntityFact source) {
            this.Weapons.RemoveAll((FocusedWeaponEntry p) => p.Source == source);
            TryRemove();
        }

        private void TryRemove() {
            if (!Weapons.Any()) { this.RemoveSelf(); }
        }

        public bool HasEntry(WeaponCategory category) {
            return this.Weapons.Any((FocusedWeaponEntry p) => p.Category == category);
        }

        public List<FocusedWeaponEntry> Weapons = new List<FocusedWeaponEntry>();

        public class FocusedWeaponEntry {
            public WeaponCategory Category;
            public EntityFact Source;
        }
    }
}
