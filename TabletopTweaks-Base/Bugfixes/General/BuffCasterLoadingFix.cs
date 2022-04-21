using HarmonyLib;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.Base.Bugfixes.General {
    internal class BuffCasterLoadingFix {
        [HarmonyPatch(typeof(MechanicsContext), nameof(MechanicsContext.OnDeserialized))]
        static class MechanicsContext_OnDeserialized_Patch {
            static bool Prefix(MechanicsContext __instance) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("FixBuffCasterOnSaveLoad")) { return true; }
                if (__instance.m_CasterRef.IsEmpty) {
                    __instance.m_CasterRef = __instance.m_OwnerRef;
                }
                return false;
            }
        }
    }
}
