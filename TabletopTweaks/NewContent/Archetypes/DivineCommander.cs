using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Archetypes {
    static class DivineCommander {

        private static readonly BlueprintCharacterClass WarpriestClass = Resources.GetBlueprint<BlueprintCharacterClass>("30b5e47d47a0e37438cc5a80c96cfb99");

        private static readonly BlueprintFeature BlessingSelection1 = Resources.GetBlueprint<BlueprintFeature>("6d9dcc2a59210a14891aeedb09d406aa");
        private static readonly BlueprintFeature BlessingSelection2 = Resources.GetBlueprint<BlueprintFeature>("b7ce4a67287cda746a59b31c042305cf");

        private static readonly BlueprintFeature BonusFeatSelection = Resources.GetBlueprint<BlueprintFeature>("303fd456ddb14437946e344bad9a893b");

        private static readonly BlueprintFeatureSelection CavalierMountSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("0605927df6e2fdd42af6ee2424eb89f2");
        private static readonly BlueprintAbility CavalierTacticianAbility = Resources.GetBlueprint<BlueprintAbility>("3ff8ef7ba7b5be0429cf32cd4ddf637c");
        private static readonly BlueprintFeature CavalierTacticianSupportFeature = Resources.GetBlueprint<BlueprintFeature>("37c496c0c2f04544b83a8d013409fd47");
        private static readonly BlueprintFeatureSelection CavalierTacticianFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("7bc55b5e381358c45b42153b8b2603a6");

        private static readonly BlueprintFeature AnimalCompanionFeatureHorse = Resources.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
        private static readonly BlueprintFeature AnimalCompanionFeatureHorse_PreorderBonus = Resources.GetBlueprint<BlueprintFeature>("bfeb9be0a3c9420b8b2beecc8171029c");
        private static readonly BlueprintFeature CavalierMountFeatureWolf = Resources.GetModBlueprint<BlueprintFeature>("CavalierMountFeatureWolf");
        private static readonly BlueprintFeature AnimalCompanionRank = Resources.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

        private static readonly BlueprintBuff MountedBuff = Resources.GetBlueprint<BlueprintBuff>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
        private static readonly BlueprintFeature MountTargetFeature = Resources.GetBlueprint<BlueprintFeature>("cb06f0e72ffb5c640a156bd9f8000c1d");
        private static readonly BlueprintFeature AnimalCompanionArchetypeSelection = Resources.GetBlueprint<BlueprintFeature>("65af7290b4efd5f418132141aaa36c1b");
        private static readonly BlueprintFeature OtherworldlyCompanionFiendish = Resources.GetBlueprint<BlueprintFeature>("4d7607a0155af7d43b49b785f2051e21");

        private static readonly BlueprintFeature TemplateCelestial = Resources.GetModBlueprint<BlueprintFeature>("TemplateCelestial");
        private static readonly BlueprintFeature TemplateEntropic = Resources.GetModBlueprint<BlueprintFeature>("TemplateEntropic");
        private static readonly BlueprintFeature TemplateFiendish = Resources.GetModBlueprint<BlueprintFeature>("TemplateFiendish");
        private static readonly BlueprintFeature TemplateResolute = Resources.GetModBlueprint<BlueprintFeature>("TemplateResolute");

        public static void AddDivineCommander() {

            var DivineCommanderArchetype = Helpers.CreateBlueprint<BlueprintArchetype>("DivineCommanderArchetype", bp => {
                bp.LocalizedName = Helpers.CreateString("DivineCommanderArchetype.Name", "Divine Commander");
                bp.LocalizedDescription = Helpers.CreateString("DivineCommanderArchetype.Description", "Some warpriests are called to lead great armies and" +
                    " face legions of foes. These divine commanders live for war and fight for glory." +
                    " Their hearts quicken at battle cries, and they charge forth with their deity’s symbol held high." +
                    " These leaders of armies do so to promote the agenda of their faith, and lead armies of devoted followers willing to give their lives for the cause.");
            });

            var DivineCommanderMobilityBuff = Helpers.CreateBuff("DivineCommanderMobilityBuff", bp => {
                bp.SetName("Divine Commander Mobility");
                bp.SetDescription("A divine commander does not take an armor check penalty on Mobility checks while riding his mount.");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<NegateArmorCheckSkillPenalty>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Stat = StatType.SkillMobility;
                });
            });

            var DivineCommanderMobilityFeature = Helpers.CreateBlueprint<BlueprintFeature>("DivineCommanderMobilityFeature", bp => {
                bp.SetName("Divine Commander Mobility");
                bp.SetDescription("A divine commander does not take an armor check penalty on Mobility checks while riding his mount.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<BuffExtraEffects>(c => {
                    c.m_CheckedBuff = MountedBuff.ToReference<BlueprintBuffReference>();
                    c.m_ExtraEffectBuff = DivineCommanderMobilityBuff.ToReference<BlueprintBuffReference>();
                });
            });

            var DivineCommanderAnimalCompanionProgression = Helpers.CreateBlueprint<BlueprintProgression>("DivineCommanderAnimalCompanionProgression", bp => {
                bp.SetName("Divine Commander Animal Companion Progression");
                bp.SetName("");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[] {
                    new BlueprintProgression.ArchetypeWithLevel(){
                        m_Archetype = DivineCommanderArchetype.ToReference<BlueprintArchetypeReference>()
                    }
                };
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeatureRankIncrease = new BlueprintFeatureReference();
                bp.LevelEntries = Enumerable.Range(2, 20)
                    .Select(i => new LevelEntry {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            AnimalCompanionRank.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
                bp.UIGroups = new UIGroup[0];
            });

            var DivineCommanderCompanionSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("DivineCommanderCompanionSelection", bp => {
                bp.SetName("Mount");
                bp.SetDescription("A divine commander gains the service of a loyal and trusty steed to carry her into battle. This mount functions " +
                    "as a druid’s animal companion, using the divine commander’s level as her effective druid level. The creature must be one that " +
                    "she is capable of riding and must be suitable as a mount. A Medium divine commander can select a horse. A Small divine commander can select a wolf " +
                    "A divine commander does not take an armor check penalty on Mobility checks while riding this mount.");
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.Mode = SelectionMode.OnlyNew;
                bp.Group = FeatureGroup.AnimalCompanion;
                bp.Ranks = 1;
                bp.m_Icon = CavalierMountSelection.m_Icon;
                bp.IsPrerequisiteFor = new List<BlueprintFeatureReference>();
                bp.AddFeatures(
                    AnimalCompanionFeatureHorse,
                    AnimalCompanionFeatureHorse_PreorderBonus,
                    CavalierMountFeatureWolf
                );
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = DivineCommanderMobilityFeature.ToReference<BlueprintFeatureReference>();
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = DivineCommanderAnimalCompanionProgression.ToReference<BlueprintFeatureReference>();
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = MountTargetFeature.ToReference<BlueprintFeatureReference>();
                });
                bp.AddComponent<AddFeatureOnApply>(c => {
                    c.m_Feature = AnimalCompanionArchetypeSelection.ToReference<BlueprintFeatureReference>();
                });
            });

            var DivineCommanderBlessedMountCelestial = Helpers.CreateBlueprint<BlueprintFeature>("DivineCommanderBlessedMountCelestial", bp => {
                bp.SetName("Blessed Mount — Celestial");
                bp.SetDescription("Animal Companion gains spell resistance equal to its level +5. It also gains:\n" +
                    "1 — 4 HD: resistance 5 to cold, acid, and electricity.\n" +
                    "5 — 10 HD: resistance 10 to cold, acid, and electricity, DR 5/evil\n" +
                    "11+ HD: resistance 15 to cold, acid, and electricity, DR 10/evil\n" +
                    "Smite Evil (Su): Once per day, the celestial creature may smite a evil-aligned creature. As a swift action, " +
                    "the creature chooses one target within sight to smite. If this target is evil, the creature adds its Charisma bonus (if any) to " +
                    "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
                bp.m_Icon = OtherworldlyCompanionFiendish.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = TemplateCelestial.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisite<PrerequisiteAlignment>(p => {
                    p.Alignment = AlignmentMaskType.Good | AlignmentMaskType.TrueNeutral;
                    p.HideInUI = true;
                });
            });

            var DivineCommanderBlessedMountEntropic = Helpers.CreateBlueprint<BlueprintFeature>("DivineCommanderBlessedMountEntropic", bp => {
                bp.SetName("Blessed Mount — Entropic");
                bp.SetDescription("Animal Companion gains spell resistance equal to its level +5. It also gains:\n" +
                    "1 — 4 HD: resistance 5 to acid and fire.\n" +
                    "5 — 10 HD: resistance 10 to acid and fire, DR 5/lawful\n" +
                    "11+ HD: resistance 15 to acid and fire, DR 10/lawful\n" +
                    "Smite Law (Su): Once per day, the entropic creature may smite a law-aligned creature. As a swift action, " +
                    "the creature chooses one target within sight to smite. If this target is lawful, the creature adds its Charisma bonus (if any) to " +
                    "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
                bp.m_Icon = OtherworldlyCompanionFiendish.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = TemplateEntropic.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisite<PrerequisiteAlignment>(p => {
                    p.Alignment = AlignmentMaskType.Chaotic | AlignmentMaskType.TrueNeutral;
                    p.HideInUI = true;
                });
            });

            var DivineCommanderBlessedMountFiendish = Helpers.CreateBlueprint<BlueprintFeature>("DivineCommanderBlessedMountFiendish", bp => {
                bp.SetName("Blessed Mount — Fiendish");
                bp.SetDescription("Animal Companion gains spell resistance equal to its level +5. It also gains:\n" +
                    "1 — 4 HD: resistance 5 to cold and fire.\n" +
                    "5 — 10 HD: resistance 10 to cold and fire, DR 5/good\n" +
                    "11+ HD: resistance 15 to cold and fire, DR 10/good\n" +
                    "Smite Good (Su): Once per day, the fiendish creature may smite a good-aligned creature. As a swift action, " +
                    "the creature chooses one target within sight to smite. If this target is good, the creature adds its Charisma bonus (if any) to " +
                    "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
                bp.m_Icon = OtherworldlyCompanionFiendish.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = TemplateFiendish.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisite<PrerequisiteAlignment>(p => {
                    p.Alignment = AlignmentMaskType.Evil | AlignmentMaskType.TrueNeutral;
                    p.HideInUI = true;
                });
            });

            var DivineCommanderBlessedMountResolute = Helpers.CreateBlueprint<BlueprintFeature>("DivineCommanderBlessedMountResolute", bp => {
                bp.SetName("Blessed Mount — Resolute");
                bp.SetDescription("Animal Companion gains spell resistance equal to its level +5. It also gains:\n" +
                    "1 — 4 HD: resistance 5 to acid, cold and fire.\n" +
                    "5 — 10 HD: resistance 10 to acid, cold and fire, DR 5/chaotic\n" +
                    "11+ HD: resistance 15 to acid, cold and fire, DR 10/chaotic\n" +
                    "Smite Chaos (Su): Once per day, the resolute creature may smite a chaos-aligned creature. As a swift action, " +
                    "the creature chooses one target within sight to smite. If this target is chaotic, the creature adds its Charisma bonus (if any) to " +
                    "attack rolls and gains a damage bonus equal to its HD against that foe. This effect persists until the target is dead or the creature rests.");
                bp.m_Icon = OtherworldlyCompanionFiendish.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = TemplateResolute.ToReference<BlueprintFeatureReference>();
                });
                bp.AddPrerequisite<PrerequisiteAlignment>(p => {
                    p.Alignment = AlignmentMaskType.Lawful | AlignmentMaskType.TrueNeutral;
                    p.HideInUI = true;
                });
            });
            var DivineCommanderBlessedMount = Helpers.CreateBlueprint<BlueprintFeatureSelection>("DivineCommanderBlessedMount", bp => {
                bp.SetName("Blessed Mount");
                bp.SetDescription("At 6th level, a divine commander’s mount becomes a creature blessed by his deity. The divine commander’s mount gains either the celestial, " +
                    "entropic, fiendish, or resolute template, matching the alignment of the warpriest (celestial for good, entropic for chaotic, fiendish for evil, " +
                    "and resolute for lawful). If the warpriest matches more than one alignment, the divine commander can select which of the two " +
                    "templates the mount receives. Once the type of template is selected, it cannot be changed.\n" +
                    "If the divine commander is neutral with no other alignment components, she may selecy any template.");
                bp.m_Icon = OtherworldlyCompanionFiendish.Icon;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddFeatures(
                    DivineCommanderBlessedMountCelestial,
                    DivineCommanderBlessedMountEntropic,
                    DivineCommanderBlessedMountFiendish,
                    DivineCommanderBlessedMountResolute
                );
            });

            var DivineCommanderBattleTacticianResource = Helpers.CreateBlueprint<BlueprintAbilityResource>("DivineCommanderBattleTacticianResource", bp => {
                bp.m_Min = 1;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1,
                    IncreasedByStat = false,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[] {
                        WarpriestClass.ToReference<BlueprintCharacterClassReference>()
                    },
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[] {
                        DivineCommanderArchetype.ToReference<BlueprintArchetypeReference>()
                    },
                    IncreasedByLevelStartPlusDivStep = true,
                    StartingLevel = 9,
                    LevelStep = 6,
                    PerStepIncrease = 1,
                    StartingIncrease = 1
                };
            });

            var DivineCommanderBattleTacticianSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("DivineCommanderBattleTacticianSelection", bp => {
                bp.SetName("Battle Tactician — Bonus Feat");
                bp.SetDescription("");
                bp.m_Icon = CavalierTacticianAbility.Icon;
                bp.m_Features = CavalierTacticianFeatSelection.m_Features;
                bp.m_AllFeatures = CavalierTacticianFeatSelection.m_AllFeatures;
                bp.Group = CavalierTacticianFeatSelection.Group;
                bp.Groups = CavalierTacticianFeatSelection.Groups;
                bp.IsClassFeature = true;
            });

            var DivineCommanderBattleTacticianAbility = Helpers.CreateBlueprint<BlueprintAbility>("DivineCommanderBattleTacticianAbility", bp => {
                bp.SetName("Battle Tactician");
                bp.SetDescription("At 3rd level, a divine commander gains a teamwork feat as a bonus feat." +
                    " She must meet the prerequisites for this feat." +
                    " As a standard action, the divine commander can grant this feat to all allies within 30 feet who can see and hear her." +
                    " Allies retain the use of this bonus feat for 4 rounds, plus 1 round for every 2 levels beyond 3rd that the divine commander possesses." +
                    " Allies do not need to meet the prerequisites of this bonus feat." +
                    " The divine commander can use this ability once per day at 3rd level, plus one additional time per day at 9th and 15th levels.");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "");
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.SavingThrow", "");
                bp.m_Icon = CavalierTacticianAbility.Icon;
                bp.Type = AbilityType.Extraordinary;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetSelf = true;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.AddComponent<AbilityTargetsAround>(c => {
                    c.m_Radius = 30.Feet();
                    c.m_TargetType = TargetType.Ally;
                    c.m_Condition = new ConditionsChecker();
                });
                bp.AddComponent(Helpers.CreateContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MaxClassLevelWithArchetype;
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 5;
                    c.m_StepLevel = 2;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[] { WarpriestClass.ToReference<BlueprintCharacterClassReference>() };
                    c.Archetype = DivineCommanderArchetype.ToReference<BlueprintArchetypeReference>();
                }));
                bp.AddComponent<AbilityApplyFact>(c => {
                    c.m_Restriction = AbilityApplyFact.FactRestriction.CasterHasFact;
                    c.m_Facts = CavalierTacticianAbility.GetComponent<AbilityApplyFact>().m_Facts;
                    c.m_HasDuration = true;
                    c.m_Duration = new ContextDurationValue() {
                        m_IsExtendable = true,
                        DiceType = Kingmaker.RuleSystem.DiceType.One,
                        DiceCountValue = 4,
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        }
                    };
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                    c.m_RequiredResource = DivineCommanderBattleTacticianResource.ToReference<BlueprintAbilityResourceReference>();
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionSpawnFx() {
                            PrefabLink = CavalierTacticianAbility.FlattenAllActions().OfType<ContextActionSpawnFx>().First().PrefabLink
                        }
                    );
                });
            });

            var DivineCommanderBattleTacticianAbilitySwift = Helpers.CreateBlueprint<BlueprintAbility>("DivineCommanderBattleTacticianAbilitySwift", bp => {
                bp.SetName("Battle Tactician (Swift)");
                bp.SetDescription("At 12th level, the divine commander gains an additional teamwork feat as a bonus feat." +
                    " She must meet the prerequisites for this feat." +
                    " The divine commander can grant this feat to her allies using the battle tactician ability." +
                    " Additionally, using the battle tactician ability is now a swift action.");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "");
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.SavingThrow", "");
                bp.m_Icon = CavalierTacticianAbility.Icon;
                bp.Type = AbilityType.Extraordinary;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetSelf = true;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = UnitCommand.CommandType.Swift;
                bp.ComponentsArray = DivineCommanderBattleTacticianAbility.ComponentsArray;
            });

            var DivineCommanderBattleTacticianFeature = Helpers.CreateBlueprint<BlueprintFeature>("DivineCommanderBattleTacticianFeature", bp => {
                bp.SetName("Battle Tactician");
                bp.SetDescription(DivineCommanderBattleTacticianAbility.Description);
                bp.m_Icon = CavalierTacticianAbility.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = DivineCommanderBattleTacticianResource.ToReference<BlueprintAbilityResourceReference>();
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        DivineCommanderBattleTacticianAbility.ToReference<BlueprintUnitFactReference>(),
                        CavalierTacticianSupportFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });

            var DivineCommanderBattleTacticianGreaterFeature = Helpers.CreateBlueprint<BlueprintFeature>("DivineCommanderBattleTacticianGreaterFeature", bp => {
                bp.SetName("Greater Battle Tactician");
                bp.SetDescription(DivineCommanderBattleTacticianAbilitySwift.Description);
                bp.m_Icon = CavalierTacticianAbility.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddFeatureIfHasFact>(c => {
                    c.m_CheckedFact = DivineCommanderBattleTacticianAbilitySwift.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = DivineCommanderBattleTacticianAbilitySwift.ToReference<BlueprintUnitFactReference>();
                    c.Not = true;
                });
            });
            DivineCommanderArchetype.RemoveFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, BlessingSelection1, BlessingSelection2),
                    Helpers.CreateLevelEntry(3, BonusFeatSelection),
                    Helpers.CreateLevelEntry(6, BonusFeatSelection),
                    Helpers.CreateLevelEntry(12, BonusFeatSelection)
                };
            DivineCommanderArchetype.AddFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, DivineCommanderCompanionSelection),
                Helpers.CreateLevelEntry(3, DivineCommanderBattleTacticianFeature, DivineCommanderBattleTacticianSelection),
                Helpers.CreateLevelEntry(6, DivineCommanderBlessedMount),
                Helpers.CreateLevelEntry(12, DivineCommanderBattleTacticianGreaterFeature, DivineCommanderBattleTacticianSelection)
            };

            if (ModSettings.AddedContent.Archetypes.IsDisabled("DivineCommander")) { return; }
            WarpriestClass.m_Archetypes = WarpriestClass.m_Archetypes.AppendToArray(DivineCommanderArchetype.ToReference<BlueprintArchetypeReference>());
            WarpriestClass.Progression.UIGroups = WarpriestClass.Progression.UIGroups.AppendToArray(
                Helpers.CreateUIGroup(DivineCommanderCompanionSelection, DivineCommanderBlessedMount),
                Helpers.CreateUIGroup(DivineCommanderBattleTacticianFeature, DivineCommanderBattleTacticianGreaterFeature),
                Helpers.CreateUIGroup(DivineCommanderBattleTacticianSelection)
            );
            Main.LogPatch("Added", DivineCommanderArchetype);
        }
    }
}
