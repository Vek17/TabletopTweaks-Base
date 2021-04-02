using UnityModManagerNet;
using HarmonyLib;
using Kingmaker.Blueprints;

namespace TabletopTweaks {
    static class Main {
        public static bool Enabled;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            Resources.Mod = modEntry;
            Resources.LoadSettings();
            harmony.PatchAll();
            return true;
        }
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            Enabled = value;
            return true;
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg) {
            Resources.Mod.Logger.Log(msg);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogPatch(string action, BlueprintScriptableObject bp) {
            Log($"{action}: {bp.AssetGuid} - {bp.name}");
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogHeader(string msg) {
            Log($"--{msg.ToUpper()}--");
        }
    }
}