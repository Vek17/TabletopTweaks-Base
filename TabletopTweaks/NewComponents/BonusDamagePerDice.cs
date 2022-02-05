using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;


namespace TabletopTweaks.NewComponents {
    [TypeId("5608ad3529df47a1821463fa85ce10b9")]
    public class BonusDamagePerDice : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleCalculateDamage>, 
        IRulebookHandler<RuleCalculateDamage>, 
        ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            MechanicsContext context = evt.Reason.Context;
            if (((context != null) ? context.SourceAbility : null) == null) {
                return;
            }
            if (CheckDescriptor && !context.SpellDescriptor.HasAnyFlag(SpellDescriptor)) {
                return;
            }
            if (SpellsOnly && !context.SourceAbility.IsSpell) {
                return;
            }
            if (context.SourceAbility.Type == AbilityType.Physical) {
                return;
            }
            foreach (BaseDamage baseDamage in evt.DamageBundle) {
                if (!baseDamage.Precision) {
                    int bonus = UseContextBonus ? (Value.Calculate(context) * baseDamage.Dice.Rolls) : baseDamage.Dice.Rolls;
                    baseDamage.AddModifier(bonus, base.Fact);
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }
        public bool CheckDescriptor = true;
        public SpellDescriptorWrapper SpellDescriptor;
        public bool SpellsOnly = true;
        public bool UseContextBonus;
        [ShowIf("UseContextBonus")]
        public ContextValue Value;
    }
}
