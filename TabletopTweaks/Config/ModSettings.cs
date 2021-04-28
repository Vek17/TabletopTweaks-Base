using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Config {
    static class ModSettings {
        public static ModEntry ModEntry;
        private static Fixes fixes;
        public static Fixes Fixes {
            get {
                if (fixes == null) {
                    LoadAllSettings();
                }
                return fixes;
            }
        }
        private static AddedContent addedContent;
        public static AddedContent AddedContent {
            get {
                if (addedContent == null) {
                    LoadAllSettings();
                }
                return addedContent;
            }
        }
        private static Blueprints blueprints;
        public static Blueprints Blueprints {
            get {
                if (addedContent == null) {
                    LoadAllSettings();
                }
                return blueprints;
            }
        }

        public static void LoadAllSettings() {
            LoadSettings("Fixes.json", ref fixes);
            LoadSettings("AddedContent.json", ref addedContent);
            LoadSettings("Blueprints.json", ref blueprints);
        }
        private static void LoadSettings<T>(string fileName, ref T setting) where T : IUpdatableSettings {
            var assembly = Assembly.GetExecutingAssembly();
            string userConfigFolder = ModEntry.Path + "UserSettings";
            Directory.CreateDirectory(userConfigFolder);
            var resourcePath = $"TabletopTweaks.Config.{fileName}";
            var userPath = $"{userConfigFolder}{Path.DirectorySeparatorChar}{fileName}";

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream)) {
                setting = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            if (File.Exists(userPath)) {
                using (StreamReader reader = File.OpenText(userPath)) {
                    try {
                        T userSettings = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                        setting.OverrideSettings(userSettings);
                    } catch {
                        Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_{fileName}", true); } catch { Main.Error("Failed to archive broken settings."); }
                    }
                }
            }
            File.WriteAllText(userPath, JsonConvert.SerializeObject(setting, Formatting.Indented));
        }
    }
}
