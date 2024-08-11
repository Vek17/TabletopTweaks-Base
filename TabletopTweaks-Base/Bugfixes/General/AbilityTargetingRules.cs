using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Base.Bugfixes.General {
    static class AbilityTargetingRules {
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.CanTarget))]
        static class AbilityData_CanTarget_Patch {
            static void Postfix(AbilityData __instance, TargetWrapper target, ref bool __result) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("AbilityTargeting")) { return; }

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
                if (!__instance.CanTargetAlly && !target.Unit.IsEnemy(__instance.Caster.Unit) && target.Unit != __instance.Caster.Unit) {
                    __result = false;
                    return;
                }
            }
        }
    }
}
