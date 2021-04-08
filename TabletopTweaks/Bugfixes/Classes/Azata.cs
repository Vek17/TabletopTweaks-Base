using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Extensions;
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
                PatchFavorableMagic();
                PatchZippyMagicFeature();
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

            static void PatchFavorableMagic() {
                if (!Resources.Fixes.Azata.Fixes["FavorableMagic"]) { return;  }
                var FavorableMagicFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("afcee6925a6eadf43820d12e0d966ebe");
                var fixedComponent = Helpers.Create<NewComponents.AzataFavorableMagicComponent>();

                FavorableMagicFeature.ReplaceComponents<AzataFavorableMagic>(fixedComponent);
                Main.LogPatch("Patched", FavorableMagicFeature);
            }

            static void PatchZippyMagicFeature() {
                if (!Resources.Fixes.Azata.Fixes["ZippyMagic"]) { return; }
                var ZippyMagicFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("30b4200f897ba25419ba3a292aed4053");
                var ZippyMagic = Helpers.Create<NewComponents.AzataZippyMagicComponent>();

                ZippyMagicFeature.ReplaceComponents<DublicateSpellComponent>(ZippyMagic);
                Main.LogPatch("Patched", ZippyMagicFeature);
                PatchCureWoundsDamage();
                PatchInflictWoundsDamage();

                void PatchCureWoundsDamage() {
                    BlueprintAbility[] cureSpells = new BlueprintAbility[] {
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("1edd1e201a608a24fa1de33d57502244"), // CureLightWoundsDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("148673963b23fae4f9fcdcc5d67a91cc"), // CureModerateWoundsDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("dd5d65e25a4e8b54a87d976c0a80f5b6"), // CureSeriousWoundsDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("7d626a2c5eee30b47bbf7fee36d05309"), // CureCriticalWoundsDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("fb7e5fe8b5750f9408398d9659b0f98f"), // CureLightWoundsMassDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("638363b5afb817d4684c021d36279904"), // CureModerateWoundsMassDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("21d02c685b2e64b4f852b3efcb0b5ca6"), // CureSeriousWoundsMassDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("0cce61a5e5108114092f9773572c78b8"), // CureCriticalWoundsMassDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("6ecd2657cb645274cbc167d667ac521d"), // HealDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("7df289eaaf1233248b7be754f894de2e")  // HealMassDamage
                    };
                    cureSpells.ForEach(spell => BlockSpellDuplication(spell));
                }
                void PatchInflictWoundsDamage() {
                    BlueprintAbility[] inflictSpells = new BlueprintAbility[] {
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("f6ff156188dc4e44c850179fb19afaf5"), // InflictLightWoundsDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("e55f5a1b875a5f242be5b92cf027b69a"), // InflictModerateWoundsDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("095eaa846e2a8c343a54e927816e00af"), // InflictSeriousWoundsDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("2737152467af53b4f9800e7a60644bb6"), // InflictCriticalWoundsDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("b70d903464a738148a19bed630b91f8c"), // InflictLightWoundsMassDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("89ddb1b4dafc5f541a3dacafbf9ea2dd"), // InflictModerateWoundsMassDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("aba480ce9381684408290f5434402a32"), // InflictSeriousWoundsMassDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("e05c263048e835043bb2784601dca339"), // InflictCriticalWoundsMassDamage
                        ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("3da67f8b941308348b7101e7ef418f52")  // HarmDamage
                    };
                    inflictSpells.ForEach(spell => BlockSpellDuplication(spell));
                }
                void BlockSpellDuplication(BlueprintAbility blueprint) {
                    blueprint.AddComponent(Helpers.Create<NewComponents.BlockSpellDuplicationComponent>());
                    Main.LogPatch("Blocked Duplication", blueprint);
                }
            }
        }
    }
}
