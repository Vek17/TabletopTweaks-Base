using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.Progression.ChupaChupses;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.Progression.Main;
using Kingmaker.UnitLogic;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.UI {
    internal class ProgressionVisibility {
        [HarmonyPatch(typeof(ProgressionVM), nameof(ProgressionVM.PlaceFeatures))]
        static class ProgressionVM_PlaceFeatures_HideUnavailableFeatures_Patch {
            static readonly FieldInfo BlueprintFeatureBase_HideInUI = AccessTools.Field(typeof(BlueprintFeatureBase), "HideInUI");
            static readonly FieldInfo BaseProgressionVM_Unit = AccessTools.Field(typeof(BaseProgressionVM<FeatureProgressionChupaChupsVM>), "Unit");
            static readonly MethodInfo Method_FeatureIsNotAvailable = AccessTools.Method(typeof(ProgressionVM_PlaceFeatures_HideUnavailableFeatures_Patch), "FeatureIsNotAvailable");

            //Subtract the targets save bonus from the DC
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixProgressionDisplay")) { return instructions; }

                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //ILUtils.LogIL(TTTContext, codes);
                codes.InsertRange(target, new CodeInstruction[] {
                    codes[target-3].Clone(),
                    codes[target-2].Clone(),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, BaseProgressionVM_Unit),
                    new CodeInstruction(OpCodes.Call, Method_FeatureIsNotAvailable),
                    new CodeInstruction(OpCodes.Or),
                });
                //ILUtils.LogIL(TTTContext, codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    if (codes[i].LoadsField(BlueprintFeatureBase_HideInUI)) {
                        return i+1;
                    }
                }
                TTTContext.Logger.LogError("ProgressionVM_PlaceFeatures_HideUnavailableFeatures_Patch: COULD NOT FIND TARGET");
                return -1;
            }
            private static bool FeatureIsNotAvailable(BlueprintFeatureBase baseFeature, UnitDescriptor unit) {
                var feature = baseFeature as BlueprintFeature;
                if (feature == null) { return false; }
                if (unit == null) { return false; }
                if (!feature.HideNotAvailibleInUI) { return false; }

                return !feature.MeetsPrerequisites(null, unit, null, true);
            }
        }
    }
}
