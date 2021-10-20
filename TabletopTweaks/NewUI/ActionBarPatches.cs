using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;

namespace TabletopTweaks.NewUI {
    class ActionBarPatches {

        [HarmonyPatch(typeof(ActionBarSpontaneousConvertedSlot), "Set", new Type[] { typeof(UnitEntityData), typeof(AbilityData) })]
        static class ActionBarSpontaneousConvertedSlot_Set_Patch {
            static bool Prefix(ActionBarSpontaneousConvertedSlot __instance, UnitEntityData selected, AbilityData spell) {
                var pseudoActivatableComponent = spell.Blueprint.GetComponent<PseudoActivatable>();
                if (pseudoActivatableComponent != null) {
                    __instance.Selected = selected;
                    if (selected == null) {
                        return true;
                    }
                    __instance.MechanicSlot = new MechanicActionBarSlotPseudoActivatableAbility {
                        Spell = spell,
                        Unit = selected,
                        BuffToWatch = pseudoActivatableComponent.BuffToWatch
                    };
                    __instance.MechanicSlot.SetSlot(__instance);
                    return false;
                } else if (spell.Blueprint.GetComponent<QuickStudyComponent>()) {
                    __instance.Selected = selected;
                    if (selected == null) {
                        return true;
                    }
                    __instance.MechanicSlot = new MechanicActionBarSlotQuickStudy {
                        Spell = spell,
                        Unit = selected
                    };
                    __instance.MechanicSlot.SetSlot(__instance);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ActionBarSlotVM), "OnShowConvertRequest")]
        static class ActionBarSlotVM_OnShowConvertRequest_Patch {
            static bool Prefix(ActionBarSlotVM __instance) {
                if (__instance.ConvertedVm.Value != null && !__instance.ConvertedVm.Value.IsDisposed) {
                    __instance.CloseConvert();
                    return false;
                }
                if (__instance.m_Conversion.Count == 0) {
                    return false;
                }
                __instance.ConvertedVm.Value = new ActionBarConvertedVM(__instance.m_Conversion.Select(abilityData => {
                    var pseudoActivatable = abilityData.Blueprint.GetComponent<PseudoActivatable>();
                    if (pseudoActivatable != null) {
                        return new MechanicActionBarSlotPseudoActivatableAbility {
                            Spell = abilityData,
                            Unit = __instance.MechanicActionBarSlot.Unit,
                            BuffToWatch = pseudoActivatable.BuffToWatch
                        };
                    } else if (abilityData.Blueprint.GetComponent<QuickStudyComponent>()) {
                        return new MechanicActionBarSlotQuickStudy {
                            Spell = abilityData,
                            Unit = __instance.MechanicActionBarSlot.Unit
                        };
                    } else {
                        return new MechanicActionBarSlotSpontaneusConvertedSpell {
                            Spell = abilityData,
                            Unit = __instance.MechanicActionBarSlot.Unit
                        };
                    }

                }).ToList(), new Action(__instance.CloseConvert));
                return false;
            }
        }
    }
}
