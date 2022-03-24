using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.WizardArcaneDiscoveries {
    public static class ArcaneDiscoverySelection {
        public static void AddArcaneDiscoverySelection() {
            var WizardClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("ba34257984f4c41408ce1dc2004e342e");

            var ArcaneDiscoverySelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "ArcaneDiscoverySelection", bp => {
                bp.SetName(TTTContext, "Arcane Discovery");
                bp.SetDescription(TTTContext, "Wizards spend much of their lives seeking deeper truths, " +
                    "hunting knowledge as if it were life itself. The wizard’s power is not necessarily the spells he wields; " +
                    "spells are merely the outward, most visible manifestation of that power. A wizard’s true power is in his " +
                    "fierce intelligence, his dedication to his craft, and his ability to peel back the surface truths of reality " +
                    "to understand the fundamental underpinnings of existence. A wizard spends much of his time researching spells, " +
                    "and would rather find an undiscovered library than a room full of gold. A wizard need not be a reclusive bookworm, " +
                    "but he must have a burning curiosity for the unknown. Arcane discoveries are the results of this obsession with magic.\n" +
                    "A wizard can learn an arcane discovery in place of a regular feat or wizard bonus feat.");
                bp.m_AllFeatures = new BlueprintFeatureReference[0];
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = WizardClass;
                    c.Level = 1;
                });
            });
        }
        public static void AddToArcaneDiscoverySelection(params BlueprintFeature[] features) {
            var ArcaneDiscoverySelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "ArcaneDiscoverySelection");

            ArcaneDiscoverySelection.AddFeatures(features);
            if (!AddedAsFeat) {
                FeatTools.AddAsFeat(ArcaneDiscoverySelection);
                AddedAsFeat = true;
            }
        }

        private static bool AddedAsFeat = false;
    }
}
