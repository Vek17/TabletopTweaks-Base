using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("fd7dabc757e74a52a6a0c6e64fd5fd6b")]
    class ContextDispelBonusOnType : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleDispelMagic>,
        IRulebookHandler<RuleDispelMagic>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleDispelMagic evt) {
            if (IgnoreType || evt.Check == Type) {
                evt.Bonus += Bonus.Calculate(this.Context);
            }
        }

        public void OnEventDidTrigger(RuleDispelMagic evt) {
        }

        public ContextValue Bonus;
        public RuleDispelMagic.CheckType Type;
        public bool IgnoreType;
    }
}
