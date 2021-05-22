using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Kineticist {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Kineticist.DisableAll) { return; }
                Main.LogHeader("Patching Kineticist");
                PatchBase();
            }
            static void PatchBase() {
                if (ModSettings.Fixes.Kineticist.Base.DisableAll) { return; }
                var ElementalOverflowProgression = Resources.GetBlueprint<BlueprintFeatureBase>("86beb0391653faf43aec60d5ec05b538");
                ElementalOverflowProgression.HideInUI = false;
            }
        }
    }
}
