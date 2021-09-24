using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI._ConsoleUI.Overtips;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TabletopTweaks.Bugfixes.UI {
    class OvertipFixes {

        [HarmonyPatch(typeof(OvertipsVM), "OnEventDidTrigger", new[] { typeof(RuleSavingThrow) })]
        static class DisplayFix_OvertipsVM_SavingThrow_Patch {
            static readonly MethodInfo RuleSavingThrow_SuccessBonus = AccessTools.PropertyGetter(typeof(RuleSavingThrow), "SuccessBonus");
            static readonly MethodInfo RuleSavingThrow_StatValue = AccessTools.PropertyGetter(typeof(RuleSavingThrow), "StatValue");

            //Subtract the targets save bonus from the DC
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                //if (!ModSettings.Fixes.FixBackgroundModifiers) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes.InsertRange(target + 4, new CodeInstruction[] {
                    codes[target].Clone(),
                    codes[target + 1].Clone(),
                    new CodeInstruction(OpCodes.Call, RuleSavingThrow_StatValue),
                    new CodeInstruction(OpCodes.Sub),
                });
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Callvirt && codes[i].Calls(RuleSavingThrow_SuccessBonus)) {
                        return i - 2;
                    }
                }
                Main.Log("DisplayFix_OvertipsVM_SavingThrow_Patch: COULD NOT FIND TARGET");
                return -1;
            }
        }

        //[HarmonyPatch(typeof(OvertipsVM), "HandleDamageDealt", new[] { typeof(RuleDealDamage) })]
        static class DisplayFix_OvertipsVM_AttackRollHit_Patch {
            static readonly MethodInfo RuleAttackRoll_Roll = AccessTools.PropertyGetter(typeof(RuleAttackRoll), "Roll");
            static readonly MethodInfo RuleAttackRoll_D20 = AccessTools.PropertyGetter(typeof(RuleAttackRoll), "D20");
            static readonly MethodInfo RuleRollDice_op_Implicit = AccessTools.Method(typeof(RuleRollDice), "op_Implicit");

            //Change roll to be unmodified value
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                //if (!ModSettings.Fixes.FixBackgroundModifiers) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes[target] = new CodeInstruction(OpCodes.Callvirt, RuleAttackRoll_D20);
                codes.Insert(target + 1, new CodeInstruction(OpCodes.Call, RuleRollDice_op_Implicit));
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Callvirt && codes[i].Calls(RuleAttackRoll_Roll)) {
                        return i;
                    }
                }
                Main.Log("DisplayFix_OvertipsVM_AttackRollHit_Patch: COULD NOT FIND TARGET");
                return -1;
            }
        }

        //[HarmonyPatch(typeof(OvertipsVM), "HandleAttackHitRoll", new[] { typeof(RuleAttackRoll) })]
        static class DisplayFix_OvertipsVM_AttackRollMiss_Patch {
            static readonly MethodInfo RuleAttackRoll_Roll = AccessTools.PropertyGetter(typeof(RuleAttackRoll), "Roll");
            static readonly MethodInfo RuleAttackRoll_D20 = AccessTools.PropertyGetter(typeof(RuleAttackRoll), "D20");
            static readonly MethodInfo RuleRollDice_op_Implicit = AccessTools.Method(typeof(RuleRollDice), "op_Implicit");

            //Change roll to be unmodified value
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                //if (!ModSettings.Fixes.FixBackgroundModifiers) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes[target] = new CodeInstruction(OpCodes.Callvirt, RuleAttackRoll_D20);
                codes.Insert(target + 1, new CodeInstruction(OpCodes.Call, RuleRollDice_op_Implicit));
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Callvirt && codes[i].Calls(RuleAttackRoll_Roll)) {
                        return i;
                    }
                }
                Main.Log("DisplayFix_OvertipsVM_AttackRollMiss_Patch: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
