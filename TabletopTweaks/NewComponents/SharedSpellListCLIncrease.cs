using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("2e06e1dac73940c3b46ca0db611856a3")]
    class SharedSpellListCLIncrease : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateAbilityParams>,
        IRulebookHandler<RuleCalculateAbilityParams>,
        ISubscriber, IInitiatorRulebookSubscriber {

        private ReferenceArrayProxy<BlueprintSpellList, BlueprintSpellListReference> SpellLists {
            get {
                return this.m_SpellLists;
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt) {
            if (SpellsOnly && evt.Spellbook == null) {
                return;
            }
            if (SpellLists.Any(list => !list.Contains(evt.Spell))) {
                return;
            }
            evt.AddBonusCasterLevel(Bonus, ModifierDescriptor);
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt) {
        }
        public BlueprintSpellListReference[] m_SpellLists;
        public int Bonus;
        public ModifierDescriptor ModifierDescriptor = ModifierDescriptor.UntypedStackable;
        public bool SpellsOnly;
    }
}
