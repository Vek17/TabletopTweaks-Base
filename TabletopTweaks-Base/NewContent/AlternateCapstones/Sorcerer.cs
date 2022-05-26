using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using System.Collections.Generic;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Sorcerer {
        public static BlueprintFeatureSelection SorcererAlternateCapstone = null;
        public static void AddAlternateCapstones() {

            var BloodlineAscendance = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19");
            var UniqueBloodline = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "UniqueBloodline", bp => {
                bp.SetName(TTTContext, "Unique Bloodline");
                bp.SetDescription(TTTContext, "At 20th level, the sorcerer’s blood grows wild and strange, become less about her ancestors and more about her specifically.\n" +
                    "The sorcerer selects a second bloodline and gains the bloodline arcana and the 1st-, 3rd-, and 9th-level bloodline powers. Her level for these powers is the same as for her primary bloodline.");
                bp.IsClassFeature = true;
            });
            var BloodlineCapstoneSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "BloodlineCapstoneSelection", bp => {
                bp.SetName(TTTContext, "Bloodline Capstone");
                bp.SetDescription(TTTContext, "At 20th level, the sorcerer gains the final power of thier bloodline.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddFeatures(BloodlineAscendance.AllFeatures);
            });
            var BloodlineCapstoneProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "BloodlineCapstoneProgression", bp => {
                bp.SetName(BloodlineCapstoneSelection.m_DisplayName);
                bp.SetDescription(BloodlineCapstoneSelection.m_Description);
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.HideNotAvailibleInUI = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[0];
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(20, BloodlineCapstoneSelection)
                };
                bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                    c.Not = true;
                });
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
                bp.AddFeatures(
                    //UniqueBloodline, 
                    BloodlineCapstoneProgression,
                    Generic.PerfectBodyFlawlessMindProgression, 
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
