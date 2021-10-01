using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace TabletopTweaks.NewComponents {
    [TypeId("6ac52746855e4c82a1427a787bb26edb")]
    class SurviveDeathWithSave : UnitFactComponentDelegate, IDamageHandler, IGlobalSubscriber, ISubscriber {

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
            if (WouldKill(unit, unit.Damage)) { return; }
            if (!HasEnoughResource()) { return; }
            Spend();

            RuleSavingThrow ruleSavingThrow = new RuleSavingThrow(unit, Type, DC);
            BlueprintBuff buff = base.Fact.Blueprint as BlueprintBuff;
            ruleSavingThrow.Buff = buff;
            ruleSavingThrow.Reason = base.Fact;
            ruleSavingThrow = Rulebook.Trigger(ruleSavingThrow);

            if (ruleSavingThrow.IsPassed) {
                if ((unit.Damage - dealDamage.Result) > unit.Stats.HitPoints && BlockIfBelowZero) {
                    unit.Damage -= dealDamage.Result;
                } else {
                    unit.Damage = unit.Stats.HitPoints - TargetHP;
                }
            }
        }
        private bool WouldKill(UnitEntityData unit, int damage) {
            return (unit.Stats.HitPoints + (unit.Stats.Constitution * (unit.State.Features.MythicHardToKill ? 2 : 1))) > damage;
        }
        private bool HasEnoughResource() {
            return Owner.Resources.HasEnoughResource(RequiredResource, SpendAmount);
        }
        private void Spend() {
            Owner.Descriptor.Resources.Spend(RequiredResource, SpendAmount);
        }

        public int DC = 10;
        public SavingThrowType Type;
        public int TargetHP;
        public bool BlockIfBelowZero;
        public int SpendAmount;
        public BlueprintAbilityResourceReference Resource;
    }
}
