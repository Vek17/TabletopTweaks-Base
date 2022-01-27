using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Localization;
using Kingmaker.UI.MVVM._PCView.Other;
using Kingmaker.UI.MVVM._PCView.Party;
using Kingmaker.UI.MVVM._PCView.ServiceWindows.CharacterInfo.Sections.Abilities;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;
using UnityEngine;

namespace TabletopTweaks.Bugfixes.UI {
    static class BuffSuppression {
        //[HarmonyPatch(typeof(UnitBuffPartPCView), "DrawBuffs")]
        private static class BuffVM_Suppression_Patch {
            [HarmonyPrepare]
            static bool ApplyPatch() {
                return ModSettings.Fixes.BaseFixes.IsEnabled("SuppressedBuffUI");
            }
            static void Postfix(UnitBuffPartPCView __instance) {
                foreach (var PCView in __instance.m_BuffList) {
                    if (PCView?.ViewModel?.Buff?.IsSuppressed ?? false) {
                        PCView.m_Icon.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(BuffPCView), "BindViewImplementation")]
        private static class BuffPCView_Suppression_Patch {
            [HarmonyPrepare]
            static bool ApplyPatch() {
                return ModSettings.Fixes.BaseFixes.IsEnabled("SuppressedBuffUI");
            }
            static void Prefix(BuffPCView __instance) {
                if (__instance?.ViewModel?.Buff?.IsSuppressed ?? false) {
                    __instance.m_Icon.color = new Color(1f, 1f, 1f, 0.5f);
                } else {
                    __instance.m_Icon.color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
        [HarmonyPatch(typeof(CharInfoFeatureView))]
        private static class CharInfoFeaturePCView_Suppression_Patchs {
            private static LocalizedString Suppressed = Helpers.CreateString("SuppressedBuff.UIString", "Suppressed");

            [HarmonyPrepare]
            static bool ApplyPatch() {
                return ModSettings.Fixes.BaseFixes.IsEnabled("SuppressedBuffUI");
            }

            [HarmonyPatch("SetupIcon"), HarmonyPostfix]
            static void SuppressIcons(CharInfoFeatureView __instance) {
                if (!__instance.ViewModel.IsActive) {
                    __instance.m_Icon.color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
            [HarmonyPatch("SetupDescription"), HarmonyPostfix]
            static void SuppressDesciption(CharInfoFeatureView __instance) {
                if (!__instance.ViewModel.IsActive) {
                    var BuffTemplate = __instance.ViewModel.Tooltip as TooltipTemplateBuff;
                    if (BuffTemplate != null && BuffTemplate.Buff.IsSuppressed) {
                        __instance.m_Description.text = __instance.m_Description.text + "\n" +
                            string.Format("<color=#{1}>{0}</color>",
                                Suppressed,
                                ColorUtility.ToHtmlStringRGB(BlueprintRoot.Instance.UIRoot.AttributeDamageColor)
                            );
                        return;
                    }
                    __instance.m_Description.text = __instance.m_Description.text + "\n" +
                        string.Format("<color=#{1}>{0}</color>",
                            UIStrings.Instance.CharacterSheet.DeactivatedFeature,
                            ColorUtility.ToHtmlStringRGB(BlueprintRoot.Instance.UIRoot.AttributeDamageColor)
                        );
                }
            }
        }
    }
}
