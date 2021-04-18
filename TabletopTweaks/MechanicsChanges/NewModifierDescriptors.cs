using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.MechanicsChanges {

	public class AdditionalModifierDescriptors {
		public enum NaturalArmor : int {
			Bonus = ModifierDescriptor.NaturalArmor,
			Size = 1717,
			Stackable = 1718
		}
		[PostPatchInitialize]
		static void Update_ModifiableValueArmorClass_FilterIsArmor() {
			Func<ModifiableValue.Modifier, bool> newFilterIsArmor = delegate (ModifiableValue.Modifier m) {
				ModifierDescriptor modDescriptor = m.ModDescriptor;
				return
					modDescriptor == ModifierDescriptor.Armor ||
					modDescriptor == ModifierDescriptor.ArmorEnhancement ||
					modDescriptor == ModifierDescriptor.ArmorFocus ||
					modDescriptor == (ModifierDescriptor)NaturalArmor.Bonus ||
					modDescriptor == (ModifierDescriptor)NaturalArmor.Size ||
					modDescriptor == (ModifierDescriptor)NaturalArmor.Stackable ||
					modDescriptor == ModifierDescriptor.NaturalArmorEnhancement;
			};

			var FilterIsArmor = AccessTools.Field(typeof(ModifiableValueArmorClass), "FilterIsArmor");
			FilterIsArmor.SetValue(null, newFilterIsArmor);
		}

		[PostPatchInitialize]
		static void Update_ModifierDescriptorComparer_SortedValues() {
			ModifierDescriptorComparer.SortedValues = ModifierDescriptorComparer
				.SortedValues.InsertAfterElement(
					(ModifierDescriptor)NaturalArmor.Size, 
					(ModifierDescriptor)NaturalArmor.Bonus);
			ModifierDescriptorComparer.SortedValues = ModifierDescriptorComparer
				.SortedValues.InsertBeforeElement(
					(ModifierDescriptor)NaturalArmor.Stackable, 
					(ModifierDescriptor)NaturalArmor.Bonus);
		}

		[HarmonyPatch(typeof(ModifierDescriptorComparer), "Compare", new Type[] { typeof(ModifierDescriptor), typeof(ModifierDescriptor) })]
		static class ModifierDescriptorComparer_Compare_Patch {
			static SortedDictionary<ModifierDescriptor, int> order;

			static bool Prefix(ModifierDescriptorComparer __instance, ModifierDescriptor x, ModifierDescriptor y, ref int __result) {
				if (order == null) {
					order = new SortedDictionary<ModifierDescriptor, int>();
					int i = 0;
					for (i = 0; i < ModifierDescriptorComparer.SortedValues.Length; i++) {
						order[ModifierDescriptorComparer.SortedValues[i]] = i;
					}
				}
				__result = order.Get(x).CompareTo(order.Get(y));
				return false;
			}
		}

		[HarmonyPatch(typeof(AbilityModifiersStrings), "GetName", new Type[] { typeof(ModifierDescriptor) })]
		static class AbilityModifierStrings_GetName_Patch {
			static void Postfix(ModifierDescriptor descriptor, ref string __result) {
				switch (descriptor) {
					case (ModifierDescriptor)NaturalArmor.Bonus:
						if (!Settings.Fixes.DisableNaturalArmorStacking) { break; }
						__result = "Natural armor bonus";
						break;
					case (ModifierDescriptor)NaturalArmor.Size:
						__result = "Natural armor size";
						break;
					case (ModifierDescriptor)NaturalArmor.Stackable:
						__result = "Natural armor";
						break;
				}
			}
		}

		[HarmonyPatch(typeof(AbilityModifiersStrings), "GetDescription", new Type[] { typeof(ModifierDescriptor) })]
		static class AbilityModifierStrings_GetDescription_Patch {
			static void Postfix(ModifierDescriptor descriptor, ref string __result) {
				switch (descriptor) {
					case (ModifierDescriptor)NaturalArmor.Bonus:
						if (!Settings.Fixes.DisableNaturalArmorStacking) { break; }
						__result = "Natrual armor bonus";
						break;
					case (ModifierDescriptor)NaturalArmor.Size:
						__result = "Natrual armor size";
						break;
					case (ModifierDescriptor)NaturalArmor.Stackable:
						__result = "Natrual armor stackable";
						break;
				}
			}
		}
	}
}
