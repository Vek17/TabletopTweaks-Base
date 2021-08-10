using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {
    [TypeId("16d26ba95fde4646ae3c19d555a191f5")]
    class AddTouchReach : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            MeleeTouchReach unitPartTouchReach = Owner.Ensure<MeleeTouchReach>();

            int num = Value * base.Fact.GetRank();
            if (unitPartTouchReach != null) {
                unitPartTouchReach.AddModifier(num, base.Runtime, this.Descriptor);
            }
        }

        public override void OnTurnOff() {
            MeleeTouchReach unitPartTouchReach = Owner.Get<MeleeTouchReach>();
            if (!unitPartTouchReach) {
                return;
            }
            unitPartTouchReach.RemoveModifiersFrom(base.Runtime);
        }

        public override void OnActivate() {
            OnTurnOn();
        }

        public override void OnDeactivate() {
            OnTurnOff();
        }

        public int Value;
        public ModifierDescriptor Descriptor;
    }
}
