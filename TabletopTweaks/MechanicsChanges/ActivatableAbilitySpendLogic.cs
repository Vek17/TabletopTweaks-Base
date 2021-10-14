using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using System;
using System.Linq;
using static Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic;

namespace TabletopTweaks.MechanicsChanges {
    static class ActivatableAbilitySpendLogic {

        public enum StandardSpendType : int {
            AeonGaze = 200
        }

        public static ResourceSpendType ToResourceType(this StandardSpendType type) {
            return (ResourceSpendType)type;
        }

        public enum ValueSpendType : int {
            Crit = 0b00000000_00000001_00000000_00000000
        }

        public static ResourceSpendType Amount(this ValueSpendType type, byte value) {
            return (ResourceSpendType)((int)type | value);
        }

        private static int Amount(this ValueSpendType type) {
            return ((int)type & 0b00000000_00000000_00000000_11111111);
        }

        private static int CustomValue(this ResourceSpendType type) {
            return ((int)type & 0b00000000_00000000_00000000_11111111);
        }

        private static bool IsCustomSpendType(this ResourceSpendType type) {
            return ((int)type & 0b11111111_11111111_00000000_00000000) > 0;
        }

        private static bool IsType(this ResourceSpendType flag, ValueSpendType type) {
            return (flag & (ResourceSpendType)type) > 0;
        }

        [HarmonyPatch(typeof(ActivatableAbilityResourceLogic),
            "Kingmaker.UnitLogic.ActivatableAbilities.IActivatableAbilitySpendResourceLogic.OnCrit")]
        class ActivatableAbilityResourceLogic_OnCrit_PerfectCritical_Patch {
            static void Postfix(ActivatableAbilityResourceLogic __instance) {
                if (__instance.SpendType.IsType(ValueSpendType.Crit)) {
                    __instance.SpendResource(true);
                }
            }
        }

        [HarmonyPatch(typeof(ActivatableAbilityResourceLogic), "IsAvailable")]
        class ActivatableAbilityResourceLogic_IsAvailable_PerfectCritical_Patch {
            static void Postfix(ActivatableAbilityResourceLogic __instance, ref bool __result, EntityFactComponent runtime) {
                using (runtime.RequestEventContext()) {
                    if (__instance.SpendType.IsCustomSpendType()) {
                        __result = __instance.Owner.Resources.HasEnoughResource(__instance.RequiredResource, __instance.SpendType.CustomValue());
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ActivatableAbilityResourceLogic), "SpendResource", new Type[] { typeof(bool) })]
        class ActivatableAbilityResourceLogic_SpendResource_PerfectCritical_Patch {
            static bool Prefix(ActivatableAbilityResourceLogic __instance) {
                if (!__instance.SpendType.IsCustomSpendType()) { return true; }
                if (__instance.Owner.HasFact(__instance.FreeBlueprint)) {
                    return false;
                }
                if (!__instance.RequiredResource) {
                    if (__instance.Fact.SourceItem != null && !__instance.Fact.SourceItem.SpendCharges()) {
                        __instance.Fact.TurnOffImmediately();
                    }
                    return false;
                }
                if (__instance.Owner.Resources.HasEnoughResource(__instance.RequiredResource, __instance.SpendType.CustomValue())) {
                    __instance.Owner.Resources.Spend(__instance.RequiredResource, __instance.SpendType.CustomValue());
                    return false;
                }
                __instance.Fact.TurnOffImmediately();
                return true;
            }
        }

        [HarmonyPatch(typeof(ActivatableAbility), "ResourceCount", MethodType.Getter)]
        class ActivatableAbility_ResourceCount_PerfectCritical_Patch {
            static void Postfix(ActivatableAbility __instance, ref int? __result) {
                __instance.m_CachedResourceLogic = __instance.m_CachedResourceLogic ??
                    __instance.SelectComponentsWithRuntime<ActivatableAbilityResourceLogic>()
                    .ToArray();
                ActivatableAbilityResourceLogic component = __instance.m_CachedResourceLogic
                    .FirstItem()
                    .Component;
                BlueprintAbilityResource blueprintAbilityResource =
                    (component != null && component.SpendType != ActivatableAbilityResourceLogic.ResourceSpendType.None)
                        ? component.RequiredResource : null;
                if (component == null) { return; }
                if (blueprintAbilityResource == null) { return; }
                if (component.FreeBlueprint != null && __instance.Owner.HasFact(component.FreeBlueprint)) {
                    return;
                }
                var spendAmount = (int)component.SpendType - (int)ValueSpendType.Crit;
                if (spendAmount < 100 && spendAmount > 0) {
                    __result = __instance.Owner.Resources.GetResourceAmount(blueprintAbilityResource) / spendAmount;
                }
            }
        }


    }
}
