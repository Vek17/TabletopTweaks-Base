using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Cavalier {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Cavalier.DisableAll) { return; }
                Main.LogHeader("Patching Cavalier");
                PatchBase();
            }
            
            static void PatchBase() {
                if (ModSettings.Fixes.Cavalier.Base.DisableAll) { return; }
                PatchCavalierMountSelection();
                PatchMountedModifiers();
                PatchMountedFeats();

                void PatchCavalierMountSelection() {
                    if (!ModSettings.Fixes.Cavalier.Base.Enabled["CavalierMountSelection"]) { return; }
                    var CavalierMountSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("0605927df6e2fdd42af6ee2424eb89f2");
                    var AnimalCompanionEmptyCompanion = Resources.GetBlueprint<BlueprintFeature>("472091361cf118049a2b4339c4ea836a");
                    var AnimalCompanionFeatureHorse = Resources.GetBlueprint<BlueprintFeature>("9dc58b5901677c942854019d1dd98374");
                    var CavalierMountFeatureWolf = Resources.GetBlueprint<BlueprintFeature>(ModSettings.Blueprints.NewBlueprints["CavalierMountFeatureWolf"]);


                    CavalierMountSelection.SetFeatures(AnimalCompanionEmptyCompanion, AnimalCompanionFeatureHorse, CavalierMountFeatureWolf);
                    CavalierMountSelection.m_Features = CavalierMountSelection.m_AllFeatures;
                    Main.LogPatch("Patched", CavalierMountSelection);
                }
                void PatchMountedModifiers() {
                    //if (!ModSettings.Fixes.Cavalier.Base.Enabled["CavalierMountSelection"]) { return; } //BuffExtraEffectsRequirements
                    var MountedBuff = Resources.GetBlueprint<BlueprintBuff>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
                    var ChargeBuff = Resources.GetBlueprint<BlueprintBuff>("f36da144a379d534cad8e21667079066");
                    var LongspearChargeBuff = Resources.GetBlueprint<BlueprintBuff>(ModSettings.Blueprints.NewBlueprints["LongspearChargeBuff"]);

                    var CavalierSupremeCharge = Resources.GetBlueprint<BlueprintFeature>("77af3c58e71118d4481c50694bd99e77");
                    var CavalierSupremeChargeBuff = Resources.GetBlueprint<BlueprintBuff>("7e9c5be79cfb3d44586dd650c7c7d198");
                    var GendarmeTransfixingCharge = Resources.GetBlueprint<BlueprintFeature>("72a0bde01943f824faa98bd55f04c06d");
                    var GendarmeTransfixingChargeBuff = Resources.GetBlueprint<BlueprintBuff>("6334e70d212add149909a36340ef5300");
                    var SpiritedCharge = Resources.GetBlueprint<BlueprintFeature>("95ef0ff14771f2549897f300ce62c95c");
                    var SpiritedChargeBuff = Resources.GetBlueprint<BlueprintBuff>("5a191fc6731bd4845bbbcc8ff3ff4c1d");

                    MountedBuff.AddComponent(Helpers.Create<BuffExtraEffectsRequirements>(c => {
                        c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                        c.CheckWeaponCategory = true;
                        c.WeaponCategory = WeaponCategory.Longspear;
                        c.ExtraEffectBuff = LongspearChargeBuff.ToReference<BlueprintBuffReference>();
                    }));

                    SpiritedCharge.RemoveComponents<BuffExtraEffects>();
                    SpiritedCharge.AddComponent(Helpers.Create<BuffExtraEffectsRequirements>(c => {
                        c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                        c.CheckFacts = true;
                        c.CheckedFacts = new BlueprintUnitFactReference[] { MountedBuff.ToReference<BlueprintUnitFactReference>() };
                        c.ExtraEffectBuff = SpiritedChargeBuff.ToReference<BlueprintBuffReference>();
                    }));
                    SpiritedChargeBuff.RemoveComponents<AddOutgoingDamageBonus>();
                    SpiritedChargeBuff.AddComponent(Helpers.Create<OutcomingAdditionalDamageAndHealingModifier>(c => {
                        c.Type = OutcomingAdditionalDamageAndHealingModifier.ModifyingType.OnlyDamage;
                        c.ModifierPercents = new ContextValue() { 
                            Value = 100
                        };
                    }));
                    SpiritedChargeBuff.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
                    Main.LogPatch("Patched", SpiritedCharge);
                    Main.LogPatch("Patched", SpiritedChargeBuff);

                    CavalierSupremeCharge.RemoveComponents<BuffExtraEffects>();
                    CavalierSupremeCharge.AddComponent(Helpers.Create<BuffExtraEffectsRequirements>(c => {
                        c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                        c.CheckFacts = true;
                        c.CheckedFacts = new BlueprintUnitFactReference[] { MountedBuff.ToReference<BlueprintUnitFactReference>() };
                        c.ExtraEffectBuff = CavalierSupremeChargeBuff.ToReference<BlueprintBuffReference>();
                    }));
                    CavalierSupremeChargeBuff.RemoveComponents<OutcomingDamageAndHealingModifier>();
                    CavalierSupremeChargeBuff.AddComponent(Helpers.Create<OutcomingAdditionalDamageAndHealingModifier>(c => {
                        c.Type = OutcomingAdditionalDamageAndHealingModifier.ModifyingType.OnlyDamage;
                        c.ModifierPercents = new ContextValue() {
                            Value = 100
                        };
                    }));
                    CavalierSupremeChargeBuff.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
                    Main.LogPatch("Patched", CavalierSupremeCharge);
                    Main.LogPatch("Patched", CavalierSupremeChargeBuff);

                    GendarmeTransfixingCharge.RemoveComponents<BuffExtraEffects>();
                    GendarmeTransfixingCharge.AddComponent(Helpers.Create<BuffExtraEffectsRequirements>(c => {
                        c.CheckedBuff = ChargeBuff.ToReference<BlueprintBuffReference>();
                        c.CheckFacts = true;
                        c.CheckedFacts = new BlueprintUnitFactReference[] { MountedBuff.ToReference<BlueprintUnitFactReference>() };
                        c.ExtraEffectBuff = GendarmeTransfixingChargeBuff.ToReference<BlueprintBuffReference>();
                    }));
                    GendarmeTransfixingChargeBuff.RemoveComponents<OutcomingDamageAndHealingModifier>();
                    GendarmeTransfixingChargeBuff.AddComponent(Helpers.Create<OutcomingAdditionalDamageAndHealingModifier>(c => {
                        c.Type = OutcomingAdditionalDamageAndHealingModifier.ModifyingType.OnlyDamage;
                        c.ModifierPercents = new ContextValue() {
                            Value = 200
                        };
                    }));
                    GendarmeTransfixingChargeBuff.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
                    Main.LogPatch("Patched", GendarmeTransfixingCharge);
                    Main.LogPatch("Patched", GendarmeTransfixingChargeBuff);
                }
                void PatchMountedFeats() {
                    var IndomitableMount = Resources.GetBlueprint<BlueprintFeature>("68e814f1f3ce55942a52c1dd536eaa5b");
                    var IndomitableMountCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("34762bab68ec86c45a15884b9a9929fc");
                    var MountedCombat = Resources.GetBlueprint<BlueprintFeature>("f308a03bea0d69843a8ed0af003d47a9");
                    var MountedCombatCooldownBuff = Resources.GetBlueprint<BlueprintBuff>("5c9ef8224acdbab4fbaf59c710d0ef23");

                    IndomitableMount.AddComponent(Helpers.Create<IndomitableMount>(c => {
                        c.m_CooldownBuff = IndomitableMountCooldownBuff.ToReference<BlueprintBuffReference>();
                    }));
                    MountedCombat.AddComponent(Helpers.Create<MountedCombatFixed>(c => {
                        c.m_CooldownBuff = MountedCombatCooldownBuff.ToReference<BlueprintBuffReference>();
                    }));
                    Main.LogPatch("Patched", MountedCombat);
                    Main.LogPatch("Patched", IndomitableMount);
                }
            }
            static void PatchArchetypes() {
            }
            [HarmonyPatch(typeof(MountedCombat), "OnEventDidTrigger", new Type[] { typeof(RuleAttackRoll) })]
            static class MountedCombat_OnEventDidTrigger_Patch {
                static bool Prefix(MountedCombat __instance , RuleAttackRoll evt) {
                    if (__instance.Buff.Context.MaybeCaster == null) {
                        return true;
                    }
                    if (evt.RuleAttackWithWeapon == null) {
                        return true;
                    }
                    if (evt.RuleAttackWithWeapon.Target != __instance.Owner.GetSaddledUnit()) {
                        return true;
                    }
                    if (!evt.IsHit) {
                        return true;
                    }
                    if (__instance.Owner.HasFact(__instance.CooldownBuff)) {
                        return true;
                    }
                    Main.LogDebug("Mounted Combat Triggered");
                    return true;
                }
                static void Postfix(MountedCombat __instance, RuleAttackRoll evt) {
                    Main.LogDebug("Mounted Combat Result");
                    Main.LogDebug($"IsAutomiss: {evt.AutoMiss}");
                }
            }
        }
    }
}
