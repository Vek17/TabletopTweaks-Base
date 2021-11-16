using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static TabletopTweaks.NewUnitParts.CustomStatTypes;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartCustomStats : OldStyleUnitPart {

        public override void OnTurnOn() {
            foreach (var stat in CustomStats) {
                stat.Value.m_Stats = Owner.Stats;
            }
        }

        private void UpdateOwner(ModifiableValue stat) {
            if (stat.m_Stats == null) {
                stat.m_Stats = Owner.Stats;
            }
        }

        public ModifiableValue GetCustomStat(CustomStatType type) {
            ModifiableValue stat;
            CustomStats.TryGetValue(type, out stat);
            if (stat == null) {
                stat = new ModifiableValue(Owner.Stats, type.Stat());
                CustomStats[type] = stat;
            }
            UpdateOwner(stat);
            return stat;
        }

        public ModifiableValueAttributeStat GetCustomAttribute(CustomStatType type) {
            ModifiableValue attribute;
            CustomStats.TryGetValue(type, out attribute);
            if (attribute == null) {
                attribute = new ModifiableValueAttributeStat(Owner.Stats, type.Stat());
                CustomStats[type] = attribute;
            }
            UpdateOwner(attribute);
            return attribute as ModifiableValueAttributeStat;
        }
        [JsonProperty]
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
                    if (type.IsCustomAttribute()) {
                        __result = __instance.Owner.Ensure<UnitPartCustomStats>().GetCustomAttribute(type.CustomStat());
                        return false;
                    }
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
        //If using new values stats are 10_000 - 1_000_000
        public enum CustomStatType : int {
            MeleeTouchReach = 10_000,
            BlackBladeEgo = 1_000_000,
            BlackBladeIntelligence = 1_000_001,
            BlackBladeWisdom = 1_000_002,
            BlackBladeCharisma = 1_000_003,
        }
        public static StatType Stat(this CustomStatType stat) {
            return (StatType)stat;
        }
        public static CustomStatType CustomStat(this StatType stat) {
            return (CustomStatType)stat;
        }
        public static bool IsCustom(this StatType stat) {
            return (int)stat >= 10_000;
        }
        public static bool IsCustomAttribute(this StatType stat) {
            return (int)stat >= 1_000_000;
        }
    }
}
