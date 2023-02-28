using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class AbundantChallenge {
        public static void AddAbundantChallenge() {
            var CavalierChallengeAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("9d5d58ff40e39ff4681670463abe99c9");
            var CavalierChallengeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("dc77cd2ad52cb0e43bb88b264d7af648");
            var CavalierChallengeResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("672e8c9c98db1df4aa66676a66036e71");

            var AbundantChallengeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AbundantChallengeFeature", bp => {
                bp.SetName(TTTContext, "Abundant Challenge");
                bp.SetDescription(TTTContext, "You've learned a way to increase the number of uses of your Challenge ability.\n" +
                    "Benefit: You can use Challenge a number of additional times per day equal to half your mythic rank.");
                bp.m_Icon = CavalierChallengeAbility.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddComponent<IncreaseResourceAmountBySharedValue>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    c.m_Resource = CavalierChallengeResource;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_UseMin = true;
                    c.m_Min = 1;
                });
                bp.AddPrerequisiteFeature(CavalierChallengeFeature);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("AbundantChallenge")) { return; }
            FeatTools.AddAsMythicAbility(AbundantChallengeFeature);
        }
    }
}
