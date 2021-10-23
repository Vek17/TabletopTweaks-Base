using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.MechanicsChanges {
    class InherentBonuses {
        [HarmonyPatch(typeof(ModifiableValueAttributeStat), "CalculatePermanentValueWithoutEnhancement")]
        static class ModifierDescriptorComparer_InherentSkillPoint_Compare_Patch {
            private static readonly Func<ModifiableValue.Modifier, bool> FilterGrantsSkillpoints = delegate (ModifiableValue.Modifier m) {
                ModifierDescriptor modDescriptor = m.ModDescriptor;
                return
                    ModifiableValue.FilterIsRacial(m)
                    || m.ModDescriptor == ModifierDescriptor.Inherent;
            };

            static void Postfix(ModifiableValueAttributeStat __instance, ref int __result) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("FixInherentSkillpoints")) { return; }
                __result = __instance.ApplyModifiersFiltered(__instance.CalculateBaseValue(__instance.BaseValue), FilterGrantsSkillpoints);
            }
        }
        
        private static readonly Func<ModifiableValue.Modifier, bool> FilterIsPermanentOriginal = ModifiableValue.FilterIsPermanent;
        [PostPatchInitialize]
        static void Update_ModifiableValue_FilterIsPermanent() {
            if (ModSettings.Fixes.BaseFixes.IsDisabled("FixInherentBonuses")) { return; }
            Func<ModifiableValue.Modifier, bool> newFilterIsPermanent = delegate (ModifiableValue.Modifier m) {
                ModifierDescriptor modDescriptor = m.ModDescriptor;
                return FilterIsPermanentOriginal(m) || modDescriptor == ModifierDescriptor.Inherent;
            };
            var FilterIsPermanent = AccessTools.Field(typeof(ModifiableValue), "FilterIsPermanent");
            FilterIsPermanent.SetValue(null, newFilterIsPermanent);
            Main.Log("Patched Inherit bonuses to be considered for feat prerequisites");
        }
    }
}
