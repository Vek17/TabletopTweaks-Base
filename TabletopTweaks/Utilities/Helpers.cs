// Copyright (c) 2019 Jennifer Messerly
// This code is licensed under MIT license (see LICENSE for details)

using Kingmaker.Blueprints;
using Kingmaker.Localization;
using System.Collections.Generic;
using System;
using UnityEngine;
using Kingmaker.ElementsSystem;
using System.Globalization;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints.Classes;
using System.Linq;
using Kingmaker.Utility;

namespace TabletopTweaks.Utilities {
    public static class Helpers {
        public static class GuidStorage {
            static Dictionary<string, string> guids_in_use = new Dictionary<string, string>();
            static bool allow_guid_generation;

            static public void load(string file_content) {
                load(file_content, false);
            }

            static public void load(string file_content, bool debug_mode) {
                allow_guid_generation = debug_mode;
                guids_in_use = new Dictionary<string, string>();
                using (System.IO.StringReader reader = new System.IO.StringReader(file_content)) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        string[] items = line.Split('\t');
                        guids_in_use.Add(items[0], items[1]);
                    }
                }
            }

            static public void dump(string guid_file_name) {
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(guid_file_name)) {
                    foreach (var pair in guids_in_use) {
                        BlueprintScriptableObject existing;
                        existing = ResourcesLibrary.TryGetBlueprint(pair.Value);
                        if (existing != null) {
                            sw.WriteLine(pair.Key + '\t' + pair.Value + '\t' + existing.GetType().FullName);
                        }
                    }
                }
            }

            static public void addEntry(string name, string guid) {
                string original_guid;
                if (guids_in_use.TryGetValue(name, out original_guid)) {
                    if (original_guid != guid) {
                        throw Main.Error($"Asset: {name}, is already registered for object with another guid: {guid}");
                    }
                }
                else {
                    guids_in_use.Add(name, guid);
                }
            }


            static public bool hasStoredGuid(string blueprint_name) {
                string stored_guid = "";
                return guids_in_use.TryGetValue(blueprint_name, out stored_guid);
            }


            static public string getGuid(string name) {
                string original_guid;
                if (guids_in_use.TryGetValue(name, out original_guid)) {
                    return original_guid;
                }
                else if (allow_guid_generation) {
                    return Guid.NewGuid().ToString("N");
                }
                else {
                    throw Main.Error($"Missing AssetId for: {name}"); //ensure that no guids generated in release mode
                }
            }


            static public string maybeGetGuid(string name, string new_guid = "") {
                string original_guid;
                if (guids_in_use.TryGetValue(name, out original_guid)) {
                    return original_guid;
                }
                else {
                    return new_guid;
                }
            }

        }

        public static T Create<T>(Action<T> init = null) where T : ScriptableObject {
            var result = ScriptableObject.CreateInstance<T>();
            if (init != null) init(result);
            return result;
        }
        public static LevelEntry LevelEntry(int level, BlueprintFeatureBase feature) {
            return new LevelEntry {
                Level = level,
                Features =
                {
                    feature
                }
            };
        }

        // Token: 0x060005A7 RID: 1447 RVA: 0x0008E01E File Offset: 0x0008C21E
        public static LevelEntry LevelEntry(int level, params BlueprintFeatureBase[] features) {
            return LevelEntry(level, features);
        }

        // Token: 0x060005A8 RID: 1448 RVA: 0x0008E038 File Offset: 0x0008C238
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
        public static ActionList CreateActionList(params GameAction[] actions) {
            if (actions == null || actions.Length == 1 && actions[0] == null) actions = Array.Empty<GameAction>();
            return new ActionList() { Actions = actions };
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
                Main.Log($"Info: duplicate localized string `{key}`, different text.");
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

    }
    public delegate ref S FastRef<T, S>(T source = default);
    public delegate void FastSetter<T, S>(T source, S value);
    public delegate S FastGetter<T, S>(T source);
    public delegate object FastInvoke(object target, params object[] paramters);
}