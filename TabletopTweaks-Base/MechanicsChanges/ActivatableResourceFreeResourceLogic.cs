using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Units;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.MechanicsChanges {
    internal class ActivatableResourceFreeResourceLogic {

        [HarmonyPatch(typeof(UnitActivatableAbilitiesController), "StopOutOfCombat")]
        static class UnitActivatableAbilitiesController_StopOutOfCombat_Patch {
            static bool Prefix(ActivatableAbility ability, bool dialogOrCutsceneActive) {
                TTTContext.Logger.LogVerbose($"{ability.Owner.CharacterName}:{ability.Blueprint.name} - StopOutOfCombat");
                if (TTTContext.Fixes.BaseFixes.IsDisabled("DisableAfterCombatDeactivationOfUnlimitedAbilities")) { return true; }
                var m_FreeBlueprint = ability.Blueprint.GetComponent<ActivatableAbilityResourceLogic>()?.m_FreeBlueprint;
                if (m_FreeBlueprint?.Get() == null) { return true; }
                //TTTContext.Logger.LogVerbose($"{__instance.Owner.CharacterName}: {__instance.Blueprint.name} - {m_FreeBlueprint?.Get()} - HandleUnitLeaveCombat");
                var OverrideDeactivateIfCombatEnded = m_FreeBlueprint?.Get() != null && ability.Owner.HasFact(m_FreeBlueprint.Get());
                if (ability.Blueprint.DeactivateIfCombatEnded && OverrideDeactivateIfCombatEnded) {
                    return false;
                }
                return true; //__instance.Stop(false);
                //TTTContext.Logger.LogVerbose($"{__instance.Owner.CharacterName}: {__instance.Blueprint.name} - {m_FreeBlueprint?.Get()} - HandleUnitLeaveCombat");
                //return false;
            }

        }

        [HarmonyPatch(typeof(ActivatableAbility), "OnNewRound")]
        static class ActivatableAbility_OnNewRound_Patch {
            static void Postfix() {
                TTTContext.Logger.LogVerbose($"POSTFIX - OnNewRound");
            }
            static bool Prefix(ActivatableAbility __instance) {
                TTTContext.Logger.LogVerbose($"{__instance.Owner.CharacterName}:{__instance.Blueprint.name} - OnNewRound");
                if (TTTContext.Fixes.BaseFixes.IsDisabled("DisableAfterCombatDeactivationOfUnlimitedAbilities")) { return true; }
                __instance.m_WasInCombat |= __instance.Owner.Unit.IsInCombat;
                var m_FreeBlueprint = __instance.Blueprint.GetComponent<ActivatableAbilityResourceLogic>()?.m_FreeBlueprint;
                if (m_FreeBlueprint?.Get() == null) { return true; }
                //TTTContext.Logger.LogVerbose($"{__instance.Owner.CharacterName}: {__instance.Blueprint.name} - {m_FreeBlueprint?.Get()} - OnNewRound");
                var OverrideDeactivateIfCombatEnded = m_FreeBlueprint?.Get() != null && __instance.Owner.HasFact(m_FreeBlueprint.Get());

                if (!(__instance.m_ShouldBeDeactivatedInNextRound || !__instance.IsOn || !__instance.IsAvailable)
                     && OverrideDeactivateIfCombatEnded) {
                    return false;
                }
                //TTTContext.Logger.LogVerbose($"{__instance.Owner.CharacterName}: {__instance.Blueprint.name} - {m_FreeBlueprint?.Get()} - OnNewRound - RETURN");
                return true;
            }
        }
    }
}
