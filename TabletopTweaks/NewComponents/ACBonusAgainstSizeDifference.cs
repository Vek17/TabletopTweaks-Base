using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.NewComponents {
	[AllowMultipleComponents]
	[TypeId("b3a1e6c7233e4388a09b149964705b03")]
	class ACBonusAgainstSizeDifference : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber {
		public void OnEventAboutToTrigger(RuleAttackRoll evt) {
			var sizeDifference = Smaller ? evt.Initiator.State.Size - Owner.Descriptor.State.Size : Owner.Descriptor.State.Size - evt.Initiator.State.Size;
			if (sizeDifference >= Steps) {
				int num = Value.Calculate(base.Context);
				evt.AddTemporaryModifier(Owner.Stats.AC.AddModifier(num * base.Fact.GetRank(), base.Runtime, Descriptor));
			}
		}

		public void OnEventDidTrigger(RuleAttackRoll evt) {
		}

		public ModifierDescriptor Descriptor;
		public ContextValue Value;
		public Size Size;
		public bool Smaller;
		public int Steps = 1;
	}
}
