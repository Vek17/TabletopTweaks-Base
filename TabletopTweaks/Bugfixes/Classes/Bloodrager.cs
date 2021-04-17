using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Bloodrager {
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
                if (Settings.Fixes.Bloodrager.DisableAllFixes) { return; }
                Main.LogHeader("Patching Bloodrager Resources");
                PatchBaseClass();
                PatchPrimalist();
                Main.LogHeader("Patching Bloodrager Resources Complete");
            }
            static void PatchBaseClass() {
                if (Settings.Fixes.Bloodrager.Base.DisableAllFixes) { return; }
                PatchSpellsPerDayTable();

                void PatchSpellsPerDayTable() {
                    if (!Settings.Fixes.Bloodrager.Base.Fixes["SpellsPerDay"]) { return; }
                    BlueprintSpellsTable BloodragerSpellPerDayTable = ResourcesLibrary.TryGetBlueprint<BlueprintSpellsTable>("caf7018942861664ebe87687893ad05d");
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
                if (Settings.Fixes.Bloodrager.Archetypes["Primalist"].DisableAllFixes) { return;  }
                PatchRagePowerFeatQualifications();
                void PatchRagePowerFeatQualifications() {
                    if (!Settings.Fixes.Bloodrager.Archetypes["Primalist"].Fixes["RagePowerFeatQualifications"]) { return; }
                    var PrimalistRagePowerSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>(Settings.Blueprints.NewBlueprints["PrimalistRagePowerSelection"]);
                    var PrimalistTakeRagePowers4 = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("8eb5c34bb8471a0438e7eb3994de3b92");
                    var Level = new LevelEntry {
                        Level = 4,
                        Features = {
                            PrimalistRagePowerSelection,
                            PrimalistRagePowerSelection
                        }
                    };
                    PrimalistTakeRagePowers4.LevelEntries = new LevelEntry[] { Level };
                    Main.LogPatch("Patched", PrimalistTakeRagePowers4);
                }
            }
        }
    }
}
