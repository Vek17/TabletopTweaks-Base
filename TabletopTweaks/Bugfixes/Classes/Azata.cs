using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
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
                patchAzataFavorableMagic();
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

            static void patchAzataFavorableMagic() {
                if (!Resources.Fixes.Azata.Fixes["FavorableMagic"]) { return;  }
                var FavorableMagicFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("afcee6925a6eadf43820d12e0d966ebe");
                var fixedComponent = Helpers.Create<AzataFavorableMagicFixed>();
                FavorableMagicFeature.ReplaceComponents<AzataFavorableMagic>(fixedComponent);
                Main.LogPatch("Patched", FavorableMagicFeature);
            }
        }
        // Patch for Zippy Magic
        [HarmonyPatch(typeof(DublicateSpellComponent), "Kingmaker.PubSubSystem.IRulebookHandler<Kingmaker.RuleSystem.Rules.Abilities.RuleCastSpell>.OnEventDidTrigger", new[] { typeof(RuleCastSpell) })]
        static class DublicateSpellComponent_OnEventDidTrigger_Patch {
            static bool disabled = Resources.Fixes.Azata.DisableAllFixes || !Resources.Fixes.Azata.Fixes["ZippyMagic"];

            static void Postfix(DublicateSpellComponent __instance, ref RuleCastSpell evt) {
                if (disabled) { return; }
                Main.Log("Zippy Trigger");
                if (evt.IsSpellFailed ||
                    evt.Spell.IsAOE ||
                    !evt.SpellTarget.Unit.IsPlayersEnemy ||
                    evt.Spell.Blueprint.Animation == UnitAnimationActionCastSpell.CastAnimationStyle.Self) {

                    Main.Log($"{evt.Spell.Name} : Zippy Trigger Early Return");
                    return;
                }
                Main.Log($"{evt.Spell.Name} : Zippy Trigger Entered Damage Trigger");
                DiceFormula dice = new DiceFormula(2, DiceType.D6);
                int mythicLevel = evt.Spell.Caster.Unit.Progression.MythicExperience;
                RuleDealDamage ruleDealDamage = new RuleDealDamage(evt.Spell.Caster, evt.SpellTarget.Unit, new EnergyDamage(dice, mythicLevel, DamageEnergyType.Divine));
                Rulebook.Trigger<RuleDealDamage>(ruleDealDamage);
            }
        }
    }
}
