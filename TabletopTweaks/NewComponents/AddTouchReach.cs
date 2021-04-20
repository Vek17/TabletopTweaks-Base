using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {
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

		public int Value;
		public ModifierDescriptor Descriptor;
	}
}
