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
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

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
                PatchHonorableJudgement();
                PatchThunderingBurst();

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
                void PatchThunderingBurst() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("ThunderingBurst")) { return; }

                    var ThunderingBurst = Resources.GetBlueprint<BlueprintWeaponEnchantment>("83bd616525288b34a8f34976b2759ea1");
                    ThunderingBurst.GetComponent<WeaponEnergyBurst>().Dice = DiceType.D10;

                    Main.LogPatch("Patched", ThunderingBurst);
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
