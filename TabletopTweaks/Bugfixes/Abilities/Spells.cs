using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewActions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Abilities {
    class Spells {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Spells.DisableAll) { return; }
                Main.LogHeader("Patching Spells");
                PatchAngelicAspect();
                PatchAngelicAspectGreater();
                PatchBelieveInYourself();
                PatchBestowCurseGreater();
                PatchCrusadersEdge();
                PatchMagicalVestment();
                PatchOdeToMiraculousMagicBuff();
                PatchProtectionFromAlignment();
                PatchProtectionFromAlignmentGreater();
                PatchRemoveFear();
                PatchWrachingRay();
            }
            static void PatchAngelicAspect() {
                if (!ModSettings.Fixes.Spells.Enabled["AngelicAspect"]) { return; }
                var AngelicAspectBuff = Resources.GetBlueprint<BlueprintBuff>("b33f44fecadb3ca48b438dacac6454c2");

                var SpellImmunityAlignment = Helpers.Create<SpellImmunityToSpellDescriptorAgainstAlignment>(c => {
                    c.Alignment = AlignmentComponent.Evil;
                    c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                });
                var BuffImmunityAlignment = Helpers.Create<BuffDescriptorImmunityAgainstAlignment>(c => {
                    c.Alignment = AlignmentComponent.Evil;
                    c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                });
                var FlyingSpellImmunity = Helpers.Create<SpellImmunityToSpellDescriptor>(c => {
                    c.Descriptor = SpellDescriptor.Ground;
                });
                var FlyingBuffImmunity = Helpers.Create<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.Ground;
                });
                AngelicAspectBuff.AddComponents(SpellImmunityAlignment, BuffImmunityAlignment, FlyingSpellImmunity, FlyingBuffImmunity);
                Main.LogPatch("Patched", AngelicAspectBuff);
            }
            static void PatchAngelicAspectGreater() {
                if (!ModSettings.Fixes.Spells.Enabled["AngelicAspectGreater"]) { return; }
                var AngelicAspectGreaterBuff = Resources.GetBlueprint<BlueprintBuff>("87fcda72043d20840b4cdc2adcc69c63");
                var AuraOfAngelicAspectGreaterEffectBuff = Resources.GetBlueprint<BlueprintBuff>("6ab366720f4b8ed4f83ada36994d0890");

                var FlyingSpellImmunity = Helpers.Create<SpellImmunityToSpellDescriptor>(c => {
                    c.Descriptor = SpellDescriptor.Ground;
                });
                var FlyingBuffImmunity = Helpers.Create<BuffDescriptorImmunity>(c => {
                    c.Descriptor = SpellDescriptor.Ground;
                });
                AngelicAspectGreaterBuff.AddComponents(FlyingSpellImmunity, FlyingBuffImmunity);
                Main.LogPatch("Patched", AngelicAspectGreaterBuff);
                var SpellImmunityAlignment = Helpers.Create<SpellImmunityToSpellDescriptorAgainstAlignment>(c => {
                    c.Alignment = AlignmentComponent.Evil;
                    c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                });
                var BuffImmunityAlignment = Helpers.Create<BuffDescriptorImmunityAgainstAlignment>(c => {
                    c.Alignment = AlignmentComponent.Evil;
                    c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                });
                AuraOfAngelicAspectGreaterEffectBuff.AddComponents(SpellImmunityAlignment, BuffImmunityAlignment);
                Main.LogPatch("Patched", AuraOfAngelicAspectGreaterEffectBuff);
            }
            static void PatchBelieveInYourself() {
                if (!ModSettings.Fixes.Spells.Enabled["BelieveInYourself"]) { return; }
                BlueprintAbility BelieveInYourself = Resources.GetBlueprint<BlueprintAbility>("3ed3cef7c267cb847bfd44ed4708b726");
                BlueprintAbilityReference[] BelieveInYourselfVariants = BelieveInYourself
                    .GetComponent<AbilityVariants>()
                    .Variants;
                foreach (BlueprintAbility Variant in BelieveInYourselfVariants) {
                    Variant.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .ForEach(b => {
                            b.Buff.GetComponent<ContextRankConfig>().m_StepLevel = 2;
                            Main.LogPatch("Patched", b.Buff);
                        });
                }
            }
            static void PatchBestowCurseGreater() {
                if (!ModSettings.Fixes.Spells.Enabled["BestowCurseGreater"]) { return; }
                var BestowCurseGreaterDeterioration = Resources.GetBlueprint<BlueprintAbility>("71196d7e6d6645247a058a3c3c9bb5fd");
                var BestowCurseGreaterFeebleBody = Resources.GetBlueprint<BlueprintAbility>("c74a7dfebd7b1004a80f7e59689dfadd");
                var BestowCurseGreaterIdiocy = Resources.GetBlueprint<BlueprintAbility>("f7739a453e2138b46978e9098a29b3fb");
                var BestowCurseGreaterWeakness = Resources.GetBlueprint<BlueprintAbility>("abb2d42dd9219eb41848ec56a8726d58");

                var BestowCurseGreaterDeteriorationCast = Resources.GetBlueprint<BlueprintAbility>("54606d540f5d3684d9f7d6e2e2be9b63");
                var BestowCurseGreaterFeebleBodyCast = Resources.GetBlueprint<BlueprintAbility>("292d630a5abae64499bb18057aaa24b4");
                var BestowCurseGreaterIdiocyCast = Resources.GetBlueprint<BlueprintAbility>("e0212142d2a426f43926edd4202996bb");
                var BestowCurseGreaterWeaknessCast = Resources.GetBlueprint<BlueprintAbility>("1168f36fac0bad64f965928206df7b86");

                var BestowCurseGreaterDeteriorationBuff = Resources.GetBlueprint<BlueprintBuff>("8f8835d083f31c547a39ebc26ae42159");
                var BestowCurseGreaterFeebleBodyBuff = Resources.GetBlueprint<BlueprintBuff>("28c9db77dfb1aa54a94e8a7413b1840a");
                var BestowCurseGreaterIdiocyBuff = Resources.GetBlueprint<BlueprintBuff>("493dcc29a21abd94d9adb579e1f40318");
                var BestowCurseGreaterWeaknessBuff = Resources.GetBlueprint<BlueprintBuff>("0493a9d25687d7e4682e250ae3ccb187");

                RebuildCurse(
                    BestowCurseGreaterDeterioration,
                    BestowCurseGreaterDeteriorationCast,
                    BestowCurseGreaterDeteriorationBuff);
                RebuildCurse(
                    BestowCurseGreaterFeebleBody,
                    BestowCurseGreaterFeebleBodyCast,
                    BestowCurseGreaterFeebleBodyBuff);
                RebuildCurse(
                    BestowCurseGreaterIdiocy,
                    BestowCurseGreaterIdiocyCast,
                    BestowCurseGreaterIdiocyBuff);
                RebuildCurse(
                    BestowCurseGreaterWeakness,
                    BestowCurseGreaterWeaknessCast,
                    BestowCurseGreaterWeaknessBuff);

                void RebuildCurse(BlueprintAbility curse, BlueprintAbility curseCast, BlueprintBuff curseBuff) {
                    curseCast.GetComponent<AbilityEffectStickyTouch>().m_TouchDeliveryAbility = curse.ToReference<BlueprintAbilityReference>();
                    Main.LogPatch("Patched", curseCast);
                    curse.GetComponent<AbilityEffectRunAction>()
                        .Actions.Actions.OfType<ContextActionConditionalSaved>().First()
                        .Failed.Actions.OfType<ContextActionApplyBuff>().First()
                        .m_Buff = curseBuff.ToReference<BlueprintBuffReference>();
                    Main.LogPatch("Patched", curse);
                    curseBuff.m_Icon = curse.m_Icon;
                    Main.LogPatch("Patched", curseBuff);
                }
            }
            static void PatchCrusadersEdge() {
                if (!ModSettings.Fixes.Spells.Enabled["CrusadersEdge"]) { return; }
                BlueprintBuff CrusadersEdgeBuff = Resources.GetBlueprint<BlueprintBuff>("7ca348639a91ae042967f796098e3bc3");
                CrusadersEdgeBuff.GetComponent<AddInitiatorAttackWithWeaponTrigger>().CriticalHit = true;
                Main.LogPatch("Patched", CrusadersEdgeBuff);
            }
            static void PatchMagicalVestment() {
                if (!ModSettings.Fixes.Spells.Enabled["MagicalVestment"]) { return; }
                PatchMagicalVestmentArmor();
                PatchMagicalVestmentShield();

                void PatchMagicalVestmentShield() {
                    var MagicalVestmentShield = Resources.GetBlueprint<BlueprintAbility>("adcda176d1756eb45bd5ec9592073b09");
                    var MagicalVestmentShieldBuff = Resources.GetBlueprint<BlueprintBuff>("2e8446f820936a44f951b50d70a82b16");
                    MagicalVestmentShield.GetComponent<AbilityEffectRunAction>().AddAction(Helpers.Create<EnhanceSheild>(a => {
                        a.EnchantLevel = new ContextValue();
                        a.EnchantLevel.ValueType = ContextValueType.Rank;
                        a.EnchantLevel.Value = 1;
                        a.EnchantLevel.ValueRank = AbilityRankType.ProjectilesCount;

                        a.DurationValue = new ContextDurationValue();
                        a.DurationValue.m_IsExtendable = true;
                        a.DurationValue.Rate = DurationRate.Hours;
                        a.DurationValue.DiceCountValue = new ContextValue();
                        a.DurationValue.BonusValue = new ContextValue();
                        a.DurationValue.BonusValue.ValueType = ContextValueType.Rank;

                        a.m_Enchantment = new BlueprintItemEnchantmentReference[] {
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("1d9b60d57afb45c4f9bb0a3c21bb3b98").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus1
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("d45bfd838c541bb40bde7b0bf0e1b684").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus2
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("51c51d841e9f16046a169729c13c4d4f").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus3
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("a23bcee56c9fcf64d863dafedb369387").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus4
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("15d7d6cbbf56bd744b37bbf9225ea83b").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus5
                        };
                    }));
                    var RankConfig = Helpers.CreateContextRankConfig();
                    RankConfig.m_Type = AbilityRankType.ProjectilesCount;
                    RankConfig.m_Progression = ContextRankProgression.DivStep;
                    RankConfig.m_StepLevel = 4;
                    RankConfig.m_Min = 1;
                    RankConfig.m_Max = 5;

                    MagicalVestmentShield.AddComponent(RankConfig);
                    MagicalVestmentShield.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>().First().IsNotDispelable = true;

                    Main.LogPatch("Patched", MagicalVestmentShield);
                    MagicalVestmentShieldBuff.RemoveComponents<BlueprintComponent>();
                    Main.LogPatch("Patched", MagicalVestmentShieldBuff);
                }
                void PatchMagicalVestmentArmor() {
                    var MagicalVestmentArmor = Resources.GetBlueprint<BlueprintAbility>("956309af83352714aa7ee89fb4ecf201");
                    var MagicalVestmentArmorBuff = Resources.GetBlueprint<BlueprintBuff>("9e265139cf6c07c4fb8298cb8b646de9");
                    MagicalVestmentArmor.GetComponent<AbilityEffectRunAction>().AddAction(Helpers.Create<EnhanceArmor>(a => {
                        a.EnchantLevel = new ContextValue();
                        a.EnchantLevel.ValueType = ContextValueType.Rank;
                        a.EnchantLevel.Value = 1;
                        a.EnchantLevel.ValueRank = AbilityRankType.ProjectilesCount;

                        a.DurationValue = new ContextDurationValue();
                        a.DurationValue.m_IsExtendable = true;
                        a.DurationValue.Rate = DurationRate.Hours;
                        a.DurationValue.DiceCountValue = new ContextValue();
                        a.DurationValue.BonusValue = new ContextValue();
                        a.DurationValue.BonusValue.ValueType = ContextValueType.Rank;

                        a.m_Enchantment = new BlueprintItemEnchantmentReference[] {
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("1d9b60d57afb45c4f9bb0a3c21bb3b98").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus1
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("d45bfd838c541bb40bde7b0bf0e1b684").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus2
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("51c51d841e9f16046a169729c13c4d4f").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus3
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("a23bcee56c9fcf64d863dafedb369387").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus4
                            Resources.GetBlueprint<BlueprintArmorEnchantment>("15d7d6cbbf56bd744b37bbf9225ea83b").ToReference<BlueprintItemEnchantmentReference>(), // TemporaryArmorEnhancementBonus5
                        };
                    }));
                    var RankConfig = Helpers.CreateContextRankConfig();
                    RankConfig.m_Type = AbilityRankType.ProjectilesCount;
                    RankConfig.m_Progression = ContextRankProgression.DivStep;
                    RankConfig.m_StepLevel = 4;
                    RankConfig.m_Min = 1;
                    RankConfig.m_Max = 5;

                    MagicalVestmentArmor.AddComponent(RankConfig);
                    MagicalVestmentArmor.GetComponent<AbilityEffectRunAction>().Actions.Actions.OfType<ContextActionApplyBuff>().First().IsNotDispelable = true;
                    Main.LogPatch("Patched", MagicalVestmentArmor);
                    MagicalVestmentArmorBuff.RemoveComponents<BlueprintComponent>();
                    Main.LogPatch("Patched", MagicalVestmentArmorBuff);
                }
            }
            static void PatchOdeToMiraculousMagicBuff() {
                if (!ModSettings.Fixes.Spells.Enabled["OdeToMiraculousMagic"]) { return; }
                BlueprintBuff OdeToMiraculousMagicBuff = Resources.GetBlueprint<BlueprintBuff>("f6ef0e25745114d46bf16fd5a1d93cc9");
                IncreaseCastersSavingThrowTypeDC bonusSaveDC = Helpers.Create<IncreaseCastersSavingThrowTypeDC>(c => {
                    c.Type = SavingThrowType.Will;
                    c.BonusDC = 2;
                });
                OdeToMiraculousMagicBuff.AddComponent(bonusSaveDC);
                Main.LogPatch("Patched", OdeToMiraculousMagicBuff);
            }
            static void PatchProtectionFromAlignment() {
                if (!ModSettings.Fixes.Spells.Enabled["ProtectionFromAlignment"]) { return; }
                var ProtectionFromAlignment = Resources.GetBlueprint<BlueprintAbility>("433b1faf4d02cc34abb0ade5ceda47c4");
                var ProtectionFromAlignmentVariants = ProtectionFromAlignment
                        .GetComponent<AbilityVariants>()
                        .Variants;
                var ProtectionFromAlignmentCommunal = Resources.GetBlueprint<BlueprintAbility>("2cadf6c6350e4684baa109d067277a45");
                var ProtectionFromAlignmentCommunalVariants = ProtectionFromAlignmentCommunal
                        .GetComponent<AbilityVariants>()
                        .Variants;
                var ProtectionFromChaosEvil = Resources.GetBlueprint<BlueprintAbility>("c28f7234f5fb8c943a77621ad96ad8f9");
                var ProtectionFromChaosEvilVariants = ProtectionFromChaosEvil
                        .GetComponent<AbilityVariants>()
                        .Variants;
                var ProtectionFromChaosEvilCommunal = Resources.GetBlueprint<BlueprintAbility>("3026de673d4d8fe45baf40e0b5edd718");
                var ProtectionFromChaosEvilCommunalVariants = ProtectionFromChaosEvilCommunal
                        .GetComponent<AbilityVariants>()
                        .Variants;

                HashSet<BlueprintBuff> completedBuffs = new HashSet<BlueprintBuff>();
                foreach (BlueprintAbility Variant in ProtectionFromAlignmentVariants
                    .Concat(ProtectionFromAlignmentCommunalVariants)
                    .Concat(ProtectionFromChaosEvilVariants)
                    .Concat(ProtectionFromChaosEvilCommunalVariants)) {
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
                        var BuffImmunity = Helpers.Create<BuffDescriptorImmunityAgainstAlignment>(c => {
                            c.Alignment = alignment;
                            c.Descriptor = SpellDescriptor.Charm | SpellDescriptor.Compulsion;
                        });
                        buff.AddComponents(SpellImmunity, BuffImmunity);
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
                ProtectionFromChaosEvil.SetDescription(baseDescription);
                ProtectionFromChaosEvilCommunal.SetDescription(baseDescription);
                Main.LogPatch("Patched", ProtectionFromAlignment);
                Main.LogPatch("Patched", ProtectionFromAlignmentCommunal);
                Main.LogPatch("Patched", ProtectionFromChaosEvil);
                Main.LogPatch("Patched", ProtectionFromChaosEvilCommunal);
            }
            static void PatchProtectionFromAlignmentGreater() {
                if (!ModSettings.Fixes.Spells.Enabled["ProtectionFromAlignmentGreater"]) { return; }
                patchUnholyAura();
                patchHolyAura();
                patchShieldOfLaw();
                patchCloakOfChoas();

                void patchHolyAura() {
                    BlueprintAbility HolyAura = Resources.GetBlueprint<BlueprintAbility>("47f9cb1c367a5e4489cfa32fce290f86");
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
                    BlueprintAbility UnholyAura = Resources.GetBlueprint<BlueprintAbility>("808ab74c12df8784ab4eeaf6a107dbea");
                    BlueprintBuff UnholyAuraBuff = UnholyAura.FlattenAllActions().OfType<ContextActionApplyBuff>().First().Buff;
                    string description = "A brilliant divine radiance surrounds the subjects, protecting them from attacks, "
                        + "granting them resistance to spells cast by evil creatures, and causing evil creatures to become "
                        + "blinded when they strike the subjects. This abjuration has four effects.\nFirst, each warded creature "
                        + "gains a +4 deflection bonus to AC and a +4 resistance bonus on saves.[LONGSTART] Unlike protection from evil, "
                        + "this benefit applies against all attacks, not just against attacks by evil creatures.[LONGEND]\nSecond, each "
                        + "warded creature gains spell resistance 25 against evil spells and spells cast by evil creatures.\n"
                        + "Third, the abjuration protects from all charm or compulsion effects from evil spells or spells cast by evil creatures."
                        + "\nFinally, if an evil creature succeeds on a melee attack against a creature warded by a "
                        + "holy aura, the offending attacker is blinded (Fortitude save negates, "
                        + "as blindness, but against holy aura's save DC).";
                    patchAbility(UnholyAura, AlignmentComponent.Evil, description);
                }
                void patchShieldOfLaw() {
                    BlueprintAbility ShieldOfLaw = Resources.GetBlueprint<BlueprintAbility>("73e7728808865094b8892613ddfaf7f5");
                    BlueprintBuff ShieldOfLawBuff = ShieldOfLaw.FlattenAllActions().OfType<ContextActionApplyBuff>().First().Buff;
                    string description = "A dim blue glow surrounds the subjects, protecting them from attacks, granting them resistance to spells "
                        + "cast by chaotic creatures, and slowing chaotic creatures when they strike the subjects. This abjuration has four effects."
                        + "\nFirst, each warded creature gains a +4 deflection bonus to AC and a +4 resistance bonus on saves.[LONGSTART] Unlike protection from chaos, "
                        + "this benefit applies against all attacks, not just against attacks by chaotic creatures.[LONGEND]\n"
                        + "Second, a warded creature gains spell resistance 25 against chaotic spells and spells cast by chaotic creatures.\n"
                        + "Third, the abjuration protects from all charm or compulsion effects from chaotic spells or spells cast by chaotic creatures."
                        + "\nFinally, if a chaotic creature succeeds on a melee attack against a warded creature, the attacker is slowed "
                        + "(Will save negates, as the slow spell, but against shield of law's save DC).";
                    patchAbility(ShieldOfLaw, AlignmentComponent.Chaotic, description);
                }
                void patchCloakOfChoas() {
                    BlueprintAbility CloakOfChoas = Resources.GetBlueprint<BlueprintAbility>("9155dbc8268da1c49a7fc4834fa1a4b1");
                    BlueprintBuff CloakOfChoasBuff = CloakOfChoas.FlattenAllActions().OfType<ContextActionApplyBuff>().First().Buff;
                    string description = "A random pattern of color surrounds the subjects, protecting them from attacks, granting them resistance to spells"
                        + "cast by lawful creatures, and causing lawful creatures that strike the subjects to become confused. This abjuration has four effects."
                        + "\nFirst, each warded creature gains a +4 deflection bonus to AC and a +4 resistance bonus on saves.[LONGSTART] Unlike protection from law, "
                        + "this benefit applies against all attacks, not just against attacks by lawful creatures.[LONGEND]\n"
                        + "Second, each warded creature gains spell resistance 25 against lawful spells and spells cast by lawful creatures.\n"
                        + "Third, the abjuration protects from all charm or compulsion effects from lawful spells or spells cast by lawful creatures."
                        + "\nFinally, if a lawful creature succeeds on a melee attack against a warded creature, the offending attacker is confused "
                        + "for 1 round (Will save negates, as with the confusion spell, but against the save DC of cloak of chaos).";
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
                    ability.SetDescription(description);
                    buff.ReplaceComponents<SpellImmunityToSpellDescriptor>(SpellImmunity);
                    buff.ReplaceComponents<BuffDescriptorImmunity>(BuffImmunity);
                    buff.SetDescription(description);

                    Main.LogPatch("Patched", ability);
                    Main.LogPatch("Patched", buff);
                }
            }
            static void PatchRemoveFear() {
                if (!ModSettings.Fixes.Spells.Enabled["RemoveFear"]) { return; }
                var RemoveFear = Resources.GetBlueprint<BlueprintAbility>("55a037e514c0ee14a8e3ed14b47061de");
                var RemoveFearBuff = Resources.GetBlueprint<BlueprintBuff>("c5c86809a1c834e42a2eb33133e90a28");
                var suppressFear = Helpers.Create<SuppressBuffsPersistant>(c => {
                    c.Descriptor = SpellDescriptor.Frightened | SpellDescriptor.Shaken | SpellDescriptor.Fear;
                });
                RemoveFearBuff.RemoveComponents<AddConditionImmunity>();
                RemoveFearBuff.AddComponent(suppressFear);
                Main.LogPatch("Patched", RemoveFearBuff);
            }
            static void PatchWrachingRay() {
                if (!ModSettings.Fixes.Spells.Enabled["WrackingRay"]) { return; }
                var WrackingRay = Resources.GetBlueprint<BlueprintAbility>("1cde0691195feae45bab5b83ea3f221e");
                foreach (AbilityEffectRunAction component in WrackingRay.GetComponents<AbilityEffectRunAction>()) {
                    foreach (ContextActionDealDamage action in component.Actions.Actions.OfType<ContextActionDealDamage>()) {
                        action.Value.DiceType = Kingmaker.RuleSystem.DiceType.D4;
                    }
                }
                Main.LogPatch("Patched", WrackingRay);
            }
        }
    }
}
