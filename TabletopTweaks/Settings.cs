using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks {
    public class Settings {
        public bool DisableNaturalArmorStacking = true;
        public bool DisablePolymorphStacking = true;
        public bool FixDemonSubtypes = true;
        public FixGroup Aeon;
        public FixGroup Azata;
        public FixGroup DragonDisciple;
        public FixGroup Slayer;
        public FixGroup Witch;
        public FixGroup Spells;
        public FixGroup Bloodlines;
        public FixGroup MythicAbilities;

        public class FixGroup {
            public bool DisableAllFixes = false;
            public Dictionary<string, bool> Fixes = new Dictionary<string, bool>();

            public FixGroup(JToken json) {
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
