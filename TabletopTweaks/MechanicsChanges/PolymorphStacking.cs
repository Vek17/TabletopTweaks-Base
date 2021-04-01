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
using System.Linq;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.BalanceAdjustments {
    class PolymorphStacking {

        [HarmonyPatch(typeof(RuleCanApplyBuff), "OnTrigger", new[] { typeof(RulebookEventContext) })]
        static class RuleCanApplyBuff_OnTrigger_Patch {
            static private BlueprintBuff[] polymorphBuffs;
            static private BlueprintBuff[] PolymorphBuffs {
                get {
                    if (polymorphBuffs == null) {
                        BlueprintBuff[] taggedPolyBuffs = Resources.GetBlueprints<BlueprintBuff>()
                            .Where(bp => bp.GetComponents<SpellDescriptorComponent>()
                                .Where(c => (c.Descriptor & SpellDescriptor.Polymorph) == SpellDescriptor.Polymorph).Count() > 0)
                            .ToArray();
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
                            .Distinct()
                            .ToArray();

                        polymorphBuffs.ForEach(c => Main.Log($"PolymorphBuff - Grabbed ID: {c.AssetGuid} - Grabbed Name: {c.name} "));
                        Main.Log($"PolymorphBuffs:{polymorphBuffs.Count()}");

                        BlueprintAbility EnlargePersonMass = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("66dc49bf154863148bd217287079245e");
                        Main.Log($"EnlargePersonMass:{EnlargePersonMass.FlattenAllActions().OfType<ContextActionApplyBuff>().Count()}");
                    }
                    return polymorphBuffs;
                }
            }

            static void Postfix(RuleCanApplyBuff __instance) {
                if (!Resources.Settings.DisablePolymorphStacking) { return; }
                if (!Array.Exists(PolymorphBuffs, bp => bp.Equals(__instance.Blueprint))) { return; }
                if (__instance.CanApply && (__instance.Context.MaybeCaster.Faction == __instance.Initiator.Faction)) {
                    BlueprintBuff[] intesection = __instance.Initiator
                        .Buffs
                        .Enumerable
                        .Select(b => b.Blueprint)
                        .Intersect(PolymorphBuffs)
                        .ToArray();
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
