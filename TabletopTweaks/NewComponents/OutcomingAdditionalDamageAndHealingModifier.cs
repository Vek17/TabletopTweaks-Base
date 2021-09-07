using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("f9bd6635909c40e09c3f4a22b711945b")]
    class OutcomingAdditionalDamageAndHealingModifier : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateDamage>,
        IRulebookHandler<RuleCalculateDamage>, ISubscriber,
        IInitiatorRulebookSubscriber,
        IInitiatorRulebookHandler<RuleHealDamage>,
        IRulebookHandler<RuleHealDamage> {
        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            if (Type == OutcomingAdditionalDamageAndHealingModifier.ModifyingType.OnlyHeal) {
                return;
            }
            evt.ParentRule.ModifierBonus = (ModifierPercents.Calculate(base.Fact.MaybeContext) / 100f) + (evt.ParentRule.ModifierBonus ?? 1f);
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }

        public void OnEventAboutToTrigger(RuleHealDamage evt) {
            if (Type == OutcomingAdditionalDamageAndHealingModifier.ModifyingType.OnlyDamage) {
                return;
            }
            evt.ModifierBonus = (ModifierPercents.Calculate(base.Fact.MaybeContext) / 100f) + (evt.ModifierBonus ?? 1f);
        }

        public void OnEventDidTrigger(RuleHealDamage evt) {
        }

        public ContextValue ModifierPercents;
        public OutcomingAdditionalDamageAndHealingModifier.ModifyingType Type;

        public enum ModifyingType {
            All,
            OnlyHeal,
            OnlyDamage
        }
    }
}
