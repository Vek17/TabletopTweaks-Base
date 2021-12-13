using JetBrains.Annotations;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using TabletopTweaks.Config;

namespace TabletopTweaks {
    static class Resources {
        public static readonly Dictionary<BlueprintGuid, SimpleBlueprint> ModBlueprints = new Dictionary<BlueprintGuid, SimpleBlueprint>();

#if false
        public static IEnumerable<T> GetBlueprints<T>() where T : BlueprintScriptableObject {
            if (blueprints == null) {
                var bundle = ResourcesLibrary.s_BlueprintsBundle;
                blueprints = bundle.LoadAllAssets<BlueprintScriptableObject>();
                //blueprints = Kingmaker.Cheats.Utilities.GetScriptableObjects<BlueprintScriptableObject>();
            }
            return blueprints.Concat(ResourcesLibrary.s_LoadedBlueprints.Values).OfType<T>().Distinct();
        }
#endif
        public static T GetModBlueprintReference<T>(string name) where T : BlueprintReferenceBase {
            BlueprintGuid? assetId = ModSettings.Blueprints.GetGUID(name);
            var reference = Activator.CreateInstance<T>();
            reference.deserializedGuid = assetId ?? BlueprintGuid.Empty;
            return reference;
        }
        public static T GetModBlueprint<T>(string name) where T : SimpleBlueprint {
            var assetId = ModSettings.Blueprints.GetGUID(name);
            ModBlueprints.TryGetValue(assetId, out var value);
            return value as T;
        }
        public static T GetBlueprintReference<T>(string id) where T : BlueprintReferenceBase {
            var assetId = BlueprintGuid.Parse(id);
            var reference = Activator.CreateInstance<T>();
            reference.deserializedGuid = assetId;
            return reference;
        }
        public static T GetBlueprint<T>(string id) where T : SimpleBlueprint {
            var assetId = BlueprintGuid.Parse(id);
            return GetBlueprint<T>(assetId);
        }
        public static T GetBlueprint<T>(BlueprintGuid id) where T : SimpleBlueprint {
            SimpleBlueprint asset = ResourcesLibrary.TryGetBlueprint(id);
            T value = asset as T;
            if (value == null) { Main.Error($"COULD NOT LOAD: {id} - {typeof(T)}"); }
            return value;
        }
        public static void AddBlueprint([NotNull] SimpleBlueprint blueprint) {
            AddBlueprint(blueprint, blueprint.AssetGuid);
        }
        public static void AddBlueprint([NotNull] SimpleBlueprint blueprint, string assetId) {
            var Id = BlueprintGuid.Parse(assetId);
            AddBlueprint(blueprint, Id);
        }
        public static void AddBlueprint([NotNull] SimpleBlueprint blueprint, BlueprintGuid assetId) {
            var loadedBlueprint = ResourcesLibrary.TryGetBlueprint(assetId);
            if (loadedBlueprint == null) {
                ModBlueprints[assetId] = blueprint;
                ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(assetId, blueprint);
                blueprint.OnEnable();
                Main.LogPatch("Added", blueprint);
            } else {
                Main.Log($"Failed to Add: {blueprint.name}");
                Main.Log($"Asset ID: {assetId} already in use by: {loadedBlueprint.name}");
            }
        }
    }
}