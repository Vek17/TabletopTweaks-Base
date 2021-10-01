using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedWeaponTrainings {
    class TrainedThrow {
        public static void AddTrainedThrow() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var WeaponTrainingSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");
            var AdvancedWeaponTraining1 = Resources.GetBlueprint<BlueprintFeatureSelection>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
            var AdvancedWeaponTraining2 = Resources.GetBlueprint<BlueprintFeatureSelection>("70a139f0a4c6c534eaa34feea0d08622");
            var AdvancedWeaponTraining3 = Resources.GetBlueprint<BlueprintFeatureSelection>("ee9ab0117ca06b84f9c66469f4428c61");
            var AdvancedWeaponTraining4 = Resources.GetBlueprint<BlueprintFeatureSelection>("0b55d725ded1ae549bb858fba1d84114");

            var TrainedThrowFeature = Helpers.CreateBlueprint<BlueprintFeature>("TrainedThrowFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WeaponTraining };
                bp.SetName("Trained Throw");
                bp.SetDescription("When the fighter makes a ranged attack with a thrown weapon and applies his Dexterity modifier" +
                    " on attack rolls and his Strength modifier on damage rolls, he doubles his weapon training bonus on damage rolls.");
                bp.AddComponent(Helpers.Create<TrainedGraceComponent>(c => {
                    c.EnforceGroup = true;
                    c.WeaponGroup = WeaponFighterGroup.Thrown;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 9;
                }));
            });
            if (ModSettings.AddedContent.FighterAdvancedWeaponTraining.IsDisabled("TrainedThrow")) { return; }
            AdvancedWeapontrainingSelection.AddToAdvancedWeaponTrainingSelection(TrainedThrowFeature);
        }
    }
}
