using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic;

namespace TabletopTweaks.MechanicsChanges {
    static class ActivatableAbilitySpendLogic {

        public enum CustomSpendType : int {
            Crit = 100
        }

        public static ResourceSpendType Amount(this CustomSpendType type, int value) {
            return (ResourceSpendType)((int)type + value);
        }

        [HarmonyPatch(typeof(ActivatableAbilityResourceLogic),
            "Kingmaker.UnitLogic.ActivatableAbilities.IActivatableAbilitySpendResourceLogic.OnCrit")]
        class ActivatableAbilityResourceLogic_OnCrit_PerfectCritical_Patch {
            static void Postfix(ActivatableAbilityResourceLogic __instance) {
                var spendAmount = (int)__instance.SpendType - (int)CustomSpendType.Crit;
                if (spendAmount < 100 && spendAmount > 0) {
                    __instance.SpendResource(true);
                }
            }
        }

        [HarmonyPatch(typeof(ActivatableAbilityResourceLogic), "IsAvailable")]
        class ActivatableAbilityResourceLogic_IsAvailable_PerfectCritical_Patch {
            static void Postfix(ActivatableAbilityResourceLogic __instance, ref bool __result, EntityFactComponent runtime) {
                using (runtime.RequestEventContext()) {
                    var spendAmount = (int)__instance.SpendType - (int)CustomSpendType.Crit;
                    if (spendAmount < 100 && spendAmount > 0) {
                        __result = __instance.Owner.Resources.HasEnoughResource(__instance.RequiredResource, spendAmount);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ActivatableAbilityResourceLogic), "SpendResource", new Type[] { typeof(bool) })]
        class ActivatableAbilityResourceLogic_SpendResource_PerfectCritical_Patch {
            static bool Prefix(ActivatableAbilityResourceLogic __instance) {
                var spendAmount = (int)__instance.SpendType - (int)CustomSpendType.Crit;
                if (spendAmount < 100 && spendAmount > 0) {
                    if (__instance.Owner.HasFact(__instance.FreeBlueprint)) {
                        return false;
                    }
                    if (!__instance.RequiredResource) {
                        if (__instance.Fact.SourceItem != null && !__instance.Fact.SourceItem.SpendCharges()) {
                            __instance.Fact.TurnOffImmediately();
                        }
                        return false;
                    }
                    if (__instance.Owner.Resources.HasEnoughResource(__instance.RequiredResource, spendAmount)) {
                        __instance.Owner.Resources.Spend(__instance.RequiredResource, spendAmount);
                        return false;
                    }
                    
                    __instance.Fact.TurnOffImmediately();
                }
                return false;
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
                var spendAmount = (int)component.SpendType - (int)CustomSpendType.Crit;
                if (spendAmount < 100 && spendAmount > 0) {
                    __result = __instance.Owner.Resources.GetResourceAmount(blueprintAbilityResource) / spendAmount;
                }
            }
        }

        
    }
}
