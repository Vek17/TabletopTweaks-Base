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
        public enum Dodge : int {
            Strength = 2121,
            Dexterity = 2122,
            Constitution = 2123,
            Intelligence = 2124,
            Wisdom = 2125,
            Charisma = 2126
        }
        public enum Untyped : int {
            Strength = 3121,
            Dexterity = 3122,
            Constitution = 3123,
            Intelligence = 3124,
            Wisdom = 3125,
            Charisma = 3126,
            WeaponTraining = 3127,
            WeaponFocus = 3128
        }
        public enum SpellFocus : int {
            Normal = 4121,
            Greater = 4122,
            Mastery = 4123
        }

        private static class FilterAdjustments {
            private static readonly Func<ModifiableValue.Modifier, bool> FilterIsDodgeOriginal = ModifiableValueArmorClass.FilterIsDodge;
            private static readonly Func<ModifiableValue.Modifier, bool> FilterIsArmorOriginal = ModifiableValueArmorClass.FilterIsArmor;

            [PostPatchInitialize]
            static void Update_ModifiableValueArmorClass_FilterIsArmor() {
                Func<ModifiableValue.Modifier, bool> newFilterIsArmor = delegate (ModifiableValue.Modifier m) {
                    ModifierDescriptor modDescriptor = m.ModDescriptor;
                    return
                        FilterIsArmorOriginal(m)
                        || modDescriptor == ModifierDescriptor.NaturalArmorForm
                        || modDescriptor == (ModifierDescriptor)NaturalArmor.Bonus
                        || modDescriptor == (ModifierDescriptor)NaturalArmor.Size
                        || modDescriptor == (ModifierDescriptor)NaturalArmor.Stackable;
                };
                var FilterIsArmor = AccessTools.Field(typeof(ModifiableValueArmorClass), "FilterIsArmor");
                FilterIsArmor.SetValue(null, newFilterIsArmor);
            }

            [PostPatchInitialize]
            static void Update_ModifiableValueArmorClass_FilterIsDodge() {
                Func<ModifiableValue.Modifier, bool> newFilterIsDodge = delegate (ModifiableValue.Modifier m) {
                    ModifierDescriptor modDescriptor = m.ModDescriptor;
                    return
                        FilterIsDodgeOriginal(m)
                        || modDescriptor == (ModifierDescriptor)Dodge.Strength
                        || modDescriptor == (ModifierDescriptor)Dodge.Dexterity
                        || modDescriptor == (ModifierDescriptor)Dodge.Constitution
                        || modDescriptor == (ModifierDescriptor)Dodge.Intelligence
                        || modDescriptor == (ModifierDescriptor)Dodge.Wisdom
                        || modDescriptor == (ModifierDescriptor)Dodge.Charisma;
                };
                var FilterIsDodge = AccessTools.Field(typeof(ModifiableValueArmorClass), "FilterIsDodge");
                FilterIsDodge.SetValue(null, newFilterIsDodge);
            }
        }

        [PostPatchInitialize]
        static void Update_ModifierDescriptorComparer_SortedValues() {
            InsertAfter((ModifierDescriptor)NaturalArmor.Size, (ModifierDescriptor)NaturalArmor.Bonus);
            InsertBefore((ModifierDescriptor)NaturalArmor.Stackable, (ModifierDescriptor)NaturalArmor.Bonus);
            InsertBefore((ModifierDescriptor)Dodge.Strength, ModifierDescriptor.Dodge);
            InsertBefore((ModifierDescriptor)Dodge.Dexterity, ModifierDescriptor.Dodge);
            InsertBefore((ModifierDescriptor)Dodge.Constitution, ModifierDescriptor.Dodge);
            InsertBefore((ModifierDescriptor)Dodge.Intelligence, ModifierDescriptor.Dodge);
            InsertBefore((ModifierDescriptor)Dodge.Wisdom, ModifierDescriptor.Dodge);
            InsertBefore((ModifierDescriptor)Dodge.Charisma, ModifierDescriptor.Dodge);
            InsertAfter((ModifierDescriptor)Untyped.Strength, ModifierDescriptor.UntypedStackable);
            InsertAfter((ModifierDescriptor)Untyped.Dexterity, ModifierDescriptor.UntypedStackable);
            InsertAfter((ModifierDescriptor)Untyped.Constitution, ModifierDescriptor.UntypedStackable);
            InsertAfter((ModifierDescriptor)Untyped.Intelligence, ModifierDescriptor.UntypedStackable);
            InsertAfter((ModifierDescriptor)Untyped.Wisdom, ModifierDescriptor.UntypedStackable);
            InsertAfter((ModifierDescriptor)Untyped.Charisma, ModifierDescriptor.UntypedStackable);
            InsertAfter((ModifierDescriptor)Untyped.WeaponTraining, ModifierDescriptor.UntypedStackable);
            InsertAfter((ModifierDescriptor)Untyped.WeaponFocus, ModifierDescriptor.UntypedStackable);

            void InsertBefore(ModifierDescriptor value, ModifierDescriptor before) {
                ModifierDescriptorComparer.SortedValues = ModifierDescriptorComparer
                .SortedValues.InsertBeforeElement(value, before);
            };
            void InsertAfter(ModifierDescriptor value, ModifierDescriptor after) {
                ModifierDescriptorComparer.SortedValues = ModifierDescriptorComparer
                .SortedValues.InsertAfterElement(value, after);
            };
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
                        if (ModSettings.Fixes.BaseFixes.IsDisabled("DisableNaturalArmorStacking")) { break; }
                        __result = "Natural armor bonus";
                        break;
                    case (ModifierDescriptor)NaturalArmor.Size:
                        __result = "Natural armor size";
                        break;
                    case (ModifierDescriptor)NaturalArmor.Stackable:
                        __result = "Natural armor stackable";
                        break;
                    case (ModifierDescriptor)Dodge.Strength:
                    case (ModifierDescriptor)Dodge.Dexterity:
                    case (ModifierDescriptor)Dodge.Constitution:
                    case (ModifierDescriptor)Dodge.Intelligence:
                    case (ModifierDescriptor)Dodge.Wisdom:
                    case (ModifierDescriptor)Dodge.Charisma:
                        __result = "Dodge";
                        break;
                    case (ModifierDescriptor)Untyped.Strength:
                    case (ModifierDescriptor)Untyped.Dexterity:
                    case (ModifierDescriptor)Untyped.Constitution:
                    case (ModifierDescriptor)Untyped.Intelligence:
                    case (ModifierDescriptor)Untyped.Wisdom:
                    case (ModifierDescriptor)Untyped.Charisma:
                    case (ModifierDescriptor)Untyped.WeaponTraining:
                    case (ModifierDescriptor)Untyped.WeaponFocus:
                        __result = "Other";
                        break;
                    case (ModifierDescriptor)SpellFocus.Normal:
                    case (ModifierDescriptor)SpellFocus.Greater:
                    case (ModifierDescriptor)SpellFocus.Mastery:
                        __result = "Feat";
                        break;
                }
            }
        }
    }
}
