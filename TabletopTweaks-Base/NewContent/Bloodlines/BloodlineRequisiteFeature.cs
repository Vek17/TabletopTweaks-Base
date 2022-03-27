using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Bloodlines {
    static class BloodlineRequisiteFeature {
        public static void AddBloodlineRequisiteFeature() {
            var BloodlineRequisiteFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodlineRequisiteFeature", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(TTTContext, "Bloodline");
                bp.SetDescription(TTTContext, "Bloodline Requisite Feature");
            });
        }
    }
}