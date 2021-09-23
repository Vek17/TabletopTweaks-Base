using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    var allFeats = FeatTools.Selections.BasicFeatSelection.m_AllFeatures;
                    Main.LogHeader("Patching Feat Selections");
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
