using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Clases {
    class Rogue {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Rogue.DisableAllFixes) { return; }
                Main.LogHeader("Patching Rogue");
                PatchArchetypes();
            }
            static void PatchArchetypes() {
                PatchEldritchScoundrel();
            }
            static void PatchEldritchScoundrel() {
                if (ModSettings.Fixes.Rogue.Archetypes["EldritchScoundrel"].DisableAllFixes) { return; }
                if (!ModSettings.Fixes.Rogue.Archetypes["EldritchScoundrel"].Fixes["SneakAttack"]) { return; }
                var EldritchScoundrelArchetype = Resources.GetBlueprint<BlueprintArchetype>("57f93dd8423c97c49989501281296c4a");
                var SneakAttack = Resources.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
                EldritchScoundrelArchetype.RemoveFeatures = EldritchScoundrelArchetype.RemoveFeatures.AddToArray(Helpers.LevelEntry(1, SneakAttack));
                Main.LogPatch("Patched", EldritchScoundrelArchetype);
            }
        }
    }
}
