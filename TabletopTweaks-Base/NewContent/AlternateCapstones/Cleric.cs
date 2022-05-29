using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Cleric {
        public static BlueprintFeatureSelection ClericAlternateCapstone = null;

        public static void AddAlternateCapstones() {
            var SecondDomainsSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("43281c3d7fe18cc4d91928395837cd1e");

            var Proxy = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "Proxy", bp => {
                bp.SetName(TTTContext, "Proxy");
                bp.SetDescription(TTTContext, "At 20th level, the cleric forges a direct, personal connection to her deity.\n" +
                    "She can select an additional domain from the list offered by her deity.");
                bp.IsClassFeature = true;
                bp.m_Features = SecondDomainsSelection.m_Features;
                bp.m_AllFeatures = SecondDomainsSelection.m_AllFeatures;
                bp.Group = FeatureGroup.None;
                bp.m_Icon = SecondDomainsSelection.m_Icon;
            });
            ClericAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ClericAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.Ranks = 1;
                bp.IgnorePrerequisites = true;
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    Proxy,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
