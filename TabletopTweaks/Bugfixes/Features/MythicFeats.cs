using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Features {
    static class MythicFeats {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Mythic Feats");
                PatchExtraMythicAbility();
                PatchExtraFeat();
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
        [HarmonyPatch(typeof(SpellFocusParametrized), "OnEventAboutToTrigger", new[] { typeof(RuleCalculateAbilityParams) })]
        static class SpellFocusParametrized_OnEventAboutToTrigger_ExpandedArsenal {
            static bool Prefix(SpellFocusParametrized __instance, RuleCalculateAbilityParams evt) {
                if (ModSettings.Fixes.MythicFeats.IsDisabled("ExpandedArsenal")) { return true; }

                if (!__instance.SpellsOnly || evt.Spellbook != null) {
                    var school = evt.Spell?.GetComponent<SpellComponent>()?.School ?? SpellSchool.None;
                    bool applyBonus = false;
                    if (!applyBonus) {
                        applyBonus = school == __instance.Param;
                    }
                    if (!applyBonus) {
                        // here we set every spell focus to work, regardless of school, when Expanded Arsenal is selected for this spell's school
                        // (unpatched code ignored Expanded Arsenal if unit had taken Spell Focus in this spell's school)
                        applyBonus = __instance.Owner.Get<UnitPartExpandedArsenal>()?.HasSpellSchoolEntry(school) ?? false;
                    }
                    if (applyBonus) {
                        var hasMythicFocus = evt.Initiator.Progression.Features.Enumerable.Any((Feature p) => p.Param == __instance.Param && p.Blueprint == __instance.MythicFocus);
                        var bonus = __instance.BonusDC * (hasMythicFocus ? 2 : 1);
                        evt.AddBonusDC(bonus, __instance.Descriptor);
                    }
                }

                return false;
            }
        }
    }
}
