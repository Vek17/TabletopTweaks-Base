using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.BalanceAdjustments {
    class PolymorphStacking {

        [HarmonyPatch(typeof(RuleCanApplyBuff), "OnTrigger", new[] { typeof(RulebookEventContext) })]
        static class RuleCanApplyBuff_OnTrigger_Patch {

            static void Postfix(RuleCanApplyBuff __instance) {
                if (!Settings.Fixes.DisablePolymorphStacking) { return; }
                if (!Resources.PolymorphBuffs.Contains(__instance.Blueprint)) { return; }
                if (__instance.CanApply && (__instance.Context.MaybeCaster.Faction == __instance.Initiator.Faction)) {
                    IEnumerable<BlueprintBuff> intesection = __instance.Initiator
                        .Buffs
                        .Enumerable
                        .Select(b => b.Blueprint)
                        .Intersect(Resources.PolymorphBuffs);
                    if (intesection.Any()) {
                        foreach (BlueprintBuff buffToRemove in intesection.ToArray()) {
                            __instance.Initiator
                                .Buffs
                                .GetBuff(buffToRemove)
                                .Remove();
                            Main.LogDebug($"Removed Polymorph Buff: {buffToRemove.Name}");
                        }
                        Main.LogDebug($"Applied Polymorph Buff: {__instance.Context.Name}");
                    }
                }
            }
        }
    }
}
