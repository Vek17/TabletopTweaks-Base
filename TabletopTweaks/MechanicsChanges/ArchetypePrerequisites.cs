using Code.NestedSelectionGroup;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UI.MVVM.CharGen.Phases.Class;
using Kingmaker.UI.MVVM.Tooltip.Templates;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.MechanicsChanges {

    // UI Hack to remove unqualified Archetypes from the list
    [HarmonyPatch(typeof(CharGenClassSelectorItemVM), "GetArchetypesList", typeof(BlueprintCharacterClass))]
    static class CharGenClassPhaseVM_GetArchetypeList_patch {
        static void Postfix(ref List<NestedSelectionGroupEntityVM>  __result, CharGenClassSelectorItemVM __instance, BlueprintCharacterClass selectedClass) {
            List<NestedSelectionGroupEntityVM> list = new List<NestedSelectionGroupEntityVM>();
            if (selectedClass == null) {
                __result =  list;
                return;
            }
            /*
            list.AddRange((from archetype in selectedClass.Archetypes
                           select new CharGenClassSelectorItemVM(selectedClass, archetype, __instance.LevelUpController, __instance, __instance.SelectedArchetype, __instance.m_InfoVM, MeetsPrerequisites(archetype), true)).ToList<CharGenClassSelectorItemVM>()) ;
            */

            list.AddRange(selectedClass.Archetypes
                .Select(archetype => new CharGenClassSelectorItemVM(
                    selectedClass, 
                    archetype, 
                    __instance.LevelUpController, 
                    __instance, 
                    __instance.SelectedArchetype, 
                    __instance.m_InfoVM, 
                    MeetsPrerequisites(archetype), 
                    true)
                ));
            __result = list;

            bool MeetsPrerequisites(BlueprintArchetype archetype) {
                UnitDescriptor unit = __instance.LevelUpController.Unit;
                LevelUpState state = __instance.LevelUpController.State;
                if (IgnorePrerequisites.Ignore) {
                    return true;
                }
                int classLevel = unit.Progression.GetClassLevel(archetype.GetParentClass());
                if (classLevel >= 20) {
                    return false;
                }
                bool? flag = null;
                bool? flag2 = null;
                for (int i = 0; i < archetype.ComponentsArray.Length; i++) {
                    Prerequisite prerequisite = archetype.ComponentsArray[i] as Prerequisite;
                    if (prerequisite) {
                        bool flag3 = prerequisite.Check(null, unit, state);
                        switch (prerequisite.Group) {
                            case Prerequisite.GroupType.All:
                                flag = new bool?((flag == null) ? flag3 : (flag.Value && flag3));
                                break;
                            case Prerequisite.GroupType.Any:
                                flag2 = new bool?((flag2 == null) ? flag3 : (flag2.Value || flag3));
                                break;
                            case Prerequisite.GroupType.ForcedTrue:
                                if (flag3) {
                                    return true;
                                }
                                break;
                        }
                    }
                }
                return (flag ?? true) && (flag2 ?? true);
            }
        }
    }

    [HarmonyPatch(typeof(TooltipTemplateLevelUp), "AddClassPrerequisites")]
    static class CharGenClassPhaseVM_AddClassPrerequisites_patch {
        static void Postfix(TooltipTemplateLevelUp __instance, List<ITooltipBrick> bricks) {
            if (__instance.ClassInfo.Archetype == null) { return; }
            if (__instance.ClassInfo.Class == null) {
                return;
            }
            IEnumerable<Prerequisite> components = __instance.ClassInfo.Archetype.GetComponents<Prerequisite>();
            __instance.AddPrerequisites(bricks, components, __instance.ClassInfo.ParentFeatureSelection);
        }
    }
}