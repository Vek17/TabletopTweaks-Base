using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewActions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Reworks {
    static class Lich {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;
            static BlueprintUnitPropertyReference LichDCProperty = Resources.GetModBlueprintReference<BlueprintUnitPropertyReference>("LichDCProperty");

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Lich Rework");
                PatchTainedSneakAttack();
                PatchDecayingTouch();
            }
            static void PatchDecayingTouch() {
                if (ModSettings.Homebrew.MythicReworks.Lich.IsDisabled("DecayingTouch")) { return; }

                var DecayingTouchFeature = Resources.GetBlueprint<BlueprintFeature>("3eb8922c8a9e25048b6689322c5ae131");
                var PlantType = Resources.GetBlueprintReference<BlueprintUnitFactReference>("706e61781d692a042b35941f14bc41c5");

                DecayingTouchFeature.SetComponents();
                DecayingTouchFeature.TemporaryContext(bp => {
                    bp.AddComponent<AdditionalDiceOnAttack>(c => {
                        c.OnHit = true;
                        c.AllNaturalAndUnarmed = true;
                        c.InitiatorConditions = new ConditionsChecker();
                        c.TargetConditions = new ConditionsChecker() { 
                            Conditions = new Condition[] {
                                new ContextConditionHasFact() {
                                    m_Fact = PlantType,
                                    Not = true
                                }
                            }
                        };
                        c.Value = new ContextDiceValue() {
                            DiceType = DiceType.D6,
                            DiceCountValue = 1,
                            BonusValue = new ContextValue() {
                                ValueType = ContextValueType.Rank
                            },
                        };
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Unholy
                        };
                    });
                    bp.AddComponent<AdditionalDiceOnAttack>(c => {
                        c.OnHit = true;
                        c.AllNaturalAndUnarmed = true;
                        c.InitiatorConditions = new ConditionsChecker();
                        c.TargetConditions = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                new ContextConditionHasFact() {
                                    m_Fact = PlantType,
                                }
                            }
                        };
                        c.Value = new ContextDiceValue() {
                            DiceType = DiceType.D6,
                            DiceCountValue = 2,
                            BonusValue = new ContextValue() {
                                ValueType = ContextValueType.Rank
                            },
                        };
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Unholy
                        };
                    });
                    bp.AddComponent<AdditionalDiceOnAttack>(c => {
                        c.OnHit = true;
                        c.CheckWeaponRangeType = true;
                        c.RangeType = WeaponRangeType.MeleeTouch;
                        c.InitiatorConditions = new ConditionsChecker();
                        c.TargetConditions = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                new ContextConditionHasFact() {
                                    m_Fact = PlantType,
                                    Not = true
                                }
                            }
                        };
                        c.Value = new ContextDiceValue() {
                            DiceType = DiceType.D6,
                            DiceCountValue = 1,
                            BonusValue = new ContextValue() {
                                ValueType = ContextValueType.Rank
                            },
                        };
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Unholy
                        };
                    });
                    bp.AddComponent<AdditionalDiceOnAttack>(c => {
                        c.OnHit = true;
                        c.CheckWeaponRangeType = true;
                        c.RangeType = WeaponRangeType.MeleeTouch;
                        c.InitiatorConditions = new ConditionsChecker();
                        c.TargetConditions = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                new ContextConditionHasFact() {
                                    m_Fact = PlantType,
                                }
                            }
                        };
                        c.Value = new ContextDiceValue() {
                            DiceType = DiceType.D6,
                            DiceCountValue = 2,
                            BonusValue = new ContextValue() {
                                ValueType = ContextValueType.Rank
                            },
                        };
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Unholy
                        };
                    });
                    bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                        c.OnlyHit = true;
                        c.AllNaturalAndUnarmed = true;
                        c.Action = Helpers.CreateActionList(
                            Helpers.Create<ContextActionDealDamage>(a => {
                                a.m_Type = ContextActionDealDamage.Type.AbilityDamage;
                                a.AbilityType = StatType.Strength;
                                a.DamageType = new DamageTypeDescription();
                                a.Duration = new ContextDurationValue() {
                                    DiceCountValue = 0,
                                    BonusValue = 0
                                };
                                a.Value = new ContextDiceValue() {
                                    DiceCountValue = 0,
                                    BonusValue = 1
                                };
                            })
                        );
                    });
                    bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                        c.OnlyHit = true;
                        c.CheckWeaponRangeType = true;
                        c.RangeType = WeaponRangeType.MeleeTouch;
                        c.Action = Helpers.CreateActionList(
                            Helpers.Create<ContextActionDealDamage>(a => {
                                a.m_Type = ContextActionDealDamage.Type.AbilityDamage;
                                a.AbilityType = StatType.Strength;
                                a.DamageType = new DamageTypeDescription();
                                a.Duration = new ContextDurationValue() {
                                    DiceCountValue = 0,
                                    BonusValue = 0
                                };
                                a.Value = new ContextDiceValue() {
                                    DiceCountValue = 0,
                                    BonusValue = 1
                                };
                            })
                        );
                    });
                    bp.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    });
                });
                
                Main.LogPatch(DecayingTouchFeature);
            }
            static void PatchTainedSneakAttack() {
                if (ModSettings.Homebrew.MythicReworks.Lich.IsDisabled("TainedSneakAttack")) { return; }

                var TaintedSneakAttackFeature = Resources.GetBlueprint<BlueprintFeature>("e6ce101a94ac9034b8b55c546e74b9dd");
                var TaintedSneakAttackBuff = Resources.GetBlueprint<BlueprintBuff>("7860e92789511a24dba5906ac8d65f90");
                var SneakAttack = Resources.GetBlueprintReference<BlueprintUnitFactReference>("9b9eac6709e1c084cb18c3a366e0ec87");

                TaintedSneakAttackFeature.TemporaryContext(bp => {
                    bp.SetDescription("Whenever Lich lands a successful sneak attack, the enemy must pass Fortitude saving throw " +
                        "(DC = 10 + 1/2 character level + twice your mythic rank) or become tainted. The tainted creature is vulnerable to " +
                        "all weapon and elemental damage, as well as suffers a –2 penalty on all attack rolls and weapon damage " +
                        "rolls, until the end of the combat.\n" +
                        "Additionally, Lich's sneak attack damage is increased by 1d6.");
                    bp.SetComponents();
                    bp.AddComponent<AddInitiatorAttackRollTrigger>(c => {
                        c.OnlyHit = true;
                        c.SneakAttack = true;
                        c.Action = Helpers.CreateActionList(
                            Helpers.Create<ContextActionSavingThrow>(save => {
                                save.Type = SavingThrowType.Fortitude;
                                save.HasCustomDC = true;
                                save.CustomDC = new ContextValue() {
                                    ValueType = ContextValueType.CasterCustomProperty,
                                    m_CustomProperty = LichDCProperty
                                };
                                save.Actions = Helpers.CreateActionList(
                                    Helpers.Create<ContextActionConditionalSaved>(condition => {
                                        condition.Succeed = Helpers.CreateActionList();
                                        condition.Failed = Helpers.CreateActionList(
                                            Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                                                applyBuff.m_Buff = TaintedSneakAttackBuff.ToReference<BlueprintBuffReference>();
                                                applyBuff.Permanent = true;
                                                applyBuff.DurationValue = new ContextDurationValue() {
                                                    DiceCountValue = new ContextValue(),
                                                    BonusValue = new ContextValue()
                                                };
                                            })
                                        );
                                    })
                                );
                            })
                        );
                    });
                    bp.AddComponent<AddFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] { SneakAttack };
                    });
                });
                TaintedSneakAttackBuff.TemporaryContext(bp => {
                    bp.m_Description = TaintedSneakAttackFeature.m_Description;
                    bp.RemoveComponents<AddDamageTypeVulnerability>();
                    bp.AddComponent<AddDamageTypeVulnerability>(c => {
                        c.PhyscicalForm = true;
                        c.FormType = PhysicalDamageForm.Bludgeoning;
                    });
                    bp.AddComponent<AddDamageTypeVulnerability>(c => {
                        c.PhyscicalForm = true;
                        c.FormType = PhysicalDamageForm.Slashing;
                    });
                    bp.AddComponent<AddDamageTypeVulnerability>(c => {
                        c.PhyscicalForm = true;
                        c.FormType = PhysicalDamageForm.Piercing;
                    });
                });

                Main.LogPatch(TaintedSneakAttackFeature);
                Main.LogPatch(TaintedSneakAttackBuff);
            }
        }
    }
}
