using UnityEngine;
using HarmonyLib;
using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Utility;

namespace TabletopTweaks {
    class UnitAdjustments {
        public static BlueprintScriptableObject[] blueprints = null;

        public static BlueprintScriptableObject[] GetBlueprints() {
            var bundle = (AssetBundle)AccessTools.Field(typeof(ResourcesLibrary), "s_BlueprintsBundle")
                .GetValue(null);
            return bundle.LoadAllAssets<BlueprintScriptableObject>();
        }

        public static void patchDemonSubtypes() {
            if (blueprints == null) {
                blueprints = GetBlueprints();
            }

            BlueprintFeature subtypeDemon = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("dc960a234d365cb4f905bdc5937e623a");
            BlueprintFeature subtypeEvil  = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("5279fc8380dd9ba419b4471018ffadd1");
            blueprints.OfType<BlueprintUnit>()
                .Where(bp => Array.Exists(bp.m_AddFacts, e => e.GetBlueprint() == subtypeDemon) 
                         && !Array.Exists(bp.m_AddFacts, e => e.GetBlueprint() == subtypeEvil))
                .ForEach(bp => {
                    bp.m_AddFacts = bp.m_AddFacts.AddItem(subtypeEvil.ToReference<BlueprintUnitFactReference>()).ToArray();
                    Main.Log($"Added SubtypeEvil: {bp.LocalizedName.name}");
                });
        }
    }
}
