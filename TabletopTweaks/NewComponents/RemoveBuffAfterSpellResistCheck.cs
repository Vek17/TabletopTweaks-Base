using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.NewComponents {
    [TypeId("ee58a44d90014e58a3c5320e53975043")]
    class RemoveBuffAfterSpellResistCheck : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSpellResistanceCheck>, IRulebookHandler<RuleSpellResistanceCheck>, ISubscriber, IInitiatorRulebookSubscriber {
        // Token: 0x0600A2AE RID: 41646 RVA: 0x000036D8 File Offset: 0x000018D8
        public void OnEventAboutToTrigger(RuleSpellResistanceCheck evt) {
        }

        // Token: 0x0600A2AF RID: 41647 RVA: 0x0027A8A4 File Offset: 0x00278AA4
        public void OnEventDidTrigger(RuleSpellResistanceCheck evt) {
            base.Owner.RemoveFact(base.Fact);
        }
    }
}
