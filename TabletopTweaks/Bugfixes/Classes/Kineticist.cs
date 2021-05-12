using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Kineticist {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Kineticist.DisableAll) { return; }
                Main.LogHeader("Patching Kineticist");
                PatchBase();
                PatchElementalEngine();
            }
            static void PatchBase() {
                if (ModSettings.Fixes.Kineticist.Base.DisableAll) { return; }
                var ElementalOverflowProgression = Resources.GetBlueprint<BlueprintFeatureBase>("86beb0391653faf43aec60d5ec05b538");
                ElementalOverflowProgression.HideInUI = false;
            }
            static void PatchElementalEngine() {
                if (ModSettings.Fixes.Kineticist.Archetypes["ElementalEngine"].DisableAll) { return; }
                if (!ModSettings.Fixes.Kineticist.Archetypes["ElementalEngine"].Enabled["FixBrokenSelection"]) { return; }
                var ElementalEngine = Resources.GetBlueprint<BlueprintArchetype>("4a8324d676d642c99edcdda6988ca3b1");
                var Supercharge = Resources.GetBlueprint<BlueprintFeatureBase>("5a13756fb4be25f46951bc3f16448276");
                var ElementalOverflowProgression = Resources.GetBlueprint<BlueprintFeatureBase>("86beb0391653faf43aec60d5ec05b538");
                ElementalEngine.RemoveFeatures = new LevelEntry[] {
                    new LevelEntry(){ Level = 1, Features = { ElementalOverflowProgression }},
                    new LevelEntry(){ Level = 11, Features = { Supercharge }}
                };
                Main.LogPatch("Patched", ElementalEngine);
            }
        }
    }
}
