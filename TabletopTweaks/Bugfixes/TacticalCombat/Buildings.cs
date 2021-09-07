using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;

namespace TabletopTweaks.Bugfixes.TacticalCombat {
    class Buildings {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Buildings");
                PatchTrainingGrounds();

                void PatchTrainingGrounds() {
                    var ArmyBuildingTrainingGrounds = Resources.GetBlueprint<BlueprintFeature>("b1ab3085e85243e8a13f6acf78023920");
                    ArmyBuildingTrainingGrounds.RemoveComponents<OutcomingDamageAndHealingModifier>();
                    ArmyBuildingTrainingGrounds.AddComponent<OutcomingAdditionalDamageAndHealingModifier>(c => {
                        c.Type = OutcomingAdditionalDamageAndHealingModifier.ModifyingType.OnlyDamage;
                        c.ModifierPercents = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    });

                    Main.LogPatch("Patched", ArmyBuildingTrainingGrounds);
                }
            }
        }
    }
}
