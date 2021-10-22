using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("5c83ff59ace044d19376f4bbe035b544")]
    class ContextArmorCheckPenaltyIncrease : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateArmorCheckPenalty>,
        IRulebookHandler<RuleCalculateArmorCheckPenalty>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public override void OnTurnOn() {
            ItemEntityArmor maybeArmor = base.Owner.Body.Armor.MaybeArmor;
            if (maybeArmor != null) {
                maybeArmor.RecalculateStats();
            }
            ItemEntityShield maybeShield = base.Owner.Body.SecondaryHand.MaybeShield;
            if (maybeShield == null) {
                return;
            }
            maybeShield.ArmorComponent.RecalculateStats();
        }

        public void OnEventAboutToTrigger(RuleCalculateArmorCheckPenalty evt) {
            if (!evt.Armor.Blueprint.IsShield && (!CheckCategory || Categorys.Contains(evt.Armor.ArmorType()))) {
                evt.AddBonus(Value.Calculate(base.Context));
                return;
            }
            if (evt.Armor.Blueprint.IsShield && (!CheckCategory || Categorys.Contains(evt.Armor.Blueprint.ProficiencyGroup))) {
                evt.AddBonus(Value.Calculate(base.Context));
            }
        }

        public void OnEventDidTrigger(RuleCalculateArmorCheckPenalty evt) {
        }

        public bool Reduce;
        public ContextValue Value;
        public bool CheckCategory;
        public ArmorProficiencyGroup[] Categorys;
    }
}
