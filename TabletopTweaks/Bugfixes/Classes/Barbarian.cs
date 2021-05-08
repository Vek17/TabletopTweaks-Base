using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Barbarian {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Monk.DisableAllFixes) { return; }
                Main.LogHeader("Patching Barbarian");
                PatchBase();
                PatchScaledFist();
            }
            static void PatchBase() {
                if (ModSettings.Fixes.Barbarian.Base.DisableAllFixes) { return; }
            }
            static void PatchScaledFist() {
                if (ModSettings.Fixes.Barbarian.Archetypes["InstinctualWarriorArchetype"].DisableAllFixes) { return; }
                PatchCunningElusionFeature();

                void PatchCunningElusionFeature() {
                    if (!ModSettings.Fixes.Barbarian.Archetypes["InstinctualWarriorArchetype"].Fixes["CunningElusion"]) { return; }
                    var InstinctualWarriorArchetype = Resources.GetBlueprint<BlueprintArchetype>("adffdd8a99094a89823a79292a503ee9");
                    var InstinctualWarriorACBonusUnlock = Resources.GetBlueprint<BlueprintFeature>(ModSettings.Blueprints.NewBlueprints["InstinctualWarriorACBonusUnlock"]);
                    var CunningElusionFeature = Resources.GetBlueprint<BlueprintFeature>("a71103ce28964f39b38442baa32a3031");

                    InstinctualWarriorArchetype.AddFeatures.Where(entry => entry.Level == 2).First().m_Features = new List<BlueprintFeatureBaseReference>() {
                        InstinctualWarriorACBonusUnlock.ToReference<BlueprintFeatureBaseReference>()
                    };
                    Main.LogPatch("Patched", InstinctualWarriorArchetype);
                    CunningElusionFeature.HideInUI = true;
                    Main.LogPatch("Patched", CunningElusionFeature);
                }
            }
        }
    }
}