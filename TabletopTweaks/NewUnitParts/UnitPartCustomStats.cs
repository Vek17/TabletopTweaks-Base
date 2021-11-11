using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static TabletopTweaks.NewUnitParts.CustomStatTypes;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartCustomStats : OldStyleUnitPart {

        public ModifiableValue GetCustomStat(CustomStatType type) {
            ModifiableValue MechanicsFeature;
            CustomStats.TryGetValue(type, out MechanicsFeature);
            if (MechanicsFeature == null) {
                MechanicsFeature = new ModifiableValue(Owner.Stats, type.Stat());
                CustomStats[type] = MechanicsFeature;
            }
            return MechanicsFeature;
        }

        public readonly Dictionary<CustomStatType, ModifiableValue> CustomStats = new Dictionary<CustomStatType, ModifiableValue>();

        //[HarmonyPatch(typeof(CharacterStats), nameof(CharacterStats.GetStat), new Type[] { typeof(StatType) })]
        [HarmonyPatch]
        static class CharacterStats_GetStat_CustomStatType_Patch {
            static MethodBase TargetMethod() {
                return AccessTools.GetDeclaredMethods(typeof(CharacterStats))
                    .Where(method => method.ReturnType == typeof(ModifiableValue)
                    && method.Name.Equals(nameof(CharacterStats.GetStat)))
                    .First();
            }
            static bool Prefix(CharacterStats __instance, StatType type, ref ModifiableValue __result) {
                if (type.IsCustom()) {
                    __result = __instance.Owner.Ensure<UnitPartCustomStats>().GetCustomStat(type.CustomStat());
                    return false;
                }
                return true;
            }
        }
        //Implementation for touch reach
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.GetApproachDistance), new Type[] { typeof(UnitEntityData) })]
        static class BlueprintAbility_GetRange_MeleeTouchReach_Patch {
            static void Postfix(ref float __result, AbilityData __instance) {
                if (__instance.Blueprint.Range == AbilityRange.Touch && !__instance.HasMetamagic(Metamagic.Reach)) {
                    __result += __instance.Caster.Unit.Stats.GetStat(CustomStatType.MeleeTouchReach.Stat()).ModifiedValue.Feet().Meters;
                }
            }
        }
    }
    static class CustomStatTypes {
        //If using new values decalre higher than 2531
        public enum CustomStatType : int {
            MeleeTouchReach = 2531,
        }
        public static StatType Stat(this CustomStatType stat) {
            return (StatType)stat;
        }
        public static CustomStatType CustomStat(this StatType stat) {
            return (CustomStatType)stat;
        }
        public static bool IsCustom(this StatType stat) {
            return (int)stat >= 2531;
        }
    }
}
