using HarmonyLib;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.EntitySystem;
using Kingmaker.UI;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.UI {
    static class EnchantmentDamageToolTips {
        [HarmonyPatch(typeof(StatModifiersBreakdown), nameof(StatModifiersBreakdown.GetBonusSourceText), new Type[] {
            typeof(IUIDataProvider),
            typeof(bool)
        })]
        class StatModifiersBreakdown_GetBonusSourceText_Patch {
            static bool Prefix(IUIDataProvider source, bool warningIfEmpty, ref string __result) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("EnchantmentModifiersBreakdown")) { return true; }

                if (source == null) {
                    __result = string.Empty;
                    return false;
                }
                ItemEnchantment itemEnchantment = source as ItemEnchantment;
                string text;
                if (itemEnchantment != null) {
                    text = itemEnchantment.Blueprint.Name;
                    if (string.IsNullOrEmpty(text) && itemEnchantment.Owner != null) {
                        text = itemEnchantment.Owner.Name;
                    }
                    if (string.IsNullOrEmpty(text)) {
                        text = source.Name;
                    }
                } else {
                    text = source.Name;
                }
                if (string.IsNullOrEmpty(text)) {
                    EntityFact entityFact = source as EntityFact;
                    if (entityFact != null) {
                        MechanicsContext maybeContext = entityFact.MaybeContext;
                        bool flag;
                        if (maybeContext == null) {
                            flag = (null != null);
                        } else {
                            MechanicsContext parentContext = maybeContext.ParentContext;
                            flag = (((parentContext != null) ? parentContext.SourceItem : null) != null);
                        }
                        if (flag) {
                            MechanicsContext maybeContext2 = entityFact.MaybeContext;
                            string text2;
                            if (maybeContext2 == null) {
                                text2 = null;
                            } else {
                                MechanicsContext parentContext2 = maybeContext2.ParentContext;
                                text2 = ((parentContext2 != null) ? parentContext2.SourceItem.Name : null);
                            }
                            text = text2;
                            goto IL_9F;
                        }
                    }
                    UnityEngine.Object @object = source as UnityEngine.Object;
                    if (@object != null) {
                        text = @object.name;
                    }
                }
                IL_9F:
                if (warningIfEmpty && BuildModeUtility.IsDevelopment && string.IsNullOrEmpty(text)) {
                    text = "[NO DISPLAY NAME] " + source.NameForAcronym;
                }
                __result = text;
                return false;
            }
        }
    }
}
