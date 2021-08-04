using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.Config {
    public class Fixes: IUpdatableSettings {
        public bool DisableNaturalArmorStacking = true;
        public bool DisablePolymorphStacking = true;
        public bool DisableCannyDefenseStacking = true;
        public bool DisableAfterCombatDeactivationOfUnlimitedAbilities = true;
        public bool DisableMonkACStacking = true;
        public bool FixMountedLongspearModifer = true;
        public bool FixShadowSpells = true;
        public SettingGroup Aeon = new SettingGroup();
        public SettingGroup Azata = new SettingGroup();
        public SettingGroup Trickster = new SettingGroup();
        public ClassGroup Arcanist = new ClassGroup();
        public ClassGroup Barbarian = new ClassGroup();
        public ClassGroup Bloodrager = new ClassGroup();
        public ClassGroup Cavalier = new ClassGroup();
        public ClassGroup Fighter = new ClassGroup();
        public ClassGroup Kineticist = new ClassGroup();
        public ClassGroup Monk = new ClassGroup();
        public ClassGroup Paladin = new ClassGroup();
        public ClassGroup Ranger = new ClassGroup();
        public ClassGroup Rogue = new ClassGroup();
        public ClassGroup Slayer = new ClassGroup();
        public ClassGroup Witch = new ClassGroup();
        public SettingGroup Spells = new SettingGroup();
        public SettingGroup Bloodlines = new SettingGroup();
        public SettingGroup Feats = new SettingGroup();
        public SettingGroup MythicAbilities = new SettingGroup();

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as Fixes;
            DisableNaturalArmorStacking = loadedSettings.DisableNaturalArmorStacking;
            DisablePolymorphStacking = loadedSettings.DisablePolymorphStacking;
            DisableCannyDefenseStacking = loadedSettings.DisableCannyDefenseStacking;
            DisableAfterCombatDeactivationOfUnlimitedAbilities = loadedSettings.DisableAfterCombatDeactivationOfUnlimitedAbilities;
            DisableMonkACStacking = loadedSettings.DisableMonkACStacking;

            FixMountedLongspearModifer = loadedSettings.FixMountedLongspearModifer;
            FixShadowSpells = loadedSettings.FixShadowSpells;

            Aeon.LoadSettingGroup(loadedSettings.Aeon);
            Azata.LoadSettingGroup(loadedSettings.Azata);
            Trickster.LoadSettingGroup(loadedSettings.Trickster);

            Arcanist.LoadClassGroup(loadedSettings.Arcanist);
            Barbarian.LoadClassGroup(loadedSettings.Barbarian);
            Bloodrager.LoadClassGroup(loadedSettings.Bloodrager);
            Cavalier.LoadClassGroup(loadedSettings.Cavalier);
            Fighter.LoadClassGroup(loadedSettings.Fighter);
            Kineticist.LoadClassGroup(loadedSettings.Kineticist);
            Monk.LoadClassGroup(loadedSettings.Monk);
            Paladin.LoadClassGroup(loadedSettings.Paladin);
            Ranger.LoadClassGroup(loadedSettings.Ranger);
            Rogue.LoadClassGroup(loadedSettings.Rogue);
            Slayer.LoadClassGroup(loadedSettings.Slayer);
            Witch.LoadClassGroup(loadedSettings.Witch);

            Spells.LoadSettingGroup(loadedSettings.Spells);
            Bloodlines.LoadSettingGroup(loadedSettings.Bloodlines);
            MythicAbilities.LoadSettingGroup(loadedSettings.MythicAbilities);
        }

        public class ClassGroup {
            public bool DisableAll = false;
            public SettingGroup Base = new SettingGroup();
            public SortedDictionary<string, SettingGroup> Archetypes = new SortedDictionary<string, SettingGroup>();
            public void LoadClassGroup(ClassGroup group) {
                DisableAll = group.DisableAll;
                Base.LoadSettingGroup(group.Base);
                group.Archetypes.ForEach(entry => {
                    if (Archetypes.ContainsKey(entry.Key)) {
                        Archetypes[entry.Key].LoadSettingGroup(entry.Value);
                    }
                });
            }
        }
    }
}
