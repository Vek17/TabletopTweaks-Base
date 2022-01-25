using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using UnityEngine;

namespace TabletopTweaks.NewComponents {
    [TypeId("8c2d2cced7a44fa7a352e65a120466d6")]
    public class AbilityRequirementHasBuff : BlueprintComponent, IAbilityRestriction {

        public string GetAbilityRestrictionUIText() {
            return !Not ? $"Required Buff: {RequiredBuff.Get().Name}" : $"Not allowed with Buff: {RequiredBuff.Get().Name}";
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability) {
            return (!Not && ability.Caster.Buffs.HasFact(RequiredBuff))
                || (Not && !ability.Caster.Buffs.HasFact(RequiredBuff));
        }

        [SerializeField]
        public BlueprintBuffReference RequiredBuff;
        public bool Not = false;
    }
}
