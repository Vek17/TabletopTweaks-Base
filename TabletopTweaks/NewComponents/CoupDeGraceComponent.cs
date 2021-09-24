using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items.Slots;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.NewComponents {
    [TypeId("5da9ff47fe484e0faadd0330ec998a95")]
    [AllowedOn(typeof(BlueprintAbility))]
    //This is brought to you by Perunq
    public class CoupDeGraceComponent : AbilityCustomLogic {
        [UsedImplicitly]
        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target) {
            UnitEntityData caster = context.MaybeCaster;
            WeaponSlot threatHand = caster.GetThreatHand();
            UnitEntityData targetUnit = target.Unit;
            if (caster == null) { yield break; }
            if (threatHand == null) { yield break; }
            if (targetUnit == null) { yield break; }
            RuleAttackWithWeapon weaponAttack = new RuleAttackWithWeapon(caster, targetUnit, threatHand.Weapon, 0) {
                AutoHit = true,
                AutoCriticalConfirmation = true,
                AutoCriticalThreat = true
            };
            context.TriggerRule(weaponAttack);
            if (!weaponAttack.AttackRoll.IsCriticalRoll) { yield break; }
            var damage = weaponAttack.MeleeDamage?.Result ?? 0;
            RuleSavingThrow coupSave = new RuleSavingThrow(targetUnit, SavingThrowType.Fortitude, damage + 10);
            context.TriggerRule(coupSave);
            if (!coupSave.IsPassed) {
                targetUnit.Descriptor.State.MarkedForDeath = true;
            }
            using (context.GetDataScope(target)) {
                Actions.Run();
            }
            yield return new AbilityDeliveryTarget(target);
        }

        public override void Cleanup(AbilityExecutionContext context) {
        }

        public ActionList Actions;
    }
}
