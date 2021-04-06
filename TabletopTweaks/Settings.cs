using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks {
    public class Settings {
        public bool DisableNaturalArmorStacking = true;
        public bool DisablePolymorphStacking = true;
        public SettingsGroup Azata;
        public SettingsGroup Spells;
        public bool DisableAllSpellFixes = true;
        public Dictionary<string, bool> SpellFixes = new Dictionary<string, bool>();
        public bool DisableAllAzataFixes = true;
        public Dictionary<string, bool> AzataFixes = new Dictionary<string, bool>();
        public bool FixDemonSubtypes = true;
        public bool FixBloodlines = true;
        public bool FixAeon = true;
        public bool FixSlayer = true;
        public bool FixWitch = true;

        public class SettingsGroup {
            public bool DisableAllFixes = false;
            public Dictionary<string, bool> Fixes = new Dictionary<string, bool>();

            public SettingsGroup (JToken json) {
                if(json == null) {
                    return;
                }
                JObject group = json.Value<JObject>();
                DisableAllFixes = group["DisableAllFixes"] != null ? group["DisableAllFixes"].Value<bool>() : false;
                Fixes = group["Fixes"] != null ? group["Fixes"].Value<JObject>()
                    .Properties()
                    .ToDictionary(
                        k => k.Name,
                        v => v.Value.Value<bool>()
                ) : new Dictionary<string, bool>();
            }
        }
    }
}
