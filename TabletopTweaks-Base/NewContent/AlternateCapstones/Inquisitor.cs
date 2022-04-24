using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Inquisitor {
        public static BlueprintFeatureSelection InquisitorAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var TrueJudgmentFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("f069b6557a2013544ac3636219186632");

            var TeamLeader = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TeamLeader", bp => {
                bp.SetName(TTTContext, "Veteran of Endless War");
                bp.SetDescription(TTTContext, "At 20th level, the inquisitor has grown accustomed to teaching farmers to fight, but what she can do with trained warriors is far more terrifying.\n" +
                    "When the inquisitor regains her spells each day, she can also spend 1 hour training a number of characters up to her Wisdom modifier in battle tactics. " +
                    "These characters receive three of the inquisitor’s teamwork feats (the inquisitor’s choice) as bonus feats for the next 24 hours.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
            });
            InquisitorAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "InquisitorAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = TrueJudgmentFeature.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    TrueJudgmentFeature,
                    TeamLeader,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
