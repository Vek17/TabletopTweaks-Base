using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Classes {
    static class Loremaster {

        private static readonly BlueprintFeatureSelection LoremasterClericSpellSecret = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("904ce918c85c9f947910340b956fb877");
        private static readonly BlueprintFeatureSelection LoremasterDruidSpellSecret = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("6b73ba9d8a718fb419a484c6e1b92c6d");
        private static readonly BlueprintFeatureSelection LoremasterWizardSpellSecret = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("f97986f19a595e2409cfe5d92bcf697c");
        private static readonly BlueprintGuid LoremasterSpellbookMasterID = TTTContext.Blueprints.GetDerivedMaster("LoremasterSpellbookMasterID");

        public static void AddLoremasterFeatures() {

            CreateSpellSecretSelection(LoremasterClericSpellSecret);
            CreateSpellSecretSelection(LoremasterDruidSpellSecret);
            CreateSpellSecretSelection(LoremasterWizardSpellSecret);
            CreateSpellbookSelection();

            void CreateSpellSecretSelection(BlueprintFeatureSelection secret) {
                var name = $"{secret.name}_TTT";
                var spellSecret = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, name, bp => {
                    bp.m_DisplayName = secret.m_DisplayName;
                    bp.m_Description = secret.m_Description;
                    bp.IsClassFeature = true;
                    bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                    bp.Mode = secret.Mode;
                    bp.HideNotAvailibleInUI = secret.HideNotAvailibleInUI;
                    bp.AddComponents(secret.GetComponents<Prerequisite>().ToArray());
                    bp.AddPrerequisite<PrerequisiteNoFeature>(c => {
                        c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                    });
                });
                spellSecret.AddFeatures(CreateSpellSecretClasses(secret));
            }
            BlueprintFeature[] CreateSpellSecretClasses(BlueprintFeatureSelection secretSelection) {
                var secret = secretSelection.m_AllFeatures.First().Get() as BlueprintParametrizedFeature;
                return SpellTools.SpellCastingClasses.AllClasses.Select(castingClass => {
                    var name = $"{secretSelection.name.Replace("Selection", "").Replace("Spell", "")}{castingClass.name.Replace("Class", "")}_TTT";
                    if (Regex.Matches(name, "Cleric").Count > 1 || Regex.Matches(name, "Druid").Count > 1) {
                        return null;
                    }
                    var spellSecret = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, name, bp => {
                        bp.SetName(TTTContext, $"{secretSelection.Name} — {castingClass.Name}");
                        bp.SetDescription(secretSelection.m_Description);
                        bp.IsClassFeature = true;
                        bp.Groups = secret.Groups;
                        bp.HideNotAvailibleInUI = true;
                        bp.AddComponent<AdditionalSpellSelection>(c => {
                            c.m_SpellCastingClass = castingClass.ToReference<BlueprintCharacterClassReference>();
                            c.m_SpellList = secret.m_SpellList;
                            c.UseOffset = true;
                            c.Count = 1;
                        });
                        bp.AddComponent<PrerequisiteClassSpellLevel>(c => {
                            c.m_CharacterClass = castingClass.ToReference<BlueprintCharacterClassReference>();
                            c.RequiredSpellLevel = 1;
                            c.HideInUI = true;
                        });
                    });
                    return spellSecret;
                }).Where(secret => secret != null).ToArray();
            }

            void CreateSpellbookSelection() {
                var spellbookSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "LoremasterSpellbookSelectionTTT", bp => {
                    bp.SetName(TTTContext, "Spellbook Selection");
                    bp.SetDescription(TTTContext, "When a new loremaster level is gained, the character gains new spells per day as if he had " +
                        "also gained a level in a spellcasting class he belonged to before adding the prestige class. He does not, however, gain other benefits " +
                        "a character of that class would have gained, except for additional spells per day, spells known (if he is a spontaneous spellcaster), " +
                        "and an increased effective level of spellcasting. If a character had more than one spellcasting class before becoming a loremaster, " +
                        "he must decide to which class he adds the new level for purposes of determining spells per day.");
                    bp.IsClassFeature = true;
                    bp.Obligatory = true;
                    bp.Ranks = 1;
                    bp.Group = FeatureGroup.ArcaneTricksterSpellbook;
                    bp.Groups = new FeatureGroup[] { FeatureGroup.ReplaceSpellbook, FeatureGroup.ArcaneTricksterSpellbook };
                });
                SpellTools.SpellCastingClasses.AllClasses.ForEach(castingClass => {
                    spellbookSelection.AddFeatures(CreateSpellbookReplacements(castingClass, spellbookSelection));
                });
                spellbookSelection.AddFeatures(CreateSpellbookReplacementsThassilonian(spellbookSelection));
            }
            BlueprintFeatureReplaceSpellbook[] CreateSpellbookReplacements(BlueprintCharacterClass characterClass, BlueprintFeatureSelection selection) {
                List<BlueprintSpellbookReference> spellbooks = characterClass
                    .m_Archetypes
                    .Select(archetype => archetype.Get().m_ReplaceSpellbook)
                    .Append(characterClass.m_Spellbook)
                    .Where(spellbook => spellbook?.Get() != null)
                    .Distinct()
                    .ToList();
                return spellbooks.Select(spellbook => CreateSpellbookReplacement(spellbook, characterClass, selection)).ToArray();
            }
            BlueprintFeatureReplaceSpellbook[] CreateSpellbookReplacementsThassilonian(BlueprintFeatureSelection selection) {
                List<BlueprintSpellbookReference> spellbooks = new List<BlueprintSpellbookReference>() {
                    SpellTools.Spellbook.ThassilonianAbjurationSpellbook.ToReference<BlueprintSpellbookReference>(),
                    SpellTools.Spellbook.ThassilonianConjurationSpellbook.ToReference<BlueprintSpellbookReference>(),
                    SpellTools.Spellbook.ThassilonianEnchantmentSpellbook.ToReference<BlueprintSpellbookReference>(),
                    SpellTools.Spellbook.ThassilonianEvocationSpellbook.ToReference<BlueprintSpellbookReference>(),
                    SpellTools.Spellbook.ThassilonianIllusionSpellbook.ToReference<BlueprintSpellbookReference>(),
                    SpellTools.Spellbook.ThassilonianNecromancySpellbook.ToReference<BlueprintSpellbookReference>(),
                    SpellTools.Spellbook.ThassilonianTransmutationSpellbook.ToReference<BlueprintSpellbookReference>()
                };
                return spellbooks.Select(spellbook => CreateSpellbookReplacement(spellbook, SpellTools.SpellCastingClasses.WizardClass, selection)).ToArray();
            }
            BlueprintFeatureReplaceSpellbook CreateSpellbookReplacement(BlueprintSpellbookReference spellbook, BlueprintCharacterClass characterClass, BlueprintFeatureSelection selection) {
                return Helpers.CreateDerivedBlueprint<BlueprintFeatureReplaceSpellbook>(TTTContext, $"LoremasterSpellbook{spellbook.Get().name.Replace("Spellbook", "")}TTT",
                    LoremasterSpellbookMasterID,
                    new SimpleBlueprint[] { spellbook },
                    bp => {
                        bp.SetName(characterClass.LocalizedName);
                        bp.SetDescription(selection.m_Description);
                        bp.IsClassFeature = true;
                        bp.m_Spellbook = spellbook;
                        bp.Groups = new FeatureGroup[] { FeatureGroup.ArcaneTricksterSpellbook };
                        bp.HideInUI = true;
                        bp.HideNotAvailibleInUI = true;
                        bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                            c.m_CharacterClass = characterClass.ToReference<BlueprintCharacterClassReference>();
                            c.RequiredSpellLevel = 3;
                        });
                        bp.AddPrerequisite<PrerequisiteSpellbook>(c => {
                            c.Spellbook = spellbook;
                            c.RequiredSpellLevel = 3;
                            c.HideInUI = true;
                        });
                    });
            }
        }
    }
}
