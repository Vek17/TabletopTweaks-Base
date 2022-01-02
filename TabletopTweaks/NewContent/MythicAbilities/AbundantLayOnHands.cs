using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class AbundantLayOnHands {
        public static void AddAbundantLayOnHands() {
            var LayOnHandsFeature = Resources.GetBlueprint<BlueprintFeature>("858a3689c285c844d9e6ce278e686491");
            var LayOnHandsResource = Resources.GetBlueprintReference<BlueprintAbilityResourceReference>("9dedf41d995ff4446a181f143c3db98c");

            var AbundantLayOnHandsFeature = Helpers.CreateBlueprint<BlueprintFeature>("AbundantLayOnHandsFeature", bp => {
                bp.SetName("Abundant Lay on Hands");
                bp.SetDescription("You've learned a way to increase the number of uses of your Lay on Hands ability.\n" +
                    "Benefit: You can use Lay on Hands a number of additional times per day equal to your mythic rank.");
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

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("AbundantLayOnHands")) { return; }
            FeatTools.AddAsMythicAbility(AbundantLayOnHandsFeature);
        }
    }
}
