using System.Collections.Generic;
using UnityModManagerNet;

namespace TabletopTweaks {
    public class Settings : UnityModManager.ModSettings {
        public bool DisableNaturalArmorStacking = true;
        public bool DisablePolymorphStacking = true;
        public bool DisableAllSpellFixes = true;
        public Dictionary<string, bool> SpellFixes = new Dictionary<string, bool>();
        public bool FixDemonSubtypes = true;
        public bool FixBloodlines = true;
        public bool FixAeon = true;
        public bool FixAzata = true;
        public bool FixSlayer = true;
        public bool FixWitch = true;

        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
    }
}
