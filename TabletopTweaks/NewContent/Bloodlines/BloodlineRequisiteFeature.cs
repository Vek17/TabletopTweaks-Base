using Kingmaker.Blueprints.Classes;
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
                bp.SetName("13594ac8a2d44700ba6fda2bda841150", "Bloodline");
                bp.SetDescription("b02942bfd62c48bd8d6a306c2d9c2b8e", "Bloodline Requisite Feature");
            });
        }
    }
}