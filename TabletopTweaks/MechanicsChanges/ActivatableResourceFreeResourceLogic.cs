using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using TabletopTweaks.Config;

namespace TabletopTweaks.MechanicsChanges {
    class ActivatableResourceFreeResourceLogic {

        [HarmonyPatch(typeof(ActivatableAbility), "HandleUnitLeaveCombat")]
        static class ActivatableAbility_HandleUnitLeaveCombat_Patch {
            static bool Prefix(ActivatableAbility __instance, UnitEntityData unit) {
                if (!ModSettings.Fixes.DisableAfterCombatDeactivationOfUnlimitedAbilities) { return true; }
                var m_FreeBlueprint = __instance.Blueprint.GetComponent<ActivatableAbilityResourceLogic>()?.m_FreeBlueprint;
                var OverrideDeactivateIfCombatEnded = m_FreeBlueprint != null && __instance.Owner.HasFact(m_FreeBlueprint);

                if (unit != __instance.Owner.Unit
                    || (!__instance.Blueprint.DeactivateIfCombatEnded && !OverrideDeactivateIfCombatEnded)
                    || !__instance.IsStarted
                    || !__instance.Blueprint.DeactivateImmediately) {
                    return false;
                }
                __instance.Stop(false);
                return false;
            }
        }

        [HarmonyPatch(typeof(ActivatableAbility), "OnNewRound")]
        static class ActivatableAbility_OnNewRound_Patch {
            static bool Prefix(ActivatableAbility __instance) {
                if (!ModSettings.Fixes.DisableAfterCombatDeactivationOfUnlimitedAbilities) { return true; }
                __instance.m_WasInCombat |= __instance.Owner.Unit.IsInCombat;
                var m_FreeBlueprint = __instance.Blueprint.GetComponent<ActivatableAbilityResourceLogic>()?.m_FreeBlueprint;
                var OverrideDeactivateIfCombatEnded = m_FreeBlueprint != null && __instance.Owner.HasFact(m_FreeBlueprint);

                if (__instance.m_ShouldBeDeactivatedInNextRound
                    || !__instance.IsOn
                    || !__instance.IsAvailable
                    || (__instance.Blueprint.DeactivateIfCombatEnded && !OverrideDeactivateIfCombatEnded
                        && !__instance.Owner.Unit.IsInCombat
                        && (__instance.Blueprint.ActivateOnCombatStarts || __instance.m_WasInCombat))
                ) {
                    __instance.Stop(false);
                } else {
                    __instance.CallComponents((IActivatableAbilitySpendResourceLogic l) => l.OnNewRound());
                    ActivatableAbilityUnitCommand component = __instance.GetComponent<ActivatableAbilityUnitCommand>();
                    if (component) {
                        __instance.Owner.Unit.SpendAction(component.Type, false, 0f);
                    }
                }
                __instance.m_ShouldBeDeactivatedInNextRound = __instance.Blueprint.DeactivateAfterFirstRound;
                return false;
            }
        }
    }
}
