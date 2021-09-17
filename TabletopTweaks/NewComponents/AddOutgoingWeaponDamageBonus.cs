using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {
    [TypeId("03f55b5c7cb0445ab32ce2c8d44704ec")]
    class AddOutgoingWeaponDamageBonus : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateDamage>,
        IRulebookHandler<RuleCalculateDamage>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            if (evt.DamageBundle.First == null) { return; }

            var WeaponDamage = evt.DamageBundle.First;
            DamageTypeDescription description = GenerateTypeDescriptiron(WeaponDamage);

            BaseDamage additionalDamage = description.CreateDamage(
                dice: new DiceFormula(WeaponDamage.Dice.Rolls * BonusDamageMultiplier, WeaponDamage.Dice.Dice),
                bonus: WeaponDamage.Bonus * BonusDamageMultiplier
            );
            additionalDamage.SourceFact = base.Fact;
            var DamageBonus = base.Owner.Ensure<OutgoingWeaponDamageBonus>();
            DamageBonus.AddBonus(evt, additionalDamage);
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
#if false
            OutgoingWeaponDamageBonus unitOutgoingWeaponDamageBonus = Owner.Get<OutgoingWeaponDamageBonus>();
            if (!unitOutgoingWeaponDamageBonus) {
                return;
            }
            Owner.Remove<OutgoingWeaponDamageBonus>();
#endif
        }

        private static DamageTypeDescription GenerateTypeDescriptiron(BaseDamage WeaponDamage) {
            DamageTypeDescription description = WeaponDamage.CreateTypeDescription();

            switch (WeaponDamage.Type) {
                case DamageType.Physical: {
                        var physical = WeaponDamage as PhysicalDamage;
                        description.Physical.Enhancement = physical.Enchantment;
                        description.Physical.EnhancementTotal = physical.EnchantmentTotal;
                        description.Physical.Form = physical.Form;
                        description.Physical.Material = physical.MaterialsMask;
                        return description;
                    }
                case DamageType.Energy:
                    var energy = WeaponDamage as EnergyDamage;
                    description.Energy = energy.EnergyType;
                    return description;
                case DamageType.Force:
                    var force = WeaponDamage as ForceDamage;
                    return description;
                case DamageType.Direct:
                    var direct = WeaponDamage as DirectDamage;
                    return description;
                case DamageType.Untyped:
                    var untyped = WeaponDamage as UntypedDamage;
                    return description;
                default:
                    return description;
            }
        }

        public int BonusDamageMultiplier = 1;
    }
}
