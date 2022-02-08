using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Units {
    static class Bosses {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Bosses");
                PatchStauntonVane();
            }
        }
        static void PatchStauntonVane() {
            if (ModSettings.Fixes.Units.Bosses.IsDisabled("StauntonVane")) { return; }

            var StauntonVane_Boss = Resources.GetBlueprint<BlueprintUnit>("88f8535a8db0154488d5e72d74e0e466");
            var WarpriestClass = Resources.GetBlueprintReference<BlueprintCharacterClassReference>("30b5e47d47a0e37438cc5a80c96cfb99");
            var WarpriestFeatSelection = Resources.GetBlueprintReference<BlueprintFeatureSelectionReference>("303fd456ddb14437946e344bad9a893b");
            var FighterFeatSelection = Resources.GetBlueprintReference<BlueprintFeatureSelectionReference>("41c8486641f7d6d4283ca9dae4147a9f");

            StauntonVane_Boss
                .GetComponent<AddClassLevels>(c => c.m_CharacterClass.Equals(WarpriestClass))?
                .GetSelection(c => c.m_Selection.Equals(FighterFeatSelection))?
                .TemporaryContext(c => {
                    c.m_Selection = WarpriestFeatSelection;
                });
            Main.LogPatch(StauntonVane_Boss);
        }
    }
}
