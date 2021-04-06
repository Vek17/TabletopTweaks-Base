using HarmonyLib;
using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Utility;

namespace TabletopTweaks.Bugfixes.Units {
    static class DemonSubtypes {

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
                Main.LogHeader("Patching Demon Subtypes");
                patchDemonSubtypes();
                Main.LogHeader("Demon Subtype Patch Complete");
            }
        }

        public static void patchDemonSubtypes() {
            if (!Resources.Fixes.FixDemonSubtypes) { return; }
            BlueprintFeature subtypeDemon = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("dc960a234d365cb4f905bdc5937e623a");
            BlueprintFeature subtypeEvil = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("5279fc8380dd9ba419b4471018ffadd1");
            BlueprintFeature subtypeChaotic = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1dd712e7f147ab84bad6ffccd21a878d");
            Resources.GetBlueprints<BlueprintUnit>()
                .Where(bp => Array.Exists(bp.m_AddFacts, e => e.GetBlueprint() == subtypeDemon)
                         && !Array.Exists(bp.m_AddFacts, e => e.GetBlueprint() == subtypeEvil))
                .OrderBy(bp => bp.name)
                .ForEach(bp => {
                    bp.m_AddFacts = bp.m_AddFacts.AddItem(subtypeEvil.ToReference<BlueprintUnitFactReference>()).ToArray();
                    Main.LogPatch("Added SubtypeEvil", bp);
                });
            Resources.GetBlueprints<BlueprintUnit>()
                .Where(bp => Array.Exists(bp.m_AddFacts, e => e.GetBlueprint() == subtypeDemon)
                         && !Array.Exists(bp.m_AddFacts, e => e.GetBlueprint() == subtypeChaotic))
                .OrderBy(bp => bp.name)
                .ForEach(bp => {
                    bp.m_AddFacts = bp.m_AddFacts.AddItem(subtypeChaotic.ToReference<BlueprintUnitFactReference>()).ToArray();
                    Main.LogPatch("Added SubtypeChaotic", bp);
                });
        }
    }
}
