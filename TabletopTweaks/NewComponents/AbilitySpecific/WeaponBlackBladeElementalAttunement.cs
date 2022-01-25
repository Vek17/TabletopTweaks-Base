using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("2f13fd3858d642d3ba16351be5168254")]
    public class WeaponBlackBladeElementalAttunement : WeaponEnchantmentLogic, IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
            evt.DamageDescription[0].TypeDescription = Type;
        }

        public DamageTypeDescription Type;
    }
}
