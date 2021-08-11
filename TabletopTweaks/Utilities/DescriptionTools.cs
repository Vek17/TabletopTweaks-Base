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
                    @"(?<!\w+)UMD(?![^\s\.{]+)"
                }
            },
            new EncyclopediaEntry {
                Entry = "Race",
                Keywords = { @"(?<!\w+)Race(?![^\s\.{]+)" }
            },
            new EncyclopediaEntry {
                Entry = "Caster_Level",
                Keywords = {
                    "Caster Level",
                    @"(?<!\w+)CL(?![^\s\.]+)" 
                }
            },
            new EncyclopediaEntry {
                Entry = "DC",
                Keywords = { @"(?<!\w+)DC(?![^\s\.{]+)" }
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
                Keywords = { @"(?<!\w+)Bonus(?![^\s\.{]+)" }
            },
            
            new EncyclopediaEntry {
                Entry = "Speed",
                Keywords = { @"(?<!\w+)Speed(?![^\s\.{]+)" }
            },
            new EncyclopediaEntry {
                Entry = "Armor_Class",
                Keywords = { 
                    "Armor Class",
                    @"(?<!\w+)AC(?![^\s\.]+)" 
                }
            },
            new EncyclopediaEntry {
                Entry = "Armor_Check_Penalty",
                Keywords = { "Armor Check Penalty" }
            },
            new EncyclopediaEntry {
                Entry = "Damage_Reduction",
                Keywords = { @"(?<!\w+)DR(?![^\s\.{]+)" }
            },
            new EncyclopediaEntry {
                Entry = "Penalty",
                Keywords = { "Penalty" }
            }
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
            var matches = Regex.Matches(str, from, RegexOptions.IgnoreCase)
                .OfType<Match>()
                .Select(m => m.Value)
                .Distinct();
            foreach (string match in matches) {
                str = str.Replace(match, entry.Tag(match));
            }
            return str;
        }
    }
}
