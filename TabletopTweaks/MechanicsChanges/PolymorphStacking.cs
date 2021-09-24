using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.MechanicsChanges {
    class PolymorphStacking {

        [HarmonyPatch(typeof(RuleCanApplyBuff), "OnTrigger", new[] { typeof(RulebookEventContext) })]
        static class RuleCanApplyBuff_OnTrigger_Patch {

            static void Postfix(RuleCanApplyBuff __instance) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("DisablePolymorphStacking")) { return; }
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
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.BaseFixes.IsDisabled("DisablePolymorphStacking")) { return; }
                Main.LogHeader("Patching Polymorph Effects");
                AddModifers();
                RemoveModifiers();

            }
            static void AddModifers() {
                IEnumerable<BlueprintBuff> polymorphBuffs = new List<BlueprintBuff>() {
                    Resources.GetBlueprint<BlueprintBuff>("082caf8c1005f114ba6375a867f638cf"), //GeniekindDjinniBuff  
                    Resources.GetBlueprint<BlueprintBuff>("d47f45f29c4cfc0469f3734d02545e0b"), //GeniekindEfreetiBuff  
                    Resources.GetBlueprint<BlueprintBuff>("4f37fc07fe2cf7f4f8076e79a0a3bfe9"), //GeniekindMaridBuff  
                    Resources.GetBlueprint<BlueprintBuff>("1d498104f8e35e246b5d8180b0faed43"), //GeniekindShaitanBuff  
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
            static void RemoveModifiers() {
                IEnumerable<BlueprintBuff> polymorphBuffs = new List<BlueprintBuff>() {
                    Resources.GetBlueprint<BlueprintBuff>("ae5d58afe8b9aa14cae977d36ff090c8"), //FormOfTheDragonISilverBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("43868c29760f7204f996dcc99ec93b39"), //FormOfTheDragonIISilverBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("b78d21189e7f6e943920236f009d30e3"), //FormOfTheDragonIIIBreathWeaponCooldownBuff  
                    Resources.GetBlueprint<BlueprintBuff>("5effa97644bb7394d8532c216ec0f216"), //FormOfTheDragonIIGreenBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("81e3330720af3d04eb65d9a2e7d92abb"), //FormOfTheDragonIIGoldBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("a7a5d1143490dae49b8603810866cf4d"), //FormOfTheDragonIIBreathWeaponCooldownBuff  
                    Resources.GetBlueprint<BlueprintBuff>("a81c1b775d3da144ea8d3c43c5b349a2"), //FormOfTheDragonIIBrassBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("ae5c9458a570b334d8dac0774f62efa1"), //FormOfTheDragonIIBlueBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("7c21715c4c4938b40ac2996a1330eeb2"), //FormOfTheDragonIIBlackBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("3980fce6bbc14604e99fcd77b326220e"), //FormOfTheDragonIGreenBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("3eebb97861a3405418396e1b9866be72"), //FormOfTheDragonIGoldBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("e8307c93669e05c4ea235a70bf5c8f98"), //FormOfTheDragonIBrassBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("93e27994169df9c43885394dc68f137f"), //FormOfTheDragonIBlueBreathWeaponBuff  
                    Resources.GetBlueprint<BlueprintBuff>("5d7089b61f459204993a1292d6f158f8")  //FormOfTheDragonIBlackBreathWeaponBuff  
                };
                polymorphBuffs
                    .OrderBy(buff => buff.name)
                    .ForEach(buff => {
                        buff.RemoveComponents<SpellDescriptorComponent>(c => c.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph));
                        Main.LogPatch("Patched", buff);
                    });
            }
        }
    }
}
