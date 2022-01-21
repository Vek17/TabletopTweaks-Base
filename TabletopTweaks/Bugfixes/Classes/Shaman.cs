using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Shaman {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Shaman");

                PatchBaseClass();
            }
            static void PatchBaseClass() {
                PatchAmelioratingHex();

                void PatchAmelioratingHex() {
                    if (ModSettings.Fixes.Shaman.Base.IsDisabled("AmelioratingHex")) { return; }

                    var ShamanHexAmelioratingDazzleSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("ea8525d5efb6870418065c14d599c297");
                    var ShamanHexAmelioratingFatuguedSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("84660c99b75e6384b8a6b8fe34b57728");
                    var ShamanHexAmelioratingShakenSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("90295b0de24b3d2469edad595528eb08");
                    var ShamanHexAmelioratingSickenedSuppressBuff = Resources.GetBlueprint<BlueprintBuff>("1ee56c72f388a4c40bf19a53ae66b6fe");

                    QuickFixTools.ReplaceSuppression(ShamanHexAmelioratingDazzleSuppressBuff);
                    QuickFixTools.ReplaceSuppression(ShamanHexAmelioratingFatuguedSuppressBuff);
                    QuickFixTools.ReplaceSuppression(ShamanHexAmelioratingShakenSuppressBuff);
                    QuickFixTools.ReplaceSuppression(ShamanHexAmelioratingSickenedSuppressBuff);
                }
            }
        }
    }
}
