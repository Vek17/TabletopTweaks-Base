using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    class ArmorMaster {
        public static void AddArmorMaster() {
            var MythicAbilitySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var ExtraMythicAbilityMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");
            var icon = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ArmorMaster.png");

            var ArmorMasterSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ArmorMasterSelection", bp => {
                bp.SetName(TTTContext, "Armor Master (Tabletop)");
                bp.SetDescription(TTTContext, "You don’t take an armor check penalty or incur an arcane spell failure chance when wearing the selected armor types or using any shield. " +
                    "In addition, the maximum Dexterity bonus of the selected armor type doesn’t apply to you.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Group = FeatureGroup.MythicAbility;
                bp.Ranks = 1;
                bp.Mode = SelectionMode.OnlyNew;
                bp.m_Icon = icon;
            });
            var ArmorMasterLightFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArmorMasterLightFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName(TTTContext, "Armor Master (Light Armor)");
                bp.SetDescription(TTTContext, "You don’t take an armor check penalty or incur an arcane spell failure chance when wearing light armor or using any shield. " +
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
            var ArmorMasterMediumFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArmorMasterMediumFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName(TTTContext, "Armor Master (Medium Armor)");
                bp.SetDescription(TTTContext, "You don’t take an armor check penalty or incur an arcane spell failure chance when wearing medium armor or using any shield. " +
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
            var ArmorMasterHeavyFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArmorMasterHeavyFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName(TTTContext, "Armor Master (Heavy Armor)");
                bp.SetDescription(TTTContext, "You don’t take an armor check penalty or incur an arcane spell failure chance when wearing heavy armor or using any shield. " +
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
                bp.AddPrerequisites(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = ArmorMasterMediumFeature.ToReference<BlueprintFeatureReference>();
                }));
            });
            ArmorMasterSelection.TemporaryContext(bp => {
                bp.AddFeatures(ArmorMasterLightFeature, ArmorMasterMediumFeature, ArmorMasterHeavyFeature);
            });
            var ArmorMasterHomebrewFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArmorMasterHomebrewFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName(TTTContext, "Armor Master");
                bp.SetDescription(TTTContext, "While wearing armor or wielding a shield, you reduce the armor check penalty by 1 per mythic rank " +
                    "and increase the maximum Dexterity bonus allowed by 1 per mythic rank. " +
                    "Additionally, you reduce your arcane spell failure chance from armor and shields by 5% per mythic rank.");
                bp.AddComponent<ContextArmorMaxDexBonusIncrease>(c => {
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

            if (TTTContext.AddedContent.MythicAbilities.IsEnabled("ArmorMaster")) {
                FeatTools.AddAsMythicAbility(ArmorMasterSelection);
            }
            if (TTTContext.AddedContent.MythicAbilities.IsEnabled("ArmorMasterHomebrew")) {
                FeatTools.AddAsMythicAbility(ArmorMasterHomebrewFeature);
            }
        }
    }
}
