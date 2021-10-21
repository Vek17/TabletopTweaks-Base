using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items.Slots;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.NewUnitParts;

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
                    __instance.MechanicSlot = new MechanicActionBarSlotPseudoActivatableAbilityVariant {
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
                        var slot = new MechanicActionBarSlotPseudoActivatableAbilityVariant {
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
                Main.LogDebug("UnitUISettings.AbilityWrapper.CreateSlot");
                if (__instance.SpellSlot == null && __instance.SpontaneousSpell == null && __instance.Ability != null) {
                    Main.LogDebug("UnitUISettings.AbilityWrapper.CreateSlot: patched section");
                    var pseudoActivatableComponent = __instance.Ability.Blueprint.GetComponent<PseudoActivatable>();
                    if (pseudoActivatableComponent != null) {
                        Main.LogDebug("UnitUISettings.AbilityWrapper.CreateSlot Patch: returning MechanicActionBarSlotPseudoActivatableAbility");
                        __result = new MechanicActionBarSlotPseudoActivatableAbility {
                            Ability = __instance.Ability.Data,
                            Unit = unit,
                            BuffToWatch = pseudoActivatableComponent.BuffToWatch
                        };
                        unit.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(__result);
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
                            slot.Unit.Ensure<UnitPartPseudoActivatableAbilities>().RegisterPseudoActivatableAbilitySlot(__result);
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ActionBarGroupElement), nameof(ActionBarGroupElement.FillSlots))]
        static class ActionBarGroupElement_FillSlots_Patch {
            static bool Prefix(ActionBarGroupElement __instance) {
                if (!__instance.PreInit) {
                    __instance.Dispose();
                }
                int index = 0;
                Main.LogDebug($"ActionBarGroupElement.FillSlots patch");
                switch (__instance.SlotType) {
                    case ActionBarSlotType.Spell: {
                            Main.LogDebug($"ActionBarGroupElement.FillSlots patch: spell");
                            ActionBarSubGroupLevels levels = __instance.m_Levels;
                            if (levels != null) {
                                levels.Clear();
                            }
                            List<AbilityData> list = new List<AbilityData>();
                            foreach (Ability ability in __instance.m_Selected.Abilities.Visible) {
                                if (ability.Blueprint.IsCantrip) {
                                    AbilityData data = ability.Data;
                                    if (!list.Contains(data)) {
                                        list.Add(data);
                                        ActionBarSubGroupLevels levels2 = __instance.m_Levels;
                                        if (levels2 != null) {
                                            var pseudoActivatableComponent = data.Blueprint.GetComponent<PseudoActivatable>();
                                            if (pseudoActivatableComponent != null) {
                                                Main.LogDebug($"ActionBarGroupElement.FillSlots patch: adding pseudo activatable ability for {data.Blueprint.NameSafe()}");
                                                levels2.AddSlot(0, new MechanicActionBarSlotPseudoActivatableAbility {
                                                    Ability = data,
                                                    Unit = __instance.m_Selected,
                                                    BuffToWatch = pseudoActivatableComponent.BuffToWatch
                                                });
                                            } else {
                                                levels2.AddSlot(0, new MechanicActionBarSlotAbility {
                                                    Ability = data,
                                                    Unit = __instance.m_Selected
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                            foreach (Spellbook spellbook in __instance.m_Selected.Descriptor.Spellbooks) {
                                if (spellbook.Blueprint.MemorizeSpells) {
                                    for (int i = 0; i <= spellbook.MaxSpellLevel; i++) {
                                        foreach (SpellSlot spellSlot in spellbook.GetMemorizedSpells(i)) {
                                            if (!list.Contains(spellSlot.Spell)) {
                                                list.Add(spellSlot.Spell);
                                                ActionBarSubGroupLevels levels3 = __instance.m_Levels;
                                                if (levels3 != null) {
                                                    levels3.AddSlot(i, new MechanicActionBarSlotMemorizedSpell(spellSlot) {
                                                        Unit = __instance.m_Selected
                                                    });
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    for (int j = 1; j <= spellbook.MaxSpellLevel; j++) {
                                        List<AbilityData> list2 = spellbook.GetSpecialSpells(j).Concat(spellbook.GetKnownSpells(j)).Distinct<AbilityData>().ToList<AbilityData>();
                                        List<AbilityData> collection = spellbook.GetCustomSpells(j).ToList<AbilityData>();
                                        list2.AddRange(collection);
                                        foreach (AbilityData abilityData in list2) {
                                            if (!list.Contains(abilityData)) {
                                                list.Add(abilityData);
                                                ActionBarSubGroupLevels levels4 = __instance.m_Levels;
                                                if (levels4 != null) {
                                                    levels4.AddSlot(j, new MechanicActionBarSlotSpontaneousSpell(abilityData) {
                                                        Unit = __instance.m_Selected
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            ActionBarSubGroupLevels levels5 = __instance.m_Levels;
                            if (levels5 == null) {
                                return false;
                            }
                            levels5.SetType(__instance.SlotType);
                            return false;
                        }
                    case ActionBarSlotType.Item:
                        foreach (UsableSlot usableSlot in __instance.m_Selected.Body.QuickSlots) {
                            ActionBarGroupSlot slot = __instance.GetSlot(index++);
                            if (usableSlot.HasItem) {
                                slot.Set(__instance.m_Selected, new MechanicActionBarSlotItem {
                                    Item = usableSlot.Item,
                                    Unit = __instance.m_Selected
                                });
                            } else {
                                slot.Set(__instance.m_Selected, new MechanicActionBarSlotEmpty());
                            }
                        }
                        break;
                    case ActionBarSlotType.ActivatableAbility:
                        foreach (Ability ability2 in __instance.m_Selected.Abilities.Visible) {
                            if (!ability2.Blueprint.IsCantrip) {
                                __instance.GetSlot(index++).Set(__instance.m_Selected, new MechanicActionBarSlotAbility {
                                    Ability = ability2.Data,
                                    Unit = __instance.m_Selected
                                });
                            }
                        }
                        foreach (ActivatableAbility activatableAbility in __instance.m_Selected.ActivatableAbilities.Visible) {
                            __instance.GetSlot(index++).Set(__instance.m_Selected, new MechanicActionBarSlotActivableAbility {
                                ActivatableAbility = activatableAbility,
                                Unit = __instance.m_Selected
                            });
                        }
                        break;
                }
                __instance.AddEmptySlots(index);
                return false;
            }
        }*/
        
    }
}
