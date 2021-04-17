using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Slayer {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;
            static bool Prefix() {
                if (Initialized) {
                    // When wrath first loads into the main menu InitializeLibrary is called by Kingmaker.GameStarter.
                    // When loading into maps, Kingmaker.Runner.Start will call InitializeLibrary which will
                    // clear the ResourcesLibrary.s_LoadedBlueprints cache which causes loaded blueprints to be garbage collected.
                    // Return false here to prevent ResourcesLibrary.InitializeLibrary from being called twice 
                    // to prevent blueprints from being garbage collected.
                    return false;
                }
                else {
                    return true;
                }
            }
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Settings.Fixes.Slayer.DisableAllFixes) { return; }
                Main.LogHeader("Patching Slayer Resources");
                PatchBaseClass();
                Main.LogHeader("Slayer Resource Patch Complete");

                //Do Stuff
            }
            static void PatchBaseClass() {
                if (Settings.Fixes.Slayer.Base.DisableAllFixes) { return; }
                PatchSlayerStudiedTarget();
            }
            static void PatchSlayerStudiedTarget() {
                if (!Settings.Fixes.Slayer.Base.Fixes["StudiedTarget"]) { return; }
                BlueprintBuff SlayerStudiedTargetBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("45548967b714e254aa83f23354f174b0");
                SlayerStudiedTargetBuff.GetComponent<ContextRankConfig>().m_Progression = ContextRankProgression.OnePlusDivStep;
                Main.LogPatch("Patched", SlayerStudiedTargetBuff);
            }
        }
    }
}
