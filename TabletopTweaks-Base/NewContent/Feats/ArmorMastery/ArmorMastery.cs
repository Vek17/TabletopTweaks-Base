using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats.ArmorMastery {
    static class ArmorMastery {
        internal static void AddArmorMasterySelection() {
            var FighterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ArmorFocusLight = BlueprintTools.GetBlueprint<BlueprintFeature>("3bc6e1d2b44b5bb4d92e6ba59577cf62");
            var ArmorFocusMedium = BlueprintTools.GetBlueprint<BlueprintFeature>("7dc004879037638489b64d5016997d12");
            var ArmorFocusHeavy = BlueprintTools.GetBlueprint<BlueprintFeature>("c27e6d2b0d33d42439f512c6d9a6a601");
            var FighterFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f");
            var FighterTrainingFakeLevel = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(TTTContext, "FighterTrainingFakeLevel");

            var ArmorMasterySelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ArmorMasterySelection", bp => {
                bp.SetName(TTTContext, "Armor Mastery");
                bp.SetDescription(TTTContext, "Armor mastery feats are a new type of feat that require armor training as a prerequisite. " +
                    "They count as combat feats for all purposes, including which classes can select them as bonus feats. " +
                    "You gain the benefits of an armor mastery feat only while wearing armor with which you are proficient " +
                    "and only while wearing a type of armor that matches the feat’s armor proficiency feat prerequisite, if any. " +
                    "Armor mastery feats without armor proficiency prerequisites can be used while wearing any suit of armor. " +
                    "Characters who lack the armor training class feature can access armor mastery feats by taking a relevent Armor Focus feat.");
                bp.m_AllFeatures = new BlueprintFeatureReference[0];
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
            });
        }
        public static void AddToArmorMasterySelection(params BlueprintFeature[] features) {
            var ArmorMasterySelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "ArmorMasterySelection");

            ArmorMasterySelection.AddFeatures(features);
            if (!AddedAsFeat) {
                FeatTools.AddAsFeat(ArmorMasterySelection);
                FighterAdvancedArmorTrainings.AdvancedArmorTraining.AddToAdvancedArmorTrainingSelection(ArmorMasterySelection);
                AddedAsFeat = true;
            }
        }

        private static bool AddedAsFeat = false;
    }
}
