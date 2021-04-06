using Kingmaker.Blueprints;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static TabletopTweaks.Settings;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks {
    static class Resources {
        public static ModEntry ModEntry;
        private static Settings settings;
        public static Settings Settings {
            get {
                if (settings == null) {
                    settings = new Settings();
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
                /*
                Settings.DisableAllSpellFixes = groups["DisableAllSpellFixes"].Value<bool>();
                Settings.SpellFixes = groups["SpellFixes"].Value<JObject>()
                    .Properties()
                    .ToDictionary(
                        k => k.Name,
                        v => v.Value.Value<bool>()
                );
                */
                Settings.Azata = new FixGroup(groups["Azata"]);
                Settings.Aeon = new FixGroup(groups["Aeon"]);
                Settings.Witch = new FixGroup(groups["Witch"]);
                Settings.Slayer = new FixGroup(groups["Slayer"]);
                Settings.Bloodlines = new FixGroup(groups["Bloodlines"]);
                Settings.MythicAbilities = new FixGroup(groups["MythicAbilities"]);
                Settings.Spells = new FixGroup(groups["Spells"]);
                Settings.DragonDisciple = new FixGroup(groups["DragonDisciple"]);
                Settings.FixDemonSubtypes = groups["FixDemonSubtypes"].Value<bool>();
                /*
                Settings.DisableAllAzataFixes = groups["DisableAllAzataFixes"].Value<bool>();
                Settings.AzataFixes = groups["AzataFixes"].Value<JObject>()
                    .Properties()
                    .ToDictionary(
                        k => k.Name,
                        v => v.Value.Value<bool>()
                );
                */
            }
        }
    }
}
