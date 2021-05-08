using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Config;
using static TabletopTweaks.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.Bugfixes.Classes {
    class Monk {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
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
