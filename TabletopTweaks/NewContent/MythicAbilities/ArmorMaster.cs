using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    class ArmorMaster {
        public static void AddArmorMaster() {
            var MythicAbilitySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var ExtraMythicAbilityMythicFeat = Resources.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");
            var icon = AssetLoader.LoadInternal("Feats", "Icon_ArmorMaster.png");

            var ArmorMasterLightFeature = Helpers.CreateBlueprint<BlueprintFeature>("ArmorMasterLightFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName("Armor Master (Light Armor)");
                bp.SetDescription("You don’t take an armor check penalty or incur an arcane spell failure chance when wearing light armor or using any shield. " +
                    "In addition, the maximum Dexterity bonus of light armor doesn’t apply to you.");
                bp.AddComponent<ArcaneArmorProficiency>(c => {
                    c.Armor = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Light,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                });
                bp.AddComponent<IgnoreArmorMaxDexBonus>(c => {
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Light
                    };
                });
                bp.AddComponent<IgnoreArmorCheckPenalty>(c => {
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Light,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                });
            });
            var ArmorMasterMediumFeature = Helpers.CreateBlueprint<BlueprintFeature>("ArmorMasterMediumFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName("Armor Master (Medium Armor)");
                bp.SetDescription("You don’t take an armor check penalty or incur an arcane spell failure chance when wearing medium armor or using any shield. " +
                    "In addition, the maximum Dexterity bonus of medium armor doesn’t apply to you.");
                bp.AddComponent(Helpers.Create<ArcaneArmorProficiency>(c => {
                    c.Armor = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Medium,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                }));
                bp.AddComponent<IgnoreArmorMaxDexBonus>(c => {
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Medium
                    };
                });
                bp.AddComponent<IgnoreArmorCheckPenalty>(c => {
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Medium,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                });
                bp.AddPrerequisite<PrerequisiteFeature>(c => {
                    c.m_Feature = ArmorMasterLightFeature.ToReference<BlueprintFeatureReference>();
                });
            });
            var ArmorMasterHeavyFeature = Helpers.CreateBlueprint<BlueprintFeature>("ArmorMasterHeavyFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName("Armor Master (Heavy Armor)");
                bp.SetDescription("You don’t take an armor check penalty or incur an arcane spell failure chance when wearing heavy armor or using any shield. " +
                    "In addition, the maximum Dexterity bonus of heavy armor doesn’t apply to you.");
                bp.AddComponent<ArcaneArmorProficiency>(c => {
                    c.Armor = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Heavy,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                });
                bp.AddComponent<IgnoreArmorMaxDexBonus>(c => {
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Heavy
                    };
                });
                bp.AddComponent<IgnoreArmorCheckPenalty>(c => {
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Heavy,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                });
                bp.AddPrerequisite(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = ArmorMasterMediumFeature.ToReference<BlueprintFeatureReference>();
                }));
            });
            var ArmorMasterHomebrewFeature = Helpers.CreateBlueprint<BlueprintFeature>("ArmorMasterHomebrewFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName("Armor Master");
                bp.SetDescription("While wearing armor or wielding a shield, you reduce the armor check penalty by 1 per mythic rank " +
                    "and increase the maximum Dexterity bonus allowed by 1 per mythic rank. " +
                    "Additionally you reduce your arcane spell failure chance from armor and sheilds by 5% per mythic rank.");
                bp.AddComponent<ContextMaxDexBonusIncrease>(c => {
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Light,
                        ArmorProficiencyGroup.Medium,
                        ArmorProficiencyGroup.Heavy,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                });
                bp.AddComponent<ContextArmorCheckPenaltyIncrease>(c => {
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Light,
                        ArmorProficiencyGroup.Medium,
                        ArmorProficiencyGroup.Heavy,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                });
                bp.AddComponent<ContextArcaneSpellFailureIncrease>(c => {
                    c.CheckCategory = true;
                    c.Categorys = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Light,
                        ArmorProficiencyGroup.Medium,
                        ArmorProficiencyGroup.Heavy,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield
                    };
                    c.Reduce = true;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<RecalculateOnLevelUp>();
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.MultiplyByModifier;
                    c.m_StepLevel = 5;
                });
            });

            if (ModSettings.AddedContent.MythicAbilities.IsEnabled("ArmorMaster")) {
                FeatTools.AddAsMythicAbility(ArmorMasterLightFeature);
                FeatTools.AddAsMythicAbility(ArmorMasterMediumFeature);
                FeatTools.AddAsMythicAbility(ArmorMasterHeavyFeature);
            }
            if (ModSettings.AddedContent.MythicAbilities.IsEnabled("ArmorMasterHomebrew")) {
                FeatTools.AddAsMythicAbility(ArmorMasterHomebrewFeature);
            }
        }
    }
}
