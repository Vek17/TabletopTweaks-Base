using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ExtraMercy {
        public static void AddExtraMercy() {
            var SelectionMercy = Resources.GetBlueprint<BlueprintFeatureSelection>("02b187038a8dce545bb34bbfb346428d");

            var ExtraMercy = FeatTools.CreateExtraSelectionFeat("ExtraMercy", SelectionMercy, bp => {
                bp.SetName("Extra Mercy");
                bp.SetDescription("Select one additional mercy for which you qualify. " +
                    "When you use lay on hands to heal damage to one target, it also receives the additional effects of this mercy.");
                bp.AddPrerequisite(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                }));
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraMercy")) { return; }
            FeatTools.AddAsFeat(ExtraMercy);
        }
    }
}
