using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Ranger {
        public static BlueprintFeatureSelection RangerAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var MasterHunter = BlueprintTools.GetBlueprint<BlueprintFeature>("9d53ef63441b5d84297587d75f72fc17");

            var SeenItBefore = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "SeenItBefore", bp => {
                bp.SetName(TTTContext, "Seen It Before");
                bp.SetDescription(TTTContext, "At 20th level, the ranger is wise to all the tricks of his prey.\n" +
                    "The ranger adds his favored enemy bonus as an insight bonus on saves against spells and abilities used by his favored enemies.");
                bp.IsClassFeature = true;
                bp.AddComponent<SavingThrowBonusAgainstFavoredEnemy>(c => {
                    c.Descriptor = ModifierDescriptor.FavoredEnemy;
                });
            });
            RangerAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "RangerAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = MasterHunter.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    MasterHunter,
                    SeenItBefore,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
