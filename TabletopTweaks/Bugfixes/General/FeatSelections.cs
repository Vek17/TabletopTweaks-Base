using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.General {
    static class FeatSelections {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        [HarmonyPriority(Priority.Last)]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                FixFeatSelections();

                static void FixFeatSelections() {
                    if (ModSettings.Fixes.BaseFixes.IsDisabled("FeatSelections")) { return; }

                    Main.LogHeader("Patching Feat Selections");
                    var allFeats = FeatTools.Selections.BasicFeatSelection.m_AllFeatures;
                    foreach (var feat in allFeats) {
                        FeatTools.Selections.FeatSelections
                            .Where(selection => feat.Get().HasGroup(selection.Group) || feat.Get().HasGroup(selection.Group2))
                            .ForEach(selection => selection.AddFeatures(feat));
                    }
                }
            }
        }
    }
}
