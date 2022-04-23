using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Cavalier {
        public static BlueprintFeatureSelection CavalierAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var CavalierSupremeCharge = BlueprintTools.GetBlueprint<BlueprintFeature>("77af3c58e71118d4481c50694bd99e77");

            CavalierAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "CavalierAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = CavalierSupremeCharge.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(CavalierSupremeCharge, Generic.PerfectBodyFlawlessMindProgression, Generic.GreatBeastMasterFeature);
            });
        }
    }
}
