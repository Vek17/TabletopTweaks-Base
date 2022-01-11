using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System.Linq;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("aabbfeda974c455aafe14d05efca4f67")]
    class MythicSneakAttack : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>,
        ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RulePrepareDamage evt) { 
        }

        public void OnEventDidTrigger(RulePrepareDamage evt) {
            evt?.ParentRule?.m_DamageBundle?
                .Where(damage => damage.Sneak)
                .ForEach(damage => {
                    var rolls = damage.Dice.Rolls;
                    var originalDice = damage.Dice.Dice;
                    
                    switch (originalDice) {
                        case DiceType.D3:
                            damage.Dice = new DiceFormula(rolls, DiceType.D4);
                            break;
                        case DiceType.D4:
                            damage.Dice = new DiceFormula(rolls, DiceType.D6);
                            break;
                        case DiceType.D6:
                            damage.Dice = new DiceFormula(rolls, DiceType.D8);
                            break;
                        case DiceType.D8:
                            damage.Dice = new DiceFormula(rolls, DiceType.D10);
                            break;
                        case DiceType.D10:
                            damage.Dice = new DiceFormula(rolls, DiceType.D12);
                            break;
                    }
                });
        }
    }
}
