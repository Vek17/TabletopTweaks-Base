using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace TabletopTweaks.NewComponents {
    class SavingThrowBonusAgainstUnaware: UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
            UnitEntityData target = evt.GetRuleTarget();
            UnitEntityData caster = evt.Reason.Caster;
            if (caster == null || target == null) { return; }
            if (!target.Memory.Contains(caster) || (UnitPartConcealment.Calculate(target, caster, false) == Concealment.Total)) {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(this.Value * base.Fact.GetRank(), base.Runtime, this.Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(this.Value * base.Fact.GetRank(), base.Runtime, this.Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(this.Value * base.Fact.GetRank(), base.Runtime, this.Descriptor));
            }
        }

        public void OnEventDidTrigger(RuleSavingThrow evt) {
        }
#pragma warning disable 0649
        public ModifierDescriptor Descriptor;
        public int Value;
#pragma warning restore 0649
    }
}
