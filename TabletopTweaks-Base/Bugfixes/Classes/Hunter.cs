using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Hunter {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Hunter");
                PatchBase();
                PatchDivineHunter();
            }
            static void PatchBase() { }

            static void PatchDivineHunter() {
                if (TTTContext.Fixes.Hunter.Archetypes["DivineHunter"].IsDisabled("OtherworldlyCompanion")) { return; }

                var OtherworldlyCompanionCelestial = BlueprintTools.GetBlueprint<BlueprintFeature>("3db2fd3394613b4438d3c844a0c034ca");
                var OtherworldlyCompanionFiendish = BlueprintTools.GetBlueprint<BlueprintFeature>("4d7607a0155af7d43b49b785f2051e21");
                var TemplateCelestial = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(TTTContext, "TemplateCelestial");
                var TemplateFiendish = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(TTTContext, "TemplateFiendish");

                OtherworldlyCompanionCelestial.RemoveComponents<AddFeatureToPet>();
                OtherworldlyCompanionCelestial.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = TemplateCelestial;
                });
                OtherworldlyCompanionFiendish.RemoveComponents<AddFeatureToPet>();
                OtherworldlyCompanionFiendish.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = TemplateFiendish;
                });
                TTTContext.Logger.LogPatch("Patched", OtherworldlyCompanionCelestial);
                TTTContext.Logger.LogPatch("Patched", OtherworldlyCompanionFiendish);
            }
        }
    }
}
