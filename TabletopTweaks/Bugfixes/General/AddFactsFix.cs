using HarmonyLib;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.General {
    static class AddFactsFix {
        [HarmonyPatch(typeof(AddFacts), nameof(AddFacts.UpdateFacts))]
        static class AddFacts_UpdateFacts_CL_Patch {
            static void Postfix(AddFacts __instance) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("FixPrebuffCLs")) { return; }
                if (__instance.CasterLevel <= 0) { return; }
                __instance?.Data?.AppliedFacts?.ForEach(f => {
                    f.MaybeContext.m_Params = f?.MaybeContext?.Params?.Clone();
                });
            }
        }

        [HarmonyPatch(typeof(AddFactsToMount), nameof(AddFacts.UpdateFacts))]
        static class AddFactsToMount_UpdateFacts_CL_Patch {
            static void Postfix(AddFactsToMount __instance) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("FixPrebuffCLs")) { return; }
                if (__instance.CasterLevel <= 0) { return; }
                __instance?.Data?.AppliedFactRefs?.ForEach(id => {
                    var fact = __instance.Data?.Mount?.Facts?.FindById(id);
                    fact.MaybeContext.m_Params = fact?.MaybeContext?.Params?.Clone();
                });
            }
        }
    }
}
