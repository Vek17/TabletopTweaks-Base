using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    static class Aeon {

        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Aeon Resources");

                PatchAeonDemythication();
                PatchAeonTenthLevelImmunities();
                PatchPowerOfLaw();

                void PatchAeonDemythication() {
                    if (TTTContext.Fixes.Aeon.IsDisabled("AeonDemythication")) { return; }
                    var AeonDemythicationBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("3c8a543e5b4e7154bb2cbe4d102a1604");
                    QuickFixTools.ReplaceSuppression(AeonDemythicationBuff, TTTContext, true);
                }
                void PatchAeonTenthLevelImmunities() {
                    if (TTTContext.Fixes.Aeon.IsDisabled("AeonTenthLevelImmunities")) { return; }
                    var AeonTenthLevelImmunities = BlueprintTools.GetBlueprint<BlueprintFeature>("711f6abfab877d342af9743a11c8f3aa");
                    AeonTenthLevelImmunities.RemoveComponents<ModifyD20>(c => c.Rule == RuleType.SavingThrow);
                    AeonTenthLevelImmunities.AddComponent<ModifySavingThrowD20>(c => {
                        c.AgainstAlignment = true;
                        c.Alignment = AlignmentComponent.Chaotic;
                        c.Replace = true;
                        c.Roll = 20;
                    });
                    AeonTenthLevelImmunities.AddComponent<IgnoreEnergyImmunityOnTarget>(c => {
                        c.CheckTargetAlignment = true;
                        c.Alignment = AlignmentComponent.Chaotic;
                        c.AllTypes = true;
                    });
                    TTTContext.Logger.LogPatch(AeonTenthLevelImmunities);
                }
                void PatchPowerOfLaw() {
                    if (TTTContext.Fixes.Aeon.IsDisabled("PowerOfLaw")) { return; }
                    var AeonPowerOfLawGazeAllyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1bf6049b6068400e8ac4e98e2e07b4f2");
                    var AeonPowerOfLawGazeEnemyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("648edba195d548f496c9367ddb4e2719");

                    AeonPowerOfLawGazeAllyBuff.RemoveComponents<ModifyD20>();
                    AeonPowerOfLawGazeAllyBuff.AddComponent<AeonPowerOfLaw>(c => {
                        c.RollResult = new ContextValue() {
                            ValueType = ContextValueType.Shared
                        };
                        c.RollCondition = ModifyD20.RollConditionType.ShouldBeLessOrEqualThan;
                        c.ValueToCompareRoll = new ContextValue() {
                            ValueType = ContextValueType.Shared,
                            ValueShared = AbilitySharedValue.Heal
                        };
                    });

                    AeonPowerOfLawGazeEnemyBuff.RemoveComponents<ModifyD20>();
                    AeonPowerOfLawGazeEnemyBuff.AddComponent<AeonPowerOfLaw>(c => {
                        c.RollResult = new ContextValue() {
                            ValueType = ContextValueType.Shared,
                            ValueShared = AbilitySharedValue.Heal
                        };
                        c.RollCondition = ModifyD20.RollConditionType.ShouldBeMoreOrEqualThan;
                        c.ValueToCompareRoll = new ContextValue() {
                            ValueType = ContextValueType.Shared,
                            ValueShared = AbilitySharedValue.Duration
                        };
                    });
                }
            }
        }
    }
}
