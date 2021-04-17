
namespace TabletopTweaks.Config {
    class AddedContent {
        public bool CauldronWitchArchetype = false;
        public bool ElementalMasterArchetype = false;

        public void OverrideFixes(AddedContent newAddedContent) {
            CauldronWitchArchetype = newAddedContent.CauldronWitchArchetype;
            ElementalMasterArchetype = newAddedContent.ElementalMasterArchetype;
        }
    }
}
