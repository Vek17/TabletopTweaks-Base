using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Core.MechanicsChanges;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.UI {
    internal class SavingThrowBreakdown {

        [HarmonyPatch(typeof(RuleSavingThrow), nameof(RuleSavingThrow.SetStatValueFromThrow))]
        static class RuleSavingThrow_SetStatValueFromThrow_Patch {
            static void Postfix(RuleSavingThrow __instance, ModifiableValueSavingThrow stat) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("SavingThrowCombatLogBreakdowns")) { return; }
                using (stat.GetTemporaryModifiersScope(__instance.AllBonuses)) {
                    var newModifiers = new List<ModifiableValue.Modifier>() {
                        new ModifiableValue.Modifier(){
                            ModValue = stat.BaseStat.Bonus,
                            ModDescriptor = AdditionalModifierDescriptors.GetUntypedDescriptor(stat.BaseStat.Type),
                            StackMode = ModifiableValue.StackMode.Default
                        }
                    };
                    newModifiers.AddRange(__instance.StatModifiersAtTheMoment);
                    __instance.StatModifiersAtTheMoment = newModifiers;
                }
            }
        }

        //[HarmonyPatch(typeof(SavingThrowMessage), nameof(SavingThrowMessage.GetData))]
        static class SavingThrowMessage_GetData_Patch {
            // ------------before------------
            // (this.IsTargetFlatFooted || this.Target.CombatState.IsFlanked);
            // ------------after-------------
            // ruleAttackWithWeapon.FirstAttack = true;
            // (this.IsTargetFlatFooted || this.Target.IsFlankedBy(this.Initiator)));
            static readonly MethodInfo StatModifiersBreakdown_AddModifiers = AccessTools.Method(
                typeof(StatModifiersBreakdown),
                "AddModifiers",
                parameters: new Type[] { 
                    typeof(IEnumerable<ModifiableValue.Modifier>), typeof(bool) 
                }
            );
            static readonly MethodInfo SavingThrowMessage_GetData_Patch_AddStatText = AccessTools.Method(
                typeof(SavingThrowMessage_GetData_Patch),
                "AddStatText",
                parameters: new Type[] { typeof(RuleSavingThrow) }
            );
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("SavingThrowCombatLogBreakdowns")) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                var index = FindInsertionTarget(codes);
                //ILUtils.LogIL(TTTContext, codes);
                codes.InsertRange(index, 
                    new CodeInstruction[] {
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Call, SavingThrowMessage_GetData_Patch_AddStatText)
                    }
                );
                //ILUtils.LogIL(TTTContext, codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].Calls(StatModifiersBreakdown_AddModifiers)) {
                        return i;
                    }
                }
                return -1;
            }
            private static void AddStatText(RuleSavingThrow rule) {
                var savingThrow = rule.Initiator.Stats.GetStat(rule.StatType) as ModifiableValueSavingThrow;
                if (savingThrow == null) { return; }
                StatsStrings stats = LocalizedTexts.Instance.Stats;
                StatModifiersBreakdown.AddBonus(savingThrow.BaseStat.Bonus, stats.GetText(savingThrow.BaseStat.Type), ModifierDescriptor.None, true, null);
            }
        }
    }
}
