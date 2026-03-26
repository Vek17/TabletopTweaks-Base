using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Hunter {
        [PatchBlueprintsCacheInit]
        static class Hunter_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Hunter")) { return; }

                var MasterHunter = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("d8a126a3ed3b62943a597c937a4bf840");
                var HunterAlternateCapstone = NewContent.AlternateCapstones.Hunter.HunterAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                MasterHunter.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.HunterClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == MasterHunter.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(HunterAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(HunterAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == MasterHunter.deserializedGuid));
                        //.ForEach(remove => remove.m_Features.Add(HunterAlternateCapstone));
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
                TTTContext.Logger.LogHeader("Patching Hunter");

                PatchBase();
                PatchDivineHunter();
            }

            static void PatchBase() { }

            static void PatchDivineHunter() {
                if (TTTContext.Fixes.Hunter.Archetypes["DivineHunter"].IsDisabled("OtherworldlyCompanion")) { return; }

                var OtherworldlyCompanionCelestial = BlueprintTools.GetBlueprint<BlueprintFeature>("3db2fd3394613b4438d3c844a0c034ca");
                var OtherworldlyCompanionFiendish = BlueprintTools.GetBlueprint<BlueprintFeature>("4d7607a0155af7d43b49b785f2051e21");
                var TemplateCelestial = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(TTTContext, "TemplateCelestial");
                var TemplateFiendish = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(TTTContext, "TemplateFiendish");

                OtherworldlyCompanionCelestial.RemoveComponents<AddFeatureToPet>();
                OtherworldlyCompanionCelestial.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = TemplateCelestial;
                });
                OtherworldlyCompanionFiendish.RemoveComponents<AddFeatureToPet>();
                OtherworldlyCompanionFiendish.AddComponent<AddFeatureToPet>(c => {
                    c.m_Feature = TemplateFiendish;
                });
                TTTContext.Logger.LogPatch("Patched", OtherworldlyCompanionCelestial);
                TTTContext.Logger.LogPatch("Patched", OtherworldlyCompanionFiendish);
            }
        }
    }
}
