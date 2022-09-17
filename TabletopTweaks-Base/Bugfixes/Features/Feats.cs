using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.NewRules;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Features {
    class Feats {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                TTTContext.Logger.LogHeader("Patching Feats");
                PatchAlliedSpellcaster();
                PatchArcaneStrike();
                PatchBrewPotions();
                PatchCleave();
                PatchCraneWing();
                PatchDestructiveDispel();
                PatchDestructiveDispelPrerequisites();
                PatchDispelSynergy();
                PatchEndurance();
                PatchFencingGrace();
                PatchIndomitableMount();
                PatchMountedCombat();
                PatchPersistantMetamagic();
                PatchBolsteredMetamagic();
                PatchEmpowerMetamagic();
                PatchMaximizeMetamagic();
                PatchShatterDefenses();
                PatchSlashingGrace();
                PatchSpellSpecialization();
                PatchSpiritedCharge();
                PatchWeaponFinesse();
                PatchMagicalTail();
                PatchLunge();
                PatchOutflank();
                PatchSiezeTheMoment();
                PatchSelectiveMetamagic();
                PatchSelectiveMetamagicPrerequisites();
            }

            static void PatchAlliedSpellcaster() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("AlliedSpellcaster")) { return; }

                var AlliedSpellcaster = BlueprintTools.GetBlueprint<BlueprintFeature>("9093ceeefe9b84746a5993d619d7c86f");
                AlliedSpellcaster.RemoveComponents<AlliedSpellcaster>();
                AlliedSpellcaster.AddComponent<AlliedSpellcasterTTT>(c => {
                    c.m_AlliedSpellcasterFact = AlliedSpellcaster.ToReference<BlueprintUnitFactReference>();
                    c.Radius = 5;
                });

                TTTContext.Logger.LogPatch("Patched", AlliedSpellcaster);
            }
            static void PatchArcaneStrike() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("ArcaneStrike")) { return; }

                var ArcaneStrikeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("98ac795afd1b2014eb9fdf2b9820808f");
                var DragonicStrikeAcid = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("b7d0c5d733a06e543b35d4e1c88d04f7");
                var DragonicStrikeCold = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("3ff9333e578d7c2448ba18b6ffacb885");
                var DragonicStrikeElectricity = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("db64de9cdbad45b4cbc8b369cd7a007c");
                var DragonicStrikeFire = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("42aee53be1b64334caa5c73d34d10fad");

                ArcaneStrikeBuff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                AddDraconicTrigger(ArcaneStrikeBuff, DragonicStrikeAcid, DamageEnergyType.Acid);
                AddDraconicTrigger(ArcaneStrikeBuff, DragonicStrikeCold, DamageEnergyType.Cold);
                AddDraconicTrigger(ArcaneStrikeBuff, DragonicStrikeElectricity, DamageEnergyType.Electricity);
                AddDraconicTrigger(ArcaneStrikeBuff, DragonicStrikeFire, DamageEnergyType.Fire);

                TTTContext.Logger.LogPatch("Patched", ArcaneStrikeBuff);

                void AddDraconicTrigger(BlueprintBuff arcaneStrike, BlueprintUnitFactReference draconic, DamageEnergyType type) {
                    arcaneStrike.AddComponent<AdditionalDiceOnAttack>(c => {
                        c.OnHit = true;
                        c.InitiatorConditions = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                new ContextConditionHasFact(){
                                    m_Fact = draconic
                                }
                            }
                        };
                        c.TargetConditions = new ConditionsChecker();
                        c.Value = new ContextDiceValue() {
                            DiceType = DiceType.D4,
                            DiceCountValue = 1,
                            BonusValue = 0
                        };
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Energy,
                            Energy = type
                        };
                    });
                }
            }
            static void PatchBrewPotions() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("BrewPotions")) { return; }

                var BrewPotions = BlueprintTools.GetBlueprint<BlueprintFeature>("c0f8c4e513eb493408b8070a1de93fc0");
                BrewPotions.Groups = new FeatureGroup[] { FeatureGroup.Feat };

                TTTContext.Logger.LogPatch("Patched", BrewPotions);
            }
            static void PatchCleave() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("Cleave")) { return; }

                var CleaveAction = BlueprintTools.GetBlueprint<BlueprintAbility>("6447d104a2222c14d9c9b8a36e4eb242");
                var PowerAttackFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("9972f33f977fc724c838e59641b2fca5");
                var CleaveFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d809b6c4ff2aaff4fa70d712a70f7d7b");
                var GreatCleaveFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("cc9c862ef2e03af4f89be5088851ea35");
                var CleaveMythicFeature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(TTTContext, "CleaveMythicFeature");
                var CleavingFinish = BlueprintTools.GetBlueprint<BlueprintFeature>("59bd93899149fa44687ff4121389b3a9");
                var ImprovedCleavingFinish = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("ffa1b373190af4f4db7a5501904a1983");
                var CleavingFinishCooldown = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("d868546877b2de24388fbdd5741d0c95");

                CleaveAction.RemoveComponents<AbilityCustomCleave>();
                CleaveAction.AddComponent<AbilityCustomCleaveTTT>(c => {
                    c.m_GreaterFeature = GreatCleaveFeature;
                    c.m_MythicFeature = CleaveMythicFeature;
                });
                CleavingFinish.TemporaryContext(bp => {
                    bp.SetComponents();
                    bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                        c.OnlyHit = true;
                        c.CheckWeaponRangeType = true;
                        c.RangeType = WeaponRangeType.Melee;
                        c.ReduceHPToZero = true;
                        c.Action = Helpers.CreateActionList(
                            new Conditional() {
                                ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionCasterHasFact(){
                                            m_Fact = CleavingFinishCooldown.Get().ToReference<BlueprintUnitFactReference>()
                                        }
                                    }
                                },
                                IfTrue = Helpers.CreateActionList(),
                                IfFalse = Helpers.CreateActionList(
                                    new ContextActionCleaveAttack() {
                                        ExtraAttack = true,
                                        m_MythicFeature = CleaveMythicFeature
                                    },
                                    new Conditional() {
                                        ConditionsChecker = new ConditionsChecker() {
                                            Conditions = new Condition[] {
                                                new ContextConditionCasterHasFact(){
                                                    m_Fact = ImprovedCleavingFinish
                                                }
                                            }
                                        },
                                        IfTrue = Helpers.CreateActionList(),
                                        IfFalse = Helpers.CreateActionList(
                                            new ContextActionApplyBuff() {
                                                m_Buff = CleavingFinishCooldown,
                                                DurationValue = new ContextDurationValue() {
                                                    Rate = DurationRate.Rounds,
                                                    DiceCountValue = 0,
                                                    BonusValue = 1
                                                },
                                                ToCaster = true
                                            }
                                        )
                                    }
                                )
                            }
                        );
                    });
                    bp.AddPrerequisiteFeature(CleaveFeature);
                    bp.AddPrerequisiteFeature(PowerAttackFeature);
                    bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                        c.Stat = StatType.Strength;
                        c.Value = 13;
                    });
                    bp.AddComponent<PureRecommendation>(c => {
                        c.Priority = RecommendationPriority.Good;
                    });
                    bp.AddComponent<FeatureTagsComponent>(c => {
                        c.FeatureTags = FeatureTag.Attack | FeatureTag.Damage | FeatureTag.Melee;
                    });
                });
                /*
                CleavingFinish
                    .GetComponent<AddInitiatorAttackWithWeaponTrigger>(c => c.ReduceHPToZero == true)?
                    .Action
                    .Actions
                    .OfType<Conditional>()
                    .FirstOrDefault()?
                    .TemporaryContext(conditional => {
                        conditional.IfTrue.Actions = conditional.IfTrue.Actions.Where(a => !(a is ContextActionMeleeAttack)).ToArray();
                        conditional.AddActionIfTrue(Helpers.Create<ContextActionCleaveAttack>(a => {
                            a.ExtraAttack = true;
                            a.m_MythicFeature = CleaveMythicFeature;
                        }));
                    });
                */
                TTTContext.Logger.LogPatch(CleaveAction);
                TTTContext.Logger.LogPatch(CleavingFinish);
            }
            static void PatchDestructiveDispel() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("DestructiveDispel")) { return; }

                var DestructiveDispel = BlueprintTools.GetBlueprint<BlueprintFeature>("d298e64e14398e848a54db5a2619ba42");
                var Sickened = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4e42460798665fd4cb9173ffa7ada323");
                var Stunned = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("09d39b38bb7c6014394b6daced9bacd3");

                DestructiveDispel.SetComponents();
                DestructiveDispel.AddComponent<DestructiveDispelComponent>(c => {
                    c.SaveSuccees = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = Sickened,
                            DurationValue = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            }
                        }
                    );
                    c.SaveFailed = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = Stunned,
                            DurationValue = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            }
                        }
                    );
                });

                TTTContext.Logger.LogPatch("Patched", DestructiveDispel);
            }
            static void PatchDestructiveDispelPrerequisites() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("DestructiveDispelPrerequisites")) { return; }

                var DestructiveDispel = BlueprintTools.GetBlueprint<BlueprintFeature>("d298e64e14398e848a54db5a2619ba42");
                var DispelMagic = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("92681f181b507b34ea87018e8f7a528a");
                var DispelMagicGreater = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("f0f761b808dc4b149b08eaf44b99f633");

                DestructiveDispel.RemoveComponents<Prerequisite>();
                DestructiveDispel.TemporaryContext(bp => {
                    bp.AddPrerequisite<PrerequisiteSpellKnown>(p => {
                        p.m_Spell = DispelMagic;
                        p.Group = Prerequisite.GroupType.Any;
                    });
                    bp.AddPrerequisite<PrerequisiteSpellKnown>(p => {
                        p.m_Spell = DispelMagicGreater;
                        p.Group = Prerequisite.GroupType.Any;
                    });
                    bp.AddPrerequisite<PrerequisiteCasterLevel>(p => {
                        p.RequiredCasterLevel = 11;
                        p.Group = Prerequisite.GroupType.All;
                    });
                });
            }
            static void PatchDispelSynergy() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("DispelSynergy")) { return; }

                var DispelSynergy = BlueprintTools.GetBlueprint<BlueprintFeature>("f3e3e29608ba07844ab3cafc4c8e4343");

                DispelSynergy.RemoveComponents<Prerequisite>();
                DispelSynergy.AddPrerequisite<PrerequisiteStatValue>(p => {
                    p.Stat = StatType.SkillKnowledgeArcana;
                    p.Value = 5;
                });
            }
            static void PatchMagicalTail() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("MagicalTail")) { return; }

                BlueprintFeature magicalTail1 = BlueprintTools.GetBlueprint<BlueprintFeature>("5114829572da5a04f896a8c5b67be413");
                BlueprintFeature magicalTail2 = BlueprintTools.GetBlueprint<BlueprintFeature>("c032f65c0bd9f6048a927fb07fc0195d"); // Abilities change for this one
                BlueprintFeature magicalTail3 = BlueprintTools.GetBlueprint<BlueprintFeature>("d5050e13742d9b64da20921aaf7c2b2a");
                BlueprintFeature magicalTail4 = BlueprintTools.GetBlueprint<BlueprintFeature>("342b6aed6b2eaab4786de243f0bcbcb8");
                BlueprintFeature magicalTail5 = BlueprintTools.GetBlueprint<BlueprintFeature>("044cd84818c36854abf61064ade542a1"); // Abilites change for this one
                BlueprintFeature magicalTail6 = BlueprintTools.GetBlueprint<BlueprintFeature>("053e37697a0d20547b06c3dbd8b71702");
                BlueprintFeature magicalTail7 = BlueprintTools.GetBlueprint<BlueprintFeature>("041f91c25586d48469dce6b4575053f6");
                BlueprintFeature magicalTail8 = BlueprintTools.GetBlueprint<BlueprintFeature>("df186ef345849d149bdbf4ddb45aee35");

                var hideousLaughterKitsune = BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "HideousLaughterKitsune");
                var heroismKitsune = BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "HeroismKitsune");

                var magicalTailDescription = "You gain a new {g|Encyclopedia:Special_Abilities}spell-like ability{/g}, each usable twice per day," +
                                             " from the following list, in order:\n1. vanish\n2. hideous laughter\n3. blur\n4. invisibility\n5. heroism\n6." +
                                             " displacement\n7. confusion\n8. dominate person.\nFor example, the first time you select this {g|Encyclopedia:Feat}feat{/g}," +
                                             " you gain vanish 2/day; the second time you select this feat, you gain hideous laughter 2/day. Your {g|Encyclopedia:Caster_Level}caster level{/g}" +
                                             " for these {g|Encyclopedia:Spell}spells{/g} is equal to your {g|Encyclopedia:Hit_Dice}Hit Dice{/g}. The DCs for these abilities are" +
                                             " {g|Encyclopedia:Charisma}Charisma{/g}-based.\nYou may select this feat up to eight times. Each time you take it, you gain an additional ability as described above.";

                magicalTail1.SetDescription(TTTContext, magicalTailDescription);
                magicalTail2.SetDescription(TTTContext, magicalTailDescription);
                magicalTail3.SetDescription(TTTContext, magicalTailDescription);
                magicalTail4.SetDescription(TTTContext, magicalTailDescription);
                magicalTail5.SetDescription(TTTContext, magicalTailDescription);
                magicalTail6.SetDescription(TTTContext, magicalTailDescription);
                magicalTail7.SetDescription(TTTContext, magicalTailDescription);
                magicalTail8.SetDescription(TTTContext, magicalTailDescription);

                BlueprintFeature v = new BlueprintFeature();

                magicalTail2.RemoveComponents<AddFacts>();

                magicalTail2.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                    hideousLaughterKitsune.ToReference<BlueprintUnitFactReference>()
                };
                });

                magicalTail5.RemoveComponents<AddFacts>();

                magicalTail5.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                    heroismKitsune.ToReference<BlueprintUnitFactReference>()
                };
                });

                TTTContext.Logger.LogPatch("Patched", magicalTail1);
                TTTContext.Logger.LogPatch("Patched", magicalTail2);
                TTTContext.Logger.LogPatch("Patched", magicalTail3);
                TTTContext.Logger.LogPatch("Patched", magicalTail4);
                TTTContext.Logger.LogPatch("Patched", magicalTail5);
                TTTContext.Logger.LogPatch("Patched", magicalTail6);
                TTTContext.Logger.LogPatch("Patched", magicalTail7);
                TTTContext.Logger.LogPatch("Patched", magicalTail8);
            }
            static void PatchCraneWing() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("CraneWing")) { return; }

                BlueprintBuff CraneStyleBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("e8ea7bd10136195478d8a5fc5a44c7da");
                var FightingDefensivlyTrigger = CraneStyleBuff.GetComponent<AddInitiatorAttackWithWeaponTrigger>();
                var Conditionals = FightingDefensivlyTrigger.Action.Actions.OfType<Conditional>();

                var newConditonal = Helpers.Create<ContextConditionHasFreeHand>();
                Conditionals.First().ConditionsChecker.Conditions = Conditionals.First().ConditionsChecker.Conditions.AddItem(newConditonal).ToArray();

                TTTContext.Logger.LogPatch("Patched", CraneStyleBuff);
            }
            static void PatchFencingGrace() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("FencingGrace")) { return; }

                var FencingGrace = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("47b352ea0f73c354aba777945760b441");
                FencingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceTTT>());
                TTTContext.Logger.LogPatch("Patched", FencingGrace);
            }
            static void PatchSlashingGrace() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("SlashingGrace")) { return; }

                var SlashingGrace = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("697d64669eb2c0543abb9c9b07998a38");
                SlashingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceTTT>());
                TTTContext.Logger.LogPatch("Patched", SlashingGrace);
            }
            static void PatchEndurance() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("Endurance")) { return; }
                var Endurance = BlueprintTools.GetBlueprint<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174");
                Endurance.SetDescription(TTTContext, "Harsh conditions or long exertions do not easily tire you.\nBenefit: You gain +4 bonus on Fortitude " +
                    "saves against fatigue and exhaustion and +2 " +
                    "bonus on Athletics checks. If you have 10 or more ranks in Athletics, the bonus increases to +4 for that skill." +
                    "\nYou may sleep in light or medium armor without becoming fatigued.");
                Endurance.RemoveComponents<AddStatBonus>();
                Endurance.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SkillAthletics;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus,
                        Value = 2
                    };
                });
                Endurance.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.BaseStat;
                    c.m_Stat = StatType.SkillAthletics;
                    c.m_Progression = ContextRankProgression.Custom;
                    c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] {
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 9,
                            ProgressionValue = 2
                        },
                        new ContextRankConfig.CustomProgressionItem() {
                            BaseValue = 100,
                            ProgressionValue = 4
                        }
                    };
                    c.m_StepLevel = 3;
                    c.m_Min = 10;
                    c.m_Max = 20;
                });
                TTTContext.Logger.LogPatch("Patched", Endurance);
            }
            static void PatchMountedCombat() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("MountedCombat")) { return; }

                var MountedCombatBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5008df9965da43c593c98ed7e6cacfc6");
                var MountedCombatCooldownBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5c9ef8224acdbab4fbaf59c710d0ef23");
                var TrickRiding = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "TrickRiding");
                var TrickRidingCooldownBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5c9ef8224acdbab4fbaf59c710d0ef23");
                MountedCombatBuff.RemoveComponents<MountedCombat>();
                MountedCombatBuff.RemoveComponents<MountedCombatTTT>();
                MountedCombatBuff.AddComponent<MountedCombatTTT>(c => {
                    c.m_CooldownBuff = MountedCombatCooldownBuff.ToReference<BlueprintBuffReference>();
                    c.m_TrickRidingCooldownBuff = TrickRidingCooldownBuff.ToReference<BlueprintBuffReference>();
                    c.m_TrickRidingFeature = TrickRiding.ToReference<BlueprintFeatureReference>();
                });
                TTTContext.Logger.LogPatch("Patched", MountedCombatBuff);
            }
            static void PatchIndomitableMount() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("IndomitableMount")) { return; }

                var IndomitableMount = BlueprintTools.GetBlueprint<BlueprintFeature>("68e814f1f3ce55942a52c1dd536eaa5b");
                var IndomitableMountCooldownBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("34762bab68ec86c45a15884b9a9929fc");
                IndomitableMount.RemoveComponents<IndomitableMount>();
                IndomitableMount.AddComponent<IndomitableMountTTT>(c => {
                    c.m_CooldownBuff = IndomitableMountCooldownBuff.ToReference<BlueprintBuffReference>();
                });
                TTTContext.Logger.LogPatch("Patched", IndomitableMount);
            }
            static void PatchOutflank() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("Outflank")) { return; }

                var Outflank = BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11");
                Outflank.RemoveComponents<OutflankProvokeAttack>();
                Outflank.AddComponent<OutflankProvokeAttackTTT>(c => {
                    c.m_OutflankFact = Outflank.ToReference<BlueprintUnitFactReference>();
                });
            }
            static void PatchSiezeTheMoment() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("SiezeTheMoment")) { return; }

                var SiezeTheMoment = BlueprintTools.GetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760");
                SiezeTheMoment.RemoveComponents<SiezeTheMoment>();
                SiezeTheMoment.AddComponent<SiezeTheMomentTTT>(c => {
                    c.m_SiezeTheMomentFact = SiezeTheMoment.ToReference<BlueprintUnitFactReference>();
                });
                TTTContext.Logger.LogPatch(SiezeTheMoment);
            }
            static void PatchPersistantMetamagic() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("PersistantMetamagic")) { return; }

                var PersistentSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
                var spells = SpellTools.GetAllSpells();
                TTTContext.Logger.LogPatch("Enabling", PersistentSpellFeat);
                foreach (var spell in spells) {
                    bool HasSavingThrow = spell.AbilityAndVariants()
                        .SelectMany(s => s.AbilityAndStickyTouch())
                        .Where(s => s != null)
                        .SelectMany(s => s.FlattenAllActions())
                        .OfType<ContextActionSavingThrow>().Any()
                            ||
                        spell.AbilityAndVariants()
                            .SelectMany(s => s.AbilityAndStickyTouch())
                            .Where(s => s != null)
                            .SelectMany(s => s.FlattenAllActions())
                            .OfType<ContextActionConditionalSaved>().Any();
                    if ((spell?.GetComponent<AbilityEffectRunAction>()?.SavingThrowType ?? SavingThrowType.Unknown) != SavingThrowType.Unknown
                        || spell.AbilityAndVariants()
                            .SelectMany(s => s.AbilityAndStickyTouch())
                            .Where(s => s != null)
                            .Any(s => (s.GetComponent<AbilityEffectRunAction>()?.SavingThrowType ?? SavingThrowType.Unknown) != SavingThrowType.Unknown)
                        || HasSavingThrow) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Persistent)) {
                            spell.AvailableMetamagic |= Metamagic.Persistent;
                            TTTContext.Logger.LogPatch("Enabled Persistant Metamagic", spell);
                        }
                    };
                }
            }
            static void PatchBolsteredMetamagic() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("BolsteredMetamagic")) { return; }

                var BolsteredSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("fbf5d9ce931f47f3a0c818b3f8ef8414");
                var spells = SpellTools.GetAllSpells();
                TTTContext.Logger.LogPatch("Enabling", BolsteredSpellFeat);
                foreach (var spell in spells) {
                    bool dealsDamage = spell.FlattenAllActions()
                        .OfType<ContextActionDealDamage>().Any()
                        || (spell?.GetComponent<AbilityEffectStickyTouch>()?
                        .TouchDeliveryAbility?
                        .FlattenAllActions()?
                        .OfType<ContextActionDealDamage>()?
                        .Any() ?? false);
                    if (dealsDamage) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Bolstered)) {
                            spell.AvailableMetamagic |= Metamagic.Bolstered;
                            TTTContext.Logger.LogPatch("Enabled Bolstered Metamagic", spell);
                        }
                    };
                }
            }
            static void PatchEmpowerMetamagic() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("EmpowerMetamagic")) { return; }

                var EmpowerSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e");
                var spells = SpellTools.GetAllSpells();
                TTTContext.Logger.LogPatch("Enabling", EmpowerSpellFeat);
                foreach (var spell in spells) {
                    bool dealsDamage = spell.FlattenAllActions()
                        .OfType<ContextActionDealDamage>().Any()
                        || (spell?.GetComponent<AbilityEffectStickyTouch>()?
                        .TouchDeliveryAbility?
                        .FlattenAllActions()?
                        .OfType<ContextActionDealDamage>()?
                        .Any() ?? false);
                    if (dealsDamage) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Empower)) {
                            spell.AvailableMetamagic |= Metamagic.Empower;
                            TTTContext.Logger.LogPatch("Enabled Empower Metamagic", spell);
                        }
                    };
                }
            }
            static void PatchMaximizeMetamagic() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("MaximizeMetamagic")) { return; }

                var MaximizeSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b");
                var spells = SpellTools.GetAllSpells();
                TTTContext.Logger.LogPatch("Enabling", MaximizeSpellFeat);
                foreach (var spell in spells) {
                    bool dealsDamage = spell.FlattenAllActions()
                        .OfType<ContextActionDealDamage>().Any()
                        || (spell?.GetComponent<AbilityEffectStickyTouch>()?
                        .TouchDeliveryAbility?
                        .FlattenAllActions()?
                        .OfType<ContextActionDealDamage>()?
                        .Any() ?? false);
                    if (dealsDamage) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Maximize)) {
                            spell.AvailableMetamagic |= Metamagic.Maximize;
                            TTTContext.Logger.LogPatch("Enabled Maximize Metamagic", spell);
                        }
                    };
                }
            }
            static void PatchSelectiveMetamagic() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("SelectiveMetamagic")) { return; }

                var SelectiveSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
                var spells = SpellTools.GetAllSpells();
                TTTContext.Logger.LogPatch("Updating", SelectiveSpellFeat);
                foreach (var spell in spells) {
                    bool isAoE = spell.AbilityAndVariants().Any(v => v.GetComponent<AbilityTargetsAround>());
                    isAoE |= spell.AbilityAndVariants().Any(v => v.GetComponent<AbilityDeliverProjectile>()?.Type == AbilityProjectileType.Cone
                        || v.GetComponent<AbilityDeliverProjectile>()?.Type == AbilityProjectileType.Line);
                    if (isAoE) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Selective)) {
                            spell.AvailableMetamagic |= Metamagic.Selective;
                            TTTContext.Logger.LogPatch("Enabled Selective Metamagic", spell);
                        }
                    } else {
                        if (spell.AvailableMetamagic.HasMetamagic(Metamagic.Selective)) {
                            spell.AvailableMetamagic &= ~Metamagic.Selective;
                            TTTContext.Logger.LogPatch("Disabled Selective Metamagic", spell);
                        }
                    }
                }
            }
            static void PatchShatterDefenses() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("ShatterDefenses")) { return; }

                var ShatterDefenses = BlueprintTools.GetBlueprint<BlueprintFeature>("61a17ccbbb3d79445b0926347ec07577");
                var ShatterDefensesBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(TTTContext, "ShatterDefensesBuff");
                var ShatterDefensesMythicFeat = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "ShatterDefensesMythicFeat");
                var ShatterDefensesMythicBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(TTTContext, "ShatterDefensesMythicBuff");

                ShatterDefenses.RemoveComponents<AddMechanicsFeature>();
                ShatterDefenses.RemoveComponents<AddFacts>();
                ShatterDefenses.AddComponent<ShatterDefensesInitiator>(c => {
                    c.Action = Helpers.CreateActionList(
                        new Conditional {
                            ConditionsChecker = new ConditionsChecker {
                                Conditions = new Condition[] {
                                    new ContextConditionHasCondition() {
                                        Conditions = new Kingmaker.UnitLogic.UnitCondition[]{
                                            Kingmaker.UnitLogic.UnitCondition.Shaken,
                                            Kingmaker.UnitLogic.UnitCondition.Frightened
                                        }
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new Conditional {
                                    ConditionsChecker = new ConditionsChecker {
                                        Conditions = new Condition[] {
                                            new ContextConditionCasterHasFact {
                                                m_Fact = ShatterDefensesMythicFeat.ToReference<BlueprintUnitFactReference>(),
                                            }
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(a => {
                                            a.m_Buff = ShatterDefensesMythicBuff.ToReference<BlueprintBuffReference>();
                                            a.DurationValue = new ContextDurationValue() {
                                                m_IsExtendable = false,
                                                Rate = DurationRate.Rounds,
                                                DiceCountValue = 0,
                                                BonusValue = 2
                                            };
                                        })
                                    ),
                                    IfFalse = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(a => {
                                            a.m_Buff = ShatterDefensesBuff.ToReference<BlueprintBuffReference>();
                                            a.DurationValue = new ContextDurationValue() {
                                                m_IsExtendable = false,
                                                Rate = DurationRate.Rounds,
                                                DiceCountValue = 0,
                                                BonusValue = 2
                                            };
                                        })
                                    ),
                                }
                            ),
                            IfFalse = Helpers.CreateActionList(),
                        }
                    );
                });
                TTTContext.Logger.LogPatch("Patched", ShatterDefenses);
            }
            static void PatchSpellSpecialization() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("SpellSpecialization")) { return; }

                var SpellSpecializationProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("fe9220cdc16e5f444a84d85d5fa8e3d5");

                Game.Instance.BlueprintRoot.Progression.CharacterClasses.ForEach(characterClass => {
                    SpellSpecializationProgression.AddClass(characterClass);
                });
                //SpellSpecializationProgression.AddClass(LoremasterClass);
                TTTContext.Logger.LogPatch("Patched", SpellSpecializationProgression);
            }
            static void PatchSpiritedCharge() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("SpiritedCharge")) { return; }

                var ChargeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f36da144a379d534cad8e21667079066");
                var MountedBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
                var SpiritedCharge = BlueprintTools.GetBlueprint<BlueprintFeature>("95ef0ff14771f2549897f300ce62c95c");
                var SpiritedChargeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5a191fc6731bd4845bbbcc8ff3ff4c1d");

                SpiritedCharge.RemoveComponents<BuffExtraEffects>();
                SpiritedCharge.AddComponent<BuffExtraEffectsRequirements>(c => {
                    c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                    c.CheckFacts = true;
                    c.CheckedFacts = new BlueprintUnitFactReference[] { MountedBuff };
                    c.ExtraEffectBuff = SpiritedChargeBuff.ToReference<BlueprintBuffReference>();
                });
                SpiritedChargeBuff.RemoveComponents<OutcomingDamageAndHealingModifier>();
                SpiritedChargeBuff.AddComponent<AddOutgoingWeaponDamageBonus>(c => {
                    c.BonusDamageMultiplier = 1;
                    c.RemoveAfterTrigger = true;
                });
                TTTContext.Logger.LogPatch("Patched", SpiritedCharge);
                TTTContext.Logger.LogPatch("Patched", SpiritedChargeBuff);
            }
            static void PatchWeaponFinesse() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("WeaponFinesse")) { return; }

                var WeaponFinesse = BlueprintTools.GetBlueprint<BlueprintFeature>("90e54424d682d104ab36436bd527af09");

                WeaponFinesse.ReplaceComponents<AttackStatReplacement>(Helpers.Create<AttackStatReplacementTTT>(c => {
                    c.ReplacementStat = StatType.Dexterity;
                    c.SubCategory = WeaponSubCategory.Finessable;
                }));
                TTTContext.Logger.LogPatch("Patched", WeaponFinesse);
            }
            static void PatchLunge() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("Lunge")) { return; }

                var LungeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d41d5bd9a775d7245929256d58a3e03e");

                LungeFeature.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                FeatTools.AddAsFeat(LungeFeature);
                TTTContext.Logger.LogPatch("Patched", LungeFeature);
            }
            static void PatchSelectiveMetamagicPrerequisites() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("SelectivePrerequisites")) { return; }

                var SelectiveSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2");
                SelectiveSpellFeat.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillKnowledgeArcana;
                    c.Value = 10;
                });
                TTTContext.Logger.LogPatch("Patched", SelectiveSpellFeat);
            }
        }

        [HarmonyPatch(typeof(MetamagicHelper), "GetBolsteredAreaEffectUnits", new Type[] { typeof(TargetWrapper) })]
        static class MetamagicHelper_GetBolsteredAreaEffectUnits_Patch {
            static void Postfix(TargetWrapper origin, ref List<UnitEntityData> __result) {
                __result = __result.Where(unit => unit.AttackFactions.IsPlayerEnemy).ToList();
            }
        }

        [HarmonyPatch]
        static class VitalStrike_OnEventDidTrigger_Rowdy_Patch {
            private static Type _type = typeof(AbilityCustomVitalStrike).GetNestedType("<Deliver>d__7", AccessTools.all);
            internal static MethodInfo TargetMethod(Harmony instance) {
                return AccessTools.Method(_type, "MoveNext");
            }

            static readonly MethodInfo AbilityCustomVitalStrike_get_RowdyFeature = AccessTools.PropertyGetter(
                typeof(AbilityCustomVitalStrike),
                "RowdyFeature"
            );
            static readonly ConstructorInfo VitalStrikeEventHandler_Constructor = AccessTools.Constructor(
                typeof(VitalStrikeEventHandler),
                new Type[] {
                    typeof(UnitEntityData),
                    typeof(int),
                    typeof(bool),
                    typeof(bool),
                    typeof(EntityFact)
                }
            );
            // ------------before------------
            // eventHandlers.Add(new AbilityCustomVitalStrike.VitalStrike(maybeCaster, this.VitalStrikeMod, maybeCaster.HasFact(this.MythicBlueprint), maybeCaster.HasFact(this.RowdyFeature)));
            // ------------after-------------
            // eventHandlers.Add(new VitalStrikeEventHandler(maybeCaster, this.VitalStrikeMod, maybeCaster.HasFact(this.MythicBlueprint), maybeCaster.HasFact(this.RowdyFeature)));
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                var codes = new List<CodeInstruction>(instructions);
                if (Main.TTTContext.Fixes.Feats.IsDisabled("VitalStrike")) { return instructions; }
                int target = FindInsertionTarget(codes);
                //TTTContext.Logger.Log($"OpperandType: {codes[71].operand.GetType()}");
                //Utilities.ILUtils.LogIL(codes);
                codes[target] = new CodeInstruction(OpCodes.Newobj, VitalStrikeEventHandler_Constructor);
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                //Looking for the arguments that define the object creation because searching for the object creation itself is hard
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == OpCodes.Call && codes[i].Calls(AbilityCustomVitalStrike_get_RowdyFeature)) {
                        if (codes[i + 6].opcode == OpCodes.Newobj) {
                            return i + 6;
                        }
                    }
                }
                TTTContext.Logger.Log("VITALSTRIKEPATCH: COULD NOT FIND TARGET");
                return -1;
            }

            private class VitalStrikeEventHandler : IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
                IRulebookHandler<RuleCalculateWeaponStats>,
                IInitiatorRulebookHandler<RulePrepareDamage>,
                IRulebookHandler<RulePrepareDamage>,
                IInitiatorRulebookHandler<RuleAttackWithWeapon>,
                IRulebookHandler<RuleAttackWithWeapon>,
                ISubscriber, IInitiatorRulebookSubscriber {

                public VitalStrikeEventHandler(UnitEntityData unit, int damageMod, bool mythic, bool rowdy, EntityFact fact) {
                    this.m_Unit = unit;
                    this.m_DamageMod = damageMod;
                    this.m_Mythic = mythic;
                    this.m_Rowdy = rowdy;
                    this.m_Fact = fact;
                }

                public UnitEntityData GetSubscribingUnit() {
                    return this.m_Unit;
                }

                public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
                }

                public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
                    DamageDescription damageDescription = evt.DamageDescription.FirstItem();
                    if (damageDescription != null && damageDescription.TypeDescription.Type == DamageType.Physical) {
                        if (!evt.DoNotScaleDamage 
                                && (evt.WeaponDamageDice.HasModifications 
                                    || !evt.Weapon.Blueprint.IsDamageDiceOverridden 
                                    || (evt.Initiator.IsPlayerFaction && !evt.Initiator.Body.IsPolymorphed) 
                                    || evt.IsDefaultUnit)) 
                        {
                            DiceFormula diceFormula = WeaponDamageScaleTable.Scale(evt.WeaponDamageDice.ModifiedValue, evt.WeaponSize, Size.Medium, evt.Weapon.Blueprint);
                            if (diceFormula != evt.WeaponDamageDice.ModifiedValue) {
                                evt.WeaponDamageDice.Modify(diceFormula, ModifierDescriptor.Size);
                            }
                        }
                        var vitalDamage = CalculateVitalDamage(evt);
                        //new DamageDescription() {
                        //Dice = new DiceFormula(damageDescription.Dice.Rolls * Math.Max(1, this.m_DamageMod - 1), damageDescription.Mo.Dice.Dice),
                        //Bonus = this.m_Mythic ? damageDescription.Bonus * Math.Max(1, this.m_DamageMod - 1) : 0,
                        //TypeDescription = damageDescription.TypeDescription,
                        //IgnoreReduction = damageDescription.IgnoreReduction,
                        //IgnoreImmunities = damageDescription.IgnoreImmunities,
                        //SourceFact = this.m_Fact,
                        //CausedByCheckFail = damageDescription.CausedByCheckFail,
                        //m_BonusWithSource = 0
                        //};
                        evt.DamageDescription.Insert(1, vitalDamage);
                    }
                }

                private DamageDescription CalculateVitalDamage(RuleCalculateWeaponStats evt) {
                    var WeaponDice = new ModifiableDiceFormula(evt.WeaponDamageDice.ModifiedValue);
                    WeaponDice.Modify(new DiceFormula(WeaponDice.ModifiedValue.Rolls * Math.Max(1, this.m_DamageMod - 1), WeaponDice.ModifiedValue.Dice), m_Fact);

                    DamageDescription damageDescriptor = evt.Weapon.Blueprint.DamageType.GetDamageDescriptor(WeaponDice, evt.Initiator.Stats.AdditionalDamage.BaseValue);
                    damageDescriptor.TemporaryContext(dd => {
                        dd.TypeDescription.Physical.Enhancement = evt.Enhancement;
                        dd.TypeDescription.Physical.EnhancementTotal = evt.EnhancementTotal + evt.Weapon.EnchantmentValue;
                        if (this.m_Mythic) {
                            dd.AddModifier(new Modifier(evt.DamageDescription.FirstItem().Bonus * Math.Max(1, this.m_DamageMod - 1), evt.Initiator.GetFact(m_Fact.Blueprint.GetComponent<AbilityCustomVitalStrike>().MythicBlueprint), ModifierDescriptor.UntypedStackable));
                        }
                        dd.TypeDescription.Common.Alignment = evt.Alignment;
                        dd.SourceFact = m_Fact;
                    });
                    return damageDescriptor;
                }

                public void OnEventAboutToTrigger(RuleAttackWithWeapon evt) {
                }

                //For Ranged - Handling of damage calcs does not occur the same due to projectiles
                public void OnEventDidTrigger(RuleAttackWithWeapon evt) {
                    if (!m_Rowdy) { return; }
                    var RowdyFact = evt.Initiator.GetFact(m_Fact.Blueprint.GetComponent<AbilityCustomVitalStrike>().RowdyFeature);
                    RuleAttackRoll ruleAttackRoll = evt.AttackRoll;
                    if (ruleAttackRoll == null) { return; }
                    if (evt.Initiator.Stats.SneakAttack < 1) { return; }
                    if (!ruleAttackRoll.TargetUseFortification) {
                        var FortificationCheck = Rulebook.Trigger<RuleFortificationCheck>(new RuleFortificationCheck(ruleAttackRoll));
                        if (FortificationCheck.UseFortification) {
                            ruleAttackRoll.FortificationChance = FortificationCheck.FortificationChance;
                            ruleAttackRoll.FortificationRoll = FortificationCheck.Roll;
                        }
                    }
                    if (!ruleAttackRoll.TargetUseFortification || ruleAttackRoll.FortificationOvercomed) {
                        DamageTypeDescription damageTypeDescription = evt.ResolveRules
                            .Select(e => e.Damage).First()
                            .DamageBundle.First<BaseDamage>().CreateTypeDescription();
                        var rowdyDice = new ModifiableDiceFormula(new DiceFormula(evt.Initiator.Stats.SneakAttack * 2, DiceType.D6));
                        var RowdyDamage = damageTypeDescription.GetDamageDescriptor(rowdyDice, 0);
                        RowdyDamage.SourceFact = RowdyFact;
                        BaseDamage baseDamage = RowdyDamage.CreateDamage();
                        baseDamage.Precision = true;
                        evt.ResolveRules.Select(e => e.Damage)
                            .ForEach(e => e.Add(baseDamage));
                    }
                }

                //For Melee
                public void OnEventAboutToTrigger(RulePrepareDamage evt) {
                    if (!m_Rowdy) { return; }
                    var RowdyFact = evt.Initiator.GetFact(m_Fact.Blueprint.GetComponent<AbilityCustomVitalStrike>().RowdyFeature);
                    RuleAttackRoll ruleAttackRoll = evt.ParentRule.AttackRoll;
                    if (ruleAttackRoll == null) { return; }
                    if (evt.Initiator.Stats.SneakAttack < 1) { return; }
                    if (!ruleAttackRoll.TargetUseFortification) {
                        var FortificationCheck = Rulebook.Trigger<RuleFortificationCheck>(new RuleFortificationCheck(ruleAttackRoll));
                        if (FortificationCheck.UseFortification) {
                            ruleAttackRoll.FortificationChance = FortificationCheck.FortificationChance;
                            ruleAttackRoll.FortificationRoll = FortificationCheck.Roll;
                        }
                    }
                    if (!ruleAttackRoll.TargetUseFortification || ruleAttackRoll.FortificationOvercomed) {
                        DamageTypeDescription damageTypeDescription = evt.DamageBundle
                            .First()
                            .CreateTypeDescription();
                        var rowdyDice = new ModifiableDiceFormula(new DiceFormula(evt.Initiator.Stats.SneakAttack * 2, DiceType.D6));
                        var RowdyDamage = damageTypeDescription.GetDamageDescriptor(rowdyDice, 0);
                        RowdyDamage.SourceFact = RowdyFact;
                        BaseDamage baseDamage = RowdyDamage.CreateDamage();
                        baseDamage.Precision = true;
                        evt.Add(baseDamage);
                    }
                }

                public void OnEventDidTrigger(RulePrepareDamage evt) {
                }

                private readonly UnitEntityData m_Unit;
                private readonly EntityFact m_Fact;
                private int m_DamageMod;
                private bool m_Mythic;
                private bool m_Rowdy;
            }
        }
    }
}
