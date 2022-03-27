using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class ExtraDiscovery {
        public static void AddExtraDiscovery() {
            var DiscoverySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("cd86c437488386f438dcc9ae727ea2a6");
            var VivsectionistDiscoverySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("67f499218a0e22944abab6fe1c9eaeee");

            var ExtraDiscovery = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraDiscovery", DiscoverySelection, bp => {
                bp.SetName(TTTContext, "Extra Discovery");
                bp.SetDescription(TTTContext, "You gain one additional discovery. You must meet all of the prerequisites for this discovery." +
                    "\nYou can gain Extra Discovery multiple times.");
            });
            var ExtraDiscoveryVivsectionist = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraDiscoveryVivsectionist", VivsectionistDiscoverySelection, bp => {
                bp.SetName(TTTContext, "Extra Medical Discovery");
                bp.SetDescription(TTTContext, "You gain one additional medical discovery. You must meet all of the prerequisites for this discovery." +
                    "\nYou can gain Extra Medical Discovery multiple times.");
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ExtraDiscovery")) { return; }
            FeatTools.AddAsFeat(ExtraDiscovery);
            FeatTools.AddAsFeat(ExtraDiscoveryVivsectionist);
        }
    }
}
