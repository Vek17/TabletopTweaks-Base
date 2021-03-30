using UnityModManagerNet;
using Kingmaker.EntitySystem.Stats;

namespace TabletopTweaks {
    public class Settings : UnityModManager.ModSettings {
        public bool DisableNaturalArmorStacking = true;
        public bool DisablePolymorphStacking = true;
        public bool FixSlayerStudiedTarget = true;
        public bool FixDemonSubtypes = true;
        public bool FixBloodlines = true;

        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
    }
}
