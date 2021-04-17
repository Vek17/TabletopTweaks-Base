
namespace TabletopTweaks.Config {
    class AddedContent {
        public bool CauldronWitchArchetype = false;
        public bool ElementalMasterArchetype = false;

        public void OverrideFixes(AddedContent userSettings) {
            CauldronWitchArchetype = userSettings.CauldronWitchArchetype;
            ElementalMasterArchetype = userSettings.ElementalMasterArchetype;
        }
    }
}
