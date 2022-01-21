using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using static Kingmaker.UI.GenericSlot.EquipSlotBase;

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
                    var Vorpal = Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("2f60bfcba52e48a479e4a69868e24ebc");

                    BalorVorpalStrikeBuff.SetComponents();
                    BalorVorpalStrikeBuff.AddComponent<BuffEnchantWornItem>(c => {
                        c.m_EnchantmentBlueprint = Vorpal;
                        c.Slot = SlotType.PrimaryHand;
                    });
                    BalorVorpalStrikeBuff.AddComponent<BuffEnchantWornItem>(c => {
                        c.m_EnchantmentBlueprint = Vorpal;
                        c.Slot = SlotType.SecondaryHand;
                    });
                    BalorVorpalStrikeFeature.AddComponent<RecalculateOnEquipmentChange>();

                    Main.LogPatch("Patched", BalorVorpalStrikeFeature);
                    Main.LogPatch("Patched", BalorVorpalStrikeBuff);
                }
            }
        }
    }
}
