using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;

namespace TabletopTweaks.NewContent.Feats {
    static class ExtraReservoir {
        public static void AddExtraReservoir() {
            var ArcanistArcaneReservoirResource = Resources.GetBlueprint<BlueprintAbilityResource>("cac948cbbe79b55459459dd6a8fe44ce");
            var ArcanistArcaneReservoirFeature = Resources.GetBlueprint<BlueprintFeature>("55db1859bd72fd04f9bd3fe1f10e4cbb");
            var ArcaneEnforcerArcaneReservoirFeature = Resources.GetBlueprint<BlueprintFeature>("9d1e2212594cf47438fff2fa3477b954");

            var ExtraReservoir = FeatTools.CreateExtraResourceFeat("ExtraReservoir", ArcanistArcaneReservoirResource, 3, bp => {
                bp.SetName("a91a7e0717f6450387e08b76443b32da", "Extra Reservoir");
                bp.SetDescription("62a89de657744e12a7b7a95698f1b5fd", "You gain 3 more points in your arcane reservoir, and the maximum number of points in your arcane reservoir " +
                    "increases by that amount.\nYou can take this feat multiple times. Its effects stack.");
                bp.AddPrerequisiteFeature(ArcanistArcaneReservoirFeature, GroupType.Any);
                bp.AddPrerequisiteFeature(ArcaneEnforcerArcaneReservoirFeature, GroupType.Any);
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraReservoir")) { return; }
            FeatTools.AddAsFeat(ExtraReservoir);
        }
    }
}
