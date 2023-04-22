using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs;

namespace TabletopTweaks.Base.Bugfixes.UI {
    internal class BuffInspectorView {
        [HarmonyPatch(typeof(Buff), nameof(Buff.HiddenInInspector), MethodType.Getter)]
        static class Buff_HiddenInInspector_Patch {
            static void Postfix(Buff __instance, ref bool __result) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("BuffInspector")) { return; }
                __result = __instance.Blueprint.IsHiddenInUI || __instance.Blueprint.GetComponent<HideFeatureInInspect>();
            }
        }
    }
}
