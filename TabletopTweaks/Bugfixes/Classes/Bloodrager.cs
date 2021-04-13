using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (Resources.Fixes.Bloodrager.DisableAllFixes) { return; }
                Main.LogHeader("Patching Bloodrager Resources");
                PatchSpellsPerDayTable();
                Main.LogHeader("Patching Bloodrager Resources Complete");
            }
            static void PatchSpellsPerDayTable() {
                if (!Resources.Fixes.Bloodrager.Fixes["SpellsPerDay"]) { return; }
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
                Main.LogPatch("patched", BloodragerSpellPerDayTable);
                SpellsLevelEntry CreateSpellLevelEntry(params int[] count) {
                    var entry = new SpellsLevelEntry();
                    entry.Count = count;
                    return entry;
                }
            }
        }
    }
}
