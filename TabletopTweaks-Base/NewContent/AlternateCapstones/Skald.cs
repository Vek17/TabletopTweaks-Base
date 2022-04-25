using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Skald {
        public static BlueprintFeatureSelection SkaldAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var MasterSkald = BlueprintTools.GetBlueprint<BlueprintFeature>("ae4d45a39a91dee4fb4200d7a677d9a7");

            var GreatKenning = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "GreatKenning", bp => {
                bp.SetName(TTTContext, "Great Kenning");
                bp.SetDescription(TTTContext, "At 20th level, the skald’s knowledge of other magic grows ever wider.\n" +
                    "The skald can use spell kenning three additional times per day and can select one additional spell list from which he can cast spells with spell kenning.");
                bp.IsClassFeature = true;
            });
            SkaldAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "SkaldAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = MasterSkald.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(MasterSkald, GreatKenning, Generic.PerfectBodyFlawlessMindProgression, Generic.GreatBeastMasterFeature);
            });
        }
    }
}
