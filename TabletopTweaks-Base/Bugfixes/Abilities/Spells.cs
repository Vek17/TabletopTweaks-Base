using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.MetamagicExtention;

namespace TabletopTweaks.Base.Bugfixes.Abilities {
    class Spells {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Spells");
                //Aeon Spells
                PatchAbsoluteOrder();
                PatchBlackHole();
                PatchCrystalMind();
                PatchEdictOfImpenetrableFortress();
                PatchEdictOfInvulnerability();
                PatchEdictOfNonresistance();
                PatchEdictOfPerseverance();
                PatchEdictOfPredetermination();
                PatchEdictOfRetaliation();
                PatchEmbodimentOfOrder();
                PatchEqualForce();
                PatchFreezingNothingness();
                PatchPerfectForm();
                PatchRelativity();
                PatchStarlight();
                PatchSupernova();
                PatchUncertanityPrinciple();
                PatchZeroState();
                PatchZoneOfPredetermination();
                //Angel Spells
                PatchEyeOfTheSun();
                PatchSunForm();
                PatchSunMarked();
                //Azata Spells
                PatchBelieveInYourself();
                PatchBurstOfSonicEnergy();
                PatchFieldOfFlowers();
                PatchFriendlyHug();
                PatchJoyOfLife();
                PatchNaturesGrasp();
                PatchOdeToMiraculousMagic();
                PatchProtectionOfNature();
                PatchRepulsiveNature();
                PatchSongsOfSteel();
                PatchSuddenSquall();
                PatchUnbreakableBond();
                PatchWaterPush();
                PatchWaterTorrent();
                PatchWindsOfFall();
                //Demon Spells
                PatchAbyssalStorm();
                //Lich Spells
                PatchCorruptMagic();
                PatchPowerFromDeath();
                PatchVampiricBlade();
                //TricksterSpells
                PatchMicroscopicProportions();
                //General Spells
                PatchAcidMaw();
                PatchAnimalGrowth();
                PatchBestowCurseGreater();
                PatchBreakEnchantment();
                PatchChainLightning();
                PatchCommand();
                PatchCommandGreater();
                PatchCrusadersEdge();
                PatchDeathWard();
                PatchDispelMagicGreater();
                PatchEcholocation();
                PatchFieryBody();
                PatchFirebrand();
                PatchFlamestrike();
                PatchFreedomOfMovement();
                PatchGeniekind();
                PatchHellfireRay();
                PatchHurricaneBow();
                PatchIcyBody();
                PatchIronBody();
                PatchLeadBlades();
                PatchLegendaryProportions();
                PatchLifeBubble();
                PatchMagicalVestment();
                PatchMagicWeaponGreater();
                PatchMindFog();
                PatchProtectionFromAlignmentGreater();
                PatchRemoveFear();
                PatchRemoveSickness();
                PatchShadowEvocation();
                PatchShadowEvocationGreater();
                PatchUnbreakableHeart();
                PatchWintersGrasp();
                PatchWrackingRay();
                PatchFromSpellFlags();
            }
            //Aeon Spells
            static void PatchAbsoluteOrder() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("AbsoluteOrder")) { return; }

                var AbsoluteOrder = BlueprintTools.GetBlueprint<BlueprintAbility>("b1723a0239d428243a1be2299696eb85");

                AbsoluteOrder.AbilityAndVariants()
                    .ForEach(ability => {
                        var descriptors = ability.GetComponent<SpellDescriptorComponent>();
                        if (descriptors is null) {
                            ability.AddComponent<SpellDescriptorComponent>();
                            descriptors = ability.GetComponent<SpellDescriptorComponent>();
                        }
                        descriptors.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion;
                        TTTContext.Logger.LogPatch(ability);
                    });
            }
            static void PatchBlackHole() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("BlackHole")) { return; }

                var BlackHole = BlueprintTools.GetBlueprint<BlueprintAbility>("ea036c023f074c6c964d858607d123b3");

                BlackHole.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/mythic rank");
                    bp.SetLocalizedSavingThrow(TTTContext, "Reflex negates/Fortitude partial");
                });

                TTTContext.Logger.LogPatch(BlackHole);
            }
            static void PatchCrystalMind() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("CrystalMind")) { return; }

                var CrystalMind = BlueprintTools.GetBlueprint<BlueprintAbility>("4733d8dd549ff544395f1684ec73c392");

                CrystalMind.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/level");
                });

                TTTContext.Logger.LogPatch(CrystalMind);
            }
            static void PatchEdictOfImpenetrableFortress() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("EdictOfImpenetrableFortress")) { return; }

                var EdictOfImpenetrableFortress = BlueprintTools.GetBlueprint<BlueprintAbility>("d7741c08ccf699e4a8a8f8ab2ed345f8");

                EdictOfImpenetrableFortress.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/5 levels");
                });

                TTTContext.Logger.LogPatch(EdictOfImpenetrableFortress);
            }
            static void PatchEdictOfInvulnerability() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("EdictOfInvulnerability")) { return; }

                var EdictOfInvulnerability = BlueprintTools.GetBlueprint<BlueprintAbility>("6d21deddd7712fd409c94d248b75643d");

                EdictOfInvulnerability.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/5 levels");
                });

                TTTContext.Logger.LogPatch(EdictOfInvulnerability);
            }
            static void PatchEdictOfNonresistance() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("EdictOfNonresistance")) { return; }

                var EdictOfNonresistance = BlueprintTools.GetBlueprint<BlueprintAbility>("dfe3594aed8907248958a25614aa5281");

                EdictOfNonresistance.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/level");
                    bp.SetLocalizedSavingThrow(TTTContext, "Will negates");
                    bp.RemoveComponents<SpellDescriptorComponent>();
                    bp.AddComponent<SpellDescriptorComponent>(c => {
                        c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion;
                    });
                });

                TTTContext.Logger.LogPatch(EdictOfNonresistance);
            }
            static void PatchEdictOfPerseverance() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("EdictOfPerseverance")) { return; }

                var EdictOfPerseverance = BlueprintTools.GetBlueprint<BlueprintAbility>("b7bc99f6e3592a3499815387e1d721e2");
                var EdictOfPerseveranceBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("d40e22faaf0bcae42ada5eae749b58e8");

                EdictOfPerseverance.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/5 levels");
                });
                QuickFixTools.ReplaceSuppression(EdictOfPerseveranceBuff, TTTContext, true);

                TTTContext.Logger.LogPatch(EdictOfPerseverance);
            }
            static void PatchEdictOfPredetermination() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("EdictOfPredetermination")) { return; }

                var EdictOfPredetermination = BlueprintTools.GetBlueprint<BlueprintAbility>("3f205f55a6e7759449772feb34edc378");

                EdictOfPredetermination.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/4 levels");
                });

                TTTContext.Logger.LogPatch(EdictOfPredetermination);
            }
            static void PatchEdictOfRetaliation() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("EdictOfRetaliation")) { return; }

                var EdictOfRetaliation = BlueprintTools.GetBlueprint<BlueprintAbility>("d57fcfdfcffa2d346985648cb77390fb");

                EdictOfRetaliation.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/4 levels");
                });

                TTTContext.Logger.LogPatch(EdictOfRetaliation);
            }
            static void PatchEmbodimentOfOrder() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("EmbodimentOfOrder")) { return; }

                var EmbodimentOfOrder = BlueprintTools.GetBlueprint<BlueprintAbility>("5ab2e32dd25724a488878c2a52c65ace");

                EmbodimentOfOrder.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "10 minutes/level");
                });

                TTTContext.Logger.LogPatch(EmbodimentOfOrder);
            }
            static void PatchEqualForce() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("EqualForce")) { return; }

                var EqualForce = BlueprintTools.GetBlueprint<BlueprintAbility>("47c80ed8e725ede4d91f06eddcc3e75a");

                EqualForce.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "10 minutes/level");
                });

                TTTContext.Logger.LogPatch(EqualForce);
            }
            static void PatchFreezingNothingness() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("FreezingNothingness")) { return; }

                var FreezingNothingness = BlueprintTools.GetBlueprint<BlueprintAbility>("89bc94bd06dcf5847bb9e4d6ba1b9767");
                var FreezingNothingnessEntangledBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("6eaeaadabfc5be44480a66da6c0323df");
                var FreezingNothingnessParalyzedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2975be18c3b5da1408caeadbc81caab4");

                FreezingNothingness.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "10 minutes/level");
                    bp.SpellResistance = false;
                });

                FreezingNothingnessEntangledBuff.TemporaryContext(bp => {
                    bp.FlattenAllActions().OfType<ContextActionSkillCheck>().First().TemporaryContext(c => {
                        c.m_ConditionalDCIncrease.ForEach(i => i.Condition.Conditions = new Condition[0]);
                    });
                });
                FreezingNothingnessParalyzedBuff.TemporaryContext(bp => {
                    bp.FlattenAllActions().OfType<ContextActionSkillCheck>().First().TemporaryContext(c => {
                        c.m_ConditionalDCIncrease.ForEach(i => i.Condition.Conditions = new Condition[0]);
                        c.Failure = Helpers.CreateActionList(
                            new ContextActionDealDamage() {
                                DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Cold
                                },
                                Duration = new ContextDurationValue() {
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                },
                                Value = new ContextDiceValue() {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = 2,
                                    BonusValue = 0
                                },
                                HalfIfSaved = true
                            },
                            new ContextActionDealDamage() {
                                DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Divine
                                },
                                Duration = new ContextDurationValue() {
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                },
                                Value = new ContextDiceValue() {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = 2,
                                    BonusValue = 0
                                },
                                HalfIfSaved = true
                            }
                        );
                    });
                });

                TTTContext.Logger.LogPatch(FreezingNothingness);
            }
            static void PatchPerfectForm() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("PerfectForm")) { return; }

                var PerfectForm = BlueprintTools.GetBlueprint<BlueprintAbility>("91d04f9180e94065ac768959323d2002");
                var perfectFormBuffs = new BlueprintBuffReference[] {
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("ccd363424d954c668a81b0024012a66a"),    // PerfectFormEqualToCharismaBuff
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("774567a0bcf54a83807f7387d5dd9c23"),    // PerfectFormEqualToConstitutionBuff
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("23b87498fac14465bc9c22cc3366e6e7"),    // PerfectFormEqualToDexterityBuff
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3bc3d8660ddc467aabea43b070fcd10b"),    // PerfectFormEqualToIntelligenceBuff
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("149e7d34927146a8804404087bf9703f"),    // PerfectFormEqualToStrengthBuff
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("06785b5665264ad1b257fa3e724ed68f")     // PerfectFormEqualToWisdomBuff
  
                };
                PerfectForm.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/level");
                    bp.AvailableMetamagic |= Metamagic.Extend;
                    bp.GetComponent<AbilityEffectRunAction>()
                        .TemporaryContext(c => {
                            c.Actions = Helpers.CreateActionList(
                                CreateRemoveBuff(perfectFormBuffs)
                                    .Concat(c.Actions.Actions)
                                    .ToArray()
                            );
                        });
                });

                IEnumerable<GameAction> CreateRemoveBuff(BlueprintBuffReference[] buffs) {
                    foreach (var buff in buffs) {
                        var removeBuff = new ContextActionRemoveBuff() {
                            m_Buff = buff,
                        };
                        var conditional = new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                new ContextConditionHasBuff() {
                                    m_Buff = buff
                                }
                            }
                            },
                            IfTrue = Helpers.CreateActionList(removeBuff),
                            IfFalse = Helpers.CreateActionList()
                        };
                        yield return conditional;
                    }
                }
                TTTContext.Logger.LogPatch(PerfectForm);
            }
            static void PatchRelativity() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("Relativity")) { return; }

                var Relativity = BlueprintTools.GetBlueprint<BlueprintAbility>("5d28f75db5d3cc141ba5783a1a139f66");

                Relativity.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/level");
                    bp.SetLocalizedSavingThrow(TTTContext, "Will negates");
                });

                TTTContext.Logger.LogPatch(Relativity);
            }
            static void PatchSupernova() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("Supernova")) { return; }
                var Supernova = BlueprintTools.GetBlueprint<BlueprintAbility>("1325e698f4a3f224b880e3b83a551228");
                var SupernovaArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("165a01a3597c0bf44a8b333ac6dd631a");
                var BlindnessBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("187f88d96a0ef464280706b63635f2af");

                Supernova.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/level");
                    bp.SetLocalizedSavingThrow(TTTContext, "Fortitude partial/Relex half");
                    bp.AvailableMetamagic |= Metamagic.Empower | Metamagic.Maximize | Metamagic.Bolstered;
                });
                SupernovaArea.TemporaryContext(bp => {
                    bp.RemoveComponents<AbilityAreaEffectRunAction>();
                    bp.AddComponent<AbilityAreaEffectRunAction>(c => {
                        c.UnitEnter = Helpers.CreateActionList(CreateBlindnessSave(), CreateDamageSave());
                        c.UnitExit = Helpers.CreateActionList();
                        c.UnitMove = Helpers.CreateActionList();
                        c.Round = Helpers.CreateActionList(CreateDamageSave());
                    });
                });

                TTTContext.Logger.LogPatch("Patched", SupernovaArea);

                ContextActionSavingThrow CreateBlindnessSave() {
                    return Helpers.Create<ContextActionSavingThrow>(save => {
                        save.Type = SavingThrowType.Fortitude;
                        save.m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0];
                        save.CustomDC = new ContextValue();
                        save.Actions = Helpers.CreateActionList(
                            Helpers.Create<ContextActionConditionalSaved>(c => {
                                c.Succeed = Helpers.CreateActionList();
                                c.Failed = Helpers.CreateActionList(
                                    Helpers.Create<ContextActionApplyBuff>(a => {
                                        a.m_Buff = BlindnessBuff;
                                        a.DurationValue = new ContextDurationValue() {
                                            m_IsExtendable = true,
                                            DiceCountValue = new ContextValue(),
                                            BonusValue = new ContextValue() {
                                                ValueType = ContextValueType.Rank
                                            }
                                        };
                                        a.AsChild = true;
                                    })
                                );
                            })
                        );
                    });
                }
                ContextActionSavingThrow CreateDamageSave() {
                    return Helpers.Create<ContextActionSavingThrow>(save => {
                        save.Type = SavingThrowType.Reflex;
                        save.m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0];
                        save.CustomDC = new ContextValue();
                        save.Actions = Helpers.CreateActionList(
                            Helpers.Create<ContextActionDealDamage>(c => {
                                c.DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Common = new DamageTypeDescription.CommomData(),
                                    Physical = new DamageTypeDescription.PhysicalData(),
                                    Energy = DamageEnergyType.Fire
                                };
                                c.Duration = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                };
                                c.Value = new ContextDiceValue() {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = 2,
                                    BonusValue = new ContextValue() {
                                        ValueType = ContextValueType.Rank
                                    }
                                };
                                c.IsAoE = true;
                                c.HalfIfSaved = true;
                                //c.WriteResultToSharedValue = true;
                                //c.WriteRawResultToSharedValue = true;
                            }),
                            Helpers.Create<ContextActionDealDamage>(c => {
                                c.DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Common = new DamageTypeDescription.CommomData(),
                                    Physical = new DamageTypeDescription.PhysicalData(),
                                    Energy = DamageEnergyType.Divine
                                };
                                c.Duration = new ContextDurationValue() {
                                    m_IsExtendable = true,
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                };
                                c.Value = new ContextDiceValue() {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = 2,
                                    BonusValue = new ContextValue() {
                                        ValueType = ContextValueType.Rank
                                    }
                                };
                                c.IsAoE = true;
                                c.HalfIfSaved = true;
                                //c.ReadPreRolledFromSharedValue = true;
                            })
                        );
                    });
                }
            }
            static void PatchStarlight() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("Starlight")) { return; }

                var StarlightAllyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f4ead47adc2ca2744a00efd4e088ecb2");

                StarlightAllyBuff.GetComponent<AddConcealment>().Descriptor = ConcealmentDescriptor.InitiatorIsBlind;
                TTTContext.Logger.LogPatch("Patched", StarlightAllyBuff);
            }
            static void PatchUncertanityPrinciple() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("UncertanityPrinciple")) { return; }

                var UncertanityPrinciple = BlueprintTools.GetBlueprint<BlueprintAbility>("4b7e6e992a4862a45bcfc9ba95bfc727");

                UncertanityPrinciple.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/level");
                });

                TTTContext.Logger.LogPatch(UncertanityPrinciple);
            }
            static void PatchZeroState() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("ZeroState")) { return; }

                var ZeroState = BlueprintTools.GetBlueprint<BlueprintAbility>("c6195ff24255d3f46a26323de9f1187a");

                ZeroState.FlattenAllActions()
                    .OfType<ContextActionDispelMagic>()
                    .ForEach(a => {
                        a.OneRollForAll = true;
                    });
                TTTContext.Logger.LogPatch("Patched", ZeroState);
            }
            static void PatchZoneOfPredetermination() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("ZoneOfPredetermination")) { return; }

                var ZoneOfPredetermination = BlueprintTools.GetBlueprint<BlueprintAbility>("756f1d07f9ae29448888ecf016fa40a7");

                ZoneOfPredetermination.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "1 round/4 levels");
                });

                TTTContext.Logger.LogPatch(ZoneOfPredetermination);
            }
            //Angel Spells
            static void PatchEyeOfTheSun() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("EyeOfTheSun")) { return; }

                var AngelEyeOfTheSun = BlueprintTools.GetBlueprint<BlueprintAbility>("a948e10ecf1fa674dbae5eaae7f25a7f");
                AngelEyeOfTheSun.FlattenAllActions().OfType<ContextActionDealDamage>().ForEach(a => {
                    if (a.WriteResultToSharedValue) {
                        a.WriteRawResultToSharedValue = true;
                    } else {
                        a.Half = true;
                        a.AlreadyHalved = false;
                        a.ReadPreRolledFromSharedValue = false;
                    }
                });
                TTTContext.Logger.LogPatch("Patched", AngelEyeOfTheSun);
            }
            static void PatchSunForm() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("SunForm")) { return; }

                var AngelSunFormRay = BlueprintTools.GetBlueprint<BlueprintAbility>("d0d8811bf5a8e2942b6b7d77d9691eb9");
                AngelSunFormRay.FlattenAllActions().OfType<ContextActionDealDamage>().ForEach(a => {
                    if (a.WriteResultToSharedValue) {
                        a.WriteRawResultToSharedValue = true;
                        a.Half = true;
                    } else {
                        a.Half = true;
                        a.AlreadyHalved = false;
                        a.ReadPreRolledFromSharedValue = false;
                    }
                    a.Value.DiceType = DiceType.D6;
                    a.Value.DiceCountValue = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    a.Value.BonusValue = new ContextValue();
                });
                TTTContext.Logger.LogPatch("Patched", AngelSunFormRay);
            }
            static void PatchSunMarked() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("SunMarked")) { return; }

                var AngelSunMarkedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("35407948549e61f4c80e6b59633d82b0");

                AngelSunMarkedBuff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                AngelSunMarkedBuff.RemoveComponents<AdditionalDiceOnAttack>();
                AngelSunMarkedBuff.AddComponent<AddAdditionalWeaponDamage>(c => {
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.D6,
                        DiceCountValue = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageDice,
                        },
                        BonusValue = 0
                    };
                    c.DamageType = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Holy
                    };
                });
                TTTContext.Logger.LogPatch("Patched", AngelSunMarkedBuff);
            }
            //Azata Spells
            static void PatchBelieveInYourself() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("BelieveInYourself")) { return; }

                var BelieveInYourself = BlueprintTools.GetBlueprint<BlueprintAbility>("3ed3cef7c267cb847bfd44ed4708b726");
                var BelieveInYourselfVariants = BelieveInYourself
                    .GetComponent<AbilityVariants>()
                    .Variants;
                foreach (BlueprintAbility variant in BelieveInYourselfVariants) {
                    variant.SetDescription(BelieveInYourself.m_Description);
                    variant.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .Select(action => action.Buff)
                        .ForEach(buff => {
                            buff.GetComponent<ContextRankConfig>().TemporaryContext(c => {
                                c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                                c.m_Progression = ContextRankProgression.DivStep;
                                c.m_StepLevel = 4;
                                TTTContext.Logger.LogPatch(buff);
                            });
                        });
                    variant.FlattenAllActions()
                       .OfType<ContextActionApplyBuff>()
                       .Select(action => action.Buff)
                       .ForEach(buff => {
                           variant.SetName(buff.m_DisplayName);
                       });
                    variant.GetComponent<ContextRankConfig>().TemporaryContext(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.AsIs;
                        TTTContext.Logger.LogPatch(variant);
                    });

                }
            }
            static void PatchBurstOfSonicEnergy() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("BreakEnchantment")) { return; }

                var BurstOfSonicEnergy = BlueprintTools.GetBlueprint<BlueprintAbility>("b5a2d0e400dd38e428c953f8a2be5f0b");
                var BurstOfSonicEnergy10Feet = BlueprintTools.GetBlueprint<BlueprintAbility>("980554934d1aa354bbb08f4b150bd9da");
                var BurstOfSonicEnergyAdjacent = BlueprintTools.GetBlueprint<BlueprintAbility>("65010ad20b1f57a4f86cd09115728831");
                var Mythic1lvlAzata_ElysiumBolt00 = BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>("f00eb27234fbc39448b142f1257c8886");

                BurstOfSonicEnergy.TemporaryContext(bp => {
                    bp.AvailableMetamagic |= Metamagic.Empower
                        | Metamagic.Maximize
                        | Metamagic.Bolstered
                        | Metamagic.Selective
                        | Metamagic.CompletelyNormal
                        | (Metamagic)CustomMetamagic.ElementalAcid
                        | (Metamagic)CustomMetamagic.ElementalCold
                        | (Metamagic)CustomMetamagic.ElementalElectricity
                        | (Metamagic)CustomMetamagic.ElementalFire;
                    bp.GetComponent<AbilityEffectRunAction>().TemporaryContext(c => {
                        c.Actions = Helpers.CreateActionList(
                            new Conditional() {
                                ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionDistanceToTarget(){
                                            DistanceGreater = 5.Feet(),
                                            Not = true
                                        }
                                    }
                                },
                                IfTrue = Helpers.CreateActionList(
                                    new ContextActionDealDamage() {
                                        DamageType = new DamageTypeDescription() {
                                            Type = DamageType.Energy,
                                            Energy = DamageEnergyType.Sonic
                                        },
                                        Duration = new ContextDurationValue() {
                                            DiceCountValue = new ContextValue(),
                                            BonusValue = new ContextValue()
                                        },
                                        Value = new ContextDiceValue() {
                                            DiceType = DiceType.D6,
                                            DiceCountValue = new ContextValue() {
                                                ValueType = ContextValueType.Rank,
                                                ValueRank = AbilityRankType.DamageDice
                                            },
                                            BonusValue = 0
                                        }
                                    }
                                ),
                                IfFalse = Helpers.CreateActionList()
                            },
                            new ContextActionDealDamage() {
                                DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Sonic
                                },
                                Duration = new ContextDurationValue() {
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                },
                                Value = new ContextDiceValue() {
                                    DiceCountValue = 0,
                                    BonusValue = new ContextValue() {
                                        ValueType = ContextValueType.Rank,
                                        ValueRank = AbilityRankType.DamageBonus
                                    }
                                }
                            }
                        );
                    });
                    bp.AddComponent<AbilityTargetsAround>(c => {
                        c.m_Radius = 10.Feet();
                        c.m_TargetType = TargetType.Any;
                        c.m_Condition = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                new ContextConditionIsCaster(){
                                    Not = true
                                }
                            }
                        };
                    });
                    bp.AddContextRankConfig(c => {
                        c.m_Type = AbilityRankType.DamageDice;
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.Div2;
                    });
                    bp.AddContextRankConfig(c => {
                        c.m_Type = AbilityRankType.DamageBonus;
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.Div2;
                    });
                    bp.AddComponent<AbilityDeliverProjectile>(c => {
                        c.m_Projectiles = new BlueprintProjectileReference[] {
                            Mythic1lvlAzata_ElysiumBolt00
                        };
                        c.m_Weapon = new BlueprintItemWeaponReference();
                        c.m_ControlledProjectileHolderBuff = new BlueprintBuffReference();
                    });
                });
                TTTContext.Logger.LogPatch(BurstOfSonicEnergy);
            }
            static void PatchFieldOfFlowers() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("FieldOfFlowers")) { return; }

                var FieldOfFlowers = BlueprintTools.GetBlueprint<BlueprintAbility>("b2d9ad1cc95d718459c09499a2e9dbf9");
                var FieldOfFlowersArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("d8f51712fce75fe42aa4cc6d3d1cf7d0");
                var FieldOfFlowersBuffDistracted = BlueprintTools.GetBlueprint<BlueprintBuff>("cfe72018943bcd446803fc8a825bb391");
                var FieldOfFlowersBuffImmunity = BlueprintTools.GetBlueprint<BlueprintBuff>("2edcd52dc77e89145b2817d23606c1b8");
                var RepulsiveNatureNauseatedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c9b528fbbd8f3b84fb068bcf9b2c0ebb");
                var RepulsiveNatureBuffImmunity = BlueprintTools.GetBlueprint<BlueprintBuff>("7401fdfea49b9874db8a72579aec7c1f");

                FieldOfFlowersArea.TemporaryContext(bp => {
                    bp.RemoveComponents<AddFactContextActions>();
                    bp.FlattenAllActions()
                        .OfType<Conditional>()
                        .Where(c => c.ConditionsChecker.Conditions.Any(condition => condition is ContextConditionIsEnemy))
                        .ForEach(c => {
                            c.ConditionsChecker.Conditions = c.ConditionsChecker.Conditions.AppendToArray(
                                new ContextConditionHasBuff() {
                                    m_Buff = FieldOfFlowersBuffImmunity.ToReference<BlueprintBuffReference>(),
                                    Not = true
                                }
                            );
                        });
                    bp.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .Where(action => action.Buff == RepulsiveNatureNauseatedBuff)
                        .ForEach(action => {
                            action.m_Buff = FieldOfFlowersBuffDistracted.ToReference<BlueprintBuffReference>();
                        });
                    bp.FlattenAllActions()
                        .OfType<Conditional>()
                        .SelectMany(c => c.ConditionsChecker.Conditions)
                        .OfType<ContextConditionHasBuff>()
                        .Where(condition => condition.Buff == RepulsiveNatureNauseatedBuff)
                        .ForEach(condition => {
                            condition.m_Buff = FieldOfFlowersBuffDistracted.ToReference<BlueprintBuffReference>();
                        });
                    bp.FlattenAllActions()
                        .OfType<ContextActionRemoveBuff>()
                        .Where(action => action.Buff == RepulsiveNatureNauseatedBuff)
                        .ForEach(action => {
                            action.m_Buff = FieldOfFlowersBuffDistracted.ToReference<BlueprintBuffReference>();
                        });

                    bp.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .Where(action => action.Buff == RepulsiveNatureBuffImmunity)
                        .ForEach(action => {
                            action.m_Buff = FieldOfFlowersBuffImmunity.ToReference<BlueprintBuffReference>();
                        });
                    bp.FlattenAllActions()
                        .OfType<Conditional>()
                        .SelectMany(c => c.ConditionsChecker.Conditions)
                        .OfType<ContextConditionHasBuff>()
                        .Where(condition => condition.Buff == RepulsiveNatureBuffImmunity)
                        .ForEach(condition => {
                            condition.m_Buff = FieldOfFlowersBuffImmunity.ToReference<BlueprintBuffReference>();
                        });
                    bp.FlattenAllActions()
                        .OfType<ContextActionRemoveBuff>()
                        .Where(action => action.Buff == RepulsiveNatureBuffImmunity)
                        .ForEach(action => {
                            action.m_Buff = FieldOfFlowersBuffImmunity.ToReference<BlueprintBuffReference>();
                        });
                });
                FieldOfFlowers.GetComponent<SpellDescriptorComponent>().TemporaryContext(c => {
                    c.Descriptor = SpellDescriptor.Ground;
                });
                TTTContext.Logger.LogPatch(FieldOfFlowers);
                TTTContext.Logger.LogPatch(FieldOfFlowersArea);
            }
            static void PatchFriendlyHug() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("FriendlyHug")) { return; }

                var FriendlyHugBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("3906504d59286b640856b3da63f389a8");
                FriendlyHugBuff.AddComponent<SpellImmunityToSpellDescriptor>(c => {
                    c.Descriptor = SpellDescriptor.MindAffecting;
                    c.m_CasterIgnoreImmunityFact = new BlueprintUnitFactReference();
                });

                TTTContext.Logger.LogPatch(FriendlyHugBuff);
            }
            static void PatchJoyOfLife() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("JoyOfLife")) { return; }

                var JoyOfLifeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c3adfe620be4a0749904d8942aaabf38");
                JoyOfLifeBuff.TemporaryContext(bp => {
                    bp.AddComponent<ChangeOutgoingDamageType>(c => {
                        c.Type = new DamageTypeDescription() {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Holy
                        };
                    });
                });

                TTTContext.Logger.LogPatch(JoyOfLifeBuff);
            }
            static void PatchNaturesGrasp() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("NaturesGrasp")) { return; }

                var NaturesGrasp = BlueprintTools.GetBlueprint<BlueprintAbility>("2b9db9808a1aad74b94f651c5732fd3c");
                var NaturesGraspBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b506053d4279ee347b56095c9714c008");

                NaturesGraspBuff.TemporaryContext(bp => {
                    bp.FlattenAllActions()
                        .OfType<ContextActionDealDamage>()
                        .ForEach(a => {
                            a.Value = new ContextDiceValue() {
                                DiceType = DiceType.D6,
                                DiceCountValue = 1,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank,
                                    ValueRank = AbilityRankType.DamageDice
                                }
                            };
                        });
                });
                TTTContext.Logger.LogPatch(NaturesGraspBuff);
            }
            static void PatchOdeToMiraculousMagic() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("OdeToMiraculousMagic")) { return; }

                var OdeToMiraculousMagic = BlueprintTools.GetBlueprint<BlueprintAbility>("f1a0dd9c0b6f9654fb025875dc4b905d");
                var OdeToMiraculousMagicBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("f6ef0e25745114d46bf16fd5a1d93cc9");

                OdeToMiraculousMagic.TemporaryContext(bp => {
                    bp.GetComponent<ContextRankConfig>().TemporaryContext(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.AsIs;
                    });
                    bp.GetComponent<AbilityEffectRunAction>().TemporaryContext(c => {
                        c.Actions = Helpers.CreateActionList(
                            new ContextActionApplyBuff() {
                                m_Buff = OdeToMiraculousMagicBuff,
                                DurationValue = new ContextDurationValue() {
                                    Rate = DurationRate.TenMinutes,
                                    DiceCountValue = 0,
                                    BonusValue = new ContextValue() {
                                        ValueType = ContextValueType.Rank
                                    }
                                }
                            }
                        );
                    });
                    bp.AddComponent<AbilityTargetsAround>(c => {
                        c.m_Radius = 60.Feet();
                        c.m_TargetType = TargetType.Ally;
                        c.m_Condition = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                new ContextConditionIsPartyMember()
                            }
                        };
                    });
                });

                TTTContext.Logger.LogPatch(OdeToMiraculousMagic);
            }
            static void PatchProtectionOfNature() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("ProtectionOfNature")) { return; }

                var ProtectionOfNatureBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4652211718a2d9f4f860af19ad689663");
                ProtectionOfNatureBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<AddConcealment>();
                    bp.AddComponent<SetAttackerMissChance>(c => {
                        c.Value = 50;
                        c.m_Type = SetAttackerMissChance.Type.All;
                        c.Conditions = new ConditionsChecker();
                    });
                });

                TTTContext.Logger.LogPatch(ProtectionOfNatureBuff);
            }
            static void PatchRepulsiveNature() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("RepulsiveNature")) { return; }

                var RepulsiveNature = BlueprintTools.GetBlueprint<BlueprintAbility>("1b148617dac4f0341a9b0dec00b11b3a");
                var RepulsiveNatureArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("e2182d05755d9a54fa4967a5f6b3ecdf");
                var RepulsiveNatureNauseatedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c9b528fbbd8f3b84fb068bcf9b2c0ebb");
                var RepulsiveNatureBuffImmunity = BlueprintTools.GetBlueprint<BlueprintBuff>("7401fdfea49b9874db8a72579aec7c1f");
                var NasuseatedEffect = new PrefabLink() { AssetId = "02b8a5bbb308cbb408f0206cef7cf9d6" }; // Nasueated Visual Effect

                RepulsiveNature.TemporaryContext(bp => {
                    bp.GetComponent<SpellDescriptorComponent>().TemporaryContext(c => {
                        c.Descriptor = SpellDescriptor.Ground;
                    });
                    bp.AddComponent<AbilityAoERadius>(c => {
                        c.m_Radius = 30.Feet();
                    });
                });
                RepulsiveNatureArea.TemporaryContext(bp => {
                    bp.FlattenAllActions()
                        .OfType<ContextActionSavingThrow>()
                        .ForEach(action => {
                            action.HasCustomDC = false;
                            action.CustomDC = new ContextValue();
                        });
                    bp.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .Where(action => action.Buff == RepulsiveNatureNauseatedBuff)
                        .ForEach(action => {
                            action.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceCountValue = 0,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                }
                            };
                        });
                    bp.GetComponent<ContextRankConfig>().TemporaryContext(c => {
                        c.m_Type = AbilityRankType.Default;
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.AsIs;
                    });
                });
                RepulsiveNatureNauseatedBuff.TemporaryContext(bp => {
                    bp.FxOnStart = NasuseatedEffect;
                    bp.ResourceAssetIds = new string[] { NasuseatedEffect.AssetId };
                    bp.SetComponents();
                    bp.AddComponent<AddCondition>(c => {
                        c.Condition = UnitCondition.Nauseated;
                    });
                    bp.AddComponent<AddFactContextActions>(c => {
                        c.Activated = Helpers.CreateActionList(
                            new ContextActionDealDamage() {
                                DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Acid
                                },
                                Duration = new ContextDurationValue() {
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                },
                                Value = new ContextDiceValue() {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = 2,
                                    BonusValue = 0
                                }
                            }
                        );
                        c.Deactivated = Helpers.CreateActionList();
                        c.NewRound = Helpers.CreateActionList(
                            new ContextActionSavingThrow() {
                                Type = SavingThrowType.Fortitude,
                                CustomDC = new ContextValue(),
                                Actions = Helpers.CreateActionList(
                                    new ContextActionConditionalSaved() {
                                        Succeed = Helpers.CreateActionList(
                                            new ContextActionApplyBuff() {
                                                m_Buff = RepulsiveNatureBuffImmunity.ToReference<BlueprintBuffReference>(),
                                                Permanent = false,
                                                AsChild = false,
                                                DurationValue = new ContextDurationValue() {
                                                    Rate = DurationRate.Rounds,
                                                    DiceCountValue = 0,
                                                    BonusValue = new ContextValue() {
                                                        ValueType = ContextValueType.Rank
                                                    }
                                                }
                                            },
                                            new ContextActionRemoveSelf()
                                        ),
                                        Failed = Helpers.CreateActionList(
                                            new ContextActionDealDamage() {
                                                DamageType = new DamageTypeDescription() {
                                                    Type = DamageType.Energy,
                                                    Energy = DamageEnergyType.Acid
                                                },
                                                Duration = new ContextDurationValue() {
                                                    DiceCountValue = new ContextValue(),
                                                    BonusValue = new ContextValue()
                                                },
                                                Value = new ContextDiceValue() {
                                                    DiceType = DiceType.D6,
                                                    DiceCountValue = 2,
                                                    BonusValue = 0
                                                }
                                            }
                                        )
                                    }
                                )
                            }
                        );
                    });
                    bp.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.AsIs;
                    });
                    bp.AddComponent<SpellDescriptorComponent>(c => {
                        c.Descriptor = SpellDescriptor.Nauseated;// | SpellDescriptor.Poison;
                    });
                    bp.AddComponent<RemoveWhenCombatEnded>();
                });
                TTTContext.Logger.LogPatch(RepulsiveNature);
                TTTContext.Logger.LogPatch(RepulsiveNatureArea);
                TTTContext.Logger.LogPatch(RepulsiveNatureNauseatedBuff);
                TTTContext.Logger.LogPatch(RepulsiveNatureBuffImmunity);
            }
            static void PatchSongsOfSteel() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("SongsOfSteel")) { return; }

                var SongsOfSteelBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("6867deda1eda183499ae61813c2f5ebb");
                SongsOfSteelBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    bp.RemoveComponents<AdditionalDiceOnAttack>();
                    bp.AddComponent<AddAdditionalWeaponDamageOnHit>(c => {
                        c.OnlyOnFirstHit = true;
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Sonic
                        };
                        c.Value = new ContextDiceValue() {
                            DiceType = DiceType.D6,
                            DiceCountValue = 2,
                            BonusValue = new ContextValue() {
                                ValueType = ContextValueType.Rank
                            }
                        };
                    });
                    bp.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.AsIs;
                    });
                });

                TTTContext.Logger.LogPatch(SongsOfSteelBuff);
            }
            static void PatchSuddenSquall() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("SuddenSquall")) { return; }

                var SuddenSquall = BlueprintTools.GetBlueprint<BlueprintAbility>("4e22d7cfda74b3644b31de8e7c044e21");
                var SuddenSquallBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("e30d46f2ac194424694df9c7f1893ef1");

                SuddenSquall.TemporaryContext(bp => {
                    bp.Range = AbilityRange.Projectile;
                    bp.CanTargetEnemies = true;
                    bp.CanTargetFriends = false;
                    bp.CanTargetPoint = true;
                    bp.GetComponent<AbilityDeliverProjectile>().TemporaryContext(c => {
                        c.m_Length = 15.Feet();
                    });
                    var actionList = bp.GetComponent<AbilityEffectRunAction>().Actions;
                    bp.GetComponent<AbilityEffectRunAction>().TemporaryContext(c => {
                        c.Actions = Helpers.CreateActionList(
                            new Conditional() {
                                ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionIsAlly()
                                    }
                                },
                                IfFalse = actionList,
                                IfTrue = Helpers.CreateActionList()
                            }
                        );
                    });
                });
                SuddenSquallBuff.TemporaryContext(bp => {
                    bp.SetComponents();
                    bp.AddComponent<ForbidSpellCasting>(c => {
                        c.m_IgnoreFeature = new BlueprintFeatureReference();
                    });
                });
                TTTContext.Logger.LogPatch(SuddenSquall);
            }
            static void PatchUnbreakableBond() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("UnbreakableBond")) { return; }

                var UnbreakableBond = BlueprintTools.GetBlueprint<BlueprintAbility>("947a929f3347d3e458a524424fbceccb");
                var UnbreakableBondArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("9063d387e8d90a24f8bdd8c0c95f72f4");
                var UnbreakableBondImmunityBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("e60806180806b4c488f0d45af1035917");

                UnbreakableBond.TemporaryContext(bp => {
                    Conditional embedded = null;
                    bp.FlattenAllActions()
                        .OfType<Conditional>()
                        .Where(conditional => conditional.ConditionsChecker.Conditions.OfType<ContextConditionIsPartyMember>().Any())
                        .ForEach(conditional => {
                            conditional.IfTrue.AddAction(
                                new ContextActionApplyBuff() {
                                    m_Buff = UnbreakableBondImmunityBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Minutes,
                                        DiceCountValue = 0,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank
                                        }
                                    }
                                }
                            );
                            embedded = conditional;
                        });
                    bp.FlattenAllActions()
                        .OfType<Conditional>()
                        .ForEach(c => c.IfFalse = Helpers.CreateActionList());
                    bp.GetComponent<AbilityEffectRunAction>().TemporaryContext(c => {
                        if (embedded != null) {
                            c.Actions.AddAction(embedded);
                        }
                    });
                });
                UnbreakableBondArea.TemporaryContext(bp => {
                    bp.GetComponent<AbilityAreaEffectRunAction>().TemporaryContext(c => {
                        c.UnitExit.RemoveActions<ContextActionRemoveBuff>(a => a.Buff == UnbreakableBondImmunityBuff);
                    });
                    bp.FlattenAllActions()
                        .OfType<Conditional>()
                        .ForEach(c => {
                            c.IfTrue.RemoveActions<ContextActionApplyBuff>(a => a.Buff == UnbreakableBondImmunityBuff);
                            c.IfFalse.RemoveActions<ContextActionApplyBuff>(a => a.Buff == UnbreakableBondImmunityBuff);
                        });
                });
                UnbreakableBondImmunityBuff.TemporaryContext(bp => {
                    bp.AddComponent<SpellImmunityToSpellDescriptor>(c => {
                        c.Descriptor = SpellDescriptor.MindAffecting
                            | SpellDescriptor.Fear
                            | SpellDescriptor.Compulsion
                            | SpellDescriptor.Charm
                            | SpellDescriptor.Emotion
                            | SpellDescriptor.Shaken
                            | SpellDescriptor.Confusion;
                        c.m_CasterIgnoreImmunityFact = new BlueprintUnitFactReference();

                    });
                });
                TTTContext.Logger.LogPatch(UnbreakableBondImmunityBuff);
            }
            static void PatchWaterPush() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("WaterPush")) { return; }

                var WaterPush = BlueprintTools.GetBlueprint<BlueprintAbility>("17712729faf427f4fa0463bc919a0ff4");

                WaterPush.TemporaryContext(bp => {
                    bp.Range = AbilityRange.Projectile;
                    bp.CanTargetEnemies = true;
                    bp.CanTargetFriends = true;
                    bp.CanTargetPoint = true;
                    bp.GetComponent<ContextRankConfig>().TemporaryContext(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.DivStep;
                        c.m_StepLevel = 4;
                    });
                    bp.FlattenAllActions()
                        .OfType<ContextActionPush>()
                        .Where(c => c.Distance.Value == 10)
                        .ForEach(c => {
                            c.Distance = 4;
                        });
                    bp.FlattenAllActions()
                        .OfType<ContextActionPush>()
                        .Where(c => c.Distance.Value == 5)
                        .ForEach(c => {
                            c.Distance = 2;
                        });
                });
                TTTContext.Logger.LogPatch(WaterPush);
            }
            static void PatchWaterTorrent() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("WaterTorrent")) { return; }

                var WaterTorrent = BlueprintTools.GetBlueprint<BlueprintAbility>("cd7b6981218a0274c916db0a2fc29855");
                var WaterTorrentBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("62a04b00cef8bba4ca63e956f6184e6e");

                WaterTorrent.TemporaryContext(bp => {
                    bp.Range = AbilityRange.Projectile;
                    bp.CanTargetEnemies = true;
                    bp.CanTargetFriends = true;
                    bp.CanTargetPoint = true;
                    bp.GetComponent<AbilityDeliverProjectile>().TemporaryContext(c => {
                        c.Type = AbilityProjectileType.Cone;
                    });
                });
                WaterTorrentBuff.TemporaryContext(bp => {
                    bp.SetName(WaterTorrent.m_DisplayName);
                    bp.SetDescription(WaterTorrent.m_Description);
                });
                TTTContext.Logger.LogPatch(WaterTorrent);
            }
            static void PatchWindsOfFall() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("WindsOfFall")) { return; }

                var WindsOfTheFall = BlueprintTools.GetBlueprint<BlueprintAbility>("af2ed41c7894b934c9a9ca5048af3f58");
                var WindsOfTheFallBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b90339c580288eb48b7fea4abba0507e");

                WindsOfTheFall.TemporaryContext(bp => {
                    bp.Range = AbilityRange.Projectile;
                    bp.CanTargetEnemies = true;
                    bp.CanTargetFriends = true;
                    bp.CanTargetPoint = true;
                });
                WindsOfTheFallBuff.TemporaryContext(bp => {
                    bp.SetComponents();
                    bp.AddComponent<ModifyD20>(c => {
                        c.Rule = RuleType.All;
                        c.RollsAmount = 1;
                        c.TakeBest = false;
                        c.m_SavingThrowType = FlaggedSavingThrowType.All;
                        c.m_TandemTripFeature = new BlueprintFeatureReference();
                        c.RollResult = new ContextValue();
                        c.Bonus = new ContextValue();
                        c.Chance = new ContextValue();
                        c.ValueToCompareRoll = new ContextValue();
                        c.Skill = new StatType[0];
                        c.Value = new ContextValue();
                    });
                });
                TTTContext.Logger.LogPatch(WindsOfTheFall);
                TTTContext.Logger.LogPatch(WindsOfTheFallBuff);
            }
            //Demon Spells
            static void PatchAbyssalStorm() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("AbyssalStorm")) { return; }

                var AbyssalStorm = BlueprintTools.GetBlueprint<BlueprintAbility>("58e9e2883bca1574e9c932e72fd361f9");
                AbyssalStorm.GetComponent<AbilityEffectRunAction>().SavingThrowType = SavingThrowType.Unknown;
                AbyssalStorm.FlattenAllActions().OfType<ContextActionDealDamage>().ForEach(a => {
                    if (a.WriteResultToSharedValue) {
                        a.WriteRawResultToSharedValue = true;
                    } else {
                        a.ReadPreRolledFromSharedValue = false;
                    }
                    a.Value.DiceType = DiceType.D6;
                    a.Value.DiceCountValue = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                    a.Value.BonusValue = new ContextValue();
                    a.HalfIfSaved = false;
                    a.IsAoE = true;
                });
                AbyssalStorm.GetComponent<AbilityTargetsAround>().TemporaryContext(c => {
                    c.m_Condition = new ConditionsChecker() {
                        Conditions = new Condition[] {
                            new ContextConditionIsCaster(){
                                Not = true
                            }
                        }
                    };
                });
                TTTContext.Logger.LogPatch("Patched", AbyssalStorm);
            }
            //Lich Spells
            static void PatchCorruptMagic() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("CorruptMagic")) { return; }

                var CorruptMagic = BlueprintTools.GetBlueprint<BlueprintAbility>("6fd7bdd6dfa9dd943b36d65faf97ac41");

                CorruptMagic.FlattenAllActions()
                    .OfType<ContextActionDispelMagic>()
                    .ForEach(a => {
                        a.OneRollForAll = true;
                    });
                TTTContext.Logger.LogPatch("Patched", CorruptMagic);
            }
            static void PatchPowerFromDeath() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("PowerFromDeath")) { return; }

                var PowerFromDeath = BlueprintTools.GetBlueprint<BlueprintAbility>("9c2c85a3782af804880c81c1712e3a7b");

                PowerFromDeath.FlattenAllActions()
                    .OfType<ContextActionApplyBuff>()
                    .ForEach(a => a.DurationValue.Rate = DurationRate.Rounds);

                TTTContext.Logger.LogPatch(PowerFromDeath);
            }
            static void PatchVampiricBlade() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("VampiricBlade")) { return; }

                var VampiricBladeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f6007b38909c3b248a8a77b316f5bc2d");

                VampiricBladeBuff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                VampiricBladeBuff.RemoveComponents<ContextCalculateSharedValue>();
                VampiricBladeBuff.RemoveComponents<RecalculateOnFactsChange>();
                VampiricBladeBuff.AddComponent<AddAdditionalWeaponDamage>(c => {
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.D6,
                        DiceCountValue = 1,
                        BonusValue = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        }
                    };
                    c.DamageType = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Unholy
                    };
                });
                VampiricBladeBuff.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.ActionsOnInitiator = true;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionHealTarget>(a => {
                            a.Value = new ContextDiceValue() {
                                DiceType = DiceType.D6,
                                DiceCountValue = 1,
                                BonusValue = new ContextValue() {
                                    ValueType = ContextValueType.Rank
                                }
                            };
                        })
                    );
                });
                TTTContext.Logger.LogPatch(VampiricBladeBuff);
            }
            //Trickster Spells
            static void PatchMicroscopicProportions() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("MicroscopicProportions")) { return; }

                var TricksterMicroscopicProportionsBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1dfc2f933e7833f41922411962e1d58a");
                TricksterMicroscopicProportionsBuff
                    .GetComponents<AddContextStatBonus>()
                    .ForEach(c => c.Descriptor = ModifierDescriptor.Size);

                TTTContext.Logger.LogPatch("Patched", TricksterMicroscopicProportionsBuff);
            }
            //General Spells
            static void PatchAcidMaw() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("AcidMaw")) { return; }

                var AcidMawBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f1a6799b05a40144d9acdbdca1d7c228");

                var AdditionalDiceOnAttack = AcidMawBuff.GetComponent<AdditionalDiceOnAttack>();
                var EnergyType = AdditionalDiceOnAttack.DamageType.Energy;
                AcidMawBuff.AddComponent<AddAdditionalWeaponDamage>(c => {
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.D4,
                        DiceCountValue = 1,
                        BonusValue = 0
                    };
                    c.DamageType = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Energy = EnergyType
                    };
                    c.CheckWeaponCatergoy = true;
                    c.Category = WeaponCategory.Bite;
                });
                AcidMawBuff.RemoveComponent(AdditionalDiceOnAttack);
                TTTContext.Logger.LogPatch("Patched", AcidMawBuff);
            }
            static void PatchAnimalGrowth() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("AnimalGrowth")) { return; }

                var AnimalCompanionSelectionBase = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("90406c575576aee40a34917a1b429254");
                IEnumerable<BlueprintFeature> AnimalCompanionUpgrades = AnimalCompanionSelectionBase.m_AllFeatures.Concat(AnimalCompanionSelectionBase.m_Features)
                    .Select(feature => feature.Get())
                    .Where(feature => feature.GetComponent<AddPet>())
                    .Select(feature => feature.GetComponent<AddPet>())
                    .Where(component => component.m_UpgradeFeature != null)
                    .Select(component => component.m_UpgradeFeature.Get())
                    .Where(feature => feature != null)
                    .Distinct();
                AnimalCompanionUpgrades.ForEach(bp => {
                    var component = bp.GetComponent<ChangeUnitSize>();
                    if (component == null) { return; }
                    bp.RemoveComponent(component);
                    if (component.IsTypeDelta) {
                        bp.AddComponent<ChangeUnitBaseSize>(c => {
                            c.m_Type = Core.NewUnitParts.UnitPartBaseSizeAdjustment.ChangeType.Delta;
                            c.SizeDelta = component.SizeDelta;
                        });
                    } else if (component.IsTypeValue) {
                        bp.AddComponent<ChangeUnitBaseSize>(c => {
                            c.m_Type = Core.NewUnitParts.UnitPartBaseSizeAdjustment.ChangeType.Value;
                            c.Size = component.Size;
                        });
                    }
                    TTTContext.Logger.LogPatch("Patched", bp);
                });
            }
            static void PatchBestowCurseGreater() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("BestowCurseGreater")) { return; }

                var BestowCurseGreaterDeterioration = BlueprintTools.GetBlueprint<BlueprintAbility>("71196d7e6d6645247a058a3c3c9bb5fd");
                var BestowCurseGreaterFeebleBody = BlueprintTools.GetBlueprint<BlueprintAbility>("c74a7dfebd7b1004a80f7e59689dfadd");
                var BestowCurseGreaterIdiocy = BlueprintTools.GetBlueprint<BlueprintAbility>("f7739a453e2138b46978e9098a29b3fb");
                var BestowCurseGreaterWeakness = BlueprintTools.GetBlueprint<BlueprintAbility>("abb2d42dd9219eb41848ec56a8726d58");

                var BestowCurseGreaterDeteriorationCast = BlueprintTools.GetBlueprint<BlueprintAbility>("54606d540f5d3684d9f7d6e2e2be9b63");
                var BestowCurseGreaterFeebleBodyCast = BlueprintTools.GetBlueprint<BlueprintAbility>("292d630a5abae64499bb18057aaa24b4");
                var BestowCurseGreaterIdiocyCast = BlueprintTools.GetBlueprint<BlueprintAbility>("e0212142d2a426f43926edd4202996bb");
                var BestowCurseGreaterWeaknessCast = BlueprintTools.GetBlueprint<BlueprintAbility>("1168f36fac0bad64f965928206df7b86");

                var BestowCurseGreaterDeteriorationBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("8f8835d083f31c547a39ebc26ae42159");
                var BestowCurseGreaterFeebleBodyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("28c9db77dfb1aa54a94e8a7413b1840a");
                var BestowCurseGreaterIdiocyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("493dcc29a21abd94d9adb579e1f40318");
                var BestowCurseGreaterWeaknessBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("0493a9d25687d7e4682e250ae3ccb187");

                RebuildCurse(
                    BestowCurseGreaterDeterioration,
                    BestowCurseGreaterDeteriorationCast,
                    BestowCurseGreaterDeteriorationBuff);
                RebuildCurse(
                    BestowCurseGreaterFeebleBody,
                    BestowCurseGreaterFeebleBodyCast,
                    BestowCurseGreaterFeebleBodyBuff);
                RebuildCurse(
                    BestowCurseGreaterIdiocy,
                    BestowCurseGreaterIdiocyCast,
                    BestowCurseGreaterIdiocyBuff);
                RebuildCurse(
                    BestowCurseGreaterWeakness,
                    BestowCurseGreaterWeaknessCast,
                    BestowCurseGreaterWeaknessBuff);

                void RebuildCurse(BlueprintAbility curse, BlueprintAbility curseCast, BlueprintBuff curseBuff) {
                    curseCast.GetComponent<AbilityEffectStickyTouch>().m_TouchDeliveryAbility = curse.ToReference<BlueprintAbilityReference>();
                    TTTContext.Logger.LogPatch("Patched", curseCast);
                    curse.GetComponent<AbilityEffectRunAction>()
                        .Actions.Actions.OfType<ContextActionConditionalSaved>().First()
                        .Failed.Actions.OfType<ContextActionApplyBuff>().First()
                        .m_Buff = curseBuff.ToReference<BlueprintBuffReference>();
                    TTTContext.Logger.LogPatch("Patched", curse);
                    curseBuff.m_Icon = curse.m_Icon;
                    TTTContext.Logger.LogPatch("Patched", curseBuff);
                }
            }
            static void PatchBreakEnchantment() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("BreakEnchantment")) { return; }

                var BreakEnchantment = BlueprintTools.GetBlueprint<BlueprintAbility>("7792da00c85b9e042a0fdfc2b66ec9a8");
                BreakEnchantment
                    .FlattenAllActions()
                    .OfType<ContextActionDispelMagic>()
                    .ForEach(dispel => {
                        dispel.OnlyTargetEnemyBuffs = true;
                        dispel.m_MaxSpellLevel = new ContextValue();
                    });
                TTTContext.Logger.LogPatch("Patched", BreakEnchantment);
            }
            static void PatchChainLightning() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("ChainLightning")) { return; }

                var ChainLightning = BlueprintTools.GetBlueprint<BlueprintAbility>("645558d63604747428d55f0dd3a4cb58");
                ChainLightning
                    .FlattenAllActions()
                    .OfType<ContextActionDealDamage>()
                    .ForEach(damage => {
                        damage.Value.DiceCountValue.ValueRank = AbilityRankType.DamageDice;
                    });
                TTTContext.Logger.LogPatch("Patched", ChainLightning);
            }
            static void PatchCommand() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("Command")) { return; }

                var Command = BlueprintTools.GetBlueprint<BlueprintAbility>("feb70aab86cc17f4bb64432c83737ac2");

                Command.AbilityAndVariants()
                    .ForEach(ability => {
                        var descriptors = ability.GetComponent<SpellDescriptorComponent>();
                        if (descriptors is null) {
                            ability.AddComponent<SpellDescriptorComponent>();
                            descriptors = ability.GetComponent<SpellDescriptorComponent>();
                        }
                        descriptors.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion;
                        TTTContext.Logger.LogPatch(ability);
                    });
            }
            static void PatchCommandGreater() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("CommandGreater")) { return; }

                var CommandGreater = BlueprintTools.GetBlueprint<BlueprintAbility>("cb15cc8d7a5480648855a23b3ba3f93d");

                CommandGreater.AbilityAndVariants()
                    .ForEach(ability => {
                        var descriptors = ability.GetComponent<SpellDescriptorComponent>();
                        if (descriptors is null) {
                            ability.AddComponent<SpellDescriptorComponent>();
                            descriptors = ability.GetComponent<SpellDescriptorComponent>();
                        }
                        descriptors.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion;
                        TTTContext.Logger.LogPatch(ability);
                    });
            }
            static void PatchCrusadersEdge() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("CrusadersEdge")) { return; }

                BlueprintBuff CrusadersEdgeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("7ca348639a91ae042967f796098e3bc3");
                CrusadersEdgeBuff.GetComponent<AddInitiatorAttackWithWeaponTrigger>().CriticalHit = true;
                TTTContext.Logger.LogPatch("Patched", CrusadersEdgeBuff);
            }
            static void PatchDeathWard() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("DeathWard")) { return; }

                var NegativeLevelsBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("b02b6b9221241394db720ca004ea9194");
                var DeathWardBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b0253e57a75b621428c1b89de5a937d1");
                var DeathWardBuff_IsFromSpell = BlueprintTools.GetBlueprint<BlueprintBuff>("a04d666d8b1f5a2419f1adc6874ae65a");

                DeathWardBuff.SetDescription(TTTContext, "The subject gains a +4 morale bonus on saves against all death spells " +
                    "and magical death effects. The subject is granted a save to negate such effects even if one is not normally allowed. " +
                    "The subject is immune to energy drain and any negative energy effects, including channeled negative energy. " +
                    "This spell does not remove negative levels that the subject has already gained, " +
                    "but it does remove the penalties from negative levels for the duration of its effect. " +
                    "Death ward does not protect against other sorts of attacks, even if those attacks might be lethal. ");
                DeathWardBuff.AddComponent<SuppressBuffsTTT>(c => {
                    c.Continuous = true;
                    c.m_Buffs = new BlueprintBuffReference[] {
                        NegativeLevelsBuff
                    };
                });
                DeathWardBuff_IsFromSpell.SetDescription(TTTContext, "The subject gains a +4 morale bonus on saves against all death spells " +
                    "and magical death effects. The subject is granted a save to negate such effects even if one is not normally allowed. " +
                    "The subject is immune to energy drain and any negative energy effects, including channeled negative energy. " +
                    "This spell does not remove negative levels that the subject has already gained, " +
                    "but it does remove the penalties from negative levels for the duration of its effect. " +
                    "Death ward does not protect against other sorts of attacks, even if those attacks might be lethal. ");
                DeathWardBuff_IsFromSpell.AddComponent<SuppressBuffsTTT>(c => {
                    c.Continuous = true;
                    c.m_Buffs = new BlueprintBuffReference[] {
                        NegativeLevelsBuff
                    };
                });
                TTTContext.Logger.LogPatch("Patched", DeathWardBuff);
                TTTContext.Logger.LogPatch("Patched", DeathWardBuff_IsFromSpell);
            }
            static void PatchDispelMagicGreater() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("DispelMagicGreater")) { return; }

                var DispelMagicGreaterTarget = BlueprintTools.GetBlueprint<BlueprintAbility>("6d490c80598f1d34bb277735b52d52c1");
                DispelMagicGreaterTarget.SetDescription(TTTContext, "This functions as a targeted dispel magic, but it can dispel one spell for every four caster " +
                    "levels you possess, starting with the highest level spells and proceeding to lower level spells.\n" +
                    "Targeted Dispel: One object, creature, or spell is the target of the dispel magic spell. You make one dispel check (1d20 + your caster level)" +
                    " and compare that to the spell with highest caster level (DC = 11 + the spell’s caster level). If successful, that spell ends. " +
                    "If not, compare the same result to the spell with the next highest caster level. Repeat this process until you have dispelled " +
                    "one spell affecting the target, or you have failed to dispel every spell.");
                TTTContext.Logger.LogPatch("Patched", DispelMagicGreaterTarget);
            }
            static void PatchEcholocation() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("Echolocation")) { return; }

                var EcholocationBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("cbfd2f5279f5946439fe82570fd61df2");

                EcholocationBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<AddCondition>(c => c.Condition == UnitCondition.Blindness);
                });

                TTTContext.Logger.LogPatch(EcholocationBuff);
            }
            static void PatchFieryBody() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("FieryBody")) { return; }

                var FieryBody = BlueprintTools.GetBlueprint<BlueprintAbility>("08ccad78cac525040919d51963f9ac39");
                var FieryBodyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b574e1583768798468335d8cdb77e94c");

                FieryBody.TemporaryContext(bp => {
                    bp.GetComponent<SpellDescriptorComponent>().Descriptor = SpellDescriptor.Fire | SpellDescriptor.RestoreHP;
                    bp.RemoveComponents<AbilityExecuteActionOnCast>();
                });
                FieryBodyBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<SpellDescriptorComponent>();
                    bp.RemoveComponents<PolymorphBonuses>();
                });

                TTTContext.Logger.LogPatch(FieryBody);
                TTTContext.Logger.LogPatch(FieryBodyBuff);
            }
            static void PatchFirebrand() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("Firebrand")) { return; }

                var FirebrandBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c6cc1c5356db4674dbd2be20ea205c86");

                FirebrandBuff.RemoveComponents<AdditionalDiceOnAttack>();
                FirebrandBuff.AddComponent<AddAdditionalWeaponDamage>(c => {
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.D6,
                        DiceCountValue = 1,
                        BonusValue = 0
                    };
                    c.DamageType = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Fire
                    };
                });
                TTTContext.Logger.LogPatch("Patched", FirebrandBuff);
            }
            static void PatchFlamestrike() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("FlameStrike")) { return; }

                var FlameStrike = BlueprintTools.GetBlueprint<BlueprintAbility>("f9910c76efc34af41b6e43d5d8752f0f");
                FlameStrike.FlattenAllActions().OfType<ContextActionDealDamage>().ForEach(a => {
                    a.Half = true;
                    a.HalfIfSaved = true;
                    a.ReadPreRolledFromSharedValue = false;
                });
                TTTContext.Logger.LogPatch("Patched", FlameStrike);
            }
            static void PatchFreedomOfMovement() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("FreedomOfMovement")) { return; }

                var SeamantleBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1c05dd3a1c78b0e4e9f7438a43e7a9fd");
                var FreedomOfMovementBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1533e782fca42b84ea370fc1dcbf4fc1");
                var FreedomOfMovementBuffPermanent = BlueprintTools.GetBlueprint<BlueprintBuff>("235533b62159790499ced35860636bb2");
                var FreedomOfMovementBuff_FD = BlueprintTools.GetBlueprint<BlueprintBuff>("60906dd9e4ddec14c8ac9a0f4e47f54c");
                var DLC3_FreedomOfMovementBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("d6fb42ec153f4d699e57891522d7f4c9");
                var FreedomOfMovementLinnorm = BlueprintTools.GetBlueprint<BlueprintBuff>("67519ff6ba615c045afca2347608bfe3");
                var BootsOfFreeReinBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("7ac8effd6341443d98da735b965b0176");
                var BootsOfFreeReinBuff2 = BlueprintTools.GetBlueprint<BlueprintBuff>("e24e9c0d77144663815c69e969ac4fdb");

                RemoveStaggerImmunity(FreedomOfMovementBuff);
                RemoveStaggerImmunity(FreedomOfMovementBuffPermanent);
                RemoveStaggerImmunity(FreedomOfMovementBuff_FD);
                RemoveStaggerImmunity(DLC3_FreedomOfMovementBuff);
                RemoveStaggerImmunity(FreedomOfMovementLinnorm);
                RemoveStaggerImmunity(BootsOfFreeReinBuff);
                RemoveStaggerImmunity(BootsOfFreeReinBuff2);

                SeamantleBuff.TemporaryContext(bp => {
                    bp.GetComponent<ACBonusUnlessFactMultiple>()?.TemporaryContext(c => {
                        c.m_Facts = new BlueprintUnitFactReference[]{
                            FreedomOfMovementBuff.ToReference<BlueprintUnitFactReference>(),
                            FreedomOfMovementBuffPermanent.ToReference<BlueprintUnitFactReference>(),
                            FreedomOfMovementBuff_FD.ToReference<BlueprintUnitFactReference>(),
                            DLC3_FreedomOfMovementBuff.ToReference<BlueprintUnitFactReference>(),
                            FreedomOfMovementLinnorm.ToReference<BlueprintUnitFactReference>(),
                            BootsOfFreeReinBuff.ToReference<BlueprintUnitFactReference>(),
                            BootsOfFreeReinBuff2.ToReference<BlueprintUnitFactReference>(),
                        };
                    });
                });
                TTTContext.Logger.LogPatch(SeamantleBuff);

                static void RemoveStaggerImmunity(BlueprintBuff buff) {
                    buff.RemoveComponents<AddConditionImmunity>(p => p.Condition == UnitCondition.Staggered);
                    buff.GetComponents<BuffDescriptorImmunity>().ForEach(c => {
                        c.Descriptor &= ~SpellDescriptor.Staggered;
                    });
                    TTTContext.Logger.LogPatch(buff);
                }
            }
            static void PatchGeniekind() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("Geniekind")) { return; }

                var GeniekindDjinniBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("082caf8c1005f114ba6375a867f638cf");
                var GeniekindEfreetiBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("d47f45f29c4cfc0469f3734d02545e0b");
                var GeniekindMaridBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4f37fc07fe2cf7f4f8076e79a0a3bfe9");
                var GeniekindShaitanBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1d498104f8e35e246b5d8180b0faed43");

                ReplaceComponents(GeniekindDjinniBuff);
                ReplaceComponents(GeniekindEfreetiBuff);
                ReplaceComponents(GeniekindMaridBuff);
                ReplaceComponents(GeniekindShaitanBuff);

                void ReplaceComponents(BlueprintBuff GeniekindBuff) {
                    var EnergyType = GeniekindBuff.GetComponent<AdditionalDiceOnAttack>().DamageType.Energy;
                    GeniekindBuff.RemoveComponents<AdditionalDiceOnAttack>();
                    GeniekindBuff.AddComponent<AddAdditionalWeaponDamage>(c => {
                        c.CheckWeaponRangeType = true;
                        c.RangeType = WeaponRangeType.Melee;
                        c.Value = new ContextDiceValue() {
                            DiceType = DiceType.D6,
                            DiceCountValue = 1,
                            BonusValue = 0
                        };
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Energy,
                            Energy = EnergyType
                        };
                    });
                    TTTContext.Logger.LogPatch("Patched", GeniekindBuff);
                }
            }
            static void PatchHellfireRay() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("HellfireRay")) { return; }

                var HellfireRay = BlueprintTools.GetBlueprint<BlueprintAbility>("700cfcbd0cb2975419bcab7dbb8c6210");
                HellfireRay.GetComponent<SpellDescriptorComponent>().Descriptor = SpellDescriptor.Evil;
                HellfireRay.AvailableMetamagic &= (Metamagic)~(CustomMetamagic.Flaring | CustomMetamagic.Burning);
                HellfireRay.FlattenAllActions().OfType<ContextActionDealDamage>().ForEach(a => {
                    a.Half = true;
                    a.Value.DiceType = DiceType.D6;
                    a.Value.DiceCountValue.ValueType = ContextValueType.Rank;
                    a.Value.BonusValue = new ContextValue();
                    a.ReadPreRolledFromSharedValue = false;
                });
                TTTContext.Logger.LogPatch("Patched", HellfireRay);
            }
            static void PatchHurricaneBow() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("HurricaneBow")) { return; }

                var HurricaneBowBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("002c51d933574824c8ef2b04c9d09ff5");

                HurricaneBowBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<RangedWeaponSizeChange>();
                    bp.AddComponent<WeaponSizeChangeTTT>(c => {
                        c.CheckRangeType = true;
                        c.RangeType = WeaponRangeType.Ranged;
                        c.CheckWeaponCategory = true;
                        c.Categories = new WeaponCategory[] {
                            WeaponCategory.Longbow,
                            WeaponCategory.Shortbow,
                            WeaponCategory.HandCrossbow,
                            WeaponCategory.HeavyCrossbow,
                            WeaponCategory.HeavyRepeatingCrossbow,
                            WeaponCategory.LightCrossbow,
                            WeaponCategory.LightRepeatingCrossbow
                        };
                        c.SizeChange = 1;
                    });
                });

                TTTContext.Logger.LogPatch(HurricaneBowBuff);
            }
            static void PatchIcyBody() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("IcyBody")) { return; }

                var IceBody = BlueprintTools.GetBlueprint<BlueprintAbility>("89778dc261fe6094bb2445cb389842d2");
                var IceBodyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a6da7d6a5c9377047a7bd2680912860f");

                IceBody.TemporaryContext(bp => {
                    bp.GetComponent<SpellDescriptorComponent>().Descriptor = SpellDescriptor.Cold;
                    bp.RemoveComponents<AbilityExecuteActionOnCast>();
                });
                IceBodyBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<SpellDescriptorComponent>();
                    bp.RemoveComponents<PolymorphBonuses>();
                });

                TTTContext.Logger.LogPatch(IceBody);
                TTTContext.Logger.LogPatch(IceBodyBuff);
            }
            static void PatchIronBody() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("IronBody")) { return; }

                var IronBody = BlueprintTools.GetBlueprint<BlueprintAbility>("198fcc43490993f49899ed086fe723c1");
                var IronBodyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2eabea6a1f9a58246a822f207e8ca79e");

                IronBody.TemporaryContext(bp => {
                    bp.RemoveComponents<SpellDescriptorComponent>();
                    bp.RemoveComponents<AbilityExecuteActionOnCast>();
                });
                IronBodyBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<SpellDescriptorComponent>();
                    bp.RemoveComponents<PolymorphBonuses>();
                });

                TTTContext.Logger.LogPatch(IronBody);
                TTTContext.Logger.LogPatch(IronBodyBuff);
            }
            static void PatchLeadBlades() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("LeadBlades")) { return; }

                var LeafBladesBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("91f43163db96f8941a41e2b584a97514");

                LeafBladesBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<IncreaseDiceSizeOnAttack>();
                    bp.AddComponent<WeaponSizeChangeTTT>(c => {
                        c.CheckRangeType = true;
                        c.RangeType = WeaponRangeType.Melee;
                        c.SizeChange = 1;
                    });
                });

                TTTContext.Logger.LogPatch(LeafBladesBuff);
            }
            static void PatchLegendaryProportions() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("LegendaryProportions")) { return; }
                if (Harmony.HasAnyPatches("WorldCrawl")) { return; } //Breaks WorldCrawl saves due to how things get patched

                var LegendaryProportions = BlueprintTools.GetBlueprint<BlueprintAbility>("da1b292d91ba37948893cdbe9ea89e28");
                var LegendaryProportionsBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4ce640f9800d444418779a214598d0a3");

                LegendaryProportions.SetDescription(TTTContext, "You call upon the primordial power of ancient megafauna to boost the size of your target. " +
                    "Because of its connection to living creatures of the distant past, the spell does not function on outsiders, undead, and summoned creatures. " +
                    "Your target grows to legendary proportions, increasing in size by one category. " +
                    "The creature's height doubles and its weight increases by a factor of 8. " +
                    "The target gains a +6 size bonus to its Strength score and a +4 size bonus to its Constitution score. " +
                    "It gains a +6 size bonus to its natural armor, and DR 10/adamantine. " +
                    "Melee and ranged weapons used by this creature deal more damage.");
                LegendaryProportionsBuff.TemporaryContext(bp => {
                    bp.SetDescription(LegendaryProportions.m_Description);
                    bp.GetComponent<ChangeUnitSize>().SizeDelta = 1;
                });

                TTTContext.Logger.LogPatch(LegendaryProportions);
                TTTContext.Logger.LogPatch(LegendaryProportionsBuff);
            }
            static void PatchLifeBubble() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("LifeBubble")) { return; }
                if (Harmony.HasAnyPatches("WorldCrawl")) { return; } //Breaks WorldCrawl saves due to how things get patched

                var LifeBubble = BlueprintTools.GetBlueprint<BlueprintAbility>("265582bc494c4b12b5860b508a2f89a2");
                var resistenergy00 = new PrefabLink() {
                    AssetId = "e23fec8d2024a8c48a8b4a57693e31a7"
                };

                LifeBubble.TemporaryContext(bp => {
                    bp.SetLocalizedDuration(TTTContext, "10 minutes/level");
                    bp.FlattenAllActions().OfType<ContextActionApplyBuff>().ForEach(a => {
                        a.DurationValue.TemporaryContext(d => {
                            d.Rate = DurationRate.TenMinutes;
                            d.DiceCountValue = 0;
                            d.BonusValue = new ContextValue() {
                                ValueType = ContextValueType.Rank
                            };
                        });
                    });
                    bp.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.AsIs;
                    });
                    bp.AddComponent<AbilityTargetsAround>(c => {
                        c.m_Radius = 20.Feet();
                        c.m_TargetType = TargetType.Ally;
                        c.m_Condition = new ConditionsChecker();
                    });
                    bp.AddComponent<AbilitySpawnFx>(c => {
                        c.PrefabLink = resistenergy00;
                        c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
                        c.PositionAnchor = AbilitySpawnFxAnchor.None;
                        c.OrientationAnchor = AbilitySpawnFxAnchor.None;
                    });
                });

            }
            static void PatchMagicalVestment() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("MagicalVestment")) { return; }

                PatchMagicalVestmentArmor();
                PatchMagicalVestmentShield();

                void PatchMagicalVestmentShield() {
                    var MagicalVestmentShield = BlueprintTools.GetBlueprint<BlueprintAbility>("adcda176d1756eb45bd5ec9592073b09");
                    var MagicalVestmentShieldBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2e8446f820936a44f951b50d70a82b16");

                    MagicalVestmentShieldBuff.SetComponents();
                    MagicalVestmentShieldBuff.AddComponent<MagicalVestmentComponent>(c => {
                        c.Shield = true;
                        c.EnchantLevel = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                        c.m_EnchantmentBlueprints = new BlueprintItemEnchantmentReference[] {
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor1TTT"),
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor2TTT"),
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor3TTT"),
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor4TTT"),
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor5TTT")
                        };
                    });
                    MagicalVestmentShieldBuff.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.DivStep;
                        c.m_StepLevel = 4;
                        c.m_Min = 1;
                        c.m_UseMin = true;
                        c.m_Max = 5;
                    });
                    TTTContext.Logger.LogPatch("Patched", MagicalVestmentShieldBuff);
                }
                void PatchMagicalVestmentArmor() {
                    var MagicalVestmentArmor = BlueprintTools.GetBlueprint<BlueprintAbility>("956309af83352714aa7ee89fb4ecf201");
                    var MagicalVestmentArmorBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("9e265139cf6c07c4fb8298cb8b646de9");

                    MagicalVestmentArmorBuff.SetComponents();
                    MagicalVestmentArmorBuff.AddComponent<MagicalVestmentComponent>(c => {
                        c.EnchantLevel = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                        c.m_EnchantmentBlueprints = new BlueprintItemEnchantmentReference[] {
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor1TTT"),
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor2TTT"),
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor3TTT"),
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor4TTT"),
                            BlueprintTools.GetModBlueprintReference<BlueprintItemEnchantmentReference>(TTTContext, "TemporaryEnhancementArmor5TTT")
                        };
                    });
                    MagicalVestmentArmorBuff.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.DivStep;
                        c.m_StepLevel = 4;
                        c.m_Min = 1;
                        c.m_UseMin = true;
                        c.m_Max = 5;
                    });
                    TTTContext.Logger.LogPatch("Patched", MagicalVestmentArmorBuff);
                }
            }
            static void PatchMagicWeaponGreater() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("GreaterMagicWeapon")) { return; }

                var MagicWeaponGreaterPrimary = BlueprintTools.GetBlueprint<BlueprintAbility>("a3fe23711486ee9489af1dadd6906149");
                var MagicWeaponGreaterSecondary = BlueprintTools.GetBlueprint<BlueprintAbility>("89c13df989e5e624692134d55195121a");
                var newEnhancements = new BlueprintItemEnchantmentReference[] {
                    BlueprintTools.GetModBlueprint<BlueprintWeaponEnchantment>(TTTContext, "TemporaryEnhancement1NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintWeaponEnchantment>(TTTContext, "TemporaryEnhancement2NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintWeaponEnchantment>(TTTContext, "TemporaryEnhancement3NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintWeaponEnchantment>(TTTContext, "TemporaryEnhancement4NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintWeaponEnchantment>(TTTContext, "TemporaryEnhancement5NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
                };

                MagicWeaponGreaterPrimary.FlattenAllActions().OfType<EnhanceWeapon>().ForEach(c => c.m_Enchantment = newEnhancements);
                MagicWeaponGreaterSecondary.FlattenAllActions().OfType<EnhanceWeapon>().ForEach(c => c.m_Enchantment = newEnhancements);

                TTTContext.Logger.LogPatch("Patched", MagicWeaponGreaterPrimary);
                TTTContext.Logger.LogPatch("Patched", MagicWeaponGreaterSecondary);
            }
            static void PatchMindFog() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("MindFog")) { return; }

                var MindFogArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("fe5102d734382b74586f56980086e5e8");
                var MindFogBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("59fa875508d497d43823bf5253299070");
                var MindFogAfterBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("5a943abc9d22d074f9bf4b1f2a447002");

                MindFogArea.TemporaryContext(bp => {
                    bp.SetComponents();
                    bp.AddComponent<AbilityAreaEffectRunAction>(c => {
                        c.UnitEnter = Helpers.CreateActionList(
                            new Conditional() {
                                ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasBuff() {
                                            m_Buff = MindFogBuff
                                        },
                                        new ContextConditionHasBuff() {
                                            m_Buff = MindFogAfterBuff
                                        }
                                    },
                                    Operation = Operation.Or
                                },
                                IfTrue = Helpers.CreateActionList(),
                                IfFalse = Helpers.CreateActionList(
                                    new ContextActionSavingThrow() {
                                        Type = SavingThrowType.Will,
                                        CustomDC = new ContextValue(),
                                        Actions = Helpers.CreateActionList(
                                            new ContextActionConditionalSaved() {
                                                Succeed = Helpers.CreateActionList(),
                                                Failed = Helpers.CreateActionList(
                                                    new ContextActionApplyBuff() {
                                                        m_Buff = MindFogBuff,
                                                        Permanent = true,
                                                        DurationValue = new ContextDurationValue() {
                                                            DiceCountValue = new ContextValue(),
                                                            BonusValue = new ContextValue()
                                                        }
                                                    }
                                                )
                                            }
                                        )
                                    }
                                )
                            }
                        );
                        c.UnitExit = Helpers.CreateActionList(
                            new Conditional() {
                                ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasBuff() {
                                            m_Buff = MindFogBuff
                                        }
                                    }
                                },
                                IfTrue = Helpers.CreateActionList(
                                    new ContextActionRemoveBuff() {
                                        m_Buff = MindFogBuff
                                    },
                                    new ContextActionApplyBuff() {
                                        m_Buff = MindFogAfterBuff,
                                        DurationValue = new ContextDurationValue() {
                                            DiceType = DiceType.D6,
                                            DiceCountValue = 2,
                                            BonusValue = 0
                                        }
                                    }
                                ),
                                IfFalse = Helpers.CreateActionList()
                            }
                        );
                        c.UnitMove = Helpers.CreateActionList();
                        c.Round = Helpers.CreateActionList();
                    });
                });

                TTTContext.Logger.LogPatch(MindFogArea);
            }
            static void PatchProtectionFromAlignmentGreater() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("ProtectionFromAlignmentGreater")) { return; }

                var CloakOfChaos = BlueprintTools.GetBlueprint<BlueprintAbility>("9155dbc8268da1c49a7fc4834fa1a4b1");
                var ShieldOfLaw = BlueprintTools.GetBlueprint<BlueprintAbility>("73e7728808865094b8892613ddfaf7f5");
                var DominatePersonBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("c0f4e1c24c9cd334ca988ed1bd9d201f");

                var HolyAuraBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a33bf327207a5904d9e38d6a80eb09e2");
                var UnholyAuraBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("9eda82a1f78558747a03c17e0e9a1a68");
                var CloakOfChaosBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("627224e071dc51f47ba402fcbb6f830d");
                var ShieldOfLawBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("0da7299aac601d445a355152084c251a");

                var SpellExceptions = HolyAuraBuff.GetComponent<AddSpellImmunity>().Exceptions.ToList();

                CloakOfChaosBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<SpellImmunityToSpellDescriptor>(c => c.Descriptor == SpellDescriptor.MindAffecting);
                    bp.RemoveComponents<BuffDescriptorImmunity>(c => c.Descriptor == SpellDescriptor.MindAffecting);
                    bp.AddComponent<SpecificBuffImmunity>(c => {
                        c.m_Buff = DominatePersonBuff;
                    });
                    bp.AddComponent<AddSpellImmunity>(c => {
                        c.Type = Kingmaker.UnitLogic.Parts.SpellImmunityType.Specific;
                        c.m_Exceptions = SpellExceptions.Select(a => a.ToReference<BlueprintAbilityReference>()).Distinct().ToArray();
                    });
                });
                ShieldOfLawBuff.TemporaryContext(bp => {
                    bp.RemoveComponents<SpellImmunityToSpellDescriptor>(c => c.Descriptor == SpellDescriptor.MindAffecting);
                    bp.RemoveComponents<BuffDescriptorImmunity>(c => c.Descriptor == SpellDescriptor.MindAffecting);
                    bp.AddComponent<SpecificBuffImmunity>(c => {
                        c.m_Buff = DominatePersonBuff;
                    });
                    bp.AddComponent<AddSpellImmunity>(c => {
                        c.Type = Kingmaker.UnitLogic.Parts.SpellImmunityType.Specific;
                        c.m_Exceptions = SpellExceptions.Select(a => a.ToReference<BlueprintAbilityReference>()).Distinct().ToArray();
                    });
                });
                ShieldOfLaw.TemporaryContext(bp => {
                    bp.SetDescription(TTTContext, "A dim blue glow surrounds the subjects, protecting them from attacks, " +
                        "granting them resistance to spells cast by chaotic creatures, and slowing chaotic creatures when they strike the subjects. " +
                        "This abjuration has four effects.\n" +
                        "First, each warded creature gains a +4 deflection bonus to AC and a +4 resistance bonus on saves. " +
                        "Unlike protection from chaos, this benefit applies against all attacks, not just against attacks by chaotic creatures.\n" +
                        "Second, a warded creature gains spell resistance 25 against chaotic spells and spells cast by chaotic creatures.\n" +
                        "Third, the abjuration protects from all mind-affecting spells and effects.\n" +
                        "Finally, if a chaotic creature succeeds on a melee attack against a warded creature, " +
                        "the attacker is slowed (Will save negates, as the slow spell, but against shield of law's save DC)");
                });
                CloakOfChaos.TemporaryContext(bp => {
                    bp.SetDescription(TTTContext, "A random pattern of color surrounds the subjects, protecting them from attacks, " +
                        "granting them resistance to spells cast by lawful creatures, and causing lawful creatures that strike the subjects to become confused. " +
                        "This abjuration has four effects.\n" +
                        "First, each warded creature gains a +4 deflection bonus to AC and a +4 resistance bonus on saves. " +
                        "Unlike protection from law, the benefit of this spell applies against all attacks, not just against attacks by lawful creatures.\n" +
                        "Second, each warded creature gains spell resistance 25 against lawful spells and spells cast by lawful creatures.\n" +
                        "Third, the abjuration protects from all mind-affecting spells and effects.\n" +
                        "Finally, if a lawful creature succeeds on a melee attack against a warded creature, " +
                        "the offending attacker is confused for 1 round (Will save negates, as with the confusion spell, " +
                        "but against the save DC of cloak of chaos).");
                });

                TTTContext.Logger.LogPatch(HolyAuraBuff);
                TTTContext.Logger.LogPatch(UnholyAuraBuff);
                TTTContext.Logger.LogPatch(CloakOfChaosBuff);
                TTTContext.Logger.LogPatch(ShieldOfLawBuff);
            }
            static void PatchRemoveFear() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("RemoveFear")) { return; }

                var RemoveFearBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c5c86809a1c834e42a2eb33133e90a28");
                RemoveFearBuff.RemoveComponents<AddConditionImmunity>();
                QuickFixTools.ReplaceSuppression(RemoveFearBuff, TTTContext);
            }
            static void PatchRemoveSickness() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("RemoveSickness")) { return; }

                var RemoveSicknessBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("91e09b2d99bb71243a97565af8b282e9");
                RemoveSicknessBuff.RemoveComponents<AddConditionImmunity>();
                RemoveSicknessBuff.AddComponent<SuppressBuffsTTT>(c => {
                    c.Descriptor = SpellDescriptor.Sickened | SpellDescriptor.Nauseated;
                });
                TTTContext.Logger.LogPatch("Patched", RemoveSicknessBuff);
            }
            static void PatchShadowEvocation() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("ShadowEvocation")) { return; }

                var ShadowEvocation = BlueprintTools.GetBlueprint<BlueprintAbility>("237427308e48c3341b3d532b9d3a001f");
                ShadowEvocation.AvailableMetamagic |= Metamagic.Empower
                    | Metamagic.Maximize
                    | Metamagic.Quicken
                    | Metamagic.Heighten
                    | Metamagic.Reach
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent
                    | Metamagic.Selective
                    | Metamagic.Bolstered;
                TTTContext.Logger.LogPatch("Patched", ShadowEvocation);
            }
            static void PatchShadowEvocationGreater() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("ShadowEvocationGreater")) { return; }

                var ShadowEvocationGreaterProperty = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("0f813eb338594c5bb840c5583fd29c3d");
                var ShadowEvocationGreater = BlueprintTools.GetBlueprint<BlueprintAbility>("3c4a2d4181482e84d9cd752ef8edc3b6");
                ShadowEvocationGreater.AvailableMetamagic |= Metamagic.Empower
                    | Metamagic.Maximize
                    | Metamagic.Quicken
                    | Metamagic.Heighten
                    | Metamagic.Reach
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent
                    | Metamagic.Selective
                    | Metamagic.Bolstered;
                ShadowEvocationGreaterProperty.BaseValue = 60;
                TTTContext.Logger.LogPatch("Patched", ShadowEvocationGreater);
            }
            static void PatchUnbreakableHeart() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("UnbreakableHeart")) { return; }

                var UnbreakableHeartBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("6603b27034f694e44a407a9cdf77c67e");
                QuickFixTools.ReplaceSuppression(UnbreakableHeartBuff, TTTContext);
            }
            static void PatchWintersGrasp() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("WintersGrasp")) { return; }

                var WinterGrasp = BlueprintTools.GetBlueprint<BlueprintAbility>("406c6e4a631b43ce8f7a77844b75bf75");
                var WinterGraspAreaBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("6eae342c76bc4c6d88c6d864731f6d81");
                var WinterGraspAreaEffect = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("ba4e3f85c4f540efa537b4745ed467a4");
                var WinterGraspDifficultTerrainBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("cb5715ae89974ff98870f13fde9cd27e");

                WinterGrasp.TemporaryContext(bp => {
                    bp.SetDescription(TTTContext, "Ice encrusts the ground, radiating supernatural cold and making it hard for creatures " +
                        "to maintain their balance. This icy ground is treated as difficult terrain and " +
                        "the DC of Mobility checks in the area is increased by 5. " +
                        "A creature that begins its turn in the affected area takes 1d6 points of cold damage and takes a " +
                        "–2 penalty on saving throws against spells with the cold descriptor for 1 round.");
                    bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                    bp.AvailableMetamagic &= ~(Metamagic.Persistent | Metamagic.Selective);
                });
                WinterGraspDifficultTerrainBuff.TemporaryContext(bp => {
                    bp.AddComponent<AddStatBonus>(c => {
                        c.Stat = StatType.SkillMobility;
                        c.Value = -5;
                        c.Descriptor = ModifierDescriptor.Penalty;
                    });
                });
                WinterGraspAreaBuff.TemporaryContext(bp => {
                    bp.Stacking = StackingType.Replace;
                });
                WinterGraspAreaEffect.TemporaryContext(bp => {
                    bp.SetComponents();
                    bp.AddComponent<AbilityAreaEffectRunAction>(c => {
                        c.UnitEnter = Helpers.CreateActionList(
                            new ContextActionApplyBuff() {
                                m_Buff = WinterGraspDifficultTerrainBuff.ToReference<BlueprintBuffReference>(),
                                Permanent = true,
                                DurationValue = new ContextDurationValue() {
                                    DiceCountValue = 0,
                                    BonusValue = 1
                                }
                            },
                            new ContextActionApplyBuff() {
                                m_Buff = WinterGraspAreaBuff.ToReference<BlueprintBuffReference>(),
                                DurationValue = new ContextDurationValue() {
                                    Rate = DurationRate.Rounds,
                                    DiceCountValue = 0,
                                    BonusValue = 1
                                }
                            },
                            new ContextActionDealDamage() {
                                DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Cold
                                },
                                Duration = new ContextDurationValue() {
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                },
                                Value = new ContextDiceValue() {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = 1,
                                    BonusValue = 0
                                }
                            }
                        );
                        c.UnitExit = Helpers.CreateActionList(
                            new ContextActionRemoveBuff() {
                                m_Buff = WinterGraspDifficultTerrainBuff.ToReference<BlueprintBuffReference>()
                            }
                        );
                        c.Round = Helpers.CreateActionList(
                            new ContextActionApplyBuff() {
                                m_Buff = WinterGraspAreaBuff.ToReference<BlueprintBuffReference>(),
                                DurationValue = new ContextDurationValue() {
                                    Rate = DurationRate.Rounds,
                                    DiceCountValue = 0,
                                    BonusValue = 1
                                }
                            },
                            new ContextActionDealDamage() {
                                DamageType = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Cold
                                },
                                Duration = new ContextDurationValue() {
                                    DiceCountValue = new ContextValue(),
                                    BonusValue = new ContextValue()
                                },
                                Value = new ContextDiceValue() {
                                    DiceType = DiceType.D6,
                                    DiceCountValue = 1,
                                    BonusValue = 0
                                }
                            }
                        );
                        c.UnitMove = Helpers.CreateActionList();
                    });
                });

                TTTContext.Logger.LogPatch(WinterGrasp);
                TTTContext.Logger.LogPatch(WinterGraspAreaBuff);
                TTTContext.Logger.LogPatch(WinterGraspAreaEffect);
                TTTContext.Logger.LogPatch(WinterGraspDifficultTerrainBuff);
            }
            static void PatchWrackingRay() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("WrackingRay")) { return; }

                var WrackingRay = BlueprintTools.GetBlueprint<BlueprintAbility>("1cde0691195feae45bab5b83ea3f221e");
                foreach (AbilityEffectRunAction component in WrackingRay.GetComponents<AbilityEffectRunAction>()) {
                    foreach (ContextActionDealDamage action in component.Actions.Actions.OfType<ContextActionDealDamage>()) {
                        action.Value.DiceType = DiceType.D4;
                    }
                }
                TTTContext.Logger.LogPatch("Patched", WrackingRay);
            }
            static void PatchFromSpellFlags() {
                if (Main.TTTContext.Fixes.Spells.IsDisabled("FixSpellFlags")) { return; }

                var DemonicRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("36ca5ecd8e755a34f8da6b42ad4c965f");
                var FreedomOfMovementBuff_FD = BlueprintTools.GetBlueprint<BlueprintBuff>("60906dd9e4ddec14c8ac9a0f4e47f54c");

                TTTContext.Logger.Log("Updating Spell Flags");
                SpellTools.SpellList.AllSpellLists
                    .SelectMany(list => list.SpellsByLevel)
                    .SelectMany(level => level.Spells)
                    .Distinct()
                    .SelectMany(spell => spell.AbilityAndVariants())
                    .SelectMany(spell => spell.AbilityAndStickyTouch())
                    .OrderBy(spell => spell.Name)
                    .SelectMany(a => a.FlattenAllActions())
                    .OfType<ContextActionApplyBuff>()
                    .Distinct()
                    .Select(a => a.Buff)
                    .Distinct()
                    .OrderBy(buff => buff.name)
                    .ForEach(buff => {
                        if (buff.GetComponent<AddCondition>() == null
                        && buff.GetComponent<BuffStatusCondition>() == null
                        && buff.GetComponent<BuffPoisonStatDamage>() == null
                        && buff.AssetGuid != DemonicRageBuff.AssetGuid
                        && (buff.SpellDescriptor & SpellDescriptor.Bleed) == 0) {
                            if ((buff.m_Flags & BlueprintBuff.Flags.IsFromSpell) == 0) {
                                buff.m_Flags |= BlueprintBuff.Flags.IsFromSpell;
                                TTTContext.Logger.LogPatch("Patched", buff);
                            }
                        }
                    });
                FreedomOfMovementBuff_FD.m_Flags |= BlueprintBuff.Flags.IsFromSpell;
                TTTContext.Logger.LogPatch("Patched", FreedomOfMovementBuff_FD);
                TTTContext.Logger.Log("Finished Spell Flags");
            }
        }
    }
}