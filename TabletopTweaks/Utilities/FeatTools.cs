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
                Selections.BasicFeatSelection.AddFeatures(features);
                Selections.FeatSelections
                    .Where(selection => feature.HasGroup(selection.Group) || feature.HasGroup(selection.Group2))
                    .ForEach(selection => selection.AddFeatures(feature));
            }
        }/*
        public static void AddAsFeat(BlueprintFeature feature) {
            var BasicFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");
            var ExtraFeatMythicFeat = Resources.GetBlueprint<BlueprintFeatureSelection>("e10c4f18a6c8b4342afe6954bde0587b");

            BasicFeatSelection.AddFeatures(feature);
            ExtraFeatMythicFeat.AddFeatures(feature);
        }*/
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
        public static void AddAsMythicAbility(BlueprintFeature feature) {
            var MythicAbilitySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var ExtraMythicAbilityMythicFeat = Resources.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");

            MythicAbilitySelection.AddFeatures(feature);
            ExtraMythicAbilityMythicFeat.AddFeatures(feature);
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
        public static class Selections {
            public static BlueprintFeatureSelection AasimarHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("67aabcbce8f8ae643a9d08a6ca67cabd");
            public static BlueprintFeatureSelection Adaptability => Resources.GetBlueprint<BlueprintFeatureSelection>("26a668c5a8c22354bac67bcd42e09a3f");
            public static BlueprintFeatureSelection AdvancedWeaponTraining1 => Resources.GetBlueprint<BlueprintFeatureSelection>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
            public static BlueprintFeatureSelection AdvancedWeaponTraining2 => Resources.GetBlueprint<BlueprintFeatureSelection>("70a139f0a4c6c534eaa34feea0d08622");
            public static BlueprintFeatureSelection AdvancedWeaponTraining3 => Resources.GetBlueprint<BlueprintFeatureSelection>("ee9ab0117ca06b84f9c66469f4428c61");
            public static BlueprintFeatureSelection AdvancedWeaponTraining4 => Resources.GetBlueprint<BlueprintFeatureSelection>("0b55d725ded1ae549bb858fba1d84114");
            public static BlueprintFeatureSelection AeonGazeFirstSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7e468a5266235454e9dcefee315ee6d5");
            public static BlueprintFeatureSelection AeonGazeSecondSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7d498e034d18ca94baea19d7edee7403");
            public static BlueprintFeatureSelection AirBlastSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("49e55e8f24e1ad24e910fefc0258adba");
            public static BlueprintFeatureSelection AngelHaloImprovementsSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("bdbc41e2bad92a640bd58acf74e2af8b");
            public static BlueprintFeatureSelection AngelSwordGreaterImprovementsSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("e0ce40968bf0007408b11089a10f36cf");
            public static BlueprintFeatureSelection AngelSwordImprovementsSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c4a7679515ae2374eb634dd8087faf47");
            public static BlueprintFeatureSelection AnimalCompanionArchetypeSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("65af7290b4efd5f418132141aaa36c1b");
            public static BlueprintFeatureSelection AnimalCompanionSelectionBase => Resources.GetBlueprint<BlueprintFeatureSelection>("90406c575576aee40a34917a1b429254");
            public static BlueprintFeatureSelection AnimalCompanionSelectionDivineHound => Resources.GetBlueprint<BlueprintFeatureSelection>("8abc5936b95d4983866f3bcace522e23");
            public static BlueprintFeatureSelection AnimalCompanionSelectionDomain => Resources.GetBlueprint<BlueprintFeatureSelection>("2ecd6c64683b59944a7fe544033bb533");
            public static BlueprintFeatureSelection AnimalCompanionSelectionDruid => Resources.GetBlueprint<BlueprintFeatureSelection>("571f8434d98560c43935e132df65fe76");
            public static BlueprintFeatureSelection AnimalCompanionSelectionHunter => Resources.GetBlueprint<BlueprintFeatureSelection>("715ac15eb8bd5e342bc8a0a3c9e3e38f");
            public static BlueprintFeatureSelection AnimalCompanionSelectionMadDog => Resources.GetBlueprint<BlueprintFeatureSelection>("738b59d0b58187f4d846b0caaf0f80d7");
            public static BlueprintFeatureSelection AnimalCompanionSelectionPrimalDruid => Resources.GetBlueprint<BlueprintFeatureSelection>("fad1127127d9413c8e3e85f8f0450bc1");
            public static BlueprintFeatureSelection AnimalCompanionSelectionRanger => Resources.GetBlueprint<BlueprintFeatureSelection>("ee63330662126374e8785cc901941ac7");
            public static BlueprintFeatureSelection AnimalCompanionSelectionSacredHuntsmaster => Resources.GetBlueprint<BlueprintFeatureSelection>("2995b36659b9ad3408fd26f137ee2c67");
            public static BlueprintFeatureSelection AnimalCompanionSelectionSylvanSorcerer => Resources.GetBlueprint<BlueprintFeatureSelection>("a540d7dfe1e2a174a94198aba037274c");
            public static BlueprintFeatureSelection AnimalCompanionSelectionUrbanHunter => Resources.GetBlueprint<BlueprintFeatureSelection>("257375cd139800e459d69ccfe4ca309c");
            public static BlueprintFeatureSelection AnimalCompanionSelectionWildlandShaman => Resources.GetBlueprint<BlueprintFeatureSelection>("164c875d6b27483faba479c7f5261915");
            public static BlueprintFeatureSelection ArcaneBombSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("0c7a9899e27cae6449ee1fb3049f128d");
            public static BlueprintFeatureSelection ArcaneBondSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("03a1781486ba98043afddaabf6b7d8ff");
            public static BlueprintFeatureSelection ArcaneRiderFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("8e627812dc034b9db12fa396fdc9ec75");
            public static BlueprintFeatureSelection ArcaneRiderMountSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("82c791d4790c45dcac6038ef6932c3e3");
            public static BlueprintFeatureSelection ArcaneTricksterSpellbookSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ae04b7cdeb88b024b9fd3882cc7d3d76");
            public static BlueprintFeatureSelection ArcanistExploitSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b8bf3d5023f2d8c428fdf6438cecaea7");
            public static BlueprintFeatureSelection ArmorFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("76d4885a395976547a13c5d6bf95b482");
            public static BlueprintFeatureSelection AscendantElementSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("535d4e5d8a5664f4797560c280941782");
            public static BlueprintFeatureSelection AzataSuperpowersSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("8a30e92cd04ff5b459ba7cb03584fda0");
            public static BlueprintFeatureSelection BackgroundsBaseSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("f926dabeee7f8a54db8f2010b323383c");
            public static BlueprintFeatureSelection BackgroundsClericSpellLikeSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("94f680dbbb083cc43962249e446a3e10");
            public static BlueprintFeatureSelection BackgroundsCraftsmanSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("fd36aaff6f41a2f4f9e91925d49a0d85");
            public static BlueprintFeatureSelection BackgroundsDruidSpellLikeSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("2fc911f0de029134585b5f35ff16be88");
            public static BlueprintFeatureSelection BackgroundsNobleSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7b11f589e81617a46b3e5eda3632508d");
            public static BlueprintFeatureSelection BackgroundsOblateSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c25021c31f302c6449ecdbc978822507");
            public static BlueprintFeatureSelection BackgroundsRegionalSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("fa621a249cc836f4382ca413b976e65e");
            public static BlueprintFeatureSelection BackgroundsScholarSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("273fab44409035f42a7e2af0858a463d");
            public static BlueprintFeatureSelection BackgroundsStreetUrchinSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("e17f74060f864ff459393e11d5e7fe2f");
            public static BlueprintFeatureSelection BackgroundsWandererSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("0cdd576724fce2240b372455889fac87");
            public static BlueprintFeatureSelection BackgroundsWarriorSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("291f372e27b29f149ad15ff219fe15d9");
            public static BlueprintFeatureSelection BackgroundsWizardSpellLikeSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("1139a014bb6cdcf4db0e11649ddfa60c");
            public static BlueprintFeatureSelection BardTalentSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("40f85fbe8cc35ef4fa96c66e06eeafe8");
            public static BlueprintFeatureSelection BardTalentSelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("94e2cd84bf3a8e04f8609fe502892f4f"); //BardTalentSelection
            public static BlueprintFeatureSelection BaseRaseHalfElfSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a9d54aab4e9243ca867341257aaa253e");
            public static BlueprintFeatureSelection BasicFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");
            public static BlueprintFeatureSelection BattleProwessSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("29b480a26a88f9e47a10d8c9fab84ee6");
            public static BlueprintFeatureSelection BattleScionTeamworkFeat => Resources.GetBlueprint<BlueprintFeatureSelection>("64960cdba39692243bef11da263ab7f3");
            public static BlueprintFeatureSelection BeastRiderMountSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("f7da602dae8944d499f00074c973c28a");
            public static BlueprintFeatureSelection BeneficialCurse => Resources.GetBlueprint<BlueprintFeatureSelection>("2dda67424ee8e0b4d83ef01a73ca6bff");
            public static BlueprintFeatureSelection BlessingSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("6d9dcc2a59210a14891aeedb09d406aa");
            public static BlueprintFeatureSelection BlightDruidBond => Resources.GetBlueprint<BlueprintFeatureSelection>("096fc02f6cc817a43991c4b437e12b8e");
            public static BlueprintFeatureSelection BloodlineAbyssalFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7781831170d794a4d8f147a1326c35fd");
            public static BlueprintFeatureSelection BloodlineArcaneArcaneBondFeature => Resources.GetBlueprint<BlueprintFeatureSelection>("1740a701ac0a14c4394a7f76f0b07799");
            public static BlueprintFeatureSelection BloodlineArcaneClassSkillSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("19ab16dba857d1a4ba617074f203f975");
            public static BlueprintFeatureSelection BloodlineArcaneFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ff4fd877b4c801342ab8e880b734a6b9");
            public static BlueprintFeatureSelection BloodlineArcaneNewArcanaSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("20a2435574bdd7f4e947f405df2b25ce");
            public static BlueprintFeatureSelection BloodlineArcaneSchoolPowerSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("3524a71d57d99bb4b835ad20582cf613");
            public static BlueprintFeatureSelection BloodlineAscendance => Resources.GetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19");
            public static BlueprintFeatureSelection BloodlineCelestialFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("101a87c6744a3f242836e657cd7f643e");
            public static BlueprintFeatureSelection BloodlineDraconicFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("f4b011d090e8ae543b1441bd594c7bf7");
            public static BlueprintFeatureSelection BloodlineElementalFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d2a4b74ee7e43a648b51d0f36db2aa34");
            public static BlueprintFeatureSelection BloodlineFeyFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d7f233e417c490545b00f49c3940638c");
            public static BlueprintFeatureSelection BloodlineInfernalFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("f19d9bfcbc1e3ea42bda754a03c40843");
            public static BlueprintFeatureSelection BloodlineSerpentineFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("76be9555a33d58a4bb419154540595df");
            public static BlueprintFeatureSelection BloodlineUndeadFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a29b72a804f7cb243b01e99c42452636");
            public static BlueprintFeatureSelection BloodOfDragonsSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("da48f9d7f697ae44ca891bfc50727988");
            public static BlueprintFeatureSelection BloodragerAbyssalFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4faaa616d43602c40806977a5c518e40");
            public static BlueprintFeatureSelection BloodragerAbyssalFeatSelectionGreenrager => Resources.GetBlueprint<BlueprintFeatureSelection>("989a6fd870f7c944dac365381a7c7c31");
            public static BlueprintFeatureSelection BloodragerArcaneFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d94311bc1342db14d8c48c08b73e94c7");
            public static BlueprintFeatureSelection BloodragerArcaneFeatSelectionGreenrager => Resources.GetBlueprint<BlueprintFeatureSelection>("c6ec51fa71aaadf4f9cd77549c4e45e8");
            public static BlueprintFeatureSelection BloodragerBloodlineSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("62b33ac8ceb18dd47ad4c8f06849bc01");
            public static BlueprintFeatureSelection BloodragerCelestialFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7eff1ba637c68b74ea562b4659636764");
            public static BlueprintFeatureSelection BloodragerCelestialFeatSelectionGreenrager => Resources.GetBlueprint<BlueprintFeatureSelection>("0c96650e80e712f439c2a4da8a4272d9");
            public static BlueprintFeatureSelection BloodragerDragonFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("8a2a8d623f6cb574f9e0b6c82ae65c74");
            public static BlueprintFeatureSelection BloodragerDragonFeatSelectionGreenrager => Resources.GetBlueprint<BlueprintFeatureSelection>("3d0b4942c400a7f448ca09e04edfb52a");
            public static BlueprintFeatureSelection BloodragerElementalFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("107b4ff0240382e4f9a4659484e1c0fa");
            public static BlueprintFeatureSelection BloodragerElementalFeatSelectionGreenrager => Resources.GetBlueprint<BlueprintFeatureSelection>("4adc208e35b9ef64285014c1af47fc4a");
            public static BlueprintFeatureSelection BloodragerFeyFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ae13cd63a0f0c4243bbc6566d1f98485");
            public static BlueprintFeatureSelection BloodragerFeyFeatSelectionGreenrager => Resources.GetBlueprint<BlueprintFeatureSelection>("4cacd14dc0ed7b94997c96e79bf67335");
            public static BlueprintFeatureSelection BloodragerInfernalFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d3139e9bd4146af41938cb10b436a6eb");
            public static BlueprintFeatureSelection BloodragerInfernalFeatSelectionGreenrager => Resources.GetBlueprint<BlueprintFeatureSelection>("56cc435ae8647354ea3ab6131ae43e8f");
            public static BlueprintFeatureSelection BloodragerSerpentineFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4a84be500fd481c40a79c2adb243905d");
            public static BlueprintFeatureSelection BloodragerSerpentineFeatSelectionGreenrager => Resources.GetBlueprint<BlueprintFeatureSelection>("f2d845da2ce13d745bbe54bf6838bb91");
            public static BlueprintFeatureSelection BloodragerUndeadFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("6980b5eb325e5fb47b12e54b31caff9e");
            public static BlueprintFeatureSelection BloodragerUndeadFeatSelectionGreenrager => Resources.GetBlueprint<BlueprintFeatureSelection>("d54d36748d84548469653d09dfb312a4");
            public static BlueprintFeatureSelection BloodriderMountSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c81bb2aa48c113c4ba3ee8582eb058ea");
            public static BlueprintFeatureSelection CavalierBonusFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("dd17090d14958ef48ba601688b611970");
            public static BlueprintFeatureSelection CavalierByMyHonorSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("6501702801e0dfc418d3e07371e8ab1d");
            public static BlueprintFeatureSelection CavalierMountedMasteryFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("20d873a12e4e6cb4ea6da9761e974dd4");
            public static BlueprintFeatureSelection CavalierMountSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("0605927df6e2fdd42af6ee2424eb89f2");
            public static BlueprintFeatureSelection CavalierOrderSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d710e30ea20240247ad87ad86bcd50f2");
            public static BlueprintFeatureSelection CavalierStarDeitySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("e8adb8d6bef3ae540bd03e0f43e81925");
            public static BlueprintFeatureSelection CavalierTacticianFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7bc55b5e381358c45b42153b8b2603a6");
            public static BlueprintFeatureSelection ChampionOfTheFaithAlignmentSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4146f0ff2c5e461488b0ace718020670");
            public static BlueprintFeatureSelection ChannelEnergySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d332c1748445e8f4f9e92763123e31bd");
            public static BlueprintFeatureSelection CombatTrick => Resources.GetBlueprint<BlueprintFeatureSelection>("c5158a6622d0b694a99efb1d0025d2c1");
            public static BlueprintFeatureSelection CrossbloodedSecondaryBloodlineSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("60c99d78a70e0b44f87ba01d02d909a6");
            public static BlueprintFeatureSelection CrusaderBonusFeat1 => Resources.GetBlueprint<BlueprintFeatureSelection>("c5357c05cf4f8414ebd0a33e534aec50");
            public static BlueprintFeatureSelection CrusaderBonusFeat10 => Resources.GetBlueprint<BlueprintFeatureSelection>("50dc57d2662ccbd479b6bc8ab44edc44");
            public static BlueprintFeatureSelection CrusaderBonusFeat20 => Resources.GetBlueprint<BlueprintFeatureSelection>("2049abc955bf6fe41a76f2cb6ba8214a");
            public static BlueprintFeatureSelection DeitySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("59e7a76987fe3b547b9cce045f4db3e4");
            public static BlueprintFeatureSelection DemonAspectSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("bbfc0d06955db514ba23337c7bf2cca6");
            public static BlueprintFeatureSelection DemonLordAspectSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("fc93daa527ec58c40afbe874c157bc91");
            public static BlueprintFeatureSelection DemonMajorAspectSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5eba1d83a078bdd49a0adc79279e1ffe");
            public static BlueprintFeatureSelection DevilbanePriestTeamworkFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("cf2ca457ffb585a4995fd79441167a72");
            public static BlueprintFeatureSelection DhampirHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("1246f548304a7654c97d8f2e9488e25f");
            public static BlueprintFeatureSelection DiscoverySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("cd86c437488386f438dcc9ae727ea2a6");
            public static BlueprintFeatureSelection DisenchanterFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("234772e17012d554f8661e3f365b68ae");
            public static BlueprintFeatureSelection DisenchanterFeatSelection12 => Resources.GetBlueprint<BlueprintFeatureSelection>("6b0dd75a4f3872543842179cf6e456e0");
            public static BlueprintFeatureSelection DisenchanterFeatSelection9 => Resources.GetBlueprint<BlueprintFeatureSelection>("f20d10e43ed9ede43959678bb246c531");
            public static BlueprintFeatureSelection DivineGuardianBonusFeat => Resources.GetBlueprint<BlueprintFeatureSelection>("5a4560c300c368b4289ae2b4e4da58cf");
            public static BlueprintFeatureSelection DivineHerbalistMysterySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c11ff5dbd8518c941849b3112d4d6b68");
            public static BlueprintFeatureSelection DivineHunterDomainsSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("72909a37a1ed5344f88ec9d1d31f5c5b");
            public static BlueprintFeatureSelection DomainsSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("48525e5da45c9c243a343fc6545dbdb9");
            public static BlueprintFeatureSelection DragonDiscipleSpellbookSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("8c1ba14c0b6dcdb439c56341385ee474");
            public static BlueprintFeatureSelection DragonheirDragonSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("729411185291d704696e58316420fe38");
            public static BlueprintFeatureSelection DragonLevel2FeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a21acdafc0169f5488a9bd3256e2e65b");
            public static BlueprintFeatureSelection DruidBondSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("3830f3630a33eba49b60f511b4c8f2a8");
            public static BlueprintFeatureSelection DruidDomainSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5edfe84c93823d04f8c40ca2b4e0f039");
            public static BlueprintFeatureSelection DwarfHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("fd6e1f53589049cbbbc6a8e058d83b74");
            public static BlueprintFeatureSelection EldritchKnightFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("da03141df23f3fe45b0c7c323a8e5a0e");
            public static BlueprintFeatureSelection EldritchKnightSpellbookSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("dc3ab8d0484467a4787979d93114ebc3");
            public static BlueprintFeatureSelection EldritchMagusArcanaSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d4b54d9db4932454ab2899f931c2042c");
            public static BlueprintFeatureSelection EldritchScionBloodlineSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("94c29f69cdc34594a6a4677441ed7375");
            public static BlueprintFeatureSelection ElemenetalRampagerSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c8befb3ccd6a46af99ef2b9aac255f6f");
            public static BlueprintFeatureSelection ElementalFocusMaster => Resources.GetBlueprint<BlueprintFeatureSelection>("1758ac999383cb1419b18cd6eb0d78e1");
            public static BlueprintFeatureSelection ElementalFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("1f3a15a3ae8a5524ab8b97f469bf4e3d");
            public static BlueprintFeatureSelection KineticistElementalFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("bb24cc01319528849b09a3ae8eec0b31"); //ElementalFocusSelection
            public static BlueprintFeatureSelection ELementalistElementSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("41964f61f24b4f38b695f36532d4b1a3");
            public static BlueprintFeatureSelection ElementalWhispersSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("f02525006521bee4eb90ab26b7b9db24");
            public static BlueprintFeatureSelection ElementalWitchPatronSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("3172b6960c774e19ad029c5e4a96d3e4");
            public static BlueprintFeatureSelection ElementaWitchEnergySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("9ec1bf2087874390a9d9be62884a0027");
            public static BlueprintFeatureSelection ElementaWitchEnergySelection11 => Resources.GetBlueprint<BlueprintFeatureSelection>("edbb5084c34a486fb39d38cc75f2f994");
            public static BlueprintFeatureSelection ElvenHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5482f879dcfd40f9a3168fdb48bc938c");
            public static BlueprintFeatureSelection EnlightenedPhilosopherMysterySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("9d5fdd3b4a6cd4f40beddbc72b2c07a0");
            public static BlueprintFeatureSelection ExoticWeaponProficiencySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("9a01b6815d6c3684cb25f30b8bf20932");
            public static BlueprintFeatureSelection ExpandedDefense => Resources.GetBlueprint<BlueprintFeatureSelection>("d741f298dfae8fc40b4615aaf83b6548");
            public static BlueprintFeatureSelection ExploiterExploitSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("2ba8a0040e0149e9ae9bfcb01a8ff01d");
            public static BlueprintFeatureSelection ExtraDomain => Resources.GetBlueprint<BlueprintFeatureSelection>("213a8480d22206b45acbfa0619ca5aaf");
            public static BlueprintFeatureSelection ExtraFeatMythicFeat => Resources.GetBlueprint<BlueprintFeatureSelection>("e10c4f18a6c8b4342afe6954bde0587b");
            public static BlueprintFeatureSelection ExtraMythicAbilityMythicFeat => Resources.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");
            public static BlueprintFeatureSelection ExtraRagePowerSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("0c7f01fbbe687bb4baff8195cb02fe6a");
            public static BlueprintFeatureSelection FaithHunterSwornEnemySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4fe6aec92d9e0bc41beab5b06ddd7872");
            public static BlueprintFeatureSelection Familiar => Resources.GetBlueprint<BlueprintFeatureSelection>("363cab72f77c47745bf3a8807074d183");
            public static BlueprintFeatureSelection FavoriteEnemyRankUp => Resources.GetBlueprint<BlueprintFeatureSelection>("c1be13839472aad46b152cf10cf46179");
            public static BlueprintFeatureSelection FavoriteEnemySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("16cc2c937ea8d714193017780e7d4fc6");
            public static BlueprintFeatureSelection FavoriteMetamagicSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            public static BlueprintFeatureSelection FavoriteTerrainSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a6ea422d7308c0d428a541562faedefd");
            public static BlueprintFeatureSelection FeyspeakerMagicSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("237efc6a131c8174bbd466ed239f21fc");
            public static BlueprintFeatureSelection FighterFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f");
            public static BlueprintFeatureSelection FinesseTrainingSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b78d146cea711a84598f0acef69462ea");
            public static BlueprintFeatureSelection FirstAscensionSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("1421e0034a3afac458de08648d06faf0");
            public static BlueprintFeatureSelection ForceOfWillRankUpSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("3e4dd44c121c753408680b1d5671641c");
            public static BlueprintFeatureSelection ForceOfWillSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("32fe6903b3a36b344a344624cda5b73f");
            public static BlueprintFeatureSelection GendarmeFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ef1cd58e0b7fc7f45baedb09407a1cd1");
            public static BlueprintFeatureSelection GnomeHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("584d8b50817b49b2bb7aab3d6add8d3a");
            public static BlueprintFeatureSelection GrandDiscoverySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("2729af328ab46274394cedc3582d6e98");
            public static BlueprintFeatureSelection GreaterElementalFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("1c17446a3eb744f438488711b792ca4d");
            public static BlueprintFeatureSelection HagboundWitchPatronSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("0b9af221d99a91842b3a2afbc6a68a1e");
            public static BlueprintFeatureSelection HalfElfHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("9df7b68d60544bcf8e5b56c0a4688e04");
            public static BlueprintFeatureSelection HalflingHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b3bebe76e6c64e2ca11585f9e3e2554a");
            public static BlueprintFeatureSelection HalfOrcHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("8c3244440e0b4d1d9d9b182685cbacbd");
            public static BlueprintFeatureSelection HellknightDisciplinePentamicFaith => Resources.GetBlueprint<BlueprintFeatureSelection>("b9750875e9d7454e85347d739a1bc894");
            public static BlueprintFeatureSelection HellknightDisciplineSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a0ea93d320e3da24aae270093e629305");
            public static BlueprintFeatureSelection HellKnightOrderSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("e31d849e4eb2578458419c9df622270f");
            public static BlueprintFeatureSelection HellknightSigniferSpellbook => Resources.GetBlueprint<BlueprintFeatureSelection>("68782aa7a302b6d43a42a71c6e9b5277");
            public static BlueprintFeatureSelection HermitMysterySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("bc86ea452bef22a45989c6fa644bd2eb");
            public static BlueprintFeatureSelection HexChannelerChannelSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c464ab12993745c44a7d21f386562cb6");
            public static BlueprintFeatureSelection HexcrafterMagusHexArcanaSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ad6b9cecb5286d841a66e23cea3ef7bf");
            public static BlueprintFeatureSelection HexcrafterMagusHexMagusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a18b8c3d6251d8641a8094e5c2a7bc78");
            public static BlueprintFeatureSelection HunterPreciseCompanion => Resources.GetBlueprint<BlueprintFeatureSelection>("75e9a5179aeff4140a8235ebdeaf8df7");
            public static BlueprintFeatureSelection HuntersBondSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b705c5184a96a84428eeb35ae2517a14");
            public static BlueprintFeatureSelection HunterTeamworkFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("01046afc774beee48abde8e35da0f4ba");
            public static BlueprintFeatureSelection IncenseSynthesizerIncenseFogSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("73d5950937bf0aa428e82c54c968f7e6");
            public static BlueprintFeatureSelection InfusionSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("58d6f8e9eea63f6418b107ce64f315ea");
            public static BlueprintFeatureSelection InstinctualWarriorRagePowerSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("609f0e5336084442a0dafa3abd4d31c5");
            public static BlueprintFeatureSelection KineticKnightElementalFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b1f296f0bd16bc242ae35d0638df82eb");
            public static BlueprintFeatureSelection KitsuneHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ec40cc350b18c8c47a59b782feb91d1f");
            public static BlueprintFeatureSelection LichSkeletalCombatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5701afd6f00754240987fbb02101824d");
            public static BlueprintFeatureSelection LichSkeletalRageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("36f0d962a9f2d0a458062d5edc9795e5");
            public static BlueprintFeatureSelection LichSkeletalTeamworkSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("03168a77717179c43bcddb71d52f6167");
            public static BlueprintFeatureSelection LichSkeletalUpgradeSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a434dddab8026e947bc16eb36d18a783");
            public static BlueprintFeatureSelection LichSkeletalWeaponSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("f832862da7f07dd4eba342a27451c3a8");
            public static BlueprintFeatureSelection LichUniqueAbilitiesSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("1f646b820a37d3d4a8ab116a24ee0022");
            public static BlueprintFeatureSelection LifeBondingFriendshipSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("cfad18f581584ac4ba066df067956477");
            public static BlueprintFeatureSelection LifeBondingFriendshipSelection1 => Resources.GetBlueprint<BlueprintFeatureSelection>("69a33d6ced23446e819667149d088898");
            public static BlueprintFeatureSelection LoremasterClericSpellSecret => Resources.GetBlueprint<BlueprintFeatureSelection>("904ce918c85c9f947910340b956fb877");
            public static BlueprintFeatureSelection LoremasterCombatFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("90f105c8e31a6224ea319e6a810e4af8");
            public static BlueprintFeatureSelection LoremasterDruidSpellSecret => Resources.GetBlueprint<BlueprintFeatureSelection>("6b73ba9d8a718fb419a484c6e1b92c6d");
            public static BlueprintFeatureSelection LoremasterRogueTalentSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4b7018b1ed4b27140a5e7adfacaaf9c6");
            public static BlueprintFeatureSelection LoremasterSecretSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("beeb25d7a7732e14f9986cdb79acecfc");
            public static BlueprintFeatureSelection LoremasterSpellbookSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7a28ab4dfc010834eabc770152997e87");
            public static BlueprintFeatureSelection LoremasterWizardFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("689959eef3e972e458b52598dcc2c752");
            public static BlueprintFeatureSelection LoremasterWizardSpellSecret => Resources.GetBlueprint<BlueprintFeatureSelection>("f97986f19a595e2409cfe5d92bcf697c");
            public static BlueprintFeatureSelection MadDogCompanionSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d91228368cb523a43ad17104adf26ba5");
            public static BlueprintFeatureSelection MagusArcanaSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");
            public static BlueprintFeatureSelection MagusFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("66befe7b24c42dd458952e3c47c93563");
            public static BlueprintFeatureSelection MasterOfAllSkillFocus => Resources.GetBlueprint<BlueprintFeatureSelection>("f2d2c1702d8a4cc6adfcbd4ebff8eee4");
            public static BlueprintFeatureSelection MetakinesisMaster => Resources.GetBlueprint<BlueprintFeatureSelection>("8c33002186eb2fd45a140eed1301e207");
            public static BlueprintFeatureSelection MonkBonusFeatSelectionLevel1 => Resources.GetBlueprint<BlueprintFeatureSelection>("ac3b7f5c11bce4e44aeb332f66b75bab");
            public static BlueprintFeatureSelection MonkBonusFeatSelectionLevel10 => Resources.GetBlueprint<BlueprintFeatureSelection>("1051170c612d5b844bfaa817d6f4cfff");
            public static BlueprintFeatureSelection MonkBonusFeatSelectionLevel6 => Resources.GetBlueprint<BlueprintFeatureSelection>("b993f42cb119b4f40ac423ae76394374");
            public static BlueprintFeatureSelection MonkKiPowerSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("3049386713ff04245a38b32483362551");
            public static BlueprintFeatureSelection MonkStyleStrike => Resources.GetBlueprint<BlueprintFeatureSelection>("7bc6a93f6e48eff49be5b0cde83c9450");
            public static BlueprintFeatureSelection MonsterMythicQualitySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("8354b789c91d5c1459ae58b5b769e2f4");
            public static BlueprintFeatureSelection MutationWarriorDiscoverySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5d297a1a9a4384e408a1098381721db1");
            public static BlueprintFeatureSelection MysticTheurgeArcaneSpellbookSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("97f510c6483523c49bc779e93e4c4568");
            public static BlueprintFeatureSelection MysticTheurgeDivineSpellbookSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7cd057944ce7896479717778330a4933");
            public static BlueprintFeatureSelection MysticTheurgeInquisitorLevelSelection1 => Resources.GetBlueprint<BlueprintFeatureSelection>("8ae18c62c0fbfeb4ea77f877883947fd");
            public static BlueprintFeatureSelection MysticTheurgeInquisitorLevelSelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("f78f63d364bd9fe4ea2885d95432c068");
            public static BlueprintFeatureSelection MysticTheurgeInquisitorLevelSelection3 => Resources.GetBlueprint<BlueprintFeatureSelection>("5f6c7b84edc68a146955be0600de4095");
            public static BlueprintFeatureSelection MysticTheurgeInquisitorLevelSelection4 => Resources.GetBlueprint<BlueprintFeatureSelection>("b93df7bf0405a974cafcda21cbd070f1");
            public static BlueprintFeatureSelection MysticTheurgeInquisitorLevelSelection5 => Resources.GetBlueprint<BlueprintFeatureSelection>("b7ed1fc44730bd1459c57763378a5a97");
            public static BlueprintFeatureSelection MysticTheurgeInquisitorLevelSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("200dec6712442e74c85803a1af72397a");
            public static BlueprintFeatureSelection MysticTheurgeOracleLevelSelection1 => Resources.GetBlueprint<BlueprintFeatureSelection>("3cb30f805e79a7944b0d0174c9a157b7");
            public static BlueprintFeatureSelection MysticTheurgeOracleLevelSelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("5290b422c5c02c44f99291688e798d8f");
            public static BlueprintFeatureSelection MysticTheurgeOracleLevelSelection3 => Resources.GetBlueprint<BlueprintFeatureSelection>("f8cf4644c7ff15d4799a59593b900091");
            public static BlueprintFeatureSelection MysticTheurgeOracleLevelSelection4 => Resources.GetBlueprint<BlueprintFeatureSelection>("7f526cef9fcb7934aaaca1bf3d26fc0f");
            public static BlueprintFeatureSelection MysticTheurgeOracleLevelSelection5 => Resources.GetBlueprint<BlueprintFeatureSelection>("2bf8b6fa7082d45409537d6112cb5647");
            public static BlueprintFeatureSelection MysticTheurgeOracleLevelSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("798bf6e210d9a0e42bf960390b5991ba");
            public static BlueprintFeatureSelection MysticTheurgeOracleLevelSelection7 => Resources.GetBlueprint<BlueprintFeatureSelection>("6bbc04c6c95fc694788ddb265dc15ea2");
            public static BlueprintFeatureSelection MysticTheurgeOracleLevelSelection8 => Resources.GetBlueprint<BlueprintFeatureSelection>("f6c08ecbd77a43b4dbc0252afc2cb578");
            public static BlueprintFeatureSelection MysticTheurgeOracleLevelSelection9 => Resources.GetBlueprint<BlueprintFeatureSelection>("57dae97f6e587f54e9c0559c6fa6590f");
            public static BlueprintFeatureSelection MythicAbilitySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            public static BlueprintFeatureSelection MythicFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("9ee0f6745f555484299b0a1563b99d81");
            public static BlueprintFeatureSelection MythicLichSkeletonSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a9b91b7f3fd9bbd49a942a4b2e98cb11");
            public static BlueprintFeatureSelection NineTailedHeirBloodlineSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7c813fb495d74246918a690ba86f9c86");
            public static BlueprintFeatureSelection NomadMountSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("8e1957da5a8144d1b0fcf8875ac6bab7");
            public static BlueprintFeatureSelection NomadPointBLankMaster => Resources.GetBlueprint<BlueprintFeatureSelection>("07e550ec04bc4cbc8642e41c20519449");
            public static BlueprintFeatureSelection OppositionSchoolSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("6c29030e9fea36949877c43a6f94ff31");
            public static BlueprintFeatureSelection OracleCureOrInflictSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("2cd080fc181122c4a9c5a705abe8ad47");
            public static BlueprintFeatureSelection OracleCurseSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b0a5118b4fb793241bc7042464b23fab");
            public static BlueprintFeatureSelection OracleMysterySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5531b975dcdf0e24c98f1ff7e017e741");
            public static BlueprintFeatureSelection OracleRevelationBondedMount => Resources.GetBlueprint<BlueprintFeatureSelection>("0234d0dd1cead22428e71a2500afa2e1");
            public static BlueprintFeatureSelection OracleRevelationManeuverMasterySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("89629bb513a70cb4596d1d780b95ea72");
            public static BlueprintFeatureSelection OracleRevelationSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("60008a10ad7ad6543b1f63016741a5d2");
            public static BlueprintFeatureSelection OracleRevelationWeaponMasteryImprovedCriticalSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ec155bbe4e3fe2a4f943851a7c25e66b");
            public static BlueprintFeatureSelection OracleRevelationWeaponMasteryWeaponFocusGreaterSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("01d6bcdf7ad8a2a41a51f8fe8d0a1d99");
            public static BlueprintFeatureSelection OracleRevelationWeaponMasteryWeaponFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7e360e025592a734bb6c3ae1210824f3");
            public static BlueprintFeatureSelection OrderOfTheNailFavoriteEnemyRankUp => Resources.GetBlueprint<BlueprintFeatureSelection>("6e887fdf89554dc29fdc3847d522c8d8");
            public static BlueprintFeatureSelection OrderOfTheNailFavoriteEnemySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5dff09eef981483baffb0de48eb07e02");
            public static BlueprintFeatureSelection OrderOfThePawCanineFerocitySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a31848a954134df4b2f048e9fecc3bd2");
            public static BlueprintFeatureSelection OrderOfThePawMountSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7b8b6474cc374a23830341d2a40eecaf");
            public static BlueprintFeatureSelection OreadHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("064e0569f7966e746a76d01ef5dbe963");
            public static BlueprintFeatureSelection OtherworldlyCompanionSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b1c0bcb356a496e4487b7dd8e7521043");
            public static BlueprintFeatureSelection PackRagerTeamworkFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("94ebbd6472c19fa4ea7196eaff11a740");
            public static BlueprintFeatureSelection PaladinDeitySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a7c8b73528d34c2479b4bd638503da1d");
            public static BlueprintFeatureSelection PaladinDivineBondSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ad7dc4eba7bf92f4aba23f716d7a9ba6");
            public static BlueprintFeatureSelection PaladinDivineMountSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("e2f0e0efc9e155e43ba431984429678e");
            public static BlueprintFeatureSelection PossessedShamanSharedSkillSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("9d0477ebd71d43041b419c216b5d6cff");
            public static BlueprintFeatureSelection PrimalistLevel12Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("43b684433ff1e7d439b87099f1717154");
            public static BlueprintFeatureSelection PrimalistLevel16Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("63c42e0d1a84f004c8e4290bb26f359a");
            public static BlueprintFeatureSelection PrimalistLevel20Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("8cfab12649049e44aaa49691a8a16d88");
            public static BlueprintFeatureSelection PrimalistLevel4Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("e6228dd80521e1149abc6257a8279b75");
            public static BlueprintFeatureSelection PrimalistLevel8Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("1c7cc7d948c64e549a45d2524c61de35");
            public static BlueprintFeatureSelection RagePowerSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("28710502f46848d48b3f0d6132817c4e");
            public static BlueprintFeatureSelection RangerStyleArcherySelection10 => Resources.GetBlueprint<BlueprintFeatureSelection>("7ef950ca681955d47bc4efbe77073e2c");
            public static BlueprintFeatureSelection RangerStyleArcherySelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("3cf94df3f1cf9a443a880bc1ae9be3c6");
            public static BlueprintFeatureSelection RangerStyleArcherySelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("6c799d09d5b93f344b9ade0e0c765c2d");
            public static BlueprintFeatureSelection RangerStyleMenacingSelection10 => Resources.GetBlueprint<BlueprintFeatureSelection>("7d5bd07830a5e2c478852bf8b860db2c");
            public static BlueprintFeatureSelection RangerStyleMenacingSelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("1c5988e90e0657645972e7b28ba4ce8b");
            public static BlueprintFeatureSelection RangerStyleMenacingSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("a9998f05a0472bb4f8950304fe26ec0c");
            public static BlueprintFeatureSelection RangerStyleSelection10 => Resources.GetBlueprint<BlueprintFeatureSelection>("78177315fc63b474ea3cbb8df38fafcd");
            public static BlueprintFeatureSelection RangerStyleSelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("c6d0da9124735a44f93ac31df803b9a9");
            public static BlueprintFeatureSelection RangerStyleSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("61f82ba786fe05643beb3cd3910233a8");
            public static BlueprintFeatureSelection RangerStyleShieldSelection10 => Resources.GetBlueprint<BlueprintFeatureSelection>("b2e73b554839a314a8c716dbf33fcfc3");
            public static BlueprintFeatureSelection RangerStyleShieldSelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("2d54cc761ab82604f93d82b4b358cf7e");
            public static BlueprintFeatureSelection RangerStyleShieldSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("10d8245011456224cb6358f640364e26");
            public static BlueprintFeatureSelection RangerStyleTwoHandedSelection10 => Resources.GetBlueprint<BlueprintFeatureSelection>("f96ad7f1fe7e62c4d8f7540d0f4c7313");
            public static BlueprintFeatureSelection RangerStyleTwoHandedSelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("f489d9f696c588d43907b879d89451be");
            public static BlueprintFeatureSelection RangerStyleTwoHandedSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("83a4b2605333e004f875e2fd22f6be30");
            public static BlueprintFeatureSelection RangerStyleTwoWeaponSelection10 => Resources.GetBlueprint<BlueprintFeatureSelection>("ee907845112b8aa4e907cf326e6142a6");
            public static BlueprintFeatureSelection RangerStyleTwoWeaponSelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("019e68517c95c5447bff125b8a91c73f");
            public static BlueprintFeatureSelection RangerStyleTwoWeaponSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("59d5445392ac62245a5ece9b01c05ee8");
            public static BlueprintFeatureSelection ReformedFiendBloodlineSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("dd62cb5011f64cd38b8b08abb19ba2cc");
            public static BlueprintFeatureSelection RogueTalentSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
            public static BlueprintFeatureSelection ScaledFistBonusFeatSelectionLevel1 => Resources.GetBlueprint<BlueprintFeatureSelection>("8705b13f703cbdc43ad1d9150f8cacca");
            public static BlueprintFeatureSelection ScaledFistBonusFeatSelectionLevel10 => Resources.GetBlueprint<BlueprintFeatureSelection>("c569fc66f22825445a7b7f3b5d6d208f");
            public static BlueprintFeatureSelection ScaledFistBonusFeatSelectionLevel6 => Resources.GetBlueprint<BlueprintFeatureSelection>("92f7b37ef1cf5484db02a924592ceb74");
            public static BlueprintFeatureSelection ScaledFistDragonSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("f9042eed12dac2745a2eb7a9a936906b");
            public static BlueprintFeatureSelection ScaledFistKiPowerSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4694f6ac27eaed34abb7d09ab67b4541");
            public static BlueprintFeatureSelection SecondatyElementalFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4204bc10b3d5db440b1f52f0c375848b");
            public static BlueprintFeatureSelection SecondBlessingSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b7ce4a67287cda746a59b31c042305cf");
            public static BlueprintFeatureSelection SecondBloodline => Resources.GetBlueprint<BlueprintFeatureSelection>("3cf2ab2c320b73347a7c21cf0d0995bd");
            public static BlueprintFeatureSelection SecondBloodragerBloodline => Resources.GetBlueprint<BlueprintFeatureSelection>("b7f62628915bdb14d8888c25da3fac56");
            public static BlueprintFeatureSelection SecondBloodragerBloodlineReformedFiend => Resources.GetBlueprint<BlueprintFeatureSelection>("5e4089c46a9f47cdadac7b19d69d11e1");
            public static BlueprintFeatureSelection SecondDomainsSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("43281c3d7fe18cc4d91928395837cd1e");
            public static BlueprintFeatureSelection SecondMystery => Resources.GetBlueprint<BlueprintFeatureSelection>("277b0164740b97945a3f8022bd572f48");
            public static BlueprintFeatureSelection SecondMysteryEnlightenedPhilosopher => Resources.GetBlueprint<BlueprintFeatureSelection>("4ff6f2905e1f4d1b92930b87f85bf86c");
            public static BlueprintFeatureSelection SecondMysteryHerbalist => Resources.GetBlueprint<BlueprintFeatureSelection>("cff5c53fe99c48bf863a0005d768f75a");
            public static BlueprintFeatureSelection SecondMysteryHermit => Resources.GetBlueprint<BlueprintFeatureSelection>("324476155d5c4968babfecda37bcc87c");
            public static BlueprintFeatureSelection SecondSpirit => Resources.GetBlueprint<BlueprintFeatureSelection>("2faa80662a56ab644aec2f875a68597f");
            public static BlueprintFeatureSelection SeekerBloodlineSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("7bda7cdb0ccda664c9eb8978cf512dbc");
            public static BlueprintFeatureSelection SeekerFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c6b609279cc3174478624182ac1ad812");
            public static BlueprintFeatureSelection SelectionMercy => Resources.GetBlueprint<BlueprintFeatureSelection>("02b187038a8dce545bb34bbfb346428d");
            public static BlueprintFeatureSelection ShamanHexBattleMasterSelection16 => Resources.GetBlueprint<BlueprintFeatureSelection>("a658b783516e2e84380a86dce6289bf0");
            public static BlueprintFeatureSelection ShamanHexBattleMasterSelection8 => Resources.GetBlueprint<BlueprintFeatureSelection>("f18fea9a4f78fc94e842c02e24b1dad3");
            public static BlueprintFeatureSelection ShamanHexSecretSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("1342b5bfb876e434797cdcf7311bccad");
            public static BlueprintFeatureSelection ShamanHexSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4223fe18c75d4d14787af196a04e14e7");
            public static BlueprintFeatureSelection ShamanNatureSpiritTrueSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("e7f4cfcd7488ac14bbc3e09426b59fd0");
            public static BlueprintFeatureSelection ShamanSpiritAnimalSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d22f319fefac4ca4b90f03ac5cb9c714");
            public static BlueprintFeatureSelection ShamanSPiritSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("00c8c566d1825dd4a871250f35285982");
            public static BlueprintFeatureSelection SkaldFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("0a1999535b4f77b4d89f689a385e5ec9");
            public static BlueprintFeatureSelection SkaldRagePowerSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("2476514e31791394fa140f1a07941c96");
            public static BlueprintFeatureSelection SkaldTalentSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("d2a8fde8985691045b90e1ec57e3cc57");
            public static BlueprintFeatureSelection SkillFocusMythicFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("a224819b9492019428c220754208ba3a");
            public static BlueprintFeatureSelection SkillFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c9629ef9eebb88b479b2fbc5e836656a");
            public static BlueprintFeatureSelection SlayerTalentSelection10 => Resources.GetBlueprint<BlueprintFeatureSelection>("913b9cf25c9536949b43a2651b7ffb66");
            public static BlueprintFeatureSelection SlayerTalentSelection2 => Resources.GetBlueprint<BlueprintFeatureSelection>("04430ad24988baa4daa0bcd4f1c7d118");
            public static BlueprintFeatureSelection SlayerTalentSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("43d1b15873e926848be2abf0ea3ad9a8");
            public static BlueprintFeatureSelection SoheiMonasticMountHorseSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("918432cc97b146a4b93e2b3060bdd1ed");
            public static BlueprintFeatureSelection SoheiMountedCombatFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("59bd6f915ba1dee44a8316f97fd51967");
            public static BlueprintFeatureSelection SoheiMountedCombatFeatSelection10 => Resources.GetBlueprint<BlueprintFeatureSelection>("8d0ea7db870327b43b44265531be256f");
            public static BlueprintFeatureSelection SoheiMountedCombatFeatSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("46e8b9c91698ac74fa3f2d283d72a970");
            public static BlueprintFeatureSelection SoheiWeaponTrainingSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5f08ae15929f02341a3f328449d443f3");
            public static BlueprintFeatureSelection SorcererBloodlineSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("24bef8d1bee12274686f6da6ccbc8914");
            public static BlueprintFeatureSelection SorcererBonusFeat => Resources.GetBlueprint<BlueprintFeatureSelection>("d6dd06f454b34014ab0903cb1ed2ade3");
            public static BlueprintFeatureSelection SorcererFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("3a60f0c0442acfb419b0c03b584e1394");
            public static BlueprintFeatureSelection SpecialistSchoolSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5f838049069f1ac4d804ce0862ab5110");
            public static BlueprintFeatureSelection SpellSpecializationSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("fe67bc3b04f1cd542b4df6e28b6e0ff5");
            public static BlueprintFeatureSelection StalwartDefenderDefensivePowerSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("2cd91c501bda80b47ac2df0d51b02973");
            public static BlueprintFeatureSelection StudentOfStoneBonusFeatSelectionLevel10 => Resources.GetBlueprint<BlueprintFeatureSelection>("6eabad04593b4f9f966cbdece0a8d3bf");
            public static BlueprintFeatureSelection StudentOfStoneBonusFeatSelectionLevel14 => Resources.GetBlueprint<BlueprintFeatureSelection>("bfca1d29e52f45ae964cf3847d80cbd5");
            public static BlueprintFeatureSelection StudentOfStoneBonusFeatSelectionLevel18 => Resources.GetBlueprint<BlueprintFeatureSelection>("dbe69e6febd74c6fa1e9a56719ad66c6");
            public static BlueprintFeatureSelection StudentOfStoneBonusFeatSelectionLevel6 => Resources.GetBlueprint<BlueprintFeatureSelection>("a4a78358ba604afb8182ce1adffada31");
            public static BlueprintFeatureSelection StudentOfWarAdditionalSKillSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("922cb8ca89628e64f914f06dd4fc2a04");
            public static BlueprintFeatureSelection StudentOfWarCombatFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("78fffe8e5d5bc574a9fd5efbbb364a03");
            public static BlueprintFeatureSelection SwordSaintChosenWeaponSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("3130e3083845b7b4fa2ac5b6f1a1ae6e");
            public static BlueprintFeatureSelection SylvanCompanionSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("bdeb45c295e93644c9fed3b3b431aa2d");
            public static BlueprintFeatureSelection SylvanTricksterTalentSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");
            public static BlueprintFeatureSelection TeamworkFeat => Resources.GetBlueprint<BlueprintFeatureSelection>("90b882830b3988446ae681c6596460cc");
            public static BlueprintFeatureSelection InquisitorTeamworkFeat => Resources.GetBlueprint<BlueprintFeatureSelection>("d87e2f6a9278ac04caeb0f93eff95fcb"); //TeamworkFeat
            public static BlueprintFeatureSelection TerrainExpertiseSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("1c3413f68270d664ea143182f2abe8a9");
            public static BlueprintFeatureSelection TerrainMastery => Resources.GetBlueprint<BlueprintFeatureSelection>("4cd06a915daa74f4094952f2b3314b3b");
            public static BlueprintFeatureSelection ThassilonianSchoolSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("f431178ec0e2b4946a34ab504bb46285");
            public static BlueprintFeatureSelection ThirdElementalFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("e2c1718828fc843479f18ab4d75ded86");
            public static BlueprintFeatureSelection TieflingHeritageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c862fd0e4046d2d4d9702dd60474a181");
            public static BlueprintFeatureSelection TricksterLoreReligionTier2Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("ae4e619162a44996b77973f3abd7781a");
            public static BlueprintFeatureSelection TricksterLoreReligionTier3Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("70a7b101edc24349ab67ac63b6bd0616");
            public static BlueprintFeatureSelection TricksterRank1Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("4fbc563529717de4d92052048143e0f1");
            public static BlueprintFeatureSelection TricksterRank2Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("5cd96c3460844fc458dc3e1656dafa42");
            public static BlueprintFeatureSelection TricksterRank3Selection => Resources.GetBlueprint<BlueprintFeatureSelection>("446f4a8b32019f5478a8dfeddac74710");
            public static BlueprintFeatureSelection TricksterStatFocusFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("0d1d80bd3820a78488412581da3ad9c7");
            public static BlueprintFeatureSelection UndergroundChemistSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("c3d08fdb481aa2e4a9f71c4349141826");
            public static BlueprintFeatureSelection UnletteredArcanistFamiliar => Resources.GetBlueprint<BlueprintFeatureSelection>("cb4d4a8b2c5f7aa49ad0ad74217dbbe3");
            public static BlueprintFeatureSelection UrbanHunterCaptorSelection12 => Resources.GetBlueprint<BlueprintFeatureSelection>("9839caff86642584bbf07699e8238c1b");
            public static BlueprintFeatureSelection UrbanHunterCaptorSelection6 => Resources.GetBlueprint<BlueprintFeatureSelection>("a27990877466ec04e80d7cfa967ed69b");
            public static BlueprintFeatureSelection VivsectionistDiscoverySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("67f499218a0e22944abab6fe1c9eaeee");
            public static BlueprintFeatureSelection WarDomainGreaterFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("79c6421dbdb028c4fa0c31b8eea95f16");
            public static BlueprintFeatureSelection WarpriestChannelEnergySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("1a871294410971745bba12ef11e41f6e");
            public static BlueprintFeatureSelection WarpriestFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("303fd456ddb14437946e344bad9a893b");
            public static BlueprintFeatureSelection WarpriestFervorHealDamageSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b0dc39bfbc6f85646adbfc6413f843b0");
            public static BlueprintFeatureSelection WarpriestShieldbearerChannelEnergySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4d4f00bb16467144780538107387af78");
            public static BlueprintFeatureSelection WarpriestSpontaneousSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("2d26a3364d65c4e4e9fb470172f638a9");
            public static BlueprintFeatureSelection WarpriestWeaponFocusSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ac384183dbfbbd7499410a21d749bef1");
            public static BlueprintFeatureSelection WaterBlastSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("53a8c2f3543147b4d913c6de0c57c7e8");
            public static BlueprintFeatureSelection WeaponMasterySelection => Resources.GetBlueprint<BlueprintFeatureSelection>("55f516d7d1fc5294aba352a5a1c92786");
            public static BlueprintFeatureSelection WeaponTrainingRankUpSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5f3cc7b9a46b880448275763fe70c0b0");
            public static BlueprintFeatureSelection WeaponTrainingSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");
            public static BlueprintFeatureSelection WildTalentBonusFeatAir => Resources.GetBlueprint<BlueprintFeatureSelection>("4ff45d291d0ee9c4b8c83a298b0b4969");
            public static BlueprintFeatureSelection WildTalentBonusFeatAir1 => Resources.GetBlueprint<BlueprintFeatureSelection>("95ab2bdf0d45b2742a357f5780aac4a3");
            public static BlueprintFeatureSelection WildTalentBonusFeatAir2 => Resources.GetBlueprint<BlueprintFeatureSelection>("a8a481c7fbcc9c446a0eecb6e5604405");
            public static BlueprintFeatureSelection WildTalentBonusFeatAir3 => Resources.GetBlueprint<BlueprintFeatureSelection>("a82e3b11fc5935d4289c807b241a2bb5");
            public static BlueprintFeatureSelection WildTalentBonusFeatEarth => Resources.GetBlueprint<BlueprintFeatureSelection>("28bd446d8aeab1341acc8d2fba91e455");
            public static BlueprintFeatureSelection WildTalentBonusFeatEarth1 => Resources.GetBlueprint<BlueprintFeatureSelection>("fc75b0bcfcb5236419d1a47e1bc555a9");
            public static BlueprintFeatureSelection WildTalentBonusFeatEarth2 => Resources.GetBlueprint<BlueprintFeatureSelection>("2205fc9ed34368548b1358a781326bab");
            public static BlueprintFeatureSelection WildTalentBonusFeatEarth3 => Resources.GetBlueprint<BlueprintFeatureSelection>("f593346da04badb4185a47af8e4c4f7f");
            public static BlueprintFeatureSelection WildTalentBonusFeatFire => Resources.GetBlueprint<BlueprintFeatureSelection>("4d14baf0ee4da2a4cb05fb4312921ee4");
            public static BlueprintFeatureSelection WildTalentBonusFeatFire1 => Resources.GetBlueprint<BlueprintFeatureSelection>("0e0207491a09a9e428409c4a1b2871a3");
            public static BlueprintFeatureSelection WildTalentBonusFeatFire2 => Resources.GetBlueprint<BlueprintFeatureSelection>("cd107bf355f84b64f9f472ca288c208b");
            public static BlueprintFeatureSelection WildTalentBonusFeatFire3 => Resources.GetBlueprint<BlueprintFeatureSelection>("0213c0c9062203540bd0365cbde44b99");
            public static BlueprintFeatureSelection WildTalentBonusFeatWater => Resources.GetBlueprint<BlueprintFeatureSelection>("31c18eae013b09c4f9ee51da71a2d61c");
            public static BlueprintFeatureSelection WildTalentBonusFeatWater1 => Resources.GetBlueprint<BlueprintFeatureSelection>("b6f42b59d000228498445526042dfd1b");
            public static BlueprintFeatureSelection WildTalentBonusFeatWater2 => Resources.GetBlueprint<BlueprintFeatureSelection>("b881e2d840eaf6044b0d243b239cccd7");
            public static BlueprintFeatureSelection WildTalentBonusFeatWater3 => Resources.GetBlueprint<BlueprintFeatureSelection>("1d341cc7535e64b4b8e2c53fb6726394");
            public static BlueprintFeatureSelection WildTalentBonusFeatWater4 => Resources.GetBlueprint<BlueprintFeatureSelection>("ebf90f9f8a5e43f40bee85fd6506b922");
            public static BlueprintFeatureSelection WildTalentBonusFeatWater5 => Resources.GetBlueprint<BlueprintFeatureSelection>("40a4fb42aafa7ee4991d3e3140e98856");
            public static BlueprintFeatureSelection WildTalentSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("5c883ae0cd6d7d5448b7a420f51f8459");
            public static BlueprintFeatureSelection WinterWitchShamanHexSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("4e760c5034fafb2438993d8a192150b9");
            public static BlueprintFeatureSelection WinterWitchSpellbookSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("ea20b26d9d0ede540af3c74246dade41");
            public static BlueprintFeatureSelection WinterWitchWitchHexSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("b921af3627142bd4d9cf3aefb5e2610a");
            public static BlueprintFeatureSelection WitchFamiliarSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("29a333b7ccad3214ea3a51943fa0d8e9");
            public static BlueprintFeatureSelection WitchHexSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");
            public static BlueprintFeatureSelection WitchPatronSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("381cf4c890815d049a4420c6f31d063f");
            public static BlueprintFeatureSelection WizardFeatSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("8c3102c2ff3b69444b139a98521a4899");
            public static BlueprintFeatureSelection WizardSchoolSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("8d4637639441f1041bee496f20af7fa3");
            public static BlueprintFeatureSelection ZenArcherBonusFeatSelectionLevel1 => Resources.GetBlueprint<BlueprintFeatureSelection>("69255e0ec330ee94b9593dfabf8f7c09");
            public static BlueprintFeatureSelection ZenArcherBonusFeatSelectionLevel10 => Resources.GetBlueprint<BlueprintFeatureSelection>("15e623d9dba4e314aa0649e4f61e132b");
            public static BlueprintFeatureSelection ZenArcherBonusFeatSelectionLevel6 => Resources.GetBlueprint<BlueprintFeatureSelection>("2a1eec5b782182f4cafbd20fcd069692");
            public static BlueprintFeatureSelection ZenArcherPointBlankMasterSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("50e6aa55653bc014aafe556aad79e5c0");
            public static BlueprintFeatureSelection ZenArcherWayOfTheBowSelection => Resources.GetBlueprint<BlueprintFeatureSelection>("53420038fdc76944695bf927f7bcd51c");

            public static BlueprintFeatureSelection[] FeatSelections = new BlueprintFeatureSelection[] {
                ArcaneRiderFeatSelection,
                BasicFeatSelection,
                BattleScionTeamworkFeat,
                CavalierBonusFeatSelection,
                CavalierMountedMasteryFeatSelection,
                CavalierTacticianFeatSelection,
                DragonLevel2FeatSelection,
                EldritchKnightFeatSelection,
                ExtraFeatMythicFeat,
                ExtraMythicAbilityMythicFeat,
                FighterFeatSelection,
                GendarmeFeatSelection,
                HunterTeamworkFeatSelection,
                LoremasterCombatFeatSelection,
                LoremasterWizardFeatSelection,
                MagusFeatSelection,
                MythicFeatSelection,
                SeekerFeatSelection,
                SkaldFeatSelection,
                SorcererBonusFeat,
                StudentOfWarCombatFeatSelection,
                TeamworkFeat,
                InquisitorTeamworkFeat,
                WarDomainGreaterFeatSelection,
                WarpriestFeatSelection,
                WizardFeatSelection,
            };
        }
    }
}
