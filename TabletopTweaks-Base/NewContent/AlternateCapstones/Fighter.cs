using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using System.Collections.Generic;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Fighter {
        public static BlueprintFeatureSelection FighterAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var WeaponMasterySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("55f516d7d1fc5294aba352a5a1c92786");

            var VeteranOfEndlessWar = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "VeteranOfEndlessWar", bp => {
                bp.SetName(TTTContext, "Veteran of Endless War");
                bp.SetDescription(TTTContext, "At 20th level, the fighter has seen more combat than entire platoons of soldiers put together.\n" +
                    "The bonuses granted by his armor training and weapon training increase by 2 each.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[0];
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_AlternateProgressionClasses = new BlueprintProgression.ClassWithLevel[0];
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(19, ArmorTraining),
                    Helpers.CreateLevelEntry(20, ArmorTraining)
                };
                bp.AddComponent<IncreaseFeatRankByGroup>(c => {
                    c.Group = FeatureGroup.WeaponTraining;
                });
                bp.AddComponent<IncreaseFeatRankByGroup>(c => {
                    c.Group = FeatureGroup.WeaponTraining;
                });
            });
            FighterAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "FighterAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = WeaponMasterySelection.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    WeaponMasterySelection,
                    VeteranOfEndlessWar,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature,
                    Generic.OldDogNewTricksProgression
                );
            });
        }
    }
}
