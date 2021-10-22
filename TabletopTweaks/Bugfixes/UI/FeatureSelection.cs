using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;

namespace TabletopTweaks.Bugfixes.UI {
    static class FeatureSelection {
        //[HarmonyPatch(typeof(NestedFeatureSelectionUtils), "AllNestedFeaturesUnavailable", new[] { typeof(UnitDescriptor), typeof(LevelUpState), typeof(FeatureSelectionState), typeof(BlueprintFeature) })]
        static class NestedFeatureSelectionUtils_AllNestedFeaturesUnavailable_Patch {
            static bool Prefix(ref bool __result, UnitDescriptor unit, LevelUpState state, FeatureSelectionState selectionState, BlueprintFeature feature) {
                IFeatureSelection selection = (feature as IFeatureSelection);
                if (selection == null) { return true; }
                FeatureSelectionState newSelectionState = new FeatureSelectionState(selectionState, selectionState.Source, selection, 0, 0);
                __result = !selection.CanSelectAny(unit, state, newSelectionState);
                return false;
            }
        }
        //[HarmonyPatch(typeof(FeatureSelectionExtensions), "CanSelectAny", new[] { typeof(IFeatureSelection), typeof(UnitDescriptor), typeof(LevelUpState), typeof(FeatureSelectionState) })]
        static class FeatureSelectionExtensions_CanSelectAny_Patch {
            static bool Prefix(ref bool __result, IFeatureSelection selection, UnitDescriptor unit, LevelUpState state, FeatureSelectionState selectionState) {
                FeatureSelectionState newSelectionState = new FeatureSelectionState(selectionState, selectionState.Source, selection, 0, 0);
                __result = selection.Items.Any((IFeatureSelectionItem item) => selection.CanSelect(unit, state, newSelectionState, item));
                return false;
            }
        }
    }
}