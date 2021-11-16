using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Armies.TacticalCombat.Parts;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace TabletopTweaks.NewComponents {
    [TypeId("d6c77d2de4804f8e9aa06e01c9fb77fa")]
    class AddWeaponDamageTrigger : WeaponEnchantmentLogic,
        IInitiatorRulebookHandler<RuleDealDamage>,
        IRulebookHandler<RuleDealDamage> {

        private void RunAction(UnitEntityData target) {
            if (this.Actions.HasActions) {
                IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                if (factContextOwner == null) {
                    return;
                }
                factContextOwner.RunActionInContext(this.Actions, target);
            }
        }

        public void OnEventAboutToTrigger(RuleDealDamage evt) {
            if (evt?.AttackRoll?.Weapon != this.Owner) {
                return;
            }
            if (TacticalCombatHelper.IsActive) {
                UnitPartTacticalCombat unitPartTacticalCombat = evt.Target.Get<UnitPartTacticalCombat>();
                int num = (unitPartTacticalCombat != null) ? unitPartTacticalCombat.Count : 1;
                WasTargetAlive = num > TacticalCombatHelper.GetDeathCount(evt.Target, evt.Target.HPLeft, num);
                return;
            }
            WasTargetAlive = !evt.Target.Descriptor.State.IsDead;
        }

        public void OnEventDidTrigger(RuleDealDamage evt) {
            if (evt?.AttackRoll?.Weapon != this.Owner) {
                return;
            }
            this.Apply(evt);
        }

        private void Apply(RuleDealDamage evt) {
            bool IsDead = (evt.Target.Descriptor.Stats.HitPoints <= evt.Target.Descriptor.Damage);
            if (this.TargetKilledByThisDamage && (!WasTargetAlive || !IsDead)) {
                return;
            }
            this.RunAction(CastOnSelf ? evt.Initiator : evt.Target);
        }

        private bool WasTargetAlive;
        public bool TargetKilledByThisDamage;
        public bool CastOnSelf;
        public ActionList Actions;
    }
}
