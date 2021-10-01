using HarmonyLib;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Linq;

namespace TabletopTweaks.Bugfixes.General {
    static class WeaponProperties {
        /**
         * This is a set of patches to apply weapon properties to all of a weapon's damage to support changes like vital strike
        */
        [HarmonyPatch(typeof(WeaponReality), "OnEventDidTrigger", new Type[] { typeof(RulePrepareDamage) })]
        class WeaponReality_OnEventAboutToTrigger_Patch {
            static void Postfix(WeaponReality __instance, RulePrepareDamage evt) {
                if (evt.DamageBundle.Weapon == __instance.Owner && evt.DamageBundle.WeaponDamage != null) {
                    foreach (var damage in evt.DamageBundle) {
                        damage.Reality |= __instance.Reality;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(WeaponMaterial), "OnEventDidTrigger", new Type[] { typeof(RulePrepareDamage) })]
        class WeaponMaterial_OnEventAboutToTrigger_Patch {
            static void Postfix(WeaponMaterial __instance, RulePrepareDamage evt) {
                if (evt.DamageBundle.Weapon == __instance.Owner && evt.DamageBundle.WeaponDamage != null) {
                    foreach (PhysicalDamage damage in evt.DamageBundle.OfType<PhysicalDamage>()) {
                        damage.AddMaterial(__instance.Material);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(WeaponAlignment), "OnEventDidTrigger", new Type[] { typeof(RulePrepareDamage) })]
        class WeaponAlignment_OnEventAboutToTrigger_Patch {
            static void Postfix(WeaponAlignment __instance, RulePrepareDamage evt) {
                if (evt.DamageBundle.Weapon == __instance.Owner && evt.DamageBundle.WeaponDamage != null) {
                    foreach (var damage in evt.DamageBundle) {
                        damage.AddAlignment(__instance.Alignment);
                    }
                }
            }
        }
    }
}
