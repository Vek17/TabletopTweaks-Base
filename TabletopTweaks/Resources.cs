using HarmonyLib;
using Kingmaker.Blueprints;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks {
    static class Resources {
        public static ModEntry Mod;
        private static Settings settings;
        public static Settings Settings {
            get {
                if (settings == null) {
                    settings = ModSettings.Load<Settings>(Mod);
                }
                return settings;
            }
        }
        private static BlueprintScriptableObject[] blueprints = null;
        public static BlueprintScriptableObject[] Blueprints {
            get {
                if (blueprints == null) {
                    blueprints = GetBlueprints();
                }
                return blueprints;
            }
        }

        private static BlueprintScriptableObject[] GetBlueprints() {
            var bundle = (AssetBundle)AccessTools.Field(typeof(ResourcesLibrary), "s_BlueprintsBundle")
                .GetValue(null);
            return bundle.LoadAllAssets<BlueprintScriptableObject>();
        }

        public static void LoadSettings() {
            using (StreamReader streamReader = File.OpenText(UnityModManager.modsPath + "/TabletopTweaks/settings.json")) {
                JObject groups = JObject.Parse(streamReader.ReadToEnd());
                Settings.DisableNaturalArmorStacking = groups["DisableNaturalArmorStacking"].Value<bool>();
            }
        }
    }
}
