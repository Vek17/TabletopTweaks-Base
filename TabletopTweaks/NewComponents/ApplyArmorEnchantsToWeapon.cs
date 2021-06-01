using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TabletopTweaks.NewComponents {
    [TypeId("49c9258f5eff4224bcf52ba3772b7d0c")]
    class ApplyArmorEnchantsToWeapon: UnitFactComponentDelegate, IUnitActiveEquipmentSetHandler,
        IGlobalSubscriber,
        ISubscriber,
        IUnitEquipmentHandler,
        IUnitBuffHandler {

        public override void OnTurnOn() {
            base.OnTurnOn();
            CheckArmor();
        }

        public override void OnTurnOff() {
            base.OnTurnOff();
            DeactivateEnchants();
        }

        public override void OnActivate() {
            CheckArmor();
        }

        public override void OnDeactivate() {
            DeactivateEnchants();
        }

        public void HandleBuffDidAdded(Buff buff) {
            CheckArmor();
        }

        public void HandleBuffDidRemoved(Buff buff) {
            CheckArmor();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            CheckArmor();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            CheckArmor();
        }

        private void CheckArmor() {
            DeactivateEnchants();
            if (Owner.Body.IsPolymorphed) {
                return;
            }
            var armorEnchants = Owner.Body.Armor?.MaybeArmor?.Enchantments;
            if (armorEnchants == null) {
                return;
            }

            var primaryHandWeapon = Owner?.Body?.PrimaryHand?.MaybeItem as ItemEntityWeapon;
            var secondaryHandWeapon = Owner?.Body?.SecondaryHand?.MaybeItem as ItemEntityWeapon;

            List<ItemEntityWeapon> weapons = new List<ItemEntityWeapon>();
            if (primaryHandWeapon?.Blueprint == Weapon.Get()) {
                weapons.Add(primaryHandWeapon);
            }
            if (secondaryHandWeapon?.Blueprint == Weapon.Get()) {
                weapons.Add(secondaryHandWeapon);
            }
            foreach (var limb in Owner.Body.AdditionalLimbs) {
                var weapon = limb?.MaybeWeapon;
                if (weapon?.Blueprint == Weapon.Get()) {
                    weapons.Add(weapon);
                }
            }

            if (weapons.Empty()) {
                return;
            }
            foreach (var e in armorEnchants) {
                if (!(e.Blueprint is BlueprintArmorEnchantment blueprint)) {
                    continue;
                }
                if (EnhancmentMap.ContainsKey(blueprint)) {
                    foreach (var w in weapons) {
                        if (w.Enchantments.HasFact(blueprint)) {
                            continue;
                        }
                        var newEnchant = w.AddEnchantment(EnhancmentMap[blueprint], this.Fact.MaybeContext);
                        newEnchant.RemoveOnUnequipItem = true;
                        m_enchants.Add(newEnchant);
                    }
                }
            }
            var enchancementBonus = Math.Min(5, GameHelper.GetItemEnhancementBonus(this.Owner.Body.Armor.Armor));
            if (enchancementBonus <= 0) {
                return;
            }
            foreach (var w in weapons) {
                var new_enchant = w.AddEnchantment(EnhancmentBonuses[enchancementBonus - 1], this.Fact.MaybeContext);
                new_enchant.RemoveOnUnequipItem = true;
                m_enchants.Add(new_enchant);
            }
        }

        private void DeactivateEnchants() {
            foreach (var e in m_enchants) {
                e.Owner?.RemoveEnchantment(e);
            }
            m_enchants = new List<ItemEnchantment>();
        }

        public BlueprintItemWeaponReference Weapon;
        [JsonProperty]
        private List<ItemEnchantment> m_enchants = new List<ItemEnchantment>();
        private static BlueprintWeaponEnchantment[] EnhancmentBonuses = new BlueprintWeaponEnchantment[] { 
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("d704f90f54f813043a525f304f6c0050"), //TemporaryEnhancement1
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("9e9bab3020ec5f64499e007880b37e52"), //TemporaryEnhancement2
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("d072b841ba0668846adeb007f623bd6c"), //TemporaryEnhancement3
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("6a6a0901d799ceb49b33d4851ff72132"), //TemporaryEnhancement4
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("746ee366e50611146821d61e391edf16")  //TemporaryEnhancement5
        };
        private static Dictionary<BlueprintArmorEnchantment, BlueprintWeaponEnchantment> EnhancmentMap = new Dictionary<BlueprintArmorEnchantment, BlueprintWeaponEnchantment>() {
            {Resources.GetBlueprint<BlueprintArmorEnchantment>("933456ff83c454146a8bf434e39b1f93"), Resources.GetBlueprint<BlueprintWeaponEnchantment>("ab39e7d59dd12f4429ffef5dca88dc7b") }, //AdamantineHeavy
            {Resources.GetBlueprint<BlueprintArmorEnchantment>("aa25531ab5bb58941945662aa47b73e7"), Resources.GetBlueprint<BlueprintWeaponEnchantment>("ab39e7d59dd12f4429ffef5dca88dc7b") }, //AdamantineMedium
            {Resources.GetBlueprint<BlueprintArmorEnchantment>("7b95a819181574a4799d93939aa99aff"), Resources.GetBlueprint<BlueprintWeaponEnchantment>("e5990dc76d2a613409916071c898eee8") }, //Mithril
    };
    }
}
