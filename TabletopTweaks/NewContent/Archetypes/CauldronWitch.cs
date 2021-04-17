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
            [HarmonyPriority(Priority.First)]
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
