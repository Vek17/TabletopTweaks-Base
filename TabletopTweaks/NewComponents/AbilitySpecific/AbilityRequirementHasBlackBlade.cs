using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("a0ff3623a0154448a082b1c5ea9898fc")]
    class AbilityRequirementHasBlackBlade : BlueprintComponent, IAbilityRestriction {
        public string GetAbilityRestrictionUIText() {
            return $"You must be wielding your Black Blade";
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability) {
            var BlackBlade = ability.Caster.Get<UnitPartBlackBlade>();
            var weapon = ability.Caster.Body.PrimaryHand.MaybeWeapon;
            if (BlackBlade == null || weapon == null) { return false; }
            return BlackBlade.IsBlackBlade(weapon);
        }
    }
}
