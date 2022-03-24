using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.FighterAdvancedWeaponTrainings {
    class TrainedGrace {
        public static void AddTrainedGrace() {
            var FighterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var WeaponFinesse = BlueprintTools.GetBlueprint<BlueprintFeature>("90e54424d682d104ab36436bd527af09");
            var WeaponTrainingSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");
            var AdvancedWeaponTraining1 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
            var AdvancedWeaponTraining2 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("70a139f0a4c6c534eaa34feea0d08622");
            var AdvancedWeaponTraining3 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ee9ab0117ca06b84f9c66469f4428c61");
            var AdvancedWeaponTraining4 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("0b55d725ded1ae549bb858fba1d84114");

            var TrainedGraceFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TrainedGraceFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WeaponTraining };
                bp.SetName(TTTContext, "Trained Grace");
                bp.SetDescription(TTTContext, "When the fighter uses Weapon Finesse to make a melee attack with a weapon, using his Dexterity modifier on " +
                    "attack rolls and his Strength modifier on damage rolls, he doubles his weapon training bonus on damage rolls. The fighter " +
                    "must have Weapon Finesse in order to choose this option.");
                bp.AddComponent(Helpers.Create<TrainedGraceComponent>(c => {
                    c.MeleeOnly = true;
                }));
                bp.AddPrerequisites(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 9;
                }));
                bp.AddPrerequisites(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = WeaponFinesse.ToReference<BlueprintFeatureReference>();
                }));
            });
            if (TTTContext.AddedContent.FighterAdvancedWeaponTraining.IsDisabled("TrainedGrace")) { return; }
            AdvancedWeapontrainingSelection.AddToAdvancedWeaponTrainingSelection(TrainedGraceFeature);
        }
    }
}
