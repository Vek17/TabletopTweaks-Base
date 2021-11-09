using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Features {
    class PrimalistRagePowerSelection {
        public static void AddPrimalistRagePowerSelection() {
            var RagePowerSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("28710502f46848d48b3f0d6132817c4e");

            var PrimalistRagePowerSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("PrimalistRagePowerSelection", bp => {
                bp.SetName("9b7c60290d884c039a1a0494b2dfd137", "Primalist Rage Power");
                bp.SetDescription("4a0e84b4ba14427aba90234004ffe55c", "At 4th level and every 4 levels thereafter, a primalist can choose to take either his bloodline power "
                + "or two barbarian rage powers. If the primalist chooses rage powers, those rage powers can be used in conjunction with "
                + "his bloodrage, and his bloodrager level acts as his barbarian level when determining the effect of those bloodrage powers "
                + "and any prerequisites. Any other prerequisites for a rage power must be met before a primalist can choose it. This ability "
                + "does not count as the rage power class feature for determining feat prerequisites and other requirements.");
                bp.m_Features = RagePowerSelection.m_Features;
                bp.m_AllFeatures = RagePowerSelection.m_AllFeatures;
                bp.Group = RagePowerSelection.Group;
                bp.Groups = RagePowerSelection.Groups;
                bp.IsClassFeature = true;
            });
        }
    }
}