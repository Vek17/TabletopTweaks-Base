using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.Config {
    public class Fixes : IUpdatableSettings {
        public bool NewSettingsOffByDefault = false;
        public SettingGroup BaseFixes = new SettingGroup();
        public SettingGroup DRRework = new SettingGroup();
        public SettingGroup Aeon = new SettingGroup();
        public SettingGroup Azata = new SettingGroup();
        public SettingGroup Lich = new SettingGroup();
        public SettingGroup Trickster = new SettingGroup();
        public ClassGroup Alchemist = new ClassGroup();
        public ClassGroup Arcanist = new ClassGroup();
        public ClassGroup Barbarian = new ClassGroup();
        public ClassGroup Bloodrager = new ClassGroup();
        public ClassGroup Cavalier = new ClassGroup();
        public ClassGroup Fighter = new ClassGroup();
        public ClassGroup Kineticist = new ClassGroup();
        public ClassGroup Magus = new ClassGroup();
        public ClassGroup Monk = new ClassGroup();
        public ClassGroup Oracle = new ClassGroup();
        public ClassGroup Paladin = new ClassGroup();
        public ClassGroup Ranger = new ClassGroup();
        public ClassGroup Rogue = new ClassGroup();
        public ClassGroup Slayer = new ClassGroup();
        public ClassGroup Witch = new ClassGroup();
        public SettingGroup Hellknight = new SettingGroup();
        public SettingGroup Loremaster = new SettingGroup();
        public SettingGroup Spells = new SettingGroup();
        public SettingGroup Bloodlines = new SettingGroup();
        public SettingGroup Feats = new SettingGroup();
        public SettingGroup MythicAbilities = new SettingGroup();
        public SettingGroup MythicFeats = new SettingGroup();
        public CrusadeGroup Crusade = new CrusadeGroup();
        public ItemGroup Items = new ItemGroup();

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as Fixes;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;

            BaseFixes.LoadSettingGroup(loadedSettings.BaseFixes, NewSettingsOffByDefault);

            DRRework.LoadSettingGroup(loadedSettings.DRRework, NewSettingsOffByDefault);

            Aeon.LoadSettingGroup(loadedSettings.Aeon, NewSettingsOffByDefault);
            Azata.LoadSettingGroup(loadedSettings.Azata, NewSettingsOffByDefault);
            Lich.LoadSettingGroup(loadedSettings.Lich, NewSettingsOffByDefault);
            Trickster.LoadSettingGroup(loadedSettings.Trickster, NewSettingsOffByDefault);

            Alchemist.LoadClassGroup(loadedSettings.Alchemist, NewSettingsOffByDefault);
            Arcanist.LoadClassGroup(loadedSettings.Arcanist, NewSettingsOffByDefault);
            Barbarian.LoadClassGroup(loadedSettings.Barbarian, NewSettingsOffByDefault);
            Bloodrager.LoadClassGroup(loadedSettings.Bloodrager, NewSettingsOffByDefault);
            Cavalier.LoadClassGroup(loadedSettings.Cavalier, NewSettingsOffByDefault);
            Fighter.LoadClassGroup(loadedSettings.Fighter, NewSettingsOffByDefault);
            Kineticist.LoadClassGroup(loadedSettings.Kineticist, NewSettingsOffByDefault);
            Magus.LoadClassGroup(loadedSettings.Magus, NewSettingsOffByDefault);
            Monk.LoadClassGroup(loadedSettings.Monk, NewSettingsOffByDefault);
            Oracle.LoadClassGroup(loadedSettings.Oracle, NewSettingsOffByDefault);
            Paladin.LoadClassGroup(loadedSettings.Paladin, NewSettingsOffByDefault);
            Ranger.LoadClassGroup(loadedSettings.Ranger, NewSettingsOffByDefault);
            Rogue.LoadClassGroup(loadedSettings.Rogue, NewSettingsOffByDefault);
            Slayer.LoadClassGroup(loadedSettings.Slayer, NewSettingsOffByDefault);
            Witch.LoadClassGroup(loadedSettings.Witch, NewSettingsOffByDefault);

            Hellknight.LoadSettingGroup(loadedSettings.Hellknight, NewSettingsOffByDefault);
            Loremaster.LoadSettingGroup(loadedSettings.Loremaster, NewSettingsOffByDefault);

            Spells.LoadSettingGroup(loadedSettings.Spells, NewSettingsOffByDefault);
            Bloodlines.LoadSettingGroup(loadedSettings.Bloodlines, NewSettingsOffByDefault);
            Feats.LoadSettingGroup(loadedSettings.Feats, NewSettingsOffByDefault);
            MythicAbilities.LoadSettingGroup(loadedSettings.MythicAbilities, NewSettingsOffByDefault);
            MythicFeats.LoadSettingGroup(loadedSettings.MythicFeats, NewSettingsOffByDefault);

            Crusade.LoadCrusadeGroup(loadedSettings.Crusade, NewSettingsOffByDefault);

            Items.LoadItemGroup(loadedSettings.Items, NewSettingsOffByDefault);
        }

        public class ClassGroup : IDisableableGroup {
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public NestedSettingGroup Base;
            public SortedDictionary<string, NestedSettingGroup> Archetypes = new SortedDictionary<string, NestedSettingGroup>();

            public ClassGroup() {
                Base = new NestedSettingGroup(this);
            }

            public void LoadClassGroup(ClassGroup group, bool frozen) {
                DisableAll = group.DisableAll;
                Base.LoadSettingGroup(group.Base, frozen);
                group.Archetypes.ForEach(entry => {
                    if (Archetypes.ContainsKey(entry.Key)) {
                        Archetypes[entry.Key].LoadSettingGroup(entry.Value, frozen);
                    }
                });
                Archetypes.ForEach(entry => entry.Value.Parent = this);
            }
        }

        public class CrusadeGroup : IDisableableGroup {
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public NestedSettingGroup Buildings;

            public CrusadeGroup() {
                Buildings = new NestedSettingGroup(this);
            }

            public void LoadCrusadeGroup(CrusadeGroup group, bool frozen) {
                DisableAll = group.DisableAll;
                Buildings.LoadSettingGroup(group.Buildings, frozen);
            }
        }

        public class ItemGroup : IDisableableGroup {
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public NestedSettingGroup Armor;
            public NestedSettingGroup Equipment;
            public NestedSettingGroup Weapons;

            public ItemGroup() {
                Armor = new NestedSettingGroup(this);
                Equipment = new NestedSettingGroup(this);
                Weapons = new NestedSettingGroup(this);
            }

            public void LoadItemGroup(ItemGroup group, bool frozen) {
                DisableAll = group.DisableAll;
                Armor.LoadSettingGroup(group.Armor, frozen);
                Equipment.LoadSettingGroup(group.Equipment, frozen);
                Weapons.LoadSettingGroup(group.Weapons, frozen);
            }
        }
    }
}
