using HarmonyLib;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Settings;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Reflection;
using UnityEngine;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.MechanicsChanges {
    internal class MetamagicDamage {
        //[HarmonyPatch(typeof(RuleCalculateDamage), "CalculateDamageValue", new[] { typeof(BaseDamage) })]
        static class RuleCalculateDamage_Metamagic_Patch {

            static void Postfix(RuleCalculateDamage __instance, ref DamageValue __result, BaseDamage damage) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("MetamagicStacking")) { return; }

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
                int num4 = 0;
                if (damage.PreRolledValue != null) {
                    num3 = damage.PreRolledValue.Value;
                } else {
                    DiceFormula modifiedValue = damage.Dice.ModifiedValue;
                    
                    if (__instance.RerollAndTakeBest) {
                        int a = __instance.Roll(modifiedValue, __instance.UnitsCount, damage.CalculationType);
                        int b = __instance.Roll(modifiedValue, __instance.UnitsCount, damage.CalculationType);
                        num4 = Mathf.Max(a, b);
                    } else {
                        num4 = __instance.Roll(modifiedValue, __instance.UnitsCount, damage.CalculationType);
                    }
                    //Custom Empower Logic
                    int empowerExtraDamage = 0;
                    if (empowerBonus > 1) {
                        var empowerMultiplier = empowerBonus - 1;
                        empowerExtraDamage = (int)Math.Round(__instance.Roll(
                            new DiceFormula((damage.Dice.ModifiedValue.Rolls), damage.Dice.ModifiedValue.Dice),
                            __instance.UnitsCount,
                            DamageCalculationType.Normal
                        ) * empowerMultiplier);
                        TTTContext.Logger.LogVerbose($"Empower Bonus {empowerMultiplier}: {damage.Dice.ModifiedValue.Rolls}{damage.Dice.ModifiedValue.Dice}/{1 / empowerMultiplier} = {empowerExtraDamage}");
                    }
                    num3 = ((num4 + empowerExtraDamage) + (damage.Bonus * __instance.UnitsCount) * empowerBonus * num2) * damage.TacticalCriticalModifier;
                }
                int rolledValue = (int)num3;
                num3 += damage.BonusTargetRelated * __instance.UnitsCount * empowerBonus * num2;
                if (damage.Half && !damage.AlreadyHalved) {
                    num3 /= 2f;
                }
                if (__instance.ParentRule.Half && !__instance.ParentRule.AlreadyHalved) {
                    num3 /= 2f;
                }
                if (__instance.ParentRule.HalfBecauseSavingThrow) {
                    float num5 = __instance.ParentRule.Initiator.State.Features.AzataFavorableMagic ? 0.75f : 0.5f;
                    num3 *= num5;
                }
                //int rolledValue = (int)num3;
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
                float num7 = 1 + (__instance.ParentRule.ModifierBonus ?? 0f);
                int num8 = Math.Max(1, (int)Math.Floor(num6 * num7));
                num8 = __instance.TryApplyShadowDamageFactor(__instance.Target, num8);
                num8 = (damage.Immune ? 0 : num8);
                __result = new DamageValue(damage, num8, rolledValue, num4, __instance.TacticalCombatDRModifier);
            }
        }
        [HarmonyPatch]
        static class ContextActionDealDamage_Bolster_Patch {
            static MethodBase TargetMethod() {
                return AccessTools.Method(typeof(ContextActionDealDamage),
                    "GetDamageRule",
                    new[] { typeof(ContextActionDealDamage.DamageInfo), typeof(int).MakeByRefType() });
            }
            [HarmonyPostfix]
            static void StripExtraBolsterDamage(ref RuleDealDamage __result, ContextActionDealDamage.DamageInfo info) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("MetamagicBolsterDoubleDiping")) { return; }
                __result.DamageBundle.First.PreRolledValue = info.PreRolledValue;
            }
        }
    }
}
