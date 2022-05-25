using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Kineticist {
        public static BlueprintFeatureSelection KineticistAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var KineticBlastFeature = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("93efbde2764b5504e98e6824cab3d27c");

            var UnbridledPower = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "UnbridledPower", bp => {
                bp.SetName(TTTContext, "Unbridled Power");
                bp.SetDescription(TTTContext, "At 20th level, the kineticist wields her chosen elements like a knife, cutting through all opposition.\n" +
                    "Her damage with her blasts increases by 2d6+2 (for physical blasts) or by 2d6 (for energy blasts).");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.AddComponent<AddFeatureIfHasFact>(c => {
                    c.m_CheckedFact = KineticBlastFeature;
                    c.m_Feature = KineticBlastFeature;
                });
                bp.AddComponent<AddFeatureIfHasFact>(c => {
                    c.m_CheckedFact = KineticBlastFeature;
                    c.m_Feature = KineticBlastFeature;
                });
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
