using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
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
                bp.SetName("Extra Discovery");
                bp.SetDescription("You gain one additional discovery. You must meet all of the prerequisites for this discovery." +
                    "\nYou can gain Extra Discovery multiple times.");
            });
            var ExtraDiscoveryVivsectionist = FeatTools.CreateExtraSelectionFeat("ExtraDiscoveryVivsectionist", VivsectionistDiscoverySelection, bp => {
                bp.SetName("Extra Medical Discovery");
                bp.SetDescription("You gain one additional medical discovery. You must meet all of the prerequisites for this discovery." +
                    "\nYou can gain Extra Medical Discovery multiple times.");
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraDiscovery")) { return; }
            FeatTools.AddAsFeat(ExtraDiscovery);
            FeatTools.AddAsFeat(ExtraDiscoveryVivsectionist);
        }
    }
}
