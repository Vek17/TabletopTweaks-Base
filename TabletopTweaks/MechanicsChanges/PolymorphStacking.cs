using HarmonyLib;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
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
                IEnumerable<BlueprintBuff> polymorphBuffs = new List<BlueprintBuff>() {
                    ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("082caf8c1005f114ba6375a867f638cf"), //GeniekindDjinniBuff  
                    ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("d47f45f29c4cfc0469f3734d02545e0b"), //GeniekindEfreetiBuff  
                    ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("4f37fc07fe2cf7f4f8076e79a0a3bfe9"), //GeniekindMaridBuff  
                    ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("1d498104f8e35e246b5d8180b0faed43"), //GeniekindShaitanBuff  
                };
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
