using HarmonyLib;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.BalanceAdjustments {
    class PolymorphStacking {

        [HarmonyPatch(typeof(RuleCanApplyBuff), "OnTrigger", new[] { typeof(RulebookEventContext) })]
        static class RuleCanApplyBuff_OnTrigger_Patch {

            static void Postfix(RuleCanApplyBuff __instance) {
                if (!Settings.Fixes.DisablePolymorphStacking) { return; }
                var Descriptor = __instance.Blueprint.GetComponent<SpellDescriptorComponent>();
                if (Descriptor == null) { return; }
                if (!Descriptor.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph)) { return; }
                if (__instance.CanApply && (__instance.Context.MaybeCaster.Faction == __instance.Initiator.Faction)) {
                    __instance.Initiator
                        .Buffs
                        .Enumerable
                        .Where(buff => buff.Context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph))
                        .ForEach(buff => {
                            Main.LogDebug($"Removing Polymorph Buff: {buff.Name}");
                            buff.Remove();
                            Main.LogDebug($"Applied Polymorph Buff: {__instance.Context.Name}");
                        });
                }
            }
        }
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (!Settings.Fixes.DisablePolymorphStacking) { return; }
                FixModifers();

            }
            static void FixModifers() {
                IEnumerable<BlueprintBuff> taggedPolyBuffs = Resources.GetBlueprints<BlueprintBuff>()
                    .Where(bp => bp.GetComponents<SpellDescriptorComponent>()
                        .Where(c => (c.Descriptor & SpellDescriptor.Polymorph) == SpellDescriptor.Polymorph).Count() > 0);
                IEnumerable<BlueprintBuff> polymorphBuffs = Resources.GetBlueprints<BlueprintAbility>()
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
                    .Except(taggedPolyBuffs)
                    .Where(bp => bp.AssetGuid != "e6f2fc5d73d88064583cb828801212f4") // Fatigued
                    .Where(bp => !bp.HasFlag(BlueprintBuff.Flags.HiddenInUi))
                    .Distinct();

                polymorphBuffs
                    .OrderBy(buff => buff.name)
                    .ForEach(buff => {
                        var originalComponent = buff.GetComponent<SpellDescriptorComponent>();
                        if (originalComponent) {
                            originalComponent.Descriptor |= SpellDescriptor.Polymorph;
                        } else {
                            buff.AddComponent(Helpers.Create<SpellDescriptorComponent>(c => {
                                c.Descriptor = SpellDescriptor.Polymorph;
                            }));
                        }
                        Main.LogPatch("Patched", buff);
                    });
            }
        }
    }
}
