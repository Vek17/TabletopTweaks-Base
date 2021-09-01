using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

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
