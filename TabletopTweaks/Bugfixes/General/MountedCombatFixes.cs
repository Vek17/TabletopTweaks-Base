using HarmonyLib;
using Kingmaker.Controllers.Combat;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Config;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.Bugfixes.General {
    class MountedCombatFixes {
        // Need to fix cursor UI somehow
        [HarmonyPatch(typeof(UnitCombatState), nameof(UnitCombatState.IsFullAttackRestrictedBecauseOfMoveAction), MethodType.Getter)]
        static class UnitCombatState_IsFullAttackRestrictedBecauseOfMoveAction_Mounted_Patch {
            static void Postfix(UnitCombatState __instance, ref bool __result) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("MountedActions")) { return; }
                if (__instance.Unit.CustomMechanicsFeature(UnitPartCustomMechanicsFeatures.CustomMechanicsFeature.MountedSkirmisher)) { return; }
                var riderPart = __instance.Unit.Get<UnitPartRider>();
                if (riderPart?.SaddledUnit?.CombatState != null) {
                    __result |= riderPart.SaddledUnit.CombatState.IsFullAttackRestrictedBecauseOfMoveAction;
                }
            }
        }
        //[HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.HasMoveAction))]
        static class UnitEntityData_HasMoveAction_Saddled_Patch {
            static void Postfix(UnitEntityData __instance, ref bool __result) {
                var saddledPart = __instance.Get<UnitPartSaddled>();
                if (saddledPart?.Rider != null) {
                    __result &= saddledPart.Rider.HasMoveAction();
                }
            }
        }
        //[HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.SpendAction))]
        static class UnitEntityData_SpendAction_Mounted_Patch {
            static void Postfix(UnitEntityData __instance, UnitCommand.CommandType type, bool isFullRound, float timeSinceCommandStart) {
                var riderPart = __instance.Get<UnitPartRider>();
                if (riderPart?.SaddledUnit != null) {
                    if (isFullRound) {
                        riderPart.SaddledUnit.SpendAction(UnitCommand.CommandType.Move, false, timeSinceCommandStart);
                        riderPart.SaddledUnit.SpendAction(UnitCommand.CommandType.Move, false, timeSinceCommandStart);
                    }
                    if (type == UnitCommand.CommandType.Move) {
                        riderPart.SaddledUnit.SpendAction(UnitCommand.CommandType.Move, false, timeSinceCommandStart);
                    }
                }
            }
        }
    }
}
