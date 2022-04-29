using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using System.Collections.Generic;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Alchemist {
        public static BlueprintFeatureSelection AlchemistAlternateCapstone = null;
        public static BlueprintProgression GrandDiscoveryProgression = null;
        public static void AddAlternateCapstones() {
            var DiscoverySelection = BlueprintTools.GetBlueprint<BlueprintFeature>("cd86c437488386f438dcc9ae727ea2a6");
            var GrandDiscoverySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("2729af328ab46274394cedc3582d6e98");
            var AlchemistBombsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c59b2f256f5a70a4d896568658315b7d");

            var VastExplosions = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "VastExplosions", bp => {
                bp.SetName(TTTContext, "Vast Explosions");
                bp.SetDescription(TTTContext, "The alchemist has been practicing his demolitions for years, and it’s paid off with ever larger explosions.\n" +
                    "The alchemist’s bomb damage increases by 3d6.");
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
                    Helpers.CreateLevelEntry(20,
                        AlchemistBombsFeature,
                        AlchemistBombsFeature,
                        AlchemistBombsFeature
                    )
                };
            });
            GrandDiscoveryProgression = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "GrandDiscoveryProgression", bp => {
                bp.m_DisplayName = GrandDiscoverySelection.m_DisplayName;
                bp.SetDescription(TTTContext, "At 20th level, the alchemist makes a grand discovery. He immediately learns two normal discoveries, " +
                    "but also learns a third discovery chosen from the list below, representing a truly astounding alchemical breakthrough of significant import. " +
                    "For many alchemists, the promise of one of these grand discoveries is the primary goal of their experiments and hard work.");
                bp.m_Icon = GrandDiscoverySelection.Icon;
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
                        GrandDiscoverySelection,
                        DiscoverySelection,
                        DiscoverySelection
                    )
                };
                bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                    c.Not = true;
                });
            });
            AlchemistAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "AlchemistAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = GrandDiscoverySelection.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(GrandDiscoveryProgression, Generic.PerfectBodyFlawlessMindProgression, Generic.GreatBeastMasterFeature);
                if (TTTContext.Fixes.AlternateCapstones.IsDisabled("Alchemist")) { return; }
                GrandDiscoverySelection.AddFeatures(VastExplosions);
            });
        }
    }
}
