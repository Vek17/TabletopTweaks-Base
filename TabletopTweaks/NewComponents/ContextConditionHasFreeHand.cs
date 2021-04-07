using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace TabletopTweaks.NewComponents {
	public class ContextConditionHasFreeHand : ContextCondition {
		public override string GetConditionCaption() {
			return "Check if caster has a free hand";
		}

        public override bool CheckCondition() {
			var secondaryHand = Target.Unit.Body.CurrentHandsEquipmentSet.SecondaryHand;
			var primaryHand = Target.Unit.Body.CurrentHandsEquipmentSet.PrimaryHand;
			bool hasFreeHand = true;
            if (secondaryHand.HasShield) {
				var maybeShield = secondaryHand.MaybeShield;
				hasFreeHand = maybeShield.Blueprint.Type.ProficiencyGroup == ArmorProficiencyGroup.Buckler ? true : false;
			}
            if (!secondaryHand.HasWeapon || secondaryHand.MaybeWeapon == Target.Unit.Body.EmptyHandWeapon) {
                if (primaryHand.HasWeapon) {
					hasFreeHand = primaryHand.MaybeWeapon.HoldInTwoHands ? false : hasFreeHand;
				}
			}
			//Main.Log($"Check Result: {hasFreeHand}");
			return hasFreeHand;
		}
	}
}
