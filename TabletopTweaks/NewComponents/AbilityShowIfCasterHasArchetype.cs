using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using UnityEngine;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("faaed80f2d01490bb8be1424f8d12665")]
    class AbilityShowIfCasterHasArchetype : BlueprintComponent, IAbilityVisibilityProvider {
        public bool IsAbilityVisible(AbilityData ability) {
            return ability.Caster.Progression.GetClassData(Class)?.Archetypes.Contains(Archetype) ?? false;
        }
        [SerializeField]
        public BlueprintArchetypeReference Archetype;
        [SerializeField]
        public BlueprintCharacterClassReference Class;
    }
}
