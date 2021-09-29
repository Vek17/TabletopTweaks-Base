
namespace TabletopTweaks.Config {
    public class AddedContent : IUpdatableSettings {
        public bool NewSettingsOffByDefault = false;
        public SettingGroup Archetypes = new SettingGroup();
        public SettingGroup BaseAbilities = new SettingGroup();
        public SettingGroup Bloodlines = new SettingGroup();
        public SettingGroup ArcanistExploits = new SettingGroup();
        public SettingGroup Feats = new SettingGroup();
        public SettingGroup FighterAdvancedArmorTraining = new SettingGroup();
        public SettingGroup FighterAdvancedWeaponTraining = new SettingGroup();
        public SettingGroup MagusArcana = new SettingGroup();
        public SettingGroup Races = new SettingGroup();
        public SettingGroup Spells = new SettingGroup();
        public SettingGroup MythicAbilities = new SettingGroup();
        public SettingGroup MythicFeats = new SettingGroup();

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as AddedContent;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;
            Archetypes.LoadSettingGroup(loadedSettings.Archetypes, NewSettingsOffByDefault);
            BaseAbilities.LoadSettingGroup(loadedSettings.BaseAbilities, NewSettingsOffByDefault);
            Bloodlines.LoadSettingGroup(loadedSettings.Bloodlines, NewSettingsOffByDefault);
            ArcanistExploits.LoadSettingGroup(loadedSettings.ArcanistExploits, NewSettingsOffByDefault);
            Feats.LoadSettingGroup(loadedSettings.Feats, NewSettingsOffByDefault);
            FighterAdvancedArmorTraining.LoadSettingGroup(loadedSettings.FighterAdvancedArmorTraining, NewSettingsOffByDefault);
            FighterAdvancedWeaponTraining.LoadSettingGroup(loadedSettings.FighterAdvancedWeaponTraining, NewSettingsOffByDefault);
            MagusArcana.LoadSettingGroup(loadedSettings.MagusArcana, NewSettingsOffByDefault);
            Races.LoadSettingGroup(loadedSettings.Races, NewSettingsOffByDefault);
            Spells.LoadSettingGroup(loadedSettings.Spells, NewSettingsOffByDefault);
            MythicAbilities.LoadSettingGroup(loadedSettings.MythicAbilities, NewSettingsOffByDefault);
            MythicFeats.LoadSettingGroup(loadedSettings.MythicFeats, NewSettingsOffByDefault);
        }
    }
}
