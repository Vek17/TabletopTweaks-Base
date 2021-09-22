using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {
    class BroadStudyMythicComponent : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartBroadStudy>().AddMythicSource(base.Fact);
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartBroadStudy>().RemoveMythicSource(base.Fact);
        }
    }
}
