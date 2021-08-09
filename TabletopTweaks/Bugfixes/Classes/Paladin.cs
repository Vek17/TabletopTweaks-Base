using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Paladin {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Paladin.DisableAll) { return; }
                Main.LogHeader("Patching Paladin");
            }
            static void PatchBase() {
            }
            static void PatchArchetypes() {
            }
        }
    }
}
