using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using System;

namespace TabletopTweaks.MechanicsChanges {
    class TemporaryHitPointDisplayFix {
        [HarmonyPatch(typeof(ModifiableValueTemporaryHitPoints), "HandleDamage", new Type[] { typeof(int) })]
        static class ActivatableAbility_HandleUnitLeaveCombat_Patch {
            static void Postfix(ModifiableValueTemporaryHitPoints __instance) {
                __instance.UpdateValue();
            }
        }
    }
}