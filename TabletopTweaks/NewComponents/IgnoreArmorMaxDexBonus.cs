using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;

namespace TabletopTweaks.NewComponents {
    [TypeId("0542dd3cbb5949a7b120f2165758db9b")]
    class IgnoreArmorMaxDexBonus: UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateArmorMaxDexBonusLimit>,
        IRulebookHandler<RuleCalculateArmorMaxDexBonusLimit>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public override void OnTurnOn() {
            base.OnTurnOn();
            if (Owner.Body.Armor.HasArmor && Owner.Body.Armor.Armor.Blueprint.IsArmor) {
                Owner.Body.Armor.Armor.RecalculateStats();
                Owner.Body.Armor.Armor.RecalculateMaxDexBonus();
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateArmorMaxDexBonusLimit evt) {
        }

        public void OnEventDidTrigger(RuleCalculateArmorMaxDexBonusLimit evt) {
            if (!evt.Armor.Blueprint.IsShield && (CheckCategory && evt.Armor.ArmorType() == Category)) {
                evt.Result = null;
            }
        }

        public bool CheckCategory;

        [ShowIf("CheckCategory")]
        public ArmorProficiencyGroup Category;
    }
}
