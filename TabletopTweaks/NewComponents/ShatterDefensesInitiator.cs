using System;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.ContextData;

namespace TabletopTweaks.NewComponents {
    class ShatterDefensesInitiator : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RuleAttackWithWeapon>,
        IRulebookHandler<RuleAttackWithWeapon>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
            Main.Log($"ShatterDefensesInitiator::OnEventAboutToTrigger - BeforeValid: {IsValid}");
            IsValid = evt.Target.State.HasCondition(UnitCondition.Shaken) || evt.Target.State.HasCondition(UnitCondition.Frightened);
            Main.Log($"ShatterDefensesInitiator::OnEventAboutToTrigger - AfterValid: {IsValid}");
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
            Main.Log($"ShatterDefensesInitiator::OnEventDidTrigger - Valid: {IsValid}");
            if (!IsSuitable(evt)) {
                return;
            }
            MechanicsContext context = base.Context;
            EntityFact fact = base.Fact;

            Main.Log($"ShatterDefensesInitiator::OnEventDidTrigger - Trigger Actions");
            foreach (RuleAttackWithWeaponResolve ruleAttackWithWeaponResolve in evt.ResolveRules) {
                if (ruleAttackWithWeaponResolve.IsTriggered) {
                    RunActions(this, evt, context, fact);
                } else {
                    Delegate onResolve = ruleAttackWithWeaponResolve.OnResolve;
                    Action<RuleAttackWithWeaponResolve> shatter = (rule) => { RunActions(this, evt, context, fact); };
                    ruleAttackWithWeaponResolve.OnResolve = (Action<RuleAttackWithWeaponResolve>)Delegate.Combine(onResolve, shatter);
                }
            }
        }

        private static void RunActions(ShatterDefensesInitiator c, RuleAttackWithWeapon rule, MechanicsContext context, EntityFact fact) {
            Main.Log($"ShatterDefensesInitiator::RunActions - Valid: {c.IsValid}");
            UnitEntityData unit = rule.Target;
            using (ContextData<ContextAttackData>.Request().Setup(rule.AttackRoll, null)) {
                if (!fact.IsDisposed) {
                    fact.RunActionInContext(c.Action, unit);
                } else {
                    using (context.GetDataScope(unit)) {
                        c.Action.Run();
                    }
                }
            }
        }

        private bool IsSuitable(RuleAttackWithWeapon evt) {
            return evt.AttackRoll.IsHit && IsValid;
        }

        private static bool IsSuitable(AddInitiatorAttackWithWeaponTrigger c, RuleDealDamage damage) {
            if (c.ReduceHPToZero) {
                return damage != null && !damage.IsFake && damage.Target.HPLeft <= 0 && damage.Target.HPLeft + damage.Result > 0;
            }
            return !c.DamageMoreTargetMaxHP || (damage != null && !damage.IsFake && damage.Target.MaxHP <= damage.Result);
        }

        private bool IsValid;
        public ActionList Action;
    }
}
