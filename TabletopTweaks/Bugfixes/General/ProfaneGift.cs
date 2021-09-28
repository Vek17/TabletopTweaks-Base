using HarmonyLib;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Bugfixes.General {
    class ProfaneGift {
        [HarmonyPatch(typeof(AddNocticulaBonus), "OnTurnOn")]
        class UnitDescriptor_FixSizeModifiers_Patch {
            static bool Prefix(AddNocticulaBonus __instance) {
                var stats = new StatType[] {
                    StatType.Strength,
                    StatType.Dexterity,
                    StatType.Constitution,
                    StatType.Intelligence,
                    StatType.Wisdom,
                    StatType.Charisma
                };
                __instance.m_HighestStat = getHighestStat(__instance.Owner, stats);
                __instance.m_SecondHighestStat = getHighestStat(__instance.Owner, stats.Where(s => s != __instance.m_HighestStat));

                int primaryBonus = __instance.HighestStatBonus.Calculate(__instance.Context);
                int secondaryBonus = __instance.SecondHighestStatBonus.Calculate(__instance.Context);
                __instance.Owner.Stats
                    .GetStat(__instance.m_HighestStat)
                    .AddModifier(primaryBonus, __instance.Runtime, __instance.Descriptor);
                __instance.Owner.Stats
                    .GetStat(__instance.m_SecondHighestStat)
                    .AddModifier(secondaryBonus, __instance.Runtime, __instance.Descriptor);
                return false;
            }
            static private StatType getHighestStat(UnitEntityData unit, IEnumerable<StatType> stats) {
                StatType highestStat = StatType.Unknown;
                int highestValue = -1;
                foreach (StatType stat in stats) {
                    var value = unit.Stats.GetStat(stat).ModifiedValue;
                    if (value > highestValue) {
                        highestStat = stat;
                        highestValue = value;
                    }
                }
                return highestStat;
            }
        }
    }
}
