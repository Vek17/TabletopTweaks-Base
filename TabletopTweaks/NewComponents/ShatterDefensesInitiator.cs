using System;
using Kingmaker.Blueprints.JsonSystem;
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
    [TypeId("52c4991ee89544a2973b7e8b95396aba")]
    class ShatterDefensesInitiator : EntityFactComponentDelegate,
        IInitiatorRulebookHandler<RuleAttackWithWeapon>,
        IRulebookHandler<RuleAttackWithWeapon>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
            IsValid = evt.Target.State.HasCondition(UnitCondition.Shaken) || evt.Target.State.HasCondition(UnitCondition.Frightened);
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
            if (!IsSuitable(evt)) {
                return;
            }
            MechanicsContext context = base.Context;
            EntityFact fact = base.Fact;

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
