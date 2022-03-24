using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class ExtraReservoir {
        public static void AddExtraReservoir() {
            var ArcanistArcaneReservoirResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("cac948cbbe79b55459459dd6a8fe44ce");
            var ArcanistArcaneReservoirFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("55db1859bd72fd04f9bd3fe1f10e4cbb");
            var ArcaneEnforcerArcaneReservoirFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9d1e2212594cf47438fff2fa3477b954");

            var ExtraReservoir = FeatTools.CreateExtraResourceFeat(TTTContext, "ExtraReservoir", ArcanistArcaneReservoirResource, 3, bp => {
                bp.SetName(TTTContext, "Extra Reservoir");
                bp.SetDescription(TTTContext, "You gain 3 more points in your arcane reservoir, and the maximum number of points in your arcane reservoir " +
                    "increases by that amount.\nYou can take this feat multiple times. Its effects stack.");
                bp.AddPrerequisiteFeature(ArcanistArcaneReservoirFeature, GroupType.Any);
                bp.AddPrerequisiteFeature(ArcaneEnforcerArcaneReservoirFeature, GroupType.Any);
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ExtraReservoir")) { return; }
            FeatTools.AddAsFeat(ExtraReservoir);
        }
    }
}
