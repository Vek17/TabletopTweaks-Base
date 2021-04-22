using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks {
    class Resources {
        private static IEnumerable<BlueprintScriptableObject> blueprints;

        public static IEnumerable<T> GetBlueprints<T>() where T : BlueprintScriptableObject {
            if (blueprints == null) {
                var bundle = ResourcesLibrary.s_BlueprintsBundle;
                blueprints = bundle.LoadAllAssets<BlueprintScriptableObject>();
                //blueprints = Kingmaker.Cheats.Utilities.GetScriptableObjects<BlueprintScriptableObject>();
            }
            return blueprints.Concat(ResourcesLibrary.s_LoadedBlueprints.Values).OfType<T>().Distinct();
        }
        public static void AddBlueprint([NotNull] BlueprintScriptableObject blueprint) {
            AddBlueprint(blueprint, blueprint.AssetGuid);
        }
        public static void AddBlueprint([NotNull] BlueprintScriptableObject blueprint, string assetId) {
            blueprint.m_AssetGuid = assetId;
            var loadedBlueprint = ResourcesLibrary.TryGetBlueprint<BlueprintScriptableObject>(assetId);
            if (loadedBlueprint == null) {
                ResourcesLibrary.s_LoadedBlueprints[assetId] = blueprint;
                Main.LogPatch("Added", blueprint);
                if (blueprint != null) {
                    blueprint.OnEnableWithLibrary();
                }
            } else {
                Main.Log($"Asset ID: {assetId} already in use by: {loadedBlueprint.name}");
            }
        }
    }
}