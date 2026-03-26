using HarmonyLib;
using TabletopTweaks.Base.ModLogic;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace TabletopTweaks.Base {
    static class Main {
        public static bool Enabled;
        public static ModContextTTTBase TTTContext;
        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            TTTContext = new ModContextTTTBase(modEntry);
            TTTContext.ModEntry.OnSaveGUI = OnSaveGUI;
            TTTContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            PatchBlueprintsCacheInit.CreateDynamicPatches(harmony);
            harmony.PatchAll();
            PostPatchInitializer.Initialize(TTTContext);
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            TTTContext.SaveAllSettings();
        }
    }
}
