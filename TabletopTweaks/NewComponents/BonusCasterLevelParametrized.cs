using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;


namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintParametrizedFeature), false)]
    [TypeId("9593096f04ca4c63afee29478efac0cc")]
    public class BonusCasterLevelParametrized : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateAbilityParams>,
        IRulebookHandler<RuleCalculateAbilityParams>,
        ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt) {
            BlueprintAbility spell = evt.Spell;
            SpellSchool spellSchool;
            if (spell == null) {
                spellSchool = SpellSchool.None;
            } else {
                SpellComponent component = spell.GetComponent<SpellComponent>();
                spellSchool = component?.School ?? SpellSchool.None;
            }
            bool matchesSchool = spellSchool == base.Param;
            bool isExpandedArsenal = false;
            if (!base.Owner.Progression.Features.Enumerable.Any((Feature p) => p.Blueprint == this.Fact.Blueprint && p.Param == spellSchool)) {
                UnitPartExpandedArsenal unitPartExpandedArsenal = base.Owner.Get<UnitPartExpandedArsenal>();
                isExpandedArsenal = unitPartExpandedArsenal?.HasSpellSchoolEntry(spellSchool) ?? false;
            }
            if (matchesSchool || isExpandedArsenal) {
                evt.AddBonusCasterLevel(Bonus.Calculate(base.Context), Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt) {
        }
        public ContextValue Bonus;
        public ModifierDescriptor Descriptor;
    }
}
