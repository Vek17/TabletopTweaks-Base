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
                Entry = "Athletics",
                Keywords = { "Athletics" }
            },
            new EncyclopediaEntry {
                Entry = "Persuasion",
                Keywords = { "Persuasion" }
            },
            new EncyclopediaEntry {
                Entry = "Knowledge_World",
                Keywords = { @"Knowledge \(*World\)*" }
            },
            new EncyclopediaEntry {
                Entry = "Knowledge_Arcana",
                Keywords = { @"Knowledge \(*Arcana\)*" }
            },
            new EncyclopediaEntry {
                Entry = "Lore_Nature",
                Keywords = { @"Lore \(*Nature\)*" }
            },
            new EncyclopediaEntry {
                Entry = "Lore_Religion",
                Keywords = { @"Lore \(*Religion\)*" }
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
                Entry = "Bonus",
                Keywords = { 
                    "Bonus",
                    "Bonuses"
                }
            },
            new EncyclopediaEntry {
                Entry = "Speed",
                Keywords = { "Speed" }
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
                Entry = "Skills",
                Keywords = {
                    "Skill Check",
                    "Skills Check",
                    "Skill Checks"
                }
            },
            new EncyclopediaEntry {
                Entry = "Concentration_Checks",
                Keywords = {
                    "Concentration Check",
                    "Concentration Checks"
                }
            },
            new EncyclopediaEntry {
                Entry = "Combat_Maneuvers",
                Keywords = {
                    "Combat Maneuvers",
                    "Combat Maneuver"
                }
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
                Entry = "Penalty",
                Keywords = { "Penalty" }
            },
            new EncyclopediaEntry {
                Entry = "Check",
                Keywords = {
                    "Check",
                    "Checks"
                }
            },
        };

        public static string TagEncyclopediaEntries(string description) {
            var result = description;
            foreach (var entry in EncyclopediaEntries) {
                foreach (var keyword in entry.Keywords) {
                    result = result.ApplyTags(keyword, entry);
                }
            }
            return result;
        }

        class EncyclopediaEntry {
            public string Entry = "";
            public List<string> Keywords = new List<string>();

            public string Tag(string keyword) {
                return $"{{g|Encyclopedia:{Entry}}}{keyword}{{/g}}";
            }
        }

        static private string ApplyTags(this string str, string from, EncyclopediaEntry entry) {
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
        static private string ExcludeTagged(this string str) {
            return $"{@"(?<!{g\|Encyclopedia:\w+}[^}]*)"}{str}{@"(?![^{]*{\/g})"}";
        }
        static private string EnforceSolo(this string str) {
            return $"{@"(?<!\w+)"}{str}{@"(?![^\s\.,""']+)"}";
        }
    }
}
