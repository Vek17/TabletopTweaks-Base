using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedWeaponTrainings {
    class AdvancedWeapontrainingSelection {
        public static void AddAdvancedWeaponTrainingSelection() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var AdvancedWeaponTraining1 = Resources.GetBlueprint<BlueprintFeatureSelection>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
            var AdvancedWeapontrainingSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("AdvancedWeaponTrainingSelection", bp => {
                bp.SetName("Advanced Weapon Training");
                bp.SetDescriptionTagged("Highly skilled and experienced fighters can gain advanced weapon training, learning techniques " +
                    "and applications of the weapon training class feature that give them special benefits in exchange for specializing " +
                    "in a smaller number of fighter weapon groups.\nBeginning at 9th level, instead of selecting an additional fighter weapon" +
                    " group, a fighter can choose an advanced weapon training option for one fighter weapon group that he previously selected with the weapon training class feature.");

                bp.m_AllFeatures = AdvancedWeaponTraining1.m_AllFeatures;
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;
                bp.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 9;
                }));
            });
        }
        public static void AddToAdvancedWeaponTrainingSelection(params BlueprintFeature[] features) {
            var AdvancedWeaponTraining1 = Resources.GetBlueprint<BlueprintFeatureSelection>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
            var AdvancedWeaponTraining2 = Resources.GetBlueprint<BlueprintFeatureSelection>("70a139f0a4c6c534eaa34feea0d08622");
            var AdvancedWeaponTraining3 = Resources.GetBlueprint<BlueprintFeatureSelection>("ee9ab0117ca06b84f9c66469f4428c61");
            var AdvancedWeaponTraining4 = Resources.GetBlueprint<BlueprintFeatureSelection>("0b55d725ded1ae549bb858fba1d84114");
            var WeaponTrainingSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");
            var AdvancedWeapontrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedWeaponTrainingSelection");

            AdvancedWeaponTraining1.AddFeatures(features);
            AdvancedWeaponTraining2.AddFeatures(features);
            AdvancedWeaponTraining3.AddFeatures(features);
            AdvancedWeaponTraining4.AddFeatures(features);
            WeaponTrainingSelection.AddFeatures(features);
            AdvancedWeapontrainingSelection.AddFeatures(features);
        }
    }
}
