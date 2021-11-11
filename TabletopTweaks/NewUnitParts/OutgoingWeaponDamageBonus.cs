using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.NewUnitParts {
    class OutgoingWeaponDamageBonus : UnitPart {

        public void AddBonus(RuleCalculateDamage evt, BaseDamage additionalDamage) {
            if (this.evt != evt) {
                if (lastAttack == evt.ParentRule?.AttackRoll) { return; }
                this.evt = evt;
                lastAttack = evt.ParentRule?.AttackRoll;
                baseDamage = null;
            }
            if (baseDamage == null) {
                baseDamage = additionalDamage;
                evt.ParentRule.m_DamageBundle.m_Chunks.Insert(1, baseDamage);
            } else {
                baseDamage.Dice = new DiceFormula(baseDamage.Dice.Rolls + additionalDamage.Dice.Rolls, baseDamage.Dice.Dice);
                baseDamage.Bonus += additionalDamage.Bonus;
            }
        }

        private BaseDamage baseDamage;
        private RuleCalculateDamage evt;
        private RuleAttackRoll lastAttack;
    }
}