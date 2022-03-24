using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class ExtraHex {
        public static void AddExtraHex() {
            var ShamanHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("4223fe18c75d4d14787af196a04e14e7");
            var WitchHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");

            var ExtraHexWitch = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraHexWitch", WitchHexSelection, bp => {
                bp.SetName(TTTContext, "Extra Hex (Witch)");
                bp.SetDescription(TTTContext, "You gain one additional hex. You must meet the prerequisites for this hex." +
                    "\nYou can take this feat multiple times. Each time you do, you gain another hex.");
            });
            var ExtraHexShaman = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraHexShaman", ShamanHexSelection, bp => {
                bp.SetName(TTTContext, "Extra Hex (Shaman)");
                bp.SetDescription(TTTContext, "You gain one additional hex. You must meet the prerequisites for this hex." +
                    "If you are a shaman, it must be a hex granted by your spirit rather than one from a wandering spirit." +
                    "\nYou can take this feat multiple times. Each time you do, you gain another hex.");
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ExtraHex")) { return; }
            FeatTools.AddAsFeat(ExtraHexWitch);
            FeatTools.AddAsFeat(ExtraHexShaman);
        }
    }
}
