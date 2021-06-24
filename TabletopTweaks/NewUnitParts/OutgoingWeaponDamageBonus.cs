using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.NewUnitParts {
    class OutgoingWeaponDamageBonus: UnitPart {

        public void AddBonus(RuleCalculateDamage evt, BaseDamage additionalDamage) {
            if (this.evt != evt) {
                this.evt = evt;
                baseDamage = null;
            }
            if (baseDamage == null) {
                baseDamage = additionalDamage;
                evt.ParentRule.m_DamageBundle.m_Chunks.Insert(1, baseDamage);
            } else {
                var Dice = baseDamage.Dice;
                baseDamage.Dice = new DiceFormula(baseDamage.Dice.Rolls + additionalDamage.Dice.Rolls, baseDamage.Dice.Dice);
                baseDamage.Bonus += additionalDamage.Bonus;
            }
        }
        private void TryRemovePart() {
            base.Owner.Remove<OutgoingWeaponDamageBonus>();
        }

        private BaseDamage baseDamage;
        private RuleCalculateDamage evt;
    }
}