using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class Shifter {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class Paladin_AlternateCapstone_Patch {
            static bool Initialized;
            [HarmonyPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }

            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Shifter")) { return; }

                var FinalShifterAspectFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("5a155f5c3f834a319feab52dc66ee185");
                var ShifterAspectSelectionFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("121829d239124685b430f5263031bf83");
                var ShifterAlternateCapstone = NewContent.AlternateCapstones.Shifter.ShifterAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                FinalShifterAspectFeature.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.ShifterClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == FinalShifterAspectFeature.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(ShifterAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => {
                            entry.m_Features.RemoveAll(f => f.deserializedGuid == ShifterAspectSelectionFeature.deserializedGuid);
                            entry.m_Features.Add(ShifterAlternateCapstone);
                        });
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == FinalShifterAspectFeature.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(ShifterAlternateCapstone));
                    });
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == ShifterAspectSelectionFeature.deserializedGuid))
                            .ForEach(remove => remove.m_Features.RemoveAll(f => f.deserializedGuid == ShifterAspectSelectionFeature.deserializedGuid));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Shifter");
                PatchBase();
                PatchArchetypes();
            }
            static void PatchBase() { 
            }

            static void PatchArchetypes() {
                PatchGriffonheartShifter();

                void PatchGriffonheartShifter() {
                    PatchPlayerAvailablilty();

                    void PatchPlayerAvailablilty() {
                        if (TTTContext.Fixes.Shifter.Archetypes["GriffonheartShifter"].IsDisabled("PlayerUsable")) { return; }

                        var GriffonheartShifterArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("aed5b306ad734a6da5d5638edcb667c9");
                        GriffonheartShifterArchetype.m_HiddenInUI = false;

                        TTTContext.Logger.LogPatch(GriffonheartShifterArchetype);
                    }
                }
            }
        }
    }
}
