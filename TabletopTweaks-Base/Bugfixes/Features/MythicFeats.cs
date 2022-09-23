using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using static TabletopTweaks.Core.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.Base.Bugfixes.Features {
    static class MythicFeats {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Mythic Feats");
                PatchExpandedArsenal();
                PatchExtraMythicAbility();
                PatchExtraFeat();
            }
            static void PatchExpandedArsenal() {
                if (TTTContext.Fixes.MythicFeats.IsDisabled("ExpandedArsenal")) { return; }
                var ExpandedArsenalSchool = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("f137089c48364014aa3ec3b92ccaf2e2");
                var SpellFocus = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");
                var SpellFocusGreater = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("5b04b45b228461c43bad768eb0f7c7bf");
                var SchoolMasteryMythicFeat = BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>("ac830015569352b458efcdfae00a948c");

                SpellFocus.GetComponent<SpellFocusParametrized>().Descriptor = (ModifierDescriptor)Untyped.SpellFocus;
                SpellFocusGreater.GetComponent<SpellFocusParametrized>().Descriptor = (ModifierDescriptor)Untyped.SpellFocusGreater;
                SchoolMasteryMythicFeat.TemporaryContext(bp => {
                    bp.RemoveComponents<SchoolMasteryParametrized>();
                    bp.AddComponent<BonusCasterLevelParametrized>(c => {
                        c.Bonus = 1;
                        c.Descriptor = (ModifierDescriptor)Untyped.SchoolMastery;
                    });
                });

                TTTContext.Logger.LogPatch(SpellFocus);
                TTTContext.Logger.LogPatch(SpellFocusGreater);
                TTTContext.Logger.LogPatch(SchoolMasteryMythicFeat);
                TTTContext.Logger.LogPatch(ExpandedArsenalSchool);
            }
            static void PatchExtraMythicAbility() {
                if (TTTContext.Fixes.MythicFeats.IsDisabled("ExtraMythicAbility")) { return; }
                FeatTools.Selections.ExtraMythicAbilityMythicFeat
                    .AddPrerequisite<PrerequisiteNoFeature>(p => {
                        p.m_Feature = FeatTools.Selections.ExtraMythicAbilityMythicFeat.ToReference<BlueprintFeatureReference>();
                    }
                );
            }
            static void PatchExtraFeat() {
                if (TTTContext.Fixes.MythicFeats.IsDisabled("ExtraFeat")) { return; }
                FeatTools.Selections.ExtraFeatMythicFeat
                    .AddPrerequisite<PrerequisiteNoFeature>(p => {
                        p.m_Feature = FeatTools.Selections.ExtraFeatMythicFeat.ToReference<BlueprintFeatureReference>();
                    }
                );
            }
        }
    }
}
