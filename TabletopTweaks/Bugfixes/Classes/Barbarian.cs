using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Barbarian {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Barbarian");

                PatchBase();
            }
            static void PatchBase() {
            }
        }
    }
}
