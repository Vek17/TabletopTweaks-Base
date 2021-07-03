using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class SelfSufficent {
        public static void AddSelfSufficent() {
            // Icon: Spell Focus? Alertness?
            var SelfSufficent = FeatTools.CreateSkillFeat(StatType.SkillLoreNature, StatType.SkillLoreReligion, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("Self-Sufficent");
                bp.name = "Self-Sufficent";
                bp.SetName("Self-Sufficent");
                bp.SetDescription("You know how to get along in the wild and how to effectively treat wounds." +
                    "\nYou get a +2 bonus on {g|Encyclopedia:Lore_Nature}Lore (Nature){/g} and " +
                    "{g|Encyclopedia:Lore_Religion}Lore (Religion){/g} skill checks. If you have 10 or more ranks in one of these skills," +
                    " the bonus increases to +4 for that skill.");
            });
            Resources.AddBlueprint(SelfSufficent);
            if (ModSettings.AddedContent.Feats.DisableAll || !ModSettings.AddedContent.Feats.Enabled["Self-Sufficent"]) { return; }
            FeatTools.AddToFeatList(SelfSufficent);
        }
    }
}
