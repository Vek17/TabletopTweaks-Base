using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.Prerequisites;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Classes {
    static class Loremaster {

        private static readonly BlueprintFeatureSelection LoremasterClericSpellSecret = Resources.GetBlueprint<BlueprintFeatureSelection>("904ce918c85c9f947910340b956fb877");
        private static readonly BlueprintFeatureSelection LoremasterDruidSpellSecret = Resources.GetBlueprint<BlueprintFeatureSelection>("6b73ba9d8a718fb419a484c6e1b92c6d");
        private static readonly BlueprintFeatureSelection LoremasterWizardSpellSecret = Resources.GetBlueprint<BlueprintFeatureSelection>("f97986f19a595e2409cfe5d92bcf697c");

        public static void AddLoremasterFeatures() {

            CreateSpellSecretSelection(LoremasterClericSpellSecret);
            CreateSpellSecretSelection(LoremasterDruidSpellSecret);
            CreateSpellSecretSelection(LoremasterWizardSpellSecret);
            CreateSpellbookSelection();

            void CreateSpellSecretSelection(BlueprintFeatureSelection secret) {
                var name = $"{secret.name}_TTT";
                var spellSecret = Helpers.CreateBlueprint<BlueprintFeatureSelection>(name, bp => {
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
                return secretSelection.m_AllFeatures.Select(feature => {
                    var secret = feature.Get() as BlueprintParametrizedFeature;
                    var name = $"{secret.name}_TTT";
                    var spellSecret = Helpers.CreateBlueprint<BlueprintFeature>(name, bp => {
                        bp.SetName($"{secretSelection.Name} — {secret.Name}");
                        bp.m_Description = secretSelection.m_Description;
                        bp.IsClassFeature = true;
                        bp.Groups = secret.Groups;
                        bp.HideNotAvailibleInUI = true;
                        bp.AddComponent<AdditionalSpellSelection>(c => {
                            c.SpellCastingClass = secret.m_SpellcasterClass;
                            c.SpellList = secret.m_SpellList;
                            c.UseOffset = true;
                            c.Count = 1;
                        });
                        bp.AddComponent<PrerequisiteClassSpellLevel>(c => {
                            c.m_CharacterClass = secret.GetComponent<PrerequisiteFeaturesFromList>()
                                .m_Features
                                .First().Get()
                                .GetComponent<PrerequisiteClassSpellLevel>()
                                .m_CharacterClass; ;
                            c.RequiredSpellLevel = 1;
                            c.HideInUI = true;
                        });
                    });
                    return spellSecret;
                }).ToArray();
            }

            void CreateSpellbookSelection() {
                var spellbookSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("LoremasterSpellbookSelectionTTT", bp => {
                    bp.SetName("Spellbook Selection");
                    bp.SetDescription("When a new loremaster level is gained, the character gains new spells per day as if he had " +
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
                return Helpers.CreateBlueprint<BlueprintFeatureReplaceSpellbook>($"LoremasterSpellbook{spellbook.Get().name.Replace("Spellbook", "")}TTT", bp => {
                    bp.m_DisplayName = characterClass.LocalizedName;
                    bp.m_Description = selection.m_Description;
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
