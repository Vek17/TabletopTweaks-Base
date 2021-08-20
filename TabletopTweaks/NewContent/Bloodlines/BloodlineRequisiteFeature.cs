using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Bloodlines {
    static class BloodlineRequisiteFeature {
        public static void AddBloodlineRequisiteFeature() {
            var BloodlineRequisiteFeature = Helpers.CreateBlueprint<BlueprintFeature>("BloodlineRequisiteFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName("Bloodline");
                bp.SetDescription("Bloodline Requisite Feature");
            });
        }
    }
}