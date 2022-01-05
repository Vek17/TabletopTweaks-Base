using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("4a2247bdf0cf4b139863f0136abd4af8")]
    class TitanStrikeComponent : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>, 
        IRulebookHandler<RuleCalculateWeaponStats>,
        IInitiatorRulebookHandler<RuleCalculateCMB>, 
        IRulebookHandler<RuleCalculateCMB>,
        IGlobalRulebookHandler<RuleSavingThrow>, 
        IRulebookHandler<RuleSavingThrow>,
        ISubscriber, IInitiatorRulebookSubscriber, IGlobalSubscriber {

        public BlueprintFact StunningFist {
            get {
                if (m_StunningFist == null) {
                    return null;
                }
                return m_StunningFist.Get();
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (evt.Weapon.Blueprint.Type.IsUnarmed) {
                evt.IncreaseWeaponSize(1);
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateCMB evt) {
            int bonus = (int)evt?.Target?.State?.Size - (int)evt?.Initiator?.State?.Size;
            if (bonus > 0 && (
                evt.Type == CombatManeuver.BullRush
                || evt.Type == CombatManeuver.Pull
                || evt.Type == CombatManeuver.Grapple
                || evt.Type == CombatManeuver.Overrun
                || evt.Type == CombatManeuver.SunderArmor
                || evt.Type == CombatManeuver.Trip
            )) {
                evt.AddModifier(new Modifier(bonus, this.Fact, ModifierDescriptor.UntypedStackable));
            }
        }

        public void OnEventAboutToTrigger(RuleSavingThrow evt) {
            MechanicsContext context = evt.Reason.Context;
            if (context?.MaybeCaster != base.Owner && evt.Reason?.Fact?.Blueprint != StunningFist) { return; }
            int bonus = (int)evt.Initiator?.State?.Size - (int)evt.Reason?.Caster?.State?.Size;
            if (bonus > 0) {
                evt.AddBonusDC(bonus);
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public void OnEventDidTrigger(RuleCalculateCMB evt) {
        }

        public void OnEventDidTrigger(RuleSavingThrow evt) {
        }

        public BlueprintBuffReference m_StunningFist;
    }
}
