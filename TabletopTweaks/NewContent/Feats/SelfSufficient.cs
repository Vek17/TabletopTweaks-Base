using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class SelfSufficient {
        public static void AddSelfSufficient() {
            // Icon: Spell Focus? Alertness?
            var SelfSufficent = FeatTools.CreateSkillFeat("Self-Sufficient", StatType.SkillLoreNature, StatType.SkillLoreReligion, bp => {
                bp.SetName("6ab187ece2734316983f1e2f444104a5", "Self-Sufficient");
                bp.SetDescription("0b84463dc3d2477684af9cef797e377f", "You know how to get along in the wild and how to effectively treat wounds." +
                    "\nYou get a +2 bonus on Lore (Nature) and " +
                    "Lore (Religion) skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill.");
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("Self-Sufficient")) { return; }
            FeatTools.AddAsFeat(SelfSufficent);
        }
    }
}
