using static Kingmaker.Blueprints.Classes.Spells.SpellDescriptor;
using Kingmaker.Blueprints.Classes.Spells;
using System.Linq;
using Kingmaker.Blueprints;
using HarmonyLib;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Abilities.Components;

namespace TabletopTweaks.Bugfixes.Classes {
    class Witch {
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
                Main.Log("Patching Witch Resources");
                patchWitchHexes();
                Main.Log("Witch Resource Patch Complete");
            }
            static void patchWitchHexes() {
                // Death Curse is not technically typed as death or curse RAW but changing it anyway because seriously
                PatchDeathCurse();
                PatchDeliciousFright();
                PatchEvilEye();
                PatchHoarfrost();
                PatchSlumber();
                PatchRestlessSlumber();

                void PatchDeathCurse() {
                    BlueprintAbility WitchHexDeathCurseAbility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("d560ab2a1b0613649833a0d92d6cfc6b");
                    WitchHexDeathCurseAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | Death | Curse;
                    Main.Log($"{WitchHexDeathCurseAbility.name} Patched");
                }
                void PatchDeliciousFright() {
                    BlueprintAbility WitchHexDeliciousFrightAbility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("e7489733ac7ccca40917d9364b406adb");
                    WitchHexDeliciousFrightAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | MindAffecting | Fear;
                    Main.Log($"{WitchHexDeliciousFrightAbility.name} Patched");
                }
                void PatchHoarfrost() {
                    BlueprintAbility WitchHexHoarfrostAbility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("7244a24f0c186ce4b8a89fd26feded50");
                    WitchHexHoarfrostAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | Cold;
                    Main.Log($"{WitchHexHoarfrostAbility.name} Patched");
                }
                void PatchSlumber() {
                    BlueprintAbility WitchHexSlumberAbility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("630ea63a63457ff4f9de059c578c930a");
                    WitchHexSlumberAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | MindAffecting | Compulsion | Sleep;
                    Main.Log($"{WitchHexSlumberAbility.name} Patched");
                }
                void PatchRestlessSlumber() {
                    BlueprintAbility WitchHexRestlessSlumberAbility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("a69fb167bb41c6f45a19c81ed4e3c0d9");
                    WitchHexRestlessSlumberAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | MindAffecting | Compulsion | Sleep;
                    Main.Log($"{WitchHexRestlessSlumberAbility.name} Patched");
                }
                void PatchEvilEye() {
                    BlueprintAbility WitchHexEvilEyeAbility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("d25c72a92dd8d38449a6a371ef36413e");
                    BlueprintAbility WitchHexSlumberAbility = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("630ea63a63457ff4f9de059c578c930a");
                    // EvilEye additionally needs to have its casting attribute corrected to use the spellbook attribute
                    // Variant abilities are not properly inheriting some properties from parents and need to be managed indivually
                    BlueprintAbilityReference[] EvilEyeVariants = WitchHexEvilEyeAbility
                        .GetComponent<AbilityVariants>()
                        .Variants;
                    foreach (BlueprintAbility Variant in EvilEyeVariants) {
                        Variant.ComponentsArray = Variant.ComponentsArray
                            .Concat(WitchHexSlumberAbility.GetComponents<ContextCalculateAbilityParams>())
                            .ToArray();
                        Variant.GetComponent<SpellDescriptorComponent>().Descriptor = Hex | MindAffecting;
                        Main.Log($"{Variant.name} Patched");
                    }
                }
            }
        }
    }
}
