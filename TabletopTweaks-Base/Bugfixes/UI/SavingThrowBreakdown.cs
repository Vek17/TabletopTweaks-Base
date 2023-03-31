using HarmonyLib;
using Kingmaker;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Combat;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Core.MechanicsChanges;
using TurnBased.Controllers;
using static TabletopTweaks.Base.Main;
using Kingmaker.Blueprints.Root.Strings.GameLog;

namespace TabletopTweaks.Base.Bugfixes.UI {
    internal class SavingThrowBreakdown {
        [HarmonyPatch(typeof(SavingThrowMessage), nameof(SavingThrowMessage.GetData))]
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
