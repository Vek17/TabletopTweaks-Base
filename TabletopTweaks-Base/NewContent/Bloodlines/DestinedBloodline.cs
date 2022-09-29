using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Bloodlines {
    class DestinedBloodline {

        static BlueprintFeatureReference BloodlineRequisiteFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "BloodlineRequisiteFeature").ToReference<BlueprintFeatureReference>();
        static BlueprintFeatureReference DestinedBloodlineRequisiteFeature = CreateBloodlineRequisiteFeature();

        static BlueprintFeatureReference CreateBloodlineRequisiteFeature() {
            var AberrantBloodlineRequisiteFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "DestinedBloodlineRequisiteFeature", bp => {
                bp.SetName(TTTContext, "Destined Bloodline");
                bp.SetDescription(TTTContext, "Destined Bloodline Requisite Feature");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
            });
            return AberrantBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
        }
        public static void AddBloodragerDestinedBloodline() {
            var BloodragerStandardRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
            var BloodragerClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499").ToReference<BlueprintCharacterClassReference>();
            var GreenragerArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("5648585af75596f4a9fa3ae385127f57").ToReference<BlueprintArchetypeReference>();
            //Used Assets
            var TrueStrike = BlueprintTools.GetBlueprint<BlueprintAbility>("2c38da66e5a599347ac95b3294acbe00");
            var LuckDomain = BlueprintTools.GetBlueprint<BlueprintAbility>("9af0b584f6f754045a0a79293d100ab3");
            //Bonus Spells
            var MageShield = BlueprintTools.GetBlueprint<BlueprintAbility>("ef768022b0785eb43a18969903c537c4").ToReference<BlueprintAbilityReference>();
            var Blur = BlueprintTools.GetBlueprint<BlueprintAbility>("14ec7a4e52e90fa47a4c8d63c69fd5c1").ToReference<BlueprintAbilityReference>();
            var ProtectionFromEnergy = BlueprintTools.GetBlueprint<BlueprintAbility>("d2f116cfe05fcdd4a94e80143b67046f").ToReference<BlueprintAbilityReference>();
            var FreedomOfMovement = BlueprintTools.GetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e").ToReference<BlueprintAbilityReference>();
            //Bonus Feats
            var Diehard = BlueprintTools.GetBlueprint<BlueprintFeature>("86669ce8759f9d7478565db69b8c19ad").ToReference<BlueprintFeatureReference>();
            var Endurance = BlueprintTools.GetBlueprint<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174").ToReference<BlueprintFeatureReference>();
            var ImprovedInitiative = BlueprintTools.GetBlueprint<BlueprintFeature>("797f25d709f559546b29e7bcb181cc74").ToReference<BlueprintFeatureReference>();
            var IntimidatingProwess = BlueprintTools.GetBlueprint<BlueprintFeature>("d76497bfc48516e45a0831628f767a0f").ToReference<BlueprintFeatureReference>();
            var SiezeTheMoment = BlueprintTools.GetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760").ToReference<BlueprintFeatureReference>();
            var LightningReflexes = BlueprintTools.GetBlueprint<BlueprintFeature>("15e7da6645a7f3d41bdad7c8c4b9de1e").ToReference<BlueprintFeatureReference>();
            var WeaponFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e").ToReference<BlueprintFeatureReference>();
            //Bloodline Powers
            var BloodragerDestinedStrikeResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "BloodragerDestinedStrikeResource", bp => {
                bp.m_Min = 0;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 3,
                    IncreasedByStat = false,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0]
                };
            });
            var BloodragerDestinedStrikeResourceIncrease = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedStrikeResourceIncrease", bp => {
                bp.SetName(TTTContext, "Destined Strike Extra Uses");
                bp.SetDescription(TTTContext, "");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = BloodragerDestinedStrikeResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = 2;
                });
            });
            var BloodragerDestinedStrikeBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BloodragerDestinedStrikeBuff", bp => {
                bp.Stacking = StackingType.Rank;
                bp.Ranks = 5;
                bp.SetName(TTTContext, "Destined Strike");
                bp.SetDescription(TTTContext, "You can grant yourself an insight bonus equal to 1/2 your bloodrager level (minimum 1) on one melee attack.");
                bp.IsClassFeature = true;
                bp.m_Icon = TrueStrike.Icon;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Insight;
                    c.Stat = StatType.AdditionalAttackBonus;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<RemoveBuffRankOnAttack>();
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_StartLevel = 1;
                    c.m_StepLevel = 2;
                    c.m_Max = 20;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                    c.m_Class = new BlueprintCharacterClassReference[] { BloodragerClass };
                });
            });
            var BloodragerDestinedStrikeAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "BloodragerDestinedAbility", bp => {
                bp.SetName(TTTContext, "Destined Strike");
                bp.SetDescription(TTTContext, "At 1st level, as a free action up to three times per day you can grant yourself an insight bonus equal to 1/2 your "
                    + "bloodrager level (minimum 1) on one melee attack. At 12th level, you can use this ability up to five times per day.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.m_Icon = TrueStrike.Icon;
                bp.Type = AbilityType.Supernatural;
                bp.ResourceAssetIds = TrueStrike.ResourceAssetIds;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = BloodragerDestinedStrikeResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                });
                var addInsightBonus = Helpers.Create<ContextActionApplyBuff>(c => {
                    c.m_Buff = BloodragerDestinedStrikeBuff.ToReference<BlueprintBuffReference>();
                    c.IsNotDispelable = true;
                    c.Permanent = true;
                    c.DurationValue = new ContextDurationValue() {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        },
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 1
                        }
                    };
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] { addInsightBonus };
                });
            });
            var BloodragerDestinedStrike = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedStrike", bp => {
                bp.SetName(BloodragerDestinedStrikeAbility.m_DisplayName);
                bp.SetDescription(BloodragerDestinedStrikeAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BloodragerDestinedStrikeAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = BloodragerDestinedStrikeResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BloodragerDestinedStrikeAbility.Icon;
            });
            var BloodragerDestinedFatedBloodrager = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedFatedBloodrager", bp => {
                bp.SetName(TTTContext, "Fated Bloodrager");
                bp.SetDescription(TTTContext, "At 4th level, you gain a +1 luck bonus to AC and on saving throws. At 8th level and every "
                    + "4 levels thereafter, this bonus increases by 1 (to a maximum of +5 at 20th level).");
                bp.IsClassFeature = true;
                bp.Ranks = 5;
            });
            var BloodragerDestinedFatedBloodragerBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BloodragerDestinedFatedBloodragerBuff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName(BloodragerDestinedFatedBloodrager.m_DisplayName);
                bp.SetDescription(BloodragerDestinedFatedBloodrager.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = StatType.AC;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = StatType.SaveFortitude;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = StatType.SaveReflex;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = StatType.SaveWill;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_Feature = BloodragerDestinedFatedBloodrager.ToReference<BlueprintFeatureReference>();
                    c.m_Progression = ContextRankProgression.AsIs;
                });
            });
            var BloodragerDestinedCertainStrike = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedCertainStrike", bp => {
                bp.SetName(TTTContext, "Certain Strike");
                bp.SetDescription(TTTContext, "At 8th level, you may reroll an attack roll once during a bloodrage. You must decide to use this ability after "
                    + "the die is rolled, but before the GM reveals the results. You must take the second result, even if it’s worse.");
                bp.IsClassFeature = true;
            });
            var BloodragerDestinedCertainStrikeBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BloodragerDestinedCertainStrikeBuff", bp => {
                //bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.m_Icon = LuckDomain.Icon;
                bp.SetName(BloodragerDestinedCertainStrike.m_DisplayName);
                bp.SetDescription(BloodragerDestinedCertainStrike.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<ModifyD20>(c => {
                    c.RerollOnlyIfFailed = true;
                    c.Rule = RuleType.AttackRoll;
                    c.DispellOnRerollFinished = true;
                    c.RollsAmount = 1;
                    c.TakeBest = true;
                    c.Bonus = new ContextValue();
                    c.Chance = new ContextValue();
                    c.Value = new ContextValue();
                    c.Skill = new StatType[0];
                });
            });
            var BloodragerDestinedDefyDeathResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "BloodragerDestinedDefyDeathResource", bp => {
                bp.m_Min = 0;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1,
                    IncreasedByStat = false,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0]
                };
            });
            var BloodragerDestinedDefyDeath = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedDefyDeath", bp => {
                bp.SetName(TTTContext, "Defy Death");
                bp.SetDescription(TTTContext, "At 12th level, once per day when an attack or spell that deals damage would result in your death"
                    + ", you can attempt a DC 20 Fortitude save. If you succeed, you are instead reduced to 1 hit point; if you "
                    + "succeed and already have less than 1 hit point, you instead take no damage.");
                bp.IsClassFeature = true;
                bp.AddComponent(Helpers.Create<AddAbilityResources>(c => {
                    c.m_Resource = BloodragerDestinedDefyDeathResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                }));
            });
            var BloodragerDestinedDefyDeathBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BloodragerDestinedDefyDeathBuff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName(BloodragerDestinedDefyDeath.m_DisplayName);
                bp.SetDescription(BloodragerDestinedDefyDeath.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<SurviveDeathWithSave>(c => {
                    c.DC = 20;
                    c.Type = SavingThrowType.Fortitude;
                    c.TargetHP = 1;
                    c.BlockIfBelowZero = true;
                    c.Resource = BloodragerDestinedDefyDeathResource.ToReference<BlueprintAbilityResourceReference>();
                    c.SpendAmount = 1;
                });
            });
            var BloodragerDestinedUnstoppable = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedUnstoppable", bp => {
                bp.SetName(TTTContext, "Unstoppable");
                bp.SetDescription(TTTContext, "At 16th level, any critical threats you score are automatically confirmed. Any critical "
                    + "threats made against you confirm only if the second roll results in a natural 20 (or is automatically confirmed).");
                bp.IsClassFeature = true;
            });
            var BloodragerDestinedUnstoppableBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BloodragerDestinedUnstoppableBuff", bp => {
                bp.SetName(BloodragerDestinedUnstoppable.m_DisplayName);
                bp.SetDescription(BloodragerDestinedUnstoppable.m_Description);
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<CriticalConfirmationACBonus>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Simple,
                    };
                    c.Bonus = 200;
                });
                bp.AddComponent(Helpers.Create<InitiatorCritAutoconfirm>());
            });
            var BloodragerDestinedVictoryOrDeath = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedVictoryOrDeath", bp => {
                bp.SetName(TTTContext, "Victory or Death");
                bp.SetDescription(TTTContext, "At 20th level, you are immune to paralysis and petrification, as well as to the stunned, dazed, "
                    + "and staggered conditions. You have these benefits constantly, even while not bloodraging.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Paralyzed;
                });
                bp.AddComponent<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Petrified;
                });
                bp.AddComponent<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Stunned;
                });
                bp.AddComponent<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Dazed;
                });
                bp.AddComponent<AddConditionImmunity>(c => {
                    c.Condition = UnitCondition.Staggered;
                });
                bp.AddComponent<SpellImmunityToSpellDescriptor>(c => {
                    c.Descriptor = SpellDescriptor.Paralysis
                    | SpellDescriptor.Petrified
                    | SpellDescriptor.Stun
                    | SpellDescriptor.Daze
                    | SpellDescriptor.Staggered;
                });
                bp.AddComponent<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.Paralysis
                    | SpellDescriptor.Petrified
                    | SpellDescriptor.Stun
                    | SpellDescriptor.Daze
                    | SpellDescriptor.Staggered;
                });
            });
            //Bloodline Feats
            var BloodragerDestinedFeatSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "BloodragerDestinedFeatSelection", bp => {
                bp.SetName(TTTContext, "Bonus Feats");
                bp.SetDescription(TTTContext, "Bonus Feats: Diehard, Endurance, Improved Initiative, Intimidating Prowess, Sieze The Moment, Lightning Reflexes, Weapon Focus.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;

                bp.m_Features = new BlueprintFeatureReference[] {
                    Diehard,
                    Endurance,
                    ImprovedInitiative,
                    IntimidatingProwess,
                    SiezeTheMoment,
                    LightningReflexes,
                    WeaponFocus
                };
                bp.m_AllFeatures = bp.m_Features;
            });
            var BloodragerDestinedFeatSelectionGreenrager = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "BloodragerDestinedFeatSelectionGreenrager", bp => {
                bp.SetName(BloodragerDestinedFeatSelection.m_DisplayName);
                bp.SetDescription(BloodragerDestinedFeatSelection.m_Description);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.HideNotAvailibleInUI = true;

                bp.m_Features = BloodragerDestinedFeatSelection.m_Features;
                bp.m_AllFeatures = bp.m_Features;
                bp.AddComponent<PrerequisiteNoArchetype>(c => {
                    c.HideInUI = true;
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Archetype = GreenragerArchetype;
                });
            });
            //Bloodline Spells
            var BloodragerDestinedSpell7 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedSpell7", bp => {
                var spell = MageShield;
                bp.SetName(TTTContext, $"Bonus Spell — Shield");
                bp.SetDescription(TTTContext, "At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 1;
                });
            });
            var BloodragerDestinedSpell10 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedSpell10", bp => {
                var spell = Blur;
                bp.SetName(TTTContext, $"Bonus Spell — Blur");
                bp.SetDescription(TTTContext, "At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 2;
                });
            });
            var BloodragerDestinedSpell13 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedSpell13", bp => {
                var spell = ProtectionFromEnergy;
                bp.SetName(TTTContext, $"Bonus Spell — Protection From Energy");
                bp.SetDescription(TTTContext, "At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 3;
                });
            });
            var BloodragerDestinedSpell16 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BloodragerDestinedSpell16", bp => {
                var spell = FreedomOfMovement;
                bp.SetName(TTTContext, $"Bonus Spell — Freedom Of Movement");
                bp.SetDescription(TTTContext, "At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 4;
                });
            });
            //Bloodline Core
            var BloodragerDestinedBloodline = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "BloodragerDestinedBloodline", bp => {
                bp.SetName(TTTContext, "Destined");
                bp.SetDescription(TTTContext, "Your bloodline is destined for great things. When you bloodrage, you exude a greatness that makes all but the most legendary creatures seem lesser.\n"
                    + "Your future greatness grants you the might to strike your enemies with awe.\n"
                    + BloodragerDestinedFeatSelection.Description
                    + "\nBonus Spells: Shield (7th), Blur (10th), Protection From Energy (13th), Freedom Of Movement (16th).");
                bp.IsClassFeature = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = BloodragerClass
                    }
                };
                bp.GiveFeaturesForPreviousLevels = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.BloodragerBloodline };
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.LevelEntries = new LevelEntry[] {
                    new LevelEntry(){ Level = 1, Features = { BloodragerDestinedStrike, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature }},
                    new LevelEntry(){ Level = 4, Features = { BloodragerDestinedFatedBloodrager }},
                    new LevelEntry(){ Level = 6, Features = { BloodragerDestinedFeatSelectionGreenrager }},
                    new LevelEntry(){ Level = 7, Features = { BloodragerDestinedSpell7 }},
                    new LevelEntry(){ Level = 8, Features = { BloodragerDestinedCertainStrike, BloodragerDestinedFatedBloodrager }},
                    new LevelEntry(){ Level = 9, Features = { BloodragerDestinedFeatSelectionGreenrager }},
                    new LevelEntry(){ Level = 10, Features = { BloodragerDestinedSpell10 }},
                    new LevelEntry(){ Level = 12, Features = { BloodragerDestinedFeatSelection, BloodragerDestinedDefyDeath, BloodragerDestinedFatedBloodrager, BloodragerDestinedStrikeResourceIncrease }},
                    new LevelEntry(){ Level = 13, Features = { BloodragerDestinedSpell13 }},
                    new LevelEntry(){ Level = 15, Features = { BloodragerDestinedFeatSelection }},
                    new LevelEntry(){ Level = 16, Features = { BloodragerDestinedUnstoppable, BloodragerDestinedSpell16, BloodragerDestinedFatedBloodrager }},
                    new LevelEntry(){ Level = 18, Features = { BloodragerDestinedFeatSelection }},
                    new LevelEntry(){ Level = 20, Features = { BloodragerDestinedVictoryOrDeath, BloodragerDestinedFatedBloodrager }},
                };
                bp.AddPrerequisite<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature;
                });
                bp.AddPrerequisite<PrerequisiteFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = DestinedBloodlineRequisiteFeature;
                });
                bp.UIGroups = new UIGroup[] {
                    Helpers.CreateUIGroup(BloodragerDestinedFeatSelection, BloodragerDestinedFeatSelectionGreenrager)
                };
            });
            var BloodragerAberrantBloodlineWandering = BloodlineTools.CreateMixedBloodFeature(TTTContext, "BloodragerDestinedBloodlineWandering", BloodragerDestinedBloodline, bp => {
                bp.m_Icon = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_DestinedBloodline.png");
            });
            var BloodragerDestinedBaseBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BloodragerDestinedBaseBuff", bp => {
                bp.SetName(TTTContext, "Destined Bloodrage");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });

            BloodragerDestinedBaseBuff.AddConditionalBuff(BloodragerDestinedFatedBloodrager, BloodragerDestinedFatedBloodragerBuff);
            BloodragerDestinedBaseBuff.AddConditionalBuff(BloodragerDestinedCertainStrike, BloodragerDestinedCertainStrikeBuff);
            BloodragerDestinedBaseBuff.AddConditionalBuff(BloodragerDestinedDefyDeath, BloodragerDestinedDefyDeathBuff);
            BloodragerDestinedBaseBuff.AddConditionalBuff(BloodragerDestinedUnstoppable, BloodragerDestinedUnstoppableBuff);
            BloodragerDestinedBaseBuff.RemoveBuffAfterRage(BloodragerDestinedStrikeBuff);

            //Register Bloodrage Abilities
            BloodragerDestinedBaseBuff.ApplyBloodrageRestriction(BloodragerDestinedStrikeAbility);
            BloodragerStandardRageBuff.AddConditionalBuff(BloodragerDestinedBloodline, BloodragerDestinedBaseBuff);

            BloodlineTools.ApplyPrimalistException(BloodragerDestinedFatedBloodrager, 4, BloodragerDestinedBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerDestinedCertainStrike, 8, BloodragerDestinedBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerDestinedDefyDeath, 12, BloodragerDestinedBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerDestinedUnstoppable, 16, BloodragerDestinedBloodline);
            BloodlineTools.ApplyPrimalistException(BloodragerDestinedVictoryOrDeath, 20, BloodragerDestinedBloodline);
            if (TTTContext.AddedContent.Bloodlines.IsDisabled("DestinedBloodline")) { return; }
            BloodlineTools.RegisterBloodragerBloodline(BloodragerDestinedBloodline, BloodragerAberrantBloodlineWandering);
        }
        public static void AddSorcererDestinedBloodline() {
            var SorcererClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf").ToReference<BlueprintCharacterClassReference>();
            var MagusClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780").ToReference<BlueprintCharacterClassReference>();
            var EldritchScionArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("d078b2ef073f2814c9e338a789d97b73").ToReference<BlueprintArchetypeReference>();
            //Used Assets
            var TrueSeeing = BlueprintTools.GetBlueprint<BlueprintAbility>("b3da3fbee6a751d4197e446c7e852bcb");
            var LawDomainBaseAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("a970537ea2da20e42ae709c0bb8f793f");
            var ThoughtSense = BlueprintTools.GetBlueprint<BlueprintAbility>("8fb1a1670b6e1f84b89ea846f589b627");
            var BloodlineInfernalClassSkill = BlueprintTools.GetBlueprint<BlueprintFeature>("f07a37a5b245304429530842cb65e213");

            //Bonus Spells
            var MageShield = BlueprintTools.GetBlueprint<BlueprintAbility>("ef768022b0785eb43a18969903c537c4").ToReference<BlueprintAbilityReference>();
            var Blur = BlueprintTools.GetBlueprint<BlueprintAbility>("14ec7a4e52e90fa47a4c8d63c69fd5c1").ToReference<BlueprintAbilityReference>();
            var ProtectionFromEnergy = BlueprintTools.GetBlueprint<BlueprintAbility>("d2f116cfe05fcdd4a94e80143b67046f").ToReference<BlueprintAbilityReference>();
            var FreedomOfMovement = BlueprintTools.GetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e").ToReference<BlueprintAbilityReference>();
            var BreakEnchantment = BlueprintTools.GetBlueprint<BlueprintAbility>("7792da00c85b9e042a0fdfc2b66ec9a8").ToReference<BlueprintAbilityReference>();
            var HeroismGreater = BlueprintTools.GetBlueprint<BlueprintAbility>("e15e5e7045fda2244b98c8f010adfe31").ToReference<BlueprintAbilityReference>();
            var CircleOfClarity = BlueprintTools.GetBlueprint<BlueprintAbility>("f333185ae986b2a45823cce86535a122").ToReference<BlueprintAbilityReference>();
            var ProtectionFromSpells = BlueprintTools.GetBlueprint<BlueprintAbility>("42aa71adc7343714fa92e471baa98d42").ToReference<BlueprintAbilityReference>();
            var Foresight = BlueprintTools.GetBlueprint<BlueprintAbility>("1f01a098d737ec6419aedc4e7ad61fdd").ToReference<BlueprintAbilityReference>();
            //Bonus Feats
            var ArcaneStrike = BlueprintTools.GetBlueprint<BlueprintFeature>("0ab2f21a922feee4dab116238e3150b4").ToReference<BlueprintFeatureReference>();
            var Diehard = BlueprintTools.GetBlueprint<BlueprintFeature>("86669ce8759f9d7478565db69b8c19ad").ToReference<BlueprintFeatureReference>();
            var Endurance = BlueprintTools.GetBlueprint<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174").ToReference<BlueprintFeatureReference>();
            var MaximizeSpell = BlueprintTools.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b").ToReference<BlueprintFeatureReference>();
            var SiezeTheMoment = BlueprintTools.GetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760").ToReference<BlueprintFeatureReference>();
            var LightningReflexes = BlueprintTools.GetBlueprint<BlueprintFeature>("15e7da6645a7f3d41bdad7c8c4b9de1e").ToReference<BlueprintFeatureReference>();
            var WeaponFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e").ToReference<BlueprintFeatureReference>();
            var SkillFocusKnowledgeWorld = BlueprintTools.GetBlueprint<BlueprintFeature>("611e863120c0f9a4cab2d099f1eb20b4").ToReference<BlueprintFeatureReference>();
            //Bloodline Powers
            var SorcererDestinedClassSkill = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedClassSkill", bp => {
                bp.SetName(TTTContext, "Class Skill — Knowledge (World)");
                bp.SetDescription(TTTContext, "Additional class skill from the destined bloodline.");
                bp.AddComponent(Helpers.Create<AddClassSkill>(c => {
                    c.Skill = StatType.SkillKnowledgeWorld;
                }));
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BloodlineInfernalClassSkill.Icon;
            });
            var SorcererDestinedBloodlineArcanaBuff1 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedBloodlineArcanaBuff1", bp => {
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 1;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff2 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedBloodlineArcanaBuff2", bp => {
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 2;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff3 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedBloodlineArcanaBuff3", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 3;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff4 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedBloodlineArcanaBuff4", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 4;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff5 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedBloodlineArcanaBuff5", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 5;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff6 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedBloodlineArcanaBuff6", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 6;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff7 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedBloodlineArcanaBuff7", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 7;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff8 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedBloodlineArcanaBuff8", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 8;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff9 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedBloodlineArcanaBuff9", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 9;
                });
            });
            var SorcererDestinedBloodlineArcana = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedBloodlineArcana", bp => {
                bp.IsClassFeature = true;
                bp.SetName(TTTContext, "Destined Bloodline Arcana");
                bp.SetDescription(TTTContext, "Whenever you cast a spell with a range of “personal,” you gain a luck bonus equal to the spell’s level on all your saving throws for 1 round.");
                bp.AddComponent<DestinedArcanaComponent>(c => {
                    c.Buffs = new BlueprintBuffReference[] {
                        SorcererDestinedBloodlineArcanaBuff1.ToReference<BlueprintBuffReference>(),
                        SorcererDestinedBloodlineArcanaBuff2.ToReference<BlueprintBuffReference>(),
                        SorcererDestinedBloodlineArcanaBuff3.ToReference<BlueprintBuffReference>(),
                        SorcererDestinedBloodlineArcanaBuff4.ToReference<BlueprintBuffReference>(),
                        SorcererDestinedBloodlineArcanaBuff5.ToReference<BlueprintBuffReference>(),
                        SorcererDestinedBloodlineArcanaBuff6.ToReference<BlueprintBuffReference>(),
                        SorcererDestinedBloodlineArcanaBuff7.ToReference<BlueprintBuffReference>(),
                        SorcererDestinedBloodlineArcanaBuff8.ToReference<BlueprintBuffReference>(),
                        SorcererDestinedBloodlineArcanaBuff9.ToReference<BlueprintBuffReference>(),
                    };
                });
            });
            var SorcererDestinedTouchOfDestinyResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "SorcererDestinedTouchOfDestinyResource", bp => {
                bp.m_Min = 0;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 3,
                    IncreasedByStat = true,
                    ResourceBonusStat = StatType.Charisma,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0]
                };
            });
            var SorcererDestinedTouchOfDestinyBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedTouchOfDestinyBuff", bp => {
                bp.m_Icon = LawDomainBaseAbility.Icon;
                bp.SetName(TTTContext, "Touch of Destiny");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Insight;
                    c.Stat = StatType.AdditionalAttackBonus;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Insight;
                    c.Stat = StatType.SaveFortitude;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Insight;
                    c.Stat = StatType.SaveReflex;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Insight;
                    c.Stat = StatType.SaveWill;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<BuffAllSkillsBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Insight;
                    c.Value = 1;
                    c.Multiplier = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_StartLevel = 1;
                    c.m_StepLevel = 2;
                    c.m_Max = 20;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                    c.m_Class = new BlueprintCharacterClassReference[] { SorcererClass, MagusClass };
                    c.Archetype = EldritchScionArchetype;
                });
            });
            var SorcererDestinedTouchOfDestinyAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "SorcererDestinedTouchOfDestinyAbility", bp => {
                bp.SetName(TTTContext, "Touch of Destiny");
                bp.SetDescription(TTTContext, "At 1st level, you can touch a creature as a standard action, giving it an insight bonus on attack rolls, skill checks, "
                    + "ability checks, and saving throws equal to 1/2 your sorcerer level (minimum 1) for 1 round. You can use this ability a number of "
                    + "times per day equal to 3 + your Charisma modifier.");
                bp.Type = AbilityType.SpellLike;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.CanTargetFriends = true;
                bp.Range = AbilityRange.Touch;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = LawDomainBaseAbility.Icon;
                bp.ResourceAssetIds = LawDomainBaseAbility.ResourceAssetIds;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = SorcererDestinedTouchOfDestinyResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(c => {
                            c.m_Buff = SorcererDestinedTouchOfDestinyBuff.ToReference<BlueprintBuffReference>();
                            c.IsNotDispelable = false;
                            c.Permanent = false;
                            c.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        })
                    );
                });
            });
            var SorcererDestinedTouchOfDestiny = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedTouchOfDestiny", bp => {
                bp.SetName(SorcererDestinedTouchOfDestinyAbility.m_DisplayName);
                bp.SetDescription(SorcererDestinedTouchOfDestinyAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        SorcererDestinedTouchOfDestinyAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = SorcererDestinedTouchOfDestinyResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = SorcererDestinedTouchOfDestinyAbility.Icon;
            });
            var SorcererDestinedWithinReachResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "SorcererDestinedWithinReachResource", bp => {
                bp.m_Min = 0;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1,
                    IncreasedByStat = false,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0]
                };
            });
            var SorcererDestinedWithinReach = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedWithinReach", bp => {
                bp.IsClassFeature = true;
                bp.SetName(TTTContext, "Within Reach");
                bp.SetDescription(TTTContext, "At 15th level, your ultimate destiny is drawing near. Once per day, when an attack or spell that causes "
                    + "damage would result in your death, you may attempt a DC 20 Will save. If successful, you are instead reduced to –1 hit "
                    + "points and are automatically stabilized. The bonus from your fated ability applies to this save.");
                bp.IsClassFeature = true;
                bp.AddComponent<SurviveDeathWithSave>(c => {
                    c.DC = 20;
                    c.Type = SavingThrowType.Will;
                    c.TargetHP = -1;
                    c.BlockIfBelowZero = false;
                    c.Resource = SorcererDestinedWithinReachResource.ToReference<BlueprintAbilityResourceReference>();
                    c.SpendAmount = 1;
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = SorcererDestinedWithinReachResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
            });
            var SorcererDestinedFatedBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedFatedBuff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName(TTTContext, "Fated");
                bp.SetDescription(TTTContext, "Starting at 3rd level, you gain a +1 luck bonus on all of your saving throws and to your AC during the first"
                    + "round of combat. At 7th level and every four levels thereafter, this bonus increases "
                    + "by +1, to a maximum of +5 at 19th level.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = StatType.SaveFortitude;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = StatType.SaveReflex;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = StatType.SaveWill;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Stat = StatType.AC;
                    c.Value = new ContextValue {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                });
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 3;
                    c.m_StepLevel = 4;
                    c.m_Max = 20;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                    c.m_Class = new BlueprintCharacterClassReference[] { SorcererClass, MagusClass };
                    c.Archetype = EldritchScionArchetype;
                });
            });
            var SorcererDestinedFated = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedFated", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 5;
                bp.SetName(TTTContext, "Fated");
                bp.SetDescription(TTTContext, "Starting at 3rd level, you gain a +1 luck bonus on all of your saving throws and to your AC during the first"
                    + "round of combat or when you are otherwise unaware of an attack. At 7th level and every four levels thereafter, this bonus increases "
                    + "by +1, to a maximum of +5 at 19th level.");
                bp.IsClassFeature = true;
                var fatedBuff = Helpers.Create<ContextActionApplyBuff>(c => {
                    c.m_Buff = SorcererDestinedFatedBuff.ToReference<BlueprintBuffReference>();
                    c.IsNotDispelable = true;
                    c.DurationValue = new ContextDurationValue() {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        },
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 1
                        }
                    };
                });
                bp.AddComponent<CombatStateTrigger>(c => {
                    c.CombatStartActions = new ActionList() {
                        Actions = new GameAction[] {
                            fatedBuff
                        }
                    };
                });
                bp.AddComponent<SavingThrowBonusWhileUnaware>(c => {
                    c.Value = 1;
                    c.Descriptor = ModifierDescriptor.Luck;
                });
                bp.AddComponent<SavingThrowBonusAgainstAbility>(c => {
                    c.m_CheckedFact = SorcererDestinedWithinReach.ToReference<BlueprintFeatureReference>();
                    c.Value = 1;
                    c.Descriptor = ModifierDescriptor.Luck;
                });
            });
            var SorcererDestinedItWasMeantToBeResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "SorcererDestinedItWasMeantToBeResource", bp => {
                bp.m_Min = 0;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1,
                    IncreasedByStat = false,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0]
                };
            });
            var SorcererDestinedItWasMeantToBeResourceIncrease = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedItWasMeantToBeResourceIncrease", bp => {
                bp.SetName(TTTContext, "It Was Meant To Be (+1 Uses)");
                bp.SetDescription(TTTContext, "It Was Meant To Be (+1 Uses)");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = SorcererDestinedItWasMeantToBeResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = 1;
                });
            });
            var SorcererDestinedItWasMeantToBeBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedItWasMeantToBeBuff", bp => {
                bp.SetName(TTTContext, "It Was Meant To Be");
                bp.SetDescription(TTTContext, "You may reroll any one attack roll, critical hit confirmation roll, or level check made to "
                    + "overcome spell resistance.");
                bp.IsClassFeature = true;
                bp.AddComponent<ModifyD20>(c => {
                    c.RerollOnlyIfFailed = true;
                    c.RollsAmount = 1;
                    c.TakeBest = true;
                    c.Rule = RuleType.SpellResistance | RuleType.AttackRoll;
                    c.DispellOnRerollFinished = true;
                    c.Bonus = new ContextValue();
                    c.Chance = new ContextValue();
                    c.Value = new ContextValue();
                    c.Skill = new StatType[0];
                });
            });
            var SorcererDestinedItWasMeantToBeAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "SorcererDestinedItWasMeantToBeAbility", bp => {
                bp.SetName(TTTContext, "It Was Meant To Be");
                bp.SetDescription(TTTContext, "At 9th level, you may reroll any one attack roll, critical hit confirmation roll, or level check made to overcome spell resistance. "
                    + "At 17th level, you can use this ability twice per day.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.CanTargetEnemies = true;
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.m_Icon = TrueSeeing.Icon;
                bp.ResourceAssetIds = TrueSeeing.ResourceAssetIds;
                var addReroll = Helpers.Create<ContextActionApplyBuff>(c => {
                    c.m_Buff = SorcererDestinedItWasMeantToBeBuff.ToReference<BlueprintBuffReference>();
                    c.IsNotDispelable = true;
                    c.Permanent = true;
                    c.DurationValue = new ContextDurationValue() {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        },
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 1
                        }
                    };
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList {
                        Actions = new GameAction[] { addReroll }
                    };
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = SorcererDestinedItWasMeantToBeResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                });
            });
            var SorcererDestinedItWasMeantToBe = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedItWasMeantToBe", bp => {
                bp.IsClassFeature = true;
                bp.m_Icon = TrueSeeing.Icon;
                bp.SetName(TTTContext, "It Was Meant To Be");
                bp.SetDescription(TTTContext, "At 9th level, you may reroll any one attack roll, critical hit confirmation roll, or level check made to overcome spell resistance. "
                    + "At 9th level, you can use this ability once per day. At 17th level, you can use this ability twice per day.");
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        SorcererDestinedItWasMeantToBeAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = SorcererDestinedItWasMeantToBeResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
            });
            var SorcererDestinedDestinyRealizedResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "SorcererDestinedDestinyRealizedResource", bp => {
                bp.m_Min = 0;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1,
                    IncreasedByStat = false,
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0]
                };
            });
            var SorcererDestinedDestinyRealizedBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SorcererDestinedDestinyRealizedBuff", bp => {
                bp.SetName(TTTContext, "Destiny Realized");
                bp.SetDescription(TTTContext, "You automatically succeed at one caster level check made to overcome spell resistance.");
                bp.IsClassFeature = true;
                bp.AddComponent<IgnoreSpellResistanceForSpells>(c => {
                    c.AllSpells = true;
                });
                bp.AddComponent<RemoveBuffAfterSpellResistCheck>();
            });
            var SorcererDestinedDestinyRealizedAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "SorcererDestinedDestinyRealizedAbility", bp => {
                bp.SetName(TTTContext, "Destiny Realized");
                bp.SetDescription(TTTContext, "Once per day, you can automatically succeed at one caster level check made to overcome " +
                    "spell resistance. You must use this ability before making the roll.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.m_Icon = ThoughtSense.Icon;
                bp.ResourceAssetIds = ThoughtSense.ResourceAssetIds;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = SorcererDestinedDestinyRealizedResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                });
                var autoSpellPen = Helpers.Create<ContextActionApplyBuff>(c => {
                    c.m_Buff = SorcererDestinedDestinyRealizedBuff.ToReference<BlueprintBuffReference>();
                    c.IsNotDispelable = true;
                    c.Permanent = true;
                    c.DurationValue = new ContextDurationValue() {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        },
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Simple,
                            Value = 1
                        }
                    };
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] { autoSpellPen };
                });
            });
            var SorcererDestinedDestinyRealized = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedDestinyRealized", bp => {
                bp.m_Icon = SorcererDestinedDestinyRealizedAbility.Icon;
                bp.SetName(TTTContext, "Destiny Realized");
                bp.SetDescription(TTTContext, "At 20th level, your moment of destiny is at hand. Any critical threats made against you only confirm if the second "
                    + "roll results in a natural 20 on the die. Any critical threats you score with a spell are automatically confirmed. Once per day, you "
                    + "can automatically succeed at one caster level check made to overcome spell resistance. You must use this ability before making the roll.");
                bp.IsClassFeature = true;
                bp.AddComponent<CriticalConfirmationACBonus>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Simple,
                    };
                    c.Bonus = 200;
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        SorcererDestinedDestinyRealizedAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = SorcererDestinedDestinyRealizedResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
                bp.AddComponent(Helpers.Create<InitiatorSpellCritAutoconfirm>());
            });
            //Bloodline Feats
            var SorcererDestinedFeatSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "SorcererDestinedFeatSelection", bp => {
                bp.name = "SorcererDestinedFeatSelection";
                bp.SetName(TTTContext, "Bloodline Feat Selection");
                bp.SetDescription(TTTContext, "At 7th level, and every six levels thereafter, a sorcerer receives one bonus feat, chosen from a list specific to each bloodline. "
                    + "The sorcerer must meet the prerequisites for these bonus feats."
                    + "\nBonus Feats: Arcane Strike, Diehard, Endurance, Sieze The Moment, Lightning Reflexes, Maximize Spell, Skill Focus (Knowledge World), Weapon Focus.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;

                bp.m_Features = new BlueprintFeatureReference[] {
                    ArcaneStrike,
                    Diehard,
                    Endurance,
                    SiezeTheMoment,
                    LightningReflexes,
                    WeaponFocus,
                    SkillFocusKnowledgeWorld,
                    MaximizeSpell
                };
                bp.m_AllFeatures = bp.m_Features;
            });
            //Bloodline Spells
            var SorcererDestinedSpell3 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedSpell3", bp => {
                var Spell = MageShield;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription(TTTContext, "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 1;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell5 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedSpell5", bp => {
                var Spell = Blur;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription(TTTContext, "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 2;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell7 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedSpell7", bp => {
                var Spell = ProtectionFromEnergy;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription(TTTContext, "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 3;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell9 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedSpell9", bp => {
                var Spell = FreedomOfMovement;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription(TTTContext, "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 4;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell11 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedSpell11", bp => {
                var Spell = BreakEnchantment;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription(TTTContext, "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 5;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell13 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedSpell13", bp => {
                var Spell = HeroismGreater;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription(TTTContext, "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 6;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell15 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedSpell15", bp => {
                var Spell = CircleOfClarity;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription(TTTContext, "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 7;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell17 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedSpell17", bp => {
                var Spell = ProtectionFromSpells;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription(TTTContext, "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 8;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell19 = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SorcererDestinedSpell19", bp => {
                var Spell = Foresight;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription(TTTContext, "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 9;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            //Bloodline Core
            var SorcererDestinedBloodline = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "SorcererDestinedBloodline", bp => {
                bp.SetName(TTTContext, "Destined Bloodline");
                bp.SetDescription(TTTContext, "Your family is destined for greatness in some way. Your birth could have been foretold in prophecy, or perhaps "
                    + "it occurred during an especially auspicious event, such as a solar eclipse. Regardless of your bloodline’s origin, you have a great future ahead.\n"
                    + "Bonus Feats of the Destined Bloodline: Arcane Strike, Diehard, Endurance, Sieze The Moment, Lightning Reflexes, Maximize Spell, Skill Focus (Knowledge World), Weapon Focus.");
                bp.IsClassFeature = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = SorcererClass
                    },
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = MagusClass
                    }
                };
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[]{
                    new BlueprintProgression.ArchetypeWithLevel {
                        m_Archetype = EldritchScionArchetype
                    }
                };
                bp.Groups = new FeatureGroup[] { FeatureGroup.BloodragerBloodline };
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, SorcererDestinedTouchOfDestiny, SorcererDestinedBloodlineArcana, SorcererDestinedClassSkill, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature),
                    Helpers.CreateLevelEntry(3, SorcererDestinedSpell3, SorcererDestinedFated),
                    Helpers.CreateLevelEntry(5, SorcererDestinedSpell5),
                    Helpers.CreateLevelEntry(7, SorcererDestinedSpell7),
                    Helpers.CreateLevelEntry(9, SorcererDestinedSpell9, SorcererDestinedItWasMeantToBe),
                    Helpers.CreateLevelEntry(11, SorcererDestinedSpell11),
                    Helpers.CreateLevelEntry(13, SorcererDestinedSpell13),
                    Helpers.CreateLevelEntry(15, SorcererDestinedSpell15, SorcererDestinedWithinReach),
                    Helpers.CreateLevelEntry(17, SorcererDestinedSpell17),
                    Helpers.CreateLevelEntry(19, SorcererDestinedSpell19),
                    Helpers.CreateLevelEntry(20, SorcererDestinedDestinyRealized)
                };
                bp.AddPrerequisite<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature;
                });
                bp.AddPrerequisite<PrerequisiteFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = DestinedBloodlineRequisiteFeature;
                });
            });
            var CrossbloodedDestinedBloodline = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "CrossbloodedDestinedBloodline", bp => {
                bp.SetName(SorcererDestinedBloodline.m_DisplayName);
                bp.SetDescription(SorcererDestinedBloodline.m_Description);
                bp.IsClassFeature = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = SorcererClass
                    },
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = MagusClass
                    }
                };
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[]{
                    new BlueprintProgression.ArchetypeWithLevel {
                        m_Archetype = EldritchScionArchetype
                    }
                };
                bp.Groups = new FeatureGroup[] { FeatureGroup.BloodragerBloodline };
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, SorcererDestinedBloodlineArcana, SorcererDestinedClassSkill, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature),
                    Helpers.CreateLevelEntry(3, SorcererDestinedSpell3),
                    Helpers.CreateLevelEntry(5, SorcererDestinedSpell5),
                    Helpers.CreateLevelEntry(7, SorcererDestinedSpell7),
                    Helpers.CreateLevelEntry(9, SorcererDestinedSpell9),
                    Helpers.CreateLevelEntry(11, SorcererDestinedSpell11),
                    Helpers.CreateLevelEntry(13, SorcererDestinedSpell13),
                    Helpers.CreateLevelEntry(15, SorcererDestinedSpell15),
                    Helpers.CreateLevelEntry(17, SorcererDestinedSpell17),
                    Helpers.CreateLevelEntry(19, SorcererDestinedSpell19)
                };
            });
            var SeekerDestinedBloodline = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "SeekerDestinedBloodline", bp => {
                bp.SetName(SorcererDestinedBloodline.m_DisplayName);
                bp.SetDescription(SorcererDestinedBloodline.m_Description);
                bp.IsClassFeature = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = SorcererClass
                    },
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = MagusClass
                    }
                };
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[]{
                    new BlueprintProgression.ArchetypeWithLevel {
                        m_Archetype = EldritchScionArchetype
                    }
                };
                bp.GiveFeaturesForPreviousLevels = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.BloodragerBloodline };
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, SorcererDestinedTouchOfDestiny, SorcererDestinedBloodlineArcana, SorcererDestinedClassSkill, DestinedBloodlineRequisiteFeature, BloodlineRequisiteFeature),
                    Helpers.CreateLevelEntry(3, SorcererDestinedSpell3),
                    Helpers.CreateLevelEntry(5, SorcererDestinedSpell5),
                    Helpers.CreateLevelEntry(7, SorcererDestinedSpell7),
                    Helpers.CreateLevelEntry(9, SorcererDestinedSpell9, SorcererDestinedItWasMeantToBe),
                    Helpers.CreateLevelEntry(11, SorcererDestinedSpell11),
                    Helpers.CreateLevelEntry(13, SorcererDestinedSpell13),
                    Helpers.CreateLevelEntry(15, SorcererDestinedSpell15),
                    Helpers.CreateLevelEntry(17, SorcererDestinedSpell17),
                    Helpers.CreateLevelEntry(19, SorcererDestinedSpell19),
                    Helpers.CreateLevelEntry(20, SorcererDestinedDestinyRealized)
                };
                bp.AddPrerequisite<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature;
                });
                bp.AddPrerequisite<PrerequisiteFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = DestinedBloodlineRequisiteFeature;
                });
            });
            BloodlineTools.RegisterSorcererFeatSelection(SorcererDestinedFeatSelection, SorcererDestinedBloodline);

            if (TTTContext.AddedContent.Bloodlines.IsDisabled("DestinedBloodline")) { return; }
            BloodlineTools.RegisterSorcererBloodline(SorcererDestinedBloodline);
            BloodlineTools.RegisterCrossbloodedBloodline(CrossbloodedDestinedBloodline);
            BloodlineTools.RegisterSeekerBloodline(SeekerDestinedBloodline);
        }
    }
}