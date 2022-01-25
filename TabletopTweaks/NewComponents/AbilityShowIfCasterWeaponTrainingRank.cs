using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Parts;

namespace TabletopTweaks.NewComponents {
    [TypeId("94cb46b01f8a458fa8fe39732047d10d")]
    public class AbilityShowIfCasterWeaponTrainingRank : BlueprintComponent, IAbilityVisibilityProvider {
        public bool IsAbilityVisible(AbilityData ability) {
            var weaponTraining = ability.Caster.Get<UnitPartWeaponTraining>();
            if (weaponTraining == null) {
                return false;
            }
            return weaponTraining.GetMaxWeaponRank() >= Rank;
        }
        public int Rank;
    }
}
