using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("e3c345a3c9ae4f84a0f86b42101b294d")]
    public class WeaponBlackBladeStrike : WeaponEnchantmentLogic, IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber {

        private int CalculateModifier() {
            var rank = WeilderProperty.Get().GetInt(base.Owner.Wielder);
            var bonus = rank switch {
                >= 5 and <= 8 => 2,
                >= 9 and <= 12 => 3,
                >= 13 and <= 16 => 4,
                >= 17 and <= 20 => 5,
                _ => 1
            };
            return bonus;
        }

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            evt.AddDamageModifier(CalculateModifier(), base.Fact);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public BlueprintUnitPropertyReference WeilderProperty;
    }
}
