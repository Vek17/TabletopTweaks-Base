using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using System.Linq;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MagusArcana {
    static class BroadStudy {
        private static readonly BlueprintGuid BroadStudyMasterID = TTTContext.Blueprints.GetDerivedMaster("BroadStudyMasterID");

        public static void AddBroadStudy() {
            var MagusClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var MagusArcanaSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");

            var BroadStudySelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "BroadStudySelection", bp => {
                bp.SetName(TTTContext, "Broad Study");
                bp.SetDescription(TTTContext, "The magus selects another one of his spellcasting classes. The magus can use his spellstrike and " +
                    "spell combat abilities while casting or using spells from the spell list of that class. This does not allow him " +
                    "to cast arcane spells from that class’s spell list without suffering the normal chances of arcane spell failure, " +
                    "unless the spell lacks somatic components.");
                bp.Group = FeatureGroup.MagusArcana;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MagusArcana };
                bp.IsClassFeature = true;
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = SpellTools.SpellCastingClasses.MagusClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 6;
                });
            });

            BroadStudySelection.AddFeatures(CreateAllBroadStudyFeatures(SpellTools.SpellCastingClasses.AllClasses, BroadStudySelection));

            BlueprintFeature[] CreateAllBroadStudyFeatures(BlueprintCharacterClass[] classes, BlueprintFeatureSelection selection) {
                return classes
                    .Where(c => c.AssetGuid != SpellTools.SpellCastingClasses.MagusClass.AssetGuid)
                    .Select(characterClass => {
                        var spellSecret = Helpers.CreateDerivedBlueprint<BlueprintFeature>(TTTContext, $"BroadStudy{characterClass.name}",
                            BroadStudyMasterID,
                            new SimpleBlueprint[] { characterClass },
                            bp => {
                                bp.SetName(TTTContext, $"Broad Study — {characterClass.Name}");
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

            if (TTTContext.AddedContent.MagusArcana.IsDisabled("BroadStudy")) { return; }
            FeatTools.AddAsMagusArcana(BroadStudySelection);
        }
    }
}
