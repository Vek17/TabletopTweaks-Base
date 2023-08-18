using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Hexes {
    internal class Withering {
        public static void AddWithering() {

            var WitchMajorHex = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("8ac781b33e380c84aa578f1b006dd6c5");
            var WitchHexDCProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("bdc230ce338f427ba74de65597b0d57a");
            var WitchHexCasterLevelProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("2d2243f4f3654512bdda92e80ef65b6d");
            var WitchHexSpellLevelProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("75efe8b64a3a4cd09dda28cef156cfb5");
            var SylvanTricksterArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("490394869f666c141bf8647b1a365220");
            var HexcrafterArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("79ccf7a306a5d5547bebd97299f6fc89");

            var DLC3_HasteIslandAge1 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0aca74909b144441a31866b977572f91");
            var DLC3_HasteIslandAge2 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b26c4f0f18ed41db9f78cfda0c7b874b");
            var DLC3_HasteIslandAge3 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("9cab5a802dfd4e3e86a0623046bf88aa");

            var AgelessFeature = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(TTTContext, "AgelessFeature");

            var WinterWitchWitchHex = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b921af3627142bd4d9cf3aefb5e2610a");
            var WitchHexSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9846043cf51251a4897728ed6e24e76f");
            var SylvanTricksterTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");
            var HexcrafterMagusHexMagusSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("a18b8c3d6251d8641a8094e5c2a7bc78");
            var HexcrafterMagusHexArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ad6b9cecb5286d841a66e23cea3ef7bf");

            var Icon_Withering = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_Withering.png");

            var WitchHexWitheringBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "WitchHexWitheringBuff", bp => {
                bp.SetName(TTTContext, "Withering — Stolen Time");
                bp.SetDescription(TTTContext, "The witch causes a creature within 30 feet to age rapidly, empowering the witch in the process.\n" +
                    "The target ages to the next age category (adult to middle-aged, and so on). " +
                    "The witch gains a number of temporary hit points equal to 1d10 + her witch level and a +2 enhancement bonus " +
                    "to Constitution for a number of hours equal to her Intelligence modifier. " +
                    "These effects last for a number of hours equal to the witch’s level.\n" +
                    "A creature cannot be aged past venerable age by this hex, and it can attempt a Fortitude save to negate the effect altogether. " +
                    "Once a creature has successfully saved against the withering hex, it cannot be affected by it again.\n\n" +
                    "Middle Age: -1 to all physical stats +1 to all mental stats.\n" +
                    "Old Age: -3 to all physical stats +2 to all mental stats.\n" +
                    "Venerated Age: -6 to all physical stats +3 to all mental stats.");
                bp.m_Icon = Icon_Withering;
                bp.ResourceAssetIds = new string[] { };
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.TemporaryHP;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.Constitution;
                    c.Descriptor = ModifierDescriptor.Enhancement;
                    c.Value = 2;
                });
                bp.AddComponent<TemporaryHitPointsFromAbilityValue>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Shared,
                        ValueShared = AbilitySharedValue.Heal
                    };
                    c.RemoveWhenHitPointsEnd = false;
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMax = true;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[] {
                        ClassTools.ClassReferences.WitchClass,
                        ClassTools.ClassReferences.WinterWitchClass,
                        ClassTools.ClassReferences.MagusClass,
                        ClassTools.ClassReferences.RogueClass,
                    };
                    c.Archetype = SylvanTricksterArchetype;
                    c.m_AdditionalArchetypes = new BlueprintArchetypeReference[] {
                        HexcrafterArchetype
                    };
                });
                bp.AddComponent<ContextCalculateSharedValue>(c => {
                    c.ValueType = AbilitySharedValue.Heal;
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.D10,
                        DiceCountValue = 1,
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageBonus,
                        }
                    };
                    c.Modifier = 1;
                });
            });
            var WitchHexWitheringCooldownBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "WitchHexWitheringCooldownBuff", bp => {
                bp.SetName(TTTContext, "Withering Cooldown");
                bp.SetDescription(TTTContext, "");
                bp.m_Icon = Icon_Withering;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });
            var WitchHexWitheringAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "WitchHexWitheringAbility", bp => {
                bp.SetName(TTTContext, "Withering");
                bp.SetDescription(WitchHexWitheringBuff.m_Description);
                bp.SetLocalizedSavingThrow(TTTContext, "Fortitude negates");
                bp.SetLocalizedDuration(TTTContext, "1 minute/level");
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.CompletelyNormal | Metamagic.Reach;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Close;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.CanTargetEnemies = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_Withering;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    new ContextConditionIsAlly()
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    ToCaster = true,
                                    m_Buff = WitchHexWitheringBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Hours,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank,
                                            ValueRank = AbilityRankType.DamageBonus
                                        },
                                        DiceCountValue = 0
                                    }
                                },
                                new Conditional() {
                                    ConditionsChecker = new ConditionsChecker() {
                                        Conditions = new Condition[] {
                                            new ContextConditionHasBuff(){
                                                m_Buff = DLC3_HasteIslandAge3
                                            }
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(),
                                    IfFalse = Helpers.CreateActionList(
                                        new Conditional() {
                                            ConditionsChecker = new ConditionsChecker() {
                                                Conditions = new Condition[] {
                                                    new ContextConditionHasBuff(){
                                                        m_Buff = DLC3_HasteIslandAge2
                                                    }
                                                 }
                                            },
                                            IfTrue = Helpers.CreateActionList(
                                                new ContextActionApplyBuff() {
                                                    IsFromSpell = true,
                                                    IsNotDispelable = true,
                                                    m_Buff = DLC3_HasteIslandAge3,
                                                    DurationValue = new ContextDurationValue() {
                                                        Rate = DurationRate.Hours,
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageBonus
                                                        },
                                                        DiceCountValue = 0
                                                    }
                                                }
                                            ),
                                            IfFalse = Helpers.CreateActionList(
                                                new Conditional() {
                                                    ConditionsChecker = new ConditionsChecker() {
                                                        Conditions = new Condition[] {
                                                            new ContextConditionHasBuff(){
                                                                m_Buff = DLC3_HasteIslandAge1
                                                            }
                                                        }
                                                    },
                                                    IfTrue = Helpers.CreateActionList(
                                                        new ContextActionApplyBuff() {
                                                            IsFromSpell = true,
                                                            IsNotDispelable = true,
                                                            m_Buff = DLC3_HasteIslandAge2,
                                                            DurationValue = new ContextDurationValue() {
                                                                Rate = DurationRate.Hours,
                                                                BonusValue = new ContextValue() {
                                                                    ValueType = ContextValueType.Rank,
                                                                    ValueRank = AbilityRankType.DamageBonus
                                                                },
                                                                DiceCountValue = 0
                                                            }
                                                        }
                                                    ),
                                                    IfFalse = Helpers.CreateActionList(
                                                        new ContextActionApplyBuff() {
                                                            IsFromSpell = true,
                                                            IsNotDispelable = true,
                                                            m_Buff = DLC3_HasteIslandAge1,
                                                            DurationValue = new ContextDurationValue() {
                                                                Rate = DurationRate.Hours,
                                                                BonusValue = new ContextValue() {
                                                                    ValueType = ContextValueType.Rank,
                                                                    ValueRank = AbilityRankType.DamageBonus
                                                                },
                                                                DiceCountValue = 0
                                                            }
                                                        }
                                                    )
                                                }
                                            )
                                        }
                                    )
                                }
                            ),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionSavingThrow() {
                                    Type = SavingThrowType.Fortitude,
                                    Actions = Helpers.CreateActionList(
                                        new ContextActionConditionalSaved() {
                                            Succeed = Helpers.CreateActionList(
                                                new ContextActionApplyBuff() {
                                                    IsFromSpell = true,
                                                    IsNotDispelable = true,
                                                    m_Buff = WitchHexWitheringCooldownBuff.ToReference<BlueprintBuffReference>(),
                                                    DurationValue = new ContextDurationValue() {
                                                        Rate = DurationRate.Days,
                                                        BonusValue = 1,
                                                        DiceCountValue = 0
                                                    }
                                                }
                                            ),
                                            Failed = Helpers.CreateActionList(
                                                new ContextActionApplyBuff() {
                                                    IsFromSpell = true,
                                                    ToCaster = true,
                                                    m_Buff = WitchHexWitheringBuff.ToReference<BlueprintBuffReference>(),
                                                    DurationValue = new ContextDurationValue() {
                                                        Rate = DurationRate.Hours,
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank,
                                                            ValueRank = AbilityRankType.DamageBonus
                                                        },
                                                        DiceCountValue = 0
                                                    }
                                                },
                                                new Conditional() {
                                                    ConditionsChecker = new ConditionsChecker() {
                                                        Conditions = new Condition[] {
                                                            new ContextConditionHasBuff(){
                                                                m_Buff = DLC3_HasteIslandAge3
                                                            }
                                                        }
                                                    },
                                                    IfTrue = Helpers.CreateActionList(),
                                                    IfFalse = Helpers.CreateActionList(
                                                        new Conditional() {
                                                            ConditionsChecker = new ConditionsChecker() {
                                                                Conditions = new Condition[] {
                                                                    new ContextConditionHasBuff(){
                                                                        m_Buff = DLC3_HasteIslandAge2
                                                                    }
                                                                 }
                                                            },
                                                            IfTrue = Helpers.CreateActionList(
                                                                new ContextActionApplyBuff() {
                                                                    IsFromSpell = true,
                                                                    IsNotDispelable = true,
                                                                    m_Buff = DLC3_HasteIslandAge3,
                                                                    DurationValue = new ContextDurationValue() {
                                                                        Rate = DurationRate.Hours,
                                                                        BonusValue = new ContextValue() {
                                                                            ValueType = ContextValueType.Rank,
                                                                            ValueRank = AbilityRankType.DamageBonus
                                                                        },
                                                                        DiceCountValue = 0
                                                                    }
                                                                }
                                                            ),
                                                            IfFalse = Helpers.CreateActionList(
                                                                new Conditional() {
                                                                    ConditionsChecker = new ConditionsChecker() {
                                                                        Conditions = new Condition[] {
                                                                            new ContextConditionHasBuff(){
                                                                                m_Buff = DLC3_HasteIslandAge1
                                                                            }
                                                                        }
                                                                    },
                                                                    IfTrue = Helpers.CreateActionList(
                                                                        new ContextActionApplyBuff() {
                                                                            IsFromSpell = true,
                                                                            IsNotDispelable = true,
                                                                            m_Buff = DLC3_HasteIslandAge2,
                                                                            DurationValue = new ContextDurationValue() {
                                                                                Rate = DurationRate.Hours,
                                                                                BonusValue = new ContextValue() {
                                                                                    ValueType = ContextValueType.Rank,
                                                                                    ValueRank = AbilityRankType.DamageBonus
                                                                                },
                                                                                DiceCountValue = 0
                                                                            }
                                                                        }
                                                                    ),
                                                                    IfFalse = Helpers.CreateActionList(
                                                                        new ContextActionApplyBuff() {
                                                                            IsFromSpell = true,
                                                                            IsNotDispelable = true,
                                                                            m_Buff = DLC3_HasteIslandAge1,
                                                                            DurationValue = new ContextDurationValue() {
                                                                                Rate = DurationRate.Hours,
                                                                                BonusValue = new ContextValue() {
                                                                                    ValueType = ContextValueType.Rank,
                                                                                    ValueRank = AbilityRankType.DamageBonus
                                                                                },
                                                                                DiceCountValue = 0
                                                                            }
                                                                        }
                                                                    )
                                                                }
                                                            )
                                                        }
                                                    )
                                                }
                                            )
                                        }
                                    )
                                }
                            )
                        }
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMax = true;
                    c.m_Max = 20;
                    c.m_Class = new BlueprintCharacterClassReference[] {
                        ClassTools.ClassReferences.WitchClass,
                        ClassTools.ClassReferences.WinterWitchClass,
                        ClassTools.ClassReferences.MagusClass,
                        ClassTools.ClassReferences.RogueClass,
                    };
                    c.Archetype = SylvanTricksterArchetype;
                    c.m_AdditionalArchetypes = new BlueprintArchetypeReference[] {
                        HexcrafterArchetype
                    };
                });
                bp.AddComponent<AbilityTargetHasFact>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        AgelessFeature
                    };
                    c.Inverted = true;
                });
                bp.AddComponent<AbilityTargetHasFact>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        WitchHexWitheringCooldownBuff.ToReference<BlueprintUnitFactReference>()
                    };
                    c.Inverted = true;
                });
                bp.AddComponent<ContextSetAbilityParams>(c => {
                    c.DC = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = WitchHexDCProperty
                    };
                    c.CasterLevel = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = WitchHexCasterLevelProperty
                    };
                    c.Concentration = new ContextValue();
                    c.SpellLevel = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = WitchHexSpellLevelProperty
                    };
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Hex;
                });
            });
            var WitchHexWitheringFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WitchHexWitheringFeature", bp => {
                bp.SetName(TTTContext, "Withering");
                bp.SetDescription(WitchHexWitheringBuff.m_Description);
                bp.m_Icon = Icon_Withering;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WitchHex };
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { WitchHexWitheringAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddPrerequisiteFeature(WitchMajorHex);
            });

            if (TTTContext.AddedContent.Hexes.IsDisabled("Withering")) { return; }
            WitchHexSelection.AddFeatures(WitchHexWitheringFeature);
            WinterWitchWitchHex.AddFeatures(WitchHexWitheringFeature);
            SylvanTricksterTalentSelection.AddFeatures(WitchHexWitheringFeature);
            HexcrafterMagusHexMagusSelection.AddFeatures(WitchHexWitheringFeature);
            HexcrafterMagusHexArcanaSelection.AddFeatures(WitchHexWitheringFeature);
        }
    }
}
