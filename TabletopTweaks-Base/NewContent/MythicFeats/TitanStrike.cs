using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicFeats {
    static class TitanStrike {
        public static void AddTitanStrike() {
            var StunningFistOwnerBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d9eaeba5690a7704da8bbf626456a50e");
            var StunningFistOwnerFatigueBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("696b29374599d4141be64e46a91bd09b");
            var StunningFistOwnerSickenedBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4d7da6df5cb3b3940a9d96311a2dc311");
            var StunningFistStaggeredBuff = BlueprintTools.GetModBlueprintReference<BlueprintBuffReference>(TTTContext, "StunningFistStaggeredBuff");
            var StunningFistBlindBuff = BlueprintTools.GetModBlueprintReference<BlueprintBuffReference>(TTTContext, "StunningFistBlindBuff");
            var StunningFistParalyzeBuff = BlueprintTools.GetModBlueprintReference<BlueprintBuffReference>(TTTContext, "StunningFistParalyzeBuff");

            var ImprovedUnarmedStrikeMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("e086a07dae105244291fb11e05d0715f");
            var TitanStrikeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TitanStrikeFeature", bp => {
                bp.m_Icon = ImprovedUnarmedStrikeMythicFeat.m_Icon;
                bp.SetName(TTTContext, "Titan Strike");
                bp.SetDescription(TTTContext, "Your fists can fell titanic foes.\n" +
                    "Your unarmed strike deals damage as if you were one size category larger. You also gain a +1 bonus " +
                    "for each size category that your target is larger than you on the following: bull rush, drag, grapple, " +
                    "overrun, sunder, and trip combat maneuver checks and the DC of your Stunning Fist.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddComponent<TitanStrikeComponent>(c => {
                    c.m_StunningFistBuffs = new BlueprintBuffReference[] {
                        StunningFistOwnerBuff,
                        StunningFistOwnerSickenedBuff,
                        StunningFistOwnerFatigueBuff,
                        StunningFistStaggeredBuff,
                        StunningFistBlindBuff,
                        StunningFistParalyzeBuff
                    };
                });
                bp.AddPrerequisiteFeature(ImprovedUnarmedStrikeMythicFeat);
            });
            if (TTTContext.AddedContent.MythicFeats.IsDisabled("TitanStrike")) { return; }
            FeatTools.AddAsMythicFeat(TitanStrikeFeature);
        }
    }
}
