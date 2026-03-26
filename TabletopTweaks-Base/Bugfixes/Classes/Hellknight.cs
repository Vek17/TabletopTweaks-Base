using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Hellknight {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Hellknight Resources");

                PatchPentamicFaith();

                void PatchPentamicFaith() {
                    if (TTTContext.Fixes.Hellknight.IsDisabled("PentamicFaith")) { return; }

                    var HellKnightOrderOfTheGodclaw = BlueprintTools.GetBlueprint<BlueprintFeature>("5636564c278583342aec54eb2b409029");
                    var HellknightDisciplinePentamicFaith = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b9750875e9d7454e85347d739a1bc894");

                    HellknightDisciplinePentamicFaith.RemovePrerequisites<PrerequisiteFeature>();
                    HellknightDisciplinePentamicFaith.AddPrerequisiteFeature(HellKnightOrderOfTheGodclaw);
                    TTTContext.Logger.LogPatch("Patched", HellknightDisciplinePentamicFaith);
                }
            }
        }
    }
}
