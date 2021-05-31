using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedWeaponTrainings {
    class AdvancedWeapontrainingSelection {
        public static void AddAdvancedWeaponTrainingSelection() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var AdvancedWeaponTraining1 = Resources.GetBlueprint<BlueprintFeatureSelection>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
            var AdvancedWeapontrainingSelection = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["AdvancedWeaponTrainingSelection"];
                bp.name = "AdvancedWeaponTrainingSelection1";
                bp.SetName("Advanced Weapon Training");
                bp.SetDescription("Highly skilled and experienced fighters can gain advanced weapon training, learning techniques " +
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
            Resources.AddBlueprint(AdvancedWeapontrainingSelection);
        }
    }
}
