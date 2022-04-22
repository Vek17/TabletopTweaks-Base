using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.AlternateCapstones {
    internal class Arcanist {
        public static BlueprintFeatureSelection ArcanistAlternateCapstone = null;
        public static void AddAlternateCapstones() {
            var ArcanistMagicalSupremacy = BlueprintTools.GetBlueprint<BlueprintFeature>("261270d064148224fb982590b7a65414");
            var ArcanistArcaneReservoirResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("cac948cbbe79b55459459dd6a8fe44ce");

            var DeepReservoir = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "DeepReservoir", bp => {
                bp.SetName(TTTContext, "Deep Reservoir");
                bp.SetDescription(TTTContext, "At 20th level, the arcanist has enough power to blast things all day long.\n" +
                    "Her arcane reservoir increases by 10.");
                bp.IsClassFeature = true;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = ArcanistArcaneReservoirResource;
                    c.Value = 10;
                });
            });
            ArcanistAlternateCapstone = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ArcanistAlternateCapstone", bp => {
                bp.SetName(TTTContext, "Capstone");
                bp.SetDescription(TTTContext, "When a character reaches the 20th level of a class, she gains a powerful class feature or ability, " +
                    "sometimes referred to as a capstone. The following section provides new capstones for characters to select at 20th level. " +
                    "A character can select one of the following capstones in place of the capstone provided by her class. " +
                    "A character can’t select a new capstone if she has previously traded away her class capstone via an archetype. " +
                    "Clerics and wizards can receive a capstone at 20th level, despite not having one to begin with.");
                bp.IsClassFeature = true;
                bp.m_Icon = ArcanistMagicalSupremacy.Icon;
                bp.Ranks = 1;
                bp.AddPrerequisite<PrerequisiteInPlayerParty>(c => {
                    c.CheckInProgression = true;
                    c.HideInUI = true;
                });
                bp.AddFeatures(ArcanistMagicalSupremacy, DeepReservoir, Generic.PerfectBodyFlawlessMindProgression, Generic.GreatBeastMasterFeature);
            });
        }
    }
}
