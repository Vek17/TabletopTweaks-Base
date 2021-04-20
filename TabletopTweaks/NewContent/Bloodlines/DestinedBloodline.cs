using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Bloodlines {
    class DestinedBloodline {

        public static BlueprintFeatureReference CreateBloodlineRequisiteFeature() {
            var AberrantBloodlineRequisiteFeature = Helpers.Create<BlueprintFeature>(bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.name = "DestinedBloodlineRequisiteFeature";
                bp.SetName("Destined Bloodline");
                bp.SetDescription("Destined Bloodline Requisite Feature");
            });
            Resources.AddBlueprint(AberrantBloodlineRequisiteFeature, Settings.Blueprints.NewBlueprints["DestinedBloodlineRequisiteFeature"]);
            return AberrantBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
        }
        public static void AddBloodragerDestinedBloodline() {
            var BloodragerDestinedBloodline = Helpers.Create<BlueprintProgression>(bp => {
            });
        }
        public static void AddSorcererDestinedBloodline() {
            var SorcererDestinedBloodline = Helpers.Create<BlueprintProgression>(bp => {
            });
        }
    }
}
