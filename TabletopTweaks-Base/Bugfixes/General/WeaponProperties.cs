using HarmonyLib;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Linq;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.General {
    static class WeaponProperties {
        /**
         * This is a set of patches to apply weapon properties to all of a weapon's damage to support changes like vital strike
        */
        [HarmonyPatch(typeof(WeaponReality), nameof(WeaponReality.OnEventDidTrigger), new Type[] { typeof(RulePrepareDamage) })]
        class WeaponReality_OnEventDidTrigger_Patch {
            static void Postfix(WeaponReality __instance, RulePrepareDamage evt) {
                if (evt.DamageBundle.Weapon == __instance.Owner && evt.DamageBundle.WeaponDamage != null) {
                    foreach (var damage in evt.DamageBundle) {
                        damage.Reality |= __instance.Reality;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(WeaponMaterial), nameof(WeaponMaterial.OnEventDidTrigger), new Type[] { typeof(RulePrepareDamage) })]
        class WeaponMaterial_OnEventDidTrigger_Patch {
            static void Postfix(WeaponMaterial __instance, RulePrepareDamage evt) {
                if (evt.DamageBundle.Weapon == __instance.Owner && evt.DamageBundle.WeaponDamage != null) {
                    foreach (PhysicalDamage damage in evt.DamageBundle.OfType<PhysicalDamage>()) {
                        damage.AddMaterial(__instance.Material);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(WeaponAlignment), nameof(WeaponAlignment.OnEventDidTrigger), new Type[] { typeof(RulePrepareDamage) })]
        class WeaponAlignment_OnEventDidTrigger_Patch {
            static void Postfix(WeaponAlignment __instance, RulePrepareDamage evt) {
                if (evt.DamageBundle.Weapon == __instance.Owner && evt.DamageBundle.WeaponDamage != null) {
                    foreach (var damage in evt.DamageBundle) {
                        damage.AddAlignment(__instance.Alignment);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(RuleCalculateWeaponStats), nameof(RuleCalculateWeaponStats.CriticalRange), MethodType.Getter)]
        class RuleCalculateWeaponStats_CriticalRange_Patch {
            static void Postfix(RuleCalculateWeaponStats __instance, ref int __result) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("CriticalRangeIncreases")) { return; }
                __result = ((21 - __instance.Weapon.Blueprint.CriticalRollEdge) * (__instance.DoubleCriticalEdge ? 2 : 1))
                    + __instance.CriticalEdgeBonus;
            }
        }
    }
}
