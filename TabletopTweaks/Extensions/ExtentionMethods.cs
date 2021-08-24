using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Utilities;
using UnityEngine;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;

namespace TabletopTweaks.Extensions {
    static class ExtentionMethods {
        public static IEnumerable<GameAction> FlattenAllActions(this BlueprintAbility Ability) {
            return
                Ability.GetComponents<AbilityExecuteActionOnCast>()
                    .SelectMany(a => a.FlattenAllActions())
                .Concat(
                Ability.GetComponents<AbilityEffectRunAction>()
                    .SelectMany(a => a.FlattenAllActions()));
        }
        public static IEnumerable<GameAction> FlattenAllActions(this AbilityExecuteActionOnCast Action) {
            return FlattenAllActions(Action.Actions.Actions);
        }
        public static IEnumerable<GameAction> FlattenAllActions(this AbilityEffectRunAction Action) {
            return FlattenAllActions(Action.Actions.Actions);
        }
        public static IEnumerable<GameAction> FlattenAllActions(this IEnumerable<GameAction> Actions) {
            List<GameAction> NewActions = new List<GameAction>();
            NewActions.AddRange(Actions.OfType<ContextActionOnRandomTargetsAround>().SelectMany(a => a.Actions.Actions));
            NewActions.AddRange(Actions.OfType<ContextActionConditionalSaved>().SelectMany(a => a.Failed.Actions));
            NewActions.AddRange(Actions.OfType<ContextActionConditionalSaved>().SelectMany(a => a.Succeed.Actions));
            NewActions.AddRange(Actions.OfType<Conditional>().SelectMany(a => a.IfFalse.Actions));
            NewActions.AddRange(Actions.OfType<Conditional>().SelectMany(a => a.IfTrue.Actions));
            if (NewActions.Count > 0) {
                return Actions.Concat(FlattenAllActions(NewActions));
            }
            return Actions;
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
            }
            selection.m_AllFeatures = selection.m_AllFeatures.OrderBy(feature => feature.Get().Name).ToArray();
        }

        public static void AddFeatures(this BlueprintFeatureSelection selection, params BlueprintFeature[] features) {
            foreach (var feature in features) {
                var featureReference = feature.ToReference<BlueprintFeatureReference>();
                if (!selection.m_AllFeatures.Contains(featureReference)) {
                    selection.m_AllFeatures = selection.m_AllFeatures.AppendToArray(featureReference);
                }
            }
            selection.m_AllFeatures = selection.m_AllFeatures.OrderBy(feature => feature.Get().Name).ToArray();
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

        public static void RemovePrerequisite<T>(this BlueprintFeature obj, T prerequisite) where T : Prerequisite {
            obj.RemoveComponent(prerequisite);
            switch (prerequisite) {
                case PrerequisiteFeature p:
                    var feature = p.Feature;
                    if (feature.IsPrerequisiteFor == null) { feature.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                    if (!feature.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                        feature.IsPrerequisiteFor.Remove(obj.ToReference<BlueprintFeatureReference>());
                    }
                    break;
                case PrerequisiteFeaturesFromList p:
                    var features = p.Features;
                    features.ForEach(f => {
                        if (f.IsPrerequisiteFor == null) { f.IsPrerequisiteFor = new List<BlueprintFeatureReference>(); }
                        if (!f.IsPrerequisiteFor.Contains(obj.ToReference<BlueprintFeatureReference>())) {
                            f.IsPrerequisiteFor.Remove(obj.ToReference<BlueprintFeatureReference>());
                        }
                    });
                    break;
                default:
                    break;
            }
        }

        public static void InsertComponent(this BlueprintScriptableObject obj, int index, BlueprintComponent component) {
            var components = obj.ComponentsArray.ToList();
            components.Insert(index, component);
            obj.SetComponents(components);
        }

        public static void AddComponent(this BlueprintScriptableObject obj, BlueprintComponent component) {
            obj.SetComponents(obj.ComponentsArray.AppendToArray(component));
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

        public static void SetNameDescription(this BlueprintUnitFact feature, String displayName, String description) {
            feature.SetName(Helpers.CreateString(feature.name + ".Name", displayName));
            feature.SetDescription(description);
        }

        public static void SetNameDescription(this BlueprintUnitFact feature, BlueprintUnitFact other) {
            feature.m_DisplayName = other.m_DisplayName;
            feature.m_Description = other.m_Description;
        }

        public static void SetName(this BlueprintUnitFact feature, LocalizedString name) {
            feature.m_DisplayName = name;
        }

        public static void SetName(this BlueprintUnitFact feature, String name) {
            feature.m_DisplayName = Helpers.CreateString(feature.name + ".Name", name);
        }

        public static void SetDescription(this BlueprintUnitFact feature, String description) {
            SetDescriptionTagged(feature, description);
        }

        public static void SetDescription(this BlueprintUnitFact feature, LocalizedString description) {
            feature.m_Description = description;
            //blueprintUnitFact_set_Description(feature) = description;
        }

        public static void SetDescriptionTagged(this BlueprintUnitFact feature, String description) {
            var taggedDescription = DescriptionTools.TagEncyclopediaEntries(description);
            SetDescriptionTagged(feature, description);
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

        static readonly FastRef<BlueprintArchetype, Sprite> blueprintArchetype_set_Icon = Helpers.CreateFieldSetter<BlueprintArchetype, Sprite>("m_Icon");

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

        public static void ReplaceComponent(this BlueprintScriptableObject blueprint, BlueprintComponent oldComponent, BlueprintComponent newComponent) {
            BlueprintComponent[] compnents_to_remove = blueprint.ComponentsArray;
            bool found = false;
            for (int i = 0; i < compnents_to_remove.Length; i++) {
                if (compnents_to_remove[i] == oldComponent) {
                    blueprint.RemoveComponent(oldComponent);
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
            var compnents_to_remove = blueprint.GetComponents<T>().ToArray();
            bool found = false;
            foreach (var c in compnents_to_remove) {
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
