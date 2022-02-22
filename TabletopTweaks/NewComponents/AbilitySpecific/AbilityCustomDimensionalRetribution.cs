using System;
using System.Collections.Generic;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("4b22554ea5ff4068ac8ef350e766bc8e")]
    public class AbilityCustomDimensionalRetribution : AbilityCustomDimensionDoor {
        public override bool LookAtTarget {
            get {
                return true;
            }
        }

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target) {
            UnitEntityData caster = context.Caster;
            if (target.Unit == null) {
                PFLog.Default.Error("Target unit is mising", Array.Empty<object>());
                yield break;
            }
            bool success = false;
            IEnumerator<AbilityDeliveryTarget> dimmensionDoor = base.Deliver(context, target);
            while (dimmensionDoor.MoveNext()) {
                AbilityDeliveryTarget abilityDeliveryTarget = dimmensionDoor.Current;
                success = ((abilityDeliveryTarget != null) ? abilityDeliveryTarget.Target : null) == target;
                yield return null;
            }
            if (success) {
                UnitAttackOfOpportunity unitAttack = new UnitAttackOfOpportunity(target.Unit, false);
                unitAttack.IgnoreCooldown(null);
                caster.Commands.Run(unitAttack);
                context.Caster.CombatState.AttackOfOpportunityCount -= 1;
            }
            yield break;
        }
    }
}
