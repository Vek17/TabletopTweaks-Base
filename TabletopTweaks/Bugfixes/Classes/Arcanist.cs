using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Arcanist {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Arcanist.DisableAll) { return; }
                Main.LogHeader("Patching Arcanist Resources");
                PatchConsumeSpells();
            }
            static void PatchConsumeSpells() {
                if (!ModSettings.Fixes.Arcanist.Base.Enabled["ConsumeSpells"]) { return; }
                var ArcanistConsumeSpellsResource = Resources.GetBlueprint<BlueprintAbilityResource>("d67ddd98ad019854d926f3d6a4e681c5");
                ArcanistConsumeSpellsResource.m_Min = 1;
                Main.LogPatch("Patched", ArcanistConsumeSpellsResource);
            }
        }
    }
}
