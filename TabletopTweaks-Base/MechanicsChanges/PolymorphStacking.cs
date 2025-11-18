using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.MechanicsChanges {
    internal class PolymorphStacking {

        private static class PolymorphMechanics {
            private static bool Initialized = false;
            [PostPatchInitialize]
            public static void Initalize() {
                if (Initialized) { return; }
                if (Main.TTTContext.Fixes.BaseFixes.IsEnabled("DisablePolymorphStacking")) {
                    EventBus.Subscribe(PolymorphStackingRules.Instance);
                }
                if (Main.TTTContext.Fixes.BaseFixes.IsEnabled("DisablePolymorphSizeStacking")) {
                    EventBus.Subscribe(PolymorphSizeRules.PolymorphBuffApply.Instance);
                }
                EventBus.Subscribe(PolymorphSizeRules.PolymorphBuffRemove.Instance);
                if (Main.TTTContext.Fixes.BaseFixes.IsEnabled("DisableSizeStacking")) {
                    EventBus.Subscribe(SizeStackingRules.SizeBuffApply.Instance);
                }
                EventBus.Subscribe(SizeStackingRules.SizeBuffRemove.Instance);
                Initialized = true;
            }
            private class PolymorphStackingRules : IAfterRulebookEventTriggerHandler<RuleCanApplyBuff>, IGlobalSubscriber {
                public static PolymorphStackingRules Instance = new();
                private PolymorphStackingRules() { }
                public void OnAfterRulebookEventTrigger(RuleCanApplyBuff evt) {
                    var Descriptor = evt.Blueprint.GetComponent<SpellDescriptorComponent>();
                    if (Descriptor == null) { return; }
                    if (!Descriptor.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph)) { return; }
                    if (evt.CanApply && (evt.Context.MaybeCaster.Faction == evt.Initiator.Faction)) {
                        evt.Initiator
                            .Buffs
                            .Enumerable
                            .Where(buff => buff.Blueprint.GetComponent<SpellDescriptorComponent>()?.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph) ?? false)
                            .ForEach(buff => buff.Remove());
                    }
                }
            }
            private class PolymorphSizeRules {
                public class PolymorphBuffApply : IUnitBuffHandler, IGlobalSubscriber, ISubscriber {
                    public static PolymorphBuffApply Instance = new();
                    private PolymorphBuffApply() { }
                    public void HandleBuffDidAdded(Buff buff) {
                        var Descriptor = buff.Blueprint.GetComponent<SpellDescriptorComponent>();
                        if (Descriptor == null) { return; }
                        if (!Descriptor.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph)) { return; }
                        var owner = buff.Owner;
                        var suppressionPart = owner?.Ensure<UnitPartBuffSupressTTT>();
                        if (suppressionPart == null) { return; }
                        suppressionPart.AddContinuousPolymorphEntry(buff);
                    }

                    public void HandleBuffDidRemoved(Buff buff) {
                    }
                }
                public class PolymorphBuffRemove : IUnitBuffHandler, IGlobalSubscriber, ISubscriber {
                    public static PolymorphBuffRemove Instance = new();
                    private PolymorphBuffRemove() { }
                    public void HandleBuffDidAdded(Buff buff) {
                    }

                    public void HandleBuffDidRemoved(Buff buff) {
                        if (!buff.Context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph)) { return; }
                        var owner = buff.Owner;
                        var suppressionPart = owner?.Get<UnitPartBuffSupressTTT>();
                        if (suppressionPart == null) { return; }
                        suppressionPart.RemoveEntry(buff);
                    }
                }
            }
            private class SizeStackingRules {
                public class SizeBuffApply : IUnitBuffHandler, IGlobalSubscriber, ISubscriber {
                    public static SizeBuffApply Instance = new();
                    private SizeBuffApply() { }
                    public void HandleBuffDidAdded(Buff buff) {
                        if (buff.Context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph) || !buff.GetComponent<ChangeUnitSize>()) { return; }
                        var owner = buff.Owner;
                        var suppressionPart = owner?.Ensure<UnitPartBuffSupressTTT>();
                        if (suppressionPart == null) { return; }
                        suppressionPart.AddSizeEntry(buff);
                    }

                    public void HandleBuffDidRemoved(Buff buff) {
                    }
                }
                public class SizeBuffRemove : IUnitBuffHandler, IGlobalSubscriber, ISubscriber {
                    public static SizeBuffRemove Instance = new();
                    private SizeBuffRemove() { }
                    public void HandleBuffDidAdded(Buff buff) {
                    }

                    public void HandleBuffDidRemoved(Buff buff) {
                        if (buff.Context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Polymorph) || !buff.GetComponent<ChangeUnitSize>()) { return; }
                        var owner = buff.Owner;
                        var suppressionPart = owner?.Get<UnitPartBuffSupressTTT>();
                        if (suppressionPart == null) { return; }
                        suppressionPart.RemoveEntry(buff);
                    }
                }
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("DisablePolymorphStacking")) { return; }
                TTTContext.Logger.LogHeader("Patching Polymorph Effects");
                AddModifers();
                RemoveModifiers();

            }
            static void AddModifers() {
                IEnumerable<BlueprintBuff> polymorphBuffs = new List<BlueprintBuff>() {
                    BlueprintTools.GetBlueprint<BlueprintBuff>("082caf8c1005f114ba6375a867f638cf"), //GeniekindDjinniBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("d47f45f29c4cfc0469f3734d02545e0b"), //GeniekindEfreetiBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("4f37fc07fe2cf7f4f8076e79a0a3bfe9"), //GeniekindMaridBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("1d498104f8e35e246b5d8180b0faed43"), //GeniekindShaitanBuff  
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
                        TTTContext.Logger.LogPatch("Patched", buff);
                    });
            }
            static void RemoveModifiers() {
                IEnumerable<BlueprintBuff> polymorphBuffs = new List<BlueprintBuff>() {
                    BlueprintTools.GetBlueprint<BlueprintBuff>("ae5d58afe8b9aa14cae977d36ff090c8"), //FormOfTheDragonISilverBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("43868c29760f7204f996dcc99ec93b39"), //FormOfTheDragonIISilverBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("b78d21189e7f6e943920236f009d30e3"), //FormOfTheDragonIIIBreathWeaponCooldownBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("5effa97644bb7394d8532c216ec0f216"), //FormOfTheDragonIIGreenBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("81e3330720af3d04eb65d9a2e7d92abb"), //FormOfTheDragonIIGoldBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("a7a5d1143490dae49b8603810866cf4d"), //FormOfTheDragonIIBreathWeaponCooldownBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("a81c1b775d3da144ea8d3c43c5b349a2"), //FormOfTheDragonIIBrassBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("ae5c9458a570b334d8dac0774f62efa1"), //FormOfTheDragonIIBlueBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("7c21715c4c4938b40ac2996a1330eeb2"), //FormOfTheDragonIIBlackBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("3980fce6bbc14604e99fcd77b326220e"), //FormOfTheDragonIGreenBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("3eebb97861a3405418396e1b9866be72"), //FormOfTheDragonIGoldBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("e8307c93669e05c4ea235a70bf5c8f98"), //FormOfTheDragonIBrassBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("93e27994169df9c43885394dc68f137f"), //FormOfTheDragonIBlueBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("5d7089b61f459204993a1292d6f158f8"), //FormOfTheDragonIBlackBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("ee6c7f5437a57ad48aaf47320129df33"), //KitsunePolymorphBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("a13e2e71485901045b1722824019d6f5"), //KitsunePolymorphBuff_Nenio  

                    BlueprintTools.GetBlueprint<BlueprintBuff>("15aa7a7b77d6410ca09730998dc13963"), //WyrmshifterSilverBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("a44fa81dbbcb462d9e648b19e720fe1a"), //WyrmshifterGreenBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("1b7290523aad4b9f8de5310fd873b000"), //WyrmshifterGoldBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("1924db960ed3476f8cf045fc1e787db2"), //WyrmshifterBrassBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("a97bed28eac746f4b756eb1473f61f5e"), //WyrmshifterBlueBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("695c5f531d084448850c0bb278ab3251"), //WyrmshifterBlackBreathWeaponBuff  

                    BlueprintTools.GetBlueprint<BlueprintBuff>("ca22ec0be6b0405d98c50d5b97085166"), //GreaterWyrmshifterBlackBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("7381efe2aa47412ea06d82c513fd3104"), //GreaterWyrmshifterBlueBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("50394b1c11aa40109ad7cad82e96b472"), //GreaterWyrmshifterBrassBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("2778a8154f1e4e3a87b5bce12ec35ac8"), //GreaterWyrmshifterBreathWeaponCooldownBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("162b2dab8b3a40bfa178bd68791da4e7"), //GreaterWyrmshifterGoldBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("92d49de79c724261b4ef264f60bd6d9c"), //GreaterWyrmshifterGreenBreathWeaponBuff 
                    BlueprintTools.GetBlueprint<BlueprintBuff>("ff0a9ac2d6044a3895f38b632dc6453d"), //GreaterWyrmshifterSilverBreathWeaponBuff  

                    BlueprintTools.GetBlueprint<BlueprintBuff>("c8c3843303d04ed79f29d3b612233663"), //FinalWyrmshifterWhiteBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("0a97e17017f048159bd7dee80b22af88"), //FinalWyrmshifterSilverBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("d4304691770f4046ae466052e7e76a1f"), //FinalWyrmshifterRedBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("70e99c84bae04cde9883e181bad0a125"), //FinalWyrmshifterGreenBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("c8379adb11f04beeb612ee11370df5f5"), //FinalWyrmshifterGoldBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("affe397f1c674600be05b9445336e514"), //FinalWyrmshifterCopperBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("571b1b4a41424f89a0c67175ac5e9596"), //FinalWyrmshifterBronzeBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("ceac742ad47d4869b59d76b5bdf5daf1"), //FinalWyrmshifterBrassBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("a22a4d5f50f940cea2013a5be888b185"), //FinalWyrmshifterBlueBreathWeaponBuff  
                    BlueprintTools.GetBlueprint<BlueprintBuff>("e2c0a5205410477d9d5e031b5bcd37bf")  //FinalWyrmshifterBlackBreathWeaponBuff 
                };
                polymorphBuffs
                    .OrderBy(buff => buff.name)
                    .ForEach(buff => {
                        buff.RemoveComponents<SpellDescriptorComponent>(c => c.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph));
                        TTTContext.Logger.LogPatch("Patched", buff);
                    });
            }
        }
    }
}
