using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TabletopTweaks.Extensions;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Config {
    static class Settings {
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
        private static AddedContent addedContent;
        public static AddedContent AddedContent {
            get {
                if (addedContent == null) {
                    LoadSettings();
                }
                return addedContent;
            }
        }
        private static Blueprints blueprints;
        public static Blueprints Blueprints {
            get {
                if (addedContent == null) {
                    LoadSettings();
                }
                return blueprints;
            }
        }


        public static void LoadSettings() {
            var assembly = Assembly.GetExecutingAssembly();
            string userConfigFolder = ModEntry.Path + "UserSettings";
            
            Directory.CreateDirectory(userConfigFolder);

            var fixesResource = "TabletopTweaks.Config.Fixes.json";
            string userFixPath = userConfigFolder + $"{Path.DirectorySeparatorChar}Fixes.json";
            using (Stream stream = assembly.GetManifestResourceStream(fixesResource))
            using (StreamReader reader = new StreamReader(stream)) {
                fixes = JsonConvert.DeserializeObject<Fixes>(reader.ReadToEnd());
            }
            if (File.Exists(userFixPath)) {
                using (StreamReader reader = File.OpenText(userFixPath)) {
                    try {
                        Fixes userFixes = JsonConvert.DeserializeObject<Fixes>(reader.ReadToEnd());
                        fixes.OverrideFixes(userFixes);
                    }
                    catch {
                        Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userFixPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_Fixes.json", true); } catch { Main.Error("Failed to archive broken settings."); }
                    }
                }
            }
            File.WriteAllText(userFixPath, JsonConvert.SerializeObject(fixes, Formatting.Indented));

            var addedContentResource = "TabletopTweaks.Config.AddedContent.json";
            string userAddedContentPath = userConfigFolder + $"{Path.DirectorySeparatorChar}AddedContent.json";
            using (Stream stream = assembly.GetManifestResourceStream(addedContentResource))
            using (StreamReader reader = new StreamReader(stream)) {
                addedContent = JsonConvert.DeserializeObject<AddedContent>(reader.ReadToEnd());
            }
            if (File.Exists(userAddedContentPath)) {
                using (StreamReader reader = File.OpenText(userAddedContentPath)) {
                    try {
                        AddedContent userAddedContent = JsonConvert.DeserializeObject<AddedContent>(reader.ReadToEnd());
                        addedContent.OverrideFixes(userAddedContent);
                    }
                    catch {
                        Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userAddedContentPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_AddedContent.json", true); } catch { Main.Error("Failed to archive broken settings."); }
                    }
                }
            }
            File.WriteAllText(userAddedContentPath, JsonConvert.SerializeObject(addedContent, Formatting.Indented));

            var blueprintsResource = "TabletopTweaks.Config.Blueprints.json";
            string blueprintsPath = userConfigFolder + $"{Path.DirectorySeparatorChar}Blueprints.json";
            using (Stream stream = assembly.GetManifestResourceStream(blueprintsResource))
            using (StreamReader reader = new StreamReader(stream)) {
                blueprints = JsonConvert.DeserializeObject<Blueprints>(reader.ReadToEnd());
            }
            if (File.Exists(blueprintsPath)) {
                using (StreamReader reader = File.OpenText(blueprintsPath)) {
                    try {
                        Blueprints userBlueprints = JsonConvert.DeserializeObject<Blueprints>(reader.ReadToEnd());
                        blueprints.OverrideSettings(userBlueprints);
                    }
                    catch {
                        Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(blueprintsPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_Blueprints.json", true); } catch { Main.Error("Failed to archive broken settings."); }
                    }
                }
            }
            File.WriteAllText(blueprintsPath, JsonConvert.SerializeObject(blueprints, Formatting.Indented));
        }
    }
}
