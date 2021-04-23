using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;
using UnityModManagerNet;

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
        public static void LogPatch(string action, [NotNull] BlueprintScriptableObject bp) {
            Log($"{action}: {bp.AssetGuid} - {bp.name}");
        }
        public static void LogHeader(string msg) {
            Log($"--{msg.ToUpper()}--");
        }
        public static Exception Error(String message) {
            Log(message);
            return new InvalidOperationException(message);
        }

        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static bool Prefix() {
                // When wrath first loads into the main menu InitializeLibrary is called by Kingmaker.GameStarter.
                // When loading into maps, Kingmaker.Runner.Start will call InitializeLibrary which will
                // clear the ResourcesLibrary.s_LoadedBlueprints cache which causes loaded blueprints to be garbage collected.
                // Return false here to prevent ResourcesLibrary.InitializeLibrary from being called twice 
                // to prevent blueprints from being garbage collected.
                return Initialized ? false : Initialized = true;
            }
        }
    }
}