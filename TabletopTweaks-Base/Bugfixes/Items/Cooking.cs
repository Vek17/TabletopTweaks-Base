using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Items {
    internal class Cooking {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Cooking");
                PatchScreamingOmelet();
            }
            static void PatchScreamingOmelet() {
                if (TTTContext.Fixes.Items.Cooking.IsDisabled("ScreamingOmelet")) { return; }

                var ScreamingOmeletBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("0c283d43dde431f43aa13ace4488fba4");

                ScreamingOmeletBuff.TemporaryContext(bp => {
                    bp.GetComponent<AddContextStatBonus>()?.TemporaryContext(c => {
                        c.Multiplier = 2;
                    });
                });

                TTTContext.Logger.LogPatch(ScreamingOmeletBuff);
            }
        }
    }
}
