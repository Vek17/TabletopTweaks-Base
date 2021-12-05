using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Extensions;
using Kingmaker.Blueprints.Classes.Selection;

namespace TabletopTweaks.Bugfixes.Classes {
    class EldritchKnight {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Eldritch Knight");

                FixEldritchKnightSpellbookSelection();

                void FixEldritchKnightSpellbookSelection() {
                    var EldritchKnightSpellbookSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("dc3ab8d0484467a4787979d93114ebc3");

                    EldritchKnightSpellbookSelection.AddFeatures(
                        Resources.GetModBlueprint<BlueprintFeature>("EldritchKnightSkald")
                    );
                    Main.LogPatch("Patched", EldritchKnightSpellbookSelection);
                }
            }
        }
    }
}
