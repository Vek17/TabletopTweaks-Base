using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class MagicalAptitude {
        public static void AddMagicalAptitude() {
            var MagicalAptitude = FeatTools.CreateSkillFeat(TTTContext, "MagicalAptitude", StatType.SkillKnowledgeArcana, StatType.SkillUseMagicDevice, bp => {
                bp.SetName(TTTContext, "Magical Aptitude");
                bp.SetDescription(TTTContext, "You are skilled at spellcasting and using magic items." +
                    "\nYou get a +2 bonus on Knowledge (Arcana) and " +
                    "Use Magic Device skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill.");
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("MagicalAptitude")) { return; }
            FeatTools.AddAsFeat(MagicalAptitude);
        }
    }
}
