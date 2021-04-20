using HarmonyLib;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewUnitParts {
    class MeleeTouchReach : UnitPart {
        public int Reach = 0;
        public ModifiableValue TouchValue {
            get {
                if (m_touchRange == null) {
                    m_touchRange = new ModifiableValue(Owner.Stats, (StatType)500);
                }
                return m_touchRange;
            }
        }
        public int GetModifiedValue() {
            return TouchValue.ModifiedValueRaw;
        }
        public void AddModifier(int value, EntityFactComponent source, ModifierDescriptor desc = ModifierDescriptor.None) {
            TouchValue.AddModifier(value, source, desc);
            
        }
        public void RemoveModifiersFrom(EntityFactComponent source) {
            TouchValue.RemoveModifiersFrom(source);
            TryRemovePart();
        }
        private void TryRemovePart() {
            if (TouchValue.ModifierList.Count == 0) {
                base.Owner.Remove<MeleeTouchReach>();
            }
        }

        private ModifiableValue m_touchRange;
    }
}

[HarmonyPatch(typeof(AbilityData), "GetApproachDistance", new Type[] { typeof(UnitEntityData) })]
static class BlueprintAbility_GetRange_Patch {
    static void Postfix(ref float __result, AbilityData __instance) {
        if (__instance.Blueprint.Range == AbilityRange.Touch && !__instance.HasMetamagic(Metamagic.Reach) && __instance.Caster.Unit.Get<MeleeTouchReach>() != null) {
            __result += __instance.Caster.Unit.Get<MeleeTouchReach>().GetModifiedValue().Feet().Meters;
        }
    }
}