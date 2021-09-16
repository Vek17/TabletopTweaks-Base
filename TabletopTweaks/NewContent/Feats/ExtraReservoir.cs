using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Feats {
    static class ExtraReservoir {
        public static void AddExtraReservoir() {
            var ArcanistArcaneReservoirResource = Resources.GetBlueprint<BlueprintAbilityResource>("cac948cbbe79b55459459dd6a8fe44ce");
            var ArcanistArcaneReservoirFeature = Resources.GetBlueprint<BlueprintFeature>("55db1859bd72fd04f9bd3fe1f10e4cbb");
            var ArcaneEnforcerArcaneReservoirFeature = Resources.GetBlueprint<BlueprintFeature>("9d1e2212594cf47438fff2fa3477b954");

            var ExtraReservoir = FeatTools.CreateExtraResourceFeat("ExtraReservoir", ArcanistArcaneReservoirResource, 3, bp => {
                bp.SetName("Extra Reservoir");
                bp.SetDescription("You gain three more points in your arcane reservoir, and the maximum number of points in your arcane reservoir " +
                    "increases by that amount.\nYou can take this feat multiple times. Its effects stack.");
                bp.AddPrerequisiteFeaturesFromList(1, ArcanistArcaneReservoirFeature, ArcaneEnforcerArcaneReservoirFeature);
            });
            if (ModSettings.AddedContent.Feats.IsDisabled("ExtraReservoir")) { return; }
            FeatTools.AddAsFeat(ExtraReservoir);
        }
    }
}
