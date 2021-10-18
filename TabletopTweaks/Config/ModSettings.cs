using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Config {
    static class ModSettings {
        public static ModEntry ModEntry;
        public static Fixes Fixes;
        public static AddedContent AddedContent;
        public static Homebrew Homebrew;
        public static Blueprints Blueprints;
        private static JsonSerializerSettings cachedSettings;
        private static JsonSerializerSettings SerializerSettings {
            get { 
                if (cachedSettings == null) { 
                    cachedSettings = new JsonSerializerSettings {
                        CheckAdditionalContent = false,
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        DefaultValueHandling = DefaultValueHandling.Include,
                        FloatParseHandling = FloatParseHandling.Double,
                        Formatting = Formatting.Indented,
                        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore,
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                        StringEscapeHandling = StringEscapeHandling.Default,
                    };
                } 
                return cachedSettings; 
            }
        }

        public static void LoadAllSettings() {
            LoadSettings("Fixes.json", ref Fixes);
            LoadSettings("AddedContent.json", ref AddedContent);
            LoadSettings("Homebrew.json", ref Homebrew);
            LoadSettings("Blueprints.json", ref Blueprints);
        }
        private static void LoadSettings<T>(string fileName, ref T setting) where T : IUpdatableSettings {
            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            var assembly = Assembly.GetExecutingAssembly();
            var userConfigFolder = ModEntry.Path + "UserSettings";
            var resourcePath = $"TabletopTweaks.Config.{fileName}";
            var userPath = $"{userConfigFolder}{Path.DirectorySeparatorChar}{fileName}";

            Directory.CreateDirectory(userConfigFolder);
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonReader jsonReader = new JsonTextReader(streamReader)) {
                setting = serializer.Deserialize<T>(jsonReader);
            }
            if (File.Exists(userPath)) {
                using (StreamReader streamReader = File.OpenText(userPath))
                using (JsonReader jsonReader = new JsonTextReader(streamReader)) {
                    try {
                        T userSettings = serializer.Deserialize<T>(jsonReader);
                        setting.OverrideSettings(userSettings);
                    } catch {
                        Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_{fileName}", true); } catch { Main.Error("Failed to archive broken settings."); }
                    }
                }
            }

            using (StreamWriter streamWriter = new StreamWriter(userPath))
            using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter)) {
                serializer.Serialize(jsonWriter, setting);
            }
        }
    }
}
