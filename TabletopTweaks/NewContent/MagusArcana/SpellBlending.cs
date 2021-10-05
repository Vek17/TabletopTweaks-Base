using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MagusArcana {
    static class SpellBlending {

        public static void AddSpellBlending() {
            var MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var MagusArcanaSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");

            var SpellBlendingSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("SpellBlendingSelection", bp => {
                bp.SetDescription("When a magus selects this arcana, he must select one spell from the wizard spell list that is of a " +
                    "magus spell level he can cast. He adds this spell to his spellbook and list of magus spells known as a magus spell" +
                    " of its wizard spell level. He can instead select two spells to add in this way, but both must be at least one level" +
                    " lower than the highest-level magus spell he can cast. " +
                    "\nA magus can select this magus arcana more than once.");
                bp.SetName("Spell Blending");
                bp.Group = FeatureGroup.MagusArcana;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MagusArcana };
                bp.IsClassFeature = true;
            });

            var SpellBlending1 = Helpers.CreateBlueprint<BlueprintFeature>("SpellBlending1", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 20;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.SetName("Spell Blending (Single)");
                bp.SetDescription("When a magus selects this arcana, he must select one spell from the wizard spell " +
                    "list that is of a magus spell level he can cast. He adds this spell to his spellbook and list of " +
                    "magus spells known as a magus spell of its wizard spell level.");
                bp.AddComponent<AdditionalSpellSelection>(c => {
                    c.m_SpellCastingClass = MagusClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_SpellList = SpellTools.SpellList.WizardSpellList.ToReference<BlueprintSpellListReference>();
                    c.UseOffset = true;
                    c.SpellLevelOffset = 0;
                    c.Count = 1;
                });
            });

            var SpellBlending2 = Helpers.CreateBlueprint<BlueprintFeature>("SpellBlending2", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 20;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.SetName("Spell Blending (Double)");
                bp.SetDescription("When a magus selects this arcana, he must select two spells from the wizard spell " +
                    "list that are one level lower of a magus spell level he can cast. He adds these spells to his spellbook and list of " +
                    "magus spells known as magus spells of thier wizard spell level.");
                bp.AddComponent<AdditionalSpellSelection>(c => {
                    c.m_SpellCastingClass = MagusClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_SpellList = SpellTools.SpellList.WizardSpellList.ToReference<BlueprintSpellListReference>();
                    c.UseOffset = true;
                    c.SpellLevelOffset = 1;
                    c.Count = 2;
                });
                bp.AddPrerequisite(Helpers.Create<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = MagusClass.ToReference<BlueprintCharacterClassReference>();
                    c.RequiredSpellLevel = 2;
                }));
            });

            SpellBlendingSelection.AddFeatures(SpellBlending1, SpellBlending2);

            if (ModSettings.AddedContent.MagusArcana.IsDisabled("SpellBlending")) { return; }
            FeatTools.AddAsMagusArcana(SpellBlendingSelection);
        }
    }
}
