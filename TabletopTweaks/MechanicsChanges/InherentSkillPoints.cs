using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using System;
using TabletopTweaks.Config;

namespace TabletopTweaks.MechanicsChanges {
    class InherentSkillPoints {
        [HarmonyPatch(typeof(ModifiableValueAttributeStat), "CalculatePermanentValueWithoutEnhancement")]
        static class ModifierDescriptorComparer_Compare_Patch {
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
    }
}
