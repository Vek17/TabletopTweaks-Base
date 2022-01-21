using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.NewComponents {
    [AllowMultipleComponents]
    [TypeId("9ea33bacd9fb466e996d243274f84f9a")]
    class WeaponExtraDamageDice : WeaponEnchantmentLogic,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (evt.Weapon == base.Owner) {
                DamageDescription Damage = new DamageDescription {
                    TypeDescription = DamageType,
                    Dice = Value,
                    SourceFact = base.Fact
                };
                evt.DamageDescription.Add(Damage);
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public DamageTypeDescription DamageType;
        public DiceFormula Value;
    }
}
