using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats.ShieldMastery {
    public static class ShieldMastery {
        internal static void AddShieldMasterySelection() {
            var FighterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ShieldFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("ac57069b6bf8c904086171683992a92a");
            var FighterFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f");
            var FighterTrainingFakeLevel = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(TTTContext, "FighterTrainingFakeLevel");

            var ShieldMasterySelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ShieldMasterySelection", bp => {
                bp.SetName(TTTContext, "Shield Mastery");
                bp.SetDescription(TTTContext, "To some combatants shields are just as important, " +
                    "if not more so, than weapons or armor. Shields are multipurpose, capable of defense or offense. " +
                    "They slide on and off far faster than armor, and allow adventurers to change their tactics on a whim. " +
                    "A handful of specialists train to maximize a shield’s use in battle. Presented here are new feats categorized " +
                    "as shield mastery feats, each requiring Shield Focus as a prerequisite. Characters with the armor training " +
                    "class feature can ignore the Shield Focus feat as a prerequisite for shield mastery feats.Shield mastery " +
                    "feats count as combat feats for all purposes, including which classes can select them as bonus feats. " +
                    "You gain the benefits of a shield mastery feat only while wielding a shield with which you are proficient.");

                bp.m_AllFeatures = new BlueprintFeatureReference[0];
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Defense;
                });
            });
        }
        public static void AddToShieldMasterySelection(params BlueprintFeature[] features) {
            var ShieldMasterySelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "ShieldMasterySelection");

            ShieldMasterySelection.AddFeatures(features);
            if (!AddedAsFeat) {
                FeatTools.AddAsFeat(ShieldMasterySelection);
                FighterAdvancedArmorTrainings.AdvancedArmorTraining.AddToAdvancedArmorTrainingSelection(ShieldMasterySelection);
                AddedAsFeat = true;
            }
        }

        private static bool AddedAsFeat = false;
    }
}
