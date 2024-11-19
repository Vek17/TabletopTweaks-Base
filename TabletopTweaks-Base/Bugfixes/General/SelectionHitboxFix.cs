using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using Kingmaker.View;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.General {
    internal class SelectionHitboxFix {
        [HarmonyPatch(typeof(UnitEntityView), nameof(UnitEntityView.SetupSelectionColliders), new Type[] { typeof(bool) })]
        static class Arcanist_ActionBarSpellbookHelper_Patch {
            static readonly FieldInfo UnitEntityView_m_Corpulence = AccessTools.Field(typeof(UnitEntityView), "m_Corpulence");
            static readonly MethodInfo UnitEntityView_Corpulence = AccessTools.PropertyGetter(typeof(UnitEntityView), "Corpulence");
            //Prevent Collision radius from scaling with size adjusted corpulance
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("SelectionHitboxFix")) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                codes[target] = new CodeInstruction(OpCodes.Ldfld, UnitEntityView_m_Corpulence);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Call && codes[i].Calls(UnitEntityView_Corpulence)) {
                        return i ;
                    }
                }

                TTTContext.Logger.Log("SelectionHitboxFix: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
