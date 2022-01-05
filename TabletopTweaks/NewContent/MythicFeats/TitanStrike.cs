using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicFeats {
    static class TitanStrike {
        public static void AddTitanStrike() {
            var ImprovedUnarmedStrikeMythicFeat = Resources.GetBlueprint<BlueprintFeature>("e086a07dae105244291fb11e05d0715f");
            var TitanStrikeFeature = Helpers.CreateBlueprint<BlueprintFeature>("TitanStrikeFeature", bp => {
                bp.m_Icon = ImprovedUnarmedStrikeMythicFeat.m_Icon;
                bp.SetName("Titan Strike");
                bp.SetDescription("Your fists can fell titanic foes.\n" +
                    "Your unarmed strike deals damage as if you were one size category larger. You also gain a +1 bonus " +
                    "for each size category that your target is larger than you on the following: bull rush, drag, grapple, " +
                    "overrun, sunder, and trip combat maneuver checks and the DC of your Stunning Fist.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddComponent<TitanStrikeComponent>();
                bp.AddPrerequisiteFeature(ImprovedUnarmedStrikeMythicFeat);
            });
            if (ModSettings.AddedContent.MythicFeats.IsDisabled("TitanStrike")) { return; }
            FeatTools.AddAsMythicFeat(TitanStrikeFeature);
        }
    }
}
