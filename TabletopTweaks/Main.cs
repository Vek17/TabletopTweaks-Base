using UnityModManagerNet;
using HarmonyLib;
using Kingmaker.Blueprints;
using System;

namespace TabletopTweaks {
    static class Main {
        public static bool Enabled;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            Resources.ModEntry = modEntry;
            //Resources.Initalize();
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
            Resources.ModEntry.Logger.Log(msg);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogPatch(string action, BlueprintScriptableObject bp) {
            Log($"{action}: {bp.AssetGuid} - {bp.name}");
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogHeader(string msg) {
            Log($"--{msg.ToUpper()}--");
        }
        internal static Exception Error(String message) {
            Log(message);
            return new InvalidOperationException(message);
        }
    }
}