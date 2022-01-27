using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("8eb56941c0744505ae2e470528f9f1dd")]
    public class IgnoreEnergyImmunityOnTarget : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleCalculateDamage>, 
        IRulebookHandler<RuleCalculateDamage>, 
        ISubscriber, IInitiatorRulebookSubscriber { 

        public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
            bool correctAlignment = evt.Target != null && evt.Target.Descriptor.Alignment.ValueRaw.HasComponent(this.Alignment);
            if (CheckTargetAlignment && !correctAlignment) { return; }
            foreach (BaseDamage baseDamage in evt.DamageBundle) {
                EnergyDamage energyDamage = baseDamage as EnergyDamage;
                DamageEnergyType? damageEnergyType = (energyDamage != null) ? new DamageEnergyType?(energyDamage.EnergyType) : null;
                bool matchesType = damageEnergyType != null ? (AllTypes || damageEnergyType == Type) : false;
                if (matchesType) {
                    baseDamage.AddDecline(new DamageDecline(DamageDeclineType.None, this));
                    energyDamage.IgnoreImmunities = true;
                    energyDamage.IgnoreReduction = true;
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) {
        }

        public bool CheckTargetAlignment;
        public AlignmentComponent Alignment;
        public bool AllTypes;
        public DamageEnergyType Type;
    }
}
