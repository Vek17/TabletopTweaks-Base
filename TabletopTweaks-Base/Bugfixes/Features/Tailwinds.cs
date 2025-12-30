using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Features {
    internal class Tailwinds {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Tailwinds");
                PatchTailwindOfGnomishFortune();

                static void PatchTailwindOfGnomishFortune() {
                    //if (TTTContext.Fixes.BaseFixes.IsDisabled("StaggeredDescriptors")) { return; }

                    var DLC3_GnomeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("eabb0b0dade34ea3baccbf2c84962f4f");
                    var DLC3_GnomeMoneyFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9c694e328bea448b84d1160afb0e03ef");

                    var VenderDiscount = DLC3_GnomeBuff.GetComponent<AddVendorDiscount>();
                    DLC3_GnomeMoneyFeature.TemporaryContext(bp => {
                        bp.AddComponent(VenderDiscount);
                        bp.AddComponent<RecalculateOnChangeParty>(c => {
                            c.m_IsRecalculateIfDeath = true;
                        });
                    });
                    DLC3_GnomeBuff.RemoveComponents<AddVendorDiscount>();


                    TTTContext.Logger.LogPatch("Patched", DLC3_GnomeMoneyFeature);
                    TTTContext.Logger.LogPatch("Patched", DLC3_GnomeBuff);
                }
            }
        }
    }
}
