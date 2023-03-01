using HarmonyLib;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;
using System.Linq;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.UI {
    static class SpellTooltips {
        [HarmonyPatch(typeof(TooltipTemplateAbility), "GetHeader", new Type[] { typeof(TooltipTemplateType) })]
        class DisplaySpellbookInSpellTooltips {
            static void Postfix(TooltipTemplateAbility __instance, ref IEnumerable<ITooltipBrick> __result) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("DisplaySpellbookInTooltips")) { return; }
                if (__instance.m_AbilityData != null
                    && __instance.m_AbilityData.Spellbook != null
                    && __instance.m_AbilityData.Blueprint.Type == AbilityType.Spell) {
                    var spellbookName = __instance.m_AbilityData.Spellbook.Blueprint.DisplayName;
                    var newTypeName = string.IsNullOrEmpty(spellbookName) ? __instance.m_Type : $"{__instance.m_Type} — {spellbookName}";
                    var list = __result.ToList();
                    if (list.Count > 0) {
                        list[0] = new TooltipBrickEntityHeader(__instance.m_Name, __instance.m_Icon, newTypeName, __instance.m_School, __instance.m_Level, isItem: false);
                    }
                    __result = list;
                }
            }
        }
    }
}
