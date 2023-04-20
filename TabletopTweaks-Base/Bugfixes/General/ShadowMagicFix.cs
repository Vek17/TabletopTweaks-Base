using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Linq;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.General {
    class ShadowMagicFix {
        [HarmonyPatch(typeof(AutoMetamagic), nameof(AutoMetamagic.ShouldApplyTo), new Type[] {
            typeof(AutoMetamagic),
            typeof(BlueprintAbility),
            typeof(AbilityData)
        })]
        static class AutoMetamagic_ShouldApplyTo_Shadow_Patch {
            static void Postfix(AutoMetamagic c, BlueprintAbility ability, AbilityData data, ref bool __result) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixShadowSpells")) { return; }
                if (data?.ConvertedFrom?.Blueprint?.GetComponent<AbilityShadowSpell>() != null) {
                    __result |= AutoMetamagic.ShouldApplyTo(c, data.ConvertedFrom.Blueprint, data.ConvertedFrom);
                }
            }
        }
        [HarmonyPatch(typeof(IncreaseSpellDescriptorDC), "OnEventAboutToTrigger", new Type[] { typeof(RuleCalculateAbilityParams) })]
        static class IncreaseSpellDescriptorDC_OnEventAboutToTrigger_Shadow_Patch {
            static bool Prefix(IncreaseSpellDescriptorDC __instance, RuleCalculateAbilityParams evt) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixShadowSpells")) { return true; }
                SpellDescriptorComponent component = evt.Spell.GetComponent<SpellDescriptorComponent>();

                var ParentAbility = evt.AbilityData?.ConvertedFrom;
                if (ParentAbility?.Blueprint?.GetComponent<AbilityShadowSpell>() != null) {
                    component = ParentAbility.Blueprint.GetComponent<SpellDescriptorComponent>();
                }
                if (component == null) {
                    return false;
                }
                SpellDescriptor spellDescriptor = component.Descriptor.Value;
                spellDescriptor = UnitPartChangeSpellElementalDamage.ReplaceSpellDescriptorIfCan(__instance.Owner, spellDescriptor);
                if (spellDescriptor.HasAnyFlag(__instance.Descriptor)) {
                    evt.AddBonusDC(__instance.BonusDC, __instance.ModifierDescriptor);
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(IncreaseSpellSchoolDC), "OnEventAboutToTrigger", new Type[] { typeof(RuleCalculateAbilityParams) })]
        static class IncreaseSpellSchoolDC_OnEventAboutToTrigger_Shadow_Patch {

            static bool Prefix(IncreaseSpellSchoolDC __instance, RuleCalculateAbilityParams evt) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixShadowSpells")) { return true; }
                var spell = evt.Spell;
                var ParentAbility = evt.AbilityData?.ConvertedFrom;
                if (ParentAbility?.Blueprint?.GetComponent<AbilityShadowSpell>() != null) {
                    spell = ParentAbility.Blueprint;
                }

                bool isSchool = __instance.School == SpellSchool.None;
                if (!isSchool) {
                    foreach (SpellComponent spellComponent in spell.GetComponents<SpellComponent>()) {
                        isSchool = (spellComponent.School == __instance.School);
                    }
                }
                if (isSchool) {
                    evt.AddBonusDC(__instance.BonusDC);
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(SpellFocusParametrized), "OnEventAboutToTrigger", new Type[] { typeof(RuleCalculateAbilityParams) })]
        static class SpellFocusParametrized_OnEventAboutToTrigger_Shadow_Patch {

            static bool Prefix(SpellFocusParametrized __instance, RuleCalculateAbilityParams evt) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixShadowSpells")) { return true; }
                var spell = evt.Spell;
                var ParentAbility = evt.AbilityData?.ConvertedFrom;
                if (ParentAbility?.Blueprint?.GetComponent<AbilityShadowSpell>() != null) {
                    spell = ParentAbility.Blueprint;
                }
                SpellSchool school = spell?.GetComponent<SpellComponent>()?.School ?? SpellSchool.None;
                //SpellSchool ? nullable = spell != null ? spell.GetComponent<SpellComponent>()?.School : new SpellSchool?();
                //SpellSchool school = nullable.HasValue ? nullable.GetValueOrDefault() : SpellSchool.None;
                int num1 = school == __instance.Param ? 1 : 0;
                int num2;
                if (!__instance.Owner.Progression.Features.Enumerable.Any(p => p.Blueprint == __instance.Fact.Blueprint && p.Param == school)) {
                    UnitPartExpandedArsenal partExpandedArsenal = __instance.Owner.Get<UnitPartExpandedArsenal>();
                    num2 = partExpandedArsenal != null ? (partExpandedArsenal.HasSpellSchoolEntry(school) ? 1 : 0) : 0;
                } else
                    num2 = 0;
                bool flag = num2 != 0;
                int num3 = evt.Initiator.Progression.Features.Enumerable.Any(p => p.Param == __instance.Param && p.Blueprint == __instance.MythicFocus) ? 2 : 1;
                int num4 = flag ? 1 : 0;
                if ((num1 | num4) == 0) {
                    return false;
                }
                evt.AddBonusDC(__instance.BonusDC * num3, __instance.Descriptor);
                return false;
            }
        }
    }
}
