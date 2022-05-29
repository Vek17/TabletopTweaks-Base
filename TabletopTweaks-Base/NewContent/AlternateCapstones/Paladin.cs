using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Paladin {
        public static BlueprintFeatureSelection PaladinAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var HolyChampion = BlueprintTools.GetBlueprint<BlueprintFeature>("eff3b63f744868845a2f511e9929f0de");

            var CrusaderChampion = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "CrusaderChampion", bp => {
                bp.SetName(TTTContext, "Crusader Champion");
                bp.SetDescription(TTTContext, "At 20th level, the paladin’s zeal is so inspiring that it affects all around her.\n" +
                    "The ranges of all of the paladin’s auras increase by 30 feet.");
                bp.IsClassFeature = true;
            });
            PaladinAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "PaladinAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = HolyChampion.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    HolyChampion,
                    //CrusaderChampion,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
