using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;

namespace TabletopTweaks.Extensions {
    static class ExtentionMethods {
        public static IEnumerable<GameAction> FlattenAllActions(this BlueprintScriptableObject blueprint) {
            List<GameAction> actions = new List<GameAction>();
            foreach (var component in blueprint.ComponentsArray) {
                Type type = component.GetType();
                var foundActions = AccessTools.GetDeclaredFields(type)
                    .Where(f => f.FieldType == typeof(ActionList))
                    .SelectMany(field => ((ActionList)field.GetValue(component)).Actions);
                actions.AddRange(FlattenAllActions(foundActions));
            }
            return actions;
        }
        public static IEnumerable<GameAction> FlattenAllActions(this IEnumerable<GameAction> actions) {
            List<GameAction> newActions = new List<GameAction>();
            foreach (var action in actions) {
                Type type = action?.GetType();
                var foundActions = AccessTools.GetDeclaredFields(type)?
                    .Where(f => f?.FieldType == typeof(ActionList))
                    .SelectMany(field => ((ActionList)field.GetValue(action)).Actions);
                if (foundActions != null) { newActions.AddRange(foundActions); }
            }
            if (newActions.Count > 0) {
                return actions.Concat(FlattenAllActions(newActions));
            }
            return actions;
        }
        public static IEnumerable<BlueprintAbility> AbilityAndVariants(this BlueprintAbility ability) {
            var List = new List<BlueprintAbility>() { ability };
            var varriants = ability.GetComponent<AbilityVariants>();
            if (varriants != null) {
                List.AddRange(varriants.Variants);
            }
            return List;
        }
        public static V PutIfAbsent<K, V>(this IDictionary<K, V> self, K key, V value) where V : class {
            V oldValue;
            if (!self.TryGetValue(key, out oldValue)) {
                self.Add(key, value);
                return value;
            }
            return oldValue;
        }

        public static V PutIfAbsent<K, V>(this IDictionary<K, V> self, K key, Func<V> ifAbsent) where V : class {
            V value;
            if (!self.TryGetValue(key, out value)) {
                self.Add(key, value = ifAbsent());
                return value;
            }
            return value;
        }

        public static T[] AppendToArray<T>(this T[] array, T value) {
            var len = array.Length;
            var result = new T[len + 1];
            Array.Copy(array, result, len);
            result[len] = value;
            return result;
        }

        public static T[] RemoveFromArrayByType<T, V>(this T[] array) {
            List<T> list = new List<T>();

            foreach (var c in array) {
                if (!(c is V)) {
                    list.Add(c);
                }
            }

            return list.ToArray();
        }

        public static T[] AppendToArray<T>(this T[] array, params T[] values) {
            var len = array.Length;
            var valueLen = values.Length;
            var result = new T[len + valueLen];
            Array.Copy(array, result, len);
            Array.Copy(values, 0, result, len, valueLen);
            return result;
        }

        public static T[] AppendToArray<T>(this T[] array, IEnumerable<T> values) => AppendToArray(array, values.ToArray());

        public static T[] InsertBeforeElement<T>(this T[] array, T value, T element) {
            var len = array.Length;
            var result = new T[len + 1];
            int x = 0;
            bool added = false;
            for (int i = 0; i < len; i++) {
                if (array[i].Equals(element) && !added) {
                    result[x++] = value;
                    added = true;
                }
                result[x++] = array[i];
            }
            return result;
        }

        public static T[] InsertAfterElement<T>(this T[] array, T value, T element) {
            var len = array.Length;
            var result = new T[len + 1];
            int x = 0;
            bool added = false;
            for (int i = 0; i < len; i++) {
                if (array[i].Equals(element) && !added) {
                    result[x++] = array[i];
                    result[x++] = value;
                    added = true;
                } else {
                    result[x++] = array[i];
                }

            }
            return result;
        }

        public static T[] RemoveFromArray<T>(this T[] array, T value) {
            var list = array.ToList();
            return list.Remove(value) ? list.ToArray() : array;
        }

        public static string StringJoin<T>(this IEnumerable<T> array, Func<T, string> map, string separator = " ") => string.Join(separator, array.Select(map));

        public static void AddClass(this BlueprintProgression progression, BlueprintCharacterClass characterClass) {
            if (progression.m_Classes.Any(a => a.m_Class.Get() == characterClass)) { return; }
            progression.m_Classes = progression.m_Classes.AppendToArray(
                new BlueprintProgression.ClassWithLevel() {
                    m_Class = characterClass.ToReference<BlueprintCharacterClassReference>(),
                });
        }

        public static void AddArchetype(this BlueprintProgression progression, BlueprintArchetype archetype) {
            if (progression.m_Archetypes.Any(a => a.m_Archetype.Get() == archetype)) { return; }
            progression.m_Archetypes = progression.m_Archetypes.AppendToArray(
                new BlueprintProgression.ArchetypeWithLevel() {
                    m_Archetype = archetype.ToReference<BlueprintArchetypeReference>(),
                });
        }

        public static void SetFeatures(this BlueprintFeatureSelection selection, IEnumerable<BlueprintFeature> features) {
            SetFeatures(selection, features.ToArray());
        }

        public static void SetFeatures(this BlueprintFeatureSelection selection, params BlueprintFeature[] features) {
            selection.m_AllFeatures = selection.m_Features = features.Select(bp => bp.ToReference<BlueprintFeatureReference>()).ToArray();
        }

        public static void RemoveFeatures(this BlueprintFeatureSelection selection, params BlueprintFeature[] features) {
            foreach (var feature in features) {
                var featureReference = feature.ToReference<BlueprintFeatureReference>();
                if (selection.m_AllFeatures.Contains(featureReference)) {
                    selection.m_AllFeatures = selection.m_AllFeatures.Where(f => !f.Equals(featureReference)).ToArray();
                }
                if (selection.m_Features.Contains(featureReference)) {
                    selection.m_Features = selection.m_Features.Where(f => !f.Equals(featureReference)).ToArray();
                }
            }
            selection.m_AllFeatures = selection.m_AllFeatures.OrderBy(feature => feature.Get().Name).ToArray();
            selection.m_Features = selection.m_Features.OrderBy(feature => feature.Get().Name).ToArray();
        }

        public static void AddFeatures(this BlueprintFeatureSelection selection, params BlueprintFeature[] features) {
            foreach (var feature in features) {
                var featureReference = feature.ToReference<BlueprintFeatureReference>();
                if (!selection.m_AllFeatures.Contains(featureReference)) {
                    selection.m_AllFeatures = selection.m_AllFeatures.AppendToArray(featureReference);
                }
                if (!selection.m_Features.Contains(featureReference)) {
                    selection.m_Features = selection.m_Features.AppendToArray(featureReference);
                }
            }
            selection.m_AllFeatures = selection.m_AllFeatures.OrderBy(feature => feature.Get().Name).ToArray();
            selection.m_Features = selection.m_Features.OrderBy(feature => feature.Get().Name).ToArray();
        }
        public static void AddPrerequisiteFeature(this BlueprintFeature obj, BlueprintFeature feature) {
            obj.AddPrerequisiteFeature(feature, GroupType.All);
        }
        public static void AddPrerequisiteFeature(this BlueprintFeature obj, BlueprintFeature feature, GroupType group) {
            obj.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                c.m_Feature = feature.ToReference<BlueprintFeatureReference>();
                c.Group = group;
            }));
            if (feature.IsPrerequisiteFor == null) { feature.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
            if (!feature.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                feature.IsPrerequisiteFor.Add(obj.ToReference<BlueprintFeatureReference>());
            }
        }
        public static void AddPrerequisiteFeaturesFromList(this BlueprintFeature obj, int amount, params BlueprintFeature[] features) {
            obj.AddPrerequisiteFeaturesFromList(amount, GroupType.All, features);
        }
        public static void AddPrerequisiteFeaturesFromList(this BlueprintFeature obj, int amount, GroupType group = GroupType.All, params BlueprintFeature[] features) {
            obj.AddComponent(Helpers.Create<PrerequisiteFeaturesFromList>(c => {
                c.m_Features = features.Select(f => f.ToReference<BlueprintFeatureReference>()).ToArray();
                c.Amount = amount;
                c.Group = group;
            }));
            features.ForEach(feature => {
                if (feature.IsPrerequisiteFor == null) { feature.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                if (!feature.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                    feature.IsPrerequisiteFor.Add(obj.ToReference<BlueprintFeatureReference>());
                }
            });
        }

        public static void AddPrerequisite<T>(this BlueprintFeature obj, Action<T> init = null) where T : Prerequisite, new() {
            obj.AddPrerequisite(Helpers.Create(init));
        }

        public static void AddPrerequisite<T>(this BlueprintFeature obj, T prerequisite) where T : Prerequisite {
            obj.AddComponent(prerequisite);
            switch (prerequisite) {
                case PrerequisiteFeature p:
                    var feature = p.Feature;
                    if (feature.IsPrerequisiteFor == null) { feature.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                    if (!feature.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                        feature.IsPrerequisiteFor.Add(obj.ToReference<BlueprintFeatureReference>());
                    }
                    break;
                case PrerequisiteFeaturesFromList p:
                    var features = p.Features;
                    features.ForEach(f => {
                        if (f.IsPrerequisiteFor == null) { f.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                        if (!f.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                            f.IsPrerequisiteFor.Add(obj.ToReference<BlueprintFeatureReference>());
                        }
                    });
                    break;
                default:
                    break;
            }
        }

        public static void AddPrerequisites<T>(this BlueprintFeature obj, params T[] prerequisites) where T : Prerequisite {
            foreach (var prerequisite in prerequisites) {
                obj.AddPrerequisite(prerequisite);
            }
        }

        public static void RemovePrerequisite<T>(this BlueprintFeature obj, T prerequisite) where T : Prerequisite {
            obj.RemoveComponent(prerequisite);
            switch (prerequisite) {
                case PrerequisiteFeature p:
                    var feature = p.Feature;
                    if (feature.IsPrerequisiteFor == null) { feature.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                    break;
                case PrerequisiteFeaturesFromList p:
                    var features = p.Features;
                    features.ForEach(f => {
                        if (f.IsPrerequisiteFor == null) { f.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                        f.IsPrerequisiteFor.RemoveAll(v => v.Guid == obj.AssetGuid);
                    });
                    break;
                default:
                    break;
            }
        }

        public static void RemovePrerequisites<T>(this BlueprintFeature obj, params T[] prerequisites) where T : Prerequisite {
            foreach (var prerequisite in prerequisites) {
                obj.RemovePrerequisite(prerequisite);
            }
        }

        public static void RemovePrerequisites<T>(this BlueprintFeature obj, Predicate<T> predicate) where T : Prerequisite {
            foreach (var prerequisite in obj.GetComponents<T>()) {
                if (predicate(prerequisite)) {
                    obj.RemovePrerequisite(prerequisite);
                }
            }
        }

        public static void RemovePrerequisites<T>(this BlueprintFeature obj) where T : Prerequisite {
            foreach (var prerequisite in obj.GetComponents<T>()) {
                obj.RemovePrerequisite(prerequisite);
            }
        }

        public static void InsertComponent(this BlueprintScriptableObject obj, int index, BlueprintComponent component) {
            var components = obj.ComponentsArray.ToList();
            components.Insert(index, component);
            obj.SetComponents(components);
        }

        public static void AddContextRankConfig(this BlueprintScriptableObject obj, Action<ContextRankConfig> init = null) {
            obj.AddComponent(Helpers.CreateContextRankConfig(init));
        }

        public static void AddComponent(this BlueprintScriptableObject obj, BlueprintComponent component) {
            obj.SetComponents(obj.ComponentsArray.AppendToArray(component));
        }

        public static void AddComponent<T>(this BlueprintScriptableObject obj, Action<T> init = null) where T : BlueprintComponent, new() {
            obj.SetComponents(obj.ComponentsArray.AppendToArray(Helpers.Create(init)));
        }

        public static void RemoveComponent(this BlueprintScriptableObject obj, BlueprintComponent component) {
            obj.SetComponents(obj.ComponentsArray.RemoveFromArray(component));
        }

        public static void RemoveComponents<T>(this BlueprintScriptableObject obj) where T : BlueprintComponent {
            var compnents_to_remove = obj.GetComponents<T>().ToArray();
            foreach (var c in compnents_to_remove) {
                obj.SetComponents(obj.ComponentsArray.RemoveFromArray(c));
            }
        }

        public static void RemoveComponents<T>(this BlueprintScriptableObject obj, Predicate<T> predicate) where T : BlueprintComponent {
            var compnents_to_remove = obj.GetComponents<T>().ToArray();
            foreach (var c in compnents_to_remove) {
                if (predicate(c)) {
                    obj.SetComponents(obj.ComponentsArray.RemoveFromArray(c));
                }
            }
        }

        public static void AddComponents(this BlueprintScriptableObject obj, IEnumerable<BlueprintComponent> components) => AddComponents(obj, components.ToArray());

        public static void AddComponents(this BlueprintScriptableObject obj, params BlueprintComponent[] components) {
            var c = obj.ComponentsArray.ToList();
            c.AddRange(components);
            obj.SetComponents(c.ToArray());
        }

        public static void SetComponents(this BlueprintScriptableObject obj, params BlueprintComponent[] components) {
            // Fix names of components. Generally this doesn't matter, but if they have serialization state,
            // then their name needs to be unique.
            var names = new HashSet<string>();
            foreach (var c in components) {
                if (string.IsNullOrEmpty(c.name)) {
                    c.name = $"${c.GetType().Name}";
                }
                if (!names.Add(c.name)) {
                    String name;
                    for (int i = 0; !names.Add(name = $"{c.name}${i}"); i++) ;
                    c.name = name;
                }
            }
            obj.ComponentsArray = components;
            obj.OnEnable(); // To make sure components are fully initialized
        }

        public static void SetComponents(this BlueprintScriptableObject obj, IEnumerable<BlueprintComponent> components) {
            SetComponents(obj, components.ToArray());
        }

        public static T CreateCopy<T>(this T original, Action<T> action = null) where T : UnityEngine.Object {
            var clone = UnityEngine.Object.Instantiate(original);
            if (action != null) {
                action(clone);
            }
            return clone;
        }

        public static void SetName(this BlueprintArchetype archetype, string description) {
            archetype.LocalizedName = Helpers.CreateString($"{archetype.name}.Name", description, shouldProcess: false);
        }

        public static void SetDescription(this BlueprintArchetype archetype, string description) {
            archetype.LocalizedDescription = Helpers.CreateString($"{archetype.name}.Description", description, shouldProcess: true);
        }

        public static void SetName(this BlueprintItem item, string description) {
            item.m_DisplayNameText = Helpers.CreateString($"{item.name}.Name", description, shouldProcess: false);
        }

        public static void SetDescription(this BlueprintItem item, string description) {
            item.m_DescriptionText = Helpers.CreateString($"{item.name}.Description", description, shouldProcess: true);
        }

        public static void UpdatePrefixSuffix(this BlueprintItemEnchantment enchantment, string prefix, string sufix, Locale targetLanguage = Locale.enGB) {
            if (LocalizationManager.CurrentLocale != targetLanguage) { return; }
            enchantment.SetPrefix(prefix);
            enchantment.SetSuffix(sufix);
        }

        public static void SetName(this BlueprintItemEnchantment enchantment, string name) {
            enchantment.m_EnchantName = Helpers.CreateString($"{enchantment.name}.Name", name);
        }

        public static void SetDescription(this BlueprintItemEnchantment enchantment, string description) {
            enchantment.m_Description = Helpers.CreateString($"{enchantment.name}.Description", description, shouldProcess: true);
        }

        public static void SetPrefix(this BlueprintItemEnchantment enchantment, string prefix) {
            enchantment.m_Prefix = Helpers.CreateString($"{enchantment.name}.Prefix", prefix);
        }

        public static void SetSuffix(this BlueprintItemEnchantment enchantment, string sufix) {
            enchantment.m_Suffix = Helpers.CreateString($"{enchantment.name}.Suffix", sufix);
        }

        public static void SetNameDescription(this BlueprintUnitFact feature, string displayName, string description) {
            feature.SetName(displayName);
            feature.SetDescription(description);
        }

        public static void SetNameDescription(this BlueprintUnitFact feature, BlueprintUnitFact other) {
            feature.m_DisplayName = other.m_DisplayName;
            feature.m_Description = other.m_Description;
        }

        public static void SetName(this BlueprintUnitFact feature, LocalizedString name) {
            feature.m_DisplayName = name;
        }

        public static void SetName(this BlueprintUnitFact feature, string name) {
            feature.m_DisplayName = Helpers.CreateString($"{feature.name}.Name", name);
        }

        public static void SetDescription(this BlueprintUnitFact feature, LocalizedString description) {
            feature.m_Description = description;
            //blueprintUnitFact_set_Description(feature) = description;
        }

        public static void SetDescription(this BlueprintUnitFact feature, string description) {
            feature.m_Description = Helpers.CreateString($"{feature.name}.Description", description, shouldProcess: true);

        }

        public static bool HasFeatureWithId(this LevelEntry level, String id) {
            return level.Features.Any(f => HasFeatureWithId(f, id));
        }

        public static bool HasFeatureWithId(this BlueprintUnitFact fact, String id) {
            if (fact.AssetGuid == id) return true;
            foreach (var c in fact.ComponentsArray) {
                var addFacts = c as AddFacts;
                if (addFacts != null) return addFacts.Facts.Any(f => HasFeatureWithId(f, id));
            }
            return false;
        }

        public static void FixDomainSpell(this BlueprintAbility spell, int level, string spellListId) {
            var spellList = Resources.GetBlueprint<BlueprintSpellList>(spellListId);
            var spells = spellList.SpellsByLevel.First(s => s.SpellLevel == level).Spells;
            spells.Clear();
            spells.Add(spell);
        }


        public static bool HasAreaEffect(this BlueprintAbility spell) {
            return spell.AoERadius.Meters > 0f || spell.ProjectileType != AbilityProjectileType.Simple;
        }

        internal static IEnumerable<BlueprintComponent> WithoutSpellComponents(this IEnumerable<BlueprintComponent> components) {
            return components.Where(c => !(c is SpellComponent) && !(c is SpellListComponent));
        }

        internal static int GetCost(this BlueprintAbility.MaterialComponentData material) {
            var item = material?.Item;
            return item == null ? 0 : item.Cost * material.Count;
        }

        public static AddConditionImmunity CreateImmunity(this UnitCondition condition) {
            var b = new AddConditionImmunity() {
                Condition = condition
            };
            return b;
        }

        public static AddCondition CreateAddCondition(this UnitCondition condition) {
            var a = new AddCondition() {
                Condition = condition
            };
            return a;
        }

        public static BuffDescriptorImmunity CreateBuffImmunity(this SpellDescriptor spell) {
            var b = new BuffDescriptorImmunity() {
                Descriptor = spell
            };
            return b;
        }

        public static SpellImmunityToSpellDescriptor CreateSpellImmunity(this SpellDescriptor spell) {
            var s = new SpellImmunityToSpellDescriptor() {
                Descriptor = spell
            };
            return s;
        }

        public static void AddAction(this Kingmaker.UnitLogic.Abilities.Components.AbilityEffectRunAction action, Kingmaker.ElementsSystem.GameAction game_action) {
            if (action.Actions != null) {
                action.Actions = Helpers.CreateActionList(action.Actions.Actions);
                action.Actions.Actions = action.Actions.Actions.AppendToArray(game_action);
            } else {
                action.Actions = Helpers.CreateActionList(game_action);
            }
        }

        public static void ReplaceComponents(this BlueprintScriptableObject blueprint, Predicate<BlueprintComponent> predicate, BlueprintComponent newComponent) {
            bool found = false;
            foreach (var component in blueprint.ComponentsArray) {
                if (predicate(component)) {
                    blueprint.SetComponents(blueprint.ComponentsArray.RemoveFromArray(component));
                    found = true;
                }
            }
            if (found) {
                blueprint.AddComponent(newComponent);
            }
        }

        public static void ReplaceComponents<T>(this BlueprintScriptableObject blueprint, BlueprintComponent newComponent) where T : BlueprintComponent {
            blueprint.ReplaceComponents<T>(c => true, newComponent);
        }

        public static void ReplaceComponents<T>(this BlueprintScriptableObject blueprint, Predicate<T> predicate, BlueprintComponent newComponent) where T : BlueprintComponent {
            var components = blueprint.GetComponents<T>().ToArray();
            bool found = false;
            foreach (var c in components) {
                if (predicate(c)) {
                    blueprint.SetComponents(blueprint.ComponentsArray.RemoveFromArray(c));
                    found = true;
                }
            }
            if (found) {
                blueprint.AddComponent(newComponent);
            }
        }
    }
}
