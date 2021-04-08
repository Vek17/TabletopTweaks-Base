using Kingmaker.Controllers.Optimization;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using System.Collections.Generic;
using UnityEngine;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Blueprints;
using System.Linq;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Enums.Damage;

namespace TabletopTweaks.NewComponents {
    class AzataZippyMagicComponent : UnitFactComponentDelegate, 
		IInitiatorRulebookHandler<RuleCastSpell>, 
		IRulebookHandler<RuleCastSpell>, 
		ISubscriber, 
		IInitiatorRulebookSubscriber {

		void IRulebookHandler<RuleCastSpell>.OnEventAboutToTrigger(RuleCastSpell evt) {
		}

		private bool isValidTrigger(RuleCastSpell evt) {
			return  !evt.IsSpellFailed &&
					!evt.Spell.IsAOE &&
					!evt.Spell.Blueprint.GetComponents<AbilityEffectStickyTouch>().Any() &&
					!evt.Spell.Blueprint.GetComponents<BlockSpellDuplicationComponent>().Any();
		}

		void IRulebookHandler<RuleCastSpell>.OnEventDidTrigger(RuleCastSpell evt) {
			if (!isValidTrigger(evt)) {
				return;
			}
			if (!m_GeneratedRules.Contains(evt)) {
				AbilityData spell = evt.Spell;
				UnitEntityData newTarget = GetNewTarget(spell, evt.SpellTarget.Unit);
				if (newTarget != null) {
					RuleCastSpell ruleCastSpell = new RuleCastSpell(spell, new TargetWrapper(newTarget));
					m_GeneratedRules.Add(ruleCastSpell);
					Rulebook.Trigger<RuleCastSpell>(ruleCastSpell);
					m_GeneratedRules.Remove(ruleCastSpell);
				}
			}
			if (evt.SpellTarget.Unit.Group.IsEnemy(Owner)) {
				DiceFormula dice = new DiceFormula(2, DiceType.D6);
				int mythicLevel = evt.Spell.Caster.Unit.Progression.MythicLevel;
				RuleDealDamage ruleDealDamage = new RuleDealDamage(evt.Spell.Caster, evt.SpellTarget.Unit, new EnergyDamage(dice, mythicLevel, DamageEnergyType.Divine));
				Rulebook.Trigger<RuleDealDamage>(ruleDealDamage);
			}
		}

		private UnitEntityData GetNewTarget(AbilityData data, UnitEntityData baseTarget) {
			List<UnitEntityData> list = EntityBoundsHelper.FindUnitsInRange(Owner.Position, m_FeetsRaiuds.Feet().Meters);
			list.Remove(baseTarget);
			list.Remove(Owner);
			list.RemoveAll((UnitEntityData x) => x.Faction != baseTarget.Faction || !data.CanTarget(x));
			if (list.Count <= 0) {
				return null;
			}
			return list.GetRandomElements(1, new System.Random())[0];
		}

		[SerializeField]
		private int m_FeetsRaiuds = 30;

		private HashSet<RuleCastSpell> m_GeneratedRules = new HashSet<RuleCastSpell>();
	}
}
