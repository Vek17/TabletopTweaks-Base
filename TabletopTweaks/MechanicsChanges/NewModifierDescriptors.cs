using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.MechanicsChanges {
	public enum ExtraModifierDescriptor : int {
		NaturalArmorBonus = ModifierDescriptor.NaturalArmor,
		NaturalArmorSize = 1717,
		NaturalArmorStackable = 1718
	}

	public class AdditionalModifierDescriptors {
		[PostPatchInitialize]
		static void Update_ModifiableValueArmorClass_FilterIsArmor() {
			Func<ModifiableValue.Modifier, bool> newFilterIsArmor = delegate (ModifiableValue.Modifier m) {
				ModifierDescriptor modDescriptor = m.ModDescriptor;
				return
					modDescriptor == ModifierDescriptor.Armor ||
					modDescriptor == ModifierDescriptor.ArmorEnhancement ||
					modDescriptor == ModifierDescriptor.ArmorFocus ||
					modDescriptor == (ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorBonus ||
					modDescriptor == (ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorSize ||
					modDescriptor == (ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorStackable ||
					modDescriptor == ModifierDescriptor.NaturalArmorEnhancement;
			};

			var FilterIsArmor = AccessTools.Field(typeof(ModifiableValueArmorClass), "FilterIsArmor");
			FilterIsArmor.SetValue(null, newFilterIsArmor);
		}

		[PostPatchInitialize]
		static void Update_ModifierDescriptorComparer_SortedValues() {
			ModifierDescriptorComparer.SortedValues = ModifierDescriptorComparer
				.SortedValues.InsertAfterElement(
					(ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorSize, 
					(ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorBonus);
			ModifierDescriptorComparer.SortedValues = ModifierDescriptorComparer
				.SortedValues.InsertBeforeElement(
					(ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorStackable, 
					(ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorBonus);
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
				ModifierDescriptorComparer.SortedValues.ForEach(v => Main.Log($"{(int)v}"));

				__result = order.Get(x).CompareTo(order.Get(y));
				Main.Log($"");
				return false;
			}
		}

		[HarmonyPatch(typeof(AbilityModifiersStrings), "GetName", new Type[] { typeof(ModifierDescriptor) })]
		static class AbilityModifierStrings_GetName_Patch {
			static void Postfix(ModifierDescriptor descriptor, ref string __result) {
				switch (descriptor) {
					case (ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorBonus:
						if (!Resources.Fixes.DisableNaturalArmorStacking) { break; }
						__result = "Natrual armor bonus";
						break;
					case (ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorSize:
						__result = "Natrual armor size";
						break;
					case (ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorStackable:
						__result = "Natrual armor";
						break;
				}
			}
		}

		[HarmonyPatch(typeof(AbilityModifiersStrings), "GetDescription", new Type[] { typeof(ModifierDescriptor) })]
		static class AbilityModifierStrings_GetDescription_Patch {
			static void Postfix(ModifierDescriptor descriptor, ref string __result) {
				switch (descriptor) {
					case (ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorBonus:
						if (!Resources.Fixes.DisableNaturalArmorStacking) { break; }
						__result = "Natrual armor bonus";
						break;
					case (ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorSize:
						__result = "Natrual armor size";
						break;
					case (ModifierDescriptor)ExtraModifierDescriptor.NaturalArmorStackable:
						__result = "Natrual armor stackable";
						break;
				}
			}
		}
	}
}
