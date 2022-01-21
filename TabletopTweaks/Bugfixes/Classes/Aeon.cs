using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Aeon {

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Aeon Resources");

                PatchAeonDemythication();
                PatchAeonTenthLevelImmunities();

                void PatchAeonDemythication() {
                    if (ModSettings.Fixes.Aeon.IsDisabled("AeonDemythication")) { return; }
                    var AeonDemythicationBuff = Resources.GetBlueprint<BlueprintBuff>("3c8a543e5b4e7154bb2cbe4d102a1604");
                    QuickFixTools.ReplaceSuppression(AeonDemythicationBuff, true);
                }
                void PatchAeonTenthLevelImmunities() {
                    if (ModSettings.Fixes.Aeon.IsDisabled("AeonTenthLevelImmunities")) { return; }
                    var AeonTenthLevelImmunities = Resources.GetBlueprint<BlueprintFeature>("711f6abfab877d342af9743a11c8f3aa");
                    AeonTenthLevelImmunities.RemoveComponents<ModifyD20>(c => c.Rule == RuleType.SavingThrow);
                    AeonTenthLevelImmunities.AddComponent<ModifySavingThrowD20>(c => {
                        c.AgainstAlignment = true;
                        c.Alignment = AlignmentComponent.Chaotic;
                        c.Replace = true;
                        c.Roll = 20;
                    });
                }
            }
        }
    }
}
