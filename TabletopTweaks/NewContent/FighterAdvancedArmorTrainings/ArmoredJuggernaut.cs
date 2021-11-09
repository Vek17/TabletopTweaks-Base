using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedArmorTrainings {
    class ArmoredJuggernaut {
        public static void AddArmoredJuggernaut() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");

            var ArmoredJuggernautLightEffect = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredJuggernautLightEffect", bp => {
                bp.SetName("304ee70cc3e94c7589e1df283b03f41c", "Armored Juggernaut Effect");
                bp.SetDescription("d2a3a146a5544d1eb17186f2d8c44621", "Armored Juggernaut");
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
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 11;
                    c.m_StepLevel = 4;
                    c.m_Max = 1;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                });
            });
            var ArmoredJuggernautMediumEffect = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredJuggernautMediumEffect", bp => {
                bp.SetName("5667d2e55b3d480787e0b95baf400b75", "Armored Juggernaut Effect");
                bp.SetDescription("2d341542313b456db6c4d2bf0a01f897", "Armored Juggernaut");
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
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 2;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                });
            });
            var ArmoredJuggernautHeavyEffect = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredJuggernautHeavyEffect", bp => {
                bp.SetName("588679264611485d9c9f4e0c0b20d69c", "Armored Juggernaut Effect");
                bp.SetDescription("b6ae66099aef48159d9076d6b6736a19", "Armored Juggernaut");
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
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 7;
                    c.m_StepLevel = 4;
                    c.m_Max = 2;
                    c.m_Min = 1;
                    c.m_UseMax = true;
                    c.m_Class = new BlueprintCharacterClassReference[] { FighterClass.ToReference<BlueprintCharacterClassReference>() };
                });
            });

            var ArmoredJuggernautDRProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("ArmoredJuggernautDRProperty", bp => {
                bp.AddComponent(Helpers.Create<ArmoredJuggernautDRProperty>());
            });

            var ArmoredJuggernautFeature = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredJuggernautFeature", bp => {
                bp.SetName("7cc8106a370647bd98f477e38c2276f0", "Armored Juggernaut");
                bp.SetDescription("30403b099bed4cf79d1619ad891aecd3", "When wearing heavy armor, the fighter gains DR 1/—. At 7th level, the fighter gains DR 1/— when wearing medium armor, " +
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

            if (ModSettings.AddedContent.FighterAdvancedArmorTraining.IsDisabled("ArmoredJuggernaut")) { return; }
            AdvancedArmorTraining.AddToAdvancedArmorTrainingSelection(ArmoredJuggernautFeature);
        }
    }
}
