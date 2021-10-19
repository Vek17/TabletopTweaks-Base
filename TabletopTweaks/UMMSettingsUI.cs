using Kingmaker.Utility;
using ModKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;
using UnityEngine;
using UnityModManagerNet;

namespace TabletopTweaks {
    public static class UMMSettingsUI {
        private static int selectedTab;
        public static void OnGUI(UnityModManager.ModEntry modEntry) {
            UI.AutoWidth();
            UI.TabBar(ref selectedTab,
                    () => UI.Label("SETTINGS WILL NOT BE UPDATED UNTIL YOU RESTART YOUR GAME.".yellow().bold()),
                    new NamedAction("Fixes", () => SettingsTabs.Fixes()),
                    new NamedAction("Homebrew", () => SettingsTabs.Homebrew()),
                    new NamedAction("Added Content", () => SettingsTabs.AddedContent())
            );
        }
    }

    static class SettingsTabs{
        public static void Fixes() {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var Fixes = ModSettings.Fixes;
            UI.Div(0, 15);
            using (UI.VerticalScope()) {
                UI.Toggle("New Settings Off By Default".bold(), ref Fixes.NewSettingsOffByDefault);
                SetttingUI.SettingGroup("BaseFixes", TabLevel, Fixes.BaseFixes);
                SetttingUI.SettingGroup("Lich", TabLevel, Fixes.Lich);
                SetttingUI.SettingGroup("Trickster", TabLevel, Fixes.Trickster);
                SetttingUI.NestedSettingGroup("Alchemist", TabLevel, Fixes.Alchemist,
                    ("Base", Fixes.Alchemist.Base),
                    Fixes.Alchemist.Archetypes
                );
                SetttingUI.NestedSettingGroup("Arcanist", TabLevel, Fixes.Arcanist,
                    ("Base", Fixes.Arcanist.Base),
                    Fixes.Arcanist.Archetypes
                );
                SetttingUI.NestedSettingGroup("Barbarian", TabLevel, Fixes.Barbarian,
                    ("Base", Fixes.Barbarian.Base),
                    Fixes.Barbarian.Archetypes
                );
                SetttingUI.NestedSettingGroup("Bloodrager", TabLevel, Fixes.Barbarian,
                    ("Base", Fixes.Bloodrager.Base),
                    Fixes.Bloodrager.Archetypes
                );
                SetttingUI.NestedSettingGroup("Cavalier", TabLevel, Fixes.Barbarian,
                    ("Base", Fixes.Cavalier.Base),
                    Fixes.Cavalier.Archetypes
                );
                SetttingUI.NestedSettingGroup("Fighter", TabLevel, Fixes.Fighter,
                    ("Base", Fixes.Fighter.Base),
                    Fixes.Fighter.Archetypes
                );
                SetttingUI.NestedSettingGroup("Kineticist", TabLevel, Fixes.Kineticist,
                    ("Base", Fixes.Kineticist.Base),
                    Fixes.Kineticist.Archetypes
                );
                SetttingUI.NestedSettingGroup("Magus", TabLevel, Fixes.Magus,
                    ("Base", Fixes.Magus.Base),
                    Fixes.Magus.Archetypes
                );
                SetttingUI.NestedSettingGroup("Magus", TabLevel, Fixes.Monk,
                    ("Base", Fixes.Monk.Base),
                    Fixes.Monk.Archetypes
                );
                SetttingUI.NestedSettingGroup("Magus", TabLevel, Fixes.Oracle,
                    ("Base", Fixes.Oracle.Base),
                    Fixes.Oracle.Archetypes
                );
                SetttingUI.NestedSettingGroup("Paladin", TabLevel, Fixes.Paladin,
                    ("Base", Fixes.Paladin.Base),
                    Fixes.Paladin.Archetypes
                );
                SetttingUI.NestedSettingGroup("Ranger", TabLevel, Fixes.Ranger,
                    ("Base", Fixes.Ranger.Base),
                    Fixes.Ranger.Archetypes
                );
                SetttingUI.NestedSettingGroup("Rogue", TabLevel, Fixes.Rogue,
                    ("Base", Fixes.Rogue.Base),
                    Fixes.Rogue.Archetypes
                );
                SetttingUI.NestedSettingGroup("Witch", TabLevel, Fixes.Witch,
                    ("Base", Fixes.Slayer.Base),
                    Fixes.Slayer.Archetypes
                );
                SetttingUI.NestedSettingGroup("Witch", TabLevel, Fixes.Witch,
                    ("Base", Fixes.Slayer.Base),
                    Fixes.Slayer.Archetypes
                );
                SetttingUI.SettingGroup("Hellknight", TabLevel, Fixes.Hellknight);
                SetttingUI.SettingGroup("Loremaster", TabLevel, Fixes.Loremaster);
                SetttingUI.SettingGroup("Spells", TabLevel, Fixes.Spells);
                SetttingUI.SettingGroup("Bloodlines", TabLevel, Fixes.Bloodlines);
                SetttingUI.SettingGroup("Feats", TabLevel, Fixes.Feats);
                SetttingUI.SettingGroup("MythicAbilities", TabLevel, Fixes.MythicAbilities);
                SetttingUI.SettingGroup("MythicFeats", TabLevel, Fixes.MythicFeats);
                SetttingUI.NestedSettingGroup("Crusade", TabLevel, Fixes.Crusade,
                    ("Buildings", Fixes.Crusade.Buildings)
                );
                SetttingUI.NestedSettingGroup("Items", TabLevel, Fixes.Items,
                    ("Armor", Fixes.Items.Armor),
                    ("Equipment", Fixes.Items.Equipment),
                    ("Weapons", Fixes.Items.Weapons)
                );
            }
        }
        public static void Homebrew() {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var Homebrew = ModSettings.Homebrew;
            UI.Div(0, 15);
            using (UI.VerticalScope()) {
                UI.Toggle("New Settings Off By Default".bold(), ref Homebrew.NewSettingsOffByDefault);
                SetttingUI.NestedSettingGroup("MythicReworks", TabLevel, Homebrew.MythicReworks, 
                    ("Aeon", Homebrew.MythicReworks.Aeon),
                    ("Azata", Homebrew.MythicReworks.Azata)
                );
            }
        }
        public static void AddedContent() {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var AddedContent = ModSettings.AddedContent;
            UI.Div(0, 15);
            using (UI.VerticalScope()) {
                UI.Toggle("New Settings Off By Default".bold(), ref AddedContent.NewSettingsOffByDefault);
                SetttingUI.SettingGroup("Archetypes", TabLevel, AddedContent.Archetypes);
                SetttingUI.SettingGroup("BaseAbilities", TabLevel, AddedContent.BaseAbilities);
                SetttingUI.SettingGroup("Bloodlines", TabLevel, AddedContent.Bloodlines);
                SetttingUI.SettingGroup("ArcanistExploits", TabLevel, AddedContent.ArcanistExploits);
                SetttingUI.SettingGroup("Feats", TabLevel, AddedContent.Feats);
                SetttingUI.SettingGroup("FighterAdvancedArmorTraining", TabLevel, AddedContent.FighterAdvancedArmorTraining);
                SetttingUI.SettingGroup("FighterAdvancedWeaponTraining", TabLevel, AddedContent.FighterAdvancedWeaponTraining);
                SetttingUI.SettingGroup("MagusArcana", TabLevel, AddedContent.MagusArcana);
                SetttingUI.SettingGroup("Races", TabLevel, AddedContent.Races);
                SetttingUI.SettingGroup("Backgrounds", TabLevel, AddedContent.Backgrounds);
                SetttingUI.SettingGroup("Spells", TabLevel, AddedContent.Spells);
                SetttingUI.SettingGroup("MythicAbilities", TabLevel, AddedContent.MythicAbilities);
                SetttingUI.SettingGroup("MythicFeats", TabLevel, AddedContent.MythicFeats);
            }
        }
    }

    static class SetttingUI {
        public enum TabLevel : int { 
            Zero,
            One,
            Two,
            Three,
            Four,
            Five
        }

        public static void Increase(ref this TabLevel level) {
            level += 1;
        }

        public static void Decrease(ref this TabLevel level) {
            level -= 1;
        }

        public static int Spacing(this TabLevel level) {
            return (int)level * 25;
        }

        public static void Indent(this TabLevel level) {
            UI.Space(level.Spacing());
        }

        public static void NestedSettingGroup(string name, TabLevel level, ICollapseableGroup rootGroup, (string, SettingGroup) baseGroup, IDictionary<string, NestedSettingGroup> dict) {
            TabbedItem(level, () => UI.DisclosureToggle(name, ref rootGroup.IsExpanded()));
            level.Increase();
            if (rootGroup.IsExpanded()) {
                SettingGroup(baseGroup.Item1, level, baseGroup.Item2);
                foreach (var group in dict) {
                    SettingGroup(group.Key, level, group.Value);
                }
            }
            level.Decrease();
        }

        public static void NestedSettingGroup(string name, TabLevel level, ICollapseableGroup rootGroup, params (string, SettingGroup)[] nestedGroups) {
            TabbedItem(level, () => UI.DisclosureToggle(name, ref rootGroup.IsExpanded()));
            level.Increase();
            foreach (var group in nestedGroups) {
                if (rootGroup.IsExpanded()) {
                    SettingGroup(group.Item1, level, group.Item2);
                }
            }
            level.Decrease();
        }

        public static void SettingGroup(string name, TabLevel level, SettingGroup group) {
            using (UI.HorizontalScope()) {
                level.Indent();
                UI.DisclosureToggle(name, ref group.IsExpanded);
            }
            if (group.IsExpanded) {
                level.Increase();
                TabbedItem(level, () => Toggle("Disable All", group.DisableAll, (v) => group.DisableAll = v));
                group.Settings.ForEach(entry => TabbedItem(level, 
                    () => Toggle(entry.Key, group.IsEnabled(entry.Key), (enabled) => group.ChangeSetting(entry.Key, enabled), UI.Width(500)),
                    () => Label(entry.Value.Description.green())
                ));
                level.Decrease();
            }
        }

        public static void TabbedItem(TabLevel level,  params Action[] actions) {
            using (UI.HorizontalScope()) {
                level.Indent();
                actions.ForEach(action => action.Invoke());
            }
        }

        public static bool Toggle(string title, bool value, Action<bool> action, params GUILayoutOption[] options) {
            options = options.AddDefaults();
            var changed = ModKit.Private.UI.CheckBox(title, value, UI.toggleStyle, options);
            if (changed) {
                action.Invoke(!value);
            }
            return changed;
        }

        public static void Label(string title) {
            GUILayout.Label(title, GUILayout.ExpandWidth(false));
        }
    }
}
