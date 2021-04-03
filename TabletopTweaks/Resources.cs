using Kingmaker.Blueprints;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static IEnumerable<BlueprintScriptableObject> blueprints;

        public static IEnumerable<T> GetBlueprints<T>() where T : BlueprintScriptableObject {
            if (blueprints == null) {
                var bundle = ResourcesLibrary.s_BlueprintsBundle;
                blueprints = bundle.LoadAllAssets<BlueprintScriptableObject>();
            }
            return blueprints.OfType<T>();
        }

        public static void LoadSettings() {
            using (StreamReader streamReader = File.OpenText(Mod.Path + "settings.json")) {
                JObject groups = JObject.Parse(streamReader.ReadToEnd());
                Settings.DisableNaturalArmorStacking = groups["DisableNaturalArmorStacking"].Value<bool>();
                Settings.DisablePolymorphStacking = groups["DisablePolymorphStacking"].Value<bool>();
                Settings.FixDemonSubtypes = groups["FixDemonSubtypes"].Value<bool>();
                Settings.FixBloodlines = groups["FixBloodlines"].Value<bool>();
                Settings.FixSpells = groups["FixSpells"].Value<bool>();
                Settings.FixAeon = groups["FixAeon"].Value<bool>();
                Settings.FixAzata = groups["FixAzata"].Value<bool>();
                Settings.FixSlayer = groups["FixSlayer"].Value<bool>();
                Settings.FixAzata = groups["FixWitch"].Value<bool>();
            }
        }
    }
}
