using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using UnityEngine;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("e7a4d4a377d44ab997dbaf1ec66b4b8b")]
    public class DimensionalRetributionLogic : UnitFactComponentDelegate<DweomerLeapLogicData>, IApplyAbilityEffectHandler, IGlobalSubscriber, ISubscriber {
        private BlueprintAbility Ability {
            get {
                BlueprintAbilityReference ability = this.m_Ability;
                if (ability == null) {
                    return null;
                }
                return ability.Get();
            }
        }

        public override void OnActivate() {
            base.Data.AppliedAbility = base.Owner.AddFact<Ability>(this.Ability, null, null);
        }

        public override void OnDeactivate() {
            base.Owner.RemoveFact(base.Data.AppliedAbility);
            base.Data.AppliedAbility = null;
        }

        public void OnAbilityEffectApplied(AbilityExecutionContext context) {
        }

        public void OnTryToApplyAbilityEffect(AbilityExecutionContext context, TargetWrapper target) {
            float weaponRange = base.Owner.View.Corpulence + context.Caster.View.Corpulence + GameConsts.MinWeaponRange.Meters;
            if (target.Unit == base.Owner
                && context.SourceAbility.IsSpell
                && base.Owner.DistanceTo(context.Caster) > weaponRange
                && base.Owner.State.CanAct
                && base.Owner.CombatState.CanAttackOfOpportunity
                && (context.MaybeCaster?.IsEnemy(target.Unit) ?? false)) {
                Rulebook.Trigger(new RuleCastSpell(this.Data.AppliedAbility, context.Caster) { 
                    IsDuplicateSpellApplied = true
                });
            }
        }

        public void OnAbilityEffectAppliedToTarget(AbilityExecutionContext context, TargetWrapper target) {
        }

        [SerializeField]
        public BlueprintAbilityReference m_Ability;
    }
}
