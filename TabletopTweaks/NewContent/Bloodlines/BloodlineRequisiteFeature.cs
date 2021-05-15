using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Bloodlines {
    static class BloodlineRequisiteFeature {
        public static void AddBloodlineRequisiteFeature() {
            var BloodlineRequisiteFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.name = "BloodlineRequisiteFeature";
                bp.SetName("Bloodline");
                bp.SetDescription("Bloodline Requisite Feature");
            });
            Resources.AddBlueprint(BloodlineRequisiteFeature, ModSettings.Blueprints.NewBlueprints["BloodlineRequisiteFeature"]);
        }
    }
}