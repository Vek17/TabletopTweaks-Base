using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using static TabletopTweaks.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.Bugfixes.Features {
    static class MythicFeats {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Mythic Feats");
                PatchExpandedArsenal();
                PatchExtraMythicAbility();
                PatchExtraFeat();
            }
            static void PatchExpandedArsenal() {
                if (ModSettings.Fixes.MythicFeats.IsDisabled("ExpandedArsenal")) { return; }
                var ExpandedArsenalSchool = Resources.GetBlueprint<BlueprintParametrizedFeature>("f137089c48364014aa3ec3b92ccaf2e2");
                var SpellFocus = Resources.GetBlueprint<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");
                var SpellFocusGreater = Resources.GetBlueprint<BlueprintParametrizedFeature>("5b04b45b228461c43bad768eb0f7c7bf");

                SpellFocus.GetComponent<SpellFocusParametrized>().Descriptor = (ModifierDescriptor)Untyped.SpellFocus;
                SpellFocusGreater.GetComponent<SpellFocusParametrized>().Descriptor = (ModifierDescriptor)Untyped.SpellFocusGreater;

                Main.LogPatch("Patched", SpellFocus);
                Main.LogPatch("Patched", SpellFocusGreater);
                Main.LogPatch("Patched", ExpandedArsenalSchool);
            }
            static void PatchExtraMythicAbility() {
                if (ModSettings.Fixes.MythicFeats.IsDisabled("ExtraMythicAbility")) { return; }
                FeatTools.Selections.ExtraMythicAbilityMythicFeat
                    .AddPrerequisite<PrerequisiteNoFeature>(p => {
                        p.m_Feature = FeatTools.Selections.ExtraMythicAbilityMythicFeat.ToReference<BlueprintFeatureReference>();
                    }
                );
            }
            static void PatchExtraFeat() {
                if (ModSettings.Fixes.MythicFeats.IsDisabled("ExtraFeat")) { return; }
                FeatTools.Selections.ExtraFeatMythicFeat
                    .AddPrerequisite<PrerequisiteNoFeature>(p => {
                        p.m_Feature = FeatTools.Selections.ExtraFeatMythicFeat.ToReference<BlueprintFeatureReference>();
                    }
                );
            }
        }
    }
}
