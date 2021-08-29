using HarmonyLib;
using Kingmaker.Blueprints.Items.Ecnchantments;
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
                PatchBladeOfTheMerciful();

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
            }
        }
    }
}
