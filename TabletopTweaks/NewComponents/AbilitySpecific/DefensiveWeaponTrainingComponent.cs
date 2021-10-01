using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Parts;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    class DefensiveWeaponTrainingComponent : UnitFactComponentDelegate,
        IUnitActiveEquipmentSetHandler,
        IGlobalSubscriber,
        ISubscriber,
        IUnitConditionsChanged,
        IUnitEquipmentHandler,
        IUnitBuffHandler {

        public override void OnTurnOn() {
            base.OnTurnOn();
            UpdateModifier();
        }

        public override void OnTurnOff() {
            base.OnTurnOff();
            DeactivateModifier();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != Owner) {
                return;
            }
            UpdateModifier();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            if (unit == Owner) { UpdateModifier(); }
        }

        private void UpdateModifier() {
            DeactivateModifier();
            ActivateModifier();
        }

        private void ActivateModifier() {
            if (Owner.State.IsHelpless || !Owner.State.CanMove || Owner.State.HasCondition(UnitCondition.Prone)) { return; }
            var weaponTraining = base.Owner.Get<UnitPartWeaponTraining>();
            var weapon = base.Owner.Body.PrimaryHand.Weapon;
            var trainingBonus = weaponTraining?.GetWeaponRank(weapon) ?? 0;
            if (trainingBonus > 0) {
                Owner.Stats.AC.AddModifierUnique(CalculateModifier(trainingBonus), base.Runtime, ModifierDescriptor.Shield);
            }
        }

        private void DeactivateModifier() {
            Owner.Stats.AC.RemoveModifiersFrom(base.Runtime);
        }

        private int CalculateModifier(int trainingBonus) {
            int baseBonus = trainingBonus >= 4 ? 2 : 1;
            int itemEnhancementBonus = GameHelper.GetItemEnhancementBonus(Owner.Body.PrimaryHand.Weapon);
            return baseBonus + (itemEnhancementBonus / 2);
        }

        public void HandleBuffDidAdded(Buff buff) {
            UpdateModifier();
        }

        public void HandleBuffDidRemoved(Buff buff) {
            UpdateModifier();
        }

        public void HandleUnitConditionsChanged(UnitEntityData unit, UnitCondition condition) {
            if (unit == Owner) { UpdateModifier(); }
        }
    }
}
