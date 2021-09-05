using HarmonyLib;
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
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Items {
    static class Weapons {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                Main.LogHeader("Patching Weapons");
                //PatchBladeOfTheMerciful();
                //PatchHonorableJudgement();

                void PatchBladeOfTheMerciful() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("BladeOfTheMerciful")) { return; }

                    var BladeOfTheMercifulEnchant = Resources.GetBlueprint<BlueprintWeaponEnchantment>("a5e3fe4a71e331e4aa41f9a07cfd3729");
                    BladeOfTheMercifulEnchant.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    BladeOfTheMercifulEnchant.AddComponent(Helpers.Create<WeaponConditionalDamageDice>(c => {
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
                    }));
                    Main.LogPatch("Patched", BladeOfTheMercifulEnchant);
                }
                void PatchHonorableJudgement() {
                    if (ModSettings.Fixes.Items.Weapons.IsDisabled("HonorableJudgement")) { return; }

                    var JudgementOfRuleItem = Resources.GetBlueprint<BlueprintItemWeapon>("f40895a7dfab41c40b42657fc3f5bdfe");
                    var JudgementOfRuleSecondItem = Resources.GetBlueprint<BlueprintItemWeapon>("ca0e81e14d675c34b862aad509be573d");
                    var JudgementOfRuleEnchantment = Resources.GetBlueprint<BlueprintWeaponEnchantment>("74a8dc2f9ce6ced4fa211c20fa4def32");
                    var BaneOutsiderEvil = Resources.GetBlueprint<BlueprintWeaponEnchantment>("20ba9055c6ae1e44ca270c03feacc53b");
                    JudgementOfRuleEnchantment.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    JudgementOfRuleEnchantment.AddComponent(Helpers.Create<WeaponConditionalDamageDice>(c => {
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
                    }));
                    Main.LogPatch("Patched", JudgementOfRuleEnchantment);
                }
            }
        }
        [HarmonyPatch(typeof(WeaponConditionalDamageDice), "OnEventAboutToTrigger")]
        static class WeaponConditionalDamageDice_OnEventAboutToTrigger_Patch {
            static void Postfix(WeaponConditionalDamageDice __instance) {
                Main.Log($"TRIGGERD: {__instance?.Fact?.Blueprint?.name}");
            }
        }
    }
}
