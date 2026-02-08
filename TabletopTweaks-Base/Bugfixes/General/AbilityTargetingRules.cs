using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;

namespace TabletopTweaks.Base.Bugfixes.General {
    static class AbilityTargetingRules {
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.CanTarget))]
        static class AbilityData_CanTarget_Patch {
            static void Postfix(AbilityData __instance, TargetWrapper target, ref bool __result) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("AbilityTargeting")) { return; }
                if (TacticalCombatHelper.IsActive) { return; }
                BlueprintAbility deliverBlueprint = __instance.GetDeliverBlueprint(false);
                if (__instance.Caster.Unit.GetSaddledUnit() != null && __instance.ShouldDelegateToMount) {
                    AbilityData sameMountAbility = __instance.SameMountAbility;
                    if (sameMountAbility != null) {
                        return;
                    }
                }
                if (!target.IsUnit || target.Unit == null) {
                    return;
                }
                if (!__instance.CanTargetEnemies && target.Unit.IsEnemy(__instance.Caster.Unit)) {
                    __result = false;
                    return;
                }
                if (!__instance.CanTargetAlly && target.Unit.IsAlly(__instance.Caster.Unit) && target.Unit != __instance.Caster.Unit) {
                    __result = false;
                    return;
                }
            }
        }
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.CanTargetAlly), MethodType.Getter)]
        static class AbilityData_CanTargetAlly_Patch {
            static void Postfix(AbilityData __instance, ref bool __result) {
                __result |= __instance.AlchemistInfusion;
                __result |= __instance.ArcanistShareTransmutation;
            }
        }
    }
}
