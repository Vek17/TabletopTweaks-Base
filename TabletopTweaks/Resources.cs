using Kingmaker.Blueprints;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TabletopTweaks.Config;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks {
    static class Resources {
        public static ModEntry ModEntry;
        private static Fixes fixes;
        public static Fixes Fixes {
            get {
                if (fixes == null) {
                    LoadSettings();
                }
                return fixes;
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
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TabletopTweaks.Fixes.json";
            string userConfigFolder = ModEntry.Path + "UserSettings";
            string userConfigPath = userConfigFolder + "\\Fixes.json";
            JsonSerializer serializer = new JsonSerializer();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream)) {
                fixes = JsonConvert.DeserializeObject<Fixes>(reader.ReadToEnd());
            }
            Directory.CreateDirectory(userConfigFolder);
            if (File.Exists(userConfigPath)) {
                using (StreamReader reader = File.OpenText(userConfigPath)) {
                    Fixes userFixes = JsonConvert.DeserializeObject<Fixes>(reader.ReadToEnd());
                    fixes.OverrideFixes(userFixes);
                }
            }
            File.WriteAllText(userConfigPath, JsonConvert.SerializeObject(fixes, Formatting.Indented));
        }
    }
}
