using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.FighterAdvancedArmorTrainings {
    class ArmoredJuggernaut {
        public static void AddArmoredJuggernaut() {
            var FighterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var FighterArmorTrainingProperty = BlueprintTools.GetModBlueprintReference<BlueprintUnitPropertyReference>(TTTContext, "FighterArmorTrainingProperty");

            var ArmoredJuggernautLightEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArmoredJuggernautLightEffect", bp => {
                bp.SetName(TTTContext, "Armored Juggernaut Effect");
                bp.SetDescription(TTTContext, "Armored Juggernaut");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent<AddDamageResistancePhysical>(c => {
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
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_CustomProperty = FighterArmorTrainingProperty;
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 11;
                    c.m_StepLevel = 4;
                    c.m_Max = 1;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                });
            });
            var ArmoredJuggernautMediumEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArmoredJuggernautMediumEffect", bp => {
                bp.SetName(TTTContext, "Armored Juggernaut Effect");
                bp.SetDescription(TTTContext, "Armored Juggernaut");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Material = PhysicalDamageMaterial.Adamantite;
                    c.MinEnhancementBonus = 1;
                    c.Alignment = DamageAlignment.Good;
                    c.Reality = DamageRealityType.Ghost;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                    c.Pool = new ContextValue { };
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_CustomProperty = FighterArmorTrainingProperty;
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 2;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                });
            });
            var ArmoredJuggernautHeavyEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArmoredJuggernautHeavyEffect", bp => {
                bp.SetName(TTTContext, "Armored Juggernaut Effect");
                bp.SetDescription(TTTContext, "Armored Juggernaut");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Material = PhysicalDamageMaterial.Adamantite;
                    c.MinEnhancementBonus = 1;
                    c.Alignment = DamageAlignment.Good;
                    c.Reality = DamageRealityType.Ghost;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                    c.Pool = new ContextValue { };
                });
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Material = PhysicalDamageMaterial.Adamantite;
                    c.MinEnhancementBonus = 1;
                    c.Alignment = DamageAlignment.Good;
                    c.Reality = DamageRealityType.Ghost;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Simple,
                        Value = 1
                    };
                    c.Pool = new ContextValue { };
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_CustomProperty = FighterArmorTrainingProperty;
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 2;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                });
            });

            var ArmoredJuggernautDRProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(TTTContext, "ArmoredJuggernautDRProperty", bp => {
                bp.AddComponent<ArmoredJuggernautDRProperty>(c => {
                    c.FighterArmorTrainingProperty = FighterArmorTrainingProperty;
                });
            });

            var ArmoredJuggernautFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArmoredJuggernautFeature", bp => {
                bp.SetName(TTTContext, "Armored Juggernaut");
                bp.SetDescription(TTTContext, "When wearing heavy armor, the fighter gains DR 1/—. At 7th level, the fighter gains DR 1/— when wearing medium armor, " +
                    "and DR 2/— when wearing heavy armor. At 11th level, the fighter gains DR 1/— when wearing light armor, DR 2/— when wearing medium armor, " +
                    "and DR 3/— when wearing heavy armor. The DR from this ability stacks with other DR/-");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.AddComponent<AddDamageResistancePhysical>(c => {
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = ArmoredJuggernautDRProperty.ToReference<BlueprintUnitPropertyReference>()
                    };
                    c.m_CheckedFactMythic = BlueprintReferenceBase.CreateTyped<BlueprintUnitFactReference>(null);
                    c.m_WeaponType = BlueprintReferenceBase.CreateTyped<BlueprintWeaponTypeReference>(null);
                    c.Pool = new ContextValue { };
                });
                bp.AddComponent<RecalculateOnEquipmentChange>();
            });

            if (TTTContext.AddedContent.FighterAdvancedArmorTraining.IsDisabled("ArmoredJuggernaut")) { return; }
            AdvancedArmorTraining.AddToAdvancedArmorTrainingSelection(ArmoredJuggernautFeature);
        }
    }
}
