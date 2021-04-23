using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.NewContent.Archetypes {
    static class ElementalMaster {
        public static void AddElementalMaster() {
            if (!Settings.AddedContent.ElementalMasterArchetype) { return; }
            var ElementalMasterArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("a61ab10cc606d2d4f9a891547871e860");
            var ArcanistClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("52dbfd8505e22f84fad8d702611f60b7");
            ArcanistClass.m_Archetypes = ArcanistClass.m_Archetypes.AddItem(ElementalMasterArchetype.ToReference<BlueprintArchetypeReference>()).ToArray();
            Main.LogPatch("Added", ElementalMasterArchetype);
        }
    }
}
