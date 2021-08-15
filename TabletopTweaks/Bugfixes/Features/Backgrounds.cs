using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Features {
    class Backgrounds {

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (!ModSettings.Fixes.FixBackgroundModifiers) { return; }
                PatchBackgrounds();
                Main.LogHeader("Patched Backgrounds");

                void PatchBackgrounds() {
                    var BackgroundsBaseSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("f926dabeee7f8a54db8f2010b323383c");
                    BackgroundsBaseSelection.m_Features
                        .Where(f => f.Get() is BlueprintFeatureSelection)
                        .SelectMany(f => ((BlueprintFeatureSelection)f.Get()).m_Features)
                        .Select(f => f.Get())
                        .OfType<BlueprintFeature>()
                        .ForEach(f => {
                            if (!f.Description.Contains("competence bonus")) { return; }
                            f.SetDescription(f.Description.Replace("competence bonus", "trait bonus"));
                            Main.LogPatch("Patched", f);
                        });
                }
            }
        }

        [HarmonyPatch(typeof(ModifiableValueSkill), "UpdateInternalModifiers")]
        static class Backgrounds_Descriptor_Patch {
            static readonly MethodInfo Modifier_AddModifier = AccessTools.Method(typeof(ModifiableValue), "AddModifier", new Type[] {
                typeof(int),
                typeof(EntityFact),
                typeof(ModifierDescriptor)
            });
            //Change bonus descriptor to Trait instead of Competence
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (!ModSettings.Fixes.FixBackgroundModifiers) { return instructions; }
                var codes = new List<CodeInstruction>(instructions);
                int target = FindInsertionTarget(codes);
                //Utilities.ILUtils.LogIL(codes);
                codes[target] = new CodeInstruction(OpCodes.Ldc_I4, (int)ModifierDescriptor.Trait);
                //Utilities.ILUtils.LogIL(codes);
                return codes.AsEnumerable();
            }
            private static int FindInsertionTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count; i++) {
                    //Add modifer is called only once directly after the descriptor is loaded onto the stack
                    if (codes[i].opcode == OpCodes.Call && codes[i].Calls(Modifier_AddModifier)) {
                        return i - 1;
                    }
                }
                Main.Error("BACKGROUND PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
