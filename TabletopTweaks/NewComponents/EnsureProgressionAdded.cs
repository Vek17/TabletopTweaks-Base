using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;

namespace TabletopTweaks.NewComponents {
    //This is a hack that should not be needed
    [TypeId("f60835ac5ca54ff4b29cf0e3c65a465e")]
    [AllowedOn(typeof(BlueprintFeature))]
    class EnsureProgressionAdded : UnitFactComponentDelegate,
        IUnitLevelUpHandler, IGlobalSubscriber, ISubscriber {

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller) {
            BlueprintProgression blueprintProgression = this.Fact.Blueprint as BlueprintProgression;
            if (blueprintProgression != null && controller.State != null) {
                LevelUpHelper.UpdateProgression(controller.State, unit, blueprintProgression);
            }
        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit) {
        }
    }
}
