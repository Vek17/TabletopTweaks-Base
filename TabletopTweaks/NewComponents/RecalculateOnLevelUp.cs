using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace TabletopTweaks.NewComponents {
    [TypeId("0104e3c3ea2e4f789bc1b1ac4a874d7b")]
    class RecalculateOnLevelUp : UnitFactComponentDelegate, IUnitLevelUpHandler {
        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller) {
            if (base.Fact.Owner == unit) {
                base.Fact.Reapply();
            }
        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit) {
        }
    }
}
