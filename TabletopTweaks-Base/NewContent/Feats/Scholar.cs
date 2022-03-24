using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class Scholar {
        public static void AddScholar() {
            // Icon: Spell Focus? Alertness?
            var Scholar = FeatTools.CreateSkillFeat(TTTContext, "Scholar", StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, bp => {
                bp.SetName(TTTContext, "Scholar");
                bp.SetDescription(TTTContext, "You have graduated from one of the many colleges, universities, and specialized schools of higher learning scattered throughout the Inner Sea region." +
                    "\nYou get a +2 bonus on Knowledge (Arcana) and " +
                    "Knowledge (World) skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill.");
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("Scholar")) { return; }
            FeatTools.AddAsFeat(Scholar);
        }
    }
}
