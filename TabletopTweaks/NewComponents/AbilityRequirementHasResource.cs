using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace TabletopTweaks.NewComponents {
    [TypeId("e797289b00ee463d886561ad79c2ad4f")]
    public class AbilityRequirementHasResource : BlueprintComponent, IAbilityRestriction {
        public string GetAbilityRestrictionUIText() {
            return $"You don't have enough of required resource";
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability) {
            return ability.Caster.Resources.HasEnoughResource(Resource, Amount);
        }

        public int Amount;
        public BlueprintAbilityResourceReference Resource;
    }
}
