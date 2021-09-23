using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.MechanicsChanges;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedArmorTrainings {
    class AdvancedArmorTraining {
        public static void AddAdvancedArmorTraining() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var FighterFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f");

            ArmorTrainingProgression();
            //Creating class progression feats

            var ArmorProgression = Resources.GetModBlueprint<BlueprintFeature>("ArmorTrainingFlag");

            var AdvancedArmorTrainingSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("AdvancedArmorTrainingSelection", bp => {
                bp.Ranks = 3;
                bp.SetName("Advanced Armor Training");
                bp.SetDescription("Beginning at 7th level, instead of increasing the benefits provided by armor training " +
                    "(reducing his armor check penalty by 1 and increasing its maximum Dexterity bonus by 1), a fighter can " +
                    "choose an advanced armor training option (see Advanced Armor Training below) . If the fighter does so, " +
                    "he still gains the ability to move at his normal speed while wearing medium armor at 3rd level, and while wearing heavy armor at 7th level.");
                bp.m_AllFeatures = new BlueprintFeatureReference[0];
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.IsClassFeature = true;
                bp.AddPrerequisite(Helpers.Create<PrerequisitePsuedoProgressionRank>(p =>
                {
                    p.m_KeyRef = ArmorProgression.ToReference<BlueprintFeatureReference>();
                    p.Level = 7;
                }));
            });
            var AdvancedArmorTraining1 = CreateAdvancedArmorFeat("AdvancedArmorTraining1", bp => {
                bp.AddPrerequisite(Helpers.Create<PrerequisitePsuedoProgressionRank>(p =>
                {
                    p.m_KeyRef = ArmorProgression.ToReference<BlueprintFeatureReference>();
                    p.Level = 3;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = ArmorTraining.ToReference<BlueprintFeatureReference>();
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining2 = CreateAdvancedArmorFeat("AdvancedArmorTraining2", bp => {
                bp.AddPrerequisite(Helpers.Create<PrerequisitePsuedoProgressionRank>(p =>
                {
                    p.m_KeyRef = ArmorProgression.ToReference<BlueprintFeatureReference>();
                    p.Level = 6;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining1.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining3 = CreateAdvancedArmorFeat("AdvancedArmorTraining3", bp => {
                bp.AddPrerequisite(Helpers.Create<PrerequisitePsuedoProgressionRank>(p =>
                {
                    p.m_KeyRef = ArmorProgression.ToReference<BlueprintFeatureReference>();
                    p.Level = 9;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining2.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining4 = CreateAdvancedArmorFeat("AdvancedArmorTraining4", bp => {
                bp.AddPrerequisite(Helpers.Create<PrerequisitePsuedoProgressionRank>(p =>
                {
                    p.m_KeyRef = ArmorProgression.ToReference<BlueprintFeatureReference>();
                    p.Level = 12;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining3.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining5 = CreateAdvancedArmorFeat("AdvancedArmorTraining5", bp => {
                bp.AddPrerequisite(Helpers.Create<PrerequisitePsuedoProgressionRank>(p =>
                {
                    p.m_KeyRef = ArmorProgression.ToReference<BlueprintFeatureReference>();
                    p.Level = 15;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining4.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });
            var AdvancedArmorTraining6 = CreateAdvancedArmorFeat("AdvancedArmorTraining6", bp => {
                bp.AddPrerequisite(Helpers.Create<PrerequisitePsuedoProgressionRank>(p =>
                {
                    p.m_KeyRef = ArmorProgression.ToReference<BlueprintFeatureReference>();
                    p.Level = 15;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = AdvancedArmorTraining5.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
                bp.AddPrerequisite(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    c.HideInUI = true;
                }));
            });

            var ArmorTrainingSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("ArmorTrainingSelection", bp => {
                bp.Ranks = 4;
                bp.m_Icon = ArmorTraining.Icon;
                bp.SetName("Armor Training");
                bp.SetDescription(ArmorTraining.Description);
                bp.m_AllFeatures = new BlueprintFeatureReference[] {
                    ArmorTraining.ToReference<BlueprintFeatureReference>(),
                    AdvancedArmorTrainingSelection.ToReference<BlueprintFeatureReference>()
                };
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.IsClassFeature = true;
                bp.AddComponent(Helpers.Create<SelectionDefaultFeature>(c => {
                    c.DefaultFeature = ArmorTraining.ToReference<BlueprintFeatureReference>();
                }));
            });

            if (ModSettings.AddedContent.FighterAdvancedArmorTraining.IsDisabled("Feats")) { return; }
            FeatTools.AddAsFeat(
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

            BlueprintFeatureSelection CreateAdvancedArmorFeat(string name, Action<BlueprintFeatureSelection> init = null) {
                var ArmorTrainingFeat = Helpers.CreateBlueprint<BlueprintFeatureSelection>(name, bp => {
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
                });
                init?.Invoke(ArmorTrainingFeat);
                return ArmorTrainingFeat;
            }
        }

        public static void SetArmorTrainingProgressionConfig(ContextRankConfig config)
        {
            config.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
            config.m_CustomProperty = Resources.GetModBlueprint<BlueprintUnitProperty>("ArmorTrainingProgressionProperty").ToReference<BlueprintUnitPropertyReference>();
        }
       
        public static void AddToAdvancedArmorTrainingSelection(params BlueprintFeature[] features) {
            var AdvancedArmorTrainingSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedArmorTrainingSelection");
            var AdvancedArmorTraining1 = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedArmorTraining1");
            var AdvancedArmorTraining2 = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedArmorTraining2");
            var AdvancedArmorTraining3 = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedArmorTraining3");
            var AdvancedArmorTraining4 = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedArmorTraining4");
            var AdvancedArmorTraining5 = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedArmorTraining5");
            var AdvancedArmorTraining6 = Resources.GetModBlueprint<BlueprintFeatureSelection>("AdvancedArmorTraining6");

            AdvancedArmorTrainingSelection.AddFeatures(features);
            AdvancedArmorTraining1.AddFeatures(features);
            AdvancedArmorTraining2.AddFeatures(features);
            AdvancedArmorTraining3.AddFeatures(features);
            AdvancedArmorTraining4.AddFeatures(features);
            AdvancedArmorTraining5.AddFeatures(features);
            AdvancedArmorTraining6.AddFeatures(features);
        }

        static void ArmorTrainingProgression()
        {
            BlueprintUnitProperty armorprop = Helpers.CreateBlueprint<BlueprintUnitProperty>("ArmorTrainingProgressionProperty", x =>
            {
                x.AddComponent(Helpers.Create<PseudoProgressionRankGetter>(y =>
                {
                    y.Key = Resources.GetModBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();

                }));
            });
            Helpers.CreateBlueprint<BlueprintFeature>("ArmorTrainingFlag", bp =>
            {
                bp.Ranks = 1;
                bp.SetName("Armor Training");
                bp.HideInCharacterSheetAndLevelUp = true;
            });
            FighterArmorTrainingProgression();
            HellknightArmorTrainingProgress();
            PurifierArmorTrainingProgression();
            ArmoredBattlemageArmorTrainingProgression();
            SteelbloodArmorTrainingProgression();
            HellknightSignifierArmorTrainingProgress();
        }

        static void SteelbloodArmorTrainingProgression()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("SteelbloodArmorTrainingProgression", x =>
            {
                var Steelblood = Resources.GetBlueprint<BlueprintArchetype>("32a5dff92373a9641b43e97d453b9369");
                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetModBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = Steelblood.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.scalar = -2; //Steelblood progression starts two levels late
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Steelblood Armor Training Progression");
                x.SetDescription("Increases your armor training rank by your Steeblood level minus two, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });
        }

        static void PurifierArmorTrainingProgression()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("CelestialArmorMastery", c =>
            {
                var PuriferArchetype = Resources.GetBlueprint<BlueprintArchetype>("c9df67160a77ecd4a97928f2455545d7");
                var CelestialArmor = Resources.GetBlueprint<BlueprintFeature>("7dc8d7dede2704640956f7bc4102760a");
                c.SetName("Celestial Armor Training Progression");
                c.SetDescription("Increases your armor training rank by your oracle level minus four, progressing Advanced Armor Training abilities.");
                c.IsClassFeature = true;
                c.HideInCharacterSheetAndLevelUp = true;
                c.Ranks = 1;
                c.m_Icon = CelestialArmor.Icon;

                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetModBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = PuriferArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.scalar = -4;
                });


                c.AddComponent(progression);

            });
        }

        static void ArmoredBattlemageArmorTrainingProgression()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("ArmoredBattlemageArmorTrainingProgression", x =>
            {
                var ArmoredBattlemageArchetype = Resources.GetBlueprint<BlueprintArchetype>("67ec8dcae6fb3d3439e5ae874ddc7b9b");
                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetModBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = ArmoredBattlemageArchetype.GetParentClass().ToReference<BlueprintCharacterClassReference>();
                    x.multiplier = 0.8;//Armored battlemage tiers up every five levels, not every four like fighter
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Armored Battlemage Armor Training Progression");
                x.SetDescription("Increases your armor training rank by four-fiifths of your Armored Battlemage level, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });
        }

        static void HellknightArmorTrainingProgress()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("HellknightArmorTrainingProgression", x =>
            {
                BlueprintCharacterClass Hellknight = Resources.GetBlueprint<BlueprintCharacterClass>("ed246f1680e667b47b7427d51e651059");

                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetModBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = Hellknight.ToReference<BlueprintCharacterClassReference>();
                    //Leaving standard progression/scaling because not giving faster-than-fighter like scaling based on their progession would dictate
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Hellknight Armor Training Progression");
                x.SetDescription("Increases your armor training rank by your Hellknight level, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });
        }

        static void HellknightSignifierArmorTrainingProgress()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("HellknightSigniferArmorTrainingProgression", x =>
            {
                BlueprintCharacterClass Signifier = Resources.GetBlueprint<BlueprintCharacterClass>("ee6425d6392101843af35f756ce7fefd");

                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetModBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = Signifier.ToReference<BlueprintCharacterClassReference>();
                    //Leaving standard progression/scaling because it's a gish build PRC and those are stuck competing with Magus, Warpriest and Battle Oracle. It should hardly be a balance issue if it gets full scaling here
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Hellknight Signifier Armor Training Progression");
                x.SetDescription("Increases your armor training rank by your Hellknight Signifier level, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });
        }

        static void FighterArmorTrainingProgression()
        {
            Helpers.CreateBlueprint<BlueprintFeature>("FighterArmorTrainingProgression", x =>
            {

                PseudoProgressionRankClassModifier progression = Helpers.Create<PseudoProgressionRankClassModifier>(
                x =>
                {

                    x.Key = Resources.GetModBlueprint<BlueprintFeature>("ArmorTrainingFlag").ToReference<BlueprintFeatureReference>();
                    x.m_ActualClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd").ToReference<BlueprintCharacterClassReference>();
                });
                x.IsClassFeature = true;
                x.m_Icon = Resources.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3").Icon;
                x.SetName("Armor Training Progression");
                x.SetDescription("Increases your armor training rank by your fighter level, progressing Advanced Armor Training abilities.");
                x.Ranks = 1;
                x.AddComponent(progression);

            });

        }

    }
}
