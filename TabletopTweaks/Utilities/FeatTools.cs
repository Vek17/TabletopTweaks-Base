using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using System.Linq;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Utilities {
    static class FeatTools {
        public static void AddAsFeat(params BlueprintFeature[] features) {
            foreach (var feature in features) {
                AddAsFeat(feature);
            }
        }
        public static void AddAsFeat(BlueprintFeature feature) {
            var BasicFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");
            var ExtraFeatMythicFeat = Resources.GetBlueprint<BlueprintFeatureSelection>("e10c4f18a6c8b4342afe6954bde0587b");
            BasicFeatSelection.AddFeatures(feature);
            ExtraFeatMythicFeat.AddFeatures(feature);
        }
        public static void AddAsRogueTalent(BlueprintFeature feature) {
            var TalentSelections = new BlueprintFeatureSelection[] {
                Resources.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452"), //SylvanTricksterTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("913b9cf25c9536949b43a2651b7ffb66"), //SlayerTalentSelection10
                Resources.GetBlueprint<BlueprintFeatureSelection>("43d1b15873e926848be2abf0ea3ad9a8"), //SlayerTalentSelection6
                Resources.GetBlueprint<BlueprintFeatureSelection>("04430ad24988baa4daa0bcd4f1c7d118"), //SlayerTalentSelection2
                Resources.GetBlueprint<BlueprintFeatureSelection>("d2a8fde8985691045b90e1ec57e3cc57"), //SkaldTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93"), //RogueTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("4b7018b1ed4b27140a5e7adfacaaf9c6"), //LoremasterRogueTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("94e2cd84bf3a8e04f8609fe502892f4f"), //BardTalentSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("40f85fbe8cc35ef4fa96c66e06eeafe8")  //BardTalentSelection
            };
            TalentSelections.ForEach(selection => selection.AddFeatures(feature));
        }
        public static void AddAsArcanistExploit(BlueprintFeature feature) {
            var TalentSelections = new BlueprintFeatureSelection[] {
                Resources.GetBlueprint<BlueprintFeatureSelection>("2ba8a0040e0149e9ae9bfcb01a8ff01d"), //ExploiterExploitSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("b8bf3d5023f2d8c428fdf6438cecaea7"), //ArcanistExploitSelection
            };
            TalentSelections.ForEach(selection => selection.AddFeatures(feature));
        }
        public static void AddAsMagusArcana(BlueprintFeature feature, params BlueprintFeatureSelection[] ignore) {
            var ArcanaSelections = new BlueprintFeatureSelection[] {
                Resources.GetBlueprint<BlueprintFeatureSelection>("e9dc4dfc73eaaf94aae27e0ed6cc9ada"), //MagusArcanaSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("ad6b9cecb5286d841a66e23cea3ef7bf"), //HexcrafterMagusHexArcanaSelection
                Resources.GetBlueprint<BlueprintFeatureSelection>("d4b54d9db4932454ab2899f931c2042c")  //EldritchMagusArcanaSelection;
            };
            ArcanaSelections.Where(selection => !ignore?.Contains(selection) ?? true).ForEach(selection => selection.AddFeatures(feature));
        }
        public static BlueprintFeature CreateSkillFeat(string name, StatType skill1, StatType skill2, Action<BlueprintFeature> init = null) {
            var SkillFeat = Helpers.CreateBlueprint<BlueprintFeature>(name, bp => {
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
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Skills;
                }));
            });
            init?.Invoke(SkillFeat);
            return SkillFeat;
        }

        public static BlueprintFeature CreateExtraResourceFeat(string name, BlueprintAbilityResource resource, int amount, Action<BlueprintFeature> init = null) {
            var extraResourceFeat = Helpers.CreateBlueprint<BlueprintFeature>(name, bp => {
                bp.Ranks = 10;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = resource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = amount;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                });
            });
            init?.Invoke(extraResourceFeat);
            return extraResourceFeat;
        }

        public static BlueprintFeatureSelection CreateExtraSelectionFeat(string name, BlueprintFeatureSelection selection, Action<BlueprintFeatureSelection> init = null) {
            var extraResourceFeat = Helpers.CreateBlueprint<BlueprintFeatureSelection>(name, bp => {
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.m_Features = selection.m_Features.ToArray();
                bp.m_AllFeatures = selection.m_AllFeatures.ToArray();
                bp.Mode = selection.Mode;
                bp.IgnorePrerequisites = selection.IgnorePrerequisites;
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                });
                bp.AddPrerequisiteFeature(selection);
            });
            init?.Invoke(extraResourceFeat);
            return extraResourceFeat;
        }
    }
}
