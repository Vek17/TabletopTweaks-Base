using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.NewContent.Custom
{
    public class AbilityDeliverVitalStrikeWithWeapon : AbilityCustomLogic
    {
        public override bool IsEngageUnit => true;
        public int VitalStrikeMod;
        public BlueprintFeatureReference m_RowdyFeature;
        public BlueprintFeatureReference m_MythicBlueprint;
        public BlueprintFeature MythicBlueprint => this.m_MythicBlueprint?.Get();
        public BlueprintFeature RowdyFeature => this.m_RowdyFeature?.Get();

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
        {
            if (target.Unit == null)
            {
                PFLog.Default.Error("Target unit is missing!");
            }
            else
            {
                var caster = context.MaybeCaster;
                var weapon = caster.GetFirstWeapon();
                if (weapon == null)
                {
                    PFLog.Default.Error("Has no weapon for attack!");
                }
                else if (caster == null)
                {
                    PFLog.Default.Error("Caster can't attack!");
                }
                else
                {
                    var eventHandlers = new AbilityCustomMeleeAttack.EventHandlers();
                    eventHandlers.Add(
                        new AbilityCustomMeleeAttack.VitalStrike(
                            caster,
                            VitalStrikeMod,
                            caster.HasFact(this.MythicBlueprint),
                            caster.HasFact(this.RowdyFeature)
                        )
                    );
                    var abilityDeliveryTarget = new AbilityDeliveryTarget(target);
                    var rule = new RuleAttackWithWeapon(caster, target.Unit, weapon, 0);
                    abilityDeliveryTarget.AttackRoll = rule.AttackRoll;
                    using (eventHandlers.Activate())
                    {
                        context.TriggerRule<RuleAttackWithWeapon>(rule);
                    }
                    yield return abilityDeliveryTarget;
                }
            }
        }

        public override void Cleanup(AbilityExecutionContext context)
        {
        }
    }
}
