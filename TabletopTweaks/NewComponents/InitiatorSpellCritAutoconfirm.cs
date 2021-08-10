using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("6342f290ed63432e9195488d8259330b")]
    class InitiatorSpellCritAutoconfirm : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleAttackRoll evt) {
            if (evt.Reason.Ability != null) {
                evt.AutoCriticalConfirmation = true;
            }
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) {
        }
    }
}
