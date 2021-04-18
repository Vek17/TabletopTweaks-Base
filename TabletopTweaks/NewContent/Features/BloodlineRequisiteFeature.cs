using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class BloodlineRequisiteFeature {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            static void Postfix() {
                if (Initialized) { return; }
                Initialized = true;
                AddPrimalistRagePowerSelection();
            }
            static void AddPrimalistRagePowerSelection() {
                var BloodlineRequisiteFeature = Helpers.Create<BlueprintFeature>(bp => {
                    bp.IsClassFeature = true;
                    bp.HideInUI = true;
                    bp.Ranks = 1;
                    bp.HideInCharacterSheetAndLevelUp = true;
                    bp.name = "BloodlineRequisiteFeature";
                    bp.SetName("Bloodline");
                    bp.SetDescription("Bloodline Requisite Feature");
                });
                Resources.AddBlueprint(BloodlineRequisiteFeature, Settings.Blueprints.NewBlueprints["BloodlineRequisiteFeature"]);
            }
        }
    }
}
