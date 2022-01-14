using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.ServiceWindows.Spellbook.Metamagic;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TabletopTweaks.Config;
using TabletopTweaks.NewUnitParts;
using TabletopTweaks.Utilities;
using UnityEngine;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

// This work is largly based on work by https://github.com/Stari0n/MagicTime Copyright (c) 2021 Starion
namespace TabletopTweaks.NewContent.MechanicsChanges {
    static class MetamagicExtention {

        [Flags]
        public enum CustomMetamagic {
            Intensified =   0b0000_0000_0000_0000_0001_0000_0000_0000,
            Dazing =        0b0000_0000_0000_0000_0010_0000_0000_0000,
            //Unused Space
            Rime =          0b0000_0000_0000_0001_0000_0000_0000_0000,
            Burning =       0b0000_0000_0000_0010_0000_0000_0000_0000,
            Flaring =       0b0000_0000_0000_0100_0000_0000_0000_0000,
            Piercing =      0b0000_0000_0000_1000_0000_0000_0000_0000,
        }

        public static void RegisterMetamagic(
            Metamagic metamagic,
            string name,
            Sprite icon,
            int defaultCost,
            CustomMechanicsFeature? favoriteMetamagic) {
            var metamagicData = new CustomMetamagicData() {
                Name = name == null ? null : Helpers.CreateString($"{name}SpellMetamagic", name),
                Icon = icon,
                DefaultCost = defaultCost,
                FavoriteMetamagic = favoriteMetamagic
            };
            RegisteredMetamagic.Add(metamagic, metamagicData);
        }

        public static string GetMetamagicName(Metamagic metamagic) {
            CustomMetamagicData result;
            RegisteredMetamagic.TryGetValue(metamagic, out result);
            return result?.Name ?? string.Empty;
        }

        public static Sprite GetMetamagicIcon(Metamagic metamagic) {
            CustomMetamagicData result;
            RegisteredMetamagic.TryGetValue(metamagic, out result);
            return result?.Icon;
        }

        public static int GetMetamagicDefaultCost(Metamagic metamagic) {
            CustomMetamagicData result;
            RegisteredMetamagic.TryGetValue(metamagic, out result);
            return result?.DefaultCost ?? 0;
        }

        public static bool HasFavoriteMetamagic(UnitDescriptor unit, Metamagic metamagic) {
            CustomMetamagicData result;
            RegisteredMetamagic.TryGetValue(metamagic, out result);
            return result?.FavoriteMetamagic == null ? false : unit.CustomMechanicsFeature(result.FavoriteMetamagic.Value);
        }

        public static bool IsRegisistered(Metamagic metamagic) {
            return RegisteredMetamagic.ContainsKey(metamagic);
        }

        private static Dictionary<Metamagic, CustomMetamagicData> RegisteredMetamagic = new();

        private class CustomMetamagicData {
            public LocalizedString Name;
            public Sprite Icon;
            public int DefaultCost;
            public CustomMechanicsFeature? FavoriteMetamagic;
        }

        public static bool IsNewMetamagic(this Metamagic metamagic) {
            return (int)metamagic >= (int)CustomMetamagic.Intensified;
        }

        [HarmonyPatch(typeof(RuleApplyMetamagic), "OnTrigger")]
        static class RuleApplyMetamagic_OnTrigger_NewMetamagic_Patch {
            static void Postfix(RuleApplyMetamagic __instance) {
                var lv_adjustment = 0;
                foreach (var metamagic in __instance.AppliedMetamagics) {
                    if (MetamagicExtention.HasFavoriteMetamagic(__instance.Initiator, metamagic)) {
                        lv_adjustment++;
                    }
                }
                __instance.Result.SpellLevelCost -= lv_adjustment;
                var CompletlyNormalCorrection = 0;
                if (__instance.AppliedMetamagics.Contains(Metamagic.CompletelyNormal)) {
                    CompletlyNormalCorrection = 1;
                }
                if (__instance.BaseLevel + __instance.Result.SpellLevelCost + CompletlyNormalCorrection < 0) {
                    __instance.Result.SpellLevelCost = -__instance.BaseLevel;
                }
            }
        }

        [HarmonyPatch(typeof(RuleCollectMetamagic), "AddMetamagic")]
        static class RuleCollectMetamagic_AddMetamagic_NewMetamagic_Patch {
            static void Postfix(RuleCollectMetamagic __instance, Feature metamagicFeature) {
                if (!__instance.KnownMetamagics.Contains(metamagicFeature)) { return; }

                AddMetamagicFeat component = metamagicFeature.GetComponent<AddMetamagicFeat>();
                Metamagic metamagic = component.Metamagic;
                if (__instance.m_SpellLevel < 0) {
                    return;
                }
                if (__instance.m_SpellLevel >= 10) {
                    return;
                }
                if (__instance.m_SpellLevel + component.Metamagic.DefaultCost() > 10) {
                    return;
                }
                if (__instance.Spell != null
                    && !__instance.SpellMetamagics.Contains(metamagicFeature)
                    && (__instance.Spell.AvailableMetamagic & metamagic) == metamagic) {
                    __instance.SpellMetamagics.Add(metamagicFeature);
                }
            }
        }

        //This has to be patched after UIUtilityTexts loads normally or everything explodes
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class UIUtilityTexts_NewMetamagic_Patchs {
            private static bool patched = false;
            static void Postfix() {
                if (patched) { return; }
                var GetMetamagicList = AccessTools.Method(typeof(UIUtilityTexts), "GetMetamagicList");
                var GetMetamagicName = AccessTools.Method(typeof(UIUtilityTexts), "GetMetamagicName");
                var GetMetamagicListPostfix = AccessTools.Method(typeof(UIUtilityTexts_NewMetamagic_Patchs), "GetMetamagicList");
                var GetMetamagicNamePostfix = AccessTools.Method(typeof(UIUtilityTexts_NewMetamagic_Patchs), "GetMetamagicName");
                var harmony = new Harmony(ModSettings.ModEntry.Info.Id);
                harmony.Patch(GetMetamagicList, postfix: new HarmonyMethod(GetMetamagicListPostfix));
                harmony.Patch(GetMetamagicName, postfix: new HarmonyMethod(GetMetamagicNamePostfix));
                patched = true;
            }
            static void GetMetamagicList(ref string __result, Metamagic mask) {
                StringBuilder stringBuilder = new StringBuilder(__result);
                var addComma = !string.IsNullOrEmpty(__result);
                foreach (object obj in Enum.GetValues(typeof(CustomMetamagic))) {
                    Metamagic metamagic = (Metamagic)obj;
                    if (mask.HasMetamagic(metamagic)) {
                        if (!MetamagicExtention.IsRegisistered(metamagic)) { continue; }
                        if (addComma) {
                            stringBuilder.Append(", ");
                        }
                        stringBuilder.Append(MetamagicExtention.GetMetamagicName(metamagic));
                        addComma = true;
                    }
                }
                __result = stringBuilder.ToString();
            }
            static void GetMetamagicName(ref string __result, Metamagic metamagic) {
                if (!string.IsNullOrEmpty(__result)) { return; }
                if (!MetamagicExtention.IsRegisistered(metamagic)) { return; }
                __result = MetamagicExtention.GetMetamagicName(metamagic);
            }
        }

        [HarmonyPatch(typeof(MetamagicHelper), "DefaultCost")]
        static class MetamagicHelper_DefaultCost_NewMetamagic_Patch {
            static void Postfix(ref int __result, Metamagic metamagic) {
                if (MetamagicExtention.IsRegisistered(metamagic)) {
                    __result = MetamagicExtention.GetMetamagicDefaultCost(metamagic);
                }
            }
        }

        [HarmonyPatch(typeof(MetamagicHelper), "SpellIcon")]
        static class MetamagicHelper_SpellIcon_NewMetamagic_Patch {
            private static void Postfix(ref Sprite __result, Metamagic metamagic) {
                if (MetamagicExtention.GetMetamagicIcon(metamagic) != null) {
                    __result = MetamagicExtention.GetMetamagicIcon(metamagic);
                }
            }
        }

        [HarmonyPatch(typeof(SpellbookMetamagicSelectorVM), "GetCost")]
        static class SpellbookMetamagicSelectorVM_GetCost_NewMetamagic_Patch {
            private static void Postfix(SpellbookMetamagicSelectorVM __instance, ref int __result, Metamagic metamagic) {
                if (MetamagicExtention.HasFavoriteMetamagic(__instance.m_Unit.Value, metamagic)) {
                    __result--;
                }
            }
        }

        [HarmonyPatch(typeof(SpellbookMetamagicSelectorVM), "AddMetamagic")]
        static class SpellbookMetamagicSelectorVM_GetCost_AddMetamagic_Patch {
            private static void Postfix(SpellbookMetamagicSelectorVM __instance) {
                var corr = 0;
                if (__instance.m_MetamagicBuilder.Value.AppliedMetamagics.Contains(Metamagic.CompletelyNormal)) {
                    corr = 1;
                }
                if (__instance.CurrentTemporarySpell.Value.SpellLevel < __instance.m_MetamagicBuilder.Value.BaseSpellLevel - corr) {
                    __instance.CurrentTemporarySpell.Value.SpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - corr;
                    __instance.m_MetamagicBuilder.Value.ResultSpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - corr;
                }
            }
        }

        [HarmonyPatch(typeof(SpellbookMetamagicSelectorVM), "RemoveMetamagic")]
        static class SpellbookMetamagicSelectorVM_GetCost_RemoveMetamagic_Patch {
            private static void Postfix(SpellbookMetamagicSelectorVM __instance) {
                var corr = 0;
                if (__instance.m_MetamagicBuilder.Value.AppliedMetamagics.Contains(Metamagic.CompletelyNormal)) {
                    corr = 1;
                }
                if (__instance.CurrentTemporarySpell.Value.SpellLevel < __instance.m_MetamagicBuilder.Value.BaseSpellLevel - corr) {
                    __instance.CurrentTemporarySpell.Value.SpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - corr;
                    __instance.m_MetamagicBuilder.Value.ResultSpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - corr;
                }
            }
        }
        //Intensified Spell Metamagic
        [HarmonyPatch(typeof(ContextActionDealDamage), nameof(ContextActionDealDamage.GetDamageInfo))]
        static class ContextActionDealDamage_IntensifyMetamagic_Patch {
            static void Postfix(ContextActionDealDamage __instance, ref ContextActionDealDamage.DamageInfo __result) {
                if (!MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Intensified)) { return; }

                var context = __instance.Context;
                if (!context.HasMetamagic((Metamagic)CustomMetamagic.Intensified)) { return; }

                var Sources = context.m_RankSources ?? context.AssociatedBlueprint?.GetComponents<ContextRankConfig>();
                if (__instance.Value.DiceCountValue.ValueType == ContextValueType.Rank) {
                    var rankConfig = Sources.Where(crc => crc.m_Type == __instance.Value.DiceCountValue.ValueRank).FirstOrDefault();
                    if (rankConfig && rankConfig.m_BaseValueType == ContextRankBaseValueType.CasterLevel) {
                        var baseValue = rankConfig.ApplyProgression(rankConfig.GetBaseValue(__instance.Context));
                        var FinalCount = Math.Min(baseValue, rankConfig.m_Max + GetMultiplierIncrease(rankConfig));
                        __result.Dices = new DiceFormula(FinalCount, __instance.Value.DiceType);
                    }
                }

                int GetMultiplierIncrease(ContextRankConfig rankConfig) {
                    switch (rankConfig.m_Progression) {
                        case ContextRankProgression.Div2:
                            return 2;
                        case ContextRankProgression.Div2PlusStep:
                            return 5 / 2;
                        case ContextRankProgression.DivStep:
                            return 5 / rankConfig.m_StepLevel;
                        case ContextRankProgression.OnePlusDivStep:
                            return 5 / rankConfig.m_StepLevel;
                        case ContextRankProgression.StartPlusDivStep:
                            return 5 / rankConfig.m_StepLevel;
                        case ContextRankProgression.DelayedStartPlusDivStep:
                            return 5 / rankConfig.m_StepLevel;
                        case ContextRankProgression.DoublePlusBonusValue:
                            return 5 * 2;
                        case ContextRankProgression.MultiplyByModifier:
                            return 5 * rankConfig.m_StepLevel;
                    }
                    return 5;
                }
            }
        }
        //Rime Spell Metamagic
        [HarmonyPatch(typeof(RuleDealDamage), nameof(RuleDealDamage.OnTrigger))]
        static class ContextActionDealDamage_RimeMetamagic_Patch {
            static BlueprintBuffReference RimeEntagledBuff = Resources.GetModBlueprintReference<BlueprintBuffReference>("RimeEntagledBuff");

            static void Postfix(RuleDealDamage __instance) {
                if (!MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Rime)) { return; }

                var context = __instance.Reason.Context;
                if (context == null) { return; }
                if (!context.HasMetamagic((Metamagic)CustomMetamagic.Rime)) { return; }
                if (!context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Cold)) { return; }
                if (!__instance.DamageBundle
                    .OfType<EnergyDamage>()
                    .Where(damage => damage.EnergyType == DamageEnergyType.Cold)
                    .Any(damage => !damage.Immune))  
                { return; }
                var rounds = Math.Max(1, context.Params?.SpellLevel ?? context.SpellLevel).Rounds();
                var buff = __instance.Target?.Descriptor?.AddBuff(RimeEntagledBuff, context, rounds.Seconds);
                if (buff != null) {
                    buff.IsFromSpell = true;
                }
            }
        }
        //Burning Spell Metamagic
        [HarmonyPatch(typeof(RuleDealDamage), nameof(RuleDealDamage.OnTrigger))]
        static class ContextActionDealDamage_BurningMetamagic_Patch {
            static BlueprintBuffReference BurningSpellAcidBuff = Resources.GetModBlueprintReference<BlueprintBuffReference>("BurningSpellAcidBuff");
            static BlueprintBuffReference BurningSpellFireBuff = Resources.GetModBlueprintReference<BlueprintBuffReference>("BurningSpellFireBuff");
            static void Postfix(RuleDealDamage __instance) {
                if (!MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Burning)) { return; }

                var context = __instance.Reason.Context;
                if (context == null) { return; }
                if (!context.HasMetamagic((Metamagic)CustomMetamagic.Burning)) { return; }
                if (!context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Fire | SpellDescriptor.Acid)) { return; }
                if (!__instance.DamageBundle
                    .OfType<EnergyDamage>()
                    .Where(damage => damage.EnergyType == DamageEnergyType.Fire || damage.EnergyType == DamageEnergyType.Acid)
                    .Any(damage => !damage.Immune)) { return; }
                var casterLevel = context.Params?.SpellLevel ?? context.SpellLevel;
                if (context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Fire)) {
                    var newContext = new MechanicsContext(
                        caster: __instance.Initiator,
                        owner: __instance.Target,
                        blueprint: BurningSpellFireBuff
                    );
                    newContext.RecalculateAbilityParams();
                    newContext.Params.CasterLevel = casterLevel * 2;
                    newContext.Params.Metamagic = 0;
                    var buff = __instance.Target?.Descriptor?.AddBuff(BurningSpellFireBuff, newContext, 1.Rounds().Seconds);
                    if (buff != null) {
                        buff.IsFromSpell = true;
                        buff.IsNotDispelable = true;
                    }
                }
                if (context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Acid)) {
                    var newContext = new MechanicsContext(
                        caster: __instance.Initiator,
                        owner: __instance.Target,
                        blueprint: BurningSpellAcidBuff
                    );
                    newContext.RecalculateAbilityParams();
                    newContext.Params.CasterLevel = casterLevel * 2;
                    newContext.Params.Metamagic = 0;
                    var buff = __instance.Target?.Descriptor?.AddBuff(BurningSpellAcidBuff, newContext, 1.Rounds().Seconds);
                    if (buff != null) {
                        buff.IsFromSpell = true;
                        buff.IsNotDispelable = true;
                    }
                }
            }
        }
        //Flaring Spell Metamagic
        [HarmonyPatch(typeof(RuleDealDamage), nameof(RuleDealDamage.OnTrigger))]
        static class ContextActionDealDamage_FlaringMetamagic_Patch {
            static BlueprintBuffReference FlaringDazzledBuff = Resources.GetModBlueprintReference<BlueprintBuffReference>("FlaringDazzledBuff");
            static void Postfix(RuleDealDamage __instance) {
                if (!MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Flaring)) { return; }

                var context = __instance.Reason.Context;
                if (context == null) { return; }
                if (!context.HasMetamagic((Metamagic)CustomMetamagic.Flaring)) { return; }
                if (!context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Fire | SpellDescriptor.Electricity)) { return; }
                if (!__instance.DamageBundle
                    .OfType<EnergyDamage>()
                    .Where(damage => damage.EnergyType == DamageEnergyType.Fire || damage.EnergyType == DamageEnergyType.Electricity)
                    .Any(damage => !damage.Immune))  
                { return; }
                var rounds = Math.Max(1, context.Params?.SpellLevel ?? context.SpellLevel).Rounds();
                var buff = __instance.Target?.Descriptor?.AddBuff(FlaringDazzledBuff, context, rounds.Seconds);
                if (buff != null) {
                    buff.IsFromSpell = true;
                }
            }
        }
        //Piercing Spell Metamagic
        [HarmonyPatch(typeof(RuleSpellResistanceCheck), nameof(RuleSpellResistanceCheck.OnTrigger))]
        static class RuleSpellResistanceCheck_PiercingMetamagic_Patch {
            static void Postfix(RuleSpellResistanceCheck __instance, RulebookEventContext context) {
                if (!MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Piercing)) { return; }
                var isPiercing = __instance.Context?.HasMetamagic((Metamagic)CustomMetamagic.Piercing) ?? false;
                if (!isPiercing) { return; }
                __instance.SpellResistance -= 5;
            }
        }
    }
}
