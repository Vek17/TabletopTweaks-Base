using Kingmaker.Blueprints;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks {
    static class Resources {
        public static ModEntry ModEntry;
        private static Settings settings;
        public static Settings Settings {
            get {
                if (settings == null) {
                    settings = ModSettings.Load<Settings>(ModEntry);
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
            using (StreamReader streamReader = File.OpenText(ModEntry.Path + "settings.json")) {
                JObject groups = JObject.Parse(streamReader.ReadToEnd());
                Settings.DisableNaturalArmorStacking = groups["disableNaturalArmorStacking"].Value<bool>();
                Settings.DisablePolymorphStacking = groups["disablePolymorphStacking"].Value<bool>();
                Settings.FixDemonSubtypes = groups["fixDemonSubtypes"].Value<bool>();
                Settings.FixBloodlines = groups["fixBloodlines"].Value<bool>();
                Settings.FixSpells = groups["fixSpells"].Value<bool>();
                Settings.FixBuffs = groups["fixBuffs"].Value<bool>();
                Settings.FixAeon = groups["fixAeon"].Value<bool>();
                Settings.FixAzata = groups["fixAzata"].Value<bool>();
                Settings.FixSlayer = groups["fixSlayer"].Value<bool>();
                Settings.FixWitch = groups["fixWitch"].Value<bool>();
            }
        }
    }
}
