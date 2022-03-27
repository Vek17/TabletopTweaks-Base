using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class ExtraSlayerTalent {
        public static void AddExtraSlayerTalent() {
            var SlayerTalentSelection2 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("04430ad24988baa4daa0bcd4f1c7d118");
            var SlayerTalentSelection6 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("43d1b15873e926848be2abf0ea3ad9a8");
            var SlayerTalentSelection10 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("913b9cf25c9536949b43a2651b7ffb66");
            var RangerStyleSelection2 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c6d0da9124735a44f93ac31df803b9a9");

            var ExtraSlayerTalent = FeatTools.CreateExtraSelectionFeat(TTTContext, "ExtraSlayerTalent", SlayerTalentSelection2, bp => {
                bp.SetName(TTTContext, "Extra Slayer Talent");
                bp.SetDescription(TTTContext, "You gain one additional slayer talent. You must meet the prerequisites for this slayer talent." +
                    "\nYou can take this feat multiple times. Each time you do, you gain another slayer talent.");
                bp.RemoveFeatures(RangerStyleSelection2);
                bp.RemoveComponents<PrerequisiteFeature>();
                bp.AddPrerequisiteFeaturesFromList(1,
                    SlayerTalentSelection2,
                    SlayerTalentSelection6,
                    SlayerTalentSelection10
                );
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ExtraSlayerTalent")) { return; }
            FeatTools.AddAsFeat(ExtraSlayerTalent);
        }
    }
}
