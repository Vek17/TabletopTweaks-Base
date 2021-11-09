using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ExtraHex {
        public static void AddExtraHex() {
            var ShamanHexSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("4223fe18c75d4d14787af196a04e14e7");
            var WitchHexSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");

            var ExtraHexWitch = FeatTools.CreateExtraSelectionFeat("ExtraHexWitch", WitchHexSelection, bp => {
                bp.SetName("e289bad91a6040a584c0c7aad91b9b1b", "Extra Hex (Witch)");
                bp.SetDescription("18d44fe69ed34ac4bd7effbac03e65cf", "You gain one additional hex. You must meet the prerequisites for this hex." +
                    "\nYou can take this feat multiple times. Each time you do, you gain another hex.");
            });
            var ExtraHexShaman = FeatTools.CreateExtraSelectionFeat("ExtraHexShaman", ShamanHexSelection, bp => {
                bp.SetName("940981a4f98146cb888c72f12457708e", "Extra Hex (Shaman)");
                bp.SetDescription("d8ba22b7278b4cc8aa125262af90d461", "You gain one additional hex. You must meet the prerequisites for this hex." +
                    "If you are a shaman, it must be a hex granted by your spirit rather than one from a wandering spirit." +
                    "\nYou can take this feat multiple times. Each time you do, you gain another hex.");
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraHex")) { return; }
            FeatTools.AddAsFeat(ExtraHexWitch);
            FeatTools.AddAsFeat(ExtraHexShaman);
        }
    }
}
