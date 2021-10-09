using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewActions {
    [TypeId("bc47e9ceb15f4e85960909fdf8b5a8b5")]
    public class EnhanceSheild : ContextAction {

        public ReferenceArrayProxy<BlueprintItemEnchantment, BlueprintItemEnchantmentReference> Enchantment {
            get {
                return m_Enchantment;
            }
        }

        public override string GetCaption() {
            return "Magic Vestment - Shield";
        }

        public override void RunAction() {
            MechanicsContext.Data data = ContextData<MechanicsContext.Data>.Current;
            MechanicsContext mechanicsContext = (data != null) ? data.Context : null;
            if (mechanicsContext == null) {
                return;
            }
            Rounds duration = DurationValue.Calculate(mechanicsContext);
            UnitEntityData unit = Target.Unit;
            if (unit == null) {
                return;
            }
            BlueprintItemEnchantment enchantment;
            int i = Math.Min(EnchantLevel.Calculate(mechanicsContext), Enchantment.Length) - 1;
            enchantment = Enchantment[i];
            ApplyMagicVestment(unit, duration, mechanicsContext, enchantment);
        }

        private void ApplyMagicVestment(UnitEntityData target, Rounds duration, MechanicsContext context, BlueprintItemEnchantment enchantment) {
            ItemEntityShield shield = target.Body.SecondaryHand.MaybeShield;
            if (shield != null) {
                EnchantSlot(shield, duration, context, enchantment);
            }
        }

        public void EnchantSlot(ItemEntityShield item, Rounds duration, MechanicsContext context, BlueprintItemEnchantment enchantment) {
            if (item == null) {
                return;
            }
            List<ItemEnchantment> enchantments = item.Enchantments;
            ItemEnchantment itemEnchantment = (enchantments != null) ? enchantments.GetFact(enchantment) : null;
            if (itemEnchantment != null) {
                if (!itemEnchantment.IsTemporary) {
                    return;
                }
                item.ArmorComponent.RemoveEnchantment(itemEnchantment);
            }
            item.ArmorComponent.AddEnchantment(enchantment, context, new Rounds?(duration));

        }

        [SerializeField]
        [FormerlySerializedAs("Enchantment")]
        public BlueprintItemEnchantmentReference[] m_Enchantment;

        public ContextValue EnchantLevel;

        public ContextDurationValue DurationValue;
    }
}
