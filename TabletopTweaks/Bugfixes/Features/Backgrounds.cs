using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Features {
    class Backgrounds {

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.BaseFixes.IsDisabled("FixBackgroundModifiers")) { return; }
                Main.LogHeader("Patching Backgrounds");
                PatchMiner();
                PatchFarmhand();
                PatchBackgrounds();

                void PatchBackgrounds() {
                    var BackgroundsBaseSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("f926dabeee7f8a54db8f2010b323383c");
                    BackgroundsBaseSelection.m_AllFeatures
                        .Where(f => f.Get() is BlueprintFeatureSelection)
                        .SelectMany(f => ((BlueprintFeatureSelection)f.Get()).m_AllFeatures)
                        .Select(f => f.Get())
                        .OfType<BlueprintFeature>()
                        .ForEach(f => {
                            bool changed = false;
                            string description = f.Description;
                            f.GetComponents<AddBackgroundWeaponProficiency>()
                                .ForEach(c => c.StackBonusType = ModifierDescriptor.Trait);
                            if (f.Description.Contains("competence")) {
                                description = description.Replace("competence", "trait");
                                changed = true;
                            }
                            if (f.Description.Contains("enhancement")) {
                                description = description.Replace("enhancement", "trait");
                                changed = true;
                            }
                            if (changed) {
                                f.SetDescription(Helpers.MergeIds("00df06db39104890a15801a0d018e922", f.AssetGuid.ToString()), description);
                                Main.LogPatch("Patched", f);
                            }
                        });
                }
                void PatchMiner() {
                    var EarthBreakerProficiency = Resources.GetModBlueprint<BlueprintFeature>("EarthBreakerProficiency");
                    var BackgroundMiner = Resources.GetBlueprint<BlueprintFeature>("e4e06f443e158e646a495fce6e024546");

                    BackgroundMiner.AddComponent<AddBackgroundWeaponProficiency>(c => {
                        c.Proficiency = WeaponCategory.EarthBreaker;
                        c.StackBonusType = ModifierDescriptor.Enhancement;
                        c.StackBonus = 1;
                    });
                    var addFacts = BackgroundMiner.GetComponent<AddFacts>();
                    addFacts.m_Facts = addFacts.m_Facts.AppendToArray(EarthBreakerProficiency.ToReference<BlueprintUnitFactReference>());
                    Main.LogPatch("Patched", BackgroundMiner);
                }
                void PatchFarmhand() {
                    var KamaProficiency = Resources.GetBlueprint<BlueprintFeature>("403740e8112651141a12f0d73d793dbc");
                    var BackgroundFarmhand = Resources.GetBlueprint<BlueprintFeature>("25b35e09665310d4faac3020f8198cfb");

                    BackgroundFarmhand.AddComponent<AddBackgroundWeaponProficiency>(c => {
                        c.Proficiency = WeaponCategory.Kama;
                        c.StackBonusType = ModifierDescriptor.Enhancement;
                        c.StackBonus = 1;
                    });
                    var addFacts = BackgroundFarmhand.GetComponent<AddFacts>();
                    addFacts.m_Facts = addFacts.m_Facts.AppendToArray(KamaProficiency.ToReference<BlueprintUnitFactReference>());
                    Main.LogPatch("Patched", BackgroundFarmhand);
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
                if (ModSettings.Fixes.BaseFixes.IsDisabled("FixBackgroundModifiers")) { return instructions; }
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
                Main.Log("BACKGROUND PATCH: COULD NOT FIND TARGET");
                return -1;
            }
        }
    }
}
