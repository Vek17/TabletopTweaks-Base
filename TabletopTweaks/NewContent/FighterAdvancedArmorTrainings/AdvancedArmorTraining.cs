using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedArmorTrainings {
    class AdvancedArmorTraining {
        public static void AddAdvancedArmorTraining() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var FighterFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f");
            var BasicFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");

            var AdvancedArmorTrainingSelection = Helpers.Create<BlueprintFeatureSelection>(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["AdvancedArmorTrainingSelection"];
                bp.name = "AdvancedArmorTrainingSelection";
                bp.SetName("Advanced Armor Training");
                bp.SetDescription("Beginning at 7th level, instead of increasing the benefits provided by armor training " +
                    "(reducing his armor’s check penalty by 1 and increasing its maximum Dexterity bonus by 1), a fighter can " +
                    "choose an advanced armor training option (see Advanced Armor Training below) . If the fighter does so, " +
                    "he still gains the ability to move at his normal speed while wearing medium armor at 3rd level, and while wearing heavy armor at 7th level.");
                bp.m_AllFeatures = new BlueprintFeatureReference[0];
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.IsClassFeature = true;
                bp.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 7;
                }));
            });
            var AdvancedArmorTraining1 = CreateAdvancedArmorFeat(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining1"];
                bp.name = "AdvancedArmorTraining1";
                bp.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 3;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining2 = CreateAdvancedArmorFeat(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining2"];
                bp.name = "AdvancedArmorTraining2";
                bp.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 6;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining1.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining3 = CreateAdvancedArmorFeat(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining3"];
                bp.name = "AdvancedArmorTraining3";
                bp.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 9;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining2.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining4 = CreateAdvancedArmorFeat(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining4"];
                bp.name = "AdvancedArmorTraining4";
                bp.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 12;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining3.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining5 = CreateAdvancedArmorFeat(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining5"];
                bp.name = "AdvancedArmorTraining5";
                bp.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 15;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining4.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining6 = CreateAdvancedArmorFeat(bp => {
                bp.AssetGuid = ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining6"];
                bp.name = "AdvancedArmorTraining6";
                bp.AddComponent(Helpers.Create<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 18;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining5.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });

            Resources.AddBlueprint(AdvancedArmorTrainingSelection);
            Resources.AddBlueprint(AdvancedArmorTraining1);
            Resources.AddBlueprint(AdvancedArmorTraining2);
            Resources.AddBlueprint(AdvancedArmorTraining3);
            Resources.AddBlueprint(AdvancedArmorTraining4);
            Resources.AddBlueprint(AdvancedArmorTraining5);
            Resources.AddBlueprint(AdvancedArmorTraining6);

            if (ModSettings.AddedContent.FighterAdvancedArmorTraining.DisableAll || !ModSettings.AddedContent.FighterAdvancedArmorTraining.Enabled["Feats"]) { return; }
            BasicFeatSelection.AddFeatures(
                AdvancedArmorTraining1,
                AdvancedArmorTraining2,
                AdvancedArmorTraining3,
                AdvancedArmorTraining4,
                AdvancedArmorTraining5,
                AdvancedArmorTraining6
            );
            FighterFeatSelection.AddFeatures(
                AdvancedArmorTraining1,
                AdvancedArmorTraining2,
                AdvancedArmorTraining3,
                AdvancedArmorTraining4,
                AdvancedArmorTraining5,
                AdvancedArmorTraining6
            );

            BlueprintFeatureSelection CreateAdvancedArmorFeat(Action<BlueprintFeatureSelection> init = null) {
                var ArmorTrainingFeat =  Helpers.Create<BlueprintFeatureSelection>(bp => {
                    bp.SetName("Advanced Armor Training");
                    bp.SetDescription("Select one advanced armor training option.");
                    bp.Groups = new FeatureGroup[] {
                        FeatureGroup.CombatFeat,
                        FeatureGroup.Feat
                    };
                    bp.m_AllFeatures = new BlueprintFeatureReference[0];
                    bp.m_Features = new BlueprintFeatureReference[0];
                    bp.IsClassFeature = true;
                    bp.HideNotAvailibleInUI = true;
                    bp.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                        c.m_Feature = ArmorTraining.ToReference<BlueprintFeatureReference>();
                    }));
                });
                init?.Invoke(ArmorTrainingFeat);
                return ArmorTrainingFeat;
            }
        }
        public static void AddToAdvancedArmorTrainingSelection(params BlueprintFeature[] features) {
            var AdvancedArmorTrainingSelection = Resources.GetBlueprint<BlueprintFeatureSelection>(ModSettings.Blueprints.NewBlueprints["AdvancedArmorTrainingSelection"]);
            var AdvancedArmorTraining1 = Resources.GetBlueprint<BlueprintFeatureSelection>(ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining1"]);
            var AdvancedArmorTraining2 = Resources.GetBlueprint<BlueprintFeatureSelection>(ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining2"]);
            var AdvancedArmorTraining3 = Resources.GetBlueprint<BlueprintFeatureSelection>(ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining3"]);
            var AdvancedArmorTraining4 = Resources.GetBlueprint<BlueprintFeatureSelection>(ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining4"]);
            var AdvancedArmorTraining5 = Resources.GetBlueprint<BlueprintFeatureSelection>(ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining5"]);
            var AdvancedArmorTraining6 = Resources.GetBlueprint<BlueprintFeatureSelection>(ModSettings.Blueprints.NewBlueprints["AdvancedArmorTraining6"]);

            AdvancedArmorTrainingSelection.AddFeatures(features);
            AdvancedArmorTraining1.AddFeatures(features);
            AdvancedArmorTraining2.AddFeatures(features);
            AdvancedArmorTraining3.AddFeatures(features);
            AdvancedArmorTraining4.AddFeatures(features);
            AdvancedArmorTraining5.AddFeatures(features);
            AdvancedArmorTraining6.AddFeatures(features);
        }
    }
}
