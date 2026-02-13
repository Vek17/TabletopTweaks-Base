using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Kineticist {
        [PatchBlueprintsCacheInit]
        static class Kineticist_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Kineticist")) { return; }

                var KineticistAlternateCapstone = NewContent.AlternateCapstones.Kineticist.KineticistAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();
                var BloodKineticistArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("365b50dba54efb74fa24c07e9b7a838c");

                ClassTools.Classes.KineticistClass.TemporaryContext(bp => {
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(KineticistAlternateCapstone));
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
                BloodKineticistArchetype.RemoveFeatures = BloodKineticistArchetype.RemoveFeatures.AppendToArray(Helpers.CreateLevelEntry(20, KineticistAlternateCapstone));
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Kineticist");
                PatchBase();
            }
            static void PatchBase() {
                var ElementalOverflowProgression = BlueprintTools.GetBlueprint<BlueprintFeatureBase>("86beb0391653faf43aec60d5ec05b538");
                ElementalOverflowProgression.HideInUI = false;
            }
        }
    }
}
