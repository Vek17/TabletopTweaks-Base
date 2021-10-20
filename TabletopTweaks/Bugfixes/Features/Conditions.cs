using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Features {
    class Conditions {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Conditions");
                PatchStaggered();

                static void PatchStaggered() {
                    if (ModSettings.Fixes.BaseFixes.IsDisabled("StaggeredDescriptors")) { return; }
                    var Staggered = Resources.GetBlueprint<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3");
                    Staggered.GetComponent<SpellDescriptorComponent>().Descriptor = SpellDescriptor.Staggered;
                    Main.LogPatch("Patched", Staggered);
                }
            }
        }
    }
}
