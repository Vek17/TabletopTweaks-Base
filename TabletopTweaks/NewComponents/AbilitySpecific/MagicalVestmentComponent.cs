using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [AllowMultipleComponents]
    [TypeId("4db6644b48ed43a69e16d8c9b60dd775")]
    class MagicalVestmentComponent : UnitBuffComponentDelegate<BuffEnchantWornItemData>, IUnitEquipmentHandler {
        private BlueprintItemEnchantment Enchantment {
            get {
                return Enchantments[EnhancementBonus - 1];
            }
        }

        private ReferenceArrayProxy<BlueprintItemEnchantment, BlueprintItemEnchantmentReference> Enchantments {
            get {
                return this.m_EnchantmentBlueprints;
            }
        }

        private int EnhancementBonus => EnchantLevel.Calculate(base.Context);

        public override void OnActivate() {
            if (!base.Data.Enchantments.Empty()) { return; }
            base.Owner.Stats.GetStat(StatType.AC).RemoveModifiersFrom(base.Runtime);
            ItemEntity itemEntity = Shield ? base.Owner.Body.SecondaryHand.MaybeShield?.ArmorComponent : base.Owner.Body.Armor.MaybeArmor;
            if (itemEntity != null) {
                base.Data.Enchantments.Add(itemEntity.AddEnchantment(this.Enchantment, base.Context, null));
            } else {
                if (!Shield) {
                    base.Owner.Stats.GetStat(StatType.AC).AddModifierUnique(EnhancementBonus, base.Runtime, ModifierDescriptor.Armor);
                }
            }
        }

        public override void OnDeactivate() {
            foreach (ItemEnchantment itemEnchantment in base.Data.Enchantments) {
                ItemEntity owner = itemEnchantment.Owner;
                if (owner != null) {
                    owner.RemoveEnchantment(itemEnchantment);
                }
            }
            base.Owner.Stats.GetStat(StatType.AC).RemoveModifiersFrom(base.Runtime);
            base.Data.Enchantments.Clear();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != Owner) {
                return;
            }
            OnActivate();
        }

        [SerializeField]
        [FormerlySerializedAs("Enchantment")]
        public BlueprintItemEnchantmentReference[] m_EnchantmentBlueprints = new BlueprintItemEnchantmentReference[5];
        public ContextValue EnchantLevel;
        public bool Shield;
    }
}
