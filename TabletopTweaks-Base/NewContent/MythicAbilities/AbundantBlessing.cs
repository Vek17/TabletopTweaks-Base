using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    class AbundantBlessing {
        public static void AddAbundantBlessing() {
            var BlessingSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("6d9dcc2a59210a14891aeedb09d406aa");
            var BlessingResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("d128a6332e4ea7c4a9862b9fdb358cca");

            var AbundantBlessingFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AbundantBlessingFeature", bp => {
                bp.SetName(TTTContext, "Abundant Blessing");
                bp.SetDescription(TTTContext, "You've learned a way to increase the number of uses of your Blessing ability.\n" +
                    "Benefit: You can use Blessings a number of additional times per day equal to your mythic rank.");
                bp.m_Icon = BlessingSelection.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddComponent<IncreaseResourceAmountBySharedValue>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.m_Resource = BlessingResource;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddPrerequisiteFeature(BlessingSelection);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("AbundantBlessing")) { return; }
            FeatTools.AddAsMythicAbility(AbundantBlessingFeature);
        }
    }
}
