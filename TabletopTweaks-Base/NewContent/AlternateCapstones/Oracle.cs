using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Oracle {
        public static BlueprintFeatureSelection OracleAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var OracleFinalRevelation = BlueprintTools.GetBlueprint<BlueprintFeature>("0336dc22538ba5f42b73da4fb3f50849");
            var OracleRevelationSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("60008a10ad7ad6543b1f63016741a5d2");
            var OracleMysterySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("5531b975dcdf0e24c98f1ff7e017e741");

            var OracleFinalRevelationSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "OracleFinalRevelationSelection", bp => {
                bp.SetName(OracleFinalRevelation.m_DisplayName);
                bp.SetDescription(OracleFinalRevelation.m_Description);
                bp.IsClassFeature = true;
                bp.m_Icon = OracleFinalRevelation.Icon;
                bp.Ranks = 1;
                bp.AddFeatures(
                    OracleMysterySelection.AllFeatures
                        .Select(mystery => mystery.GetComponent<AddFeatureOnClassLevel>(c => c.Level == 20).Feature)
                        .ToArray()
                );
            });
            var OracleFinalRevelationProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "OracleFinalRevelationProgression", bp => {
                bp.SetName(OracleFinalRevelation.m_DisplayName);
                bp.SetDescription(OracleFinalRevelation.m_Description);
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
                    Helpers.CreateLevelEntry(20, OracleFinalRevelationSelection)
                };
                bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                    c.Not = true;
                });
            });
            var DiverseMysteriesRevelationSelection = OracleRevelationSelection.CreateCopy(TTTContext, "DiverseMysteriesRevelationSelection");
            var DiverseMysteries = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "DiverseMysteries", bp => {
                bp.SetName(TTTContext, "Diverse Mysteries");
                bp.SetDescription(TTTContext, "At 20th level, the oracle knows that sometimes she needs different tools to do her patron’s work.\n" +
                    "The oracle can select two revelations from another mystery. She must meet the prerequisites for these revelations.");
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
                    Helpers.CreateLevelEntry(19, DiverseMysteriesRevelationSelection),
                    Helpers.CreateLevelEntry(20, DiverseMysteriesRevelationSelection)
                };
                bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                    c.Not = true;
                });
            });
            OracleAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "OracleAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = OracleFinalRevelation.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    OracleFinalRevelationProgression,
                    DiverseMysteries,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
