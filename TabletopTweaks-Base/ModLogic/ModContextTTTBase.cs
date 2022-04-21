using TabletopTweaks.Base.Config;
using TabletopTweaks.Core.ModLogic;
using static UnityModManagerNet.UnityModManager;

namespace TabletopTweaks.Base.ModLogic {
    internal class ModContextTTTBase : ModContextBase {
        public Config.Bugfixes Fixes;
        public AddedContent AddedContent;

        public ModContextTTTBase(ModEntry ModEntry) : base(ModEntry) {
#if DEBUG
            Debug = true;
#endif
            LoadAllSettings();
        }
        public override void LoadAllSettings() {
            LoadSettings("Fixes.json", "TabletopTweaks.Base.Config", ref Fixes);
            LoadSettings("AddedContent.json", "TabletopTweaks.Base.Config", ref AddedContent);
            LoadBlueprints("TabletopTweaks.Base.Config", this);
            LoadLocalization("TabletopTweaks.Base.Localization");
        }
        public override void AfterBlueprintCachePatches() {
            base.AfterBlueprintCachePatches();
            if (Debug) {
                //Blueprints.RemoveUnused();
                //SaveSettings(BlueprintsFile, Blueprints);
                ModLocalizationPack.RemoveUnused();
                SaveLocalization(ModLocalizationPack);
            }
        }
        public override void SaveAllSettings() {
            base.SaveAllSettings();
            SaveSettings("Fixes.json", Fixes);
            SaveSettings("AddedContent.json", AddedContent);
        }
    }
}
