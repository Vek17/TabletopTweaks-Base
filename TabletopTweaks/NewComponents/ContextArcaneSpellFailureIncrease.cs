using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("83ce1f63bacb4494887fb0b2080eddcd")]
    class ContextArcaneSpellFailureIncrease : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateArcaneSpellFailureChance>,
        IRulebookHandler<RuleCalculateArcaneSpellFailureChance>,
        ISubscriber, IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateArcaneSpellFailureChance evt) {
            if (!evt.Armor.Blueprint.IsShield && (!CheckCategory || Categorys.Contains(evt.Armor.ArmorType()))) {
                evt.AddArmorBonus(Reduce ? -Value.Calculate(base.Context) : Value.Calculate(base.Context));
            }
            if (evt.Armor.Blueprint.IsShield && (!CheckCategory || Categorys.Contains(evt.Armor.Blueprint.ProficiencyGroup))) {
                evt.AddShieldBonus(Reduce ? -Value.Calculate(base.Context) : Value.Calculate(base.Context));
            }
        }

        public void OnEventDidTrigger(RuleCalculateArcaneSpellFailureChance evt) {
        }

        public bool Reduce;
        public ContextValue Value;
        public bool CheckCategory;
        public ArmorProficiencyGroup[] Categorys;
    }
}
