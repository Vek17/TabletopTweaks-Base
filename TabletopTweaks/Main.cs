using UnityModManagerNet;
using HarmonyLib;

namespace TabletopTweaks {
    static class Main {

        public static bool Enabled;

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg) {
            Resources.Mod.Logger.Log(msg);
        }

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
    }
}