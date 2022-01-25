using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintParametrizedFeature))]
    [TypeId("fd74a9c62e844ac09f9aa5ce81a427cc")]
    public class FocusedWeaponComponent : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            WeaponCategory? category = base.Param.WeaponCategory;
            base.Owner.Ensure<UnitPartFocusedWeapon>().AddEntry(category, base.Fact);
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartFocusedWeapon>().RemoveEntry(base.Fact);
        }

    }
}
