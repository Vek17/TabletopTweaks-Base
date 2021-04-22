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
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace TabletopTweaks.Utilities {
    public static class Helpers {

        public static T Create<T>(Action<T> init = null) where T : ScriptableObject {
            var result = ScriptableObject.CreateInstance<T>();
            if (init != null) init(result);
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
        public static ContextRankConfig CreateContextRankConfig(
            ContextRankBaseValueType baseValueType = ContextRankBaseValueType.CasterLevel,
            ContextRankProgression progression = ContextRankProgression.AsIs,
            AbilityRankType type = AbilityRankType.Default,
            int? min = null, int? max = null, int startLevel = 0, int stepLevel = 0,
            bool exceptClasses = false, StatType stat = StatType.Unknown,
            BlueprintUnitProperty customProperty = null,
            BlueprintCharacterClass[] classes = null, BlueprintArchetype[] archetypes = null,
            BlueprintFeature feature = null, BlueprintFeature[] featureList = null) {

            var config = Create<ContextRankConfig>();
            config.m_Type = type;
            config.m_BaseValueType = baseValueType;
            config.m_Progression = progression;
            config.m_UseMin = min.HasValue;
            config.m_Min = min.GetValueOrDefault();
            config.m_UseMax = max.HasValue;
            config.m_Max = max.GetValueOrDefault();
            config.m_StartLevel = startLevel;
            config.m_StepLevel = stepLevel;
            config.m_Feature = feature.ToReference<BlueprintFeatureReference>();
            config.m_ExceptClasses = exceptClasses;
            config.m_CustomProperty = customProperty.ToReference<BlueprintUnitPropertyReference>();
            config.m_Stat = stat;
            config.m_Class = classes != null ? classes.Select(c => c.ToReference<BlueprintCharacterClassReference>()).ToArray() : Array.Empty<BlueprintCharacterClassReference>();
            config.m_AdditionalArchetypes = archetypes != null ? archetypes.Select(c => c.ToReference<BlueprintArchetypeReference>()).ToArray() : Array.Empty<BlueprintArchetypeReference>();
            config.m_FeatureList = featureList != null ? featureList.Select(c => c.ToReference<BlueprintFeatureReference>()).ToArray() : Array.Empty<BlueprintFeatureReference>();

            return config;
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
    }
    public delegate ref S FastRef<T, S>(T source = default);
    public delegate void FastSetter<T, S>(T source, S value);
    public delegate S FastGetter<T, S>(T source);
    public delegate object FastInvoke(object target, params object[] paramters);
}