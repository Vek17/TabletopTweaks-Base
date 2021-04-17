using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.NewContent.Archetypes {
    class CauldronWitch {
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
                if (!Settings.AddedContent.CauldronWitchArchetype) { return; }
                Main.LogHeader("Adding Cauldren Witch");
                AddCauldrenWitch();
            }
            static void AddCauldrenWitch() {
                var CauldronWitchArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("e0012a7015774e140be217f4a1480b6f");
                var WitchClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("1b9873f1e7bfe5449bc84d03e9c8e3cc");
                WitchClass.m_Archetypes = WitchClass.m_Archetypes.AddItem(CauldronWitchArchetype.ToReference<BlueprintArchetypeReference>()).ToArray();
            }
        }
    }
}
