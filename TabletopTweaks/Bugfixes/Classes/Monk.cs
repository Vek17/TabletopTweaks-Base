using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Monk {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Monk.DisableAllFixes) { return; }
                Main.LogHeader("Patching Monk");
                PatchBase();
            }
            static void PatchBase() {
                if (ModSettings.Fixes.Monk.Base.DisableAllFixes) { return; }
            }
        }
    }
}
