using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Paladin {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Paladin.DisableAllFixes) { return; }
                Main.LogHeader("Paladin Resources");
                PatchDivineMountSelection();
                Main.LogHeader("Paladin Complete");
            }
            static void PatchDivineMountSelection() {
                if (!ModSettings.Fixes.Paladin.Base.Fixes["DivineMountSelection"]) { return; }
                var PaladinDivineMountSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("e2f0e0efc9e155e43ba431984429678e");
                var AnimalCompanionEmptyCompanion = Resources.GetBlueprint<BlueprintFeature>("472091361cf118049a2b4339c4ea836a");
                var AnimalCompanionFeatureHorse = Resources.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
                PaladinDivineMountSelection.m_AllFeatures = new BlueprintFeatureReference[] {
                    AnimalCompanionEmptyCompanion.ToReference<BlueprintFeatureReference>(),
                    AnimalCompanionFeatureHorse.ToReference<BlueprintFeatureReference>(),
                };
                PaladinDivineMountSelection.m_Features = PaladinDivineMountSelection.m_AllFeatures;
                Main.LogPatch("Patched", PaladinDivineMountSelection);
            }
            static void PatchArchetypes() {
            }
        }
    }
}
