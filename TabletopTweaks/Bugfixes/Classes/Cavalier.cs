using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Cavalier {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Cavalier.DisableAllFixes) { return; }
                Main.LogHeader("Cavalier Resources");
                CavalierMountSelection();
                Main.LogHeader("Cavalier Complete");
            }
            static void CavalierMountSelection() {
                if (!ModSettings.Fixes.Cavalier.Base.Fixes["CavalierMountSelection"]) { return; }
                var CavalierMountSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("0605927df6e2fdd42af6ee2424eb89f2");
                var AnimalCompanionEmptyCompanion = Resources.GetBlueprint<BlueprintFeature>("472091361cf118049a2b4339c4ea836a");
                var AnimalCompanionFeatureHorse = Resources.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
                var CavalierMountFeatureWolf = Resources.GetBlueprint<BlueprintFeature>(ModSettings.Blueprints.NewBlueprints["CavalierMountFeatureWolf"]);

                CavalierMountSelection.m_AllFeatures = new BlueprintFeatureReference[] {
                    AnimalCompanionEmptyCompanion.ToReference<BlueprintFeatureReference>(),
                    AnimalCompanionFeatureHorse.ToReference<BlueprintFeatureReference>(),
                    CavalierMountFeatureWolf.ToReference<BlueprintFeatureReference>(),
                };
                CavalierMountSelection.m_Features = CavalierMountSelection.m_AllFeatures;
                Main.LogPatch("Patched", CavalierMountSelection);
            }
            static void PatchArchetypes() {
            }
        }
    }
}
