using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.MechanicsChanges;

namespace TabletopTweaks.NewUnitParts {
    [TypeId("cbb3b43ab725484e81a261e0b64d07d8")]
    class UnitPartAeonGazeManager : OldStyleUnitPart {

        public void AddEntry(EntityFact source) {
            GazeEntry gaze = new GazeEntry {
                Source = source
            };
            ActiveGazes.Add(gaze);
            TrySpend();
        }
        public void RemoveEntry(EntityFact source) {
            ActiveGazes.RemoveAll((GazeEntry gaze) => gaze.Source == source);
            TryRemove();
        }
        private void TryRemove() {
            if (!ActiveGazes.Any()) { this.RemoveSelf(); }
        }
        public bool IsActive() {
            return ActiveGazes.Any();
        }
        public void TrySpend() {
            if (!HasSpent && !Owner.HasFact(limitlessFeature)) {
                Owner.Resources.Spend(spendResource, 1);
            }
            HasSpent = true;
        }

        private readonly BlueprintAbilityResourceReference spendResource = 
            Resources.GetBlueprint<BlueprintAbilityResource>("905722fe39d87474aa6d41bffa327ff3").ToReference<BlueprintAbilityResourceReference>();
        private readonly BlueprintFeatureReference limitlessFeature = 
            Resources.GetBlueprint<BlueprintAbilityResource>("a2f5852d76a165f4d8d6fe670e8013fb").ToReference< BlueprintFeatureReference>();
        private bool HasSpent = false;
        private readonly List<GazeEntry> ActiveGazes = new();
        public class GazeEntry {
            public EntityFact Source;
        }

        [HarmonyPatch(typeof(ActivatableAbilityResourceLogic), "IsAvailable")]
        class ActivatableAbilityResourceLogic_IsAvailable_PerfectCritical_Patch {
            static void Postfix(ActivatableAbilityResourceLogic __instance, ref bool __result, EntityFactComponent runtime) {
                using (runtime.RequestEventContext()) {
                    if (__instance.RequiredResource && __instance.SpendType == ActivatableAbilitySpendLogic.StandardSpendType.AeonGaze.ToResourceType()) {
                        var AeonGazeController = __instance.Owner.Get<UnitPartAeonGazeManager>();
                        if (AeonGazeController == null) {
                            __result = __instance.Owner.Resources.HasEnoughResource(__instance.RequiredResource, 1);
                            return;
                        }
                        __result = __instance.Owner.Resources.HasEnoughResource(__instance.RequiredResource,(!AeonGazeController.IsActive()) ? 1 : 0);
                    }
                }
            }
        }
    }
}
