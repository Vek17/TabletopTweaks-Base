using static UnityModManagerNet.UnityModManager;
using UnityModManagerNet;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.Facts;

namespace TabletopTweaks {
    static class Main {

        public static bool Enabled;
        public static ModEntry Mod;

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg) {
            Mod.Logger.Log(msg);
        }

        static bool Load(UnityModManager.ModEntry modEntry) {
            var harmony = new Harmony(modEntry.Info.Id);
            Mod = modEntry;
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
                //Do Stuff
            }
        }

        [HarmonyPatch(typeof(ACBonusAgainstFactOwnerMultiple), "OnEventAboutToTrigger", new[] { typeof(RuleAttackRoll) })]
        static class ACBonusAgainstFactOwnerMultiple_OnEventAboutToTrigger_Patch {
            //private static HashSet<RuleDealDamage> m_GeneratedRules = new HashSet<RuleDealDamage>();

            static void Postfix(ACBonusAgainstFactOwnerMultiple __instance, ref RuleAttackRoll evt) {
                Main.Log("ACBonusAgainstFactOwnerMultiple");
                foreach (BlueprintUnitFact blueprintUnitFact in __instance.Facts) {
                    Main.Log($"{blueprintUnitFact.name}");
                }
                foreach (var fact in evt.Initiator.Facts.m_Facts) {
                    Main.Log($"Taret: {fact.Name}");
                }

            }
        }
    }
}