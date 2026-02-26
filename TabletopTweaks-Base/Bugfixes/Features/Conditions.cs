using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Features {
    class Conditions {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Conditions");
                PatchStaggered();
                PatchNauseated();

                static void PatchStaggered() {
                    if (TTTContext.Fixes.BaseFixes.IsDisabled("StaggeredDescriptors")) { return; }
                    var Staggered = BlueprintTools.GetBlueprint<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3");
                    Staggered.GetComponent<SpellDescriptorComponent>().Descriptor = SpellDescriptor.Staggered;
                    TTTContext.Logger.LogPatch("Patched", Staggered);
                }

                static void PatchNauseated() {
                    if (TTTContext.Fixes.BaseFixes.IsDisabled("NauseatedDescriptors")) { return; }
                    var Nauseated = BlueprintTools.GetBlueprint<BlueprintBuff>("956331dba5125ef48afe41875a00ca0e");
                    Nauseated.GetComponent<SpellDescriptorComponent>().Descriptor = SpellDescriptor.Nauseated;
                    TTTContext.Logger.LogPatch("Patched", Nauseated);
                }
            }
        }

        /*
            Removes the following IL:

            //     if (this.Descriptor.State.HasCondition(UnitCondition.Staggered))

            IL_0078: ldarg.0
		    IL_0079: call instance class Kingmaker.UnitLogic.UnitDescriptor Kingmaker.EntitySystem.Entities.UnitEntityData::get_Descriptor()
            IL_007E: ldfld class Kingmaker.UnitLogic.UnitState Kingmaker.UnitLogic.UnitDescriptor::State
            IL_0083: ldc.i4.6
            IL_0084: callvirt instance bool Kingmaker.UnitLogic.UnitState::HasCondition(valuetype Kingmaker.UnitLogic.UnitCondition)
            IL_0089: brfalse.s IL_0093

            //         num *= 0.5f;

		    IL_008B: ldloc.1
		    IL_008C: ldc.r4    0.5
		    IL_0091: mul
            IL_0092: stloc.1


            This invalidates the label at IL_0078, so we need to find the next jump point, and update all the jump points to go
            there instead.
        */
        [HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.CalculateSpeedModifier))]
        static class UnitEntityData_CalculateSpeedModifier_Transpile {
            static readonly MethodInfo UnitEntityData_Descriptor = AccessTools.PropertyGetter(typeof(UnitEntityData), nameof(UnitEntityData.Descriptor));

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("StaggeredDescriptors")) { return instructions; }

                var codes = instructions.ToList();
                int target = FindTarget(codes);
                if(target == -1) { return instructions; }

                List<Label> removedLabels = codes[target].labels;
                if (codes[target+10].labels.Empty()) {
                    TTTContext.Logger.LogError("UnitEntityData CalculateSpeedModifier: could not find next label");
                }
                Label newLabel = codes[target + 10].labels[0];

                // Replace existing jump instructions to use the label after the removed block
                for(int i = 0; i < codes.Count; ++i) {
                    if (codes[i].operand is Label l && removedLabels.Contains(l)) {
                        codes[i].operand = newLabel;
                    }
                }

                // Remove the block
                codes.RemoveRange(target, 10);

                return codes;
            }

            static int FindTarget(List<CodeInstruction> codes) {
                for (int i = 0; i < codes.Count - 10; i++) {
                    if (codes[i].opcode == OpCodes.Ldarg_0
                        && codes[i+3].opcode == OpCodes.Ldc_I4_6) {
                        return i;
                    }
                }

                TTTContext.Logger.LogError("UnitEntityData CalculateSpeedModifier: could not find target");
                return -1;
            }
        }
    }
}
