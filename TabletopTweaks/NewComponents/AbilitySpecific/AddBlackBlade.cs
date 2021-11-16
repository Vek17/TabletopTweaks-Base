using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("446f9e9a24684cbdb77e3b270af7b5dc")]
    class AddBlackBlade : UnitFactComponentDelegate, IUnitLevelUpHandler {

        public override void OnTurnOn() {
            
        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit) {
        }

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller) {
            //Main.Log($"Mode: {controller.State.Mode}");
            var part = base.Owner.Ensure<UnitPartBlackBlade>();
            part.AddBlackBlade(BlackBlade, base.Context, base.Fact);
        }

        public BlueprintItemWeaponReference BlackBlade;
    }
}
