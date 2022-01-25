using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    public class TwoWeaponDefenseComponent : UnitFactComponentDelegate,
        IUnitActiveEquipmentSetHandler,
        IGlobalSubscriber,
        ISubscriber,
        IUnitEquipmentHandler,
        IUnitBuffHandler {

        private BlueprintBuff FightDefensivelyBuff {
            get {
                if (m_FightDefensivelyBuff == null) {
                    return null;
                }
                return m_FightDefensivelyBuff.Get();
            }
        }

        private BlueprintFeature MythicBlueprint {
            get {
                if (m_MythicBlueprint == null) {
                    return null;
                }
                return m_MythicBlueprint.Get();
            }
        }

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
            var primaryWeapon = base.Owner.Body.PrimaryHand.MaybeWeapon;
            var secondaryWeapon = base.Owner.Body.SecondaryHand.MaybeWeapon;
            if (primaryWeapon == null || secondaryWeapon == null
                || primaryWeapon.Blueprint.IsNatural || secondaryWeapon.Blueprint.IsNatural
                || primaryWeapon == base.Owner.Body.EmptyHandWeapon || secondaryWeapon == base.Owner.Body.EmptyHandWeapon) {
                return;
            }
            int bonus = 1;
            if (Owner.HasFact(FightDefensivelyBuff)) {
                bonus += 1;
            }
            if (Owner.HasFact(MythicBlueprint)) {
                int highestEnhancment = Math.Max(GameHelper.GetItemEnhancementBonus(primaryWeapon), GameHelper.GetItemEnhancementBonus(secondaryWeapon));
                bonus += highestEnhancment;
            }

            Owner.Stats.AC.AddModifierUnique(bonus, base.Runtime, ModifierDescriptor.Shield);
        }

        private void DeactivateModifier() {
            Owner.Stats.AC.RemoveModifiersFrom(base.Runtime);
        }

        public void HandleBuffDidAdded(Buff buff) {
            UpdateModifier();
        }

        public void HandleBuffDidRemoved(Buff buff) {
            UpdateModifier();
        }

        public BlueprintBuffReference m_FightDefensivelyBuff;
        public BlueprintFeatureReference m_MythicBlueprint;
    }
}
