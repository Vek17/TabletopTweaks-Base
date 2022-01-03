using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class AbundantFervor {
        public static void AddAbundantFervor() {
            var WarpriestFervorBase = Resources.GetBlueprint<BlueprintFeature>("2d5140fd9f19a8e41be32d300eea2e18");
            var WarpriestFervorResource = Resources.GetBlueprintReference<BlueprintAbilityResourceReference>("da0fb35828917f344b1cd72c98b70498");

            var AbundantFervorFeature = Helpers.CreateBlueprint<BlueprintFeature>("AbundantFervorFeature", bp => {
                bp.SetName("Abundant Fervor");
                bp.SetDescription("You've learned a way to increase the number of uses of your Fervor ability.\n" +
                    "Benefit: You can use Fervor a number of additional times per day equal to your mythic rank.");
                bp.m_Icon = WarpriestFervorBase.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddComponent<IncreaseResourceAmountBySharedValue>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.m_Resource = WarpriestFervorResource;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddPrerequisiteFeature(WarpriestFervorBase);
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("AbundantFervor")) { return; }
            FeatTools.AddAsMythicAbility(AbundantFervorFeature);
        }
    }
}
