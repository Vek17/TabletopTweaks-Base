using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Warpriest {
        public static BlueprintFeatureSelection WarpriestAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var WarpriestAspectOfWar = BlueprintTools.GetBlueprint<BlueprintFeature>("65cc7abc21826a344aa156e2a40dcecc");

            var HammerOfGod = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "HammerOfGod", bp => {
                bp.SetName(TTTContext, "Hammer of God");
                bp.SetDescription(TTTContext, "At 20th level, the warpriest has become one of his deity’s favorite weapons—the first tool that comes to hand when destruction is called for.\n" +
                    "The warpriest gains two additional blessings from the list offered by his deity. He can also call upon his blessings two more times each day.");
                bp.IsClassFeature = true;
            });
            WarpriestAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "WarpriestAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = WarpriestAspectOfWar.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    WarpriestAspectOfWar, 
                    HammerOfGod, 
                    Generic.PerfectBodyFlawlessMindProgression, 
                    Generic.GreatBeastMasterFeature, 
                    Generic.OldDogNewTricksProgression
                );
            });
        }
    }
}
