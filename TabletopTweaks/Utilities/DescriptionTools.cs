using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TabletopTweaks.Utilities {
    static class DescriptionTools {
        private static readonly EncyclopediaEntry[] EncyclopediaEntries = new EncyclopediaEntry[] {
            new EncyclopediaEntry {
                Entry = "Strength",
                Keywords = { "Strength" }
            },
            new EncyclopediaEntry {
                Entry = "Dexterity",
                Keywords = { "Dexterity" }
            },
            new EncyclopediaEntry {
                Entry = "Constitution",
                Keywords = { "Constitution" }
            },
            new EncyclopediaEntry {
                Entry = "Intelligence",
                Keywords = { "Intelligence" }
            },
            new EncyclopediaEntry {
                Entry = "Wisdom",
                Keywords = { "Wisdom" }
            },
            new EncyclopediaEntry {
                Entry = "Charisma",
                Keywords = { "Charisma" }
            },
            new EncyclopediaEntry {
                Entry = "Ability_Scores",
                Keywords = { "Ability Scores?" }
            },
            new EncyclopediaEntry {
                Entry = "Athletics",
                Keywords = { "Athletics" }
            },
            new EncyclopediaEntry {
                Entry = "Persuasion",
                Keywords = { "Persuasion" }
            },
            new EncyclopediaEntry {
                Entry = "Knowledge_World",
                Keywords = { @"Knowledge \(?World\)?" }
            },
            new EncyclopediaEntry {
                Entry = "Knowledge_Arcana",
                Keywords = { @"Knowledge \(?Arcana\)?" }
            },
            new EncyclopediaEntry {
                Entry = "Lore_Nature",
                Keywords = { @"Lore \(?Nature\)?" }
            },
            new EncyclopediaEntry {
                Entry = "Lore_Religion",
                Keywords = { @"Lore \(?Religion\)?" }
            },
            new EncyclopediaEntry {
                Entry = "Mobility",
                Keywords = { "Mobility" }
            },
            new EncyclopediaEntry {
                Entry = "Perception",
                Keywords = { "Perception" }
            },
            new EncyclopediaEntry {
                Entry = "Stealth",
                Keywords = { "Stealth" }
            },
            new EncyclopediaEntry {
                Entry = "Trickery",
                Keywords = { "Trickery" }
            },
            new EncyclopediaEntry {
                Entry = "Use_Magic_Device",
                Keywords = { 
                    "Use Magic Device",
                    "UMD"
                }
            },
            new EncyclopediaEntry {
                Entry = "Race",
                Keywords = { "Race" }
            },
            new EncyclopediaEntry {
                Entry = "Alignment",
                Keywords = { "Alignment" }
            },
            new EncyclopediaEntry {
                Entry = "Caster_Level",
                Keywords = {
                    "Caster Level",
                    "CL" 
                }
            },
            new EncyclopediaEntry {
                Entry = "DC",
                Keywords = { "DC" }
            },
            new EncyclopediaEntry {
                Entry = "Saving_Throw",
                Keywords = { "Saving Throw" }
            },
            new EncyclopediaEntry {
                Entry = "Spell_Resistance",
                Keywords = { "Spell Resistance" }
            },
            new EncyclopediaEntry {
                Entry = "Spell_Fail_Chance",
                Keywords = { "Arcane Spell Failure" }
            },
            new EncyclopediaEntry {
                Entry = "Concentration_Checks",
                Keywords = { "Concentration Checks?" }
            },
            new EncyclopediaEntry {
                Entry = "Concealment",
                Keywords = { "Concealment" }
            },
            new EncyclopediaEntry {
                Entry = "Bonus",
                Keywords = {"Bonus(es)?"}
            },
            new EncyclopediaEntry {
                Entry = "Speed",
                Keywords = { "Speed" }
            },
            new EncyclopediaEntry {
                Entry = "Flat_Footed_AC",
                Keywords = { 
                    "Flat Footed AC",
                    "Flat Footed Armor Class"
                }
            },
            new EncyclopediaEntry {
                Entry = "Flat_Footed",
                Keywords = { "Flat Footed" }
            },
            new EncyclopediaEntry {
                Entry = "Armor_Class",
                Keywords = { 
                    "Armor Class",
                    "AC" 
                }
            },
            new EncyclopediaEntry {
                Entry = "Armor_Check_Penalty",
                Keywords = { "Armor Check Penalty" }
            },
            new EncyclopediaEntry {
                Entry = "Damage_Reduction",
                Keywords = { "DR" }
            },
            new EncyclopediaEntry {
                Entry = "Free_Action",
                Keywords = { "Free Action" }
            },
            new EncyclopediaEntry {
                Entry = "Swift_Action",
                Keywords = { "Swift Action" }
            },
            new EncyclopediaEntry {
                Entry = "Standard_Actions",
                Keywords = { "Standard Action" }
            },
            new EncyclopediaEntry {
                Entry = "Full_Round_Action",
                Keywords = { "Full Round Action" }
            },
            new EncyclopediaEntry {
                Entry = "Skills",
                Keywords = { "Skills? Checks?" }
            },
            new EncyclopediaEntry {
                Entry = "Combat_Maneuvers",
                Keywords = { "Combat Maneuvers?" }
            },
            new EncyclopediaEntry {
                Entry = "CMB",
                Keywords = { 
                    "Combat Maneuver Bonus",
                    "CMB" 
                }
            },
            new EncyclopediaEntry {
                Entry = "CMD",
                Keywords = {
                    "Combat Maneuver Defense",
                    "CMD" 
                }
            },
            new EncyclopediaEntry {
                Entry = "BAB",
                Keywords = { 
                    "Base Attack Bonus",
                    "BAB" 
                }
            },
            new EncyclopediaEntry {
                Entry = "Incorporeal_Touch_Attack",
                Keywords = { "Incorporeal Touch Attacks?" }
            },
            new EncyclopediaEntry {
                Entry = "TouchAttack",
                Keywords = { "Touch Attacks?" }
            },
            new EncyclopediaEntry {
                Entry = "NaturalAttack",
                Keywords = { 
                    "Natural Attacks?",
                    "Natural Weapons?"
                }
            },
            new EncyclopediaEntry {
                Entry = "Attack_Of_Opportunity",
                Keywords = {
                    "Attacks? Of Opportunity",
                    "AoO"
                }
            },
            new EncyclopediaEntry {
                Entry = "Penalty",
                Keywords = { "Penalty" }
            },
            new EncyclopediaEntry {
                Entry = "Check",
                Keywords = { "Checks?" }
            },
            new EncyclopediaEntry {
                Entry = "Spells",
                Keywords = { "Spells?" }
            },
            new EncyclopediaEntry {
                Entry = "Attack",
                Keywords = { "Attacks?" }
            },
            new EncyclopediaEntry {
                Entry = "Feat",
                Keywords = { "Feats?" }
            },
            new EncyclopediaEntry {
                Entry = "Charge",
                Keywords = { "Charge" }
            },
            new EncyclopediaEntry {
                Entry = "Critical",
                Keywords = { "Critical Hit" }
            },
            new EncyclopediaEntry {
                Entry = "Fast_Healing",
                Keywords = { "Fast Healing" }
            },
            new EncyclopediaEntry {
                Entry = "Temporary_HP",
                Keywords = { "Temporary HP" }
            },
            new EncyclopediaEntry {
                Entry = "Flanking",
                Keywords = { 
                    "Flanking",
                    "Flanked"
                }
            },
            new EncyclopediaEntry {
                Entry = "Magic_School",
                Keywords = { "School of Magic" }
            },
            new EncyclopediaEntry {
                Entry = "Damage_Type",
                Keywords = { 
                    "Bludgeoning",
                    "Piercing",
                    "Slashing"
                }
            }
        };

        public static string TagEncyclopediaEntries(string description) {
            var result = description;
            result = result.StripHTML();
            foreach (var entry in EncyclopediaEntries) {
                foreach (var keyword in entry.Keywords) {
                    result = result.ApplyTags(keyword, entry);
                }
            }
            return result;
        }

        private class EncyclopediaEntry {
            public string Entry = "";
            public List<string> Keywords = new List<string>();

            public string Tag(string keyword) {
                return $"{{g|Encyclopedia:{Entry}}}{keyword}{{/g}}";
            }
        }

        private static string ApplyTags(this string str, string from, EncyclopediaEntry entry) {
            var pattern = from.EnforceSolo().ExcludeTagged();
            var matches = Regex.Matches(str, pattern, RegexOptions.IgnoreCase)
                .OfType<Match>()
                .Select(m => m.Value)
                .Distinct();
            foreach (string match in matches) {
                str = Regex.Replace(str, pattern, entry.Tag(match), RegexOptions.IgnoreCase);
            }
            return str;
        }
        private static string StripHTML(this string str) {
            return Regex.Replace(str, "<.*?>", string.Empty);
        }
        private static string ExcludeTagged(this string str) {
            return $"{@"(?<!{g\|Encyclopedia:\w+}[^}]*)"}{str}{@"(?![^{]*{\/g})"}";
        }
        private static string EnforceSolo(this string str) {
            return $"{@"(?<![\w>]+)"}{str}{@"(?![^\s\.,""'<)]+)"}";
        }
    }
}
