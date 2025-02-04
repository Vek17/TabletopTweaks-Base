using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class EldritchKnight {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Eldritch Knight Resources");
                PatchDiverseTrainingArcane();
                PatchDiverseTrainingFighter();
            }
            static void PatchDiverseTrainingArcane() {
                if (TTTContext.Fixes.EldritchKnight.IsDisabled("PatchDiverseTrainingArcane")) { return; }

                var EldritchKnightDiverseTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("983e8ad193160b44da80b38af4927e75");

                AddFakeClassLevels(EldritchKnightDiverseTraining, ClassTools.ClassReferences.ArcanistClass);
                AddFakeClassLevels(EldritchKnightDiverseTraining, ClassTools.ClassReferences.BardClass);
                AddFakeClassLevels(EldritchKnightDiverseTraining, ClassTools.ClassReferences.BloodragerClass);
                AddFakeClassLevels(EldritchKnightDiverseTraining, ClassTools.ClassReferences.MagusClass);
                AddFakeClassLevels(EldritchKnightDiverseTraining, ClassTools.ClassReferences.SkaldClass);
                AddFakeClassLevels(EldritchKnightDiverseTraining, ClassTools.ClassReferences.SorcererClass);
                AddFakeClassLevels(EldritchKnightDiverseTraining, ClassTools.ClassReferences.WitchClass);
                AddFakeClassLevels(EldritchKnightDiverseTraining, ClassTools.ClassReferences.WizardClass);

                void AddFakeClassLevels(BlueprintFeature feature, BlueprintCharacterClassReference fakeClass) {
                    feature.AddComponent<ClassLevelsForPrerequisitesTTT>(c => {
                        c.m_ActualClass = ClassTools.ClassReferences.EldritchKnightClass;
                        c.m_FakeClass = fakeClass;
                        c.Modifier = 1;
                        c.Summand = 0;
                        c.CheckedGroups = new FeatureGroup[] { FeatureGroup.Feat };
                    });
                }

                TTTContext.Logger.LogPatch(EldritchKnightDiverseTraining);
            }
            static void PatchDiverseTrainingFighter() {
                if (TTTContext.Fixes.EldritchKnight.IsDisabled("DiverseTrainingFighter")) { return; }

                var EldritchKnightDiverseTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("983e8ad193160b44da80b38af4927e75");
                QuickFixTools.ReplaceClassLevelsForPrerequisites(EldritchKnightDiverseTraining, TTTContext, FeatureGroup.Feat);

                TTTContext.Logger.LogPatch(EldritchKnightDiverseTraining);
            }
        }
    }
}
