using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Kineticist {
        public static BlueprintFeatureSelection KineticistAlternateCapstone = null;
        public static void AddAlternateCapstones() {

            var UnbridledPower = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "UnbridledPower", bp => {
                bp.SetName(TTTContext, "Unbridled Power");
                bp.SetDescription(TTTContext, "At 20th level, the kineticist wields her chosen element like a knife, cutting through all opposition.\n" +
                    "The kineticist chooses one blast. Her damage with that blast increases by 2d6+2 (for physical blasts) or by 2d6 (for energy blasts), " +
                    "and the blast ignores the first 10 points of damage reduction or energy resistance that the target has.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
            });
            KineticistAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "KineticistAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                //bp.m_Icon = WeaponMasterySelection.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    UnbridledPower,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
