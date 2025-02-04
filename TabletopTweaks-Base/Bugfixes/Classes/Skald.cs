using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class Skald {
        [PatchBlueprintsCacheInit]
        static class Skald_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Skald")) { return; }

                var MasterSkald = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("ae4d45a39a91dee4fb4200d7a677d9a7");
                var SkaldAlternateCapstone = NewContent.AlternateCapstones.Skald.SkaldAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                MasterSkald.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.SkaldClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == MasterSkald.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(SkaldAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(SkaldAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == MasterSkald.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(SkaldAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Skald");
                PatchBase();
                PatchBattleScion();
            }
            static void PatchBase() {
                AddSpellKenning();

                void AddSpellKenning() {
                    if (Main.TTTContext.Fixes.Skald.Base.IsDisabled("SpellKenning")) { return; }

                    var SkaldSpellKenning = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("d385b8c302e720c43aa17b8170bc6ae2");
                    var SkaldSpellKenningExtraUse = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("590d0f09d7da13d4a9382d144b8439f6");
                    var SkaldProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("26418fed2bc153245972a5b54204ed75");
                    var HeraldOfTheHornArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("72fa7b1c7a6ede44eb490080bf4a2f90");

                    SkaldProgression.LevelEntries.Where(entry => entry.Level == 5).FirstOrDefault()?.m_Features.Add(SkaldSpellKenning);
                    SkaldProgression.LevelEntries.Where(entry => entry.Level == 11).FirstOrDefault()?.m_Features.Add(SkaldSpellKenningExtraUse);
                    SkaldProgression.LevelEntries.Where(entry => entry.Level == 17).FirstOrDefault()?.m_Features.Add(SkaldSpellKenningExtraUse);

                    HeraldOfTheHornArchetype.RemoveFeatures = HeraldOfTheHornArchetype.RemoveFeatures.AppendToArray(
                        //Helpers.CreateLevelEntry(5, SkaldSpellKenning),
                        Helpers.CreateLevelEntry(11, SkaldSpellKenningExtraUse),
                        Helpers.CreateLevelEntry(17, SkaldSpellKenningExtraUse)
                    );

                    TTTContext.Logger.LogPatch(SkaldProgression);

                    SaveGameFix.AddUnitPatch((unit) => {
                        var progressionData = unit.Progression;
                        var classData = unit.Progression.GetClassData(ClassTools.Classes.SkaldClass);
                        if (classData == null) { return; }
                        var levelEntries = progressionData.SureProgressionData(classData.CharacterClass.Progression).LevelEntries;
                        foreach (LevelEntry entry in levelEntries.Where(e => e.Level <= classData.Level)) {
                            foreach (BlueprintFeatureBase feature in entry.Features) {
                                if (feature.AssetGuid == SkaldSpellKenning.deserializedGuid) {
                                    if (progressionData.Features.HasFact(SkaldSpellKenning)) { continue; }
                                    var addedFeature = progressionData.Features.AddFeature((BlueprintFeature)feature, null);
                                    var characterClass = classData.CharacterClass;
                                    addedFeature.SetSource(characterClass.Progression, entry.Level);
                                    TTTContext.Logger.Log($"{unit.CharacterName}: Applied Spell Kenning");
                                } else if (feature.AssetGuid == SkaldSpellKenningExtraUse.deserializedGuid) {
                                    if (progressionData.Features.HasFact(SkaldSpellKenningExtraUse)) {
                                        if (entry.Level == 11) { continue; }
                                        if (entry.Level == 17 && progressionData.Features.GetRank((BlueprintFeature)SkaldSpellKenningExtraUse.Get()) >= 2) { continue; }
                                    }
                                    var addedFeature = progressionData.Features.AddFeature((BlueprintFeature)feature, null);
                                    var characterClass = classData.CharacterClass;
                                    addedFeature.SetSource(characterClass.Progression, entry.Level);
                                    TTTContext.Logger.Log($"{unit.CharacterName}: Applied Spell Kenning Extra Use {addedFeature.Rank}");
                                }
                            }
                        }
                    });
                }
            }
            static void PatchBattleScion() {
                PatchDamageReduction();

                void PatchDamageReduction() {
                    if (TTTContext.Fixes.Skald.Archetypes["BattleScion"].IsDisabled("BattleProwessSelection")) { return; }
                    var SkaldRagePowerSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("2476514e31791394fa140f1a07941c96");
                    var BattleProwessSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("29b480a26a88f9e47a10d8c9fab84ee6");

                    BattleProwessSelection.AddFeatures(SkaldRagePowerSelection.AllFeatures);
                    TTTContext.Logger.LogPatch(BattleProwessSelection);
                }
            }
        }
    }
}
