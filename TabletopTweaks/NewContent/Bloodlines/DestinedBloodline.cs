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
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Bloodlines {
    class DestinedBloodline {

        static BlueprintFeatureReference BloodlineRequisiteFeature = Resources.GetModBlueprint<BlueprintFeature>("BloodlineRequisiteFeature").ToReference<BlueprintFeatureReference>();
        static BlueprintFeatureReference DestinedBloodlineRequisiteFeature = CreateBloodlineRequisiteFeature();

        static BlueprintFeatureReference CreateBloodlineRequisiteFeature() {
            var AberrantBloodlineRequisiteFeature = Helpers.CreateBlueprint<BlueprintFeature>("DestinedBloodlineRequisiteFeature", bp => {
                bp.SetName("1863f837707c4e33af4daa46d92226dc", "Destined Bloodline");
                bp.SetDescription("593225aeee6e485285a086fb2b5a5bfd", "Destined Bloodline Requisite Feature");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 1;
                bp.HideInCharacterSheetAndLevelUp = true;
            });
            return AberrantBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
        }
        public static void AddBloodragerDestinedBloodline() {
            var BloodragerStandardRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
            var BloodragerClass = Resources.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499").ToReference<BlueprintCharacterClassReference>();
            var GreenragerArchetype = Resources.GetBlueprint<BlueprintArchetype>("5648585af75596f4a9fa3ae385127f57").ToReference<BlueprintArchetypeReference>();
            //Used Assets
            var TrueStrike = Resources.GetBlueprint<BlueprintAbility>("2c38da66e5a599347ac95b3294acbe00");
            var LuckDomain = Resources.GetBlueprint<BlueprintAbility>("9af0b584f6f754045a0a79293d100ab3");
            //Bonus Spells
            var MageShield = Resources.GetBlueprint<BlueprintAbility>("ef768022b0785eb43a18969903c537c4").ToReference<BlueprintAbilityReference>();
            var Blur = Resources.GetBlueprint<BlueprintAbility>("14ec7a4e52e90fa47a4c8d63c69fd5c1").ToReference<BlueprintAbilityReference>();
            var ProtectionFromEnergy = Resources.GetBlueprint<BlueprintAbility>("d2f116cfe05fcdd4a94e80143b67046f").ToReference<BlueprintAbilityReference>();
            var FreedomOfMovement = Resources.GetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e").ToReference<BlueprintAbilityReference>();
            //Bonus Feats
            var Diehard = Resources.GetBlueprint<BlueprintFeature>("86669ce8759f9d7478565db69b8c19ad").ToReference<BlueprintFeatureReference>();
            var Endurance = Resources.GetBlueprint<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174").ToReference<BlueprintFeatureReference>();
            var ImprovedInitiative = Resources.GetBlueprint<BlueprintFeature>("797f25d709f559546b29e7bcb181cc74").ToReference<BlueprintFeatureReference>();
            var IntimidatingProwess = Resources.GetBlueprint<BlueprintFeature>("d76497bfc48516e45a0831628f767a0f").ToReference<BlueprintFeatureReference>();
            var SiezeTheMoment = Resources.GetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760").ToReference<BlueprintFeatureReference>();
            var LightningReflexes = Resources.GetBlueprint<BlueprintFeature>("15e7da6645a7f3d41bdad7c8c4b9de1e").ToReference<BlueprintFeatureReference>();
            var WeaponFocus = Resources.GetBlueprint<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e").ToReference<BlueprintFeatureReference>();
            //Bloodline Powers
            var BloodragerDestinedStrikeResource = Helpers.CreateBlueprint<BlueprintAbilityResource>("BloodragerDestinedStrikeResource", bp => {
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
            var BloodragerDestinedStrikeResourceIncrease = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedStrikeResourceIncrease", bp => {
                bp.SetName("925282403ce94ffeb3d9a0c090fa3bf4", "Destined Strike Extra Uses");
                bp.SetDescription("8a1f62151f9a42839a8e12b1bb60ee35", "");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = BloodragerDestinedStrikeResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = 2;
                });
            });
            var BloodragerDestinedStrikeBuff = Helpers.CreateBuff("BloodragerDestinedStrikeBuff", bp => {
                bp.Stacking = StackingType.Rank;
                bp.Ranks = 5;
                bp.SetName("8ae8b99ac9334677b78ccccba4d2e272", "Destined Strike");
                bp.SetDescription("a1a247afb20f4654a875796736957ec6", "You can grant yourself an insight bonus equal to 1/2 your bloodrager level (minimum 1) on one melee attack.");
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
            var BloodragerDestinedStrikeAbility = Helpers.CreateBlueprint<BlueprintAbility>("BloodragerDestinedAbility", bp => {
                bp.SetName("7ac8db184ec84c44aeee42e7ea5ae8bd", "Destined Strike");
                bp.SetDescription("56966cd022b149f5ab2568f3fa08eaa7", "At 1st level, as a free action up to three times per day you can grant yourself an insight bonus equal to 1/2 your "
                    + "bloodrager level (minimum 1) on one melee attack. At 12th level, you can use this ability up to five times per day.");
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.m_Icon = TrueStrike.Icon;
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
            var BloodragerDestinedStrike = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedStrike", bp => {
                bp.SetName(BloodragerDestinedStrikeAbility.m_DisplayName);
                bp.SetDescription(BloodragerDestinedStrikeAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent(Helpers.Create<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BloodragerDestinedStrikeAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                }));
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = BloodragerDestinedStrikeResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BloodragerDestinedStrikeAbility.Icon;
            });
            var BloodragerDestinedFatedBloodrager = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedFatedBloodrager", bp => {
                bp.SetName("a0c04610dce8407fb65b10a2aa4a7dd9", "Fated Bloodrager");
                bp.SetDescription("750ed74bf4e948eb84d7e31e39b7ae63", "At 4th level, you gain a +1 luck bonus to AC and on saving throws. At 8th level and every "
                    + "4 levels thereafter, this bonus increases by 1 (to a maximum of +5 at 20th level).");
                bp.IsClassFeature = true;
                bp.Ranks = 5;
            });
            var BloodragerDestinedFatedBloodragerBuff = Helpers.CreateBuff("BloodragerDestinedFatedBloodragerBuff", bp => {
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
            var BloodragerDestinedCertainStrike = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedCertainStrike", bp => {
                bp.SetName("a7bd008044be416faf9be138a63960b7", "Certain Strike");
                bp.SetDescription("ed63717ccaa747fc92b3ada250213595", "At 8th level, you may reroll an attack roll once during a bloodrage. You must decide to use this ability after "
                    + "the die is rolled, but before the GM reveals the results. You must take the second result, even if it’s worse.");
                bp.IsClassFeature = true;
            });
            var BloodragerDestinedCertainStrikeBuff = Helpers.CreateBuff("BloodragerDestinedCertainStrikeBuff", bp => {
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
            var BloodragerDestinedDefyDeathResource = Helpers.CreateBlueprint<BlueprintAbilityResource>("BloodragerDestinedDefyDeathResource", bp => {
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
            var BloodragerDestinedDefyDeath = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedDefyDeath", bp => {
                bp.SetName("e79130c05e5045289dfa9cb152e16b09", "Defy Death");
                bp.SetDescription("2ede159c32404023a44f87e891de9441", "At 12th level, once per day when an attack or spell that deals damage would result in your death"
                    + ", you can attempt a DC 20 Fortitude save. If you succeed, you are instead reduced to 1 hit point; if you "
                    + "succeed and already have less than 1 hit point, you instead take no damage.");
                bp.IsClassFeature = true;
                bp.AddComponent(Helpers.Create<AddAbilityResources>(c => {
                    c.m_Resource = BloodragerDestinedDefyDeathResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                }));
            });
            var BloodragerDestinedDefyDeathBuff = Helpers.CreateBuff("BloodragerDestinedDefyDeathBuff", bp => {
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
            var BloodragerDestinedUnstoppable = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedUnstoppable", bp => {
                bp.SetName("4b61cb8f309e45bebb66b0335d705d53", "Unstoppable");
                bp.SetDescription("9f49ac7b482644f9a2b95f72cbdb192e", "At 16th level, any critical threats you score are automatically confirmed. Any critical "
                    + "threats made against you confirm only if the second roll results in a natural 20 (or is automatically confirmed).");
                bp.IsClassFeature = true;
            });
            var BloodragerDestinedUnstoppableBuff = Helpers.CreateBuff("BloodragerDestinedUnstoppableBuff", bp => {
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
            var BloodragerDestinedVictoryOrDeath = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedVictoryOrDeath", bp => {
                bp.SetName("d4326a38472045c9af02fa0879dddb78", "Victory or Death");
                bp.SetDescription("d2d1d6e922b34f24b4f80c4bb4a58b24", "At 20th level, you are immune to paralysis and petrification, as well as to the stunned, dazed, "
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
            var BloodragerDestinedFeatSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("BloodragerDestinedFeatSelection", bp => {
                bp.SetName("86a4900ba1294875980869d17a74ea67", "Bonus Feats");
                bp.SetDescription("091e7415edf649419323ab41441c6ad0", "Bonus Feats: Diehard, Endurance, Improved Initiative, Intimidating Prowess, Sieze The Moment, Lightning Reflexes, Weapon Focus.");
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
            var BloodragerDestinedFeatSelectionGreenrager = Helpers.CreateBlueprint<BlueprintFeatureSelection>("BloodragerDestinedFeatSelectionGreenrager", bp => {
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
            var BloodragerDestinedSpell7 = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedSpell7", bp => {
                var spell = MageShield;
                bp.SetName("f570bd6fccc546f4a838f5fe173413fb", $"Bonus Spell — {spell.Get().Name}");
                bp.SetDescription("8eeaba31029045d28acca26df7be7bc4", "At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 1;
                });
            });
            var BloodragerDestinedSpell10 = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedSpell10", bp => {
                var spell = Blur;
                bp.SetName("2f94d2d774684993b154686a8089c2ff", $"Bonus Spell — {spell.Get().Name}");
                bp.SetDescription("19dd047504744023a2ff1d2a4c58e2ed", "At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 2;
                });
            });
            var BloodragerDestinedSpell13 = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedSpell13", bp => {
                var spell = ProtectionFromEnergy;
                bp.SetName("b295607398344eb2a768fa516c80e8c3", $"Bonus Spell — {spell.Get().Name}");
                bp.SetDescription("a084ab078f1944229ca5864af791fc3e", "At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 3;
                });
            });
            var BloodragerDestinedSpell16 = Helpers.CreateBlueprint<BlueprintFeature>("BloodragerDestinedSpell16", bp => {
                var spell = FreedomOfMovement;
                bp.SetName("c48322e101334947a678019582e607ff", $"Bonus Spell — {spell.Get().Name}");
                bp.SetDescription("4ff194e0c6c44cd1b9f0e0afe8985de7", "At 7th, 10th, 13th, and 16th levels, a bloodrager learns an additional spell derived from his bloodline.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = BloodragerClass;
                    c.m_Spell = spell;
                    c.SpellLevel = 4;
                });
            });
            //Bloodline Core
            var BloodragerDestinedBloodline = Helpers.CreateBlueprint<BlueprintProgression>("BloodragerDestinedBloodline", bp => {
                bp.SetName("c6a74ccfd3064e4cad25c85486cccf9b", "Destined");
                bp.SetDescription("ca748a99353f479c8698bfb187ec77fa", "Your bloodline is destined for great things. When you bloodrage, you exude a greatness that makes all but the most legendary creatures seem lesser.\n"
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
            var BloodragerAberrantBloodlineWandering = BloodlineTools.CreateMixedBloodFeature("BloodragerDestinedBloodlineWandering", BloodragerDestinedBloodline, bp => {
                bp.m_Icon = AssetLoader.LoadInternal("Abilities", "Icon_DestinedBloodline.png");
            });
            var BloodragerDestinedBaseBuff = Helpers.CreateBuff("BloodragerDestinedBaseBuff", bp => {
                bp.SetName("9bb2259ae8f44653b390bd5d638d048e", "Destined Bloodrage");
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
            if (ModSettings.AddedContent.Bloodlines.IsDisabled("DestinedBloodline")) { return; }
            BloodlineTools.RegisterBloodragerBloodline(BloodragerDestinedBloodline, BloodragerAberrantBloodlineWandering);
        }
        public static void AddSorcererDestinedBloodline() {
            var SorcererClass = Resources.GetBlueprint<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf").ToReference<BlueprintCharacterClassReference>();
            var MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780").ToReference<BlueprintCharacterClassReference>();
            var EldritchScionArchetype = Resources.GetBlueprint<BlueprintArchetype>("d078b2ef073f2814c9e338a789d97b73").ToReference<BlueprintArchetypeReference>();
            //Used Assets
            var TrueSeeing = Resources.GetBlueprint<BlueprintAbility>("b3da3fbee6a751d4197e446c7e852bcb");
            var LawDomainBaseAbility = Resources.GetBlueprint<BlueprintAbility>("a970537ea2da20e42ae709c0bb8f793f");
            var ThoughtSense = Resources.GetBlueprint<BlueprintAbility>("8fb1a1670b6e1f84b89ea846f589b627");
            var BloodlineInfernalClassSkill = Resources.GetBlueprint<BlueprintFeature>("f07a37a5b245304429530842cb65e213");

            //Bonus Spells
            var MageShield = Resources.GetBlueprint<BlueprintAbility>("ef768022b0785eb43a18969903c537c4").ToReference<BlueprintAbilityReference>();
            var Blur = Resources.GetBlueprint<BlueprintAbility>("14ec7a4e52e90fa47a4c8d63c69fd5c1").ToReference<BlueprintAbilityReference>();
            var ProtectionFromEnergy = Resources.GetBlueprint<BlueprintAbility>("d2f116cfe05fcdd4a94e80143b67046f").ToReference<BlueprintAbilityReference>();
            var FreedomOfMovement = Resources.GetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e").ToReference<BlueprintAbilityReference>();
            var BreakEnchantment = Resources.GetBlueprint<BlueprintAbility>("7792da00c85b9e042a0fdfc2b66ec9a8").ToReference<BlueprintAbilityReference>();
            var HeroismGreater = Resources.GetBlueprint<BlueprintAbility>("e15e5e7045fda2244b98c8f010adfe31").ToReference<BlueprintAbilityReference>();
            var CircleOfClarity = Resources.GetBlueprint<BlueprintAbility>("f333185ae986b2a45823cce86535a122").ToReference<BlueprintAbilityReference>();
            var ProtectionFromSpells = Resources.GetBlueprint<BlueprintAbility>("42aa71adc7343714fa92e471baa98d42").ToReference<BlueprintAbilityReference>();
            var Foresight = Resources.GetBlueprint<BlueprintAbility>("1f01a098d737ec6419aedc4e7ad61fdd").ToReference<BlueprintAbilityReference>();
            //Bonus Feats
            var ArcaneStrike = Resources.GetBlueprint<BlueprintFeature>("0ab2f21a922feee4dab116238e3150b4").ToReference<BlueprintFeatureReference>();
            var Diehard = Resources.GetBlueprint<BlueprintFeature>("86669ce8759f9d7478565db69b8c19ad").ToReference<BlueprintFeatureReference>();
            var Endurance = Resources.GetBlueprint<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174").ToReference<BlueprintFeatureReference>();
            var MaximizeSpell = Resources.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b").ToReference<BlueprintFeatureReference>();
            var SiezeTheMoment = Resources.GetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760").ToReference<BlueprintFeatureReference>();
            var LightningReflexes = Resources.GetBlueprint<BlueprintFeature>("15e7da6645a7f3d41bdad7c8c4b9de1e").ToReference<BlueprintFeatureReference>();
            var WeaponFocus = Resources.GetBlueprint<BlueprintFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e").ToReference<BlueprintFeatureReference>();
            var SkillFocusKnowledgeWorld = Resources.GetBlueprint<BlueprintFeature>("611e863120c0f9a4cab2d099f1eb20b4").ToReference<BlueprintFeatureReference>();
            //Bloodline Powers
            var SorcererDestinedClassSkill = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedClassSkill", bp => {
                bp.SetName("b95f3e7604e4475f93f6ae87ac34bd3c", "Class Skill — Knowledge (World)");
                bp.SetDescription("6631aa6251cc41f0b55912aec5a39441", "Additional class skill from the destined bloodline.");
                bp.AddComponent(Helpers.Create<AddClassSkill>(c => {
                    c.Skill = StatType.SkillKnowledgeWorld;
                }));
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BloodlineInfernalClassSkill.Icon;
            });
            var SorcererDestinedBloodlineArcanaBuff1 = Helpers.CreateBuff("SorcererDestinedBloodlineArcanaBuff1", bp => {
                bp.SetName("ca0b427830304f1bb42b6991c83220ae", "Destined Bloodline Arcana");
                bp.SetDescription("683df7538cc54385b4833eeb1fb260ba", "");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 1;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff2 = Helpers.CreateBuff("SorcererDestinedBloodlineArcanaBuff2", bp => {
                bp.SetName("0467ccae8420404da5981fb6ee8bee7a", "Destined Bloodline Arcana");
                bp.SetDescription("123b975a754241d9a75b2214f96516d3", "");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 2;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff3 = Helpers.CreateBuff("SorcererDestinedBloodlineArcanaBuff3", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName("b3a76c89e25b40bbbfa515aae575f68b", "Destined Bloodline Arcana");
                bp.SetDescription("e692ccc7c3304d06b8da94125d21197e", "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 3;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff4 = Helpers.CreateBuff("SorcererDestinedBloodlineArcanaBuff4", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName("ef9164d4529b439d997bcd6dd16a0eab", "Destined Bloodline Arcana");
                bp.SetDescription("f621d83a3dc44ae283deb71d53065543", "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 4;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff5 = Helpers.CreateBuff("SorcererDestinedBloodlineArcanaBuff5", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName("0c31cb32c4bc43a9a79c65a196c0dd6b", "Destined Bloodline Arcana");
                bp.SetDescription("1e8bbd701dbb4d0e9f163244b18959f4", "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 5;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff6 = Helpers.CreateBuff("SorcererDestinedBloodlineArcanaBuff6", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName("9906696befb94b449e63f6ce01240a48", "Destined Bloodline Arcana");
                bp.SetDescription("378f4055658b488da0273ecf196355e7", "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 6;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff7 = Helpers.CreateBuff("SorcererDestinedBloodlineArcanaBuff7", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName("58cdd24b7fd9412781de6f8fc9a1175a", "Destined Bloodline Arcana");
                bp.SetDescription("88f77ceb01fc450db77e0e1c3413fb91", "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 7;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff8 = Helpers.CreateBuff("SorcererDestinedBloodlineArcanaBuff8", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName("1e58a171bf6842ba9fc9ddd1d6820133", "Destined Bloodline Arcana");
                bp.SetDescription("f005fb4b047e47168b8e62c121a5404a", "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 8;
                });
            });
            var SorcererDestinedBloodlineArcanaBuff9 = Helpers.CreateBuff("SorcererDestinedBloodlineArcanaBuff9", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName("516580a132ec414e9032d33e001fb77c", "Destined Bloodline Arcana");
                bp.SetDescription("951040f8f4d34818ac7d70ddb89a621a", "");
                bp.AddComponent<BuffAllSavesBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Luck;
                    c.Value = 9;
                });
            });
            var SorcererDestinedBloodlineArcana = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedBloodlineArcana", bp => {
                bp.IsClassFeature = true;
                bp.SetName("04caaf91381642da89fe26c223788347", "Destined Bloodline Arcana");
                bp.SetDescription("dc0c4c68d5164b84a16578e1c22d5a57", "Whenever you cast a spell with a range of “personal,” you gain a luck bonus equal to the spell’s level on all your saving throws for 1 round.");
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
            var SorcererDestinedTouchOfDestinyResource = Helpers.CreateBlueprint<BlueprintAbilityResource>("SorcererDestinedTouchOfDestinyResource", bp => {
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
            var SorcererDestinedTouchOfDestinyBuff = Helpers.CreateBuff("SorcererDestinedTouchOfDestinyBuff", bp => {
                bp.m_Icon = LawDomainBaseAbility.Icon;
                bp.SetName("04c30fee86104187971df8138b089da3", "Touch of Destiny");
                bp.SetDescription("6322fbe145e7443abc06f19d28c3add0", "");
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
            var SorcererDestinedTouchOfDestinyAbility = Helpers.CreateBlueprint<BlueprintAbility>("SorcererDestinedTouchOfDestinyAbility", bp => {
                bp.SetName("09af093386604f369bc409f872ff718c", "Touch of Destiny");
                bp.SetDescription("453a5ff31b4f42af8cd41a92621b6ee5", "At 1st level, you can touch a creature as a standard action, giving it an insight bonus on attack rolls, skill checks, "
                    + "ability checks, and saving throws equal to 1/2 your sorcerer level (minimum 1) for 1 round. You can use this ability a number of "
                    + "times per day equal to 3 + your Charisma modifier.");
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
                var addInsightBonus = Helpers.Create<ContextActionApplyBuff>(c => {
                    c.m_Buff = SorcererDestinedTouchOfDestinyBuff.ToReference<BlueprintBuffReference>();
                    c.IsNotDispelable = false;
                    c.Permanent = false;
                    c.DurationValue = new ContextDurationValue() {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.One,
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
                        Actions = new GameAction[] { addInsightBonus }
                    };
                });
            });
            var SorcererDestinedTouchOfDestiny = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedTouchOfDestiny", bp => {
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
            var SorcererDestinedWithinReachResource = Helpers.CreateBlueprint<BlueprintAbilityResource>("SorcererDestinedWithinReachResource", bp => {
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
            var SorcererDestinedWithinReach = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedWithinReach", bp => {
                bp.IsClassFeature = true;
                bp.SetName("928cd2f5d1e942a1a548c34b2863a35d", "Within Reach");
                bp.SetDescription("baee0b6e3a3b4363b1adf515b86777a0", "At 15th level, your ultimate destiny is drawing near. Once per day, when an attack or spell that causes "
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
            var SorcererDestinedFatedBuff = Helpers.CreateBuff("SorcererDestinedFatedBuff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName("2e09711aaeaa4af0934dffa3144760b0", "Fated");
                bp.SetDescription("dbffa6a7a6cf443e9add54d77c8d6e97", "Starting at 3rd level, you gain a +1 luck bonus on all of your saving throws and to your AC during the first"
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
            var SorcererDestinedFated = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedFated", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 5;
                bp.SetName("1bf8b9bc022041ed8b3370831ec8b5ce", "Fated");
                bp.SetDescription("5d41eea2c689495395166c68de4df695", "Starting at 3rd level, you gain a +1 luck bonus on all of your saving throws and to your AC during the first"
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
            var SorcererDestinedItWasMeantToBeResource = Helpers.CreateBlueprint<BlueprintAbilityResource>("SorcererDestinedItWasMeantToBeResource", bp => {
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
            var SorcererDestinedItWasMeantToBeResourceIncrease = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedItWasMeantToBeResourceIncrease", bp => {
                bp.SetName("18110660a0654d978c318c6c15c4fab2", "It Was Meant To Be (+1 Uses)");
                bp.SetDescription("3602295b98124e0ba99754d8ff2de402", "It Was Meant To Be (+1 Uses)");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = SorcererDestinedItWasMeantToBeResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = 1;
                });
            });
            var SorcererDestinedItWasMeantToBeBuff = Helpers.CreateBuff("SorcererDestinedItWasMeantToBeBuff", bp => {
                bp.SetName("067da0fcb89345769edc22f850e47a89", "It Was Meant To Be");
                bp.SetDescription("ef62968a31874ef2b9f4eadb262e41eb", "You may reroll any one attack roll, critical hit confirmation roll, or level check made to "
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
            var SorcererDestinedItWasMeantToBeAbility = Helpers.CreateBlueprint<BlueprintAbility>("SorcererDestinedItWasMeantToBeAbility", bp => {
                bp.SetName("2989fcf2758047ca9c8660da9440eb37", "It Was Meant To Be");
                bp.SetDescription("262a648ef72249cfa6c0a131ca682fad", "At 9th level, you may reroll any one attack roll, critical hit confirmation roll, or level check made to overcome spell resistance. "
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
            var SorcererDestinedItWasMeantToBe = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedItWasMeantToBe", bp => {
                bp.IsClassFeature = true;
                bp.m_Icon = TrueSeeing.Icon;
                bp.SetName("d00eda71d73f45d398690ce3b6ed0b37", "It Was Meant To Be");
                bp.SetDescription("7b11bed6783d4437b2af8a607b7a405c", "At 9th level, you may reroll any one attack roll, critical hit confirmation roll, or level check made to overcome spell resistance. "
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
            var SorcererDestinedDestinyRealizedResource = Helpers.CreateBlueprint<BlueprintAbilityResource>("SorcererDestinedDestinyRealizedResource", bp => {
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
            var SorcererDestinedDestinyRealizedBuff = Helpers.CreateBuff("SorcererDestinedDestinyRealizedBuff", bp => {
                bp.SetName("dc958b33696f4789b8e9563bd6f62481", "Destiny Realized");
                bp.SetDescription("aa2bceb0880d40f4a671af64b1d33469", "You automatically succeed at one caster level check made to overcome spell resistance.");
                bp.IsClassFeature = true;
                bp.AddComponent<IgnoreSpellResistanceForSpells>(c => {
                    c.AllSpells = true;
                });
                bp.AddComponent<RemoveBuffAfterSpellResistCheck>();
            });
            var SorcererDestinedDestinyRealizedAbility = Helpers.CreateBlueprint<BlueprintAbility>("SorcererDestinedDestinyRealizedAbility", bp => {
                bp.SetName("bf0ce2c1494a4675923b3fcc875a85df", "Destiny Realized");
                bp.SetDescription("39024e5740c049d080972c60c785b9e2", "Once per day, you can automatically succeed at one caster level check made to overcome " +
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
            var SorcererDestinedDestinyRealized = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedDestinyRealized", bp => {
                bp.m_Icon = SorcererDestinedDestinyRealizedAbility.Icon;
                bp.SetName("352c65f97a874a51a1cc0b1d0e3cba34", "Destiny Realized");
                bp.SetDescription("e3cab258d635488d85a1b8fa546001f6", "At 20th level, your moment of destiny is at hand. Any critical threats made against you only confirm if the second "
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
            var SorcererDestinedFeatSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("SorcererDestinedFeatSelection", bp => {
                bp.name = "SorcererDestinedFeatSelection";
                bp.SetName("7a34167651034fb381484a67bec1f6fb", "Bloodline Feat Selection");
                bp.SetDescription("2c3452639a634d669d50c36f9aa137c1", "At 7th level, and every six levels thereafter, a sorcerer receives one bonus feat, chosen from a list specific to each bloodline. "
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
            var SorcererDestinedSpell3 = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedSpell3", bp => {
                var Spell = MageShield;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription("e7f17d8d00d0464d9b51cec53d09a58d", "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 1;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell5 = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedSpell5", bp => {
                var Spell = Blur;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription("4a82ce0b906844d2ae53b97e42cdf8d0", "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 2;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell7 = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedSpell7", bp => {
                var Spell = ProtectionFromEnergy;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription("4ec168fe1dd34d689dfb67232f3fe7b2", "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 3;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell9 = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedSpell9", bp => {
                var Spell = FreedomOfMovement;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription("cac92f7f6e724415b1cdadbe3c7d2915", "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 4;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell11 = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedSpell11", bp => {
                var Spell = BreakEnchantment;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription("e857cd613e85488094a8388af3aeb146", "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 5;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell13 = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedSpell13", bp => {
                var Spell = HeroismGreater;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription("42cb522f994f42639de53b7cfaa13c17", "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 6;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell15 = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedSpell15", bp => {
                var Spell = CircleOfClarity;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription("ed91351bc04644e48b1377bb0f6ffa3d", "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 7;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell17 = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedSpell17", bp => {
                var Spell = ProtectionFromSpells;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription("6fba8820ed0342288d3f7fe73862fd52", "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 8;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            var SorcererDestinedSpell19 = Helpers.CreateBlueprint<BlueprintFeature>("SorcererDestinedSpell19", bp => {
                var Spell = Foresight;
                bp.IsClassFeature = true;
                bp.SetName(Spell.Get().m_DisplayName);
                bp.SetDescription("baa77127cf4f4967b0e83404858c1a7c", "At 3rd level, and every two levels thereafter, a sorcerer learns an additional spell, derived from her bloodline.\n"
                    + $"{Spell.Get().Name}: {Spell.Get().Description}");
                bp.AddComponent<AddKnownSpell>(c => {
                    c.m_CharacterClass = SorcererClass;
                    c.m_Spell = Spell;
                    c.SpellLevel = 9;
                });
                bp.m_Icon = Spell.Get().Icon;
            });
            //Bloodline Core
            var SorcererDestinedBloodline = Helpers.CreateBlueprint<BlueprintProgression>("SorcererDestinedBloodline", bp => {
                bp.SetName("15c50bd77f0b4286a85de8cf5a5078f6", "Destined Bloodline");
                bp.SetDescription("5a3de6e67cbc48fe9b012d3ee6558ede", "Your family is destined for greatness in some way. Your birth could have been foretold in prophecy, or perhaps "
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
            var CrossbloodedDestinedBloodline = Helpers.CreateBlueprint<BlueprintProgression>("CrossbloodedDestinedBloodline", bp => {
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
            var SeekerDestinedBloodline = Helpers.CreateBlueprint<BlueprintProgression>("SeekerDestinedBloodline", bp => {
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

            if (ModSettings.AddedContent.Bloodlines.IsDisabled("DestinedBloodline")) { return; }
            BloodlineTools.RegisterSorcererBloodline(SorcererDestinedBloodline);
            BloodlineTools.RegisterCrossbloodedBloodline(CrossbloodedDestinedBloodline);
            BloodlineTools.RegisterSeekerBloodline(SeekerDestinedBloodline);
        }
    }
}