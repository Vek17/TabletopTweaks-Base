using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Sorcerer {
        public static BlueprintFeatureSelection SorcererAlternateCapstone = null;
        public static void AddAlternateCapstones() {

            var UniqueBloodline = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "UniqueBloodline", bp => {
                bp.SetName(TTTContext, "Unique Bloodline");
                bp.SetDescription(TTTContext, "At 20th level, the sorcerer’s blood grows wild and strange, become less about her ancestors and more about her specifically.\n" +
                    "The sorcerer selects a second bloodline and gains the bloodline arcana and the 1st-, 3rd-, and 9th-level bloodline powers. Her level for these powers is the same as for her primary bloodline.");
                bp.IsClassFeature = true;
            });
            SorcererAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "SorcererAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                //bp.m_Icon = MasterSlayerFeature.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(UniqueBloodline, Generic.PerfectBodyFlawlessMindProgression, Generic.GreatBeastMasterFeature);
            });
        }
    }
}
