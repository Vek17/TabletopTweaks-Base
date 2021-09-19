using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ExtraSlayerTalent {
        public static void AddExtraSlayerTalent() {
            var SlayerTalentSelection2 = Resources.GetBlueprint<BlueprintFeatureSelection>("04430ad24988baa4daa0bcd4f1c7d118");
            var RangerStyleSelection2 = Resources.GetBlueprint<BlueprintFeatureSelection>("c6d0da9124735a44f93ac31df803b9a9");

            var ExtraSlayerTalent = FeatTools.CreateExtraSelectionFeat("ExtraSlayerTalent", SlayerTalentSelection2, bp => {
                bp.SetName("Extra Slayer Talent");
                bp.SetDescription("You gain one additional slayer talent. You must meet the prerequisites for this slayer talent." +
                    "\nYou can take this feat multiple times. Each time you do, you gain another slayer talent.");
                bp.RemoveFeatures(RangerStyleSelection2);
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraSlayerTalent")) { return; }
            FeatTools.AddAsFeat(ExtraSlayerTalent);
        }
    }
}
