using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.TacticalCombat {
    class Buildings {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Buildings");
                PatchTrainingGrounds();

                void PatchTrainingGrounds() {
                    if (TTTContext.Fixes.Crusade.Buildings.IsDisabled("TrainingGrounds")) { return; }
                    var ArmyBuildingTrainingGrounds = BlueprintTools.GetBlueprint<BlueprintFeature>("b1ab3085e85243e8a13f6acf78023920");
                    ArmyBuildingTrainingGrounds.RemoveComponents<OutcomingDamageAndHealingModifier>();
                    ArmyBuildingTrainingGrounds.SetComponents(
                        Helpers.Create<OutcomingAdditionalDamageAndHealingModifier>(c => {
                            c.Type = OutcomingAdditionalDamageAndHealingModifier.ModifyingType.OnlyDamage;
                            c.ModifierPercents = new ContextValue() {
                                ValueRank = AbilityRankType.DamageBonus,
                                ValueType = ContextValueType.Rank
                            };
                        }),
                        Helpers.CreateContextRankConfig(c => {
                            c.m_Type = AbilityRankType.DamageBonus;
                            c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                            c.m_Feature = ArmyBuildingTrainingGrounds.ToReference<BlueprintFeatureReference>();
                            c.m_FeatureList = new BlueprintFeatureReference[0];
                            c.m_Progression = ContextRankProgression.MultiplyByModifier;
                            c.m_StepLevel = 10;
                        })
                    );
                    TTTContext.Logger.LogPatch("Patched", ArmyBuildingTrainingGrounds);
                }
            }
        }
    }
}
