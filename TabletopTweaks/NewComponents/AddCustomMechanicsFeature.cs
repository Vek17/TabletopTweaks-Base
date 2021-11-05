using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {
    [TypeId("38965e585f1c4da78db9276a47571209")]
    class AddCustomMechanicsFeature : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            Owner.CustomMechanicsFeature(Feature).Retain();
        }

        public override void OnTurnOff() {
            Owner.CustomMechanicsFeature(Feature).Release();
        }

        public UnitPartCustomMechanicsFeatures.CustomMechanicsFeature Feature;
    }
}
