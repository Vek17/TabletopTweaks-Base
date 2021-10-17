using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("a23b7a08964d4a3792a6754884eee8aa")]
    class AddWarriorSpiritEnchantment : UnitFactComponentDelegate {
        public override void OnTurnOn() {
            base.Owner.Ensure<UnitPartWarriorSpirit>().AddEntry(base.Fact, Cost, Enchants);
        }

        public override void OnTurnOff() {
            base.Owner.Ensure<UnitPartWarriorSpirit>().RemoveEntry(base.Fact);
        }

        public BlueprintWeaponEnchantmentReference[] Enchants;
        public int Cost;
    }
}
