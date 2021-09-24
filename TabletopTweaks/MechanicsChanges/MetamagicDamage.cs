using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Settings;
using Kingmaker.UnitLogic.Mechanics;
using System;
using TabletopTweaks.Config;
using UnityEngine;

namespace TabletopTweaks.MechanicsChanges {
    class MetamagicDamage {
        [HarmonyPatch(typeof(RuleCalculateDamage), "CalculateDamageValue", new[] { typeof(BaseDamage) })]
        static class RuleCalculateDamage_Metamagic_Patch {

            static void Postfix(RuleCalculateDamage __instance, ref DamageValue __result, BaseDamage damage) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("MetamagicStacking")) { return; }

                SettingsEntityEnum<CriticalHitPower> enemyCriticalHits = SettingsRoot.Difficulty.EnemyCriticalHits;
                int num = damage.CriticalModifier ?? 1;
                if (damage.IgnoreImmunities) {
                    damage.ResetDecline();
                }
                if (__instance.CritImmunity && !__instance.IgnoreCritImmunity) {
                    num = 1;
                }
                int criticalModifier = (!__instance.Target.IsPlayerFaction || enemyCriticalHits != CriticalHitPower.Off) ? num : 1;
                int num2 = (!__instance.Target.IsPlayerFaction || enemyCriticalHits == CriticalHitPower.Normal || damage.Bonus + damage.BonusTargetRelated < 0) ? num : 1;
                float empowerBonus = damage.EmpowerBonus;
                float num3;
                if (damage.PreRolledValue != null) {
                    num3 = damage.PreRolledValue.Value;
                } else {
                    int num4;
                    if (__instance.RerollAndTakeBest) {
                        int a = __instance.Roll(damage.Dice, criticalModifier, __instance.UnitsCount, damage.CalculationType);
                        int b = __instance.Roll(damage.Dice, criticalModifier, __instance.UnitsCount, damage.CalculationType);
                        num4 = Mathf.Max(a, b);
                    } else {
                        num4 = __instance.Roll(damage.Dice, criticalModifier, __instance.UnitsCount, damage.CalculationType);
                    }
                    //Custom Empower Logic
                    int empowerExtraDamage = 0;
                    if (empowerBonus > 1) {
                        var empowerMultiplier = empowerBonus - 1;
                        empowerExtraDamage = (int)Math.Round(__instance.Roll(
                            new DiceFormula((damage.Dice.Rolls), damage.Dice.Dice),
                            criticalModifier,
                            __instance.UnitsCount,
                            DamageCalculationType.Normal
                        ) * empowerMultiplier);
                        Main.LogDebug($"Empower Bonus {empowerMultiplier}: {damage.Dice.Rolls}{damage.Dice.Dice}/{1 / empowerMultiplier} = {empowerExtraDamage}");
                    }
                    num3 = ((num4 + empowerExtraDamage) + (damage.Bonus * __instance.UnitsCount) * empowerBonus * num2) * damage.TacticalCriticalModifier;
                }
                int rolledValue = (int)num3;
                num3 += damage.BonusTargetRelated * __instance.UnitsCount * empowerBonus * num2;
                if (damage.Half && !damage.AlreadyHalved) {
                    num3 /= 2f;
                }
                if (__instance.ParentRule.HalfBecauseSavingThrow) {
                    float num5 = __instance.ParentRule.Initiator.State.Features.AzataFavorableMagic ? 0.75f : 0.5f;
                    num3 *= num5;
                }
                DamageDeclineType decline = damage.Decline;
                if (decline != DamageDeclineType.ByQuarter) {
                    if (decline == DamageDeclineType.ByHalf) {
                        num3 *= 0.5f;
                    }
                } else {
                    num3 *= 0.75f;
                }
                num3 = Math.Max(1f, num3);
                float num6 = num3 * (1f + damage.BonusPercent / 100f) * damage.Vulnerability * damage.Durability * __instance.TacticalCombatFactor;
                float num7 = __instance.ParentRule.ModifierBonus ?? 1f;
                int num8 = Math.Max(1, (int)Math.Floor(num6 * num7));
                num8 = __instance.TryApplyShadowDamageFactor(__instance.Target, num8);
                num8 = (damage.Immune ? 0 : num8);
                __result = new DamageValue(damage, num8, rolledValue, __instance.TacticalCombatDRModifier);
            }
        }
    }
}
