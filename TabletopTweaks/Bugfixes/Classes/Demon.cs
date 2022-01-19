using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;
namespace TabletopTweaks.Bugfixes.Classes {
    static class Demon {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Demon Resources");

                PatchBalorVorpalStrike();

                void PatchBalorVorpalStrike() {
                    if (ModSettings.Fixes.Demon.IsDisabled("BalorVorpalStrike")) { return; }
                    var BalorVorpalStrikeFeature = Resources.GetBlueprint<BlueprintFeature>("acc4a16c4088f2546b4237dcbb774f14");
                    var BalorVorpalStrikeBuff = Resources.GetBlueprint<BlueprintBuff>("5220bc4386bf3e147b1beb93b0b8b5e7");

                    BalorVorpalStrikeFeature.AddComponent<RecalculateOnEquipmentChange>();
                    BalorVorpalStrikeBuff.GetComponent<BuffEnchantWornItem>().AllWeapons = true;

                    Main.LogPatch("Patched", BalorVorpalStrikeFeature);
                    Main.LogPatch("Patched", BalorVorpalStrikeBuff);
                }
            }
        }
    }
}
