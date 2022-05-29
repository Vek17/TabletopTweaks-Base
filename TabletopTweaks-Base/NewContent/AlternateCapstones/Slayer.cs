using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Slayer {
        public static BlueprintFeatureSelection SlayerAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var MasterSlayerFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("a26c0279a423fc94cabeea898f4d9f8a");
            var SlayerStudyTargetAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("b96d810ceb1708b4e895b695ddbb1813");
            var SlayerSwiftStudyTargetAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("90fdfeb61368f15488031bee72ab7c18");

            var AgainstTheOdds = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "AgainstTheOdds", bp => {
                bp.SetName(TTTContext, "Against the Odds");
                bp.SetDescription(TTTContext, "At 20th level, the slayer is used to fighting when the numbers are not in his favor.\n" +
                    "When the slayer uses studied target, he can study up to two additional foes within 30 feet in the same action.");
                bp.IsClassFeature = true;
                bp.AddComponent<DuplicateAbilityComponent>(c => {
                    c.Radius = 30.Feet();
                    c.AdditionalTargets = 2;
                    c.m_Abilities = new BlueprintAbilityReference[] {
                        SlayerStudyTargetAbility,
                        SlayerSwiftStudyTargetAbility
                    };
                });
            });
            SlayerAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "SlayerAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = MasterSlayerFeature.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    MasterSlayerFeature,
                    AgainstTheOdds,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
