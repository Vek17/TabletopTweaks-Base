using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Barbarian {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Monk.DisableAll) { return; }
                Main.LogHeader("Patching Barbarian");
                PatchBase();
            }
            static void PatchBase() {
                if (ModSettings.Fixes.Barbarian.Base.DisableAll) { return; }
            }
        }
    }
}