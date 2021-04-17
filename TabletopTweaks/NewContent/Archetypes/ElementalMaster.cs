using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System.Linq;

namespace TabletopTweaks.NewContent.Archetypes {
    class ElementalMaster {
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
                if (!Resources.AddedContent.ElementalMasterArchetype) { return; }
                Main.LogHeader("Added Elemental Master");
                ElementalMaster();
            }
            static void ElementalMaster() {
                var ElementalMasterArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("a61ab10cc606d2d4f9a891547871e860");
                var ArcanistClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("52dbfd8505e22f84fad8d702611f60b7");
                ArcanistClass.m_Archetypes = ArcanistClass.m_Archetypes.AddItem(ElementalMasterArchetype.ToReference<BlueprintArchetypeReference>()).ToArray();
            }
        }
    }
}
