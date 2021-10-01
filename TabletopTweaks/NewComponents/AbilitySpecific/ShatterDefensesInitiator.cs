using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.ContextData;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("52c4991ee89544a2973b7e8b95396aba")]
    class ShatterDefensesInitiator : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RuleAttackRoll>,
        IRulebookHandler<RuleAttackRoll>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleAttackRoll evt) {
            IsValid = evt.Target.State.HasCondition(UnitCondition.Shaken) || evt.Target.State.HasCondition(UnitCondition.Frightened);
        }

        public void OnEventDidTrigger(RuleAttackRoll evt) {
            if (IsSuitable(evt)) {
                RunActions(evt);
            }
        }

        private void RunActions(RuleAttackRoll rule) {
            UnitEntityData unit = rule.Target;
            using (ContextData<ContextAttackData>.Request().Setup(rule, null)) {
                if (!base.Fact.IsDisposed) {
                    base.Fact.RunActionInContext(this.Action, unit);
                } else {
                    using (base.Context.GetDataScope(unit)) {
                        this.Action.Run();
                    }
                }
            }
        }

        private bool IsSuitable(RuleAttackRoll evt) {
            return evt.IsHit && IsValid;
        }

        private bool IsValid;
        public ActionList Action;
    }
}
