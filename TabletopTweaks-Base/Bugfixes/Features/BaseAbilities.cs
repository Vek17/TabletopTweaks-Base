using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Features {
    class BaseAbilities {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Abilities");
                PatchMobilityUseAbility();

                void PatchMobilityUseAbility() {
                    var MobilityUseAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("4be5757b85af47545a5789f1d03abda9");
                    MobilityUseAbility.DeactivateIfCombatEnded = false;

                    TTTContext.Logger.LogPatch("Patched", MobilityUseAbility);
                }
            }
        }
    }
}
