using HarmonyLib;
using Kingmaker.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace TabletopTweaks.Base.Bugfixes.General {
    internal static class SizeShiftLimits {
        [HarmonyPatch(typeof(WeaponSizeExtension), nameof(WeaponSizeExtension.Shift))]
        private class UnitDescriptor_FixSizeModifiers_Patch {
            /* 
            Original:
            if (size2 < Size.Tiny)
			{
				return Size.Tiny;
			}
            Transpiled:
            if (size2 < Size.Fine)
			{
				return Size.Fine;
			}
            */
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("FixSizeShiftLimits")) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Ldc_I4_2) {
                        codes[i].opcode = OpCodes.Ldc_I4_0;
                    }
                }
                //ILUtils.LogIL(TTTContext, codes);
                return codes.AsEnumerable();
            }
        }
    }
}
