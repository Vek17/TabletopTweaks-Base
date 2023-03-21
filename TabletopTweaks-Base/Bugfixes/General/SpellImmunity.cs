using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

namespace TabletopTweaks.Base.Bugfixes.General {
    internal class SpellImmunity {
        [HarmonyPatch(typeof(RuleSpellResistanceCheck), nameof(RuleSpellResistanceCheck.IsSpellResisted), MethodType.Getter)]
        static class RuleSpellResistanceCheck_IsSpellResisted_Patch {
            static void Postfix(RuleSpellResistanceCheck __instance, ref bool __result) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("SpellImmunity")) { return; }
                __result = __instance.TargetIsImmune
                        || (!__instance.IgnoreSpellResistance
                            && (__instance.HasResistanceRoll && __instance.SpellResistance > __instance.SpellPenetration + __instance.Roll));
            }
        }

        [HarmonyPatch(typeof(RuleSpellResistanceCheck), nameof(RuleSpellResistanceCheck.IgnoreSpellResistance), MethodType.Getter)]
        static class RuleSpellResistanceCheck_IgnoreSpellResistance_Patch {
            static void Postfix(RuleSpellResistanceCheck __instance, ref bool __result) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("SpellImmunity")) { return; }
                __result |= !__instance.Initiator.Descriptor.State.Features.PrimalMagicEssence;
            }
        }
    }
}
