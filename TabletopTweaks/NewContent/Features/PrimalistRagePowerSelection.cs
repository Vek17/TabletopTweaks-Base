using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class PrimalistRagePowerSelection {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            [HarmonyPriority(Priority.First)]
            static void Postfix() {
                AddPrimalistRagePowerSelection();
            }
            static void AddPrimalistRagePowerSelection() {
                var RagePowerSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("28710502f46848d48b3f0d6132817c4e");
                var PrimalistRagePowerSelection = Helpers.CreateCopy<BlueprintFeatureSelection>(RagePowerSelection, bp => {
                    bp.name = "PrimalistRagePowerSelection";
                    bp.SetDescription("At 4th level and every 4 levels thereafter, a primalist can choose to take either his bloodline power "
                    + "or two barbarian rage powers. If the primalist chooses rage powers, those rage powers can be used in conjunction with "
                    + "his bloodrage, and his bloodrager level acts as his barbarian level when determining the effect of those bloodrage powers "
                    + "and any prerequisites. Any other prerequisites for a rage power must be met before a primalist can choose it. This ability "
                    + "does not count as the rage power class feature for determining feat prerequisites and other requirements.");
                    bp.SetName("Primalist Rage Power");
                });
                Resources.AddBlueprint(PrimalistRagePowerSelection, Settings.Blueprints.NewBlueprints["PrimalistRagePowerSelection"]);
            }
        }
    }
}
