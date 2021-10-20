using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("a945c1d2b2d44247bd37d651665d4f54")]
    class FocusedWeaponDamageComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            Main.Log("FocusedWeaponComponent::OnEventAboutToTrigger");
            //if (!base.Owner.Buffs.HasFact(ToggleBuff)) { return; }
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
                Main.Log($"classLevel: {classLevel}");
                Main.Log($"formula: {formula?.Rolls}{formula?.Dice}");
                evt.WeaponDamageDiceOverride = formula;
            }
        }

        public bool HasWeaponTraining(ItemEntityWeapon weapon) {
            var weaponTaining = this.Owner.Get<UnitPartWeaponTraining>();
            Main.Log($"Weapon Training Rank: {weaponTaining?.GetWeaponRank(weapon)}");
            return (weaponTaining?.GetWeaponRank(weapon) > 0);
        }

        public bool IsValidWeapon(ItemEntityWeapon weapon) {
            var focusedWeaponPart = base.Owner.Ensure<UnitPartFocusedWeapon>();
            Main.Log($"Is Focused Weapon: {weapon.Blueprint.Name} - {focusedWeaponPart.HasEntry(weapon.Blueprint.Category)}");
            return HasWeaponTraining(weapon) && focusedWeaponPart.HasEntry(weapon.Blueprint.Category);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public BlueprintCharacterClassReference CheckedClass;
    }
}
