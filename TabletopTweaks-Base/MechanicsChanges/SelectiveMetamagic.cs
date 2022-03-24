using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.MechanicsChanges {
    internal static class SelectiveMetamagic {
        [HarmonyPatch(typeof(AreaEffectEntityData), "CheckSelective")]
        class UnitDescriptor_FixSizeModifiers_Patch {
            static void Postfix(AreaEffectEntityData __instance) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("SelectiveMetamagicNonInstantaneous")) { return; }
                __instance.m_CanAffectAllies = true;
            }
        }
    }
}
