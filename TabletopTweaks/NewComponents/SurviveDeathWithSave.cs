using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace TabletopTweaks.NewComponents {
    class SurviveDeathWithSave: UnitFactComponentDelegate, IDamageHandler, IGlobalSubscriber, ISubscriber {

        public BlueprintAbilityResource RequiredResource {
            get {
                BlueprintAbilityResourceReference requiredResource = Resource;
                if (requiredResource == null) {
                    return null;
                }
                return requiredResource.Get();
            }
        }

        public void HandleDamageDealt(RuleDealDamage dealDamage) {
            var unit = dealDamage.Target;
            if (unit != base.Owner) { return; }
            if ((unit.Stats.HitPoints + (unit.Stats.Constitution * (unit.State.Features.MythicHardToKill ? 2 : 1))) > unit.Damage) { return; }
            if (!HasEnoughResource()) { return; }
            Spend();

            RuleSavingThrow ruleSavingThrow = new RuleSavingThrow(unit, Type, DC);
            BlueprintBuff buff = base.Fact.Blueprint as BlueprintBuff;
            ruleSavingThrow.Buff = buff;
            ruleSavingThrow.Reason = base.Fact;
            ruleSavingThrow = Rulebook.Trigger(ruleSavingThrow);

            if (ruleSavingThrow.IsPassed) {
                UnitState state = unit.Descriptor.State;
                unit.Damage = unit.Stats.HitPoints - ForcedHP;
            }
        }
        private bool HasEnoughResource() {
            return Owner.Resources.HasEnoughResource(RequiredResource, SpendAmount);
        }
        private void Spend() {
            Owner.Descriptor.Resources.Spend(RequiredResource, SpendAmount);
        }

        public int DC = 10;
        public SavingThrowType Type;
        public int ForcedHP;
        public int SpendAmount;
        public BlueprintAbilityResourceReference Resource;
    }
}
