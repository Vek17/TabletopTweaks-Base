using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Kingmaker.View.MapObjects;
using System;

namespace TabletopTweaks.Base.Bugfixes.General {
    internal class AreaOfEffectsTick {
        [HarmonyPatch(typeof(AreaEffectEntityData), MethodType.Constructor, new Type[]{
            typeof(AreaEffectView),
            typeof(MechanicsContext),
            typeof(BlueprintAbilityAreaEffect),
            typeof(TargetWrapper),
            typeof(TimeSpan),
            typeof(TimeSpan?),
            typeof(bool),
        })]
        static class AreaOfEffectsTick_Round_Patch {
            static void Postfix(AreaEffectEntityData __instance) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("AreaOfEffectDoubleTrigger")) { return; }
                if (__instance.Blueprint.GetComponent<AbilityAreaEffectRunAction>().UnitEnter.HasActions) {
                    __instance.m_TimeToNextRound = 6f;
                }
            }
        }
    }
}
