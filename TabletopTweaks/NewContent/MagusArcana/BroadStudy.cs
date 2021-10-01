using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MagusArcana {
    static class BroadStudy {
        public static void AddBroadStudy() {
            var MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var MagusArcanaSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");

            var BroadStudySelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("BroadStudySelection", bp => {
                bp.SetDescription("The magus selects another one of his spellcasting classes. The magus can use his spellstrike and " +
                    "spell combat abilities while casting or using spells from the spell list of that class. This does not allow him " +
                    "to cast arcane spells from that class’s spell list without suffering the normal chances of arcane spell failure, " +
                    "unless the spell lacks somatic components. ");
                bp.SetName("Broad Study");
                bp.Group = FeatureGroup.MagusArcana;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MagusArcana };
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = SpellTools.SpellCastingClasses.MagusClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 6;
                });
            });

            BroadStudySelection.AddFeatures(CreateAllBroadStudtFeatures(SpellTools.SpellCastingClasses.AllClasses, BroadStudySelection));

            BlueprintFeature[] CreateAllBroadStudtFeatures(BlueprintCharacterClass[] classes, BlueprintFeatureSelection selection) {
                return classes
                    .Where(c => c.AssetGuid != SpellTools.SpellCastingClasses.MagusClass.AssetGuid)
                    .Select(characterClass => {
                        var spellSecret = Helpers.CreateBlueprint<BlueprintFeature>($"BroadStudy{characterClass.name}", bp => {
                            bp.SetName($"Broad Study — {characterClass.Name}");
                            bp.m_Description = selection.m_Description;
                            bp.IsClassFeature = true;
                            bp.Groups = selection.Groups;
                            bp.HideNotAvailibleInUI = true;
                            bp.AddComponent<BroadStudyComponent>(c => {
                                c.CharacterClass = characterClass.ToReference<BlueprintCharacterClassReference>();
                            });
                            bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                                c.m_CharacterClass = characterClass.ToReference<BlueprintCharacterClassReference>();
                                c.RequiredSpellLevel = 1;
                                c.HideInUI = true;
                            });
                        });
                        return spellSecret;
                    }).ToArray();
            }

            if (ModSettings.AddedContent.MagusArcana.IsDisabled("BroadStudy")) { return; }
            FeatTools.AddAsMagusArcana(BroadStudySelection);
        }
    }
}
