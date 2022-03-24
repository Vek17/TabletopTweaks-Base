using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class StreetSmarts {
        public static void AddStreetSmarts() {
            var StreetSmarts = FeatTools.CreateSkillFeat(TTTContext, "StreetSmarts", StatType.SkillKnowledgeWorld, StatType.SkillPerception, bp => {
                bp.SetName(TTTContext, "Street Smarts");
                bp.SetDescription(TTTContext, "You are able to navigate the streets and personalities of whatever locale you run across." +
                    "\nYou get a +2 bonus on Knowledge (World) and " +
                    "Perception skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill.");
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("StreetSmarts")) { return; }
            FeatTools.AddAsFeat(StreetSmarts);
        }
    }
}
