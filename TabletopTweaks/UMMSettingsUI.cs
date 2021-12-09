using Kingmaker.Utility;
using ModKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

    static class SettingsTabs {
        public static void Fixes() {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var Fixes = ModSettings.Fixes;
            UI.Div(0, 15);
            using (UI.VerticalScope()) {
                UI.Toggle("New Settings Off By Default".bold(), ref Fixes.NewSettingsOffByDefault);
                UI.Space(25);

                SetttingUI.SettingGroup("Base Fixes", TabLevel, Fixes.BaseFixes);
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
                SetttingUI.NestedSettingGroup("Bloodrager", TabLevel, Fixes.Bloodrager,
                    ("Base", Fixes.Bloodrager.Base),
                    Fixes.Bloodrager.Archetypes
                );
                SetttingUI.NestedSettingGroup("Cavalier", TabLevel, Fixes.Cavalier,
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
                SetttingUI.NestedSettingGroup("Monk", TabLevel, Fixes.Monk,
                    ("Base", Fixes.Monk.Base),
                    Fixes.Monk.Archetypes
                );
                SetttingUI.NestedSettingGroup("Oracle", TabLevel, Fixes.Oracle,
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
                SetttingUI.NestedSettingGroup("Slayer", TabLevel, Fixes.Slayer,
                    ("Base", Fixes.Slayer.Base),
                    Fixes.Slayer.Archetypes
                );
                SetttingUI.NestedSettingGroup("Sorcerer", TabLevel, Fixes.Sorcerer,
                    ("Base", Fixes.Slayer.Base),
                    Fixes.Sorcerer.Archetypes
                );
                SetttingUI.NestedSettingGroup("Witch", TabLevel, Fixes.Witch,
                    ("Base", Fixes.Witch.Base),
                    Fixes.Witch.Archetypes
                );
                SetttingUI.SettingGroup("Hellknight", TabLevel, Fixes.Hellknight);
                SetttingUI.SettingGroup("Loremaster", TabLevel, Fixes.Loremaster);
                SetttingUI.SettingGroup("Spells", TabLevel, Fixes.Spells);
                SetttingUI.SettingGroup("Bloodlines", TabLevel, Fixes.Bloodlines);
                SetttingUI.SettingGroup("Feats", TabLevel, Fixes.Feats);
                SetttingUI.SettingGroup("Mythic Abilities", TabLevel, Fixes.MythicAbilities);
                SetttingUI.SettingGroup("Mythic Feats", TabLevel, Fixes.MythicFeats);
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
                UI.Space(25);

                SetttingUI.NestedSettingGroup("Mythic Reworks", TabLevel, Homebrew.MythicReworks,
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
                UI.Space(25);

                SetttingUI.SettingGroup("Archetypes", TabLevel, AddedContent.Archetypes);
                SetttingUI.SettingGroup("Base Abilities", TabLevel, AddedContent.BaseAbilities);
                SetttingUI.SettingGroup("Bloodlines", TabLevel, AddedContent.Bloodlines);
                SetttingUI.SettingGroup("Arcanist Exploits", TabLevel, AddedContent.ArcanistExploits);
                SetttingUI.SettingGroup("Feats", TabLevel, AddedContent.Feats);
                SetttingUI.SettingGroup("Fighter Advanced Armor Training", TabLevel, AddedContent.FighterAdvancedArmorTraining);
                SetttingUI.SettingGroup("Fighter Advanced Weapon Training", TabLevel, AddedContent.FighterAdvancedWeaponTraining);
                SetttingUI.SettingGroup("Magus Arcana", TabLevel, AddedContent.MagusArcana);
                SetttingUI.SettingGroup("Races", TabLevel, AddedContent.Races);
                SetttingUI.SettingGroup("Backgrounds", TabLevel, AddedContent.Backgrounds);
                SetttingUI.SettingGroup("Spells", TabLevel, AddedContent.Spells);
                SetttingUI.SettingGroup("Mythic Abilities", TabLevel, AddedContent.MythicAbilities);
                SetttingUI.SettingGroup("Mythic Feats", TabLevel, AddedContent.MythicFeats);
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
            if ((int)level > 0) {
                level -= 1;
            }
        }
        public static int Spacing(this TabLevel level) {
            return (int)level * 50;
        }
        public static void Indent(this TabLevel level) {
            UI.Space(level.Spacing());
        }

        public static void NestedSettingGroup(string name, TabLevel level, IDisableableGroup rootGroup, (string, NestedSettingGroup) baseGroup, IDictionary<string, NestedSettingGroup> dict) {
            if (baseGroup.Item2.Settings.Empty() && !dict.Any(entry => !entry.Value.Settings.Empty())) { return; }
            RootGroup(name, level, rootGroup);
            level.Increase();
            if (rootGroup.IsExpanded()) {
                if (!baseGroup.Item2.Settings.Empty()) {
                    SettingGroup(baseGroup.Item1, level, baseGroup.Item2);
                }
                foreach (var group in dict) {
                    SettingGroup(group.Key, level, group.Value);
                }
            }
            level.Decrease();
        }

        public static void NestedSettingGroup(string name, TabLevel level, IDisableableGroup rootGroup, params (string, SettingGroup)[] nestedGroups) {
            if (!nestedGroups.Any(group => !group.Item2.Settings.Empty())) { return; }
            RootGroup(name, level, rootGroup);
            level.Increase();
            foreach (var group in nestedGroups) {
                if (rootGroup.IsExpanded()) {
                    SettingGroup(group.Item1, level, group.Item2);
                }
            }
            level.Decrease();
        }

        public static void SettingGroup(string name, TabLevel level, SettingGroup group) {
            if (group.Settings.Empty()) { return; }
            RootGroup(name, level, group);
            if (group.IsExpanded) {
                level.Increase();
                if (group.Settings.Any()) { TabbedItem(level, () => UI.Div(Color.grey, 500)); }
                group.Settings.ForEach(entry => {
                    TabbedItem(level,
                        () => Toggle(String.Join(" ", entry.Key.SplitOnCapitals()), group.IsEnabled(entry.Key), (enabled) => group.ChangeSetting(entry.Key, enabled), UI.Width(430 - level.Spacing())),
                        () => Label(entry.Value.Homebrew ? "Homebrew".yellow() : "", UI.Width(70)),
                        () => Label(entry.Value.Description.green()));
                    TabbedItem(level, () => UI.Div(Color.grey, 500));
                });
                level.Decrease();
            }
        }

        public static void RootGroup(string name, TabLevel level, IDisableableGroup rootGroup) {
            using (UI.HorizontalScope()) {
                level.Indent();
                Toggle("", !rootGroup.GroupIsDisabled(), (v) => rootGroup.SetGroupDisabled(!v), UI.AutoWidth());
                UI.DisclosureToggle(name.bold(), ref rootGroup.IsExpanded(), 140);
            }
        }

        public static void TabbedItem(TabLevel level, params Action[] actions) {
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

        public static void Label(string title, params GUILayoutOption[] options) {
            GUILayout.Label(title, options);
        }

        public static IEnumerable<string> SplitOnCapitals(this string text) {
            Regex regex = new Regex(@"\p{Lu}\p{Ll}*");
            foreach (Match match in regex.Matches(text)) {
                yield return match.Value;
            }
        }
    }
}
