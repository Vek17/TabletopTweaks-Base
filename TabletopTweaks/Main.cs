using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;
using UnityModManagerNet;

namespace TabletopTweaks {
    static class Main {
        public static bool Enabled;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            ModSettings.ModEntry = modEntry;
            ModSettings.LoadAllSettings();
            ModSettings.ModEntry.OnSaveGUI = OnSaveGUI;
            ModSettings.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize();
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            ModSettings.SaveSettings("Fixes.json", ModSettings.Fixes);
            ModSettings.SaveSettings("AddedContent.json", ModSettings.AddedContent);
            ModSettings.SaveSettings("Homebrew.json", ModSettings.Homebrew);
        }

        public static void Log(string msg) {
            ModSettings.ModEntry.Logger.Log(msg);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogDebug(string msg) {
            Log(msg);
        }

        public static void LogPatch([NotNull] IScriptableObjectWithAssetId bp, bool debug = false) {
            LogPatch("Patched", bp, debug);
        }

        public static void LogPatch(string action, [NotNull] IScriptableObjectWithAssetId bp, bool debug = false) {
            if (debug) {
                LogDebug($"{action}: {bp.AssetGuid} - {bp.name}");
            } else {
                Log($"{action}: {bp.AssetGuid} - {bp.name}");
            }
        }

        public static void LogHeader(string msg) {
            Log($"--{msg.ToUpper()}--");
        }

        public static void Error(Exception e, string message) {
            Log(message);
            Log(e.ToString());
            PFLog.Mods.Error(message);
        }

        public static void Error(string message) {
            Log(message);
            PFLog.Mods.Error(message);
        }
    }
}