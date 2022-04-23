using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Bloodrager {
        public static BlueprintFeatureSelection BloodragerAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var BloodragerMightyBloodrage = BlueprintTools.GetBlueprint<BlueprintFeature>("a6cd3eca05ee24840ab159ca47b4cd88");

            BloodragerAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "BloodragerAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = BloodragerMightyBloodrage.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(BloodragerMightyBloodrage, Generic.PerfectBodyFlawlessMindProgression, Generic.GreatBeastMasterFeature);
            });
        }
    }
}
