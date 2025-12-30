using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
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
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Items {
    static class Weapons {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                TTTContext.Logger.LogHeader("Patching Weapons");

                PatchBladeOfTheMerciful();
                PatchDevastatingBlowFromAbove();
                PatchEyeForAnEye();
                PatchFinnean();
                PatchHonorableJudgement();
                PatchRadiance();
                PatchTerribleTremble();
                PatchSoundOfTheVoid();
                PatchMusicOfDeath();

                PatchThunderingBurst();
                PatchElementalBurst();
                PatchVorpal();

                PatchNaturalWeapons();
                PatchSai();

                void PatchBladeOfTheMerciful() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("BladeOfTheMerciful")) { return; }

                    var BladeOfTheMercifulEnchant = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("a5e3fe4a71e331e4aa41f9a07cfd3729");
                    BladeOfTheMercifulEnchant.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    BladeOfTheMercifulEnchant.AddComponent<WeaponConditionalDamageDice>(c => {
                        c.Damage = new DamageDescription() {
                            Dice = new DiceFormula() {
                                m_Dice = DiceType.D6,
                                m_Rolls = 2
                            },
                            TypeDescription = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Energy = DamageEnergyType.Fire
                            },
                        };
                        c.Conditions = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                new ContextConditionAlignment(){
                                    Alignment = AlignmentComponent.Good,
                                    Not = true
                                }
                            }
                        };
                    });
                    TTTContext.Logger.LogPatch("Patched", BladeOfTheMercifulEnchant);
                }
                void PatchDevastatingBlowFromAbove() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("DevastatingBlowFromAbove")) { return; }

                    var MountedBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
                    var DevastatingBlowFromAboveEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("1684be60cc3f8fd46b7aec30dc03e227");
                    DevastatingBlowFromAboveEnchantment.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    DevastatingBlowFromAboveEnchantment.AddComponent<WeaponConditionalDamageDice>(c => {
                        c.Damage = new DamageDescription() {
                            Dice = new DiceFormula() {
                                m_Dice = DiceType.D4,
                                m_Rolls = 1
                            },
                            TypeDescription = new DamageTypeDescription() {
                                Type = DamageType.Physical,
                                Physical = new PhysicalData() {
                                    Form = PhysicalDamageForm.Piercing
                                },
                                Common = new CommomData()
                            },
                        };
                        c.Conditions = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                new ContextConditionCasterHasFact(){
                                    m_Fact = MountedBuff
                                }
                            }
                        };
                    });
                    TTTContext.Logger.LogPatch("Patched", DevastatingBlowFromAboveEnchantment);
                }
                void PatchEyeForAnEye() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("EyeForAnEye")) { return; }

                    var EyeForAnEyeBowEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("5f8e3638fc9c6794c8eb6eb671356d52");
                    EyeForAnEyeBowEnchantment.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    EyeForAnEyeBowEnchantment.RemoveComponents<AdditionalDiceOnAttack>();
                    EyeForAnEyeBowEnchantment.AddComponent<WeaponExtraDamageDice>(c => {
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Physical,
                            Physical = new PhysicalData() {
                                Form = PhysicalDamageForm.Piercing,
                                Enhancement = 3,
                                EnhancementTotal = 3
                            }
                        };
                        c.Value = new DiceFormula() {
                            m_Dice = DiceType.D12,
                            m_Rolls = 1
                        };
                    });
                    TTTContext.Logger.LogPatch(EyeForAnEyeBowEnchantment);
                }
                void PatchHonorableJudgement() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("HonorableJudgement")) { return; }

                    var JudgementOfRuleItem = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("f40895a7dfab41c40b42657fc3f5bdfe");
                    var JudgementOfRuleSecondItem = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("ca0e81e14d675c34b862aad509be573d");
                    var JudgementOfRuleEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("74a8dc2f9ce6ced4fa211c20fa4def32");
                    JudgementOfRuleEnchantment.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    JudgementOfRuleEnchantment.RemoveComponents<AdditionalDiceOnAttack>();
                    JudgementOfRuleEnchantment.AddComponent<WeaponConditionalDamageDice>(c => {
                        c.Damage = new DamageDescription() {
                            Dice = new DiceFormula() {
                                m_Dice = DiceType.D6,
                                m_Rolls = 1
                            },
                            TypeDescription = new DamageTypeDescription() {
                                Type = DamageType.Energy,
                                Energy = DamageEnergyType.Holy
                            },
                        };
                        c.Conditions = new ConditionsChecker() {
                            Conditions = new Condition[] {
                                new ContextConditionAlignment(){
                                    Alignment = AlignmentComponent.Chaotic
                                },
                                new ContextConditionAlignment(){
                                    Alignment = AlignmentComponent.Lawful,
                                    CheckCaster = true
                                }
                            }
                        };
                    });
                    TTTContext.Logger.LogPatch("Patched", JudgementOfRuleEnchantment);
                }
                void PatchFinnean() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("Finnean")) { return; }

                    var FinneanChapter3Enchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("b183bd491793d194c9e4c96cd11769b1");
                    var FinneanChapter5EnchantmentBase = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("6b66e949f348ccd4989a5fd9254f8958");
                    var FinneanChapter5EnchantmentLich = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("9aa9af4b654662945a410644d3db8d99");

                    FinneanChapter3Enchantment.RemoveComponents<AdditionalDiceOnAttack>();
                    FinneanChapter3Enchantment.AddComponent<WeaponExtraDamageDice>(c => {
                        c.Value = new DiceFormula(1, DiceType.D6);
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Force
                        };
                    });

                    TTTContext.Logger.LogPatch("Patched", FinneanChapter3Enchantment);
                }
                void PatchTerribleTremble() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("TerrifyingTremble")) { return; }

                    var TerrifyingTrembleItem = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("8c31891423c4405393741e829aebec85");
                    var Enhancement5 = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("bdba267e951851449af552aa9f9e3992");
                    var Ultrasound = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("582849db96824254ebcc68f0b7484e51");
                    var TerrifyingTrembleEnchant_TTT = BlueprintTools.GetModBlueprint<BlueprintWeaponEnchantment>(TTTContext, "TerrifyingTrembleEnchant_TTT");

                    TerrifyingTrembleItem.SetDescription(TTTContext, "Whenever the wielder of this +5 ultrasound earthbreaker lands a killing blow, he deals sonic damage equal to his ranks in " +
                        "the Athletics skill to all enemies within 10 feet. Successful Reflex save (DC 30) halves the damage.");

                    TerrifyingTrembleItem.m_Enchantments = new BlueprintWeaponEnchantmentReference[] {
                        Enhancement5.ToReference<BlueprintWeaponEnchantmentReference>(),
                        Ultrasound.ToReference<BlueprintWeaponEnchantmentReference>(),
                        TerrifyingTrembleEnchant_TTT.ToReference<BlueprintWeaponEnchantmentReference>(),
                    };

                    TTTContext.Logger.LogPatch("Patched", TerrifyingTrembleItem);
                }
                void PatchSoundOfTheVoid() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("SoundOfTheVoid")) { return; }

                    var SoundOfVoidEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("69df5e137a08d9b4ead5d87bf4d5d0ac");
                    var SoundOfVoidBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4fefa9f7fd3f57743bcbec63b10121d8");

                    SoundOfVoidEnchantment.TemporaryContext(bp => {
                        bp.SetComponents();
                        bp.AddComponent<WeaponAttackTrigger>(c => {
                            c.OnlyHit = true;
                            c.OnlyFlatFooted = true;
                            c.Action = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    m_Buff = SoundOfVoidBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        DiceType = DiceType.D4,
                                        DiceCountValue = 1,
                                        BonusValue = 0
                                    }
                                }
                            );
                        });
                    });
                    SoundOfVoidBuff.TemporaryContext(bp => {
                        bp.RemoveComponents<AddSpellResistance>();
                        bp.AddComponent<AddSpellResistancePenaltyTTT>(c => {
                            c.Penalty = 10000;
                        });
                    });

                    TTTContext.Logger.LogPatch(SoundOfVoidBuff);
                }
                void PatchMusicOfDeath() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("MusicOfDeath")) { return; }

                    var MusicOfDeathEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("183a4d2cc996c6f4db8641bed4b3b0c1");

                    MusicOfDeathEnchantment.TemporaryContext(bp => {
                        bp.SetComponents();
                        bp.AddComponent<WeaponConditionalDamageDiceTTT>(c => {
                            c.OnlyHit = true;
                            c.OnlyFlatFooted = true;
                            c.Damage = new DamageDescription() {
                                Dice = new DiceFormula(2, DiceType.D6),
                                Bonus = 0,
                                TypeDescription = new DamageTypeDescription() {
                                    Type = DamageType.Energy,
                                    Energy = DamageEnergyType.Sonic
                                }
                            };
                        });
                    });

                    TTTContext.Logger.LogPatch(MusicOfDeathEnchantment);
                }
                void PatchRadiance() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("Radiance")) { return; }

                    var RadianceEffectBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("0c03ba5e0c3fd304eb0a221e83f4ce1d");
                    RadianceEffectBuff.RemoveComponents<SpellPenetrationBonus>();
                    RadianceEffectBuff.AddComponent<AddSpellResistance>(c => {
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    });

                    TTTContext.Logger.LogPatch("Patched", RadianceEffectBuff);
                }
                void PatchThunderingBurst() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("ThunderingBurst")) { return; }

                    var ThunderingBurst = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("83bd616525288b34a8f34976b2759ea1");
                    ThunderingBurst.GetComponent<WeaponEnergyBurst>().Dice = DiceType.D10;

                    TTTContext.Logger.LogPatch("Patched", ThunderingBurst);
                }
                void PatchElementalBurst() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("ElementalBurst")) { return; }

                    var Shock = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("7bda5277d36ad114f9f9fd21d0dab658");
                    var Flaming = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("30f90becaaac51f41bf56641966c4121");
                    var Frost = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("421e54078b7719d40915ce0672511d0b");

                    var ShockingBurst = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("914d7ee77fb09d846924ca08bccee0ff");
                    var FlamingBurst = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("30f90becaaac51f41bf56641966c4121");
                    var IcyBurst = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("564a6924b246d254c920a7c44bf2a58b");

                    ShockingBurst.WeaponFxPrefab = Shock.WeaponFxPrefab;
                    FlamingBurst.WeaponFxPrefab = Flaming.WeaponFxPrefab;
                    IcyBurst.WeaponFxPrefab = Frost.WeaponFxPrefab;

                    TTTContext.Logger.LogPatch(ShockingBurst);
                    TTTContext.Logger.LogPatch(FlamingBurst);
                    TTTContext.Logger.LogPatch(IcyBurst);
                }
                void PatchVorpal() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("Vorpal")) { return; }

                    var Vorpal = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("2f60bfcba52e48a479e4a69868e24ebc");
                    var VorpalBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4c02715a54a497a408a93a5d80e91a24");
                    Vorpal.SetComponents();
                    Vorpal.AddComponent<WeaponBuffOnConfirmedCritTTT>(c => {
                        c.m_Buff = VorpalBuff;
                        c.Duration = 1.Rounds();
                        c.Fx = new PrefabLink();
                        c.OnlyNatural20 = true;
                        c.OnTarget = true;
                    });
                    VorpalBuff.Get().FlattenAllActions().OfType<Conditional>().First().ConditionsChecker.TemporaryContext(checker => {
                        checker.Conditions = checker.Conditions.Where(c => c is not ContextConditionIsAlly).ToArray();
                    });

                    TTTContext.Logger.LogPatch("Patched", Vorpal);
                }
                void PatchNaturalWeapons() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("NaturalWeaponsFinesse")) { return; }

                    WeaponCategoryExtension.Data.Where(weapon => weapon.Category == WeaponCategory.Slam)
                        .ForEach(weapon => {
                            AddToSubCategories(weapon, WeaponSubCategory.Finessable);
                        });

                    void AddToSubCategories(WeaponCategoryExtension.DataItem weapon, params WeaponSubCategory[] categories) {
                        var SubCategories = AccessTools.Field(typeof(WeaponCategoryExtension.DataItem), "SubCategories");
                        SubCategories.SetValue(weapon, weapon.SubCategories.AppendToArray(categories).Distinct().ToArray());

                        TTTContext.Logger.Log($"Patched: {weapon.Category} - SubCategories");
                    }
                }
                void PatchSai() {
                    if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("SaiDamageType")) { return; }

                    var Sai = BlueprintTools.GetBlueprint<BlueprintWeaponType>("0944f411666c7594aa1398a7476ecf7d");

                    Sai.TemporaryContext(c => {
                        c.DamageType.Physical.Form = PhysicalDamageForm.Bludgeoning;
                    });
                    WeaponCategoryExtension.Data.Where(weapon => weapon.Category == WeaponCategory.Sai)
                        .ForEach(weapon => {
                            RemoveSubCategories(weapon, WeaponSubCategory.OneHandedPiercing);
                        });
                    TTTContext.Logger.LogPatch(Sai);
                    void RemoveSubCategories(WeaponCategoryExtension.DataItem weapon, params WeaponSubCategory[] categories) {
                        var SubCategories = AccessTools.Field(typeof(WeaponCategoryExtension.DataItem), "SubCategories");
                        SubCategories.SetValue(weapon, weapon.SubCategories.Where(c => !categories.Contains(c)).Distinct().ToArray());

                        TTTContext.Logger.Log($"Patched: {weapon.Category} - SubCategories");
                    }
                }
            }
        }

        [HarmonyPatch(typeof(WeaponEnergyBurst), nameof(WeaponEnergyBurst.OnEventAboutToTrigger), new Type[] { typeof(RuleDealDamage) })]
        class WeaponEnergyBurst_OnEventDidTrigger_Patch {
            static readonly MethodInfo get_Instance = AccessTools.PropertyGetter(typeof(Game), "Instance");
            static readonly FieldInfo field_Initiator = AccessTools.Field(typeof(RuleDealDamage), "Initiator");
            // ------------before------------
            // RuleCalculateWeaponStats ruleCalculateWeaponStats = Rulebook.Trigger<RuleCalculateWeaponStats>(new RuleCalculateWeaponStats(Game.Instance.DefaultUnit, base.Owner, null, null));
            // ------------after-------------
            // RuleCalculateWeaponStats ruleCalculateWeaponStats = Rulebook.Trigger<RuleCalculateWeaponStats>(new RuleCalculateWeaponStats(evt.Initiator, base.Owner, null, null));
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (Main.TTTContext.Fixes.Items.Weapons.IsDisabled("EnergyBurst")) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                // Replace the default unit with the initiator
                codes[target].opcode = OpCodes.Ldarg_1; codes[target++].operand = null;
                codes[target++] = new CodeInstruction(OpCodes.Ldfld, field_Initiator);
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                // Find where the game is loading the default unit
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].Calls(get_Instance)) {
                        return i;
                    }
                }
                TTTContext.Logger.Log("ENERGY BURST PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }

    }
}
