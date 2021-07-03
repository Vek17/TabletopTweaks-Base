using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Aeon {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Aeon.DisableAll) { return; }
                Main.LogHeader("Patching Aeon Resources");
                PatchAeonBaneUses();
            }
            static void PatchAeonBaneUses() {
                if (!ModSettings.Fixes.Aeon.Enabled["AeonBaneUses"]) { return; }
                var AeonClass = Resources.GetBlueprint<BlueprintCharacterClass>("15a85e67b7d69554cab9ed5830d0268e");
                var AeonBaneFeature = Resources.GetBlueprint<BlueprintFeature>("0b25e8d8b0488c84c9b5714e9ca0a204");
                var AeonBaneIncreaseResourceFeature = Resources.GetBlueprint<BlueprintFeature>(ModSettings.Blueprints.GetGUID("AeonBaneIncreaseResourceFeature"));
                AeonBaneFeature.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                    c.m_Feature = AeonBaneIncreaseResourceFeature.ToReference<BlueprintFeatureReference>();
                }));
                Main.LogPatch("Patched", AeonBaneFeature);
            }
        }
    }
}
