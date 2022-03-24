using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    static class AbundantLayOnHands {
        public static void AddAbundantLayOnHands() {
            var LayOnHandsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("858a3689c285c844d9e6ce278e686491");
            var LayOnHandsResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9dedf41d995ff4446a181f143c3db98c");

            var AbundantLayOnHandsFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AbundantLayOnHandsFeature", bp => {
                bp.SetName(TTTContext, "Abundant Lay On Hands");
                bp.SetDescription(TTTContext, "You've learned a way to increase the number of uses of your Lay on Hands ability.\n" +
                    "Benefit: You can use Lay On Hands a number of additional times per day equal to your mythic rank.");
                bp.m_Icon = LayOnHandsFeature.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddComponent<IncreaseResourceAmountBySharedValue>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.m_Resource = LayOnHandsResource;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddPrerequisiteFeature(LayOnHandsFeature);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("AbundantLayOnHands")) { return; }
            FeatTools.AddAsMythicAbility(AbundantLayOnHandsFeature);
        }
    }
}
