using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class StreetSmarts {
        public static void AddStreetSmarts() {
            var StreetSmarts = FeatTools.CreateSkillFeat("StreetSmarts", StatType.SkillKnowledgeWorld, StatType.SkillPerception, (System.Action<Kingmaker.Blueprints.Classes.BlueprintFeature>)(bp => {
                bp.SetName("Street Smarts");
                ExtentionMethods.SetDescription(bp, (string)("You are able to navigate the streets and personalities of whatever locale you run across." +
                    "\nYou get a +2 bonus on Knowledge (World) and " +
                    "Perception skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill."));
            }));
            if (ModSettings.AddedContent.Feats.DisableAll || !ModSettings.AddedContent.Feats.Enabled["StreetSmarts"]) { return; }
            FeatTools.AddAsFeat(StreetSmarts);
        }
    }
}
