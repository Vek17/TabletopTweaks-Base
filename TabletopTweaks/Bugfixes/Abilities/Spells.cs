using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Abilities {
    class Spells {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;
            static bool Prefix() {
                if (Initialized) {
                    // When wrath first loads into the main menu InitializeLibrary is called by Kingmaker.GameStarter.
                    // When loading into maps, Kingmaker.Runner.Start will call InitializeLibrary which will
                    // clear the ResourcesLibrary.s_LoadedBlueprints cache which causes loaded blueprints to be garbage collected.
                    // Return false here to prevent ResourcesLibrary.InitializeLibrary from being called twice 
                    // to prevent blueprints from being garbage collected.
                    return false;
                }
                else {
                    return true;
                }
            }
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Spell Resources");
                patchWrachingRay();
                patchGreaterBestowCurse();
                patchProtectionFromAlignment();
                Main.LogHeader("Patching Spells Complete");
            }
            static void patchWrachingRay() {
                var WrackingRay = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("1cde0691195feae45bab5b83ea3f221e");
                foreach (AbilityEffectRunAction component in WrackingRay.GetComponents<AbilityEffectRunAction>()) {
                    foreach (ContextActionDealDamage action in component.Actions.Actions.OfType<ContextActionDealDamage>()) {
                        action.Value.DiceType = Kingmaker.RuleSystem.DiceType.D4;
                    }
                }
                Main.LogPatch("Patched", WrackingRay);
            }
            static void patchGreaterBestowCurse() {
                var BestowCurseGreaterDeterioration = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("71196d7e6d6645247a058a3c3c9bb5fd");
                var BestowCurseGreaterFeebleBody    = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("c74a7dfebd7b1004a80f7e59689dfadd");
                var BestowCurseGreaterIdiocy        = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("f7739a453e2138b46978e9098a29b3fb");
                var BestowCurseGreaterWeakness      = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("abb2d42dd9219eb41848ec56a8726d58");

                var BestowCurseGreaterDeteriorationCast = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("54606d540f5d3684d9f7d6e2e2be9b63");
                var BestowCurseGreaterFeebleBodyCast    = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("292d630a5abae64499bb18057aaa24b4");
                var BestowCurseGreaterIdiocyCast        = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("e0212142d2a426f43926edd4202996bb");
                var BestowCurseGreaterWeaknessCast      = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("1168f36fac0bad64f965928206df7b86");

                var BestowCurseGreaterDeteriorationBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("8f8835d083f31c547a39ebc26ae42159");
                var BestowCurseGreaterFeebleBodyBuff    = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("28c9db77dfb1aa54a94e8a7413b1840a");
                var BestowCurseGreaterIdiocyBuff        = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("493dcc29a21abd94d9adb579e1f40318");
                var BestowCurseGreaterWeaknessBuff      = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("0493a9d25687d7e4682e250ae3ccb187");

                // Update the on cast trigger
                BestowCurseGreaterDeteriorationCast
                    .GetComponent<AbilityEffectStickyTouch>().m_TouchDeliveryAbility = BestowCurseGreaterDeterioration.ToReference<BlueprintAbilityReference>();
                Main.LogPatch("Patched", BestowCurseGreaterDeteriorationCast);
                BestowCurseGreaterFeebleBodyCast
                    .GetComponent<AbilityEffectStickyTouch>().m_TouchDeliveryAbility = BestowCurseGreaterFeebleBody.ToReference<BlueprintAbilityReference>();
                Main.LogPatch("Patched", BestowCurseGreaterFeebleBodyCast);
                BestowCurseGreaterIdiocyCast
                    .GetComponent<AbilityEffectStickyTouch>().m_TouchDeliveryAbility = BestowCurseGreaterIdiocy.ToReference<BlueprintAbilityReference>();
                Main.LogPatch("Patched", BestowCurseGreaterIdiocyCast);
                BestowCurseGreaterWeaknessCast
                    .GetComponent<AbilityEffectStickyTouch>().m_TouchDeliveryAbility = BestowCurseGreaterWeakness.ToReference<BlueprintAbilityReference>();
                Main.LogPatch("Patched", BestowCurseGreaterWeaknessCast);

                // Update the applied buff
                BestowCurseGreaterDeterioration.GetComponent<AbilityEffectRunAction>()
                    .Actions.Actions.OfType<ContextActionConditionalSaved>().First()
                    .Failed.Actions.OfType<ContextActionApplyBuff>().First()
                    .m_Buff = BestowCurseGreaterDeteriorationBuff.ToReference<BlueprintBuffReference>();
                Main.LogPatch("Patched", BestowCurseGreaterDeterioration);

                BestowCurseGreaterFeebleBody.GetComponent<AbilityEffectRunAction>()
                    .Actions.Actions.OfType<ContextActionConditionalSaved>().First()
                    .Failed.Actions.OfType<ContextActionApplyBuff>().First()
                    .m_Buff = BestowCurseGreaterFeebleBodyBuff.ToReference<BlueprintBuffReference>();
                Main.LogPatch("Patched", BestowCurseGreaterFeebleBody);

                BestowCurseGreaterIdiocy.GetComponent<AbilityEffectRunAction>()
                    .Actions.Actions.OfType<ContextActionConditionalSaved>().First()
                    .Failed.Actions.OfType<ContextActionApplyBuff>().First()
                    .m_Buff = BestowCurseGreaterIdiocyBuff.ToReference<BlueprintBuffReference>();
                Main.LogPatch("Patched", BestowCurseGreaterIdiocy);

                BestowCurseGreaterWeakness.GetComponent<AbilityEffectRunAction>()
                    .Actions.Actions.OfType<ContextActionConditionalSaved>().First()
                    .Failed.Actions.OfType<ContextActionApplyBuff>().First()
                    .m_Buff = BestowCurseGreaterWeaknessBuff.ToReference<BlueprintBuffReference>();
                Main.LogPatch("Patched", BestowCurseGreaterWeakness);
            }
            static void patchProtectionFromAlignment() {
                BlueprintAbility ProtectionFromAlignment = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("433b1faf4d02cc34abb0ade5ceda47c4");
                BlueprintAbility ProtectionFromAlignmentCommunal = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("2cadf6c6350e4684baa109d067277a45");
                BlueprintAbilityReference[] ProtectionFromAlignmentVariants = ProtectionFromAlignment
                        .GetComponent<AbilityVariants>()
                        .Variants;
                BlueprintAbilityReference[] ProtectionFromAlignmentCommunalVariants = ProtectionFromAlignmentCommunal
                        .GetComponent<AbilityVariants>()
                        .Variants;
                HashSet<BlueprintBuff> completedBuffs = new HashSet<BlueprintBuff>();
                foreach (BlueprintAbility Variant in ProtectionFromAlignmentVariants.Concat(ProtectionFromAlignmentCommunalVariants)) {
                    var buff = Variant.FlattenAllActions().OfType<ContextActionApplyBuff>().First().Buff;
                    var alignment = buff.GetComponent<SavingThrowBonusAgainstAlignment>().Alignment;

                    string variantDescription = "The subject gains a +2 deflection bonus to AC and a +2 resistance bonus on saves. "
                        + "Both these bonuses apply against attacks made or effects created by good creatures.\n"
                        + "While under the effects of this spell, the target is immune to any new charm or compulsion effects from "
                        + $"{alignment.ToString().ToLower()} spells or spells cast by {alignment.ToString().ToLower()} creatrues.";
                    if (completedBuffs.Add(buff)) { 
                        var SpellImmunity = Helpers.Create<SpellImmunityToSpellDescriptorAgainstAlignment>(c => {
                            c.Alignment = alignment;
                            c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                        });
                        buff.AddComponent(SpellImmunity);
                        buff.SetDescription(variantDescription);
                        Main.LogPatch("Patched", buff);
                    }
                    Variant.SetDescription(variantDescription);
                }
                string baseDescription = "The subject gains a +2 deflection bonus to AC and a +2 resistance bonus on saves. "
                        + "Both these bonuses apply against attacks made or effects created by good creatures.\n"
                        + "While under the effects of this spell, the target is immune to any new charm or compulsion effects from "
                        + $"spells of the corresponding alignment or spells cast by creatrues of the corresponding alignment.";
                ProtectionFromAlignment.SetDescription(baseDescription);
                ProtectionFromAlignmentCommunal.SetDescription(baseDescription);
            }
        }
    }
}
