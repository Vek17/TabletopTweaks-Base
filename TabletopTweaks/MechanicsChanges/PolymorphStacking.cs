using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.BalanceAdjustments {
    class PolymorphStacking {

        [HarmonyPatch(typeof(RuleCanApplyBuff), "OnTrigger", new[] { typeof(RulebookEventContext) })]
        static class RuleCanApplyBuff_OnTrigger_Patch {
            static private IEnumerable<BlueprintBuff> polymorphBuffs;
            static private IEnumerable<BlueprintBuff> PolymorphBuffs {
                get {
                    if (polymorphBuffs == null) {
                        Main.LogHeader($"Identifying Polymorph Buffs");
                        IEnumerable <BlueprintBuff> taggedPolyBuffs = Resources.GetBlueprints<BlueprintBuff>()
                            .Where(bp => bp.GetComponents<SpellDescriptorComponent>()
                                .Where(c => (c.Descriptor & SpellDescriptor.Polymorph) == SpellDescriptor.Polymorph).Count() > 0);
                        polymorphBuffs = Resources.GetBlueprints<BlueprintAbility>()
                            .Where(bp =>
                                (bp.GetComponents<SpellDescriptorComponent>()
                                    .Where(c => c != null)
                                    .Where(d => d.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph)).Count() > 0)
                                || (bp.GetComponents<AbilityExecuteActionOnCast>()
                                    .SelectMany(c => c.Actions.Actions.OfType<ContextActionRemoveBuffsByDescriptor>())
                                    .Where(c => c.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph)).Count() > 0)
                                || (bp.GetComponents<AbilityEffectRunAction>()
                                    .SelectMany(c => c.Actions.Actions.OfType<ContextActionRemoveBuffsByDescriptor>()
                                        .Concat(c.Actions.Actions.OfType<ContextActionConditionalSaved>()
                                            .SelectMany(a => a.Failed.Actions
                                            .OfType<ContextActionRemoveBuffsByDescriptor>())))
                                    .Where(c => c.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph)).Count() > 0))
                            .SelectMany(a => a.FlattenAllActions())
                            .OfType<ContextActionApplyBuff>()
                            .Where(c => c.Buff != null)
                            .Select(c => c.Buff)
                            .Concat(taggedPolyBuffs)
                            .Where(bp => bp.AssetGuid != "e6f2fc5d73d88064583cb828801212f4") // Fatigued
                            .Distinct();

                        polymorphBuffs
                            .OrderBy(c => c.name)
                            .ForEach(c => Main.LogPatch("PolymorphBuff Found", c));
                        Main.LogHeader($"Identified: {polymorphBuffs.Count()} Polymorph Buffs");
                    }
                    return polymorphBuffs;
                }
            }

            static void Postfix(RuleCanApplyBuff __instance) {
                if (!Resources.Fixes.DisablePolymorphStacking) { return; }
                if (!PolymorphBuffs.Contains(__instance.Blueprint)) { return; }
                if (__instance.CanApply && (__instance.Context.MaybeCaster.Faction == __instance.Initiator.Faction)) {
                    IEnumerable<BlueprintBuff> intesection = __instance.Initiator
                        .Buffs
                        .Enumerable
                        .Select(b => b.Blueprint)
                        .Intersect(PolymorphBuffs);
                    if (intesection.Any()) {
                        foreach (BlueprintBuff buffToRemove in intesection) {
                            __instance.Initiator
                                .Buffs
                                .GetBuff(buffToRemove)
                                .Remove();
                            Main.Log($"Removed Polymorph Buff: {buffToRemove.Name}");
                        }
                        Main.Log($"Applied Polymorph Buff: {__instance.Context.Name}");
                    }
                }
            }
        }
    }
}
