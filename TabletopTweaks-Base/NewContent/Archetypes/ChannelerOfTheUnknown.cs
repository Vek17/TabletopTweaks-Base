﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Archetypes {
    static class ChannelerOfTheUnknown {
        private static readonly BlueprintFeatureSelection DeitySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("59e7a76987fe3b547b9cce045f4db3e4");
        private static readonly BlueprintFeatureSelection MartialWeaponProficencySelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "MartialWeaponProficencySelection");
        private static readonly BlueprintFeatureSelection ExoticWeaponProficiencySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9a01b6815d6c3684cb25f30b8bf20932");
        private static readonly BlueprintFeature LightArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132");
        private static readonly BlueprintFeature MediumArmorProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("46f4fb320f35704488ba3d513397789d");
        private static readonly BlueprintFeature SimpleWeaponProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("e70ecf1ed95ca2f40b754f1adb22bbdd");
        private static readonly BlueprintFeature ShieldsProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("cb8686e7357a68c42bdd9d4e65334633");

        private static readonly BlueprintCharacterClass ClericClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("67819271767a9dd4fbfd4ae700befea0");
        private static readonly BlueprintSpellsTable CrusaderSpellLevels = BlueprintTools.GetBlueprint<BlueprintSpellsTable>("799265ebe0ed27641b6d415251943d03");
        private static readonly BlueprintFeature ClericProficiencies = BlueprintTools.GetBlueprint<BlueprintFeature>("8c971173613282844888dc20d572cfc9");
        private static readonly BlueprintFeatureSelection ChannelEnergySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("d332c1748445e8f4f9e92763123e31bd");
        private static readonly BlueprintFeatureSelection DomainsSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("48525e5da45c9c243a343fc6545dbdb9");
        private static readonly BlueprintFeatureSelection SecondDomainsSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("43281c3d7fe18cc4d91928395837cd1e");

        private static readonly BlueprintFeature DarknessDomainProgression = BlueprintTools.GetBlueprint<BlueprintFeature>("1e1b4128290b11a41ba55280ede90d7d");
        private static readonly BlueprintFeature DestructionDomainProgression = BlueprintTools.GetBlueprint<BlueprintFeature>("269ff0bf4596f5248864bc2653a2f0e0");
        private static readonly BlueprintFeature LuckDomainProgression = BlueprintTools.GetBlueprint<BlueprintFeature>("8bd8cfad69085654b9118534e4aa215e");
        private static readonly BlueprintFeature MadnessDomainProgression = BlueprintTools.GetBlueprint<BlueprintFeature>("9ebe166b9b901c746b1858029f13a2c5");

        private static readonly BlueprintUnitFact ChannelEnergyFact = BlueprintTools.GetBlueprint<BlueprintUnitFact>("93f062bc0bf70e84ebae436e325e30e8");
        private static readonly BlueprintAbility ChannelNegativeEnergy = BlueprintTools.GetBlueprint<BlueprintAbility>("89df18039ef22174b81052e2e419c728");
        private static readonly BlueprintAbilityResource ChannelEnergyResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("5e2bba3e07c37be42909a12945c27de7");
        private static readonly BlueprintUnitProperty MythicChannelProperty = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("152e61de154108d489ff34b98066c25c");
        private static readonly BlueprintFeature SelectiveChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff");
        private static readonly BlueprintFeature ExtraChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("cd9f19775bd9d3343a31a065e93f0c47");

        private static readonly BlueprintCharacterClass MysticTheurgeClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("0920ea7e4fd7a404282e3d8b0ac41838");
        private static readonly BlueprintFeatureSelection MysticTheurgeDivineSpellbookSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7cd057944ce7896479717778330a4933");
        private static readonly BlueprintProgression MysticTheurgeClericProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8bac42667e6f67047acbcbd668cf2029");

        private static readonly BlueprintCharacterClass HellknightSigniferClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("ee6425d6392101843af35f756ce7fefd");
        private static readonly BlueprintFeatureSelection HellknightSigniferSpellbook = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("68782aa7a302b6d43a42a71c6e9b5277");
        private static readonly BlueprintProgression HellknightSigniferClericProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("e673d91c731469549b8962016f48410e");

        private static readonly BlueprintFeatureSelectMythicSpellbook AngelIncorporateSpellbook = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("e1fbb0e0e610a3a4d91e5e5284587939");

        public static void AddChannelerOfTheUnknown() {
            var ChannelEntropyIcon = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_ChannelEntropy.png");
            var ChannelerOfTheUnknownSpellLevels = Helpers.CreateBlueprint<BlueprintSpellsTable>(TTTContext, "ChannelerOfTheUnknownSpellLevels", bp => {
                bp.Levels = CrusaderSpellLevels.Levels.Select(level => SpellTools.CreateSpellLevelEntry(level.Count)).ToArray();
            });
            var ChannelerOfTheUnknownSpellbook = Helpers.CreateBlueprint<BlueprintSpellbook>(TTTContext, "ChannelerOfTheUnknownSpellbook", bp => {
                bp.Name = ClericClass.Spellbook.Name;
                bp.CastingAttribute = ClericClass.Spellbook.CastingAttribute;
                bp.AllSpellsKnown = ClericClass.Spellbook.AllSpellsKnown;
                bp.CantripsType = ClericClass.Spellbook.CantripsType;
                bp.HasSpecialSpellList = ClericClass.Spellbook.HasSpecialSpellList;
                bp.SpecialSpellListName = ClericClass.Spellbook.SpecialSpellListName;
                bp.m_SpellsPerDay = ChannelerOfTheUnknownSpellLevels.ToReference<BlueprintSpellsTableReference>();
                bp.m_SpellsKnown = ClericClass.Spellbook.m_SpellsKnown;
                bp.m_SpellSlots = ClericClass.Spellbook.m_SpellSlots;
                bp.m_SpellList = ClericClass.Spellbook.m_SpellList;
                bp.m_MythicSpellList = ClericClass.Spellbook.m_MythicSpellList;
                bp.m_CharacterClass = ClericClass.Spellbook.m_CharacterClass;
                bp.m_Overrides = ClericClass.Spellbook.m_Overrides;
                bp.IsArcane = false;
                bp.AddComponent<CustomSpecialSlotAmount>(c => c.Amount = 2);
                SpellTools.Spellbook.AllSpellbooks.Add(bp);
            });
            var ChannelerOfTheUnknownProficiencies = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ChannelerOfTheUnknownProficiencies", bp => {
                bp.SetName(TTTContext, "Channeler Of The Unknown Proficiencies");
                bp.SetDescription(TTTContext, "Channelers Of The Unknown are proficient with all {g|Encyclopedia:Weapon_Proficiency}simple weapons{/g}, light armor, medium armor, and shields (except tower shields).");
                bp.m_Icon = ClericProficiencies.Icon;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        LightArmorProficiency.ToReference<BlueprintUnitFactReference>(),
                        MediumArmorProficiency.ToReference<BlueprintUnitFactReference>(),
                        SimpleWeaponProficiency.ToReference<BlueprintUnitFactReference>(),
                        ShieldsProficiency.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            var ChannelerOfTheUnknownWeaponProficiency = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ChannelerOfTheUnknownWeaponProficiency", bp => {
                bp.SetName(TTTContext, "Bonus Weapon Proficiency");
                bp.SetDescription(TTTContext, "A channeler of the unknown loses proficiency with her deity’s favored weapon. She instead gains proficiency with one martial or exotic weapon, " +
                    "chosen when she first takes this archetype, which thereafter effectively functions as her holy or unholy symbol for the purposes of class abilities and spellcasting. " +
                    "Once she makes this choice, she can’t later change it.");
                bp.m_Icon = ClericProficiencies.Icon;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddFeatures(MartialWeaponProficencySelection.AllFeatures.Select(feature => feature).ToArray());
                bp.AddFeatures(ExoticWeaponProficiencySelection.AllFeatures.Select(feature => feature).ToArray());
            });
            var ChannelerOfTheUnknownPowerOfTheUnknown = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ChannelerOfTheUnknownPowerOfTheUnknown", bp => {
                bp.SetName(TTTContext, "Power of the Unknown");
                bp.SetDescription(TTTContext, "A channeler of the unknown has lost the benefit of the domains granted by her deity, but the unknown entity that answers her " +
                    "supplications instead grants her the benefits of one domain from the following list: Darkness, Destruction, Luck, Madness, or Void. Instead " +
                    "of a single domain spell slot, the channeler of the unknown gains two domain spell slots per spell level she can cast. A channeler of the " +
                    "unknown cannot select a subdomain in place of the domain available to her.");
                bp.m_Icon = DomainsSelection.Icon;
                bp.Group = FeatureGroup.Domain;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.AddFeatures(DarknessDomainProgression, DestructionDomainProgression, LuckDomainProgression, MadnessDomainProgression);
                bp.AddComponent<AddFacts>(c => {
                    // To support all features that check for domains this way
                    c.m_Facts = new BlueprintUnitFactReference[] { DomainsSelection.ToReference<BlueprintUnitFactReference>() };
                });
            });
            var ChannelerOfTheUnknownChannelEntropyAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "ChannelerOfTheUnknownChannelEntropyAbility", bp => {
                bp.SetName(TTTContext, "Channel Entropy");
                bp.SetDescription(TTTContext, "Channeling entropy causes a burst that damages all creatures in a 30-foot radius centered on the cleric. The amount of damage " +
                    "inflicted is equal to 1d6 points of damage plus 1d6 points of damage for every two cleric levels beyond 1st (2d6 at 3rd, 3d6 at 5th, and so on). " +
                    "Creatures that take damage from channeled energy receive a Will save to halve the damage. " +
                    "The DC of this save is equal to 10 + 1/2 the cleric's level + the cleric's Charisma modifier.");
                bp.m_Icon = ChannelEntropyIcon;
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AvailableMetamagic = Metamagic.Empower | Metamagic.Maximize | Metamagic.Heighten | Metamagic.Quicken;
                bp.Range = AbilityRange.Personal;
                bp.Type = AbilityType.Special;
                bp.CanTargetEnemies = true;
                bp.CanTargetFriends = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.ResourceAssetIds = ChannelNegativeEnergy.ResourceAssetIds;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = ChannelEnergyResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                });
                bp.AddComponent<AbilityTargetsAround>(c => {
                    c.m_Radius = 30.Feet();
                    c.m_TargetType = TargetType.Any;
                    c.m_Condition = new ConditionsChecker() {
                        Conditions = new Condition[0]
                    };
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = ChannelNegativeEnergy.GetComponent<AbilitySpawnFx>().PrefabLink;
                    c.PositionAnchor = AbilitySpawnFxAnchor.None;
                    c.OrientationAnchor = AbilitySpawnFxAnchor.None;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = new SpellDescriptorWrapper(SpellDescriptor.ChannelNegativeHarm);
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { ClericClass.ToReference<BlueprintCharacterClassReference>() };
                    c.m_Progression = ContextRankProgression.DelayedStartPlusDivStep;
                    c.m_StartLevel = 1;
                    c.m_StepLevel = 2;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_CustomProperty = MythicChannelProperty.ToReference<BlueprintUnitPropertyReference>();
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_Min = 0;
                    c.m_UseMin = true;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    new ContextConditionCasterHasFact() {
                                        m_Fact = SelectiveChannel.ToReference<BlueprintUnitFactReference>()
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new Conditional() {
                                    ConditionsChecker = new ConditionsChecker() {
                                        Conditions = new Condition[] {
                                            new ContextConditionIsEnemy()
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(
                                        new ContextActionSavingThrow() {
                                            m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                                            Type = SavingThrowType.Will,
                                            CustomDC = new ContextValue(),
                                            Actions = Helpers.CreateActionList(
                                                new ContextActionDealDamage() {
                                                    DamageType = new DamageTypeDescription() {
                                                        Type = DamageType.Direct,
                                                        Common = new DamageTypeDescription.CommomData(),
                                                        Physical = new DamageTypeDescription.PhysicalData(),
                                                    },
                                                    Duration = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        DiceCountValue = new ContextValue(),
                                                        BonusValue = new ContextValue()
                                                    },
                                                    Value = new ContextDiceValue() {
                                                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                                        DiceCountValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageDice
                                                        },
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageBonus
                                                        }
                                                    },
                                                    IsAoE = true,
                                                    HalfIfSaved = true
                                                }
                                            )
                                        }
                                    ),
                                    IfFalse = Helpers.CreateActionList()
                                }
                            ),
                            IfFalse = Helpers.CreateActionList(
                                new Conditional() {
                                    ConditionsChecker = new ConditionsChecker() {
                                        Conditions = new Condition[] {
                                            new ContextConditionIsCaster(){
                                                Not = true
                                            }
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(
                                        new ContextActionSavingThrow() {
                                            m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                                            Type = SavingThrowType.Will,
                                            CustomDC = new ContextValue(),
                                            Actions = Helpers.CreateActionList(
                                                new ContextActionDealDamage() {
                                                    DamageType = new DamageTypeDescription() {
                                                        Type = DamageType.Direct,
                                                        Common = new DamageTypeDescription.CommomData(),
                                                        Physical = new DamageTypeDescription.PhysicalData(),
                                                    },
                                                    Duration = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        DiceCountValue = new ContextValue(),
                                                        BonusValue = new ContextValue()
                                                    },
                                                    Value = new ContextDiceValue() {
                                                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                                                        DiceCountValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageDice
                                                        },
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageBonus
                                                        }
                                                    },
                                                    IsAoE = true,
                                                    HalfIfSaved = true
                                                }
                                            )
                                        }
                                    ),
                                    IfFalse = Helpers.CreateActionList()
                                }
                            )
                        }
                    );
                });
            });
            var ChannelerOfTheUnknownChannelEntropyFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ChannelerOfTheUnknownChannelEntropyFeature", bp => {
                bp.SetName(TTTContext, "Channel Entropy");
                bp.SetDescription(TTTContext, "A channeler of the unknown can channel entropy as a cleric channels negative or positive energy, releasing a wave of twisting void that harms " +
                    "creatures in the area of effect. The amount of damage dealt is equal to that an evil cleric of her level would deal by channeling negative energy, except it " +
                    "affects living, unliving, and undead creatures alike. This functions in all other ways as a cleric’s channel energy class feature, including benefiting from feats " +
                    "that affect channel energy (such as Selective Channeling).");
                bp.m_Icon = ChannelEntropyIcon;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        ChannelEnergyFact.ToReference<BlueprintUnitFactReference>(),
                        ChannelerOfTheUnknownChannelEntropyAbility.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            var ChannelerOfTheUnknownSpontaneousCasting = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ChannelerOfTheUnknownSpontaneousCasting", bp => {
                bp.SetName(TTTContext, "Spontaneous Casting");
                bp.SetDescription(TTTContext, "Instead of converting prepared spells into cure or inflict spells, a channeler of the unknown can channel stored spell energy into her domain spells.\n" +
                    "She can lose a prepared spell, including a domain spell, to spontaneously cast a domain spell of the same spell level or lower.");
                bp.m_Icon = DomainsSelection.Icon;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddComponent<SpontaneousSpecialListConversion>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                });
            });
            var ChannelerOfTheUnknownArchetype = Helpers.CreateBlueprint<BlueprintArchetype>(TTTContext, "ChannelerOfTheUnknownArchetype", bp => {
                bp.SetName(TTTContext, "Channeler of the Unknown");
                bp.SetDescription(TTTContext, "While most clerics who fall out of favor with their deities " +
                    "simply lose their divine connection and the powers it granted, a few continue to go through the motions of prayer and obedience, persisting " +
                    "in the habits of faith even when their faith itself has faded. Among these, an even smaller number find that while their original deity no " +
                    "longer answers their prayers, something else does: an unknown entity or force of the universe channeling its power through a trained and " +
                    "practicing vessel.");
                bp.m_ReplaceSpellbook = ChannelerOfTheUnknownSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.RemoveFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1,
                        ClericProficiencies,
                        ChannelEnergySelection,
                        DomainsSelection,
                        SecondDomainsSelection
                    ),
                };
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1,
                        ChannelerOfTheUnknownProficiencies,
                        ChannelerOfTheUnknownWeaponProficiency,
                        ChannelerOfTheUnknownPowerOfTheUnknown,
                        ChannelerOfTheUnknownSpontaneousCasting,
                        ChannelerOfTheUnknownChannelEntropyFeature
                    ),
                };
            });

            var MysticTheurgeChannelerOfTheUnknownLevelUp = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MysticTheurgeChannelerOfTheUnknownLevelUp", bp => {
                bp.SetName(TTTContext, "Channeler Of The Unknown");
                bp.SetDescription(TTTContext, "At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 10;
                bp.HideInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.AddComponent<AddSpellbookLevel>(c => {
                    c.m_Spellbook = ChannelerOfTheUnknownSpellbook.ToReference<BlueprintSpellbookReference>();
                });
            });
            var MysticTheurgeChannelerOfTheUnknownProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "MysticTheurgeChannelerOfTheUnknownProgression", bp => {
                bp.SetName(TTTContext, "Channeler Of The Unknown");
                bp.SetDescription(TTTContext, "At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.HideNotAvailibleInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel{
                        m_Class = MysticTheurgeClass.ToReference<BlueprintCharacterClassReference>()
                    }
                };
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(2, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(3, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(4, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(5, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(6, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(7, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(8, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(9, MysticTheurgeChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(10, MysticTheurgeChannelerOfTheUnknownLevelUp)
                };
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.RequiredSpellLevel = 2;
                });
                bp.AddComponent<MysticTheurgeSpellbook>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_MysticTheurge = MysticTheurgeClass.ToReference<BlueprintCharacterClassReference>();
                });
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });

            var HellknightSigniferChannelerOfTheUnknownLevelUp = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "HellknightSigniferChannelerOfTheUnknownLevelUp", bp => {
                bp.SetName(TTTContext, "Channeler Of The Unknown");
                bp.SetDescription(TTTContext, "At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 10;
                bp.HideInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.AddComponent<AddSpellbookLevel>(c => {
                    c.m_Spellbook = ChannelerOfTheUnknownSpellbook.ToReference<BlueprintSpellbookReference>();
                });
            });
            var HellknightSigniferChannelerOfTheUnknownProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "HellknightSigniferChannelerOfTheUnknownProgression", bp => {
                bp.SetName(TTTContext, "Channeler Of The Unknown");
                bp.SetDescription(TTTContext, "At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.HideNotAvailibleInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel{
                        m_Class = HellknightSigniferClass.ToReference<BlueprintCharacterClassReference>()
                    }
                };
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(2, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(3, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(4, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(5, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(6, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(7, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(8, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(9, HellknightSigniferChannelerOfTheUnknownLevelUp),
                    Helpers.CreateLevelEntry(10, HellknightSigniferChannelerOfTheUnknownLevelUp)
                };
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.RequiredSpellLevel = 2;
                });
                bp.AddComponent<MysticTheurgeSpellbook>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_MysticTheurge = MysticTheurgeClass.ToReference<BlueprintCharacterClassReference>();
                });
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });

            if (TTTContext.AddedContent.Archetypes.IsDisabled("ChannelerOfTheUnknown")) { return; }
            ClericClass.m_Archetypes = ClericClass.m_Archetypes.AppendToArray(ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>());
            SelectiveChannel.AddPrerequisiteFeature(ChannelerOfTheUnknownChannelEntropyFeature, Prerequisite.GroupType.Any);
            TTTContext.Logger.LogPatch("Patched", SelectiveChannel);
            ExtraChannel.AddPrerequisiteFeature(ChannelerOfTheUnknownChannelEntropyFeature, Prerequisite.GroupType.Any);
            TTTContext.Logger.LogPatch("Patched", ExtraChannel);
            DeitySelection.AllFeatures.ForEach(deity => {
                var addFeature = deity.GetComponent<AddFeatureOnClassLevel>();
                if (addFeature != null) {
                    deity.AddComponent<AddFeatureOnClassLevelExclude>(c => {
                        c.m_Class = addFeature.m_Class;
                        c.m_AdditionalClasses = addFeature.m_AdditionalClasses;
                        c.m_Archetypes = addFeature.m_Archetypes;
                        c.m_ExcludeClass = addFeature.m_Class;
                        c.m_AdditionalExcludeClasses = new BlueprintCharacterClassReference[0];
                        c.m_ExcludeArchetypes = new BlueprintArchetypeReference[] { ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>() };
                        c.m_Feature = addFeature.m_Feature;
                        c.Level = addFeature.Level;
                        c.BeforeThisLevel = addFeature.BeforeThisLevel;
                    });
                    deity.RemoveComponent(addFeature);
                    TTTContext.Logger.LogPatch("Patched", deity);
                }
            });
            // Add to Mystic Theurge
            MysticTheurgeClericProgression.AddPrerequisite<PrerequisiteNoArchetype>(c => {
                c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                c.m_Archetype = ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>();
            });
            MysticTheurgeDivineSpellbookSelection.AddFeatures(MysticTheurgeChannelerOfTheUnknownProgression);
            TTTContext.Logger.LogPatch("Patched", MysticTheurgeClericProgression);
            TTTContext.Logger.LogPatch("Patched", MysticTheurgeDivineSpellbookSelection);
            // Add to Hellknight Signifier
            HellknightSigniferClericProgression.AddPrerequisite<PrerequisiteNoArchetype>(c => {
                c.m_CharacterClass = ClericClass.ToReference<BlueprintCharacterClassReference>();
                c.m_Archetype = ChannelerOfTheUnknownArchetype.ToReference<BlueprintArchetypeReference>();
            });
            HellknightSigniferSpellbook.AddFeatures(HellknightSigniferChannelerOfTheUnknownProgression);
            TTTContext.Logger.LogPatch("Patched", HellknightSigniferClericProgression);
            TTTContext.Logger.LogPatch("Patched", HellknightSigniferSpellbook);
            // Enable Angel Merge
            AngelIncorporateSpellbook.m_AllowedSpellbooks = AngelIncorporateSpellbook.m_AllowedSpellbooks.AppendToArray(ChannelerOfTheUnknownSpellbook.ToReference<BlueprintSpellbookReference>());
            TTTContext.Logger.LogPatch("Patched", AngelIncorporateSpellbook);
        }
    }
}
