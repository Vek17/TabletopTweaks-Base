using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Features {
    internal class AnimalCompanions {
        private static class LevelAdjustments {
            [PostPatchInitialize]
            static void Update_AddPet_RankToLevelAnimalCompanion() {
                if (TTTContext.Fixes.AnimalCompanions.IsDisabled("Progression")) { return; }
                AddPet.RankToLevelAnimalCompanion[0] = 0;
                AddPet.RankToLevelAnimalCompanion[1] = 2;
                AddPet.RankToLevelAnimalCompanion[2] = 3;
                AddPet.RankToLevelAnimalCompanion[3] = 3;
                AddPet.RankToLevelAnimalCompanion[4] = 4;
                AddPet.RankToLevelAnimalCompanion[5] = 5;
                AddPet.RankToLevelAnimalCompanion[6] = 6;
                AddPet.RankToLevelAnimalCompanion[7] = 6;
                AddPet.RankToLevelAnimalCompanion[8] = 7;
                AddPet.RankToLevelAnimalCompanion[9] = 8;
                AddPet.RankToLevelAnimalCompanion[10] = 9;
                AddPet.RankToLevelAnimalCompanion[11] = 9;
                AddPet.RankToLevelAnimalCompanion[12] = 10;
                AddPet.RankToLevelAnimalCompanion[13] = 11;
                AddPet.RankToLevelAnimalCompanion[14] = 12;
                AddPet.RankToLevelAnimalCompanion[15] = 12;
                AddPet.RankToLevelAnimalCompanion[16] = 13;
                AddPet.RankToLevelAnimalCompanion[17] = 14;
                AddPet.RankToLevelAnimalCompanion[18] = 15;
                AddPet.RankToLevelAnimalCompanion[19] = 15;
                AddPet.RankToLevelAnimalCompanion[20] = 16;

                Main.TTTContext.Logger.Log(String.Join(",", AddPet.RankToLevelAnimalCompanion));
            }
            [HarmonyPatch(typeof(AddPet), nameof(AddPet.TryLevelUpPet))]
            static class AddPet_TryLevelUpPet_Patch {
                static void Postfix(AddPet __instance) {
                    if (TTTContext.Fixes.AnimalCompanions.IsDisabled("Progression")) { return; }

                    if (__instance.ProgressionType != Kingmaker.Enums.PetProgressionType.AnimalCompanion) { return; }
                    if (__instance.m_UseContextValueLevel) { return; }
                    if (__instance.LevelRank == null) { return; }

                    var pet = __instance.Data.SpawnedPetRef.Value;
                    if (pet == null) { return; }
                    var rankFact = __instance.Owner.GetFact(__instance.LevelRank);
                    int currentRank = Mathf.Min(20, rankFact?.GetRank() ?? 0);

                    if (currentRank >= __instance.UpgradeLevel && __instance.UpgradeFeature != null) {
                        pet.Progression.Features.AddFeature(__instance.UpgradeFeature, null);
                    }
                }
            }
            [HarmonyPatch(typeof(BlueprintsCache), "Init")]
            static class Patch_AnimalCompanion_Classes {
                [HarmonyPriority(Priority.Last)]
                static void Postfix() {
                    UpdateProgressions();
                    UpdateAnimalCompanionModifiers();
                }

                static void UpdateProgressions() {
                    if (TTTContext.Fixes.AnimalCompanions.IsDisabled("Progression")) { return; }

                    var AnimalCompanionClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("26b10d4340839004f960f9816f6109fe");
                    var AnimalCompanionStatFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("1e570d5407a942b478e79297e0885101");

                    AnimalCompanionClass.TemporaryContext(bp => {
                        bp.Progression.LevelEntries.ForEach(UpdateLevelEntry);
                        bp.Progression.LevelEntries.Where(entry => entry.Level == 1).ForEach(entry => {
                            entry.m_Features.Remove(AnimalCompanionStatFeature);
                        });
                        TTTContext.Logger.LogPatch(bp);
                        bp.Archetypes.ForEach(a => {
                            a.AddFeatures.ForEach(UpdateLevelEntry);
                            a.RemoveFeatures.ForEach(UpdateLevelEntry);
                            TTTContext.Logger.LogPatch(a);
                        });
                    });
                    /*
                    if (Harmony.HasAnyPatches("ExpandedContent")) {
                        TTTContext.Logger.LogHeader("ExpandedContent compatability patch for Drake animal companions");
                        var DrakeCompanionClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("557496bca2644c2d93c4a88b2b546430");

                        DrakeCompanionClass?.TemporaryContext(bp => {
                            bp.Progression.LevelEntries.ForEach(UpdateLevelEntry);
                            bp.Progression.LevelEntries.Where(entry => entry.Level == 1).ForEach(entry => {
                                entry.m_Features.Remove(AnimalCompanionStatFeature);
                            });
                            TTTContext.Logger.LogPatch(bp);
                            bp.Archetypes.ForEach(a => {
                                a.AddFeatures.ForEach(UpdateLevelEntry);
                                a.RemoveFeatures.ForEach(UpdateLevelEntry);
                                TTTContext.Logger.LogPatch(a);
                            });
                        });
                    }
                    */
                    void UpdateLevelEntry(LevelEntry entry) {
                        if (entry.Level == 1) { return; }
                        entry.Level = AddPet.RankToLevelAnimalCompanion[entry.Level];
                    }
                }
                static void UpdateAnimalCompanionModifiers() {
                    if (TTTContext.Fixes.AnimalCompanions.IsDisabled("Modifiers")) { return; }

                    var AnimalCompanionSelectionBase = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("90406c575576aee40a34917a1b429254");
                    var AnimalCompanionStatFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1e570d5407a942b478e79297e0885101");
                    var AnimalCompanionFeatureHorse_PreorderBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("bfeb9be0a3c9420b8b2beecc8171029c");
                    IEnumerable<BlueprintFeature> AnimalCompanionUpgrades = AnimalCompanionSelectionBase.m_AllFeatures.Concat(AnimalCompanionSelectionBase.m_Features)
                        .Select(feature => feature.Get())
                        .AddItem(AnimalCompanionFeatureHorse_PreorderBonus)
                        .Where(feature => feature.GetComponent<AddPet>())
                        .Select(feature => feature.GetComponent<AddPet>())
                        .Where(component => component.m_UpgradeFeature != null)
                        .Select(component => component.m_UpgradeFeature.Get())
                        .Where(feature => feature != null)
                        .Distinct();
                    AnimalCompanionStatFeature.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Descriptor == ModifierDescriptor.UntypedStackable)
                        .ForEach(c => c.Descriptor = ModifierDescriptor.Racial);
                    TTTContext.Logger.LogPatch("Patched", AnimalCompanionStatFeature);
                    AnimalCompanionUpgrades.ForEach(bp => {
                        bp.SetName(TTTContext, "Animal Companion Upgrade");
                        bp.GetComponents<AddStatBonus>()
                            .Where(c => c.Descriptor == ModifierDescriptor.UntypedStackable || c.Descriptor == ModifierDescriptor.None)
                            .ForEach(c => c.Descriptor = ModifierDescriptor.Racial);
                        TTTContext.Logger.LogPatch("Patched", bp);
                    });
                }
            }
        }
    }
}
