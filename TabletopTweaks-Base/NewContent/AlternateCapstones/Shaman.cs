using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Shaman {
        public static BlueprintFeatureSelection ShamanAlternateCapstone = null;

        public static void AddAlternateCapstones() {
            var HexMastery = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "HexMastery", bp => {
                bp.SetName(TTTContext, "Hex Mastery");
                bp.SetDescription(TTTContext, "At 20th level, the shaman has learned ever more terrible hexes.\n" +
                    "She can select one grand hex from the list available to witches.");
            });
            ShamanAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ShamanAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.Ranks = 1;
                bp.IgnorePrerequisites = true;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(HexMastery, Generic.PerfectBodyFlawlessMindProgression, Generic.GreatBeastMasterFeature);
            });
        }
    }
}
