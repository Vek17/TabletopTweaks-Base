using HarmonyLib;
using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Utility;

namespace TabletopTweaks {
    static class UnitAdjustments {
        public static void patchDemonSubtypes() {
            BlueprintFeature subtypeDemon = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("dc960a234d365cb4f905bdc5937e623a");
            BlueprintFeature subtypeEvil = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("5279fc8380dd9ba419b4471018ffadd1");
            Resources.Blueprints.OfType<BlueprintUnit>()
                .Where(bp => Array.Exists(bp.m_AddFacts, e => e.GetBlueprint() == subtypeDemon)
                         && !Array.Exists(bp.m_AddFacts, e => e.GetBlueprint() == subtypeEvil))
                .ForEach(bp => {
                    bp.m_AddFacts = bp.m_AddFacts.AddItem(subtypeEvil.ToReference<BlueprintUnitFactReference>()).ToArray();
                    Main.Log($"Added SubtypeEvil: {bp.LocalizedName.name}");
                });
        }
    }
}
