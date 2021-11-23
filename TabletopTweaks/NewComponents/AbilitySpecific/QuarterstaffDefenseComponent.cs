using System;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;

namespace TabletopTweaks.NewComponents {
    class QuarterstaffDefenseComponent : UnitFactComponentDelegate, 
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
            if (slot.Owner != base.Owner) {
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
            var quarterstaff = Resources.GetBlueprint<BlueprintWeaponType>("629736dabac7f9f4a819dc854eaed2d6");
            if (Owner.Body.CurrentHandsEquipmentSet.PrimaryHand.Weapon.Blueprint.Type == quarterstaff) {
                Owner.Stats.AC.AddModifierUnique(CalculateModifier(), base.Runtime, ModifierDescriptor.Shield);
            }
        }

        private void DeactivateModifier() {
            Owner.Stats.AC.RemoveModifiersFrom(base.Runtime);
        }
        private int CalculateModifier() {
            int qstaffbonus = GameHelper.GetItemEnhancementBonus(base.Owner.Body.CurrentHandsEquipmentSet.PrimaryHand.Weapon);
            var improveddefense = Resources.GetModBlueprint<BlueprintFeature>("QuarterstaffDefenseImproved");
            if (Owner.HasFact(improveddefense)) {
                return qstaffbonus + 3;
            }
            return qstaffbonus;
        }

        public void HandleBuffDidAdded(Buff buff) {
            UpdateModifier();
        }

        public void HandleBuffDidRemoved(Buff buff) {
            UpdateModifier();
        }
    }
}
