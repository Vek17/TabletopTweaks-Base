using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.NewContent.Archetypes {
    static class CauldronWitch {
        public static void AddCauldrenWitch() {
            if (!Settings.AddedContent.CauldronWitchArchetype) { return; }
            Main.LogHeader("Adding Cauldren Witch");
            var CauldronWitchArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("e0012a7015774e140be217f4a1480b6f");
            var WitchClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("1b9873f1e7bfe5449bc84d03e9c8e3cc");
            WitchClass.m_Archetypes = WitchClass.m_Archetypes.AddItem(CauldronWitchArchetype.ToReference<BlueprintArchetypeReference>()).ToArray();
        }
    }
}
