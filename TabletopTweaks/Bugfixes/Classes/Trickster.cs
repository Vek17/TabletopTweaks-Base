using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Classes {
    class Trickster {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                //Main.LogHeader("Patching Trickster Resources");
            }
        }
        [HarmonyPatch(typeof(BlueprintItemEquipment), "CanBeEquippedBy", new Type[] { typeof(UnitDescriptor) })]
        static class BlueprintItemEquipment_CanBeEquippedBy_TricksterUMD2_Patch {
            static void Postfix(UnitDescriptor unit, ref bool __result) {
                if (unit.State.Features.TricksterUseMagicDeviceUnlimitedWands) { __result = true; }
            }
        }
    }
}
