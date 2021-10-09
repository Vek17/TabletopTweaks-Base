using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.NewEvents;

namespace TabletopTweaks.NewComponents {
    [TypeId("a0c9a0074a8944aca4b83d16126bfdb2")]
    class SpontaneousSpecialListConversion : UnitFactComponentDelegate, ISpontaneousConversionHandler {

        public void HandleGetConversions(AbilityData ability, ref IEnumerable<AbilityData> conversions) {
            var conversionList = conversions.ToList();
            var spellbook = Owner.DemandSpellbook(m_CharacterClass);
            if (spellbook == null) { return; }
            if (ability.Spellbook?.Blueprint == spellbook.Blueprint) {
                for (int level = ability.SpellLevel; level > 0; level--) {
                    foreach (var spellList in spellbook.SpecialLists.Where(list => !list.IsMythic)) {
                        foreach (var spell in spellList.GetSpells(level)) {
                            if (spell == ability.Blueprint) { continue; }
                            AbilityVariants variantComponent = spell.GetComponent<AbilityVariants>();
                            if (variantComponent != null) {
                                foreach (var variant in variantComponent.Variants) {
                                    AbilityData.AddAbilityUnique(ref conversionList, new AbilityData(ability, variant));
                                }
                                continue;
                            }
                            AbilityData.AddAbilityUnique(ref conversionList, new AbilityData(ability, spell));
                        }
                    }
                }
                conversions = conversionList;
            }
        }
        public BlueprintCharacterClassReference m_CharacterClass;
    }
}
