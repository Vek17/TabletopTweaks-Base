using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ShingleRunner {
        public static void AddShingleRunner() {
            var ShingleRunner = FeatTools.CreateSkillFeat(StatType.SkillAthletics, StatType.SkillMobility, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("ShingleRunner");
                bp.name = "ShingleRunner";
                bp.SetName("Shingle Runner");
                bp.SetDescriptionTagged("Many of those who dwell among the rooftops become skillful at making bounding " +
                    "leaps and clambering up steep surfaces, and learn how to land more safely when they fall." +
                    "\nYou get a +2 bonus on Athletics and " +
                    "Mobility skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill.");
            });
            Resources.AddBlueprint(ShingleRunner);
            if (ModSettings.AddedContent.Feats.DisableAll || !ModSettings.AddedContent.Feats.Enabled["ShingleRunner"]) { return; }
            FeatTools.AddAsFeat(ShingleRunner);
        }
    }
}
