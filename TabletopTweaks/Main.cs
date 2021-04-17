using UnityModManagerNet;
using HarmonyLib;
using Kingmaker.Blueprints;
using System;
using TabletopTweaks.Utilities;
using TabletopTweaks.Config;

namespace TabletopTweaks {
    static class Main {
        public static bool Enabled;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            Settings.ModEntry = modEntry;
            Settings.LoadSettings();
            harmony.PatchAll();
            PostPatchInitializer.Initialize();
            return true;
        }
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            Enabled = value;
            return true;
        }
        public static void Log(string msg) {
            Settings.ModEntry.Logger.Log(msg);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogDebug(string msg) {
            Settings.ModEntry.Logger.Log(msg);
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogPatch(string action, BlueprintScriptableObject bp) {
            LogDebug($"{action}: {bp.AssetGuid} - {bp.name}");
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogHeader(string msg) {
            LogDebug($"--{msg.ToUpper()}--");
        }
        public static Exception Error(String message) {
            Log(message);
            return new InvalidOperationException(message);
        }
    }
}