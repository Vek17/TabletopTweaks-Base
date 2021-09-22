
namespace TabletopTweaks.Config {
    public class AddedContent : IUpdatableSettings {
        public SettingGroup Archetypes = new SettingGroup();
        public SettingGroup BaseAbilities = new SettingGroup();
        public SettingGroup Bloodlines = new SettingGroup();
        public SettingGroup ArcanistExploits = new SettingGroup();
        public SettingGroup Feats = new SettingGroup();
        public SettingGroup FighterAdvancedArmorTraining = new SettingGroup();
        public SettingGroup FighterAdvancedWeaponTraining = new SettingGroup();
        public SettingGroup MagusArcana = new SettingGroup();
        public SettingGroup PurifierCelestialArmor = new SettingGroup();
        public SettingGroup Races = new SettingGroup();
        public SettingGroup Spells = new SettingGroup();
        public SettingGroup MythicAbilities = new SettingGroup();

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as AddedContent;
            Archetypes.LoadSettingGroup(loadedSettings.Archetypes);
            BaseAbilities.LoadSettingGroup(loadedSettings.BaseAbilities);
            Bloodlines.LoadSettingGroup(loadedSettings.Bloodlines);
            ArcanistExploits.LoadSettingGroup(loadedSettings.ArcanistExploits);
            Feats.LoadSettingGroup(loadedSettings.Feats);
            FighterAdvancedArmorTraining.LoadSettingGroup(loadedSettings.FighterAdvancedArmorTraining);
            FighterAdvancedWeaponTraining.LoadSettingGroup(loadedSettings.FighterAdvancedWeaponTraining);
            MagusArcana.LoadSettingGroup(loadedSettings.MagusArcana);
            PurifierCelestialArmor.LoadSettingGroup(loadedSettings.PurifierCelestialArmor);
            Races.LoadSettingGroup(loadedSettings.Races);
            Spells.LoadSettingGroup(loadedSettings.Spells);
            MythicAbilities.LoadSettingGroup(loadedSettings.MythicAbilities);
        }
    }
}
