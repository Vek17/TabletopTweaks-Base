using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    internal class SandsOfTime {
        public static void AddSandsOfTime() {
            var Icon_SandsOfTime = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_SandsOfTime.png");
            var Icon_ScrollOfSandsOfTime = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfSandsOfTime.png");

            var DLC3_HasteIslandAge1 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0aca74909b144441a31866b977572f91");
            var DLC3_HasteIslandAge2 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b26c4f0f18ed41db9f78cfda0c7b874b");
            var DLC3_HasteIslandAge3 = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("9cab5a802dfd4e3e86a0623046bf88aa");

            var ConstructType = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd389783027d63343b4a5634bd81645f");
            var UndeadType = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33");

            var TouchItem = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("bb337517547de1a4189518d404ec49d4");

            var SandsOfTimeBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "SandsOfTimeBuff", bp => {
                bp.SetName(TTTContext, "Sands Of Time");
                bp.SetDescription(TTTContext, "You temporarily age the target, immediately advancing it to the next age category. " +
                    "The target immediately takes the age penalties to Strength, Dexterity, and Constitution for its new age category, " +
                    "but does not gain the bonuses for that category. A creature whose age is unknown is treated as if the spell advances it to middle age. " +
                    "If you cast this on an object, construct, or undead creature, it takes 3d6 points of damage + 1 point per caster level(maximum + 15) " +
                    "as time weathers and corrodes it instead.\n\n" +
                    "Middle Age: -1 to all physical stats +1 to all mental stats.\n" +
                    "Old Age: -3 to all physical stats +2 to all mental stats.\n" +
                    "Venerated Age: -6 to all physical stats +3 to all mental stats.");
                bp.m_Icon = Icon_SandsOfTime;
                bp.Stacking = StackingType.Replace;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.MiddleAge;
                    c.Type = UnitPartAgeTTT.NegateType.Mental;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.OldAge;
                    c.Type = UnitPartAgeTTT.NegateType.Mental;
                });
                bp.AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.Venerable;
                    c.Type = UnitPartAgeTTT.NegateType.Mental;
                });
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
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
                                            Permanent = true,
                                            AsChild = true,
                                            m_Buff = DLC3_HasteIslandAge3,
                                            DurationValue = new ContextDurationValue() {
                                                Rate = DurationRate.TenMinutes,
                                                BonusValue = new ContextValue() {
                                                    ValueType = ContextValueType.Rank
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
                                                    Permanent = true,
                                                    AsChild = true,
                                                    m_Buff = DLC3_HasteIslandAge2,
                                                    DurationValue = new ContextDurationValue() {
                                                        Rate = DurationRate.Hours,
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank
                                                        },
                                                        DiceCountValue = 0
                                                    }
                                                }
                                            ),
                                            IfFalse = Helpers.CreateActionList(
                                                new ContextActionApplyBuff() {
                                                    IsFromSpell = true,
                                                    IsNotDispelable = true,
                                                    Permanent = true,
                                                    AsChild = true,
                                                    m_Buff = DLC3_HasteIslandAge1,
                                                    DurationValue = new ContextDurationValue() {
                                                        Rate = DurationRate.Hours,
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.Rank
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
                    );
                    c.Deactivated = Helpers.CreateActionList();
                    c.NewRound = Helpers.CreateActionList();
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
            });
            var SandsOfTimeEffect = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "SandsOfTimeEffect", bp => {
                bp.SetName(SandsOfTimeBuff.m_DisplayName);
                bp.SetDescription(SandsOfTimeBuff.m_Description);
                bp.SetLocalizedDuration(TTTContext, "10 minutes/level");
                bp.SetLocalizedSavingThrow(TTTContext, "");
                bp.AvailableMetamagic = Metamagic.Extend
                    | Metamagic.Heighten
                    | Metamagic.Quicken
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent
                    | Metamagic.Reach;
                bp.Range = AbilityRange.Touch;
                bp.CanTargetFriends = true;
                bp.CanTargetEnemies = true;
                bp.SpellResistance = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.ShouldTurnToTarget = true;
                bp.m_Icon = Icon_SandsOfTime;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new Conditional() { 
                            ConditionsChecker = new ConditionsChecker() { 
                                Conditions = new Condition[] { 
                                    new ContextConditionHasFact() {
                                        m_Fact = ConstructType
                                    },
                                    new ContextConditionHasFact() {
                                        m_Fact = UndeadType
                                    }
                                },
                                Operation = Operation.Or
                            },
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    m_Buff = SandsOfTimeBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.TenMinutes,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank,
                                            ValueRank = AbilityRankType.Default
                                        },
                                        DiceCountValue = 0
                                    }
                                }
                            ),
                            IfTrue = Helpers.CreateActionList(
                                new ContextActionDealDamage() {
                                    DamageType = new DamageTypeDescription() {
                                        Type = DamageType.Direct
                                    },
                                    Duration = new ContextDurationValue() {
                                        DiceCountValue = new ContextValue(),
                                        BonusValue = new ContextValue()
                                    },
                                    Value = new ContextDiceValue() {
                                        DiceType = DiceType.D6,
                                        DiceCountValue = 3,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank,
                                            ValueRank = AbilityRankType.DamageBonus
                                        }
                                    }
                                }
                            )
                        }
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMax = true;
                    c.m_Max = 15;
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Necromancy;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Debuff;
                    c.SavingThrow = CraftSavingThrow.Will;
                    c.AOEType = CraftAOE.None;
                });
            });
            var SandsOfTimeCast = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "SandsOfTimeCast", bp => {
                bp.SetName(SandsOfTimeBuff.m_DisplayName);
                bp.SetDescription(SandsOfTimeBuff.m_Description);
                bp.SetLocalizedDuration(TTTContext, "10 minutes/level");
                bp.SetLocalizedSavingThrow(TTTContext, "");
                bp.AvailableMetamagic = Metamagic.Extend
                    | Metamagic.Heighten
                    | Metamagic.Quicken
                    | Metamagic.CompletelyNormal
                    | Metamagic.Reach;
                bp.Range = AbilityRange.Touch;
                bp.CanTargetFriends = true;
                bp.CanTargetEnemies = true;
                bp.SpellResistance = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.ShouldTurnToTarget = true;
                bp.m_Icon = Icon_SandsOfTime;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectStickyTouch>(c => {
                    c.m_TouchDeliveryAbility = SandsOfTimeEffect.ToReference<BlueprintAbilityReference>();
                });
                bp.AddComponent<AbilityDeliverTouch>(c  => {
                    c.m_TouchWeapon = TouchItem;
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Necromancy;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Debuff;
                    c.SavingThrow = CraftSavingThrow.None;
                    c.AOEType = CraftAOE.None;
                });
            });

            var ScrollOfSandsOfTime = ItemTools.CreateScroll(TTTContext, "ScrollOfSandsOfTime", Icon_ScrollOfSandsOfTime, SandsOfTimeCast, 3, 5);

            if (TTTContext.AddedContent.Spells.IsDisabled("SandsOfTime")) { return; }

            VenderTools.AddScrollToLeveledVenders(ScrollOfSandsOfTime);

            SandsOfTimeCast.AddToSpellList(SpellTools.SpellList.ClericSpellList, 3);
            SandsOfTimeCast.AddToSpellList(SpellTools.SpellList.ShamanSpelllist, 3);
            SandsOfTimeCast.AddToSpellList(SpellTools.SpellList.WizardSpellList, 3);
            SandsOfTimeCast.AddToSpellList(SpellTools.SpellList.WitchSpellList, 3);
        }
    }
}
