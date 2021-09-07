using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class SelfSufficent {
        public static void AddSelfSufficent() {
            // Icon: Spell Focus? Alertness?
            var SelfSufficent = FeatTools.CreateSkillFeat("Self-Sufficent", StatType.SkillLoreNature, StatType.SkillLoreReligion, bp => {
                bp.SetName("Self-Sufficent");
                bp.SetDescription("You know how to get along in the wild and how to effectively treat wounds." +
                    "\nYou get a +2 bonus on Lore (Nature) and " +
                    "Lore (Religion) skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill.");
            });
            if (ModSettings.AddedContent.Feats.DisableAll || !ModSettings.AddedContent.Feats.Enabled["Self-Sufficent"]) { return; }
            FeatTools.AddAsFeat(SelfSufficent);
        }
    }
}
