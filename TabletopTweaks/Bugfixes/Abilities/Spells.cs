using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewActions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.NewComponents.OwlcatReplacements;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Abilities {
    class Spells {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Spells");
                PatchBelieveInYourself();
                PatchBestowCurseGreater();
                PatchBreakEnchantment();
                PatchChainLightning();
                PatchCrusadersEdge();
                PatchDispelMagicGreater();
                PatchFirebrand();
                PatchGeniekind();
                PatchMagicalVestment();
                PatchMagicWeaponGreater();
                PatchRemoveFear();
                PatchShadowConjuration();
                PatchShadowEvocation();
                PatchShadowEvocationGreater();
                PatchStarlight();
                PatchWrachingRay();
                PatchFromSpellFlags();
            }

            static void PatchBelieveInYourself() {
                if (ModSettings.Fixes.Spells.IsDisabled("BelieveInYourself")) { return; }
                BlueprintAbility BelieveInYourself = Resources.GetBlueprint<BlueprintAbility>("3ed3cef7c267cb847bfd44ed4708b726");
                BlueprintAbilityReference[] BelieveInYourselfVariants = BelieveInYourself
                    .GetComponent<AbilityVariants>()
                    .Variants;
                foreach (BlueprintAbility Variant in BelieveInYourselfVariants) {
                    Variant.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .ForEach(b => {
                            var ContextRankConfig = b.Buff.GetComponent<ContextRankConfig>();
                            ContextRankConfig.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                            ContextRankConfig.m_Progression = ContextRankProgression.DivStep;
                            ContextRankConfig.m_StepLevel = 4;
                            Main.LogPatch("Patched", b.Buff);
                        });
                }
            }

            static void PatchBestowCurseGreater() {
                if (ModSettings.Fixes.Spells.IsDisabled("BestowCurseGreater")) { return; }
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

            static void PatchBreakEnchantment() {
                if (ModSettings.Fixes.Spells.IsDisabled("BreakEnchantment")) { return; }
                var BreakEnchantment = Resources.GetBlueprint<BlueprintAbility>("7792da00c85b9e042a0fdfc2b66ec9a8");
                BreakEnchantment
                    .FlattenAllActions()
                    .OfType<ContextActionDispelMagic>()
                    .ForEach(dispel => {
                        dispel.OnlyTargetEnemyBuffs = true;
                        //dispel.m_MaxSpellLevel.Value = 10;
                    });
                Main.LogPatch("Patched", BreakEnchantment);
            }

            static void PatchChainLightning() {
                if (ModSettings.Fixes.Spells.IsDisabled("ChainLightning")) { return; }
                var ChainLightning = Resources.GetBlueprint<BlueprintAbility>("645558d63604747428d55f0dd3a4cb58");
                ChainLightning
                    .FlattenAllActions()
                    .OfType<ContextActionDealDamage>()
                    .ForEach(damage => {
                        damage.Value.DiceCountValue.ValueRank = AbilityRankType.DamageDice;
                    });
                Main.LogPatch("Patched", ChainLightning);
            }

            static void PatchCrusadersEdge() {
                if (ModSettings.Fixes.Spells.IsDisabled("CrusadersEdge")) { return; }
                BlueprintBuff CrusadersEdgeBuff = Resources.GetBlueprint<BlueprintBuff>("7ca348639a91ae042967f796098e3bc3");
                CrusadersEdgeBuff.GetComponent<AddInitiatorAttackWithWeaponTrigger>().CriticalHit = true;
                Main.LogPatch("Patched", CrusadersEdgeBuff);
            }

            static void PatchDispelMagicGreater() {
                if (ModSettings.Fixes.Spells.IsDisabled("DispelMagicGreater")) { return; }
                var DispelMagicGreaterTarget = Resources.GetBlueprint<BlueprintAbility>("6d490c80598f1d34bb277735b52d52c1");
                DispelMagicGreaterTarget.SetDescription("This functions as a targeted dispel magic, but it can dispel one spell for every four caster " +
                    "levels you possess, starting with the highest level spells and proceeding to lower level spells.\n" +
                    "Targeted Dispel: One object, creature, or spell is the target of the dispel magic spell. You make one dispel check (1d20 + your caster level)" +
                    " and compare that to the spell with highest caster level (DC = 11 + the spell’s caster level). If successful, that spell ends. " +
                    "If not, compare the same result to the spell with the next highest caster level. Repeat this process until you have dispelled " +
                    "one spell affecting the target, or you have failed to dispel every spell.");
                Main.LogPatch("Patched", DispelMagicGreaterTarget);
            }

            static void PatchFirebrand() {
                if (ModSettings.Fixes.Spells.IsDisabled("Firebrand")) { return; }
                var FirebrandBuff = Resources.GetBlueprint<BlueprintBuff>("c6cc1c5356db4674dbd2be20ea205c86");

                var EnergyType = FirebrandBuff.FlattenAllActions().OfType<ContextActionDealDamage>().First().DamageType.Energy;
                FirebrandBuff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                FirebrandBuff.AddComponent<AddAdditionalWeaponDamage>(c => {
                    c.Value = new ContextDiceValue() {
                        DiceType = DiceType.D6,
                        DiceCountValue = 1,
                        BonusValue = 0
                    };
                    c.DamageType = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Energy = EnergyType
                    };
                });
                Main.LogPatch("Patched", FirebrandBuff);
            }

            static void PatchGeniekind() {
                if (ModSettings.Fixes.Spells.IsDisabled("Geniekind")) { return; }
                var GeniekindDjinniBuff = Resources.GetBlueprint<BlueprintBuff>("082caf8c1005f114ba6375a867f638cf");
                var GeniekindEfreetiBuff = Resources.GetBlueprint<BlueprintBuff>("d47f45f29c4cfc0469f3734d02545e0b");
                var GeniekindMaridBuff = Resources.GetBlueprint<BlueprintBuff>("4f37fc07fe2cf7f4f8076e79a0a3bfe9");
                var GeniekindShaitanBuff = Resources.GetBlueprint<BlueprintBuff>("1d498104f8e35e246b5d8180b0faed43");

                ReplaceComponents(GeniekindDjinniBuff);
                ReplaceComponents(GeniekindEfreetiBuff);
                ReplaceComponents(GeniekindMaridBuff);
                ReplaceComponents(GeniekindShaitanBuff);

                void ReplaceComponents(BlueprintBuff GeniekindBuff) {
                    var EnergyType = GeniekindBuff.FlattenAllActions().OfType<ContextActionDealDamage>().First().DamageType.Energy;
                    GeniekindBuff.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                    GeniekindBuff.AddComponent<AddAdditionalWeaponDamage>(c => {
                        c.CheckWeaponRangeType = true;
                        c.RangeType = WeaponRangeType.Melee;
                        c.Value = new ContextDiceValue() {
                            DiceType = DiceType.D6,
                            DiceCountValue = 1,
                            BonusValue = 0
                        };
                        c.DamageType = new DamageTypeDescription() {
                            Type = DamageType.Energy,
                            Energy = EnergyType
                        };
                    });
                    Main.LogPatch("Patched", GeniekindBuff);
                }
            }

            static void PatchMagicalVestment() {
                if (ModSettings.Fixes.Spells.IsDisabled("MagicalVestment")) { return; }
                PatchMagicalVestmentArmor();
                PatchMagicalVestmentShield();

                void PatchMagicalVestmentShield() {
                    var MagicalVestmentShield = Resources.GetBlueprint<BlueprintAbility>("adcda176d1756eb45bd5ec9592073b09");
                    var MagicalVestmentShieldBuff = Resources.GetBlueprint<BlueprintBuff>("2e8446f820936a44f951b50d70a82b16");

                    MagicalVestmentShieldBuff.SetComponents();
                    MagicalVestmentShieldBuff.AddComponent<MagicalVestmentComponent>(c => {
                        c.Shield = true;
                        c.EnchantLevel = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                        c.m_EnchantmentBlueprints = new BlueprintItemEnchantmentReference[] {
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("1d9b60d57afb45c4f9bb0a3c21bb3b98"), // TemporaryArmorEnhancementBonus1
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("d45bfd838c541bb40bde7b0bf0e1b684"), // TemporaryArmorEnhancementBonus2
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("51c51d841e9f16046a169729c13c4d4f"), // TemporaryArmorEnhancementBonus3
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("a23bcee56c9fcf64d863dafedb369387"), // TemporaryArmorEnhancementBonus4
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("15d7d6cbbf56bd744b37bbf9225ea83b")  // TemporaryArmorEnhancementBonus5
                        };
                    });
                    MagicalVestmentShieldBuff.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.DivStep;
                        c.m_StepLevel = 4;
                        c.m_Min = 1;
                        c.m_Max = 5;
                    });
                    Main.LogPatch("Patched", MagicalVestmentShieldBuff);
                }
                void PatchMagicalVestmentArmor() {
                    var MagicalVestmentArmor = Resources.GetBlueprint<BlueprintAbility>("956309af83352714aa7ee89fb4ecf201");
                    var MagicalVestmentArmorBuff = Resources.GetBlueprint<BlueprintBuff>("9e265139cf6c07c4fb8298cb8b646de9");

                    MagicalVestmentArmorBuff.SetComponents();
                    MagicalVestmentArmorBuff.AddComponent<MagicalVestmentComponent>(c => {
                        c.EnchantLevel = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                        c.m_EnchantmentBlueprints = new BlueprintItemEnchantmentReference[] {
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("1d9b60d57afb45c4f9bb0a3c21bb3b98"), // TemporaryArmorEnhancementBonus1
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("d45bfd838c541bb40bde7b0bf0e1b684"), // TemporaryArmorEnhancementBonus2
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("51c51d841e9f16046a169729c13c4d4f"), // TemporaryArmorEnhancementBonus3
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("a23bcee56c9fcf64d863dafedb369387"), // TemporaryArmorEnhancementBonus4
                            Resources.GetBlueprintReference<BlueprintItemEnchantmentReference>("15d7d6cbbf56bd744b37bbf9225ea83b")  // TemporaryArmorEnhancementBonus5
                        };
                    });
                    MagicalVestmentArmorBuff.AddContextRankConfig(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.DivStep;
                        c.m_StepLevel = 4;
                        c.m_Min = 1;
                        c.m_Max = 5;
                    });
                    Main.LogPatch("Patched", MagicalVestmentArmorBuff);
                }
            }

            static void PatchMagicWeaponGreater() {
                if (ModSettings.Fixes.Spells.IsDisabled("GreaterMagicWeapon")) { return; }

                var MagicWeaponGreaterPrimary = Resources.GetBlueprint<BlueprintAbility>("a3fe23711486ee9489af1dadd6906149");
                var MagicWeaponGreaterSecondary = Resources.GetBlueprint<BlueprintAbility>("89c13df989e5e624692134d55195121a");
                var newEnhancements = new BlueprintItemEnchantmentReference[] {
                Resources.GetModBlueprint<BlueprintWeaponEnchantment>("TemporaryEnhancement1NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
                Resources.GetModBlueprint<BlueprintWeaponEnchantment>("TemporaryEnhancement2NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
                Resources.GetModBlueprint<BlueprintWeaponEnchantment>("TemporaryEnhancement3NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
                Resources.GetModBlueprint<BlueprintWeaponEnchantment>("TemporaryEnhancement4NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
                Resources.GetModBlueprint<BlueprintWeaponEnchantment>("TemporaryEnhancement5NonStacking").ToReference<BlueprintItemEnchantmentReference>(),
            };

                MagicWeaponGreaterPrimary.FlattenAllActions().OfType<EnhanceWeapon>().ForEach(c => c.m_Enchantment = newEnhancements);
                MagicWeaponGreaterSecondary.FlattenAllActions().OfType<EnhanceWeapon>().ForEach(c => c.m_Enchantment = newEnhancements);

                Main.LogPatch("Patched", MagicWeaponGreaterPrimary);
                Main.LogPatch("Patched", MagicWeaponGreaterSecondary);
            }

            static void PatchRemoveFear() {
                if (ModSettings.Fixes.Spells.IsDisabled("RemoveFear")) { return; }
                var RemoveFear = Resources.GetBlueprint<BlueprintAbility>("55a037e514c0ee14a8e3ed14b47061de");
                var RemoveFearBuff = Resources.GetBlueprint<BlueprintBuff>("c5c86809a1c834e42a2eb33133e90a28");
                var suppressFear = Helpers.Create<SuppressBuffsPersistant>(c => {
                    c.Descriptor = SpellDescriptor.Frightened | SpellDescriptor.Shaken | SpellDescriptor.Fear;
                });
                RemoveFearBuff.RemoveComponents<AddConditionImmunity>();
                RemoveFearBuff.AddComponent(suppressFear);
                Main.LogPatch("Patched", RemoveFearBuff);
            }

            static void PatchShadowConjuration() {
                if (ModSettings.Fixes.Spells.IsDisabled("ShadowConjuration")) { return; }
                var ShadowConjuration = Resources.GetBlueprint<BlueprintAbility>("caac251ca7601324bbe000372a0a1005");
                ShadowConjuration.AddToSpellList(SpellTools.SpellList.WizardSpellList, 4);
                Main.LogPatch("Patched", ShadowConjuration);
            }

            static void PatchShadowEvocation() {
                if (ModSettings.Fixes.Spells.IsDisabled("ShadowEvocation")) { return; }
                var ShadowEvocation = Resources.GetBlueprint<BlueprintAbility>("237427308e48c3341b3d532b9d3a001f");
                ShadowEvocation.AvailableMetamagic |= Metamagic.Empower
                    | Metamagic.Maximize
                    | Metamagic.Quicken
                    | Metamagic.Heighten
                    | Metamagic.Reach
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent
                    | Metamagic.Selective
                    | Metamagic.Bolstered;
                Main.LogPatch("Patched", ShadowEvocation);
            }

            static void PatchShadowEvocationGreater() {
                if (ModSettings.Fixes.Spells.IsDisabled("ShadowEvocationGreater")) { return; }
                var ShadowEvocationGreater = Resources.GetBlueprint<BlueprintAbility>("3c4a2d4181482e84d9cd752ef8edc3b6");
                ShadowEvocationGreater.AvailableMetamagic |= Metamagic.Empower
                    | Metamagic.Maximize
                    | Metamagic.Quicken
                    | Metamagic.Heighten
                    | Metamagic.Reach
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent
                    | Metamagic.Selective
                    | Metamagic.Bolstered;
                Main.LogPatch("Patched", ShadowEvocationGreater);
            }

            static void PatchStarlight() {
                if (ModSettings.Fixes.Spells.IsDisabled("Starlight")) { return; }
                var StarlightAllyBuff = Resources.GetBlueprint<BlueprintBuff>("f4ead47adc2ca2744a00efd4e088ecb2");
                StarlightAllyBuff.GetComponent<AddConcealment>().Descriptor = ConcealmentDescriptor.InitiatorIsBlind;
                Main.LogPatch("Patched", StarlightAllyBuff);
            }

            static void PatchWrachingRay() {
                if (ModSettings.Fixes.Spells.IsDisabled("WrackingRay")) { return; }
                var WrackingRay = Resources.GetBlueprint<BlueprintAbility>("1cde0691195feae45bab5b83ea3f221e");
                foreach (AbilityEffectRunAction component in WrackingRay.GetComponents<AbilityEffectRunAction>()) {
                    foreach (ContextActionDealDamage action in component.Actions.Actions.OfType<ContextActionDealDamage>()) {
                        action.Value.DiceType = Kingmaker.RuleSystem.DiceType.D4;
                    }
                }
                Main.LogPatch("Patched", WrackingRay);
            }

            static void PatchFromSpellFlags() {
                if (ModSettings.Fixes.Spells.IsDisabled("FixSpellFlags")) { return; }
                Main.Log("Updating Spell Flags");
                SpellTools.SpellList.AllSpellLists
                    .SelectMany(list => list.SpellsByLevel)
                    .Where(spellList => spellList.SpellLevel != 0)
                    .SelectMany(level => level.Spells)
                    .Distinct()
                    .SelectMany(spell => spell.AbilityAndVariants())
                    .OrderBy(spell => spell.Name)
                    .SelectMany(a => a.FlattenAllActions())
                    .OfType<ContextActionApplyBuff>()
                    .Distinct()
                    .Select(a => a.Buff)
                    .Distinct()
                    .OrderBy(buff => buff.name)
                    .ForEach(buff => {
                        if (buff.GetComponent<AddCondition>() == null
                        && buff.GetComponent<BuffStatusCondition>() == null
                        && buff.GetComponent<BuffPoisonStatDamage>() == null
                        && (buff.SpellDescriptor & SpellDescriptor.Bleed) == 0) {
                            buff.m_Flags |= BlueprintBuff.Flags.IsFromSpell;
                            Main.LogPatch("Patched", buff);
                        }
                    });
                Main.Log("Finished Spell Flags");
            }
        }
    }
}