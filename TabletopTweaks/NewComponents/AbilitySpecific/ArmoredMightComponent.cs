using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using System;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("e90c706b6fd84f90b4dcd35ef2699483")]
    class ArmoredMightComponent : UnitFactComponentDelegate,
        IUnitActiveEquipmentSetHandler,
        IGlobalSubscriber,
        ISubscriber,
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
            UpdateModifier();
        }

        private void UpdateModifier() {
            DeactivateModifier();
            ActivateModifier();
        }

        private void ActivateModifier() {
            if (Owner.Body.Armor.HasArmor && Owner.Body.Armor.Armor.Blueprint.IsArmor) {
                Owner.Stats.AC.AddModifierUnique(CalculateModifier(), base.Runtime, ModifierDescriptor.ArmorFocus);
            }
        }

        private void DeactivateModifier() {
            Owner.Stats.AC.RemoveModifiersFrom(base.Runtime);
        }

        private int CalculateModifier() {
            int itemEnhancementBonus = GameHelper.GetItemEnhancementBonus(Owner.Body.Armor.Armor);
            int mythicBonus = (Owner.Body.Armor.Armor.Blueprint.ArmorBonus + itemEnhancementBonus) / 2;
            int limit = (Owner.Progression.MythicLevel + 1) / 2;
            return Math.Min(mythicBonus, limit);
        }

        public void HandleBuffDidAdded(Buff buff) {
            UpdateModifier();
        }

        public void HandleBuffDidRemoved(Buff buff) {
            UpdateModifier();
        }
    }
}
