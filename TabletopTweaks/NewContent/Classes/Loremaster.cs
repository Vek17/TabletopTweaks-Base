using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using System.Linq;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
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
        }
    }
}
