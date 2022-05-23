using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Classes {
    internal class Monk {
        public static void AddMonkFeatures() {
            var ScaledFistArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("5868fc82eb11a4244926363983897279");
            var MonkClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("e8f21e5b58e0569468e420ebea456124");
            var MonkProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a91753b978e3b34b9425419179aafd6");

            var StunningFistMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("a29a582c3daa4c24bb0e991c596ccb28");
            var StunningFist = BlueprintTools.GetBlueprint<BlueprintFeature>("a29a582c3daa4c24bb0e991c596ccb28");
            var StunningFistResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("d2bae584db4bf4f4f86dd9d15ae56558");
            var StunningFistAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("732ae7773baf15447a6737ae6547fc1e");
            var StunningFistSickenedFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d256ab3837538cc489d4b571e3a813eb");
            var StunningFistFatigueFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("819645da2e446f84d9b168ed1676ec29");
            var StunningFistFatigueAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("32f92fea1ab81c843a436a49f522bfa1");
            var StunningFistSickenedAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c81906c75821cbe4c897fa11bdaeee01");
            var DragonFerocityBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("8709a00782de26d4a8524732879000fa");

            var StunningFistOwnerBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("d9eaeba5690a7704da8bbf626456a50e");
            var StunningFistOwnerFatigueBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("696b29374599d4141be64e46a91bd09b");
            var StunningFistOwnerSickenedBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("4d7da6df5cb3b3940a9d96311a2dc311");

            var Shaken = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("25ec6cb6ab1845c48a95f9c20b034220");
            var Staggered = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("df3950af5a783bd4d91ab73eb8fa0fd3");
            var BlindnessBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("187f88d96a0ef464280706b63635f2af");
            var Paralyzed = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("af1e2d232ebbb334aaf25e2a46a92591");

            var Icon_StunningFistStagger = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_StunningFistStagger.png");
            var Icon_StunningFistBlind = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_StunningFistBlind.png");
            var Icon_StunningFistParalyze = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_StunningFistParalyze.png");

            var StunningFistStaggeredOwnerBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "StunningFistStaggeredOwnerBuff", bp => {
                bp.SetName(TTTContext, "Stunning Fist: Stagger");
                bp.SetDescription(TTTContext, "This ability works as Stunning Fist, but it makes the target staggered for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.");
                bp.m_Icon = Icon_StunningFistStagger;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.UnarmedStrike;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionSavingThrow>(savingThrow => {
                            savingThrow.Type = SavingThrowType.Fortitude;
                            savingThrow.CustomDC = new ContextValue();
                            savingThrow.Actions = Helpers.CreateActionList(
                                Helpers.Create<ContextActionConditionalSaved>(saved => {
                                    saved.Succeed = Helpers.CreateActionList();
                                    saved.Failed = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                                            applyBuff.m_Buff = Staggered;
                                            applyBuff.DurationValue = new ContextDurationValue() {
                                                m_IsExtendable = true,
                                                Rate = DurationRate.Rounds,
                                                DiceType = DiceType.D6,
                                                DiceCountValue = 1,
                                                BonusValue = 1
                                            };
                                        }),
                                        Helpers.Create<Conditional>(conditional => {
                                            conditional.ConditionsChecker = new ConditionsChecker() { 
                                                Conditions = new Condition[] {
                                                    new ContextConditionCasterHasFact(){
                                                        m_Fact = DragonFerocityBuff
                                                    }
                                                }
                                            };
                                            conditional.IfFalse = Helpers.CreateActionList();
                                            conditional.IfTrue = Helpers.CreateActionList(
                                                Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                                                    applyBuff.m_Buff = Shaken;
                                                    applyBuff.DurationValue = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        Rate = DurationRate.Rounds,
                                                        DiceType = DiceType.D4,
                                                        DiceCountValue = 1,
                                                        BonusValue = new ContextValue() { 
                                                            ValueType = ContextValueType.CasterProperty,
                                                            Property = UnitProperty.StatBonusStrength
                                                        }
                                                    };
                                                })
                                            );
                                        })
                                    );
                                })
                            );
                        })    
                    );
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = false;
                    c.ActionsOnInitiator = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.UnarmedStrike;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionRemoveSelf>()
                    );
                });
            });
            var StunningFistStaggeredAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "StunningFistStaggeredAbility", bp => {
                bp.SetName(TTTContext, "Stunning Fist: Stagger");
                bp.SetDescription(TTTContext, "This ability works as Stunning Fist, but it makes the target staggered for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.");
                bp.m_Icon = Icon_StunningFistStagger;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Special;
                bp.Range = AbilityRange.Personal;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.Duration", "");
                bp.LocalizedSavingThrow = Helpers.CreateString(TTTContext, $"{bp.name}.SavingThrow", "");
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                            applyBuff.m_Buff = StunningFistStaggeredOwnerBuff.ToReference<BlueprintBuffReference>();
                            applyBuff.Permanent = true;
                            applyBuff.DurationValue = new ContextDurationValue() {
                                m_IsExtendable = true,
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            };
                        })
                    );
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = StunningFistResource;
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostIncreasingFacts = new();
                    c.ResourceCostDecreasingFacts = new();
                });
                bp.AddComponent<AbilityCasterMainWeaponCheck>(c => {
                    c.Category = new WeaponCategory[] { WeaponCategory.UnarmedStrike};
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { };
                });
                StunningFistMythicFeat.AddComponent<IncreaseSpellDC>(c => {
                    c.m_Spell = bp.ToReference<BlueprintAbilityReference>();
                    c.HalfMythicRank = true;
                    c.Value = new ContextValue();
                });
            });
            var StunningFistStaggeredFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "StunningFistStaggeredFeature", bp => {
                bp.SetName(TTTContext, "Stunning Fist: Stagger");
                bp.SetDescription(TTTContext, "This ability works as Stunning Fist, but it makes the target staggered for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.");
                bp.m_Icon = Icon_StunningFistStagger;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { StunningFistStaggeredAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent<MonkReplaceAbilityDC>(c => {
                    c.m_Ability = StunningFistStaggeredAbility.ToReference<BlueprintAbilityReference>();
                    c.m_Monk = MonkClass;
                    c.m_ScaledFist = ScaledFistArchetype;
                });
            });

            var StunningFistBlindOwnerBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "StunningFistBlindOwnerBuff", bp => {
                bp.SetName(TTTContext, "Stunning Fist: Blind");
                bp.SetDescription(TTTContext, "This ability works as Stunning Fist, but it permanently blinds the target on a failed save instead of stunning for 1 round.");
                bp.m_Icon = Icon_StunningFistBlind;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.UnarmedStrike;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionSavingThrow>(savingThrow => {
                            savingThrow.Type = SavingThrowType.Fortitude;
                            savingThrow.CustomDC = new ContextValue();
                            savingThrow.Actions = Helpers.CreateActionList(
                                Helpers.Create<ContextActionConditionalSaved>(saved => {
                                    saved.Succeed = Helpers.CreateActionList();
                                    saved.Failed = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                                            applyBuff.m_Buff = BlindnessBuff;
                                            applyBuff.Permanent = true;
                                            applyBuff.DurationValue = new ContextDurationValue() {
                                                DiceCountValue = new ContextValue(),
                                                BonusValue = new ContextValue()
                                            };
                                        }),
                                        Helpers.Create<Conditional>(conditional => {
                                            conditional.ConditionsChecker = new ConditionsChecker() {
                                                Conditions = new Condition[] {
                                                    new ContextConditionCasterHasFact(){
                                                        m_Fact = DragonFerocityBuff
                                                    }
                                                }
                                            };
                                            conditional.IfFalse = Helpers.CreateActionList();
                                            conditional.IfTrue = Helpers.CreateActionList(
                                                Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                                                    applyBuff.m_Buff = Shaken;
                                                    applyBuff.DurationValue = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        Rate = DurationRate.Rounds,
                                                        DiceType = DiceType.D4,
                                                        DiceCountValue = 1,
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.CasterProperty,
                                                            Property = UnitProperty.StatBonusStrength
                                                        }
                                                    };
                                                })
                                            );
                                        })
                                    );
                                })
                            );
                        })
                    );
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = false;
                    c.ActionsOnInitiator = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.UnarmedStrike;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionRemoveSelf>()
                    );
                });
            });
            var StunningFistBlindAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "StunningFistBlindAbility", bp => {
                bp.SetName(TTTContext, "Stunning Fist: Blind");
                bp.SetDescription(TTTContext, "This ability works as Stunning Fist, but it permanently blinds the target on a failed save instead of stunning for 1 round.");
                bp.m_Icon = Icon_StunningFistBlind;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Special;
                bp.Range = AbilityRange.Personal;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.Duration", "");
                bp.LocalizedSavingThrow = Helpers.CreateString(TTTContext, $"{bp.name}.SavingThrow", "");
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                            applyBuff.m_Buff = StunningFistBlindOwnerBuff.ToReference<BlueprintBuffReference>();
                            applyBuff.Permanent = true;
                            applyBuff.DurationValue = new ContextDurationValue() {
                                m_IsExtendable = true,
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            };
                        })
                    );
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = StunningFistResource;
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostIncreasingFacts = new();
                    c.ResourceCostDecreasingFacts = new();
                });
                bp.AddComponent<AbilityCasterMainWeaponCheck>(c => {
                    c.Category = new WeaponCategory[] { WeaponCategory.UnarmedStrike };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { };
                });
                StunningFistMythicFeat.AddComponent<IncreaseSpellDC>(c => {
                    c.m_Spell = bp.ToReference<BlueprintAbilityReference>();
                    c.HalfMythicRank = true;
                    c.Value = new ContextValue();
                });
            });
            var StunningFistBlindFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "StunningFistBlindFeature", bp => {
                bp.SetName(TTTContext, "Stunning Fist: Blind");
                bp.SetDescription(TTTContext, "This ability works as Stunning Fist, but it permanently blinds the target on a failed save instead of stunning for 1 round.");
                bp.m_Icon = Icon_StunningFistBlind;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { StunningFistBlindAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent<MonkReplaceAbilityDC>(c => {
                    c.m_Ability = StunningFistBlindAbility.ToReference<BlueprintAbilityReference>();
                    c.m_Monk = MonkClass;
                    c.m_ScaledFist = ScaledFistArchetype;
                });
            });

            var StunningFistParalyzeOwnerBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "StunningFistParalyzeOwnerBuff", bp => {
                bp.SetName(TTTContext, "Stunning Fist: Paralyze");
                bp.SetDescription(TTTContext, "This ability works as Stunning Fist, but it paralyzes the target for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.");
                bp.m_Icon = Icon_StunningFistParalyze;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.UnarmedStrike;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionSavingThrow>(savingThrow => {
                            savingThrow.Type = SavingThrowType.Fortitude;
                            savingThrow.CustomDC = new ContextValue();
                            savingThrow.Actions = Helpers.CreateActionList(
                                Helpers.Create<ContextActionConditionalSaved>(saved => {
                                    saved.Succeed = Helpers.CreateActionList();
                                    saved.Failed = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                                            applyBuff.m_Buff = Paralyzed;
                                            applyBuff.DurationValue = new ContextDurationValue() {
                                                m_IsExtendable = true,
                                                Rate = DurationRate.Rounds,
                                                DiceType = DiceType.D6,
                                                DiceCountValue = 1,
                                                BonusValue = 1
                                            };
                                        }),
                                        Helpers.Create<Conditional>(conditional => {
                                            conditional.ConditionsChecker = new ConditionsChecker() {
                                                Conditions = new Condition[] {
                                                    new ContextConditionCasterHasFact(){
                                                        m_Fact = DragonFerocityBuff
                                                    }
                                                }
                                            };
                                            conditional.IfFalse = Helpers.CreateActionList();
                                            conditional.IfTrue = Helpers.CreateActionList(
                                                Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                                                    applyBuff.m_Buff = Shaken;
                                                    applyBuff.DurationValue = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        Rate = DurationRate.Rounds,
                                                        DiceType = DiceType.D4,
                                                        DiceCountValue = 1,
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.CasterProperty,
                                                            Property = UnitProperty.StatBonusStrength
                                                        }
                                                    };
                                                })
                                            );
                                        })
                                    );
                                })
                            );
                        })
                    );
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = false;
                    c.ActionsOnInitiator = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.UnarmedStrike;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionRemoveSelf>()
                    );
                });
            });
            var StunningFistParalyzeAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "StunningFistParalyzeAbility", bp => {
                bp.SetName(TTTContext, "Stunning Fist: Paralyze");
                bp.SetDescription(TTTContext, "This ability works as Stunning Fist, but it paralyzes the target for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.");
                bp.m_Icon = Icon_StunningFistParalyze;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Special;
                bp.Range = AbilityRange.Personal;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.Duration", "");
                bp.LocalizedSavingThrow = Helpers.CreateString(TTTContext, $"{bp.name}.SavingThrow", "");
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                            applyBuff.m_Buff = StunningFistParalyzeOwnerBuff.ToReference<BlueprintBuffReference>();
                            applyBuff.Permanent = true;
                            applyBuff.DurationValue = new ContextDurationValue() {
                                m_IsExtendable = true,
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            };
                        })
                    );
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = StunningFistResource;
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostIncreasingFacts = new();
                    c.ResourceCostDecreasingFacts = new();
                });
                bp.AddComponent<AbilityCasterMainWeaponCheck>(c => {
                    c.Category = new WeaponCategory[] { WeaponCategory.UnarmedStrike };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { };
                });
                StunningFistMythicFeat.AddComponent<IncreaseSpellDC>(c => {
                    c.m_Spell = bp.ToReference<BlueprintAbilityReference>();
                    c.HalfMythicRank = true;
                    c.Value = new ContextValue();
                });
            });
            var StunningFistParalyzeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "StunningFistParalyzeFeature", bp => {
                bp.SetName(TTTContext, "Stunning Fist: Paralyze");
                bp.SetDescription(TTTContext, "This ability works as Stunning Fist, but it paralyzes the target for 1d6 + 1 rounds on a failed save instead of stunning for 1 round.");
                bp.m_Icon = Icon_StunningFistParalyze;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { StunningFistParalyzeAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent<MonkReplaceAbilityDC>(c => {
                    c.m_Ability = StunningFistParalyzeAbility.ToReference<BlueprintAbilityReference>();
                    c.m_Monk = MonkClass;
                    c.m_ScaledFist = ScaledFistArchetype;
                });
            });

            AddAbilityRestrictions(StunningFistAbility);
            AddAbilityRestrictions(StunningFistFatigueAbility);
            AddAbilityRestrictions(StunningFistSickenedAbility);
            AddAbilityRestrictions(StunningFistStaggeredAbility);
            AddAbilityRestrictions(StunningFistBlindAbility);
            AddAbilityRestrictions(StunningFistParalyzeAbility);

            void AddAbilityRestrictions(BlueprintAbility ability) {
                ability.TemporaryContext(bp => {
                    bp.RemoveComponents<AbilityCasterHasNoFacts>();
                    bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] {
                            StunningFistOwnerBuff
                        };
                    });
                    bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] {
                            StunningFistOwnerFatigueBuff
                        };
                    });
                    bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] {
                            StunningFistOwnerSickenedBuff
                        };
                    });
                    bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] {
                            StunningFistStaggeredOwnerBuff.ToReference<BlueprintUnitFactReference>()
                        };
                    });
                    bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] {
                            StunningFistBlindOwnerBuff.ToReference<BlueprintUnitFactReference>()
                        };
                    });
                    bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] {
                            StunningFistParalyzeOwnerBuff.ToReference<BlueprintUnitFactReference>()
                        };
                    });
                });
            }
        }
    }
}
