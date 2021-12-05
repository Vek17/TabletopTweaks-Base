using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class EldritchKnightSpellbooks {
        public static void AddEldritchKnightSpellbooks() {

            var EldritchKnightSpellbookSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("dc3ab8d0484467a4787979d93114ebc3");

            var SkaldClass = Resources.GetBlueprint<BlueprintCharacterClass>("6afa347d804838b48bda16acb0573dc0");

            var EldritchKnightSkald = Helpers.CreateBlueprint<BlueprintFeatureReplaceSpellbook>("EldritchKnightSkald", bp => {
                bp.SetName("Skald");
                bp.m_Description = EldritchKnightSpellbookSelection.m_Description;
                bp.Groups = new FeatureGroup[] { FeatureGroup.EldritchKnightSpellbook };
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideNotAvailibleInUI = true;
                bp.m_Spellbook = SpellTools.Spellbook.SkaldSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = SkaldClass.ToReference<BlueprintCharacterClassReference>();
                    c.RequiredSpellLevel = 3;
                });
            });

        }
    }
}
