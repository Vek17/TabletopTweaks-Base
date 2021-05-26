using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    [TypeId("f361784981e6444d84312f063c506e76")]
    class IgnoreArmorCheckPenalty: UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateArmorCheckPenalty>,
        IRulebookHandler<RuleCalculateArmorCheckPenalty>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public override void OnTurnOn() {
            base.OnTurnOn();
            if (Owner.Body.Armor.HasArmor && Owner.Body.Armor.Armor.Blueprint.IsArmor) {
                Owner.Body.Armor.Armor.RecalculateStats();
                Owner.Body.Armor.Armor.RecalculateMaxDexBonus();
                if (Owner.Body.SecondaryHand.HasShield) {
                    Owner.Body.SecondaryHand.MaybeShield.ArmorComponent.RecalculateStats();
                    Owner.Body.SecondaryHand.MaybeShield.ArmorComponent.RecalculateMaxDexBonus();
                }

            }
        }

        public void OnEventAboutToTrigger(RuleCalculateArmorCheckPenalty evt) {
        }

        public void OnEventDidTrigger(RuleCalculateArmorCheckPenalty evt) {
            if (!CheckCategory) {
                evt.Result = 0;
                return;
            }
            if (!evt.Armor.Blueprint.IsShield && CheckCategory && Categorys.Contains(evt.Armor.ArmorType())) {
                evt.Result = 0;
                return;
            }
            if (evt.Armor.Blueprint.IsShield && CheckCategory && Categorys.Contains(evt.Armor.Blueprint.ProficiencyGroup)) {
                evt.Result = 0;
            }

        }

        public bool CheckCategory = true;
        [ShowIf("CheckCategory")]
        public ArmorProficiencyGroup[] Categorys;
    }
}
