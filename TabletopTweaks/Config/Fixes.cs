using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.Config {
    public class Fixes: IUpdatableSettings {
        public bool DisableNaturalArmorStacking = true;
        public bool DisablePolymorphStacking = true;
        public bool DisableCannyDefenseStacking = true;
        public bool DisableMonkACStacking = true;
        public bool FixDemonSubtypes = true;
        public FixGroup Aeon = new FixGroup();
        public FixGroup Azata = new FixGroup();
        public ClassGroup Barbarian = new ClassGroup();
        public ClassGroup Bloodrager = new ClassGroup();
        public ClassGroup Cavalier = new ClassGroup();
        public ClassGroup Kineticist = new ClassGroup();
        public ClassGroup Monk = new ClassGroup();
        public ClassGroup Paladin = new ClassGroup();
        public ClassGroup Ranger = new ClassGroup();
        public ClassGroup Rogue = new ClassGroup();
        public ClassGroup Slayer = new ClassGroup();
        public ClassGroup Witch = new ClassGroup();
        public FixGroup Spells = new FixGroup();
        public FixGroup Bloodlines = new FixGroup();
        public FixGroup Feats = new FixGroup();
        public FixGroup MythicAbilities = new FixGroup();

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as Fixes;
            DisableNaturalArmorStacking = loadedSettings.DisableNaturalArmorStacking;
            DisablePolymorphStacking = loadedSettings.DisablePolymorphStacking;
            DisableCannyDefenseStacking = loadedSettings.DisableCannyDefenseStacking;
            DisableMonkACStacking = loadedSettings.DisableMonkACStacking;

            FixDemonSubtypes = loadedSettings.FixDemonSubtypes;

            Aeon.LoadFixgroup(loadedSettings.Aeon);
            Azata.LoadFixgroup(loadedSettings.Azata);

            Barbarian.LoadClassGroup(loadedSettings.Barbarian);
            Bloodrager.LoadClassGroup(loadedSettings.Bloodrager);
            Cavalier.LoadClassGroup(loadedSettings.Cavalier);
            Kineticist.LoadClassGroup(loadedSettings.Kineticist);
            Monk.LoadClassGroup(loadedSettings.Monk);
            Paladin.LoadClassGroup(loadedSettings.Paladin);
            Ranger.LoadClassGroup(loadedSettings.Ranger);
            Rogue.LoadClassGroup(loadedSettings.Rogue);
            Slayer.LoadClassGroup(loadedSettings.Slayer);
            Witch.LoadClassGroup(loadedSettings.Witch);

            Spells.LoadFixgroup(loadedSettings.Spells);
            Bloodlines.LoadFixgroup(loadedSettings.Bloodlines);
            MythicAbilities.LoadFixgroup(loadedSettings.MythicAbilities);
        }

        public class FixGroup {
            public bool DisableAllFixes = false;
            public SortedDictionary<string, bool> Fixes = new SortedDictionary<string, bool>();
            public void LoadFixgroup(FixGroup group) {
                DisableAllFixes = group.DisableAllFixes;
                group.Fixes.ForEach(entry => {
                    if (Fixes.ContainsKey(entry.Key)) {
                        Fixes[entry.Key] = entry.Value;
                    }
                });
            }
        }

        public class ClassGroup {
            public bool DisableAllFixes = false;
            public FixGroup Base = new FixGroup();
            public SortedDictionary<string, FixGroup> Archetypes = new SortedDictionary<string, FixGroup>();
            public void LoadClassGroup(ClassGroup group) {
                DisableAllFixes = group.DisableAllFixes;
                Base.LoadFixgroup(group.Base);
                group.Archetypes.ForEach(entry => {
                    if (Archetypes.ContainsKey(entry.Key)) {
                        Archetypes[entry.Key].LoadFixgroup(entry.Value);
                    }
                });
            }
        }
    }
}
