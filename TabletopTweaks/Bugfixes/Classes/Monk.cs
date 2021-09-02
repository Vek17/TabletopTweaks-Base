using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;

namespace TabletopTweaks.Bugfixes.Classes {
    class Monk {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Monk");

                PatchBase();
            }
            static void PatchBase() {
            }
        }
    }
}
