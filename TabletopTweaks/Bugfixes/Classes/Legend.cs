using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;

namespace TabletopTweaks.MechanicsChanges
{
    class Legend {
        static bool Initialized;

        static BlueprintCharacterClass LegendClass;

        static void Init() {
            if (Initialized) { return; }

            Initialized = true;

            LegendClass = Resources.GetBlueprint<BlueprintCharacterClass>("3d420403f3e7340499931324640efe96");
        }

        static bool IsLegend(UnitEntityData unit) {
            Init();

            if (LegendClass == null) {
                return false;
            }

            if (unit == null) {
                return false;
            }

            if (unit.Progression?.LastMythicClass != LegendClass) {
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(Spellbook), "CasterLevel", MethodType.Getter)]
        static class Spellbook_CasterLevel_Patch {
            static void Postfix(Spellbook __instance, ref int __result) {
                if (ModSettings.Fixes.Legend.IsDisabled("CasterLevel")) {
                    return;
                }

                var unit = __instance.Owner.Unit;

                if (!unit.IsMainCharacter) {
                    return;
                }

                if (!IsLegend(unit)) {
                    return;
                }

                // normally, caster level is capped at 20
                // let legend use real caster level
                // TODO: review ApplySpellbook.TryApplyCustomSpells
                __result = __instance.m_BaseLevelInternal;
            }
        }

    }
}
