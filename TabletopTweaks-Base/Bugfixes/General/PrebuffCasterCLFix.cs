﻿using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.General {
    static class PrebuffCasterCLFix {
        [HarmonyPatch(typeof(AddFacts), nameof(AddFacts.UpdateFacts))]
        static class AddFacts_UpdateFacts_CL_Patch {
            static void Postfix(AddFacts __instance) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixPrebuffCasterLevels")) { return; }
                if (__instance.CasterLevel <= 0) {
                    var OwnerCR = __instance.Owner?.Blueprint?.GetComponent<Experience>()?.CR ?? 0;
                    if (OwnerCR > 0) {
                        __instance.Data?.AppliedFacts?.ForEach(fact => {
                            if (fact?.MaybeContext?.m_Params != null) {
                                fact.MaybeContext.m_Params.CasterLevel = OwnerCR;
                            }
                        });
                    }
                }
                __instance?.Data?.AppliedFacts?.ForEach(fact => {
                    if (fact?.MaybeContext?.m_Params != null) {
                        fact.MaybeContext.m_Params = fact?.MaybeContext?.Params?.Clone();
                    }
                });
            }
        }

        [HarmonyPatch(typeof(AddFactsToMount), nameof(AddFacts.UpdateFacts))]
        static class AddFactsToMount_UpdateFacts_CL_Patch {
            static void Postfix(AddFactsToMount __instance) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixPrebuffCasterLevels")) { return; }
                if (__instance.CasterLevel <= 0) { return; }
                __instance?.Data?.AppliedFactRefs?.ForEach(id => {
                    var fact = __instance.Data?.Mount?.Facts?.FindById(id);
                    var m_Params = fact?.MaybeContext?.m_Params;
                    if (fact?.MaybeContext != null) {
                        fact.MaybeContext.m_Params = fact.MaybeContext.Params?.Clone();
                    }
                });
            }
        }

        [HarmonyPatch(typeof(Kingmaker.UnitLogic.Buffs.BuffCollection), "AddBuff",
            new Type[] {
                typeof(BlueprintBuff),
                typeof(UnitEntityData),
                typeof(TimeSpan?),
                typeof(AbilityParams)
            }
        )]
        static class BuffCollection_AddBuff_CL_Patch {
            static bool Prefix(
                Kingmaker.UnitLogic.Buffs.BuffCollection __instance,
                BlueprintBuff blueprint,
                UnitEntityData caster,
                TimeSpan? duration,
                AbilityParams abilityParams,
                ref Buff __result) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixPrebuffCasterLevels")) { return true; }

                MechanicsContext mechanicsContext = new MechanicsContext(caster, __instance.Owner, blueprint, null, null);
                if (abilityParams != null) {
                    mechanicsContext.SetParams(abilityParams);
                }

                var actualCaster = caster?.Descriptor ?? __instance?.Owner;
                if (actualCaster == null) { return true; }
                if (mechanicsContext == null) { return true; }
                if (abilityParams == null) {
                    var OwnerCR = actualCaster.Progression.CharacterLevel;// actualCaster.Blueprint?.GetComponent<Experience>()?.CR; ?? actualCaster.Progression.CharacterLevel;
                    if (OwnerCR == 0) { return true; }
                    var clonedParams = mechanicsContext.Params.Clone();
                    clonedParams.CasterLevel = OwnerCR;
                    mechanicsContext.m_Params = clonedParams;
                    //__result.Reapply();
                }
                __result = __instance.Manager.Add<Buff>(new Buff(blueprint, mechanicsContext, duration));
                return false;
            }
        }

        //[HarmonyPatch(typeof(RuleCalculateAbilityParams), nameof(RuleCalculateAbilityParams.OnTrigger))]
        static class RuleCalculateAbilityParams_CL_Patch {
            static void Postfix(RuleCalculateAbilityParams __instance) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixPrebuffCasterLevels")) { return; }
                TTTContext.Logger.Log($"CL: {__instance.Result?.CasterLevel} Spell: {__instance.Spell?.name} ");
            }
        }
    }
}
