using Kingmaker.Blueprints;
using Newtonsoft.Json.Linq;
using System;
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
                Settings.DisableNaturalArmorStacking = groups["DisableNaturalArmorStacking"].Value<bool>();
                Settings.DisablePolymorphStacking = groups["DisablePolymorphStacking"].Value<bool>();
                Settings.DisableAllSpellFixes = groups["DisableAllSpellFixes"].Value<bool>();
                Settings.SpellFixes = groups["SpellFixes"].Value<JObject>()
                    .Properties()
                    .ToDictionary(
                        k => k.Name,
                        v => v.Value.Value<bool>()
                );
                Settings.FixDemonSubtypes = groups["FixDemonSubtypes"].Value<bool>();
                Settings.FixBloodlines = groups["FixBloodlines"].Value<bool>();
                Settings.FixAeon = groups["FixAeon"].Value<bool>();
                Settings.FixAzata = groups["FixAzata"].Value<bool>();
                Settings.FixSlayer = groups["FixSlayer"].Value<bool>();
                Settings.FixWitch = groups["FixWitch"].Value<bool>();
            }
        }

        private static int Dictionary<T1, T2>() {
            throw new NotImplementedException();
        }
    }
}
