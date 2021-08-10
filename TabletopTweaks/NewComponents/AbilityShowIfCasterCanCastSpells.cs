using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using UnityEngine;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("974cd54a86984c36b459847fec6b2c4b")]
    class AbilityShowIfCasterCanCastSpells : BlueprintComponent, IAbilityVisibilityProvider {
        public bool IsAbilityVisible(AbilityData ability) {
            if (ability.Caster.Progression.GetClassLevel(Class.Get()) < 1) { return false; }
            return ability.Caster.GetSpellbook(Class.Get()).MaxSpellLevel >= Level;
        }
        [SerializeField]
        public BlueprintCharacterClassReference Class;
        [SerializeField]
        public int Level = 1;
    }
}
