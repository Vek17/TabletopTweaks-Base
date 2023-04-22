using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Inquisitor {
        public static BlueprintFeatureSelection InquisitorAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var TrueJudgmentFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("f069b6557a2013544ac3636219186632");
            var CavalierTacticianAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("3ff8ef7ba7b5be0429cf32cd4ddf637c");
            var CavalierTacticianSupportFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("37c496c0c2f04544b83a8d013409fd47");
            var CavalierTacticianFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7bc55b5e381358c45b42153b8b2603a6");

            var TeamLeaderResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "TeamLeaderResource", bp => {
                bp.m_Min = 1;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 3,
                    IncreasedByStat = false,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    IncreasedByLevelStartPlusDivStep = false,
                };
            });

            var TeamLeaderAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "TeamLeaderAbility", bp => {
                bp.SetName(TTTContext, "Team Leader");
                bp.SetDescription(TTTContext, "As a standard action the inquisitor can spend a standard action granting characters up to three of the inquisitor’s " +
                    "teamwork feats (the inquisitor’s choice) as bonus feats for the next 24 hours.");
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.Duration", "");
                bp.LocalizedSavingThrow = Helpers.CreateString(TTTContext, $"{bp.name}.SavingThrow", "");
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
                bp.AddComponent<AbilityApplyFact>(c => {
                    c.m_Restriction = AbilityApplyFact.FactRestriction.CasterHasFact;
                    c.m_Facts = CavalierTacticianAbility.GetComponent<AbilityApplyFact>().m_Facts;
                    c.m_HasDuration = true;
                    c.m_Duration = new ContextDurationValue() {
                        m_IsExtendable = true,
                        Rate = DurationRate.Days,
                        DiceCountValue = 0,
                        BonusValue = 1
                    };
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                    c.m_RequiredResource = TeamLeaderResource.ToReference<BlueprintAbilityResourceReference>();
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionSpawnFx() {
                            PrefabLink = CavalierTacticianAbility.FlattenAllActions().OfType<ContextActionSpawnFx>().First().PrefabLink
                        }
                    );
                });
            });
            var TeamLeader = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TeamLeaderFeature", bp => {
                bp.SetName(TTTContext, "Team Leader");
                bp.SetDescription(TTTContext, "At 20th level, the inquisitor has grown accustomed to teaching farmers to fight, but what she can do with trained warriors is far more terrifying.\n" +
                    "As a standard action the inquisitor can spend a standard action granting characters up to three of the inquisitor’s " +
                    "teamwork feats (the inquisitor’s choice) as bonus feats for the next 24 hours.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_Icon = CavalierTacticianAbility.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = TeamLeaderResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        TeamLeaderAbility.ToReference<BlueprintUnitFactReference>(),
                        CavalierTacticianSupportFeature.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            InquisitorAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "InquisitorAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = TrueJudgmentFeature.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    TrueJudgmentFeature,
                    TeamLeader,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
        public static void UpdateTeamworkFeats() {
            var TeamLeaderAbility = BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "TeamLeaderAbility");
            var CavalierTacticianAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("3ff8ef7ba7b5be0429cf32cd4ddf637c");

            TeamLeaderAbility.TemporaryContext(bp => {
                bp.GetComponent<AbilityApplyFact>()?.TemporaryContext(c => {
                    c.m_Restriction = AbilityApplyFact.FactRestriction.CasterHasFact;
                    c.m_Facts = CavalierTacticianAbility.GetComponent<AbilityApplyFact>().m_Facts;
                    c.m_HasDuration = true;
                    c.m_Duration = new ContextDurationValue() {
                        m_IsExtendable = true,
                        Rate = DurationRate.Days,
                        DiceCountValue = 0,
                        BonusValue = 1
                    };
                });
            });
        }
    }
}
