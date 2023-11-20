using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs;
using System;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.MechanicsChanges {
    internal class InherentBonuses {
        [HarmonyPatch(typeof(ModifiableValueAttributeStat), "CalculatePermanentValueWithoutEnhancement")]
        static class ModifierDescriptorComparer_InherentSkillPoint_Compare_Patch {
            private static readonly Func<ModifiableValue.Modifier, bool> FilterGrantsSkillpoints = delegate (ModifiableValue.Modifier m) {
                ModifierDescriptor modDescriptor = m.ModDescriptor;
                return
                    ModifiableValue.FilterIsRacialOrInherent(m)
                    || m.ModDescriptor == ModifierDescriptor.Inherent;
            };

            static void Postfix(ModifiableValueAttributeStat __instance, ref int __result) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixInherentSkillpoints")) { return; }
                __result = __instance.ApplyModifiersFiltered(__instance.CalculateBaseValue(__instance.BaseValue), FilterGrantsSkillpoints);
            }
        }

        private static readonly Func<ModifiableValue.Modifier, bool> FilterIsPermanentOriginal = ModifiableValue.FilterIsPermanent;
        [PostPatchInitialize]
        static void Update_ModifiableValue_FilterIsPermanent() {
            if (TTTContext.Fixes.BaseFixes.IsDisabled("FixInherentBonuses")) { return; }
            Func<ModifiableValue.Modifier, bool> newFilterIsPermanent = delegate (ModifiableValue.Modifier m) {
                ModifierDescriptor modDescriptor = m.ModDescriptor;
                return FilterIsPermanentOriginal(m) || (modDescriptor == ModifierDescriptor.Inherent && m.Source is not Buff);
            };
            var FilterIsPermanent = AccessTools.Field(typeof(ModifiableValue), "FilterIsPermanent");
            FilterIsPermanent.SetValue(null, newFilterIsPermanent);
            TTTContext.Logger.Log("Patched Inherit bonuses to be considered for feat prerequisites");
        }
    }
}
