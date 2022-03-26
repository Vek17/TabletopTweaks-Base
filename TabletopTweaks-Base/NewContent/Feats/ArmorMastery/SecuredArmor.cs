using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats.ArmorMastery {
    static class SecuredArmor {
        internal static void AddSecuredArmor() {
            var FighterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ArmorFocusLight = BlueprintTools.GetBlueprint<BlueprintFeature>("3bc6e1d2b44b5bb4d92e6ba59577cf62");
            var ArmorFocusMedium = BlueprintTools.GetBlueprint<BlueprintFeature>("7dc004879037638489b64d5016997d12");
            var ArmorFocusHeavy = BlueprintTools.GetBlueprint<BlueprintFeature>("c27e6d2b0d33d42439f512c6d9a6a601");

            var LightArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132");
            var MediumArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("46f4fb320f35704488ba3d513397789d");
            var HeavyArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("1b0f68188dcc435429fb87a022239681");

            var Fortification25Enchant = BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("1e69e9029c627914eb06608dad707b36");
            var Fortification50Enchant = BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("62ec0b22425fb424c82fd52d7f4c02a5");

            var SecuredArmorLightFortification = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SecuredArmorLightFortification", bp => {
                bp.SetName(TTTContext, "");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });
            var SecuredArmorModerateFortification = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SecuredArmorModerateFortification", bp => {
                bp.SetName(TTTContext, "");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });
            var SecuredArmorFortificationProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(TTTContext, "SecuredArmorFortificationProperty", bp => {
                bp.BaseValue = 25;
                bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
                bp.AddComponent<CompositeCustomPropertyGetter>(c => {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Highest;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                        new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = new PropertyWithFactRankGetter(){
                                m_Property = UnitProperty.None,
                                m_RankMultiplier = 25,
                                m_Fact = SecuredArmorLightFortification.ToReference<BlueprintUnitFactReference>()
                            }
                        },
                        new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = new PropertyWithFactRankGetter(){
                                m_Property = UnitProperty.None,
                                m_RankMultiplier = 50,
                                m_Fact = SecuredArmorModerateFortification.ToReference<BlueprintUnitFactReference>()
                            }
                        }
                    };
                });
            });
            var SecuredArmorEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SecuredArmorEffect", bp => {
                bp.SetName(TTTContext, "Secured Armor");
                bp.SetDescription(TTTContext, "You make the most of your armor’s magical capabilities.\n" +
                    "Benefit: When you are hit by a confirmed critical hit or a sneak attack while wearing medium or heavy armor, " +
                    "there is a 25% chance that the critical hit or sneak attack is negated and damage is instead rolled normally.\n" +
                    "Special: This chance stacks with the light fortification and moderate fortification armor special abilities.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFortification>(c => {
                    c.UseContextValue = true;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_CustomProperty = SecuredArmorFortificationProperty.ToReference<BlueprintUnitPropertyReference>();
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_Max = 75;
                    c.m_UseMax = true;
                    c.m_Min = 25;
                    c.m_UseMin = true;
                });
                bp.AddComponent<RecalculateOnEquipmentChange>();
                bp.AddComponent<RecalculateOnFactsChange>();
            });
            var SecuredArmorFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SecuredArmorFeature", bp => {
                bp.SetName(SecuredArmorEffect.m_DisplayName);
                bp.SetDescription(SecuredArmorEffect.m_Description);
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = SecuredArmorEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Medium, ArmorProficiencyGroup.Heavy };
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.Dexterity;
                    c.Value = 13;
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass;
                    c.Level = 8;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 11;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisiteFeaturesFromList(1, ArmorTraining, ArmorFocusMedium, ArmorFocusHeavy);
            });

            if (TTTContext.AddedContent.ArmorMasteryFeats.IsDisabled("SecuredArmor")) { return; }
            Fortification25Enchant.AddComponent<AddUnitFeatureEquipment>(c => {
                c.m_Feature = SecuredArmorLightFortification.ToReference<BlueprintFeatureReference>();
            });
            Fortification50Enchant.AddComponent<AddUnitFeatureEquipment>(c => {
                c.m_Feature = SecuredArmorModerateFortification.ToReference<BlueprintFeatureReference>();
            });
            ArmorMastery.AddToArmorMasterySelection(SecuredArmorFeature);
        }
    }
}
