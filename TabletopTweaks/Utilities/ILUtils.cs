using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace TabletopTweaks.Utilities {
    public static class ILUtils {
        public static void LogIL(List<CodeInstruction> codes) {
            Main.LogDebug("");
            for (int i = 0; i < codes.Count; i++) {
                object operand = codes[i].operand;
                if (operand is Label) {
                    Main.LogDebug($"{i} - {codes[i].labels.Aggregate("", (s, label) => $"{s}[{label.GetHashCode()}]")} - {codes[i].opcode} - {operand.GetHashCode()}");
                } else {
                    Main.LogDebug($"{i} - {codes[i].labels.Aggregate("", (s, label) => $"{s}[{label.GetHashCode()}]")} - {codes[i].opcode} - {codes[i].operand}");
                }
            }
        }
    }
}
