using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintBuff))]
    class RemoveBuffRankOnAttack: UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleAttackWithWeapon>,
        IRulebookHandler<RuleAttackWithWeapon>, ISubscriber,
        IInitiatorRulebookSubscriber {

        // Token: 0x0600A2AE RID: 41646 RVA: 0x000036D8 File Offset: 0x000018D8
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
        }

        // Token: 0x0600A2AF RID: 41647 RVA: 0x0027A8A4 File Offset: 0x00278AA4
        public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
            using (ContextData<Kingmaker.UnitLogic.Buffs.BuffCollection.RemoveByRank>.RequestIf(true)) {
                if (Owner != null) {
                    Owner.Buffs.RemoveFact(base.Fact);
                }
            }
        }
    }
}
