using HarmonyLib;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Features {
    class Backgrounds {
        [HarmonyPatch(typeof(ModifiableValueSkill), "UpdateInternalModifiers")]
        static class Arcanist_SpellbookActionBar_Patch {
            static readonly MethodInfo Modifier_AddModifier = AccessTools.Method(typeof(ModifiableValue), "AddModifier", new Type[] {
                typeof(int),
                typeof(EntityFact),
                typeof(ModifierDescriptor)
            });
            //Change bonus descriptor to Trait instead of Competence
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (ModSettings.Fixes.Arcanist.Base.DisableAll || !ModSettings.Fixes.Arcanist.Base.Enabled["PreparedSpellUI"]) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes[target] = new CodeInstruction(OpCodes.Ldc_I4, (int)ModifierDescriptor.Trait);
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    //Add modifer is called only once directly after the descriptor is loaded onto the stack
                    if (codes[i].opcode == OpCodes.Call && codes[i].Calls(Modifier_AddModifier)) {
                        return i - 1;
                    }
                }
                Main.Error("BACKGROUND PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
