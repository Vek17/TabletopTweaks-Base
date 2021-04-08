using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Azata {
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
                if (Resources.Fixes.Azata.DisableAllFixes) { return; }
                Main.LogHeader("Patching Azata Resources");
                PatchAzataPerformanceResource();
                patchFavorableMagic();
                patchZippyMagicFeature();
                Main.LogHeader("Azata Resource Patch Complete");
            }
            
            static void PatchAzataPerformanceResource() {
                if (!Resources.Fixes.Azata.Fixes["AzataPerformanceResource"]) { return; }
                var AzataPerformanceResource = ResourcesLibrary.TryGetBlueprint<BlueprintAbilityResource>("83f8a1c45ed205a4a989b7826f5c0687");

                BlueprintCharacterClassReference[] characterClasses = ResourcesLibrary
                    .GetRoot()
                    .Progression
                    .CharacterClasses
                    .Where(c => c != null)
                    .Select(c => c.ToReference<BlueprintCharacterClassReference>())
                    .ToArray();
                AzataPerformanceResource.m_MaxAmount.m_Class = characterClasses;
                Main.LogPatch("Patched", AzataPerformanceResource);
            }

            static void patchFavorableMagic() {
                if (!Resources.Fixes.Azata.Fixes["FavorableMagic"]) { return;  }
                var FavorableMagicFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("afcee6925a6eadf43820d12e0d966ebe");
                var fixedComponent = Helpers.Create<NewComponents.AzataFavorableMagicComponent>();
                FavorableMagicFeature.ReplaceComponents<AzataFavorableMagic>(fixedComponent);
                Main.LogPatch("Patched", FavorableMagicFeature);
            }

            static void patchZippyMagicFeature() {
                if (!Resources.Fixes.Azata.Fixes["ZippyMagic"]) { return; }
                var ZippyMagicFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("30b4200f897ba25419ba3a292aed4053");
                var ZippyMagicDamage = Helpers.Create<NewComponents.AzataZippyMagicDamageComponent>();
                ZippyMagicFeature.AddComponent(ZippyMagicDamage);
                Main.LogPatch("Patched", ZippyMagicFeature);
            }
        }
        
        // Patch for ZippyMagic
        [HarmonyPatch(typeof(DublicateSpellComponent), "CheckAOE", new[] { typeof(AbilityData) })]
        static class DublicateSpellComponent_CheckAOE_Patch {
            static bool disabled = Resources.Fixes.Azata.DisableAllFixes || !Resources.Fixes.Azata.Fixes["ZippyMagic"];

            static void Postfix(DublicateSpellComponent __instance, ref AbilityData spell, ref bool __result) {
                if (disabled) { return; }
                if (spell.Blueprint.GetComponents<AbilityEffectStickyTouch>().Any()) { __result = false; }
            }
        }
        
    }
}
