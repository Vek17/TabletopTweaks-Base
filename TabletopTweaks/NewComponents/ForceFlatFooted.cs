using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.NewComponents {
    [TypeId("68de0e05a62241a6bd00a9107a493041")]
    class ForceFlatFooted : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCheckTargetFlatFooted>, ITargetRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCheckTargetFlatFooted evt) {
            if (TacticalCombatHelper.IsActive) { return; }
            var Caster = Context?.MaybeCaster;
            if (!AgainstCaster) {
                evt.ForceFlatFooted = true;
            } else if(Caster == evt.Initiator) {
                evt.ForceFlatFooted = true;
            }
        }

        public void OnEventDidTrigger(RuleCheckTargetFlatFooted evt) {
        }

        public bool AgainstCaster;
    }
}
