#if false
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Class;
using Kingmaker.UI.MVVM._VM.Other.NestedSelectionGroup;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.NewComponents;

namespace TabletopTweaks.MechanicsChanges {

    static class ArchetypePrerequisites {

        // Force Archetypes to Consider Prerequisites
        [HarmonyPatch(typeof(CharGenClassSelectorItemVM), "GetArchetypesList", typeof(BlueprintCharacterClass))]
        static class CharGenClassSelectorItemVM_GetArchetypeList_Patch {
            static void Postfix(ref List<NestedSelectionGroupEntityVM> __result, CharGenClassSelectorItemVM __instance, BlueprintCharacterClass selectedClass) {
                //if (!ModSettings.Fixes.EnableArchetypePrerequisites) { return; }
                List<NestedSelectionGroupEntityVM> list = new List<NestedSelectionGroupEntityVM>();
                if (selectedClass == null) {
                    __result = list;
                }
                list.AddRange((from archetype in selectedClass.Archetypes
                               select new CharGenClassSelectorItemVM(selectedClass, archetype,
                               levelUpController: __instance.LevelUpController,
                               source: __instance,
                               selectedArchetype: __instance.SelectedArchetype,
                               tooltipTemplate: __instance.m_TooltipTemplate,
                               prerequisitesDone: __instance.IsArchetypeAvailable(__instance.LevelUpController, archetype),
                               canSelect: __instance.LevelUpController.State.IsFirstCharacterLevel || __instance.IsArchetypeAvailable(__instance.LevelUpController, archetype),
                               allowSwitchOff: true))
                               .ToList<CharGenClassSelectorItemVM>());
                __result = list;
            }
        }
        [HarmonyPatch(typeof(CharGenClassPhaseVM), "CreateClassListSelector")]
        static class CharGenClassPhaseVM_CreateClassListSelector_Patch {
            static void Postfix(CharGenClassPhaseVM __instance) {
                //if (!ModSettings.Fixes.EnableArchetypePrerequisites) { return; }
                ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference> referenceArrayProxy = (__instance.LevelUpController.State.Mode == LevelUpState.CharBuildMode.Mythic) ? Game.Instance.BlueprintRoot.Progression.CharacterMythics : Game.Instance.BlueprintRoot.Progression.CharacterClasses;
                __instance.m_ClassesVMs = (from cls in referenceArrayProxy
                                     where cls.IsDlcAvailable() && (CharGenClassPhaseVM.MeetsPrerequisites(__instance.LevelUpController, cls) || !cls.HideIfRestricted)
                                     select new CharGenClassSelectorItemVM(cls, null, __instance.LevelUpController, __instance, __instance.SelectedArchetypeVM, __instance.ReactiveTooltipTemplate, CharGenClassPhaseVM.IsClassAvailable(__instance.LevelUpController, cls), true, true)).ToList<CharGenClassSelectorItemVM>();
                __instance.ClassSelector = new NestedSelectionGroupRadioVM<CharGenClassSelectorItemVM>(__instance);
            }
        }
        [HarmonyPatch(typeof(BlueprintCharacterClass), "MeetsPrerequisites", new Type[] { typeof(UnitDescriptor), typeof(LevelUpState) })]
        static class BlueprintCharacterClass_MeetsPrerequisites_Patch {
            static void Postfix(ref bool __result, BlueprintCharacterClass __instance, UnitDescriptor unit, LevelUpState state) {
                //if (!ModSettings.Fixes.EnableArchetypePrerequisites) { return; }
                bool meetsArchetypePrerequisites = true;
                int classLevel = unit.Progression.GetClassLevel(__instance);
                if (classLevel >= 1) {
                    var archetypes = unit.Progression.GetClassData(__instance).Archetypes;
                    if (archetypes.Count >= 1) {
                        archetypes = archetypes.Where(a => a.ComponentsArray.Where(c => c is IgnoreClassPrerequisites).Count() > 0).ToList();
                        if(archetypes.Count > 1) {
                            __result = archetypes.Aggregate(true, (bool pass, BlueprintArchetype archetype) => pass &= archetype.MeetsPrerequisites(unit, state));
                        }
                    }
                    __result = __result && meetsArchetypePrerequisites;
                }
            }
        }

        [HarmonyPatch(typeof(BlueprintArchetype), "MeetsPrerequisites", new Type[] { typeof(UnitDescriptor), typeof(LevelUpState) })]
        static class BlueprintArchetype_MeetsPrerequisites_Patch {
            static void Postfix(ref bool __result, BlueprintArchetype __instance, UnitDescriptor unit, LevelUpState state) {
                //if (!ModSettings.Fixes.EnableArchetypePrerequisites) { return; }
                Main.LogDebug($"{__instance.name}");
                var temp = __instance.GetComponent<IgnoreClassPrerequisites>();
                if (__instance.GetComponent<IgnoreClassPrerequisites>() != null) {
                    Main.LogDebug($"IgnoreClassPrerequisites");
                    int classLevel = unit.Progression.GetClassLevel(__instance.GetParentClass());
                    if (IgnorePrerequisites.Ignore) { __result = true; return; }
                    if (classLevel >= 20) { __result = false; return; }

                    bool? All = null;
                    bool? Any = null;
                    foreach (Prerequisite prerequisite in __instance.GetComponents<Prerequisite>()) {
                        Main.LogDebug($"{prerequisite.name}");
                        bool Check = prerequisite.Check(null, unit, state);
                        switch (prerequisite.Group) {
                            case Prerequisite.GroupType.All:
                                All = (All == null) ? Check : All.Value && Check;
                                break;
                            case Prerequisite.GroupType.Any:
                                Any = (Any == null) ? Check : Any.Value || Check;
                                break;
                            case Prerequisite.GroupType.ForcedTrue:
                                if (Check) { __result = true; return; }
                                break;
                        }
                    }
                    Main.LogDebug($"All: {(All ?? true)}");
                    Main.LogDebug($"Any: {(Any ?? true)}");
                    __result = (All ?? true) & (Any ?? true);
                }
            }
        }
        // Enforce both Class and Archetype prerequisites for seletion
        [HarmonyPatch(typeof(BlueprintCharacterClass), "RestrictPrerequisites", new Type[] { typeof(UnitDescriptor), typeof(LevelUpState) })]
        static class BlueprintCharacterClass_RestrictPrerequisites_Patch {
            static void Postfix(BlueprintCharacterClass __instance, UnitDescriptor unit, LevelUpState state) {
                //if (!ModSettings.Fixes.EnableArchetypePrerequisites) { return; }
                int classLevel = unit.Progression.GetClassLevel(__instance);
                if (classLevel >= 1) {
                    var archetypes = unit.Progression.GetClassData(__instance).Archetypes;
                    if (archetypes.Count >= 1) {
                        archetypes = archetypes.Where(a => a.ComponentsArray.Where(c => c is IgnoreClassPrerequisites).Count() > 0).ToList();
                    }
                    //__result = __result && meetsArchetypePrerequisites;
                }
            }
        }
        //IMPORTANT
        [HarmonyPatch(typeof(AddArchetype), "Apply", new Type[] { typeof(LevelUpState), typeof(UnitDescriptor) })]
        static class AddArchetype_Apply_Patch {
            static bool Prefix(AddArchetype __instance, LevelUpState state, UnitDescriptor unit) {
                Main.LogDebug($"{__instance.Archetype.name} AddArchetype - Apply");
                if(__instance.Archetype.GetComponent<IgnoreClassPrerequisites>() != null) {
                    state.AlignmentRestriction.m_Entries.Clear();
                }
                return true;
                //state.SelectedClass.RestrictPrerequisites(unit, state);
            }
        }

        [HarmonyPatch(typeof(SelectClass), "Apply", new Type[] { typeof(LevelUpState), typeof(UnitDescriptor) })]
        static class SelectClass_Apply_Patch {
            static void Postfix(LevelUpState state, UnitDescriptor unit) {
                Main.LogDebug($"{state.SelectedClass.name} SelectClass - Apply");
                //state.SelectedClass.RestrictPrerequisites(unit, state);
            }
        }

        // Enable Class Prerequisite Ignore
        [HarmonyPatch(typeof(TooltipTemplateLevelUp), "AddClassPrerequisites")]
        static class TooltipTemplateLevelUp_AddClassPrerequisites_Patch {
            static bool Prefix(TooltipTemplateLevelUp __instance, List<ITooltipBrick> bricks) {
                //if (!ModSettings.Fixes.EnableArchetypePrerequisites) { return true; }
                if (__instance.ClassInfo.Class == null) { return true; }
                var unit = __instance.LevelupInfo.Unit;
                var selectionClass = __instance.ClassInfo.Class;
                var archetypes = new List<BlueprintArchetype>();
                if (__instance.ClassInfo.Archetype != null) {
                    archetypes.Add(__instance.ClassInfo.Archetype);
                } else if (unit.Progression.GetClassLevel(__instance.ClassInfo.Class) >= 1) {
                    archetypes.AddRange(unit.Progression.GetClassData(__instance.ClassInfo.Class).Archetypes);
                }

                if (archetypes.Count() == 0) { return true; }
                Main.LogDebug($"{__instance.ClassInfo.Archetype.name}: Checking for ignore");
                archetypes = archetypes.Where(a => a.ComponentsArray.Where(c => c is IgnoreClassPrerequisites).Count() > 0).ToList();
                if(archetypes.Count() > 0) {
                    Main.LogDebug($"{__instance.ClassInfo.Archetype.name}: Ignoring");
                    IEnumerable<Prerequisite> components = archetypes.SelectMany(bp => bp.GetComponents<Prerequisite>());
                    __instance.AddPrerequisites(bricks, components, __instance.ClassInfo.ParentFeatureSelection);
                    return false;
                }
                return true;
            }
        }

        private static bool MeetsPrerequisites(this BlueprintArchetype archetype, UnitDescriptor unit, LevelUpState state) {
            int classLevel = unit.Progression.GetClassLevel(archetype.GetParentClass());
            if (IgnorePrerequisites.Ignore) { return true; }
            if (classLevel >= 20) { return false; }

            bool? All = null;
            bool? Any = null;
            foreach (Prerequisite prerequisite in archetype.GetComponents<Prerequisite>()) {
                bool Check = prerequisite.Check(null, unit, state);
                switch (prerequisite.Group) {
                    case Prerequisite.GroupType.All:
                        All = (All == null) ? Check : All.Value && Check;
                        break;
                    case Prerequisite.GroupType.Any:
                        Any = (Any == null) ? Check : Any.Value || Check;
                        break;
                    case Prerequisite.GroupType.ForcedTrue:
                        if (Check) { return true; }
                        break;
                }
            }
            return (All ?? true) & (Any ?? true);
        }
        private static bool MeetsPrerequisites(this BlueprintArchetype archetype, LevelUpController levelUpController) {
            UnitDescriptor unit = levelUpController.Unit;
            LevelUpState state = levelUpController.State;
            return archetype.MeetsPrerequisites(unit, state);
        }
    }
}
#endif