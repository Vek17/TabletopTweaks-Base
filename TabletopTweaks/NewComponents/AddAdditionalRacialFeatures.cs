using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.FeatureSelector;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    [TypeId("459d17478d434bbb881b99d766113cb9")]
    class AddAdditionalRacialFeatures : UnitFactComponentDelegate {

        public override void OnActivate() {
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            if (controller.State.Mode == LevelUpState.CharBuildMode.Mythic) { return; }
            if (Owner.Descriptor.Progression.CharacterLevel > 1) { return; }
            LevelUpHelper.AddFeaturesFromProgression(controller.State, Owner, this.Features.Select(f => f.Get()).ToArray(), Owner.Progression.Race, 0);
        }

        public BlueprintFeatureBaseReference[] Features;

        [HarmonyPatch(typeof(CharGenFeatureSelectorPhaseVM), "OrderPriority", MethodType.Getter)]
        static class Background_OrderPriority_Patch {
            static void Postfix(ref int __result, CharGenFeatureSelectorPhaseVM __instance) {
                FeatureGroup featureGroup = UIUtilityUnit.GetFeatureGroup(__instance.FeatureSelectorStateVM?.Feature);
                if (featureGroup == FeatureGroup.BackgroundSelection) { __result += 500; }
            }
        }
    }
}
