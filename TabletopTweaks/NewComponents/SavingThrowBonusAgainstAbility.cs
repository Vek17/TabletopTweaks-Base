using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace TabletopTweaks.NewComponents {
    class SavingThrowBonusAgainstAbility: UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber {

        public BlueprintFeature CheckedFact {
            get {
                BlueprintFeatureReference checkedFact = this.m_CheckedFact;
                if (checkedFact == null) {
                    return null;
                }
                return checkedFact.Get();
            }
        }

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
            if (evt.Reason.Fact != null && evt.Reason.Fact.Blueprint == CheckedFact) {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(this.Value * base.Fact.GetRank(), base.Runtime, this.Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(this.Value * base.Fact.GetRank(), base.Runtime, this.Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(this.Value * base.Fact.GetRank(), base.Runtime, this.Descriptor));
            }
        }

        public void OnEventDidTrigger(RuleSavingThrow evt) {
        }

        [SerializeField]
        public BlueprintFeatureReference m_CheckedFact;
        public ModifierDescriptor Descriptor;
        public int Value;

    }
}
