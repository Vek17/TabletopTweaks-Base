using HarmonyLib;
using Kingmaker.UI.MVVM._VM.ServiceWindows.Spellbook.MemorizingPanel;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.UI {
    internal class MythicSpellBookMemorizingUI {
        [HarmonyPatch(typeof(SpellbookMemorizingPanelVM), "CollectLevelData")]
        class SpellbookMemorizingPanelVM_CollectLevelData_Patch {
            static void Postfix(SpellbookMemorizingPanelVM __instance) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("FixMythicSpellbookSlotsUI")) { return; }
                if (__instance.m_CurrentSpellbook.Value.IsMythic) {
                    __instance.NotEnoughStat = false;
                }
            }
        }
    }
}
