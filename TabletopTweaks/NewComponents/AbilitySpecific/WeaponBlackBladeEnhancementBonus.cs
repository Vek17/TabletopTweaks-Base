using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("c178aa28c5c545139b7702bbf4fd4cf1")]
    class WeaponBlackBladeEnhancementBonus : WeaponEnhancementBonus, IUnitLevelUpHandler, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler {
        public override void OnTurnOn() {
            base.EnhancementBonus = CalculateModifier();
        }

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller) {
            base.EnhancementBonus = CalculateModifier();
        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit) {
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            base.EnhancementBonus = CalculateModifier();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            base.EnhancementBonus = CalculateModifier();
        }

        private int CalculateModifier() {
            var MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var MagusLevel = Owner?.Wielder?.Progression?.GetClassLevel(MagusClass) ?? 0;
            var bonus = MagusLevel switch {
                >= 3 and <= 4 => 1,
                >= 5 and <= 8 => 2,
                >= 9 and <= 12 => 3,
                >= 13 and <= 16 => 4,
                >= 17 and <= 20 => 5,
                _ => 0
            };
            return bonus;
        }
    }
}
