using HarmonyLib;
using Kingmaker.UnitLogic.FactLogic;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.General {
    static class AddFactsFix {
        [HarmonyPatch(typeof(AddFacts), nameof(AddFacts.UpdateFacts))]
        static class AddFacts_UpdateFacts_CL_Patch {
            static void Postfix(AddFacts __instance) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixPrebuffCasterLevels")) { return; }
                if (__instance.CasterLevel <= 0) { return; }
                __instance?.Data?.AppliedFacts?.ForEach(fact => {
                    if (fact?.MaybeContext != null) {
                        fact.MaybeContext.m_Params = fact?.MaybeContext?.Params?.Clone();
                    }
                });
            }
        }

        [HarmonyPatch(typeof(AddFactsToMount), nameof(AddFacts.UpdateFacts))]
        static class AddFactsToMount_UpdateFacts_CL_Patch {
            static void Postfix(AddFactsToMount __instance) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixPrebuffCasterLevels")) { return; }
                if (__instance.CasterLevel <= 0) { return; }
                __instance?.Data?.AppliedFactRefs?.ForEach(id => {
                    var fact = __instance.Data?.Mount?.Facts?.FindById(id);
                    var m_Params = fact?.MaybeContext?.m_Params;
                    if (fact?.MaybeContext != null) {
                        fact.MaybeContext.m_Params = fact.MaybeContext.Params?.Clone();
                    }
                });
            }
        }
    }
}
