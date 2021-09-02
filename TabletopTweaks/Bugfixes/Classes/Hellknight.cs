using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Classes {
    class Hellknight {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Hellknight Resources");

                PatchPentamicFaith();

                void PatchPentamicFaith() {
                    if (ModSettings.Fixes.Hellknight.IsDisabled("PentamicFaith")) { return; }

                    var HellKnightOrderOfTheGodclaw = Resources.GetBlueprint<BlueprintFeature>("5636564c278583342aec54eb2b409029");
                    var HellknightDisciplinePentamicFaith = Resources.GetBlueprint<BlueprintFeatureSelection>("b9750875e9d7454e85347d739a1bc894");

                    HellknightDisciplinePentamicFaith.RemovePrerequisites<PrerequisiteFeature>();
                    HellknightDisciplinePentamicFaith.AddPrerequisiteFeature(HellKnightOrderOfTheGodclaw);
                    Main.LogPatch("Patched", HellknightDisciplinePentamicFaith);
                }
            }
        }
    }
}
