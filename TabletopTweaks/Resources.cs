using Kingmaker.Blueprints;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
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
        private static BlueprintScriptableObject[] blueprints;

        public static T[] GetBlueprints<T>() where T : BlueprintScriptableObject {
            if (blueprints == null) {
                var bundle = ResourcesLibrary.s_BlueprintsBundle;
                blueprints = bundle.LoadAllAssets<BlueprintScriptableObject>();
            }
            return blueprints.OfType<T>().ToArray();
        }

        public static void LoadSettings() {
            using (StreamReader streamReader = File.OpenText(UnityModManager.modsPath + "/TabletopTweaks/settings.json")) {
                JObject groups = JObject.Parse(streamReader.ReadToEnd());
                Settings.DisableNaturalArmorStacking = groups["DisableNaturalArmorStacking"].Value<bool>();
                Settings.DisablePolymorphStacking = groups["DisablePolymorphStacking"].Value<bool>();
                Settings.FixSlayerStudiedTarget = groups["FixSlayerStudiedTarget"].Value<bool>();
                Settings.FixDemonSubtypes = groups["FixDemonSubtypes"].Value<bool>();
                Settings.FixBloodlines = groups["FixBloodlines"].Value<bool>();
            }
        }
    }
}
