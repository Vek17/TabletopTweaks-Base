using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;

namespace TabletopTweaks.NewComponents {
    [TypeId("34455dfff53d4c349bd19eb9ebb85ab6")]
    class WeaponDamageMultiplierReplacement : WeaponEnchantmentLogic,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (evt.Weapon == base.Owner) {
                evt.OverrideDamageBonusStatMultiplier(this.Multiplier);
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public float Multiplier;
    }
}
