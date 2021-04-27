using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Bloodrager {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Settings.Fixes.Bloodrager.DisableAllFixes) { return; }
                Main.LogHeader("Patching Bloodrager Resources");
                PatchBaseClass();
                PatchPrimalist();
                PatchSteelblood();
                Main.LogHeader("Patching Bloodrager Resources Complete");
            }
            static void PatchBaseClass() {
                if (Settings.Fixes.Bloodrager.Base.DisableAllFixes) { return; }
                PatchSpellsPerDayTable();

                void PatchSpellsPerDayTable() {
                    if (!Settings.Fixes.Bloodrager.Base.Fixes["SpellsPerDay"]) { return; }
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
            }
            static void PatchPrimalist() {
                if (Settings.Fixes.Bloodrager.Archetypes["Primalist"].DisableAllFixes) { return; }
                PatchRagePowerFeatQualifications();
                void PatchRagePowerFeatQualifications() {
                    if (!Settings.Fixes.Bloodrager.Archetypes["Primalist"].Fixes["RagePowerFeatQualifications"]) { return; }
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
                        var PrimalistRagePowerSelection = Resources.GetBlueprint<BlueprintFeatureSelection>(Settings.Blueprints.NewBlueprints["PrimalistRagePowerSelection"]);
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
                if (Settings.Fixes.Bloodrager.Archetypes["Steelblood"].DisableAllFixes) { return; }
                PatchArmoredSwiftness();

                void PatchArmoredSwiftness() {
                    if (!Settings.Fixes.Bloodrager.Archetypes["Steelblood"].Fixes["ArmoredSwiftness"]) { return; }
                    var ArmoredHulkArmoredSwiftness = Resources.GetBlueprint<BlueprintFeature>("f95f4f3a10917114c82bcbebc4d0fd36");
                    var SteelbloodArmoredSwiftness = Resources.GetBlueprint<BlueprintFeature>("bd4397ee26a3baf4cadaeb766b018cff");
                    SteelbloodArmoredSwiftness.ComponentsArray = ArmoredHulkArmoredSwiftness.ComponentsArray;
                }
            }
        }
    }
}
