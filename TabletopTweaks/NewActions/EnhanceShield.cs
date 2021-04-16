using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewActions {

	public class EnhanceSheild : ContextAction {

		public ReferenceArrayProxy<BlueprintItemEnchantment, BlueprintItemEnchantmentReference> Enchantment {
			get {
				return m_Enchantment;
			}
		}

		public override string GetCaption() {
			return "Magic Vestment";
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
			Main.Log($"Selected Enchant: {enchantment.name}");
			ApplyMagicWeapon(unit, duration, mechanicsContext, enchantment);
		}

		private void ApplyMagicWeapon(UnitEntityData target, Rounds duration, MechanicsContext context, BlueprintItemEnchantment enchantment) {
			ItemEntityShield shield = target.Body.SecondaryHand.MaybeShield;
			if (shield != null) {
				Main.Log($"Calling Enchant: {enchantment.name}");
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
				Main.Log($"Removeing Enchant: {enchantment.name}");
				item.ArmorComponent.RemoveEnchantment(itemEnchantment);
			}
			Main.Log($"Adding Enchant: {enchantment.name} to {item.Name}");
			item.ArmorComponent.AddEnchantment(enchantment, context, new Rounds?(duration));
			Main.Log($"Added");

			foreach (var enchant in item.Enchantments) {
				Main.Log($"Enchantment {enchant.Blueprint.name}");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("Enchantment")]
		public BlueprintItemEnchantmentReference[] m_Enchantment;

		public ContextValue EnchantLevel;

		public ContextDurationValue DurationValue;
	}
}
