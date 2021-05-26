
namespace TabletopTweaks.Config {
    public class AddedContent: IUpdatableSettings {
        public SettingGroup Archetypes = new SettingGroup();
        public SettingGroup BaseAbilities = new SettingGroup();
        public SettingGroup Bloodlines = new SettingGroup();
        public SettingGroup ArcanistExploits = new SettingGroup();
        public SettingGroup Races = new SettingGroup();
        public SettingGroup Spells = new SettingGroup();
        public SettingGroup MythicAbilities = new SettingGroup();

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as AddedContent;
            Archetypes.LoadSettingGroup(loadedSettings.Archetypes);
            BaseAbilities.LoadSettingGroup(loadedSettings.BaseAbilities);
            Bloodlines.LoadSettingGroup(loadedSettings.Bloodlines);
            ArcanistExploits.LoadSettingGroup(loadedSettings.ArcanistExploits);
            Spells.LoadSettingGroup(loadedSettings.Spells);
            MythicAbilities.LoadSettingGroup(loadedSettings.MythicAbilities);
        }
    }
}
