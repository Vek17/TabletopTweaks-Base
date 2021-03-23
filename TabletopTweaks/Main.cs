using static UnityModManagerNet.UnityModManager;
using UnityModManagerNet;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Facts;

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

        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;
            static bool Prefix() {
                if (Initialized) {
                    // When wrath first loads into the main menu InitializeLibrary is called by Kingmaker.GameStarter.
                    // When loading into maps, Kingmaker.Runner.Start will call InitializeLibrary which will
                    // clear the ResourcesLibrary.s_LoadedBlueprints cache which causes loaded blueprints to be garbage collected.
                    // Return false here to prevent ResourcesLibrary.InitializeLibrary from being called twice 
                    // to prevent blueprints from being garbage collected.
                    return false;
                }
                else {
                    return true;
                }
            }
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                UnitAdjustments.patchDemonSubtypes();
                BalanceAdjustments.patchNaturalArmorEffects();
                //Do Stuff
            }
        }

        [HarmonyPatch(typeof(ModifierDescriptorHelper), "IsStackable", new[] { typeof(ModifierDescriptor) })]
        static class ModifierDescriptorHelper_IsStackable_Patch {

            static void Postfix(ref bool __result, ref ModifierDescriptor descriptor) {
                if (!Resources.Settings.DisableNaturalArmorStacking) { return; }
                if (descriptor == ModifierDescriptor.NaturalArmor) {
                    Main.Log($"{descriptor} - { (descriptor == ModifierDescriptor.NaturalArmor ? false : __result)}");
                    __result = descriptor == ModifierDescriptor.NaturalArmor ? false : __result;
                }
            }
        }
    }
}