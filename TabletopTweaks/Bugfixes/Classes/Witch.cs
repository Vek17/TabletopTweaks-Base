using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using static Kingmaker.Blueprints.Classes.Spells.SpellDescriptor;

namespace TabletopTweaks.Bugfixes.Classes {
    class Witch {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Witch.DisableAll) { return; }
                Main.LogHeader("Patching Witch");
                PatchBaseClass();
            }
            static void PatchBaseClass() {
                if (ModSettings.Fixes.Witch.Base.DisableAll) { return; }
                PatchWitchHexes();
            }
            static void PatchWitchHexes() {
                // Death Curse is not technically typed as death or curse RAW but changing it anyway because seriously
                PatchDeathCurse();
                PatchDeliciousFright();
                PatchEvilEye();
                PatchHoarfrost();
                PatchSlumber();
                PatchRestlessSlumber();

                void PatchDeathCurse() {
                    if (!ModSettings.Fixes.Witch.Base.Enabled["DeathCurse"]) { return; }
                    BlueprintAbility WitchHexDeathCurseAbility = Resources.GetBlueprint<BlueprintAbility>("d560ab2a1b0613649833a0d92d6cfc6b");
                    WitchHexDeathCurseAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | Death | Curse;
                    Main.LogPatch("Patched", WitchHexDeathCurseAbility);
                }
                void PatchDeliciousFright() {
                    if (!ModSettings.Fixes.Witch.Base.Enabled["DeliciousFright"]) { return; }
                    BlueprintAbility WitchHexDeliciousFrightAbility = Resources.GetBlueprint<BlueprintAbility>("e7489733ac7ccca40917d9364b406adb");
                    WitchHexDeliciousFrightAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | MindAffecting | Fear;
                    Main.LogPatch("Patched", WitchHexDeliciousFrightAbility);
                }
                void PatchEvilEye() {
                    if (!ModSettings.Fixes.Witch.Base.Enabled["EvilEye"]) { return; }
                    BlueprintAbility WitchHexEvilEyeAbility = Resources.GetBlueprint<BlueprintAbility>("d25c72a92dd8d38449a6a371ef36413e");
                    BlueprintAbility WitchHexSlumberAbility = Resources.GetBlueprint<BlueprintAbility>("630ea63a63457ff4f9de059c578c930a");
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
                        Main.LogPatch("Patched", Variant);
                    }
                }
                void PatchHoarfrost() {
                    if (!ModSettings.Fixes.Witch.Base.Enabled["Hoarfrost"]) { return; }
                    BlueprintAbility WitchHexHoarfrostAbility = Resources.GetBlueprint<BlueprintAbility>("7244a24f0c186ce4b8a89fd26feded50");
                    WitchHexHoarfrostAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | Cold;
                    Main.LogPatch("Patched", WitchHexHoarfrostAbility);
                }
                void PatchRestlessSlumber() {
                    if (!ModSettings.Fixes.Witch.Base.Enabled["RestlessSlumber"]) { return; }
                    BlueprintAbility WitchHexRestlessSlumberAbility = Resources.GetBlueprint<BlueprintAbility>("a69fb167bb41c6f45a19c81ed4e3c0d9");
                    WitchHexRestlessSlumberAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | MindAffecting | Compulsion | Sleep;
                    Main.LogPatch("Patched", WitchHexRestlessSlumberAbility);
                }
                void PatchSlumber() {
                    if (!ModSettings.Fixes.Witch.Base.Enabled["Slumber"]) { return; }
                    BlueprintAbility WitchHexSlumberAbility = Resources.GetBlueprint<BlueprintAbility>("630ea63a63457ff4f9de059c578c930a");
                    WitchHexSlumberAbility
                        .GetComponent<SpellDescriptorComponent>().Descriptor = Hex | MindAffecting | Compulsion | Sleep;
                    Main.LogPatch("Patched", WitchHexSlumberAbility);
                }
            }
        }
    }
}
