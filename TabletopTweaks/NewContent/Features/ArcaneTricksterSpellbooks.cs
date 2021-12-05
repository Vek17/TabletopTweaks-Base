using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    static class ArcaneTricksterSpellbooks {
        public static void AddArcaneTricksterSpellbooks() {
            
            var ArcaneTricksterSpellbookSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ae04b7cdeb88b024b9fd3882cc7d3d76");

            var SkaldClass = Resources.GetBlueprint<BlueprintCharacterClass>("6afa347d804838b48bda16acb0573dc0");

            var ArcaneTricksterSkald = Helpers.CreateBlueprint<BlueprintFeatureReplaceSpellbook>("ArcaneTricksterSkald", bp => {
                bp.SetName("Skald");
                bp.m_Description = ArcaneTricksterSpellbookSelection.m_Description;
                bp.Groups = new FeatureGroup[] { FeatureGroup.ArcaneTricksterSpellbook };
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;
                bp.m_Spellbook = SpellTools.Spellbook.SkaldSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = SkaldClass.ToReference<BlueprintCharacterClassReference>();
                    c.RequiredSpellLevel = 2;
                });
            });



        }
    }
}