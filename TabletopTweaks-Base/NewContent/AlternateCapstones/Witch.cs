using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Witch {
        public static BlueprintFeatureSelection WitchAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var WitchHexSelection = BlueprintTools.GetBlueprint<BlueprintFeature>("9846043cf51251a4897728ed6e24e76f");

            var WitchsDance = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WitchsDance", bp => {
                bp.SetName(TTTContext, "Witch's Dance");
                bp.SetDescription(TTTContext, "Once per day as a standard action, the witch can proclaim a celebration, " +
                    "leading her friends in a riotous and bloodthirsty dance. " +
                    "All allies within 30 feet gain the ability to fly (as per the flight hex) and a +2 dodge bonus to AC, " +
                    "and when they take a 5-foot step, they can instead move up to 10 feet instead of just 5 feet. " +
                    "The benefits remain for 1 round but can be extended with the cackle hex.");
                bp.IsClassFeature = true;
            });
            WitchAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "WitchAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = WitchHexSelection.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    //WitchHexSelection,
                    //WitchsDance,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
