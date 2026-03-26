using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class Inquisitor {
        [PatchBlueprintsCacheInit]
        static class Inquisitor_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Inquisitor")) { return; }

                var TrueJudgmentFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("f069b6557a2013544ac3636219186632");
                var InquisitorAlternateCapstone = NewContent.AlternateCapstones.Inquisitor.InquisitorAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                TrueJudgmentFeature.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.InquisitorClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == TrueJudgmentFeature.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(InquisitorAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(InquisitorAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == TrueJudgmentFeature.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(InquisitorAlternateCapstone));
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
                TTTContext.Logger.LogHeader("Patching Inquisitor");

            }
        }
    }
}
