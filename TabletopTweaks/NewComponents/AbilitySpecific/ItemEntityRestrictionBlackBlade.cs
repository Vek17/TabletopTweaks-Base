using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewComponents.NewBaseTypes;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintItemWeapon), false)]
    [TypeId("914ecd4aa8a04c24aa8dae7fd9d618de")]
    class ItemEntityRestrictionBlackBlade : ItemEntityRestriction {

        public override bool CanBeEquippedBy(UnitDescriptor unit, ItemEntity item) {
            var BlackBlade = unit.Get<UnitPartBlackBlade>();
            if (BlackBlade == null) { return false; }
            return BlackBlade.IsBlackBlade(item);
        }
    }
}
