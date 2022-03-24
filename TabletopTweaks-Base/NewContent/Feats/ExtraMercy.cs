using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class ExtraMercy {
        public static void AddExtraMercy() {
            var SelectionMercy = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("02b187038a8dce545bb34bbfb346428d");

            var ExtraMercy = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraMercy", SelectionMercy, bp => {
                bp.SetName(TTTContext, "Extra Mercy");
                bp.SetDescription(TTTContext, "Select one additional mercy for which you qualify. " +
                    "When you use lay on hands to heal damage to one target, it also receives the additional effects of this mercy.");
                bp.AddPrerequisites(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                }));
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ExtraMercy")) { return; }
            FeatTools.AddAsFeat(ExtraMercy);
        }
    }
}
