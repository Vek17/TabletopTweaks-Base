using Kingmaker.Utility;
using System.Collections.Generic;
using TabletopTweaks.Base.Bugfixes.Classes;
using TabletopTweaks.Core.Config;

namespace TabletopTweaks.Base.Config {
    public class Bugfixes : IUpdatableSettings {

        public bool NewSettingsOffByDefault = false;
        public SettingGroup BaseFixes = new SettingGroup();
        public SettingGroup Aeon = new SettingGroup();
        public SettingGroup Demon = new SettingGroup();
        public SettingGroup Lich = new SettingGroup();
        public SettingGroup Trickster = new SettingGroup();
        public SettingGroup AlternateCapstones = new SettingGroup();
        public ClassGroup Alchemist = new ClassGroup();
        public ClassGroup Arcanist = new ClassGroup();
        public ClassGroup Barbarian = new ClassGroup();
        public ClassGroup Bloodrager = new ClassGroup();
        public ClassGroup Cavalier = new ClassGroup();
        public ClassGroup Cleric = new ClassGroup();
        public ClassGroup Fighter = new ClassGroup();
        public ClassGroup Hunter = new ClassGroup();
        public ClassGroup Kineticist = new ClassGroup();
        public ClassGroup Magus = new ClassGroup();
        public ClassGroup Monk = new ClassGroup();
        public ClassGroup Oracle = new ClassGroup();
        public ClassGroup Paladin = new ClassGroup();
        public ClassGroup Ranger = new ClassGroup();
        public ClassGroup Rogue = new ClassGroup();
        public ClassGroup Shaman = new ClassGroup();
        public ClassGroup Skald = new ClassGroup();
        public ClassGroup Slayer = new ClassGroup();
        public ClassGroup Sorcerer = new ClassGroup();
        public ClassGroup Warpriest = new ClassGroup();
        public ClassGroup Witch = new ClassGroup();
        public SettingGroup Hellknight = new SettingGroup();
        public SettingGroup Loremaster = new SettingGroup();
        public SettingGroup WinterWitch = new SettingGroup();
        public SettingGroup Spells = new SettingGroup();
        public SettingGroup Bloodlines = new SettingGroup();
        public SettingGroup Features = new SettingGroup();
        public SettingGroup Feats = new SettingGroup();
        public SettingGroup MythicAbilities = new SettingGroup();
        public SettingGroup MythicFeats = new SettingGroup();
        public UnitGroup Units = new UnitGroup();
        public CrusadeGroup Crusade = new CrusadeGroup();
        public ItemGroup Items = new ItemGroup();

        public void Init() {
            Alchemist.SetParents();
            Arcanist.SetParents();
            Barbarian.SetParents();
            Bloodrager.SetParents();
            Cavalier.SetParents();
            Cleric.SetParents();
            Fighter.SetParents();
            Hunter.SetParents();
            Kineticist.SetParents();
            Magus.SetParents();
            Monk.SetParents();
            Oracle.SetParents();
            Paladin.SetParents();
            Ranger.SetParents();
            Rogue.SetParents();
            Shaman.SetParents();
            Skald.SetParents();
            Slayer.SetParents();
            Sorcerer.SetParents();
            Warpriest.SetParents();
            Witch.SetParents();

            Units.SetParents();
            Crusade.SetParents();
            Items.SetParents();
        }

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as Bugfixes;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;

            BaseFixes.LoadSettingGroup(loadedSettings.BaseFixes, NewSettingsOffByDefault);

            Aeon.LoadSettingGroup(loadedSettings.Aeon, NewSettingsOffByDefault);
            Demon.LoadSettingGroup(loadedSettings.Demon, NewSettingsOffByDefault);
            Lich.LoadSettingGroup(loadedSettings.Lich, NewSettingsOffByDefault);
            Trickster.LoadSettingGroup(loadedSettings.Trickster, NewSettingsOffByDefault);

            AlternateCapstones.LoadSettingGroup(loadedSettings.AlternateCapstones, NewSettingsOffByDefault);

            Alchemist.LoadClassGroup(loadedSettings.Alchemist, NewSettingsOffByDefault);
            Arcanist.LoadClassGroup(loadedSettings.Arcanist, NewSettingsOffByDefault);
            Barbarian.LoadClassGroup(loadedSettings.Barbarian, NewSettingsOffByDefault);
            Bloodrager.LoadClassGroup(loadedSettings.Bloodrager, NewSettingsOffByDefault);
            Cavalier.LoadClassGroup(loadedSettings.Cavalier, NewSettingsOffByDefault);
            Cleric.LoadClassGroup(loadedSettings.Cleric, NewSettingsOffByDefault);
            Fighter.LoadClassGroup(loadedSettings.Fighter, NewSettingsOffByDefault);
            Hunter.LoadClassGroup(loadedSettings.Hunter, NewSettingsOffByDefault);
            Kineticist.LoadClassGroup(loadedSettings.Kineticist, NewSettingsOffByDefault);
            Magus.LoadClassGroup(loadedSettings.Magus, NewSettingsOffByDefault);
            Monk.LoadClassGroup(loadedSettings.Monk, NewSettingsOffByDefault);
            Oracle.LoadClassGroup(loadedSettings.Oracle, NewSettingsOffByDefault);
            Paladin.LoadClassGroup(loadedSettings.Paladin, NewSettingsOffByDefault);
            Ranger.LoadClassGroup(loadedSettings.Ranger, NewSettingsOffByDefault);
            Rogue.LoadClassGroup(loadedSettings.Rogue, NewSettingsOffByDefault);
            Shaman.LoadClassGroup(loadedSettings.Shaman, NewSettingsOffByDefault);
            Skald.LoadClassGroup(loadedSettings.Skald, NewSettingsOffByDefault);
            Slayer.LoadClassGroup(loadedSettings.Slayer, NewSettingsOffByDefault);
            Sorcerer.LoadClassGroup(loadedSettings.Sorcerer, NewSettingsOffByDefault);
            Warpriest.LoadClassGroup(loadedSettings.Warpriest, NewSettingsOffByDefault);
            Witch.LoadClassGroup(loadedSettings.Witch, NewSettingsOffByDefault);

            Hellknight.LoadSettingGroup(loadedSettings.Hellknight, NewSettingsOffByDefault);
            Loremaster.LoadSettingGroup(loadedSettings.Loremaster, NewSettingsOffByDefault);
            WinterWitch.LoadSettingGroup(loadedSettings.WinterWitch, NewSettingsOffByDefault);

            Spells.LoadSettingGroup(loadedSettings.Spells, NewSettingsOffByDefault);
            Bloodlines.LoadSettingGroup(loadedSettings.Bloodlines, NewSettingsOffByDefault);
            Features.LoadSettingGroup(loadedSettings.Features, NewSettingsOffByDefault);
            Feats.LoadSettingGroup(loadedSettings.Feats, NewSettingsOffByDefault);
            MythicAbilities.LoadSettingGroup(loadedSettings.MythicAbilities, NewSettingsOffByDefault);
            MythicFeats.LoadSettingGroup(loadedSettings.MythicFeats, NewSettingsOffByDefault);

            Units.LoadUnitGroup(loadedSettings.Units, NewSettingsOffByDefault);

            Crusade.LoadCrusadeGroup(loadedSettings.Crusade, NewSettingsOffByDefault);

            Items.LoadItemGroup(loadedSettings.Items, NewSettingsOffByDefault);
        }

        public class ClassGroup : IDisableableGroup, ICollapseableGroup {
            private bool IsExpanded = true;
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public void SetGroupDisabled(bool value) => DisableAll = value;
            public NestedSettingGroup Base;
            public SortedDictionary<string, NestedSettingGroup> Archetypes = new SortedDictionary<string, NestedSettingGroup>();

            public ClassGroup() {
                Base = new NestedSettingGroup(this);
            }

            public void SetParents() {
                Base.Parent = this;
                Archetypes.ForEach(entry => entry.Value.Parent = this);
            }

            public void LoadClassGroup(ClassGroup group, bool frozen) {
                DisableAll = group.DisableAll;
                Base.LoadSettingGroup(group.Base, frozen);
                group.Archetypes.ForEach(entry => {
                    if (Archetypes.ContainsKey(entry.Key)) {
                        Archetypes[entry.Key].LoadSettingGroup(entry.Value, frozen);
                    }
                });
            }

            ref bool ICollapseableGroup.IsExpanded() {
                return ref IsExpanded;
            }

            public void SetExpanded(bool value) {
                IsExpanded = value;
            }
        }

        public class CrusadeGroup : IDisableableGroup, ICollapseableGroup {
            private bool IsExpanded = true;
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public void SetGroupDisabled(bool value) => DisableAll = value;
            public NestedSettingGroup Buildings;

            public CrusadeGroup() {
                Buildings = new NestedSettingGroup(this);
            }

            public void SetParents() {
                Buildings.Parent = this;
            }

            public void LoadCrusadeGroup(CrusadeGroup group, bool frozen) {
                DisableAll = group.DisableAll;
                Buildings.LoadSettingGroup(group.Buildings, frozen);
            }

            ref bool ICollapseableGroup.IsExpanded() {
                return ref IsExpanded;
            }

            public void SetExpanded(bool value) {
                IsExpanded = value;
            }
        }

        public class ItemGroup : IDisableableGroup, ICollapseableGroup {
            private bool IsExpanded = true;
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public void SetGroupDisabled(bool value) => DisableAll = value;
            public NestedSettingGroup Armor;
            public NestedSettingGroup Equipment;
            public NestedSettingGroup Weapons;

            public ItemGroup() {
                Armor = new NestedSettingGroup(this);
                Equipment = new NestedSettingGroup(this);
                Weapons = new NestedSettingGroup(this);
            }

            public void SetParents() {
                Armor.Parent = this;
                Equipment.Parent = this;
                Weapons.Parent = this;
            }

            public void LoadItemGroup(ItemGroup group, bool frozen) {
                DisableAll = group.DisableAll;
                Armor.LoadSettingGroup(group.Armor, frozen);
                Equipment.LoadSettingGroup(group.Equipment, frozen);
                Weapons.LoadSettingGroup(group.Weapons, frozen);
            }

            ref bool ICollapseableGroup.IsExpanded() {
                return ref IsExpanded;
            }

            public void SetExpanded(bool value) {
                IsExpanded = value;
            }
        }

        public class UnitGroup : IDisableableGroup, ICollapseableGroup {
            private bool IsExpanded = true;
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public void SetGroupDisabled(bool value) => DisableAll = value;
            public NestedSettingGroup Companions;
            public NestedSettingGroup NPCs;
            public NestedSettingGroup Bosses;
            public NestedSettingGroup Enemies;

            public UnitGroup() {
                Companions = new NestedSettingGroup(this);
                NPCs = new NestedSettingGroup(this);
                Bosses = new NestedSettingGroup(this);
                Enemies = new NestedSettingGroup(this);
            }

            public void SetParents() {
                Companions.Parent = this;
                NPCs.Parent = this;
                Bosses.Parent = this;
                Enemies.Parent = this;
            }

            public void LoadUnitGroup(UnitGroup group, bool frozen) {
                DisableAll = group.DisableAll;
                Bosses.LoadSettingGroup(group.Bosses, frozen);
                Enemies.LoadSettingGroup(group.Enemies, frozen);
                Companions.LoadSettingGroup(group.Companions, frozen);
                NPCs.LoadSettingGroup(group.NPCs, frozen);
            }

            ref bool ICollapseableGroup.IsExpanded() {
                return ref IsExpanded;
            }

            public void SetExpanded(bool value) {
                IsExpanded = value;
            }
        }
    }
}
