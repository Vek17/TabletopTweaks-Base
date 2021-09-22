using Kingmaker.Utility;
using System.Collections.Generic;

namespace TabletopTweaks.Config {
    public class Fixes : IUpdatableSettings {
        public bool DisableNaturalArmorStacking = true;
        public bool DisablePolymorphStacking = true;
        public bool DisableCannyDefenseStacking = true;
        public bool DisableAfterCombatDeactivationOfUnlimitedAbilities = true;
        public bool FixMountedLongspearModifer = true;
        public bool FixInherentSkillpoints = true;
        public bool FixBackgroundModifiers = true;
        public bool FixShadowSpells = true;
        public bool MetamagicStacking = true;
        public bool SelectiveMetamagicNonInstantaneous = true;
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
        public CrusadeGroup Crusade = new CrusadeGroup();
        public ItemGroup Items = new ItemGroup();

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as Fixes;
            DisableNaturalArmorStacking = loadedSettings.DisableNaturalArmorStacking;
            DisablePolymorphStacking = loadedSettings.DisablePolymorphStacking;
            DisableCannyDefenseStacking = loadedSettings.DisableCannyDefenseStacking;
            DisableAfterCombatDeactivationOfUnlimitedAbilities = loadedSettings.DisableAfterCombatDeactivationOfUnlimitedAbilities;

            FixMountedLongspearModifer = loadedSettings.FixMountedLongspearModifer;
            FixShadowSpells = loadedSettings.FixShadowSpells;
            MetamagicStacking = loadedSettings.MetamagicStacking;
            SelectiveMetamagicNonInstantaneous = loadedSettings.SelectiveMetamagicNonInstantaneous;

            Aeon.LoadSettingGroup(loadedSettings.Aeon);
            Azata.LoadSettingGroup(loadedSettings.Azata);
            Lich.LoadSettingGroup(loadedSettings.Lich);
            Trickster.LoadSettingGroup(loadedSettings.Trickster);

            Alchemist.LoadClassGroup(loadedSettings.Alchemist);
            Arcanist.LoadClassGroup(loadedSettings.Arcanist);
            Barbarian.LoadClassGroup(loadedSettings.Barbarian);
            Bloodrager.LoadClassGroup(loadedSettings.Bloodrager);
            Cavalier.LoadClassGroup(loadedSettings.Cavalier);
            Fighter.LoadClassGroup(loadedSettings.Fighter);
            Kineticist.LoadClassGroup(loadedSettings.Kineticist);
            Magus.LoadClassGroup(loadedSettings.Magus);
            Monk.LoadClassGroup(loadedSettings.Monk);
            Oracle.LoadClassGroup(loadedSettings.Oracle);
            Paladin.LoadClassGroup(loadedSettings.Paladin);
            Ranger.LoadClassGroup(loadedSettings.Ranger);
            Rogue.LoadClassGroup(loadedSettings.Rogue);
            Slayer.LoadClassGroup(loadedSettings.Slayer);
            Witch.LoadClassGroup(loadedSettings.Witch);

            Hellknight.LoadSettingGroup(loadedSettings.Hellknight);
            Loremaster.LoadSettingGroup(loadedSettings.Loremaster);

            Spells.LoadSettingGroup(loadedSettings.Spells);
            Bloodlines.LoadSettingGroup(loadedSettings.Bloodlines);
            Feats.LoadSettingGroup(loadedSettings.Feats);
            MythicAbilities.LoadSettingGroup(loadedSettings.MythicAbilities);

            Crusade.LoadCrusadeGroup(loadedSettings.Crusade);

            Items.LoadItemGroup(loadedSettings.Items);
        }

        public class ClassGroup : IDisableableGroup {
            public bool DisableAll = false;
            public bool GroupIsDisabled() => DisableAll;
            public NestedSettingGroup Base;
            public SortedDictionary<string, NestedSettingGroup> Archetypes = new SortedDictionary<string, NestedSettingGroup>();

            public ClassGroup() {
                Base = new NestedSettingGroup(this);
            }

            public void LoadClassGroup(ClassGroup group) {
                DisableAll = group.DisableAll;
                Base.LoadSettingGroup(group.Base);
                group.Archetypes.ForEach(entry => {
                    if (Archetypes.ContainsKey(entry.Key)) {
                        Archetypes[entry.Key].LoadSettingGroup(entry.Value);
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

            public void LoadCrusadeGroup(CrusadeGroup group) {
                DisableAll = group.DisableAll;
                Buildings.LoadSettingGroup(group.Buildings);
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

            public void LoadItemGroup(ItemGroup group) {
                DisableAll = group.DisableAll;
                Armor.LoadSettingGroup(group.Armor);
                Equipment.LoadSettingGroup(group.Equipment);
                Weapons.LoadSettingGroup(group.Weapons);
            }
        }
    }
}
