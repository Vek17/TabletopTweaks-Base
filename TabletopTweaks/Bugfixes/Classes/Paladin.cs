using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Paladin {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Paladin");
                PatchBase();
            }
            static void PatchBase() {
                PatchDivineMount();

                void PatchDivineMount() {
                    if (ModSettings.Fixes.Paladin.Base.IsDisabled("DivineMountTemplate")) { return; }

                    var TemplateCelestial = Resources.GetModBlueprint<BlueprintFeature>("TemplateCelestial");
                    var PaladinDivineMount11Feature = Resources.GetBlueprint<BlueprintFeature>("ea31185f4e0f91041bf766d67214182f");
                    var addFeatureToPet = PaladinDivineMount11Feature.Components.OfType<AddFeatureToPet>().FirstOrDefault();
                    if (addFeatureToPet != null) {
                        addFeatureToPet.m_Feature = TemplateCelestial.ToReference<BlueprintFeatureReference>();
                    }
                }
            }

            static void PatchArchetypes() {
            }
        }
    }
}
