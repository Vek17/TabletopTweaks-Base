using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.General {
    class CriticalConfirmOn20 {
        [HarmonyPatch(typeof(RuleAttackRoll), "OnTrigger", new Type[] { typeof(RulebookEventContext) })]
        static class RuleAttackRoll_OnTrigger_CritConfirm_Patch {
            static readonly MethodInfo get_CriticalConfirmationRoll = AccessTools.PropertyGetter(typeof(RuleAttackRoll), "CriticalConfirmationRoll");
            static readonly MethodInfo get_CriticalConfirmationD20 = AccessTools.PropertyGetter(typeof(RuleAttackRoll), "CriticalConfirmationD20");
            static readonly MethodInfo RuleRollDice_op_Implicit = AccessTools.Method(typeof(RuleRollDice), "op_Implicit");

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("FixCriticalConfirmationOn20")) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes.InsertRange(target, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, get_CriticalConfirmationD20),
                    new CodeInstruction(OpCodes.Call, RuleRollDice_op_Implicit),
                    new CodeInstruction(OpCodes.Ldc_I4, 20),
                    new CodeInstruction(OpCodes.Ceq),
                    new CodeInstruction(OpCodes.Or),
                });
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Call && codes[i].Calls(get_CriticalConfirmationRoll)) {
                        for (int e = i; i < codes.Count; e++) {
                            if (codes[e].opcode == OpCodes.Br_S && codes[e].operand.GetHashCode() == 40) {
                                return e;
                            }
                        }
                        break;
                    }
                }
                Main.Log("CRITICAL CONFIRM PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
