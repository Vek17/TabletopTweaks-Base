
namespace TabletopTweaks.Config {
    public class AddedContent: IUpdatableSettings {
        public bool AberrantBloodline = true;
        public bool DestinedBloodline = true;
        public bool CauldronWitchArchetype = true;
        public bool ElementalMasterArchetype = true;

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as AddedContent;
            AberrantBloodline = loadedSettings.AberrantBloodline;
            DestinedBloodline = loadedSettings.AberrantBloodline;
            CauldronWitchArchetype = loadedSettings.CauldronWitchArchetype;
            ElementalMasterArchetype = loadedSettings.ElementalMasterArchetype;
        }
    }
}
