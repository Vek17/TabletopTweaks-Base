using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Parts;
using System;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.MechanicsChanges;
using static TabletopTweaks.MechanicsChanges.ActivatableAbilitySpendLogic;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Magus {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Magus Resources");

                PatchBase();
                PatchSwordSaint();
            }
            static void PatchBase() {
            }
            static void PatchSwordSaint() {
                PatchPerfectCritical();

                void PatchPerfectCritical() {
                    if (ModSettings.Fixes.Magus.Archetypes["SwordSaint"].IsDisabled("PerfectCritical")) { return; }

                    var SwordSaintPerfectStrikeCritAbility = Resources.GetBlueprint<BlueprintActivatableAbility>("c6559839738a7fc479aadc263ff9ffff");
                    
                    SwordSaintPerfectStrikeCritAbility.SetDescription("At 4th level, when a sword saint confirms a critical hit, " +
                        "he can spend 2 points from his arcane pool to increase his weapon's critical multiplier by 1.");
                    SwordSaintPerfectStrikeCritAbility
                        .GetComponent<ActivatableAbilityResourceLogic>()
                        .SpendType = CustomSpendType.Crit.Amount(2);
                    Main.LogPatch("Patched", SwordSaintPerfectStrikeCritAbility);
                }
            }
        }
        [HarmonyPatch(typeof(UnitPartMagus), "IsSpellFromMagusSpellList", new Type[] { typeof(AbilityData) })]
        class UnitPartMagus_IsSpellFromMagusSpellList_VarriantAbilities_Patch {
            static void Postfix(UnitPartMagus __instance, ref bool __result, AbilityData spell) {
                if (ModSettings.Fixes.Magus.Base.IsDisabled("SpellCombatAbilityVariants")) { return; }
                if (spell.ConvertedFrom != null) {
                    __result |= __instance.IsSpellFromMagusSpellList(spell.ConvertedFrom);
                }
            }
        }
    }
}