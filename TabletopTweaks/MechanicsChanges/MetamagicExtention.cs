using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
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
            // Owlcat stops at 1 << 9
            Intensified = 1 << 12,
            Dazing = 1 << 13,
            //Unused Space
            Rime = 1 << 16,
            Burning = 1 << 17,
            Flaring = 1 << 18,
            Piercing = 1 << 19,
            SolidShadows = 1 << 20,
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
            static bool Prefix(ref int __result, Metamagic metamagic) {
                if (MetamagicExtention.IsRegisistered(metamagic)) {
                    __result = MetamagicExtention.GetMetamagicDefaultCost(metamagic);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(MetamagicHelper), "SpellIcon")]
        static class MetamagicHelper_SpellIcon_NewMetamagic_Patch {
            private static bool Prefix(ref Sprite __result, Metamagic metamagic) {
                if (MetamagicExtention.GetMetamagicIcon(metamagic) != null) {
                    __result = MetamagicExtention.GetMetamagicIcon(metamagic);
                    return false;
                }
                return true;
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
        // I think these are general patches for base game bugs... Not 100% sure but I am scared to touch things
        [HarmonyPatch(typeof(SpellbookMetamagicSelectorVM), "AddMetamagic")]
        static class SpellbookMetamagicSelectorVM_GetCost_AddMetamagic_Patch {
            private static void Postfix(SpellbookMetamagicSelectorVM __instance) {
                var CompletlyNormalCorrection = 0;
                if (__instance.m_MetamagicBuilder.Value.AppliedMetamagics.Contains(Metamagic.CompletelyNormal)) {
                    CompletlyNormalCorrection = 1;
                }
                if (__instance.CurrentTemporarySpell.Value.SpellLevel < __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection) {
                    __instance.CurrentTemporarySpell.Value.SpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection;
                    __instance.m_MetamagicBuilder.Value.ResultSpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection;
                }
            }
        }
        // I think these are general patches for base game bugs... Not 100% sure but I am scared to touch things
        [HarmonyPatch(typeof(SpellbookMetamagicSelectorVM), "RemoveMetamagic")]
        static class SpellbookMetamagicSelectorVM_GetCost_RemoveMetamagic_Patch {
            private static void Postfix(SpellbookMetamagicSelectorVM __instance) {
                var CompletlyNormalCorrection = 0;
                if (__instance.m_MetamagicBuilder.Value.AppliedMetamagics.Contains(Metamagic.CompletelyNormal)) {
                    CompletlyNormalCorrection = 1;
                }
                if (__instance.CurrentTemporarySpell.Value.SpellLevel < __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection) {
                    __instance.CurrentTemporarySpell.Value.SpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection;
                    __instance.m_MetamagicBuilder.Value.ResultSpellLevel = __instance.m_MetamagicBuilder.Value.BaseSpellLevel - CompletlyNormalCorrection;
                }
            }
        }

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        private static class MetamagicMechanics {
            private static bool MetamagicInitialized = false;
            private static PiercingSpellMechanics PiercingSpell = new();
            private static FlaringSpellMechanics FlaringSpell = new();
            private static BurningSpellMechanics BurningSpell = new();
            private static RimeSpellMechanics RimeSpell = new();
            private static SolidShadowsMechanics SolidShadows = new();
            [HarmonyPriority(Priority.Last)]
            [HarmonyPostfix]
            public static void InitalizeMetamagic() {
                if (MetamagicInitialized) { return; }
                if (MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Piercing)) {
                    EventBus.Subscribe(PiercingSpell);
                }
                if (MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Flaring)) {
                    EventBus.Subscribe(FlaringSpell);
                }
                if (MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Burning)) {
                    EventBus.Subscribe(BurningSpell);
                }
                if (MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.Rime)) {
                    EventBus.Subscribe(RimeSpell);
                }
                if (MetamagicExtention.IsRegisistered((Metamagic)CustomMetamagic.SolidShadows)) {
                    EventBus.Subscribe(SolidShadows);
                }
                MetamagicInitialized = true;
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
            private class RimeSpellMechanics : IAfterRulebookEventTriggerHandler<RuleDealDamage>, IGlobalSubscriber {
                static BlueprintBuffReference RimeEntagledBuff = Resources.GetModBlueprintReference<BlueprintBuffReference>("RimeEntagledBuff");
                public void OnAfterRulebookEventTrigger(RuleDealDamage evt) {
                    var context = evt.Reason.Context;
                    if (context == null) { return; }
                    if (!context.HasMetamagic((Metamagic)CustomMetamagic.Rime)) { return; }
                    if (!context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Cold)) { return; }
                    if (!evt.DamageBundle
                        .OfType<EnergyDamage>()
                        .Where(damage => damage.EnergyType == DamageEnergyType.Cold)
                        .Any(damage => !damage.Immune)) { return; }
                    var rounds = Math.Max(1, context.Params?.SpellLevel ?? context.SpellLevel).Rounds();
                    var buff = evt.Target?.Descriptor?.AddBuff(RimeEntagledBuff, context, rounds.Seconds);
                    if (buff != null) {
                        buff.IsFromSpell = true;
                    }
                }
            }
            private class BurningSpellMechanics : IAfterRulebookEventTriggerHandler<RuleDealDamage>, IGlobalSubscriber {
                private static BlueprintBuffReference BurningSpellAcidBuff = Resources.GetModBlueprintReference<BlueprintBuffReference>("BurningSpellAcidBuff");
                private static BlueprintBuffReference BurningSpellFireBuff = Resources.GetModBlueprintReference<BlueprintBuffReference>("BurningSpellFireBuff");
                public void OnAfterRulebookEventTrigger(RuleDealDamage evt) {
                    var context = evt.Reason.Context;
                    if (context == null) { return; }
                    if (!context.HasMetamagic((Metamagic)CustomMetamagic.Burning)) { return; }
                    if (!context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Fire | SpellDescriptor.Acid)) { return; }
                    if (!evt.DamageBundle
                        .OfType<EnergyDamage>()
                        .Where(damage => damage.EnergyType == DamageEnergyType.Fire || damage.EnergyType == DamageEnergyType.Acid)
                        .Any(damage => !damage.Immune)) { return; }
                    var casterLevel = context.Params?.SpellLevel ?? context.SpellLevel;
                    if (context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Fire)) {
                        Main.Log("Burning FIRE");
                        var fakeContext = new MechanicsContext(
                            caster: evt.Initiator,
                            owner: evt.Target,
                            blueprint: context.SourceAbility
                        );
                        fakeContext.RecalculateAbilityParams();
                        fakeContext.Params.CasterLevel = casterLevel * 2;
                        fakeContext.Params.Metamagic = 0;
                        var buff = evt.Target?.Descriptor?.AddBuff(BurningSpellFireBuff, fakeContext, 1.Rounds().Seconds);
                        if (buff != null) {
                            buff.IsFromSpell = true;
                            buff.IsNotDispelable = true;
                        } else {
                            Main.Log("Buff was null?");
                        }
                    }
                    if (context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Acid)) {
                        var fakeContext = new MechanicsContext(
                            caster: evt.Initiator,
                            owner: evt.Target,
                            blueprint: context.SourceAbility
                        );
                        fakeContext.RecalculateAbilityParams();
                        fakeContext.Params.CasterLevel = casterLevel * 2;
                        fakeContext.Params.Metamagic = 0;
                        var buff = evt.Target?.Descriptor?.AddBuff(BurningSpellAcidBuff, fakeContext, 1.Rounds().Seconds);
                        if (buff != null) {
                            buff.IsFromSpell = true;
                            buff.IsNotDispelable = true;
                        }
                    }
                }
            }
            private class FlaringSpellMechanics : IAfterRulebookEventTriggerHandler<RuleDealDamage>, IGlobalSubscriber {
                private static BlueprintBuffReference FlaringDazzledBuff = Resources.GetModBlueprintReference<BlueprintBuffReference>("FlaringDazzledBuff");
                public void OnAfterRulebookEventTrigger(RuleDealDamage evt) {
                    var context = evt.Reason.Context;
                    if (context == null) { return; }
                    if (!context.HasMetamagic((Metamagic)CustomMetamagic.Flaring)) { return; }
                    if (!context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Fire | SpellDescriptor.Electricity)) { return; }
                    if (!evt.DamageBundle
                        .OfType<EnergyDamage>()
                        .Where(damage => damage.EnergyType == DamageEnergyType.Fire || damage.EnergyType == DamageEnergyType.Electricity)
                        .Any(damage => !damage.Immune)) { return; }

                    var rounds = Math.Max(1, context.Params?.SpellLevel ?? context.SpellLevel).Rounds();
                    var buff = evt.Target?.Descriptor?.AddBuff(FlaringDazzledBuff, context, rounds.Seconds);
                    if (buff != null) {
                        buff.IsFromSpell = true;
                    }
                }
            }
            private class PiercingSpellMechanics : IAfterRulebookEventTriggerHandler<RuleSpellResistanceCheck>, IGlobalSubscriber {
                public void OnAfterRulebookEventTrigger(RuleSpellResistanceCheck evt) {
                    var isPiercing = evt.Context?.HasMetamagic((Metamagic)CustomMetamagic.Piercing) ?? false;
                    if (!isPiercing) { return; }
                    evt.SpellResistance -= 5;
                }
            }
            private class SolidShadowsMechanics : IAfterRulebookEventTriggerHandler<RuleCastSpell>, IGlobalSubscriber {
                public void OnAfterRulebookEventTrigger(RuleCastSpell evt) {
                    var isSolidShadows = evt.Context?.HasMetamagic((Metamagic)CustomMetamagic.SolidShadows) ?? false;
                    if (!isSolidShadows || !evt.Context.IsShadow) { return; }
                    evt.Context.ShadowFactorPercents += 20;
                }
            }
        }
    }
}
