using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedArmorTrainings {
    class ArmoredConfidence {
        public static void AddArmoredConfidence() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");

            var ArmoredConfidenceLightEffect = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredConfidenceLightEffect", bp => {
                bp.SetName("Armored Confidence Effect");
                bp.SetDescription("Armored Confidence");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.CheckIntimidate;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.CheckIntimidate;
                    c.Value = 1;
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    //c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    AdvancedArmorTraining.SetArmorTrainingProgressionConfig(c);
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 4;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    //c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                }));
            });

            var ArmoredConfidenceMediumEffect = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredConfidenceMediumEffect", bp => {
                bp.SetName("Armored Confidence Effect");
                bp.SetDescription("Armored Confidence");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.CheckIntimidate;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.CheckIntimidate;
                    c.Value = 2;
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    //c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    AdvancedArmorTraining.SetArmorTrainingProgressionConfig(c);
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 4;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    //c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                }));
            });
            var ArmoredConfidenceHeavyEffect = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredConfidenceHeavyEffect", bp => {
                bp.SetName("Armored Confidence Effect");
                bp.SetDescription("Armored Confidence");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.CheckIntimidate;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.CheckIntimidate;
                    c.Value = 3;
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    //c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    AdvancedArmorTraining.SetArmorTrainingProgressionConfig(c);
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 4;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    //c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                }));
            });
            var ArmoredConfidenceFeature = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredConfidenceFeature", bp => {
                bp.SetName("Armored Confidence");
                bp.SetDescription("While wearing armor, the fighter gains a bonus on Intimidate checks based upon the type of armor he is wearing: " +
                    "+1 for light armor, +2 for medium armor, or +3 for heavy armor. This bonus increases by 1 at 7th level and every 4 fighter " +
                    "levels thereafter, to a maximum of +4 at 19th level.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<ArmorFeatureUnlock>(c => {
                    c.NewFact = ArmoredConfidenceLightEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Light };
                }));
                bp.AddComponent(Helpers.Create<ArmorFeatureUnlock>(c => {
                    c.NewFact = ArmoredConfidenceMediumEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Medium };
                }));
                bp.AddComponent(Helpers.Create<ArmorFeatureUnlock>(c => {
                    c.NewFact = ArmoredConfidenceHeavyEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy };
                }));
            });

            if (ModSettings.AddedContent.FighterAdvancedArmorTraining.IsDisabled("ArmoredConfidence")) { return; }
            AdvancedArmorTraining.AddToAdvancedArmorTrainingSelection(ArmoredConfidenceFeature);
        }
    }
}
