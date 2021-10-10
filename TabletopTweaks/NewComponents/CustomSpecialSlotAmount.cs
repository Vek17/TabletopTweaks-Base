using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintSpellbook), false)]
    [TypeId("076df57c9d7d415c81a3b968437d98ec")]
    class CustomSpecialSlotAmount : BlueprintComponent {
        public int Amount = 1;
    }

    [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.CalcSlotsLimit))]
    static class Spellbook_CalcSlotsLimit_CustomSpecialSlotAmount_Patch {
        static void Postfix(Spellbook __instance, SpellSlotType slotType, ref int __result) {
            if (slotType != SpellSlotType.Domain && slotType != SpellSlotType.Favorite) { return; }
            var customComponent = __instance.Blueprint.GetComponent<CustomSpecialSlotAmount>();
            if (customComponent != null) {
                __result = customComponent.Amount;
            }
        }
    }
}
