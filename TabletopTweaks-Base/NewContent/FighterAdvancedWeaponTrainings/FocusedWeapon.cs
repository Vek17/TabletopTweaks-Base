using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.FighterAdvancedWeaponTrainings {
    class FocusedWeapon {
        public static void AddFocusedWeapon() {
            var FighterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var FighterWeaponTrainingProperty = BlueprintTools.GetModBlueprintReference<BlueprintUnitPropertyReference>(TTTContext, "FighterWeaponTrainingProperty");
            var WeaponFinesse = BlueprintTools.GetBlueprint<BlueprintFeature>("90e54424d682d104ab36436bd527af09");
            var WeaponFocus = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");

            var FocusedWeaponBuff = Helpers.CreateBuff(TTTContext, "FocusedWeaponBuff", bp => {
                bp.SetName(TTTContext, "Focused Weapon");
                bp.SetDescription(TTTContext, "The fighter selects one weapon for which he has Weapon Focus and that belongs to the associated fighter weapon group. " +
                    "The fighter can deal damage with this weapon based on the damage of the warpriest’s sacred weapon class feature, treating his fighter" +
                    " level as his warpriest level. The fighter must have Weapon Focus and Weapon Training with the selected weapon in order to choose this option.\n" +
                    "A focused weapon deals 1d6 damage, this increases to 1d8 at level 6, 1d10 at level 10, 2d6 at level 15, and 2d8 at level 20");
                bp.m_Icon = WeaponFinesse.Icon;
                bp.AddComponent<FocusedWeaponDamageComponent>(c => {
                    c.FighterWeaponTrainingProperty = FighterWeaponTrainingProperty;
                });
            });
            var FocusedWeaponToggleAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "FocusedWeaponToggleAbility", bp => {
                bp.SetName(TTTContext, "Focused Weapon");
                bp.SetDescription(FocusedWeaponBuff.m_Description);
                bp.m_Buff = FocusedWeaponBuff.ToReference<BlueprintBuffReference>();
                bp.m_Icon = WeaponFinesse.Icon;
                bp.IsOnByDefault = true;
                bp.DoNotTurnOffOnRest = true;
                bp.DeactivateImmediately = true;
            });
            var FocusedWeaponFeature = Helpers.CreateBlueprint<BlueprintParametrizedFeature>(TTTContext, "FocusedWeaponFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WeaponTraining };
                bp.SetName(TTTContext, "Focused Weapon");
                bp.SetDescription(FocusedWeaponBuff.m_Description);
                bp.ParameterType = FeatureParameterType.WeaponCategory;
                bp.m_Prerequisite = WeaponFocus.ToReference<BlueprintParametrizedFeatureReference>();
                bp.AddComponent(Helpers.Create<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        FocusedWeaponToggleAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                }));
                bp.AddPrerequisites(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 9;
                }));
                bp.AddComponent<FocusedWeaponComponent>();
            });

            if (TTTContext.AddedContent.FighterAdvancedWeaponTraining.IsDisabled("FocusedWeapon")) { return; }
            AdvancedWeapontrainingSelection.AddToAdvancedWeaponTrainingSelection(FocusedWeaponFeature);
        }
    }
}
