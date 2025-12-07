using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
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
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Features {
    class Feats {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPostfix]
            static void UpdateFeats() {
                if (Initialized) return;
                Initialized = true;

                TTTContext.Logger.LogHeader("Patching Feats");
                PatchAlliedSpellcaster();
                PatchArcaneStrike();
                PatchArmorFocus();
                PatchShieldFocus();
                PatchBrewPotions();
                PatchCleave();
                PatchCraneWing();
                PatchDestructiveDispel();
                PatchDestructiveDispelPrerequisites();
                PatchDispelSynergy();
                PatchEndurance();
                PatchEnergizedWildShape();
                PatchFencingGrace();
                PatchFrightfulShape();
                PatchIndomitableMount();
                PatchMountedCombat();
                PatchNaturalSpell();
                PatchRakingClaws();
                PatchShatterDefenses();
                PatchShiftersEdge();
                PatchShifterRush();
                PatchSlashingGrace();
                PatchSpellBane();
                PatchSpellSpecialization();
                PatchSpiritedCharge();
                PatchWeaponFinesse();
                PatchMagicalTail();
                PatchLunge();
                PatchSiezeTheMoment();
                PatchSelectiveMetamagicPrerequisites();
            }
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            [PatchBlueprintsCacheInitPostfix]
            static void UpdateMetamagic() {
                PatchExtendMetamagic();
                PatchPersistantMetamagic();
                PatchBolsteredMetamagic();
                PatchEmpowerMetamagic();
                PatchMaximizeMetamagic();
                PatchSelectiveMetamagic();
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
            static void PatchArmorFocus() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("ArmorFocus")) { return; }

                var ArmorFocusBanded = BlueprintTools.GetBlueprint<BlueprintFeature>("57c6a8b9ca2f3b1489e9defe8b121055");
                var ArmorFocusBreastplate = BlueprintTools.GetBlueprint<BlueprintFeature>("619f035282c82994895d67e337fe150a");
                var ArmorFocusChainmail = BlueprintTools.GetBlueprint<BlueprintFeature>("adb1548368313094ba608788befff12c");
                var ArmorFocusChainshirt = BlueprintTools.GetBlueprint<BlueprintFeature>("8fa6c8751d4548540809b045f9a65dc0");
                var ArmorFocusFullplate = BlueprintTools.GetBlueprint<BlueprintFeature>("e1a220a4cf2111d4884ab2372946909b");
                var ArmorFocusHalfplate = BlueprintTools.GetBlueprint<BlueprintFeature>("ade9beb95f41d9d4b94e1537dbaf44d6");
                var ArmorFocusHeavy = BlueprintTools.GetBlueprint<BlueprintFeature>("c27e6d2b0d33d42439f512c6d9a6a601");
                var ArmorFocusHide = BlueprintTools.GetBlueprint<BlueprintFeature>("e31e66c4670152945969e719112709d8");
                var ArmorFocusLeather = BlueprintTools.GetBlueprint<BlueprintFeature>("8c5a2c385181eb64a8b86f0bf751d96f");
                var ArmorFocusLight = BlueprintTools.GetBlueprint<BlueprintFeature>("3bc6e1d2b44b5bb4d92e6ba59577cf62");
                var ArmorFocusMedium = BlueprintTools.GetBlueprint<BlueprintFeature>("7dc004879037638489b64d5016997d12");
                var ArmorFocusPadded = BlueprintTools.GetBlueprint<BlueprintFeature>("82fbb68796a4e6d4a8b79cf3f14600b7");
                var ArmorFocusScalemail = BlueprintTools.GetBlueprint<BlueprintFeature>("b1cccd1b5fec8a6438858cb39c08a7f6");
                var ArmorFocusStudded = BlueprintTools.GetBlueprint<BlueprintFeature>("57770bba6c22f1e42b396f2bcb1c420a");

                ArmorFocusLight.TemporaryContext(bp => {
                    bp.RemoveComponents<ArmorFocus>();
                    bp.AddComponent<AddArmorACModifier>(c => {
                        c.Descriptor = ModifierDescriptor.ArmorFocus;
                        c.Value = 1;
                        c.CheckArmorType = true;
                        c.ArmorTypes = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Light };
                    });
                });
                ArmorFocusMedium.TemporaryContext(bp => {
                    bp.RemoveComponents<ArmorFocus>();
                    bp.AddComponent<AddArmorACModifier>(c => {
                        c.Descriptor = ModifierDescriptor.ArmorFocus;
                        c.Value = 1;
                        c.CheckArmorType = true;
                        c.ArmorTypes = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Medium };
                    });
                });
                ArmorFocusHeavy.TemporaryContext(bp => {
                    bp.RemoveComponents<ArmorFocus>();
                    bp.AddComponent<AddArmorACModifier>(c => {
                        c.Descriptor = ModifierDescriptor.ArmorFocus;
                        c.Value = 1;
                        c.CheckArmorType = true;
                        c.ArmorTypes = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy };
                    });
                });
            }
            static void PatchShieldFocus() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("ShieldFocus")) { return; }

                var ShieldFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("ac57069b6bf8c904086171683992a92a");
                var ShieldFocusGreater = BlueprintTools.GetBlueprint<BlueprintFeature>("afd05ca5363036c44817c071189b67e1");
                var DLC3_ShieldAAttackBlueprintFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e30c63d0aef044c4b604b49fab0c3bdd");

                ShieldFocus.TemporaryContext(bp => {
                    bp.RemoveComponents<ArmorFocus>();
                    bp.RemoveComponents<ShieldFocus>();
                    bp.AddComponent<AddArmorACModifier>(c => {
                        c.Descriptor = ModifierDescriptor.ShieldFocus;
                        c.Value = 1;
                        c.IsShield = true;
                        c.CheckArmorType = true;
                        c.ArmorTypes = new ArmorProficiencyGroup[] {
                            ArmorProficiencyGroup.HeavyShield,
                            ArmorProficiencyGroup.LightShield,
                            ArmorProficiencyGroup.TowerShield,
                            ArmorProficiencyGroup.Buckler,
                        };
                    });
                });
                ShieldFocusGreater.TemporaryContext(bp => {
                    bp.RemoveComponents<ArmorFocus>();
                    bp.RemoveComponents<ShieldFocus>();
                    bp.AddComponent<AddArmorACModifier>(c => {
                        c.Descriptor = ModifierDescriptor.ShieldFocus;
                        c.Value = 1;
                        c.IsShield = true;
                        c.CheckArmorType = true;
                        c.ArmorTypes = new ArmorProficiencyGroup[] {
                            ArmorProficiencyGroup.HeavyShield,
                            ArmorProficiencyGroup.LightShield,
                            ArmorProficiencyGroup.TowerShield,
                            ArmorProficiencyGroup.Buckler,
                        };
                    });
                });
                DLC3_ShieldAAttackBlueprintFeature.TemporaryContext(bp => {
                    bp.RemoveComponents<ShieldFocus>();
                    bp.AddComponent<AddArmorACModifier>(c => {
                        c.Descriptor = ModifierDescriptor.ShieldFocus;
                        c.Value = 1;
                        c.IsShield = true;
                        c.CheckArmorType = true;
                        c.ArmorTypes = new ArmorProficiencyGroup[] {
                            ArmorProficiencyGroup.HeavyShield,
                            ArmorProficiencyGroup.LightShield,
                            ArmorProficiencyGroup.TowerShield,
                            ArmorProficiencyGroup.Buckler,
                        };
                    });
                });
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
                                    },
                                    new ContextActionCleaveAttack() {
                                        ExtraAttack = true,
                                        m_MythicFeature = CleaveMythicFeature
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
            static void PatchSpellBane() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("SpellBane")) { return; }

                var SpellBane = BlueprintTools.GetBlueprint<BlueprintFeature>("d2d1b1f27bdf4ddfa5bf8b7244786ff9");
                var AeonBaneFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("0b25e8d8b0488c84c9b5714e9ca0a204");
                var AeonBaneBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("345160619fc2ddc44b8ad98c94dde448");

                SpellBane.TemporaryContext(bp => {
                    bp.GetComponent<OwnerAbilityTargetSavingThrowBonus>().TemporaryContext(c => {
                        c.Conditions.TemporaryContext(checker => {
                            checker.Operation = Operation.Or;
                            checker.Conditions = checker.Conditions.AppendToArray(
                                new ContextConditionCasterHasFact() {
                                    m_Fact = AeonBaneBuff
                                }
                            );
                        });
                    });
                    bp.AddPrerequisiteFeature(AeonBaneFeature, Prerequisite.GroupType.Any);
                });

                TTTContext.Logger.LogPatch("Patched", SpellBane);
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
            static void PatchEnergizedWildShape() {
                PatchDamageTriggers();
                PatchPrerequisites();

                static void PatchPrerequisites() {
                    if (Main.TTTContext.Fixes.Feats.IsDisabled("EnergizedWildShapePrerequisites")) { return; }

                    var EnergizedWildShapeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("92df031ed2cb4153950853d6a3b9813e");

                    var WildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("19bb148cb92db224abb431642d10efeb");
                    var MajorFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6e843ca5ae8e41aea17458fb4c16a15d");
                    var FeralChampnionWildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1b60050091002ad458bd49788e84f13a");
                    var GriffonheartShifterGriffonShapeFakeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1d3656c3090e48f59888d86ff7014acc");
                    var ShifterWildShapeFeyFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("24a4fb8991344fd5beb2a1a1a517da87");
                    var ShifterDragonFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d8e9d249a426400bb47fefa6d0158049");
                    var ShifterWildShapeManticoreFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("719be33c87f94ed58414ba3eb5a4b664");

                    EnergizedWildShapeFeature.TemporaryContext(bp => {
                        bp.RemoveComponents<PrerequisiteCondition>();
                        bp.AddPrerequisiteFeaturesFromList(1,
                            WildShapeIWolfFeature,
                            FeralChampnionWildShapeIWolfFeature,
                            MajorFormFeature,
                            ShifterDragonFormFeature,
                            GriffonheartShifterGriffonShapeFakeFeature,
                            ShifterWildShapeFeyFeatureLevelUp,
                            ShifterWildShapeManticoreFeatureLevelUp
                        );
                    });
                    TTTContext.Logger.LogPatch(EnergizedWildShapeFeature);
                }
                static void PatchDamageTriggers() {
                    if (Main.TTTContext.Fixes.Feats.IsDisabled("EnergizedWildShapeDamage")) { return; }

                    var EnergizedDamageFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d808863c4bd44fd8bd9cf5892460705d");
                    var EnergizedWildShapeAcidBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("d5cad83598c84964b9e743c3b485b1c6");
                    var EnergizedWildShapeColdBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("e66bff2197724a508111f26cb8071a31");
                    var EnergizedWildShapeElectricityBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("9cdacfc7e8a940fea80312ba3f1a02b8");
                    var EnergizedWildShapeFireBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("5d2155c13f9842b2be8196edc82ef057");

                    EnergizedDamageFeature.TemporaryContext(bp => {
                        bp.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                        bp.AddComponent<AddAdditionalWeaponDamage>(c => {
                            c.CheckFacts = true;
                            c.m_Facts = new BlueprintUnitFactReference[] {
                            EnergizedWildShapeAcidBuff
                        };
                            c.CheckWeaponGroup = true;
                            c.Group = WeaponFighterGroup.Natural;
                            c.Value = new ContextDiceValue() {
                                DiceType = DiceType.D6,
                                DiceCountValue = 1,
                                BonusValue = 0
                            };
                            c.DamageType = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Energy = DamageEnergyType.Acid
                            };
                        });
                        bp.AddComponent<AddAdditionalWeaponDamage>(c => {
                            c.CheckFacts = true;
                            c.m_Facts = new BlueprintUnitFactReference[] {
                            EnergizedWildShapeColdBuff
                        };
                            c.CheckWeaponGroup = true;
                            c.Group = WeaponFighterGroup.Natural;
                            c.Value = new ContextDiceValue() {
                                DiceType = DiceType.D6,
                                DiceCountValue = 1,
                                BonusValue = 0
                            };
                            c.DamageType = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Energy = DamageEnergyType.Cold
                            };
                        });
                        bp.AddComponent<AddAdditionalWeaponDamage>(c => {
                            c.CheckFacts = true;
                            c.m_Facts = new BlueprintUnitFactReference[] {
                            EnergizedWildShapeElectricityBuff
                        };
                            c.CheckWeaponGroup = true;
                            c.Group = WeaponFighterGroup.Natural;
                            c.Value = new ContextDiceValue() {
                                DiceType = DiceType.D6,
                                DiceCountValue = 1,
                                BonusValue = 0
                            };
                            c.DamageType = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Energy = DamageEnergyType.Electricity
                            };
                        });
                        bp.AddComponent<AddAdditionalWeaponDamage>(c => {
                            c.CheckFacts = true;
                            c.m_Facts = new BlueprintUnitFactReference[] {
                                EnergizedWildShapeFireBuff
                            };
                            c.CheckWeaponGroup = true;
                            c.Group = WeaponFighterGroup.Natural;
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
                    });
                    TTTContext.Logger.LogPatch(EnergizedDamageFeature);
                }
            }
            static void PatchFrightfulShape() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("FrightfulShape")) { return; }

                var FrightfulShape = BlueprintTools.GetBlueprint<BlueprintFeature>("8e8a34c754d649aa9286fe8ee5cc3f10");

                var WildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("19bb148cb92db224abb431642d10efeb");
                var MajorFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6e843ca5ae8e41aea17458fb4c16a15d");
                var FeralChampnionWildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1b60050091002ad458bd49788e84f13a");
                var GriffonheartShifterGriffonShapeFakeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1d3656c3090e48f59888d86ff7014acc");
                var ShifterWildShapeFeyFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("24a4fb8991344fd5beb2a1a1a517da87");
                var ShifterDragonFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d8e9d249a426400bb47fefa6d0158049");
                var ShifterWildShapeManticoreFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("719be33c87f94ed58414ba3eb5a4b664");

                FrightfulShape.TemporaryContext(bp => {
                    bp.RemoveComponents<PrerequisiteCondition>();
                    bp.AddPrerequisiteFeaturesFromList(1,
                        WildShapeIWolfFeature,
                        FeralChampnionWildShapeIWolfFeature,
                        MajorFormFeature,
                        ShifterDragonFormFeature,
                        GriffonheartShifterGriffonShapeFakeFeature,
                        ShifterWildShapeFeyFeatureLevelUp,
                        ShifterWildShapeManticoreFeatureLevelUp
                    );
                });
                TTTContext.Logger.LogPatch(FrightfulShape);
            }
            static void PatchRakingClaws() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("RakingClaws")) { return; }

                var RakingClawsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("a1b262d2b1ef478994113fc941fa3a32");

                var WildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("19bb148cb92db224abb431642d10efeb");
                var MajorFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6e843ca5ae8e41aea17458fb4c16a15d");
                var FeralChampnionWildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1b60050091002ad458bd49788e84f13a");
                var GriffonheartShifterGriffonShapeFakeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1d3656c3090e48f59888d86ff7014acc");
                var ShifterWildShapeFeyFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("24a4fb8991344fd5beb2a1a1a517da87");
                var ShifterDragonFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d8e9d249a426400bb47fefa6d0158049");
                var ShifterWildShapeManticoreFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("719be33c87f94ed58414ba3eb5a4b664");

                RakingClawsFeature.TemporaryContext(bp => {
                    bp.RemoveComponents<PrerequisiteCondition>();
                    bp.AddPrerequisiteFeaturesFromList(1,
                        WildShapeIWolfFeature,
                        FeralChampnionWildShapeIWolfFeature,
                        MajorFormFeature,
                        ShifterDragonFormFeature,
                        GriffonheartShifterGriffonShapeFakeFeature,
                        ShifterWildShapeFeyFeatureLevelUp,
                        ShifterWildShapeManticoreFeatureLevelUp
                    );
                });
                TTTContext.Logger.LogPatch(RakingClawsFeature);
            }
            static void PatchShiftersEdge() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("ShiftersEdge")) { return; }

                var ShiftersEdgeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("0e7ec9a341ca46fcaf4d49759e047c83");

                ShiftersEdgeFeature.TemporaryContext(bp => {
                    bp.GetComponent<AddFactContextActions>().Disabled = true;
                    bp.AddComponent<ShiftersEdgeComponent>(c => {
                        c.DamageBonus = new ContextValue() {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageBonus
                        };
                        c.Descriptor = ModifierDescriptor.UntypedStackable;
                    });
                    bp.AddContextRankConfig(c => {
                        c.m_Type = AbilityRankType.DamageBonus;
                        c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                        c.m_Progression = ContextRankProgression.Div2;
                        c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.ShifterClass };
                    });
                });


                TTTContext.Logger.LogPatch(ShiftersEdgeFeature);
            }
            static void PatchShifterRush() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("ShifterRush")) { return; }

                var ShiftersRushFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("4ddc88f422a84f76a952e24bec7b53e1");

                var WildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("19bb148cb92db224abb431642d10efeb");
                var MajorFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6e843ca5ae8e41aea17458fb4c16a15d");
                var FeralChampnionWildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1b60050091002ad458bd49788e84f13a");
                var GriffonheartShifterGriffonShapeFakeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1d3656c3090e48f59888d86ff7014acc");
                var ShifterWildShapeFeyFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("24a4fb8991344fd5beb2a1a1a517da87");
                var ShifterDragonFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d8e9d249a426400bb47fefa6d0158049");
                var ShifterWildShapeManticoreFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("719be33c87f94ed58414ba3eb5a4b664");

                ShiftersRushFeature.TemporaryContext(bp => {
                    bp.RemoveComponents<PrerequisiteCondition>();
                    bp.AddPrerequisiteFeaturesFromList(1,
                        WildShapeIWolfFeature,
                        FeralChampnionWildShapeIWolfFeature,
                        MajorFormFeature,
                        ShifterDragonFormFeature,
                        GriffonheartShifterGriffonShapeFakeFeature,
                        ShifterWildShapeFeyFeatureLevelUp,
                        ShifterWildShapeManticoreFeatureLevelUp
                    );
                });
                TTTContext.Logger.LogPatch(ShiftersRushFeature);
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
            static void PatchNaturalSpell() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("NaturalSpell")) { return; }

                var NaturalSpell = BlueprintTools.GetBlueprint<BlueprintFeature>("c806103e27cce6f429e5bf47067966cf");

                var WildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("19bb148cb92db224abb431642d10efeb");
                var MajorFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6e843ca5ae8e41aea17458fb4c16a15d");
                var FeralChampnionWildShapeIWolfFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1b60050091002ad458bd49788e84f13a");
                var GriffonheartShifterGriffonShapeFakeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1d3656c3090e48f59888d86ff7014acc");
                var ShifterWildShapeFeyFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("24a4fb8991344fd5beb2a1a1a517da87");
                var ShifterDragonFormFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d8e9d249a426400bb47fefa6d0158049");
                var ShifterWildShapeManticoreFeatureLevelUp = BlueprintTools.GetBlueprint<BlueprintFeature>("719be33c87f94ed58414ba3eb5a4b664");

                NaturalSpell.TemporaryContext(bp => {
                    bp.RemoveComponents<PrerequisiteFeature>();
                    bp.AddPrerequisiteFeaturesFromList(1,
                        WildShapeIWolfFeature,
                        FeralChampnionWildShapeIWolfFeature,
                        MajorFormFeature,
                        ShifterDragonFormFeature,
                        GriffonheartShifterGriffonShapeFakeFeature,
                        ShifterWildShapeFeyFeatureLevelUp,
                        ShifterWildShapeManticoreFeatureLevelUp
                    );
                });
                TTTContext.Logger.LogPatch(NaturalSpell);
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
            static void PatchExtendMetamagic() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("ExtendMetamagic")) { return; }

                var ExtendSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("f180e72e4a9cbaa4da8be9bc958132ef");
                var spells = SpellTools.GetAllSpells()
                    .SelectMany(s => s.AbilityAndVariants())
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .ToArray();
                TTTContext.Logger.LogPatch("Enabling", ExtendSpellFeat);
                foreach (var spell in spells) {
                    bool appliesBuff = spell.AbilityAndVariants()
                        .SelectMany(s => s.AbilityAndStickyTouch())
                        .Where(s => s != null)
                        .SelectMany(s => s.FlattenAllActions())
                        .OfType<ContextActionApplyBuff>().Any(c => c?.DurationValue?.IsExtendable ?? false)
                            ||
                        spell.AbilityAndVariants()
                            .SelectMany(s => s.AbilityAndStickyTouch())
                            .Where(s => s != null)
                            .SelectMany(s => s.FlattenAllActions())
                            .OfType<ContextActionApplyBuff>().Any(c => c?.DurationValue?.IsExtendable ?? false);
                    if (appliesBuff) {
                        if (!spell.AvailableMetamagic.HasMetamagic(Metamagic.Extend)) {
                            spell.AvailableMetamagic |= Metamagic.Extend;
                            TTTContext.Logger.LogPatch("Enabled Extend Metamagic", spell);
                        }
                    };
                }
            }
            static void PatchPersistantMetamagic() {
                if (Main.TTTContext.Fixes.Feats.IsDisabled("PersistantMetamagic")) { return; }

                var PersistentSpellFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("cd26b9fa3f734461a0fcedc81cafaaac");
                var spells = SpellTools.GetAllSpells()
                    .SelectMany(s => s.AbilityAndVariants())
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .ToArray();
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
                var spells = SpellTools.GetAllSpells()
                    .SelectMany(s => s.AbilityAndVariants())
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .ToArray();
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
                var spells = SpellTools.GetAllSpells()
                    .SelectMany(s => s.AbilityAndVariants())
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .ToArray();
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
                var spells = SpellTools.GetAllSpells()
                    .SelectMany(s => s.AbilityAndVariants())
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .ToArray();
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
                var spells = SpellTools.GetAllSpells()
                    .SelectMany(s => s.AbilityAndVariants())
                    .SelectMany(s => s.AbilityAndStickyTouch())
                    .ToArray();
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

            static readonly MethodInfo MechanicsContext_TriggerRule = AccessTools.Method(
                typeof(MechanicsContext),
                "TriggerRule",
                generics: new Type[] { typeof(RuleAttackWithWeapon) }
            );
            static readonly MethodInfo RuleAttackWithWeapon_IsFirstAttack_Set = AccessTools.PropertySetter(
                typeof(RuleAttackWithWeapon),
                "IsFirstAttack"
            );
            // ------------before------------
            // context.TriggerRule<RuleAttackWithWeapon>(ruleAttackWithWeapon);
            // ------------after-------------
            // ruleAttackWithWeapon.FirstAttack = true;
            // context.TriggerRule<RuleAttackWithWeapon>(ruleAttackWithWeapon);
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                var codes = new List<CodeInstruction>(instructions);
                if (Main.TTTContext.Fixes.Feats.IsDisabled("VitalStrike")) { return instructions; }
                int target = FindInsertionTarget(codes);
                //TTTContext.Logger.Log($"OpperandType: {codes[71].operand.GetType()}");
                //ILUtils.LogIL(TTTContext, codes);
                var load = codes[target - 1];
                codes.InsertRange(target, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Call, RuleAttackWithWeapon_IsFirstAttack_Set),
                    load.Clone()
                });
                //ILUtils.LogIL(TTTContext, codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                //Looking for the arguments that define the object creation because searching for the object creation itself is hard
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].Calls(MechanicsContext_TriggerRule)) {
                        return i;
                    }
                }
                TTTContext.Logger.Log("VITALSTRIKEPATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
