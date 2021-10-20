using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Linq;
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
                    Main.LogDebug("ActionBarSpontaneousConvertedSlot_Set_Patch: registering mechanics slot in unit part");
                    selected.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(__instance.MechanicSlot);
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
                        Main.LogDebug("ActionBarSlotVM_OnShowConvertRequest_Patch: registering mechanics slot in unit part");
                        __instance.MechanicActionBarSlot.Unit.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(slot);
                        return slot;
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

        [HarmonyPatch(typeof(ActionBarVM), nameof(ActionBarVM.CollectAbilities))]
        static class ActionBarVM_CollectAbilities_Patch {
            static bool Prefix(ActionBarVM __instance, UnitEntityData unit) {
                foreach (Ability ability in unit.Abilities) {
                    if (!ability.Hidden && !ability.Blueprint.IsCantrip) {
                        List<ActionBarSlotVM> groupAbilities = __instance.GroupAbilities;
                        if (ability.GetComponent<PseudoActivatable>() != null) {
                            MechanicActionBarSlotPseudoActivatableAbility actionBarSlotPseudoActivatableAbility = new MechanicActionBarSlotPseudoActivatableAbility {
                                Ability = ability.Data,
                                Unit = unit,
                                BuffToWatch = ability.GetComponent<PseudoActivatable>().BuffToWatch
                            };
                            Main.LogDebug("ActionBarVM_CollectAbilities_Patch: registering mechanics slot in unit part");
                            unit.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(actionBarSlotPseudoActivatableAbility);
                            ActionBarSlotVM actionBarSlotVm = new ActionBarSlotVM(actionBarSlotPseudoActivatableAbility);
                            groupAbilities.Add(actionBarSlotVm);
                        } else {
                            MechanicActionBarSlotAbility actionBarSlotAbility = new MechanicActionBarSlotAbility();
                            actionBarSlotAbility.Ability = ability.Data;
                            actionBarSlotAbility.Unit = unit;
                            ActionBarSlotVM actionBarSlotVm = new ActionBarSlotVM((MechanicActionBarSlot)actionBarSlotAbility);
                            groupAbilities.Add(actionBarSlotVm);
                        }
                    }
                }
                foreach (ActivatableAbility activatableAbility in unit.ActivatableAbilities) {
                    List<ActionBarSlotVM> groupAbilities = __instance.GroupAbilities;
                    MechanicActionBarSlotActivableAbility activableAbility = new MechanicActionBarSlotActivableAbility();
                    activableAbility.ActivatableAbility = activatableAbility;
                    activableAbility.Unit = unit;
                    ActionBarSlotVM actionBarSlotVm = new ActionBarSlotVM((MechanicActionBarSlot)activableAbility);
                    groupAbilities.Add(actionBarSlotVm);
                }
                return false;
            }
        }

        /*[HarmonyPatch(typeof(UnitUISettings.AbilityWrapper), nameof(UnitUISettings.AbilityWrapper.CreateSlot))]
        static class UnitUISettings_CreateSlot_Patch {
            static bool Prefix(UnitUISettings.AbilityWrapper __instance, UnitEntityData unit, ref MechanicActionBarSlot __result) {
                Main.LogDebug("UnitUISettings.AbilityWrapper.CreateSlot Patch");
                if (__instance.SpellSlot == null && __instance.SpontaneousSpell == null && __instance.Ability != null) {
                    Main.LogDebug($"UnitUISettings.AbilityWrapper.CreateSlot Patch: Ability: {__instance.Ability.Name}");
                    var pseudoActivatableComponent = __instance.Ability.Blueprint.GetComponent<PseudoActivatable>();
                    Main.LogDebug($"UnitUISettings.AbilityWrapper.CreateSlot Patch: pseudoActivatableComponent != null : {pseudoActivatableComponent != null}");
                    if (pseudoActivatableComponent != null) {
                        Main.LogDebug("UnitUISettings.AbilityWrapper.CreateSlot Patch: returning MechanicActionBarSlotPseudoActivatableAbility");
                        __result = new MechanicActionBarSlotPseudoActivatableAbility {
                            Ability = __instance.Ability.Data,
                            Unit = unit,
                            BuffToWatch = pseudoActivatableComponent.BuffToWatch
                        };
                        return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(UnitUISettings), nameof(UnitUISettings.GetBadSlotReplacement))]
        static class UnitUISettings_GetBadSlotReplacement_Patch {
            static bool Prefix(UnitUISettings __instance, MechanicActionBarSlot slot, UnitDescriptor owner, ref MechanicActionBarSlot __result) {
                Main.LogDebug("UnitUISetings.GetBadSlotReplacement Patch");
                if (owner == null) {
                    Main.LogDebug("UnitUISetings.GetBadSlotReplacement Patch: owner is null");
                    __result = null;
                    return false;
                }
                if (slot is MechanicActionBarSlotAbility abilitySlot) {
                    Main.LogDebug("UnitUISetings.GetBadSlotReplacement Patch: slot is MechanicActionBarSlotAbility");
                    Ability ability = owner.Abilities.Visible.FirstOrDefault<Ability>((Func<Ability, bool>)(a => a.Blueprint == abilitySlot.Ability.Blueprint && a.SourceItem == abilitySlot.Ability.SourceItem));
                    if (ability != null) {
                        Main.LogDebug($"UnitUISetings.GetBadSlotReplacement Patch: ability is not null. Name: {ability.Name}");
                        Main.LogDebug($"UnitUISetings.GetBadSlotReplacement Patch: ability.GetComponent<PseudoActivatable>() != null : {ability.GetComponent<PseudoActivatable>() != null}");
                        if (ability.GetComponent<PseudoActivatable>() != null) {
                            Main.LogDebug($"UnitUISetings.GetBadSlotReplacement Patch: returning new pseudo activatable ability");
                            __result = new MechanicActionBarSlotPseudoActivatableAbility {
                                Ability = ability.Data,
                                Unit = slot.Unit,
                                BuffToWatch = ability.GetComponent<PseudoActivatable>().BuffToWatch
                            };
                            return false;
                        }
                    }
                }
                return true;
            }
        }*/
    }
}
