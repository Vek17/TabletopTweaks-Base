using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Features {
    static class DragonDiscipleSpellbooks {
        public static void AddDragonDiscipleSpellbooks() {
            var DragonDiscipleSpellbookSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8c1ba14c0b6dcdb439c56341385ee474");

            var SorcererClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("b3a505fb61437dc4097f43c3f8f9a4cf");
            var SageSorcererArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("00b990c8be2117e45ae6514ee4ef561c");
            var EmpyrealSorcererArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("aa00d945f7cf6c34c909a29a25f2df38");

            var ArcanistClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("52dbfd8505e22f84fad8d702611f60b7");
            var UnletteredArcanistArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("44f3ba33839a87f48a66b2b9b2f7c69b");
            var NatureMageArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("26185cfb81b34e778ad370407300de9a");

            var WitchClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("1b9873f1e7bfe5449bc84d03e9c8e3cc");
            var AccursedWitchArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("c5f6e53e71059fb4d802ce81a277a12d");

            var DragonDiscipleSageSorcerer = Helpers.CreateBlueprint<BlueprintFeatureReplaceSpellbook>(TTTContext, "DragonDiscipleSageSorcerer", bp => {
                bp.SetName(TTTContext, "Sorcerer");
                bp.m_Description = DragonDiscipleSpellbookSelection.m_Description;
                bp.Groups = new FeatureGroup[] { FeatureGroup.DragonDiscipleSpellbook };
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;
                bp.m_Spellbook = SpellTools.Spellbook.SageSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = SorcererClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = SageSorcererArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });
            var DragonDiscipleEmpyrealSorcerer = Helpers.CreateBlueprint<BlueprintFeatureReplaceSpellbook>(TTTContext, "DragonDiscipleEmpyrealSorcerer", bp => {
                bp.SetName(TTTContext, "Sorcerer");
                bp.m_Description = DragonDiscipleSpellbookSelection.m_Description;
                bp.Groups = new FeatureGroup[] { FeatureGroup.DragonDiscipleSpellbook };
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;
                bp.m_Spellbook = SpellTools.Spellbook.EmpyrealSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = SorcererClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = EmpyrealSorcererArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });
            var DragonDiscipleUnletteredArcanist = Helpers.CreateBlueprint<BlueprintFeatureReplaceSpellbook>(TTTContext, "DragonDiscipleUnletteredArcanist", bp => {
                bp.SetName(TTTContext, "Arcanist");
                bp.m_Description = DragonDiscipleSpellbookSelection.m_Description;
                bp.Groups = new FeatureGroup[] { FeatureGroup.DragonDiscipleSpellbook };
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;
                bp.m_Spellbook = SpellTools.Spellbook.UnletteredArcanistSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = ArcanistClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = UnletteredArcanistArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });
            var DragonDiscipleNatureMage = Helpers.CreateBlueprint<BlueprintFeatureReplaceSpellbook>(TTTContext, "DragonDiscipleNatureMage", bp => {
                bp.SetName(TTTContext, "Arcanist");
                bp.m_Description = DragonDiscipleSpellbookSelection.m_Description;
                bp.Groups = new FeatureGroup[] { FeatureGroup.DragonDiscipleSpellbook };
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;
                bp.m_Spellbook = SpellTools.Spellbook.NatureMageSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = ArcanistClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = NatureMageArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });
            var DragonDiscipleAccursedWitch = Helpers.CreateBlueprint<BlueprintFeatureReplaceSpellbook>(TTTContext, "DragonDiscipleAccursedWitch", bp => {
                bp.SetName(TTTContext, "Witch");
                bp.m_Description = DragonDiscipleSpellbookSelection.m_Description;
                bp.Groups = new FeatureGroup[] { FeatureGroup.DragonDiscipleSpellbook };
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;
                bp.m_Spellbook = SpellTools.Spellbook.AccursedWitchSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = ArcanistClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = AccursedWitchArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });
        }
    }
}
