using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Slayer {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Slayer.DisableAllFixes) { return; }
                Main.LogHeader("Patching Slayer");
                PatchBaseClass();
            }
            static void PatchBaseClass() {
                if (ModSettings.Fixes.Slayer.Base.DisableAllFixes) { return; }
                PatchSlayerStudiedTarget();
            }
            static void PatchSlayerStudiedTarget() {
                if (!ModSettings.Fixes.Slayer.Base.Fixes["StudiedTarget"]) { return; }
                BlueprintBuff SlayerStudiedTargetBuff = Resources.GetBlueprint<BlueprintBuff>("45548967b714e254aa83f23354f174b0");
                SlayerStudiedTargetBuff.GetComponent<ContextRankConfig>().m_Progression = ContextRankProgression.OnePlusDivStep;
                Main.LogPatch("Patched", SlayerStudiedTargetBuff);
            }
        }
    }
}
