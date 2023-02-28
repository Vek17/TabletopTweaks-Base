using HarmonyLib;
using Kingmaker.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TabletopTweaks.Core.MechanicsChanges;

namespace TabletopTweaks.Base.Bugfixes.General {
    internal class WeaponEnhancementBonuses {
        //[HarmonyPatch(typeof(WeaponEnhancementBonus), "OnEventAboutToTrigger", new Type[] { typeof(RuleCalculateAttackBonusWithoutTarget) })]
        static class WeaponEnhancementBonus_Descriptor_Patch {
            //Change bonus descriptor to Trait instead of Competence
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("FixWeaponEnhancmentBonusModifiers")) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes[target] = new CodeInstruction(OpCodes.Ldc_I4, (int)AdditionalModifierDescriptors.Enhancement.Weapon);
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].LoadsConstant(ModifierDescriptor.Enhancement)) {
                        return i;
                    }
                }
                Main.TTTContext.Logger.Log("WEAPON ENHANCEMENT BONUS PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
