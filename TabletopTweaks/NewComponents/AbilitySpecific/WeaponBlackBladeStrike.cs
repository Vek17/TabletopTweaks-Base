using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("e3c345a3c9ae4f84a0f86b42101b294d")]
    class WeaponBlackBladeStrike : WeaponEnchantmentLogic, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, 
        IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber {

        public override void OnTurnOn() {
            bonus = CalculateModifier();
        }

        private int CalculateModifier() {
            var MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var MagusLevel = Owner?.Wielder?.Progression?.GetClassLevel(MagusClass) ?? 0;
            var bonus = MagusLevel switch {
                >= 5 and <= 8 => 2,
                >= 9 and <= 12 => 3,
                >= 13 and <= 16 => 4,
                >= 17 and <= 20 => 5,
                _ => 1
            };
            return bonus;
        }

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            evt.AddDamageModifier(bonus, base.Fact);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        private int bonus;
    }
}
