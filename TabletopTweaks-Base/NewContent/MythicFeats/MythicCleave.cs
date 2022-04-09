using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicFeats {
    class MythicCleave {
        public static void AddMythicCleave() {
            var CleaveFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d809b6c4ff2aaff4fa70d712a70f7d7b");
            var CleaveMythicFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "CleaveMythicFeature", bp => {
                bp.m_Icon = CleaveFeature.m_Icon;
                bp.SetName(TTTContext, "Cleave (Mythic)");
                bp.SetDescription(TTTContext, "You can cleave any foe within your reach.\n" +
                    "Whenever you use Cleave or Cleaving Finish, your attacks can be made against a foe that is within your reach, " +
                    "but not adjacent to the foe you attacked. You can’t use this ability to attack a foe more than once per round.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddPrerequisiteFeature(CleaveFeature);
            });
            if (Main.TTTContext.Fixes.Feats.IsDisabled("Cleave")) { return; }
            if (TTTContext.AddedContent.MythicFeats.IsDisabled("MythicCleave")) { return; }
            FeatTools.AddAsMythicFeat(CleaveMythicFeature);
        }
    }
}
