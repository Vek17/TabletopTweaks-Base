using TabletopTweaks.Core.UMMTools;
using UnityModManagerNet;

namespace TabletopTweaks.Base {
    internal static class UMMSettingsUI {
        private static int selectedTab;
        public static void OnGUI(UnityModManager.ModEntry modEntry) {
            UI.AutoWidth();
            UI.TabBar(ref selectedTab,
                    () => UI.Label("SETTINGS WILL NOT BE UPDATED UNTIL YOU RESTART YOUR GAME.".yellow().bold()),
                    new NamedAction("Fixes", () => SettingsTabs.Fixes()),
                    new NamedAction("Added Content", () => SettingsTabs.AddedContent())
            );
        }
    }

    internal static class SettingsTabs {
        public static void Fixes() {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var Fixes = Main.TTTContext.Fixes;
            UI.Div(0, 15);
            using (UI.VerticalScope()) {
                UI.Toggle("New Settings Off By Default".bold(), ref Fixes.NewSettingsOffByDefault);
                UI.Space(25);

                SetttingUI.SettingGroup("Base Fixes", TabLevel, Fixes.BaseFixes);
                SetttingUI.SettingGroup("Aeon", TabLevel, Fixes.Aeon);
                SetttingUI.SettingGroup("Demon", TabLevel, Fixes.Demon);
                SetttingUI.SettingGroup("Lich", TabLevel, Fixes.Lich);
                SetttingUI.SettingGroup("Trickster", TabLevel, Fixes.Trickster);
                SetttingUI.SettingGroup("Alternate Capstones", TabLevel, Fixes.AlternateCapstones);
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
                SetttingUI.NestedSettingGroup("Cleric", TabLevel, Fixes.Cleric,
                    ("Base", Fixes.Cleric.Base),
                    Fixes.Cleric.Archetypes
                );
                SetttingUI.NestedSettingGroup("Fighter", TabLevel, Fixes.Fighter,
                    ("Base", Fixes.Fighter.Base),
                    Fixes.Fighter.Archetypes
                );
                SetttingUI.NestedSettingGroup("Hunter", TabLevel, Fixes.Hunter,
                    ("Base", Fixes.Hunter.Base),
                    Fixes.Hunter.Archetypes
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
                SetttingUI.NestedSettingGroup("Shaman", TabLevel, Fixes.Shaman,
                    ("Base", Fixes.Shaman.Base),
                    Fixes.Shaman.Archetypes
                );
                SetttingUI.NestedSettingGroup("Skald", TabLevel, Fixes.Skald,
                    ("Base", Fixes.Skald.Base),
                    Fixes.Skald.Archetypes
                );
                SetttingUI.NestedSettingGroup("Slayer", TabLevel, Fixes.Slayer,
                    ("Base", Fixes.Slayer.Base),
                    Fixes.Slayer.Archetypes
                );
                SetttingUI.NestedSettingGroup("Sorcerer", TabLevel, Fixes.Sorcerer,
                    ("Base", Fixes.Sorcerer.Base),
                    Fixes.Sorcerer.Archetypes
                );
                SetttingUI.NestedSettingGroup("Warpriest", TabLevel, Fixes.Warpriest,
                    ("Base", Fixes.Warpriest.Base),
                    Fixes.Warpriest.Archetypes
                );
                SetttingUI.NestedSettingGroup("Witch", TabLevel, Fixes.Witch,
                    ("Base", Fixes.Witch.Base),
                    Fixes.Witch.Archetypes
                );
                SetttingUI.SettingGroup("Hellknight", TabLevel, Fixes.Hellknight);
                SetttingUI.SettingGroup("Loremaster", TabLevel, Fixes.Loremaster);
                SetttingUI.SettingGroup("Winter Witch", TabLevel, Fixes.WinterWitch);
                SetttingUI.SettingGroup("Spells", TabLevel, Fixes.Spells);
                SetttingUI.SettingGroup("Bloodlines", TabLevel, Fixes.Bloodlines);
                SetttingUI.SettingGroup("Features", TabLevel, Fixes.Features);
                SetttingUI.SettingGroup("Feats", TabLevel, Fixes.Feats);
                SetttingUI.SettingGroup("Mythic Abilities", TabLevel, Fixes.MythicAbilities);
                SetttingUI.SettingGroup("Mythic Feats", TabLevel, Fixes.MythicFeats);
                SetttingUI.NestedSettingGroup("Units", TabLevel, Fixes.Units,
                    ("Companions", Fixes.Units.Companions),
                    ("NPCs", Fixes.Units.NPCs),
                    ("Bosses", Fixes.Units.Bosses),
                    ("Enemies", Fixes.Units.Enemies)
                );
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
        public static void AddedContent() {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var AddedContent = Main.TTTContext.AddedContent;
            UI.Div(0, 15);
            using (UI.VerticalScope()) {
                UI.Toggle("New Settings Off By Default".bold(), ref AddedContent.NewSettingsOffByDefault);
                UI.Space(25);

                SetttingUI.SettingGroup("Archetypes", TabLevel, AddedContent.Archetypes);
                SetttingUI.SettingGroup("Base Abilities", TabLevel, AddedContent.BaseAbilities);
                SetttingUI.SettingGroup("Bloodlines", TabLevel, AddedContent.Bloodlines);          
                SetttingUI.SettingGroup("Feats", TabLevel, AddedContent.Feats);
                SetttingUI.SettingGroup("Armor Mastery Feats", TabLevel, AddedContent.ArmorMasteryFeats);
                SetttingUI.SettingGroup("Shield Mastery Feats", TabLevel, AddedContent.ShieldMasteryFeats);
                SetttingUI.SettingGroup("Fighter Advanced Armor Training", TabLevel, AddedContent.FighterAdvancedArmorTraining);
                SetttingUI.SettingGroup("Fighter Advanced Weapon Training", TabLevel, AddedContent.FighterAdvancedWeaponTraining);
                SetttingUI.SettingGroup("Arcanist Exploits", TabLevel, AddedContent.ArcanistExploits);
                SetttingUI.SettingGroup("Magus Arcana", TabLevel, AddedContent.MagusArcana);
                SetttingUI.SettingGroup("Rogue Talents", TabLevel, AddedContent.RogueTalents);
                SetttingUI.SettingGroup("Wizard Arcane Discoveries", TabLevel, AddedContent.WizardArcaneDiscoveries);
                SetttingUI.SettingGroup("Races", TabLevel, AddedContent.Races);
                SetttingUI.SettingGroup("Backgrounds", TabLevel, AddedContent.Backgrounds);
                SetttingUI.SettingGroup("Spells", TabLevel, AddedContent.Spells);
                SetttingUI.SettingGroup("Mythic Abilities", TabLevel, AddedContent.MythicAbilities);
                SetttingUI.SettingGroup("Mythic Feats", TabLevel, AddedContent.MythicFeats);
            }
        }
    }
}
