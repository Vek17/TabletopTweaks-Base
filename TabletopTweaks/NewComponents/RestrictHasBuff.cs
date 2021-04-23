using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using UnityEngine;

namespace TabletopTweaks.NewComponents {
    class RestrictHasBuff: BlueprintComponent, IAbilityRestriction {

        public string GetAbilityRestrictionUIText() {
            return $"Required Buff: {RequiredBuff.Get().Name}";
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability) {
            return ability.Caster.Buffs.HasFact(RequiredBuff);
        }

        [SerializeField]
        public BlueprintBuffReference RequiredBuff;
    }
}
