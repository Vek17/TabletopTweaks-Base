using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Wizard {
        public static BlueprintFeatureSelection WizardAlternateCapstone = null;

        public static void AddAlternateCapstones() {
            var WellPrepared = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WellPrepared", bp => {
                bp.SetName(TTTContext, "Well-Prepared");
                bp.SetDescription(TTTContext, "At 20th level, the wizard has a spell for every occasion he can imagine—and a few he can’t.\n" +
                    "The wizard gains six additional 1st- and 2nd-level spell slots, four additional 3rd- and 4th-level spell slots, two additional 5th-level spell slots, and one additional 6th-level spell slot.");
                bp.AddComponent<AddSpellsPerDay>(c => {
                    c.Amount = 6;
                    c.Levels = new int[] {
                        1,
                        2
                    };
                });
                bp.AddComponent<AddSpellsPerDay>(c => {
                    c.Amount = 4;
                    c.Levels = new int[] {
                        3,
                        4
                    };
                });
                bp.AddComponent<AddSpellsPerDay>(c => {
                    c.Amount = 2;
                    c.Levels = new int[] {
                        5
                    };
                });
                bp.AddComponent<AddSpellsPerDay>(c => {
                    c.Amount = 1;
                    c.Levels = new int[] {
                        6
                    };
                });
            });
            WizardAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "WizardAlternateCapstone", bp => {
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
                bp.AddFeatures(WellPrepared, Generic.PerfectBodyFlawlessMindProgression, Generic.GreatBeastMasterFeature);
            });
        }
    }
}
