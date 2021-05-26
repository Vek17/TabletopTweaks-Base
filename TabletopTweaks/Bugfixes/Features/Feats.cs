using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Features {
    class Feats {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Feats.DisableAll) { return; }
                Main.LogHeader("Patching Feats");
                PatchCraneWing();
                PatchFencingGrace();
                PatchSlashingGrace();
            }

            static void PatchCraneWing() {
                if (!ModSettings.Fixes.Feats.Enabled["CraneWing"]) { return; }
                BlueprintBuff CraneStyleBuff = Resources.GetBlueprint<BlueprintBuff>("e8ea7bd10136195478d8a5fc5a44c7da");
                var FightingDefensivlyTrigger = CraneStyleBuff.GetComponent<AddInitiatorAttackWithWeaponTrigger>();
                var Conditionals = FightingDefensivlyTrigger.Action.Actions.OfType<Conditional>();

                var newConditonal = Helpers.Create<ContextConditionHasFreeHand>();
                Conditionals.First().ConditionsChecker.Conditions = Conditionals.First().ConditionsChecker.Conditions.AddItem(newConditonal).ToArray();

                Main.LogPatch("Patched", CraneStyleBuff);
            }
            static void PatchFencingGrace() {
                if (!ModSettings.Fixes.Feats.Enabled["FencingGrace"]) { return; }
                var FencingGrace = Resources.GetBlueprint<BlueprintParametrizedFeature>("47b352ea0f73c354aba777945760b441");
                FencingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceEnforced>());
                Main.LogPatch("Patched", FencingGrace);
            }
            static void PatchSlashingGrace() {
                if (!ModSettings.Fixes.Feats.Enabled["SlashingGrace"]) { return; }
                var SlashingGrace = Resources.GetBlueprint<BlueprintParametrizedFeature>("697d64669eb2c0543abb9c9b07998a38");
                SlashingGrace.ReplaceComponents<DamageGrace>(Helpers.Create<DamageGraceEnforced>());
                Main.LogPatch("Patched", SlashingGrace);
            }
        }
    }
}
