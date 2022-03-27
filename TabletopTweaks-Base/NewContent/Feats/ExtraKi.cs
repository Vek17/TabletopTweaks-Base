using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    class ExtraKi {
        public static void AddExtraKi() {
            var KiPowerResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");
            var KiPowerFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e9590244effb4be4f830b1e3fffced13");

            var ExtraKi = FeatTools.CreateExtraResourceFeat(TTTContext, "ExtraKi", KiPowerResource, 2, bp => {
                bp.SetName(TTTContext, "Extra Ki");
                bp.SetDescription(TTTContext, "Your ki pool increases by 2." +
                    "\nYou can take this feat multiple times. Its effects stack.");
                bp.AddPrerequisiteFeature(KiPowerFeature);
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ExtraKi")) { return; }
            FeatTools.AddAsFeat(ExtraKi);
        }
    }
}
