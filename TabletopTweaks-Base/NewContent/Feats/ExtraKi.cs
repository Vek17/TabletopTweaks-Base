using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    class ExtraKi {
        public static void AddExtraKi() {
            var KiPowerResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");
            var ScaledFistPowerResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("7d002c1025fbfe2458f1509bf7a89ce1");
            var KiPowerFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e9590244effb4be4f830b1e3fffced13");
            var ScaledFistPowerFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("ae98ab7bda409ef4bb39149a212d6732");
            var ExtraKiOwlcat = BlueprintTools.GetBlueprint<BlueprintFeature>("231a2a603d0b437e939553e6da3e7247");

            var ExtraKi = FeatTools.CreateExtraResourceFeat(TTTContext, "ExtraKi", KiPowerResource, 2, bp => {
                bp.SetName(TTTContext, "Extra Ki");
                bp.SetDescription(TTTContext, "Your ki pool increases by 2." +
                    "\nYou can take this feat multiple times. Its effects stack.");
                bp.AddComponent<IncreaseResourceAmountBySharedValue>(c => {
                    c.m_Resource = ScaledFistPowerResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = 2;
                });
                bp.AddPrerequisiteFeature(KiPowerFeature, Prerequisite.GroupType.Any);
                bp.AddPrerequisiteFeature(ScaledFistPowerFeature, Prerequisite.GroupType.Any);
            });
            if (TTTContext.AddedContent.Feats.IsDisabled("ExtraKi")) { return; }
            FeatTools.AddAsFeat(ExtraKi);
            FeatTools.RemoveAsFeat(ExtraKiOwlcat);
        }
    }
}
