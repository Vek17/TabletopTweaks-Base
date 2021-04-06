using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
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
                if (Resources.Settings.DisableAllSpellFixes) { return; }
                Main.LogHeader("Patching Spell Resources");
                patchWrachingRay();
                patchGreaterBestowCurse();
                patchProtectionFromAlignment();
                patchGreaterProtectionFromAlignment();
                Main.LogHeader("Patching Spells Complete");
            }
            static void patchWrachingRay() {
                if (!Resources.Settings.SpellFixes["WrackingRay"]) { return;  }
                var WrackingRay = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("1cde0691195feae45bab5b83ea3f221e");
                foreach (AbilityEffectRunAction component in WrackingRay.GetComponents<AbilityEffectRunAction>()) {
                    foreach (ContextActionDealDamage action in component.Actions.Actions.OfType<ContextActionDealDamage>()) {
                        action.Value.DiceType = Kingmaker.RuleSystem.DiceType.D4;
                    }
                }
                Main.LogPatch("Patched", WrackingRay);
            }
            static void patchGreaterBestowCurse() {
                if (!Resources.Settings.SpellFixes["GreaterBestowCurse"]) { return; }
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
                if (!Resources.Settings.SpellFixes["ProtectionFromAlignment"]) { return; }
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
                    Main.LogPatch("Patched", Variant);
                }
                string baseDescription = "The subject gains a +2 deflection bonus to AC and a +2 resistance bonus on saves. "
                        + "Both these bonuses apply against attacks made or effects created by good creatures.\n"
                        + "While under the effects of this spell, the target is immune to any new charm or compulsion effects from "
                        + $"spells of the corresponding alignment or spells cast by creatrues of the corresponding alignment.";
                ProtectionFromAlignment.SetDescription(baseDescription);
                ProtectionFromAlignmentCommunal.SetDescription(baseDescription);
                Main.LogPatch("Patched", ProtectionFromAlignment);
                Main.LogPatch("Patched", ProtectionFromAlignmentCommunal);
            }
            static void patchGreaterProtectionFromAlignment() {
                if (!Resources.Settings.SpellFixes["GreaterProtectionFromAlignment"]) { return; }
                patchUnholyAura();
                patchHolyAura();
                patchShieldOfLaw();
                patchCloakOfChoas();

                void patchHolyAura() {
                    BlueprintAbility HolyAura = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("47f9cb1c367a5e4489cfa32fce290f86");
                    BlueprintBuff HolyAuraBuff = HolyAura.FlattenAllActions().OfType<ContextActionApplyBuff>().First().Buff;
                    string description = "A malevolent darkness surrounds the subjects, protecting them from attacks, "
                        + "granting them resistance to spells cast by good creatures, and weakening good creatures when they strike the subjects. "
                        + "This abjuration has four effects.\nFirst, each warded creature gains a +4 deflection bonus to AC and a +4 resistance bonus on saves."
                        + "[LONGSTART] Unlike the effect of protection from good, this benefit applies against all attacks, not just against attacks by good creatures."
                        + "[LONGEND]\nSecond, warded creatures gain spell resistance 25 against good spells and spells cast by good creatures.\n"
                        + "Third, the abjuration protects from all charm or compulsion effects from good spells or spells cast by good creatures.\n"
                        + "Finally, if a good creature succeeds on a melee attack against a warded creature, "
                        + "the offending attacker takes 1d6 points of Strength damage (Fortitude negates).";
                    patchAbility(HolyAura, AlignmentComponent.Good, description);
                }
                void patchUnholyAura() {
                    BlueprintAbility UnholyAura = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("808ab74c12df8784ab4eeaf6a107dbea");
                    BlueprintBuff UnholyAuraBuff = UnholyAura.FlattenAllActions().OfType<ContextActionApplyBuff>().First().Buff;
                    string description = "A brilliant divine radiance surrounds the subjects, protecting them from attacks, "
                        +"granting them resistance to spells cast by evil creatures, and causing evil creatures to become "
                        +"blinded when they strike the subjects. This abjuration has four effects.\nFirst, each warded creature "
                        +"gains a +4 deflection bonus to AC and a +4 resistance bonus on saves.[LONGSTART] Unlike protection from evil, "
                        +"this benefit applies against all attacks, not just against attacks by evil creatures.[LONGEND]\nSecond, each "
                        +"warded creature gains spell resistance 25 against evil spells and spells cast by evil creatures.\n"
                        +"Third, the abjuration protects from all charm or compulsion effects from evil spells or spells cast by evil creatures."
                        +"\nFinally, if an evil creature succeeds on a melee attack against a creature warded by a "
                        +"holy aura, the offending attacker is blinded (Fortitude save negates, "
                        +"as blindness, but against holy aura's save DC).";
                    patchAbility(UnholyAura, AlignmentComponent.Evil, description);
                }
                void patchShieldOfLaw() {
                    BlueprintAbility ShieldOfLaw = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("73e7728808865094b8892613ddfaf7f5");
                    BlueprintBuff ShieldOfLawBuff = ShieldOfLaw.FlattenAllActions().OfType<ContextActionApplyBuff>().First().Buff;
                    string description = "A dim blue glow surrounds the subjects, protecting them from attacks, granting them resistance to spells "
                        +"cast by chaotic creatures, and slowing chaotic creatures when they strike the subjects. This abjuration has four effects."
                        +"\nFirst, each warded creature gains a +4 deflection bonus to AC and a +4 resistance bonus on saves.[LONGSTART] Unlike protection from chaos, "
                        +"this benefit applies against all attacks, not just against attacks by chaotic creatures.[LONGEND]\n"
                        +"Second, a warded creature gains spell resistance 25 against chaotic spells and spells cast by chaotic creatures.\n"
                        + "Third, the abjuration protects from all charm or compulsion effects from chaotic spells or spells cast by chaotic creatures."
                        + "\nFinally, if a chaotic creature succeeds on a melee attack against a warded creature, the attacker is slowed "
                        +"(Will save negates, as the slow spell, but against shield of law's save DC).";
                    patchAbility(ShieldOfLaw, AlignmentComponent.Chaotic, description);
                }
                void patchCloakOfChoas() {
                    BlueprintAbility CloakOfChoas = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("9155dbc8268da1c49a7fc4834fa1a4b1");
                    BlueprintBuff CloakOfChoasBuff = CloakOfChoas.FlattenAllActions().OfType<ContextActionApplyBuff>().First().Buff;
                    string description = "A random pattern of color surrounds the subjects, protecting them from attacks, granting them resistance to spells"
                        +"cast by lawful creatures, and causing lawful creatures that strike the subjects to become confused. This abjuration has four effects."
                        +"\nFirst, each warded creature gains a +4 deflection bonus to AC and a +4 resistance bonus on saves.[LONGSTART] Unlike protection from law, "
                        + "this benefit applies against all attacks, not just against attacks by lawful creatures.[LONGEND]\n"
                        +"Second, each warded creature gains spell resistance 25 against lawful spells and spells cast by lawful creatures.\n"
                        + "Third, the abjuration protects from all charm or compulsion effects from lawful spells or spells cast by lawful creatures."
                        + "\nFinally, if a lawful creature succeeds on a melee attack against a warded creature, the offending attacker is confused "
                        +"for 1 round (Will save negates, as with the confusion spell, but against the save DC of cloak of chaos).";
                    patchAbility(CloakOfChoas, AlignmentComponent.Lawful, description);
                }

                void patchAbility(BlueprintAbility ability, AlignmentComponent alignment, string description) {
                    BlueprintBuff buff = ability.FlattenAllActions().OfType<ContextActionApplyBuff>().First().Buff;
                    var SpellImmunity = Helpers.Create<SpellImmunityToSpellDescriptorAgainstAlignment>(c => {
                        c.Alignment = alignment;
                        c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                    });
                    var BuffImmunity = Helpers.Create<BuffDescriptorImmunityAgainstAlignment>(c => {
                        c.Alignment = alignment;
                        c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                    });
                    ability.ReplaceComponents<SpellImmunityToSpellDescriptor>(SpellImmunity);
                    ability.ReplaceComponents<BuffDescriptorImmunity>(BuffImmunity);
                    ability.SetDescription(description);
                    buff.SetDescription(description);

                    Main.LogPatch("Patched", ability);
                    Main.LogPatch("Patched", buff);
                }
            }
        }
    }
}
