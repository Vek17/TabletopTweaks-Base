using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedArmorTrainings {
    class ArmoredJuggernaut {
        public static void AddArmoredJuggernaut() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");

            var ArmoredJuggernautLightEffect = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredJuggernautLightEffect", bp => {
                bp.SetName("Armored Juggernaut Effect");
                bp.SetDescription("Armored Juggernaut");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<AddDamageResistancePhysical>(c => {
                    c.Material = PhysicalDamageMaterial.Adamantite;
                    c.MinEnhancementBonus = 1;
                    c.Alignment = DamageAlignment.Good;
                    c.Reality = DamageRealityType.Ghost;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                    c.Pool = new ContextValue {
                        Value = 12
                    };
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    //c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    AdvancedArmorTraining.SetArmorTrainingProgressionConfig(c);
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 11;
                    c.m_StepLevel = 4;
                    c.m_Max = 1;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    //c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                }));
            });

            var ArmoredJuggernautMediumEffect = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredJuggernautMediumEffect", bp => {
                bp.SetName("Armored Juggernaut Effect");
                bp.SetDescription("Armored Juggernaut");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<AddDamageResistancePhysical>(c => {
                    c.Material = PhysicalDamageMaterial.Adamantite;
                    c.MinEnhancementBonus = 1;
                    c.Alignment = DamageAlignment.Good;
                    c.Reality = DamageRealityType.Ghost;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                    c.Pool = new ContextValue { };
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    //c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    AdvancedArmorTraining.SetArmorTrainingProgressionConfig(c);
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 2;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    //c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                }));
            });
            var ArmoredJuggernautHeavyEffect = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredJuggernautHeavyEffect", bp => {
                bp.SetName("Armored Juggernaut Effect");
                bp.SetDescription("Armored Juggernaut");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<AddDamageResistancePhysical>(c => {
                    c.Material = PhysicalDamageMaterial.Adamantite;
                    c.MinEnhancementBonus = 1;
                    c.Alignment = DamageAlignment.Good;
                    c.Reality = DamageRealityType.Ghost;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                    c.Pool = new ContextValue { };
                }));
                bp.AddComponent(Helpers.Create<AddDamageResistancePhysical>(c => {
                    c.Material = PhysicalDamageMaterial.Adamantite;
                    c.MinEnhancementBonus = 1;
                    c.Alignment = DamageAlignment.Good;
                    c.Reality = DamageRealityType.Ghost;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Simple,
                        Value = 1
                    };
                    c.Pool = new ContextValue { };
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    //c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    AdvancedArmorTraining.SetArmorTrainingProgressionConfig(c);
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 2;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    //c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                }));
            });
            var ArmoredJuggernautFeature = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredJuggernautFeature", bp => {
                bp.SetName("Armored Juggernaut");
                bp.SetDescription("When wearing heavy armor, the fighter gains DR 1/—. At 7th level, the fighter gains DR 1/— when wearing medium armor, " +
                    "and DR 2/— when wearing heavy armor. At 11th level, the fighter gains DR 1/— when wearing light armor, DR 2/— when wearing medium armor, " +
                    "and DR 3/— when wearing heavy armor. The DR from this ability stacks with other DR/-");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent(Helpers.Create<ArmorFeatureUnlock>(c => {
                    c.NewFact = ArmoredJuggernautLightEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Light };
                }));
                bp.AddComponent(Helpers.Create<ArmorFeatureUnlock>(c => {
                    c.NewFact = ArmoredJuggernautMediumEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Medium };
                }));
                bp.AddComponent(Helpers.Create<ArmorFeatureUnlock>(c => {
                    c.NewFact = ArmoredJuggernautHeavyEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy };
                }));
            });

            if (ModSettings.AddedContent.FighterAdvancedArmorTraining.IsDisabled("ArmoredJuggernaut")) { return; }
            AdvancedArmorTraining.AddToAdvancedArmorTrainingSelection(ArmoredJuggernautFeature);
        }
    }
}
