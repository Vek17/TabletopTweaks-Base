using Code.NestedSelectionGroup;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UI.MVVM.CharGen.Phases.Class;
using Kingmaker.UI.MVVM.Tooltip.Templates;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.MechanicsChanges {

    static class ArchetypePrerequisites {

        // Force Archetypes to Consider Prerequisites
        [HarmonyPatch(typeof(CharGenClassSelectorItemVM), "GetArchetypesList", typeof(BlueprintCharacterClass))]
        static class CharGenClassSelectorItemVM_GetArchetypeList_Patch {
            static void Postfix(ref List<NestedSelectionGroupEntityVM> __result, CharGenClassSelectorItemVM __instance, BlueprintCharacterClass selectedClass) {
                if (!Settings.Fixes.EnableArchetypePrerequisites) { return; }
                List<NestedSelectionGroupEntityVM> list = new List<NestedSelectionGroupEntityVM>();
                if (selectedClass == null) {
                    __result = list;
                    return;
                }

                list.AddRange(selectedClass.Archetypes
                    .Select(archetype => new CharGenClassSelectorItemVM(
                        selectedClass,
                        archetype,
                        __instance.LevelUpController,
                        __instance,
                        __instance.SelectedArchetype,
                        __instance.m_InfoVM,
                       archetype.MeetsPrerequisites(__instance.LevelUpController),
                        true)
                    ));
                __result = list;
            }
        }

        // Enforce both Class and Archetype prerequisites for seletion
        [HarmonyPatch(typeof(BlueprintCharacterClass), "MeetsPrerequisites", new Type[] { typeof(UnitDescriptor), typeof(LevelUpState) })]
        static class BlueprintCharacterClass_MeetsPrerequisites_Patch {
            static void Postfix(ref bool __result, BlueprintCharacterClass __instance, UnitDescriptor unit, LevelUpState state) {
                if (!Settings.Fixes.EnableArchetypePrerequisites) { return; }
                bool meetsArchetypePrerequisites = true;
                int classLevel = unit.Progression.GetClassLevel(__instance);
                if (classLevel >= 1) {
                    var archetypes = unit.Progression.GetClassData(__instance).Archetypes;
                    if (archetypes.Count >= 1) {
                        meetsArchetypePrerequisites = !archetypes.Select(archetype => archetype.MeetsPrerequisites(unit, state)).Any(b => !b);
                    }
                    __result = __result && meetsArchetypePrerequisites;
                }
            }
        }

        // Force Archetypes to Display Prerequisites
        [HarmonyPatch(typeof(TooltipTemplateLevelUp), "AddClassPrerequisites")]
        static class TooltipTemplateLevelUp_AddClassPrerequisites_Patch {
            static bool Prefix(TooltipTemplateLevelUp __instance, List<ITooltipBrick> bricks) {
                if (!Settings.Fixes.EnableArchetypePrerequisites) { return true; }
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

                IEnumerable<Prerequisite> components = archetypes.SelectMany(bp => bp.GetComponents<Prerequisite>())
                    .Concat(__instance.ClassInfo.Class.GetComponents<Prerequisite>());
                __instance.AddPrerequisites(bricks, components, __instance.ClassInfo.ParentFeatureSelection);
                return false;
            }
            /*
            static void Postfix(TooltipTemplateLevelUp __instance, List<ITooltipBrick> bricks) {
                if (!Resources.Fixes.EnableArchetypePrerequisites) { return; }
                if (__instance.ClassInfo.Class == null) { return; }
                var unit = __instance.LevelupInfo.Unit;
                var selectionClass = __instance.ClassInfo.Class;
                var archetypes = new List<BlueprintArchetype>();
                if (__instance.ClassInfo.Archetype != null) {
                    archetypes.Add(__instance.ClassInfo.Archetype);
                }
                else if (unit.Progression.GetClassLevel(__instance.ClassInfo.Class) >= 1) {
                    archetypes.AddRange(unit.Progression.GetClassData(__instance.ClassInfo.Class).Archetypes);
                }

                if (archetypes.Count() == 0) { return; }

                IEnumerable<Prerequisite> components = archetypes.SelectMany(bp => bp.GetComponents<Prerequisite>());
                __instance.AddPrerequisites(bricks, components, __instance.ClassInfo.ParentFeatureSelection);
            }
            */
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