using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    class PrecisionCriticalComponent : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber {

        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartPrecisionCritical>().AddEntry(CriticalMultiplier, Additional, base.Fact);
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartPrecisionCritical>().RemoveEntry(base.Fact);
        }

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            if ((evt.ParentRule?.DamageBundle?.WeaponDamage?.CriticalModifier ?? 1) < 2) { return; }
            var PrecisionCritical = base.Owner.Get<UnitPartPrecisionCritical>();

            foreach (var baseDamage in evt.ParentRule.DamageBundle) {
                if (baseDamage.Precision || baseDamage.Sneak) {
                    baseDamage.CriticalModifier = PrecisionCritical?.GetMultiplier();
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }

        public int CriticalMultiplier;
        public bool Additional;
    }
}
