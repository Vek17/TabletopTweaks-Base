using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("a945c1d2b2d44247bd37d651665d4f54")]
    class FocusedWeaponComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public override void OnTurnOn() {
            WeaponCategory? category = base.Param?.WeaponCategory ?? Category;
            base.Owner.Ensure<UnitPartFocusedWeapon>().AddEntry(category, base.Fact);
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartFocusedWeapon>().RemoveEntry(base.Fact);
        }

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            if (!base.Owner.Buffs.HasFact(ToggleBuff)) { return; }
            if (IsValidWeapon(evt.Weapon)) {
                var classLevel = this.Owner.Progression.GetClassLevel(CheckedClass);
                DiceFormula? formula = classLevel switch {
                    >= 1 and < 6 => new DiceFormula(1, DiceType.D6),
                    >= 6 and < 10 => new DiceFormula(1, DiceType.D8),
                    >= 10 and < 15 => new DiceFormula(1, DiceType.D10),
                    >= 15 and < 20 => new DiceFormula(2, DiceType.D6),
                    20 => new DiceFormula(2, DiceType.D8),
                    _ => null
                };
                evt.WeaponDamageDiceOverride = formula;
            }
        }

        public bool HasWeaponTraining(ItemEntityWeapon weapon) {
            var weaponTaining = this.Owner.Get<UnitPartWeaponTraining>();
            return (weaponTaining?.GetWeaponRank(weapon) > 0);
        }

        public bool IsValidWeapon(ItemEntityWeapon weapon) {
            var focusedWeaponPart = base.Owner.Ensure<UnitPartFocusedWeapon>();
            return HasWeaponTraining(weapon) && focusedWeaponPart.HasEntry(weapon.Blueprint.Category);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public BlueprintBuffReference ToggleBuff;
        public BlueprintCharacterClassReference CheckedClass;
        public WeaponCategory? Category;
    }
}
