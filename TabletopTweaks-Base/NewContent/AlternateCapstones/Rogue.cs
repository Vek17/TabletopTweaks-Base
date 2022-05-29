using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal static class Rogue {
        public static BlueprintFeatureSelection RogueAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var MasterStrike = BlueprintTools.GetBlueprint<BlueprintFeature>("72dcf1fb106d5054a81fd804fdc168d3");

            var MasterfulTalent = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MasterfulTalent", bp => {
                bp.SetName(TTTContext, "Masterful Talent");
                bp.SetDescription(TTTContext, "At 20th level, the rogue has been a thief, an actor, a merchant, a scout, a confessor, a friend, an assassin, and a dozen more things besides.\n" +
                    "The rogue gains a +4 bonus on all of her skills.");
                bp.IsClassFeature = true;
                bp.AddComponent<BuffAllSkillsBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 4;
                    c.Multiplier = 1;
                });
            });
            RogueAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "RogueAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.IgnorePrerequisites = true;
                bp.m_Icon = MasterStrike.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(
                    MasterStrike,
                    MasterfulTalent,
                    Generic.PerfectBodyFlawlessMindProgression,
                    Generic.GreatBeastMasterFeature
                );
            });
        }
    }
}
