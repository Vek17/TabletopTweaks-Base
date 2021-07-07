using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using System;

namespace TabletopTweaks.Bugfixes.General {
    class TemporaryHitPointDisplayFix {
        [HarmonyPatch(typeof(ModifiableValueTemporaryHitPoints), "HandleDamage", new Type[] { typeof(int) })]
        static class ModifiableValueTemporaryHitPoints_HandleDamage_Patch {
            static void Postfix(ModifiableValueTemporaryHitPoints __instance) {
                __instance.UpdateValue();
            }
        }
    }
}