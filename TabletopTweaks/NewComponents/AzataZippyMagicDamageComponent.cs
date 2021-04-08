using Kingmaker.Blueprints;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    class AzataZippyMagicDamageComponent : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCastSpell>, IRulebookHandler<RuleCastSpell>, ISubscriber, IInitiatorRulebookSubscriber {

		void IRulebookHandler<RuleCastSpell>.OnEventAboutToTrigger(RuleCastSpell evt) {
		}

		private bool isValidTrigger(RuleCastSpell evt) {
			return (!evt.IsSpellFailed &&
					!evt.Spell.IsAOE &&
					evt.SpellTarget.Unit.IsPlayersEnemy &&
					!evt.Spell.Blueprint.GetComponents<AbilityEffectStickyTouch>().Any()); 
		}

		void IRulebookHandler<RuleCastSpell>.OnEventDidTrigger(RuleCastSpell evt) {
			if (!isValidTrigger(evt)) {
				return;
			}
			DiceFormula dice = new DiceFormula(2, DiceType.D6);
			int mythicLevel = evt.Spell.Caster.Unit.Progression.MythicLevel;
			RuleDealDamage ruleDealDamage = new RuleDealDamage(evt.Spell.Caster, evt.SpellTarget.Unit, new EnergyDamage(dice, mythicLevel, DamageEnergyType.Divine));
			Rulebook.Trigger<RuleDealDamage>(ruleDealDamage);
		}
	}
}