using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using static Kingmaker.Designers.Mechanics.Facts.ModifyD20;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    class AeonPowerOfLaw : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleSavingThrow>,
        IRulebookHandler<RuleSavingThrow>,
        IInitiatorRulebookHandler<RuleRollD20>,
        IRulebookHandler<RuleRollD20>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
        }
        public void OnEventDidTrigger(RuleSavingThrow evt) {
        }

        public void OnEventAboutToTrigger(RuleRollD20 evt) {
            
        }

        public void OnEventDidTrigger(RuleRollD20 evt) {
            if (ShouldReroll(evt)) {
                evt.Override(RollResult.Calculate(base.Context));
            }
        }

        private bool ShouldReroll(RuleRollD20 rollRule) {
            switch (this.RollCondition) {
                case RollConditionType.ShouldBeMoreThan:
                    return rollRule.Result > this.ValueToCompareRoll.Calculate(base.Context);
                case RollConditionType.ShouldBeLessThan:
                    return rollRule.Result < this.ValueToCompareRoll.Calculate(base.Context);
                case RollConditionType.ShouldBeLessOrEqualThan:
                    return rollRule.Result <= this.ValueToCompareRoll.Calculate(base.Context);
                case RollConditionType.ShouldBeMoreOrEqualThan:
                    return rollRule.Result >= this.ValueToCompareRoll.Calculate(base.Context);
                case RollConditionType.Equal:
                    return rollRule.Result == this.ValueToCompareRoll.Calculate(base.Context);
                default:
                    return true;
            }
        }

        public ContextValue RollResult;
        public ContextValue ValueToCompareRoll;
        public RollConditionType RollCondition;
    }
}
