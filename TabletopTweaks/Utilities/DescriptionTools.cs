using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TabletopTweaks.Utilities {
    static class DescriptionTools {
        private static readonly EncyclopediaEntry[] EncyclopediaEntries = new EncyclopediaEntry[] {
            new EncyclopediaEntry {
                Entry = "Strength",
                Patterns = { "Strength" }
            },
            new EncyclopediaEntry {
                Entry = "Dexterity",
                Patterns = { "Dexterity" }
            },
            new EncyclopediaEntry {
                Entry = "Constitution",
                Patterns = { "Constitution" }
            },
            new EncyclopediaEntry {
                Entry = "Intelligence",
                Patterns = { "Intelligence" }
            },
            new EncyclopediaEntry {
                Entry = "Wisdom",
                Patterns = { "Wisdom" }
            },
            new EncyclopediaEntry {
                Entry = "Charisma",
                Patterns = { "Charisma" }
            },
            new EncyclopediaEntry {
                Entry = "Ability_Scores",
                Patterns = { "Ability Scores?" }
            },
            new EncyclopediaEntry {
                Entry = "Athletics",
                Patterns = { "Athletics" }
            },
            new EncyclopediaEntry {
                Entry = "Persuasion",
                Patterns = { "Persuasion" }
            },
            new EncyclopediaEntry {
                Entry = "Knowledge_World",
                Patterns = { @"Knowledge \(?World\)?" }
            },
            new EncyclopediaEntry {
                Entry = "Knowledge_Arcana",
                Patterns = { @"Knowledge \(?Arcana\)?" }
            },
            new EncyclopediaEntry {
                Entry = "Lore_Nature",
                Patterns = { @"Lore \(?Nature\)?" }
            },
            new EncyclopediaEntry {
                Entry = "Lore_Religion",
                Patterns = { @"Lore \(?Religion\)?" }
            },
            new EncyclopediaEntry {
                Entry = "Mobility",
                Patterns = { "Mobility" }
            },
            new EncyclopediaEntry {
                Entry = "Perception",
                Patterns = { "Perception" }
            },
            new EncyclopediaEntry {
                Entry = "Stealth",
                Patterns = { "Stealth" }
            },
            new EncyclopediaEntry {
                Entry = "Trickery",
                Patterns = { "Trickery" }
            },
            new EncyclopediaEntry {
                Entry = "Use_Magic_Device",
                Patterns = {
                    "Use Magic Device",
                    "UMD"
                }
            },
            new EncyclopediaEntry {
                Entry = "Race",
                Patterns = { "Race" }
            },
            new EncyclopediaEntry {
                Entry = "Alignment",
                Patterns = { "Alignment" }
            },
            new EncyclopediaEntry {
                Entry = "Caster_Level",
                Patterns = {
                    "Caster Level",
                    "CL"
                }
            },
            new EncyclopediaEntry {
                Entry = "DC",
                Patterns = { "DC" }
            },
            new EncyclopediaEntry {
                Entry = "Saving_Throw",
                Patterns = { "Saving Throw" }
            },
            new EncyclopediaEntry {
                Entry = "Spell_Resistance",
                Patterns = { "Spell Resistance" }
            },
            new EncyclopediaEntry {
                Entry = "Spell_Fail_Chance",
                Patterns = { "Arcane Spell Failure" }
            },
            new EncyclopediaEntry {
                Entry = "Concentration_Checks",
                Patterns = { "Concentration Checks?" }
            },
            new EncyclopediaEntry {
                Entry = "Concealment",
                Patterns = { "Concealment" }
            },
            new EncyclopediaEntry {
                Entry = "Bonus",
                Patterns = {"Bonus(es)?"}
            },
            new EncyclopediaEntry {
                Entry = "Speed",
                Patterns = { "Speed" }
            },
            new EncyclopediaEntry {
                Entry = "Flat_Footed_AC",
                Patterns = {
                    "Flat Footed AC",
                    "Flat Footed Armor Class"
                }
            },
            new EncyclopediaEntry {
                Entry = "Flat_Footed",
                Patterns = {
                    "Flat Footed",
                    "Flat-Footed"
                }
            },
            new EncyclopediaEntry {
                Entry = "Armor_Class",
                Patterns = {
                    "Armor Class",
                    "AC"
                }
            },
            new EncyclopediaEntry {
                Entry = "Armor_Check_Penalty",
                Patterns = { "Armor Check Penalty" }
            },
            new EncyclopediaEntry {
                Entry = "Damage_Reduction",
                Patterns = { "DR" }
            },
            new EncyclopediaEntry {
                Entry = "Free_Action",
                Patterns = { "Free Action" }
            },
            new EncyclopediaEntry {
                Entry = "Swift_Action",
                Patterns = { "Swift Action" }
            },
            new EncyclopediaEntry {
                Entry = "Standard_Actions",
                Patterns = { "Standard Action" }
            },
            new EncyclopediaEntry {
                Entry = "Full_Round_Action",
                Patterns = { "Full Round Action" }
            },
            new EncyclopediaEntry {
                Entry = "Skills",
                Patterns = { "Skills? Checks?" }
            },
            new EncyclopediaEntry {
                Entry = "Combat_Maneuvers",
                Patterns = { "Combat Maneuvers?" }
            },
            new EncyclopediaEntry {
                Entry = "CMB",
                Patterns = {
                    "Combat Maneuver Bonus",
                    "CMB"
                }
            },
            new EncyclopediaEntry {
                Entry = "CMD",
                Patterns = {
                    "Combat Maneuver Defense",
                    "CMD"
                }
            },
            new EncyclopediaEntry {
                Entry = "BAB",
                Patterns = {
                    "Base Attack Bonus",
                    "BAB"
                }
            },
            new EncyclopediaEntry {
                Entry = "Incorporeal_Touch_Attack",
                Patterns = { "Incorporeal Touch Attacks?" }
            },
            new EncyclopediaEntry {
                Entry = "TouchAttack",
                Patterns = { "Touch Attacks?" }
            },
            new EncyclopediaEntry {
                Entry = "NaturalAttack",
                Patterns = {
                    "Natural Attacks?",
                    "Natural Weapons?"
                }
            },
            new EncyclopediaEntry {
                Entry = "Attack_Of_Opportunity",
                Patterns = {
                    "Attacks? Of Opportunity",
                    "AoO"
                }
            },
            new EncyclopediaEntry {
                Entry = "Penalty",
                Patterns = { "Penalty" }
            },
            new EncyclopediaEntry {
                Entry = "Check",
                Patterns = { "Checks?" }
            },
            new EncyclopediaEntry {
                Entry = "Spells",
                Patterns = { "Spells?" }
            },
            new EncyclopediaEntry {
                Entry = "Attack",
                Patterns = { "Attacks?" }
            },
            new EncyclopediaEntry {
                Entry = "Feat",
                Patterns = { "Feats?" }
            },
            new EncyclopediaEntry {
                Entry = "Charge",
                Patterns = { "Charge" }
            },
            new EncyclopediaEntry {
                Entry = "Critical",
                Patterns = { "Critical Hit" }
            },
            new EncyclopediaEntry {
                Entry = "Fast_Healing",
                Patterns = { "Fast Healing" }
            },
            new EncyclopediaEntry {
                Entry = "Temporary_HP",
                Patterns = { "Temporary HP" }
            },
            new EncyclopediaEntry {
                Entry = "Flanking",
                Patterns = {
                    "Flanking",
                    "Flanked"
                }
            },
            new EncyclopediaEntry {
                Entry = "Magic_School",
                Patterns = { "School of Magic" }
            },
            new EncyclopediaEntry {
                Entry = "Damage_Type",
                Patterns = {
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
                foreach (var pattern in entry.Patterns) {
                    result = result.ApplyTags(pattern, entry);
                }
            }
            return result;
        }

        private class EncyclopediaEntry {
            public string Entry = "";
            public List<string> Patterns = new List<string>();

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
                str = Regex.Replace(str, Regex.Escape(match).EnforceSolo().ExcludeTagged(), entry.Tag(match), RegexOptions.IgnoreCase);
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
