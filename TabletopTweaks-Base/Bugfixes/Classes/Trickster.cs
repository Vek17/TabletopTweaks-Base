using HarmonyLib;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using System;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Trickster {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                //TTTContext.Logger.LogHeader("Patching Trickster Resources");
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
