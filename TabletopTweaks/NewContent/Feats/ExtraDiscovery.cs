using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ExtraDiscovery {
        public static void AddExtraDiscovery() {
            var DiscoverySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("cd86c437488386f438dcc9ae727ea2a6");
            var VivsectionistDiscoverySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("67f499218a0e22944abab6fe1c9eaeee");

            var ExtraDiscovery = FeatTools.CreateExtraSelectionFeat("ExtraDiscovery", DiscoverySelection, bp => {
                bp.SetName("cec19a935b2444c2ae1b8af736e9440c", "Extra Discovery");
                bp.SetDescription("cc0e6c42faa340c5b8ed3221c5b9d9d3", "You gain one additional discovery. You must meet all of the prerequisites for this discovery." +
                    "\nYou can gain Extra Discovery multiple times.");
            });
            var ExtraDiscoveryVivsectionist = FeatTools.CreateExtraSelectionFeat("ExtraDiscoveryVivsectionist", VivsectionistDiscoverySelection, bp => {
                bp.SetName("70f504147e484530b9103fc3f67adabb", "Extra Medical Discovery");
                bp.SetDescription("ee7128652e1c46f68338e0a0c23f2830", "You gain one additional medical discovery. You must meet all of the prerequisites for this discovery." +
                    "\nYou can gain Extra Medical Discovery multiple times.");
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraDiscovery")) { return; }
            FeatTools.AddAsFeat(ExtraDiscovery);
            FeatTools.AddAsFeat(ExtraDiscoveryVivsectionist);
        }
    }
}
