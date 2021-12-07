using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.OwlcatReplacements;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.MythicReworks {
    class Azata {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Azata Rework");

                PatchAzataPerformanceResource();
                PatchAzataSongToggles();
                PatchFavorableMagic();
                PatchZippyMagicFeature();
            }

            static void PatchAzataPerformanceResource() {
                if (ModSettings.Homebrew.MythicReworks.Azata.IsDisabled("AzataPerformanceResource")) { return; }
                var AzataPerformanceResource = Resources.GetBlueprint<BlueprintAbilityResource>("83f8a1c45ed205a4a989b7826f5c0687");

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

            static void PatchAzataSongToggles() {
                if (ModSettings.Homebrew.MythicReworks.Azata.IsDisabled("AzataSongToggles")) { return; }

                var SongOfHeroicResolveToggleAbility = Resources.GetBlueprint<BlueprintActivatableAbility>("a95449d0ea0714a4ea5cffc83fc7624f");
                var SongOfBrokenChainsToggleAbility = Resources.GetBlueprint<BlueprintActivatableAbility>("ac08e4d23e2928148a7b4109e9485e6a");
                var SongOfDefianceToggleAbility = Resources.GetBlueprint<BlueprintActivatableAbility>("661ad9ab9c8af2e4c86a7cfa4c2be3f2");
                var SongOfCourageousDefenderToggleAbility = Resources.GetBlueprint<BlueprintActivatableAbility>("66864464f529c264f8c08ec2f4bf1cb5");

                SongOfHeroicResolveToggleAbility.DeactivateImmediately = false;
                SongOfBrokenChainsToggleAbility.DeactivateImmediately = false;
                SongOfDefianceToggleAbility.DeactivateImmediately = false;
                SongOfCourageousDefenderToggleAbility.DeactivateImmediately = false;

                Main.LogPatch("Patched", SongOfHeroicResolveToggleAbility);
                Main.LogPatch("Patched", SongOfBrokenChainsToggleAbility);
                Main.LogPatch("Patched", SongOfDefianceToggleAbility);
                Main.LogPatch("Patched", SongOfCourageousDefenderToggleAbility);
            }

            static void PatchFavorableMagic() {
                if (ModSettings.Homebrew.MythicReworks.Azata.IsDisabled("FavorableMagic")) { return; }
                var FavorableMagicFeature = Resources.GetBlueprint<BlueprintFeature>("afcee6925a6eadf43820d12e0d966ebe");
                var fixedComponent = new AzataFavorableMagicTTT();

                FavorableMagicFeature.SetComponents(
                    Helpers.Create<AzataFavorableMagicTTT>()
                //Helpers.Create<AzataFavorableMagic>()
                );
                Main.LogPatch("Patched", FavorableMagicFeature);
            }

            static void PatchZippyMagicFeature() {
                if (ModSettings.Homebrew.MythicReworks.Azata.IsDisabled("ZippyMagic")) { return; }
                var ZippyMagicFeature = Resources.GetBlueprint<BlueprintFeature>("30b4200f897ba25419ba3a292aed4053");

                ZippyMagicFeature.RemoveComponents<DublicateSpellComponent>();
                ZippyMagicFeature.AddComponent<AzataZippyMagicTTT>();
                Main.LogPatch("Patched", ZippyMagicFeature);
                PatchCureWoundsDamage();
                PatchInflictWoundsDamage();

                void PatchCureWoundsDamage() {
                    BlueprintAbility[] cureSpells = new BlueprintAbility[] {
                        Resources.GetBlueprint<BlueprintAbility>("1edd1e201a608a24fa1de33d57502244"), // CureLightWoundsDamage
                        Resources.GetBlueprint<BlueprintAbility>("148673963b23fae4f9fcdcc5d67a91cc"), // CureModerateWoundsDamage
                        Resources.GetBlueprint<BlueprintAbility>("dd5d65e25a4e8b54a87d976c0a80f5b6"), // CureSeriousWoundsDamage
                        Resources.GetBlueprint<BlueprintAbility>("7d626a2c5eee30b47bbf7fee36d05309"), // CureCriticalWoundsDamage
                        Resources.GetBlueprint<BlueprintAbility>("fb7e5fe8b5750f9408398d9659b0f98f"), // CureLightWoundsMassDamage
                        Resources.GetBlueprint<BlueprintAbility>("638363b5afb817d4684c021d36279904"), // CureModerateWoundsMassDamage
                        Resources.GetBlueprint<BlueprintAbility>("21d02c685b2e64b4f852b3efcb0b5ca6"), // CureSeriousWoundsMassDamage
                        Resources.GetBlueprint<BlueprintAbility>("0cce61a5e5108114092f9773572c78b8"), // CureCriticalWoundsMassDamage
                        Resources.GetBlueprint<BlueprintAbility>("6ecd2657cb645274cbc167d667ac521d"), // HealDamage
                        Resources.GetBlueprint<BlueprintAbility>("7df289eaaf1233248b7be754f894de2e")  // HealMassDamage
                    };
                    cureSpells.ForEach(spell => BlockSpellDuplication(spell));
                }
                void PatchInflictWoundsDamage() {
                    BlueprintAbility[] inflictSpells = new BlueprintAbility[] {
                        Resources.GetBlueprint<BlueprintAbility>("f6ff156188dc4e44c850179fb19afaf5"), // InflictLightWoundsDamage
                        Resources.GetBlueprint<BlueprintAbility>("e55f5a1b875a5f242be5b92cf027b69a"), // InflictModerateWoundsDamage
                        Resources.GetBlueprint<BlueprintAbility>("095eaa846e2a8c343a54e927816e00af"), // InflictSeriousWoundsDamage
                        Resources.GetBlueprint<BlueprintAbility>("2737152467af53b4f9800e7a60644bb6"), // InflictCriticalWoundsDamage
                        Resources.GetBlueprint<BlueprintAbility>("b70d903464a738148a19bed630b91f8c"), // InflictLightWoundsMassDamage
                        Resources.GetBlueprint<BlueprintAbility>("89ddb1b4dafc5f541a3dacafbf9ea2dd"), // InflictModerateWoundsMassDamage
                        Resources.GetBlueprint<BlueprintAbility>("aba480ce9381684408290f5434402a32"), // InflictSeriousWoundsMassDamage
                        Resources.GetBlueprint<BlueprintAbility>("e05c263048e835043bb2784601dca339"), // InflictCriticalWoundsMassDamage
                        Resources.GetBlueprint<BlueprintAbility>("3da67f8b941308348b7101e7ef418f52")  // HarmDamage
                    };
                    inflictSpells.ForEach(spell => BlockSpellDuplication(spell));
                }
                void BlockSpellDuplication(BlueprintAbility blueprint) {
                    blueprint.AddComponent(new NewComponents.BlockSpellDuplicationComponent());
                    Main.LogPatch("Blocked Duplication", blueprint);
                }
            }
        }
    }
}
