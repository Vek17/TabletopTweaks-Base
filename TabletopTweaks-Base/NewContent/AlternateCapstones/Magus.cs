using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Magus {
        public static BlueprintFeatureSelection MagusAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var TrueMagusFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("789c7539ba659174db702e18d7c2d330");

            var LegendaryBlade = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "LegendaryBlade", bp => {
                bp.SetName(TTTContext, "Legendary Blade");
                bp.SetDescription(TTTContext, "At 20th level, the magus can turn his weapon into a thing of terror and wonder.\n" +
                    "When the magus enhances his weapon with his arcane pool, he grants it an additional +2 enhancement bonus (for a total of +7).");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
            });
            MagusAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "MagusAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = TrueMagusFeature.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    TrueMagusFeature,
                    //LegendaryBlade,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
