using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Aeon {

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Aeon Resources");

                PatchAeonDemythication();

                void PatchAeonDemythication() {
                    if (ModSettings.Fixes.Aeon.IsDisabled("AeonDemythication")) { return; }
                    var AeonDemythicationBuff = Resources.GetBlueprint<BlueprintBuff>("3c8a543e5b4e7154bb2cbe4d102a1604");
                    QuickFixTools.ReplaceSuppression(AeonDemythicationBuff);
                }
            }
        }
    }
}
