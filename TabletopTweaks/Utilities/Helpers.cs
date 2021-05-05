// Copyright (c) 2019 Jennifer Messerly
// This code is licensed under MIT license (see LICENSE for details)

using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TabletopTweaks.Utilities {
    public static class Helpers {
        public static T Create<T>(Action<T> init = null) where T : new() {
            var result = new T();
            init?.Invoke(result);
            return result;
        }

        public static LevelEntry LevelEntry(int level, BlueprintFeatureBase feature) {
            return new LevelEntry {
                Level = level,
                Features = {
                    feature
                }
            };
        }

        public static LevelEntry LevelEntry(int level, params BlueprintFeatureBase[] features) {
            return LevelEntry(level, features);
        }

        public static LevelEntry LevelEntry(int level, IEnumerable<BlueprintFeatureBase> features) {
            LevelEntry levelEntry = new LevelEntry();
            levelEntry.Level = level;
            features.ForEach(f => levelEntry.Features.Add(f));
            return levelEntry;
        }
        public static ContextValue CreateContextValueRank(AbilityRankType value = AbilityRankType.Default) => value.CreateContextValue();
        public static ContextValue CreateContextValue(this AbilityRankType value) {
            return new ContextValue() { ValueType = ContextValueType.Rank, ValueRank = value };
        }
        public static ContextValue CreateContextValue(this AbilitySharedValue value) {
            return new ContextValue() { ValueType = ContextValueType.Shared, ValueShared = value };
        }
        public static ActionList CreateActionList(params GameAction[] actions) {
            if (actions == null || actions.Length == 1 && actions[0] == null) actions = Array.Empty<GameAction>();
            return new ActionList() { Actions = actions };
        }
#if false
        public static ContextActionSavingThrow CreateActionSavingThrow(this SavingThrowType savingThrow, params GameAction[] actions) {
            var c = Create<ContextActionSavingThrow>();
            c.Type = savingThrow;
            c.Actions = CreateActionList(actions);
            return c;
        }
        public static ContextActionConditionalSaved CreateConditionalSaved(GameAction[] success, GameAction[] failed) {
            var c = Create<ContextActionConditionalSaved>();
            c.Succeed = CreateActionList(success);
            c.Failed = CreateActionList(failed);
            return c;
        }
#endif

        // All localized strings created in this mod, mapped to their localized key. Populated by CreateString.
        static Dictionary<String, LocalizedString> textToLocalizedString = new Dictionary<string, LocalizedString>();
        static FastRef<LocalizedString, string> localizedString_m_Key = Helpers.CreateFieldSetter<LocalizedString, string>("m_Key");
        public static LocalizedString CreateString(string key, string value) {
            // See if we used the text previously.
            // (It's common for many features to use the same localized text.
            // In that case, we reuse the old entry instead of making a new one.)
            LocalizedString localized;
            if (textToLocalizedString.TryGetValue(value, out localized)) {
                return localized;
            }
            var strings = LocalizationManager.CurrentPack.Strings;
            String oldValue;
            if (strings.TryGetValue(key, out oldValue) && value != oldValue) {
#if DEBUG
                Main.LogDebug($"Info: duplicate localized string `{key}`, different text.");
#endif
            }
            strings[key] = value;
            localized = new LocalizedString();
            localized.m_Key = key;
            //localizedString_m_Key(localized) = key;
            textToLocalizedString[value] = localized;
            return localized;
        }
        public static FastRef<T, S> CreateFieldSetter<T, S>(string name) {
            return new FastRef<T, S>(HarmonyLib.AccessTools.FieldRefAccess<T, S>(HarmonyLib.AccessTools.Field(typeof(T), name)));
            //return new FastSetter<T, S>(HarmonyLib.FastAccess.CreateSetterHandler<T, S>(HarmonyLib.AccessTools.Field(typeof(T), name)));
        }
        public static FastRef<T, S> CreateFieldGetter<T, S>(string name) {
            return new FastRef<T, S>(HarmonyLib.AccessTools.FieldRefAccess<T, S>(HarmonyLib.AccessTools.Field(typeof(T), name)));
            //return new FastGetter<T, S>(HarmonyLib.FastAccess.CreateGetterHandler<T, S>(HarmonyLib.AccessTools.Field(typeof(T), name)));
        }
        public static void SetField(object obj, string name, object value) {
            HarmonyLib.AccessTools.Field(obj.GetType(), name).SetValue(obj, value);
        }
        public static object GetField(object obj, string name) {
            return HarmonyLib.AccessTools.Field(obj.GetType(), name).GetValue(obj);
        }
        // Parses the lowest 64 bits of the Guid (which corresponds to the last 16 characters).
        static ulong ParseGuidLow(String id) => ulong.Parse(id.Substring(id.Length - 16), NumberStyles.HexNumber);
        // Parses the high 64 bits of the Guid (which corresponds to the first 16 characters).
        static ulong ParseGuidHigh(String id) => ulong.Parse(id.Substring(0, id.Length - 16), NumberStyles.HexNumber);
        public static String MergeIds(String guid1, String guid2, String guid3 = null) {
            // Parse into low/high 64-bit numbers, and then xor the two halves.
            ulong low = ParseGuidLow(guid1);
            ulong high = ParseGuidHigh(guid1);

            low ^= ParseGuidLow(guid2);
            high ^= ParseGuidHigh(guid2);

            if (guid3 != null) {
                low ^= ParseGuidLow(guid3);
                high ^= ParseGuidHigh(guid3);
            }
            return high.ToString("x16") + low.ToString("x16");
        }
        public static T CreateCopy<T>(this T original, Action<T> action = null) where T : UnityEngine.Object {
            var clone = UnityEngine.Object.Instantiate(original);
            if (action != null) {
                action(clone);
            }
            return clone;
        }
        public static ContextRankConfig CreateContextRankConfig(
            ContextRankBaseValueType baseValueType = ContextRankBaseValueType.CasterLevel,
            ContextRankProgression progression = ContextRankProgression.AsIs,
            AbilityRankType type = AbilityRankType.Default,
            int? min = null, int? max = null, int startLevel = 0, int stepLevel = 0,
            bool exceptClasses = false, StatType stat = StatType.Unknown,
            BlueprintUnitProperty customProperty = null,
            BlueprintCharacterClass[] classes = null, BlueprintArchetype[] archetypes = null, BlueprintArchetype archetype = null,
            BlueprintFeature feature = null, BlueprintFeature[] featureList = null,
            (int, int)[] customProgression = null) {
            var config = new ContextRankConfig() {
                m_Type = type,
                m_BaseValueType = baseValueType,
                m_Progression = progression,
                m_UseMin = min.HasValue,
                m_Min = min.GetValueOrDefault(),
                m_UseMax = max.HasValue,
                m_Max = max.GetValueOrDefault(),
                m_StartLevel = startLevel,
                m_StepLevel = stepLevel,
                m_Feature = feature.ToReference<BlueprintFeatureReference>(),
                m_ExceptClasses = exceptClasses,
                m_CustomProperty = customProperty.ToReference<BlueprintUnitPropertyReference>(),
                m_Stat = stat,
                m_Class = classes == null ? Array.Empty<BlueprintCharacterClassReference>() : classes.Select(c => c.ToReference<BlueprintCharacterClassReference>()).ToArray(),
                Archetype = archetype.ToReference<BlueprintArchetypeReference>(),
                m_AdditionalArchetypes = archetypes == null ? Array.Empty<BlueprintArchetypeReference>() : archetypes.Select(c => c.ToReference<BlueprintArchetypeReference>()).ToArray(),
                m_FeatureList = featureList == null ? Array.Empty<BlueprintFeatureReference>() : featureList.Select(c => c.ToReference<BlueprintFeatureReference>()).ToArray()
            };
#if false
            var config = Helpers.Create<ContextRankConfig>(bp => {
                bp.m_Type = type;
                bp.m_BaseValueType = baseValueType;
                bp.m_Progression = progression;
                bp.m_UseMin = min.HasValue;
                bp.m_Min = min.GetValueOrDefault();
                bp.m_UseMax = max.HasValue;
                bp.m_Max = max.GetValueOrDefault();
                bp.m_StartLevel = startLevel;
                bp.m_StepLevel = stepLevel;
                bp.m_Feature = feature.ToReference<BlueprintFeatureReference>();
                bp.m_ExceptClasses = exceptClasses;
                bp.m_CustomProperty = customProperty.ToReference<BlueprintUnitPropertyReference>();
                bp.m_Stat = stat;
                bp.m_Class = classes == null ? Array.Empty<BlueprintCharacterClassReference>() : classes.Select(c => c.ToReference<BlueprintCharacterClassReference>()).ToArray();
                bp.Archetype = archetype.ToReference<BlueprintArchetypeReference>();
                bp.m_AdditionalArchetypes = archetypes == null ? Array.Empty<BlueprintArchetypeReference>() : archetypes.Select(c => c.ToReference<BlueprintArchetypeReference>()).ToArray();
                bp.m_FeatureList = featureList == null ? Array.Empty<BlueprintFeatureReference>() : featureList.Select(c => c.ToReference<BlueprintFeatureReference>()).ToArray();
            });
#endif
            return config;
        }
    }
    public delegate ref S FastRef<T, S>(T source = default);
    public delegate void FastSetter<T, S>(T source, S value);
    public delegate S FastGetter<T, S>(T source);
    public delegate object FastInvoke(object target, params object[] paramters);
}