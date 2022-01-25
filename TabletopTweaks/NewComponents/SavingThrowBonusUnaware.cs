using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace TabletopTweaks.NewComponents {
    [TypeId("95dfe16fb6c4457bafb5eb56c983b8b5")]
    public class SavingThrowBonusWhileUnaware : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
            UnitEntityData target = evt.GetRuleTarget();
            UnitEntityData caster = evt.Reason.Caster;
            if (caster == null || target == null) { return; }
            if (!target.Memory.Contains(caster) || (UnitPartConcealment.Calculate(target, caster, false) == Concealment.Total)) {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(Value * base.Fact.GetRank(), base.Runtime, Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(Value * base.Fact.GetRank(), base.Runtime, Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(Value * base.Fact.GetRank(), base.Runtime, Descriptor));
            }
        }

        public void OnEventDidTrigger(RuleSavingThrow evt) {
        }

        public ModifierDescriptor Descriptor;
        public int Value;
    }
}
