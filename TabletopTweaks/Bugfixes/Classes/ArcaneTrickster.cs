using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Extensions;
using Kingmaker.Blueprints.Classes.Selection;

namespace TabletopTweaks.Bugfixes.Classes {
    class ArcaneTrickster {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Arcane Trickster");

                FixArcaneTricksterSpellbookSelection();

                void FixArcaneTricksterSpellbookSelection() {
                    var ArcaneTricksterSpellbookSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ae04b7cdeb88b024b9fd3882cc7d3d76");

                    ArcaneTricksterSpellbookSelection.AddFeatures(
                        Resources.GetModBlueprint<BlueprintFeature>("ArcaneTricksterSkald")
                    );
                    Main.LogPatch("Patched", ArcaneTricksterSpellbookSelection);
                }
            }
        }
    }
}
