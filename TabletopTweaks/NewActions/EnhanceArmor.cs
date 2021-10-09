using HarmonyLib;
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
    [TypeId("94f7fe11fb1c4c38b13736bd164a53de")]
    public class EnhanceArmor : ContextAction {

        public ReferenceArrayProxy<BlueprintItemEnchantment, BlueprintItemEnchantmentReference> Enchantment {
            get {
                return m_Enchantment;
            }
        }

        public override string GetCaption() {
            return "Magic Vestment - Armor";
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
            ItemEntityArmor armor = target.Body.Armor.MaybeArmor;
            if (armor != null) {
                EnchantSlot(armor, duration, context, enchantment);
            }
        }

        public void EnchantSlot(ItemEntityArmor item, Rounds duration, MechanicsContext context, BlueprintItemEnchantment enchantment) {
            if (item == null) {
                return;
            }
            List<ItemEnchantment> enchantments = item.Enchantments;
            ItemEnchantment itemEnchantment = (enchantments != null) ? enchantments.GetFact(enchantment) : null;
            if (itemEnchantment != null) {
                if (!itemEnchantment.IsTemporary) {
                    return;
                }
                item.RemoveEnchantment(itemEnchantment);
            }
            item.AddEnchantment(enchantment, context, new Rounds?(duration));
        }

        [SerializeField]
        [FormerlySerializedAs("Enchantment")]
        public BlueprintItemEnchantmentReference[] m_Enchantment;

        public ContextValue EnchantLevel;

        public ContextDurationValue DurationValue;

        [HarmonyPatch(typeof(ItemEntity), "AddEnchantment")]
        static class ItemEntity_AddEnchantment_Patch {
            static void Postfix(ItemEntity __instance) {
                var armor = __instance as ItemEntityArmor;
                if (armor != null) {
                    armor.RecalculateStats();
                    armor.RecalculateMaxDexBonus();
                }
            }
        }
        [HarmonyPatch(typeof(ItemEntity), "RemoveEnchantment")]
        static class ItemEntity_RemoveEnchantment_Patch {
            static void Postfix(ItemEntity __instance) {
                var armor = __instance as ItemEntityArmor;
                if (armor != null) {
                    armor.RecalculateStats();
                    armor.RecalculateMaxDexBonus();
                }
            }
        }
    }
}
