using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using System;

namespace TabletopTweaks.NewUnitParts {
    // Marked Obsolete on 2021-11-11
    [Obsolete("use UnitPartCustomStats instead", true)]
    class MeleeTouchReach : UnitPart {
        public int Reach = 0;
        public ModifiableValue TouchValue {
            get {
                if (m_touchRange == null) {
                    m_touchRange = new ModifiableValue(Owner.Stats, (StatType)500);
                }
                return m_touchRange;
            }
        }
        public int GetModifiedValue() {
            return TouchValue.ModifiedValueRaw;
        }
        public void AddModifier(int value, EntityFactComponent source, ModifierDescriptor desc = ModifierDescriptor.None) {
            TouchValue.AddModifier(value, source, desc);

        }
        public void RemoveModifiersFrom(EntityFactComponent source) {
            TouchValue.RemoveModifiersFrom(source);
            TryRemovePart();
        }
        private void TryRemovePart() {
            if (TouchValue.ModifierList.Count == 0) {
                base.Owner.Remove<MeleeTouchReach>();
            }
        }
        public override void OnTurnOn() {
            base.Owner.Remove<MeleeTouchReach>();
        }

        private ModifiableValue m_touchRange;
    }
}