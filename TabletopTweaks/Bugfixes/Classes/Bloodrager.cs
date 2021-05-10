using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Bloodrager {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Bloodrager.DisableAllFixes) { return; }
                Main.LogHeader("Patching Bloodrager");
                PatchBaseClass();
                PatchPrimalist();
                PatchSteelblood();
                PatchReformedFiend();
            }
            static void PatchBaseClass() {
                if (ModSettings.Fixes.Bloodrager.Base.DisableAllFixes) { return; }
                PatchSpellsPerDayTable();
                PatchAbysalBulk();
                PatchLimitlessRage();

                void PatchAbysalBulk() {
                    if (!ModSettings.Fixes.Bloodrager.Base.Fixes["AbysalBulk"]) { return; }
                    var BloodragerAbyssalBloodlineBaseBuff = Resources.GetBlueprint<BlueprintBuff>("2ba7b4b3b87156543b43d0686404655a");
                    var BloodragerAbyssalDemonicBulkBuff = Resources.GetBlueprint<BlueprintBuff>("031a8053a7c02ab42ad53f50dd2e9437");
                    var BloodragerAbyssalDemonicBulkEnlargeBuff = Resources.GetBlueprint<BlueprintBuff>(ModSettings.Blueprints.NewBlueprints["BloodragerAbyssalDemonicBulkEnlargeBuff"]);

                    var ApplyBuff = new ContextActionApplyBuff() {
                        m_Buff = BloodragerAbyssalDemonicBulkEnlargeBuff.ToReference<BlueprintBuffReference>(),
                        AsChild = true,
                        Permanent = true
                    };
                    var RemoveBuff = new ContextActionRemoveBuff() {
                        m_Buff = BloodragerAbyssalDemonicBulkEnlargeBuff.ToReference<BlueprintBuffReference>()
                    };
                    var AddFactContext = BloodragerAbyssalBloodlineBaseBuff.GetComponent<AddFactContextActions>();

                    AddFactContext.Activated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().AddActionIfTrue(ApplyBuff);
                    AddFactContext.Deactivated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().IfTrue = null;
                    AddFactContext.Deactivated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().AddActionIfTrue(RemoveBuff);
                }
                void PatchSpellsPerDayTable() {
                    if (!ModSettings.Fixes.Bloodrager.Base.Fixes["SpellsPerDay"]) { return; }
                    BlueprintSpellsTable BloodragerSpellPerDayTable = Resources.GetBlueprint<BlueprintSpellsTable>("caf7018942861664ebe87687893ad05d");
                    BloodragerSpellPerDayTable.Levels = new SpellsLevelEntry[] {
                    // 18 Spell levels is correct for w/e reason
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0,1),
                        CreateSpellLevelEntry(0,1),
                        CreateSpellLevelEntry(0,1),
                        CreateSpellLevelEntry(0,1,1),
                        CreateSpellLevelEntry(0,1,1),
                        CreateSpellLevelEntry(0,2,1),
                        CreateSpellLevelEntry(0,2,1,1),
                        CreateSpellLevelEntry(0,2,1,1),
                        CreateSpellLevelEntry(0,2,2,1),
                        CreateSpellLevelEntry(0,3,2,1,1),
                        CreateSpellLevelEntry(0,3,2,1,1),
                        CreateSpellLevelEntry(0,3,2,2,1),
                        CreateSpellLevelEntry(0,3,3,2,1),
                        CreateSpellLevelEntry(0,4,3,2,1),
                        CreateSpellLevelEntry(0,4,3,2,2),
                        CreateSpellLevelEntry(0,4,3,3,2),
                        CreateSpellLevelEntry(0,4,4,3,2)
                    };
                    Main.LogPatch("Patched", BloodragerSpellPerDayTable);
                    SpellsLevelEntry CreateSpellLevelEntry(params int[] count) {
                        var entry = new SpellsLevelEntry();
                        entry.Count = count;
                        return entry;
                    }
                }
                void PatchLimitlessRage() {
                    if (!ModSettings.Fixes.Bloodrager.Base.Fixes["LimitlessRage"]) { return; }
                    var BloodragerStandartRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
                    var BloodragerRageResource = Resources.GetBlueprint<BlueprintAbilityResource>("4aec9ec9d9cd5e24a95da90e56c72e37");
                    BloodragerStandartRageBuff
                        .GetComponent<TemporaryHitPointsPerLevel>()
                        .m_LimitlessRageResource = BloodragerRageResource.ToReference<BlueprintAbilityResourceReference>();
                    Main.LogPatch("Patched", BloodragerRageResource);
                }
            }
            static void PatchPrimalist() {
                if (ModSettings.Fixes.Bloodrager.Archetypes["Primalist"].DisableAllFixes) { return; }
                PatchRagePowerFeatQualifications();
                void PatchRagePowerFeatQualifications() {
                    if (!ModSettings.Fixes.Bloodrager.Archetypes["Primalist"].Fixes["RagePowerFeatQualifications"]) { return; }
                    var PrimalistTakeRagePowers4 = Resources.GetBlueprint<BlueprintProgression>("8eb5c34bb8471a0438e7eb3994de3b92");
                    var PrimalistTakeRagePowers8 = Resources.GetBlueprint<BlueprintProgression>("db2710cd915bbcf4193fa54083e56b27");
                    var PrimalistTakeRagePowers12 = Resources.GetBlueprint<BlueprintProgression>("e43a7bfd5c90a514cab1c11b41c550b1");
                    var PrimalistTakeRagePowers16 = Resources.GetBlueprint<BlueprintProgression>("b6412ff44f3a82f499d0dd6748a123bc");
                    var PrimalistTakeRagePowers20 = Resources.GetBlueprint<BlueprintProgression>("5905a80d5934248439e83612d9101b4b");

                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers4, 4);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers8, 8);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers12, 12);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers16, 16);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers20, 20);

                    void PatchPrimalistTakeRagePowers(BlueprintProgression PrimalistTakeRagePowers, int level) {
                        var PrimalistRagePowerSelection = Resources.GetBlueprint<BlueprintFeatureSelection>(ModSettings.Blueprints.NewBlueprints["PrimalistRagePowerSelection"]);
                        PrimalistTakeRagePowers.LevelEntries = new LevelEntry[] {
                            new LevelEntry {
                                Level = level,
                                Features = {
                                    PrimalistRagePowerSelection,
                                    PrimalistRagePowerSelection
                                }
                            }
                        };
                        Main.LogPatch("Patched", PrimalistTakeRagePowers);
                    }
                }
            }
            static void PatchSteelblood() {
                if (ModSettings.Fixes.Bloodrager.Archetypes["Steelblood"].DisableAllFixes) { return; }
                PatchArmoredSwiftness();

                void PatchArmoredSwiftness() {
                    if (!ModSettings.Fixes.Bloodrager.Archetypes["Steelblood"].Fixes["ArmoredSwiftness"]) { return; }
                    var ArmoredHulkArmoredSwiftness = Resources.GetBlueprint<BlueprintFeature>("f95f4f3a10917114c82bcbebc4d0fd36");
                    var SteelbloodArmoredSwiftness = Resources.GetBlueprint<BlueprintFeature>("bd4397ee26a3baf4cadaeb766b018cff");
                    SteelbloodArmoredSwiftness.ComponentsArray = ArmoredHulkArmoredSwiftness.ComponentsArray;
                }
            }
            static void PatchReformedFiend() {
                if (ModSettings.Fixes.Bloodrager.Archetypes["ReformedFiend"].DisableAllFixes) { return; }
                PatchHatredAgainstEvil();
                PatchDamageReduction();

                void PatchHatredAgainstEvil() {
                    if (!ModSettings.Fixes.Bloodrager.Archetypes["ReformedFiend"].Fixes["HatredAgainstEvil"]) { return; }
                    var BloodragerClass = Resources.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
                    var ReformedFiendBloodrageBuff = Resources.GetBlueprint<BlueprintBuff>("72a679f712bd4f69a07bf03d5800900b");
                    var rankConfig = ReformedFiendBloodrageBuff.GetComponent<ContextRankConfig>();

                    rankConfig.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    rankConfig.m_Class = new BlueprintCharacterClassReference[] { BloodragerClass.ToReference<BlueprintCharacterClassReference>() };
                    rankConfig.m_UseMin = true;
                }
                void PatchDamageReduction() {
                    if (!ModSettings.Fixes.Bloodrager.Archetypes["ReformedFiend"].Fixes["DamageReduction"]) { return; }
                    var ReformedFiendDamageReductionFeature = Resources.GetBlueprint<BlueprintFeature>("2a3243ad1ccf43d5a5d69de3f9d0420e");
                    ReformedFiendDamageReductionFeature.GetComponent<AddDamageResistancePhysical>().BypassedByAlignment = true;
                }
            }
        }
    }
}
