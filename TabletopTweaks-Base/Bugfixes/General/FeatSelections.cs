using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.General {
    static class FeatSelections {
        [PatchBlueprintsCacheInit(Priority.Last)]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                FixFeatSelections();

                static void FixFeatSelections() {
                    if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("FeatSelections")) { return; }

                    TTTContext.Logger.LogHeader("Patching Feat Selections");
                    var allFeats = FeatTools.Selections.BasicFeatSelection.m_AllFeatures;
                    foreach (var feat in allFeats.Where(f => f.Get() is not null)) {
                        FeatTools.Selections.FeatSelections
                            .Where(selection => feat.Get().HasGroup(selection.Group) || feat.Get().HasGroup(selection.Group2))
                            .ForEach(selection => AddFeaturesNoSort(selection, feat));
                    }
                    FeatTools.Selections.FeatSelections.ForEach(x => SortFeatures(x));
                    var ArcaneDiscoverySelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "ArcaneDiscoverySelection");
                    FeatTools.Selections.LoremasterWizardFeatSelection.RemoveFeatures(ArcaneDiscoverySelection);
                }
            }

            private static void AddFeaturesNoSort(BlueprintFeatureSelection selection, params BlueprintFeatureReference[] features) {
                foreach (BlueprintFeatureReference value in features) {
                    if (!Enumerable.Contains(selection.m_AllFeatures, value)) {
                        selection.m_AllFeatures = selection.m_AllFeatures.AppendToArray(value);
                    }

                    if (!Enumerable.Contains(selection.m_Features, value)) {
                        selection.m_Features = selection.m_Features.AppendToArray(value);
                    }
                }
            }

            private static void SortFeatures(BlueprintFeatureSelection selection) {
                selection.m_AllFeatures = selection.m_AllFeatures.OrderBy(feature => feature?.Get()?.Name ?? feature?.Get()?.name).ToArray();
                selection.m_Features = selection.m_Features.OrderBy(feature => feature?.Get()?.Name ?? feature?.Get()?.name).ToArray();
            }
        }
    }
}
