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

    // Forces Archetypes to Consider Prerequisites
    [HarmonyPatch(typeof(CharGenClassSelectorItemVM), "GetArchetypesList", typeof(BlueprintCharacterClass))]
    static class CharGenClassPhaseVM_GetArchetypeList_patch {
        static void Postfix(ref List<NestedSelectionGroupEntityVM>  __result, CharGenClassSelectorItemVM __instance, BlueprintCharacterClass selectedClass) {
            List<NestedSelectionGroupEntityVM> list = new List<NestedSelectionGroupEntityVM>();
            if (selectedClass == null) {
                __result =  list;
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
                    MeetsPrerequisites(archetype), 
                    true)
                ));
            __result = list;

            bool MeetsPrerequisites(BlueprintArchetype archetype) {
                UnitDescriptor unit = __instance.LevelUpController.Unit;
                LevelUpState state = __instance.LevelUpController.State;
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
        }
    }

    // Forces Archetypes to Display Prerequisites
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