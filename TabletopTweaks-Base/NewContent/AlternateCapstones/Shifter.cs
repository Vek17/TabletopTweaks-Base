using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using System.Collections.Generic;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Shifter {
        public static BlueprintFeatureSelection ShifterAlternateCapstone = null;
        public static BlueprintProgression FinalShifterAspectProgression = null;

        public static void AddAlternateCapstones() {
            var FinalShifterAspectFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("5a155f5c3f834a319feab52dc66ee185");
            var ShifterAspectSelectionFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("121829d239124685b430f5263031bf83");

            FinalShifterAspectProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "FinalShifterAspectProgression", bp => {
                bp.m_DisplayName = FinalShifterAspectFeature.m_DisplayName;
                bp.m_Description = FinalShifterAspectFeature.m_Description;
                bp.m_Icon = FinalShifterAspectFeature.Icon;
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
                    Helpers.CreateLevelEntry(20,
                        FinalShifterAspectFeature,
                        ShifterAspectSelectionFeature
                    )
                };
                bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                    c.Not = true;
                });
                bp.AddClass(ClassTools.Classes.ShifterClass);
            });
            ShifterAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ShifterAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = FinalShifterAspectFeature.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    FinalShifterAspectProgression,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
