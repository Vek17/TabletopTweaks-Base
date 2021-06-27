using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Utilities {
    static class FeatTools {
        public static void AddToFeatList(BlueprintFeature feature) {
            var BasicFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");
            BasicFeatSelection.AddFeatures(feature);
        }
        public static BlueprintFeature CreateSkillFeat(StatType skill1, StatType skill2, Action<BlueprintFeature> init = null) {
            var SkillFeat = Helpers.Create<BlueprintFeature>(bp => {
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                    c.Stat = skill1;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus,
                        Value = 2
                    };
                }));
                bp.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                    c.Stat = skill2;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default,
                        Value = 2
                    };
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.BaseStat;
                    c.m_Stat = skill1;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 2
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 100,
                            ProgressionValue = 4
                        }
                    };
                    c.m_StepLevel = 3;
                    c.m_Min = 10;
                    c.m_Max = 20;
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.BaseStat;
                    c.m_Stat = skill2;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 2
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 100,
                            ProgressionValue = 4
                        }
                    };
                    c.m_StepLevel = 3;
                    c.m_Min = 10;
                    c.m_Max = 20;
                }));
            });
            init?.Invoke(SkillFeat);
            return SkillFeat;
        }
    }
}
