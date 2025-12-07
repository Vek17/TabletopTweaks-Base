using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;


namespace TabletopTweaks.Base.Bugfixes.Features {
    internal class Types {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Types/Subtypes");
                PatchAlignmentTypes();
                PatchUndeadType();

                static void PatchAlignmentTypes() {
                    if (TTTContext.Fixes.Types.IsDisabled("AlignmentSubtypes")) { return; }

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
                static void PatchUndeadType() {
                    if (TTTContext.Fixes.Types.IsDisabled("UndeadType")) { return; }

                    var UndeadImmunities = BlueprintTools.GetBlueprint<BlueprintFeature>("8a75eb16bfff86949a4ddcb3dd2f83ae");

                    UndeadImmunities.TemporaryContext(bp => {
                        bp.RemoveComponents<AddConditionImmunity>(c => c.Condition == UnitCondition.Sickened);
                        bp.RemoveComponents<AddConditionImmunity>(c => c.Condition == UnitCondition.Nauseated);
                        bp.GetComponents<BuffDescriptorImmunity>().ForEach(c => {
                            c.Descriptor &= ~(SpellDescriptor.Sickened | SpellDescriptor.Nauseated);
                        });
                        bp.GetComponents<SpellImmunityToSpellDescriptor>().ForEach(c => {
                            c.Descriptor &= ~(SpellDescriptor.Sickened | SpellDescriptor.Nauseated);
                        });
                    });

                    TTTContext.Logger.LogPatch(UndeadImmunities);
                }
            }
        }
    }
}
