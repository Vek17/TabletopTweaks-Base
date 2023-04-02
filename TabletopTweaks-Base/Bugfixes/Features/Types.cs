using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;


namespace TabletopTweaks.Base.Bugfixes.Features {
    internal class Types {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Types/Subtypes");
                PatchAlignmentTypes();

                static void PatchAlignmentTypes() {
                    if (TTTContext.Fixes.BaseFixes.IsDisabled("FixAlignmentSubtypes")) { return; }

                    var SubtypeGood = BlueprintTools.GetBlueprint<BlueprintFeature>("23247ff3b44fd3a42ab752cd04e629b0");
                    var SubtypeEvil = BlueprintTools.GetBlueprint<BlueprintFeature>("5279fc8380dd9ba419b4471018ffadd1");
                    var SubtypeLawful = BlueprintTools.GetBlueprint<BlueprintFeature>("56af493a739e14f44aa56a6cba0b477b");
                    var SubtypeChaotic = BlueprintTools.GetBlueprint<BlueprintFeature>("1dd712e7f147ab84bad6ffccd21a878d");

                    FixDamageAlignment(SubtypeGood);
                    FixDamageAlignment(SubtypeEvil);
                    FixDamageAlignment(SubtypeLawful);
                    FixDamageAlignment(SubtypeChaotic);

                    void FixDamageAlignment(BlueprintFeature subtype) {
                        subtype.TemporaryContext(bp => {
                            bp.GetComponent<AddOutgoingPhysicalDamageProperty>().TemporaryContext(c => {
                                c.AffectAnyPhysicalDamage = true;
                            });
                        });
                        TTTContext.Logger.LogPatch(subtype);
                    }
                }
            }
        }
    }
}
