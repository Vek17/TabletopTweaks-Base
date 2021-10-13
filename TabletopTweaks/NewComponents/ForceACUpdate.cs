using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;


namespace TabletopTweaks.NewComponents {
    [TypeId("a736dbb1331c456488c601cefdbcd886")]
    class ForceACUpdate : UnitFactComponentDelegate {

        public override void OnTurnOn() {
            UpdateAC();
        }
        public override void OnTurnOff() {
            UpdateAC();
        }
        public override void OnActivate() {
            UpdateAC();
        }
        public override void OnDeactivate() {
            UpdateAC();
        }
        private void UpdateAC() {
            Owner.Descriptor.Stats.AC.UpdateValue();
        }
    }
}
