using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class ChainChallenge {
        public static void AddChainChallenge() {
            var CavalierChallengeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("dc77cd2ad52cb0e43bb88b264d7af648");
            var CavalierChallengeBuffTarget = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4f0218323ad379248b69de8a9501159f");
            var CavalierChallengeAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("9d5d58ff40e39ff4681670463abe99c9");
            var CavalierKnightsChallengeBuffTarget = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("6f6eb1b83de988143ad3ed6c6e0a65de");
            var CavalierKnightsChallengeAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("e9ebf785e2dc1c341b333a763cf8fb52");

            var ChainChallengeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ChainChallengeFeature", bp => {
                bp.SetName(TTTContext, "Chain Challenge");
                bp.SetDescription(TTTContext, "You feed off the rush of victory over your enemies, and channel that fervor into battle.\n" +
                    "Benefit(s): When the target of your challenge ability is killed or knocked unconscious, you can declare a new challenge on the nearest target within 30 feet as a free action.\n" +
                    "If you declare a new challenge using this feat, it doesn’t count against your total daily uses of challenge. " +
                    "You can chain together a number of challenges beyond the first equal to your Charisma bonus (minimum 1).");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddComponent<ChainChallengeComponent>(c => {
                    c.m_CavalierChallengeAbility = CavalierChallengeAbility;
                    c.m_CavalierChallengeBuff = CavalierChallengeBuffTarget;
                    c.m_KnightsChallengeAbility = CavalierKnightsChallengeAbility;
                    c.m_KnightsChallengeBuff = CavalierKnightsChallengeBuffTarget;
                    c.TriggerCount = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
                    c.m_Stat = StatType.Charisma;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMin = true;
                    c.m_Min = 1;
                });
                bp.AddPrerequisite<PrerequisiteCharacterLevel>(c => {
                    c.Level = 7;
                });
                bp.AddPrerequisiteFeature(CavalierChallengeFeature);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("AbundantChallenge")) { return; }
            FeatTools.AddAsFeat(ChainChallengeFeature);
        }
    }
}
