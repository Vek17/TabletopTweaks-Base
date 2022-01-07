using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.NewComponents {
    [AllowMultipleComponents]
    [TypeId("9ea33bacd9fb466e996d243274f84f9a")]
    class AddAdditionalWeaponDamage : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (CheckWeaponRangeType && !RangeType.IsSuitableWeapon(evt.Weapon)) {
                return;
            }
            DamageDescription Damage = new DamageDescription {
                TypeDescription = DamageType,
                Dice = new DiceFormula(Value.DiceCountValue.Calculate(base.Context), Value.DiceType),
                SourceFact = base.Fact
            };
            evt.DamageDescription.Add(Damage);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public DamageTypeDescription DamageType;
        public ContextDiceValue Value;
        public bool CheckWeaponRangeType;
        public WeaponRangeType RangeType;
    }
}
