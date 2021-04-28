using HarmonyLib;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums;
using System;
using TabletopTweaks.Config;
using static TabletopTweaks.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.MechanicsChanges {
    class CannyDefenseStacking {
        [HarmonyPatch(typeof(CannyDefensePermanent), "ActivateModifier")]
        static class ModifierDescriptorHelper_IsStackable_Patch {

            static bool Prefix(ref CannyDefensePermanent __instance) {
                if (!ModSettings.Fixes.DisableCannyDefenseStacking) { return true; }
                int value = Math.Min(__instance.Owner.Stats.Intelligence.Bonus, __instance.Owner.Progression.GetClassLevel(__instance.CharacterClass));
                __instance.Owner.Stats.AC.AddModifierUnique(value, __instance.Runtime, (ModifierDescriptor)Dodge.Intelligence);
                return false;
            }
        }
    }
}
