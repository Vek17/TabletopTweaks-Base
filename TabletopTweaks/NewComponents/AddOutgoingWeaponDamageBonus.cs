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
            BaseDamage additionalDamage = WeaponDamage.CreateTypeDescription().CreateDamage(
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

        public int BonusDamageMultiplier = 1;
    }
}
