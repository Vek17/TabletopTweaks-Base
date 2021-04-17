using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using TabletopTweaks.Utilities;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Features {
    class Feats {
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
                if (Settings.Fixes.Feats.DisableAllFixes) { return; }
                Main.LogHeader("Patching Feat Resources");
                PatchCraneWing();
                Main.LogHeader("Patching Feat Resources Complete");
            }

            static void PatchCraneWing() {
                if (!Settings.Fixes.Feats.Fixes["CraneWing"]) { return; }
                BlueprintBuff CraneStyleBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("e8ea7bd10136195478d8a5fc5a44c7da");
                var FightingDefensivlyTrigger = CraneStyleBuff.GetComponent<AddInitiatorAttackWithWeaponTrigger>();
                var Conditionals = FightingDefensivlyTrigger.Action.Actions.OfType<Conditional>();

                var newConditonal = Helpers.Create<ContextConditionHasFreeHand>();
                Conditionals.First().ConditionsChecker.Conditions = Conditionals.First().ConditionsChecker.Conditions.AddItem(newConditonal).ToArray();

                Main.LogPatch("Patched", CraneStyleBuff);
            }
        }
    }
}
