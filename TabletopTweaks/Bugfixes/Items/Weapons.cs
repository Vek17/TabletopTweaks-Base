using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
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
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.OwlcatReplacements;

namespace TabletopTweaks.Bugfixes.Items {
    static class Weapons {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                Main.LogHeader("Patching Weapons");

                PatchBladeOfTheMerciful();
                PatchFinnean();
                PatchHonorableJudgement();
                PatchRadiance();
                PatchTerribleTremble();

                PatchThunderingBurst();
                PatchVorpal();

                void PatchBladeOfTheMerciful() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("BladeOfTheMerciful")) { return; }

                    var BladeOfTheMercifulEnchant = Resources.GetBlueprint<BlueprintWeaponEnchantment>("a5e3fe4a71e331e4aa41f9a07cfd3729");
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
                    Main.LogPatch("Patched", BladeOfTheMercifulEnchant);
                }
                void PatchHonorableJudgement() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("HonorableJudgement")) { return; }

                    var JudgementOfRuleItem = Resources.GetBlueprint<BlueprintItemWeapon>("f40895a7dfab41c40b42657fc3f5bdfe");
                    var JudgementOfRuleSecondItem = Resources.GetBlueprint<BlueprintItemWeapon>("ca0e81e14d675c34b862aad509be573d");
                    var JudgementOfRuleEnchantment = Resources.GetBlueprint<BlueprintWeaponEnchantment>("74a8dc2f9ce6ced4fa211c20fa4def32");
                    var BaneOutsiderEvil = Resources.GetBlueprint<BlueprintWeaponEnchantment>("20ba9055c6ae1e44ca270c03feacc53b");
                    JudgementOfRuleEnchantment.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
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
                    Main.LogPatch("Patched", JudgementOfRuleEnchantment);
                }
                void PatchFinnean() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("Finnean")) { return; }

                    var FinneanChapter3Enchantment = Resources.GetBlueprint<BlueprintWeaponEnchantment>("b183bd491793d194c9e4c96cd11769b1");
                    var FinneanChapter5EnchantmentBase = Resources.GetBlueprint<BlueprintWeaponEnchantment>("6b66e949f348ccd4989a5fd9254f8958");
                    var FinneanChapter5EnchantmentLich = Resources.GetBlueprint<BlueprintWeaponEnchantment>("9aa9af4b654662945a410644d3db8d99");

                    FinneanChapter3Enchantment.RemoveComponents<AdditionalDiceOnAttack>();
                    FinneanChapter3Enchantment.AddComponent<WeaponExtraDamageDice>(c => {
                        c.Value = new DiceFormula(1, DiceType.D6);
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Force
                        };
                    });

                    Main.LogPatch("Patched", FinneanChapter3Enchantment);
                }
                void PatchTerribleTremble() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("TerrifyingTremble")) { return; }

                    var TerrifyingTrembleItem = Resources.GetBlueprint<BlueprintItemWeapon>("8c31891423c4405393741e829aebec85");
                    var Enhancement5 = Resources.GetBlueprint<BlueprintWeaponEnchantment>("bdba267e951851449af552aa9f9e3992");
                    var Ultrasound = Resources.GetBlueprint<BlueprintWeaponEnchantment>("582849db96824254ebcc68f0b7484e51");
                    var TerrifyingTrembleEnchant_TTT = Resources.GetModBlueprint<BlueprintWeaponEnchantment>("TerrifyingTrembleEnchant_TTT");

                    TerrifyingTrembleItem.SetDescription("Whenever the wielder of this +5 ultrasound earthbreaker lands a killing blow, he deals sonic damage equal to his ranks in " +
                        "the Athletics skill to all enemies within 10 feet. Successful Reflex save (DC 30) halves the damage.");

                    TerrifyingTrembleItem.m_Enchantments = new BlueprintWeaponEnchantmentReference[] {
                        Enhancement5.ToReference<BlueprintWeaponEnchantmentReference>(),
                        Ultrasound.ToReference<BlueprintWeaponEnchantmentReference>(),
                        TerrifyingTrembleEnchant_TTT.ToReference<BlueprintWeaponEnchantmentReference>(),
                    };

                    Main.LogPatch("Patched", TerrifyingTrembleItem);
                }
                void PatchRadiance() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("Radiance")) { return; }

                    var RadianceEffectBuff = Resources.GetBlueprint<BlueprintBuff>("0c03ba5e0c3fd304eb0a221e83f4ce1d");
                    RadianceEffectBuff.RemoveComponents<SpellPenetrationBonus>();
                    RadianceEffectBuff.AddComponent<AddSpellResistance>(c => {
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    });

                    Main.LogPatch("Patched", RadianceEffectBuff);
                }
                void PatchThunderingBurst() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("ThunderingBurst")) { return; }

                    var ThunderingBurst = Resources.GetBlueprint<BlueprintWeaponEnchantment>("83bd616525288b34a8f34976b2759ea1");
                    ThunderingBurst.GetComponent<WeaponEnergyBurst>().Dice = DiceType.D10;

                    Main.LogPatch("Patched", ThunderingBurst);
                }
                void PatchVorpal() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("Vorpal")) { return; }

                    var Vorpal = Resources.GetBlueprint<BlueprintWeaponEnchantment>("2f60bfcba52e48a479e4a69868e24ebc");
                    var VorpalBuff = Resources.GetBlueprintReference<BlueprintBuffReference>("4c02715a54a497a408a93a5d80e91a24");
                    Vorpal.SetComponents();
                    Vorpal.AddComponent<WeaponBuffOnConfirmedCritTTT>(c => {
                        c.m_Buff = VorpalBuff;
                        c.Duration = 1.Rounds();
                        c.Fx = new PrefabLink();
                        c.OnlyNatural20 = true;
                        c.OnTarget = true;
                    });

                    Main.LogPatch("Patched", Vorpal);
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
                if (ModSettings.Fixes.Items.Weapons.IsDisabled("EnergyBurst")) { return instructions; }
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
                Main.Log("ENERGY BURST PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }

    }
}
