using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Aeon {

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Aeon Resources");

                PatchAeonDemythication();
                PatchAeonTenthLevelImmunities();
                PatchPowerOfLaw();

                void PatchAeonDemythication() {
                    if (ModSettings.Fixes.Aeon.IsDisabled("AeonDemythication")) { return; }
                    var AeonDemythicationBuff = Resources.GetBlueprint<BlueprintBuff>("3c8a543e5b4e7154bb2cbe4d102a1604");
                    QuickFixTools.ReplaceSuppression(AeonDemythicationBuff, true);
                }
                void PatchAeonTenthLevelImmunities() {
                    if (ModSettings.Fixes.Aeon.IsDisabled("AeonTenthLevelImmunities")) { return; }
                    var AeonTenthLevelImmunities = Resources.GetBlueprint<BlueprintFeature>("711f6abfab877d342af9743a11c8f3aa");
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
                    Main.LogPatch(AeonTenthLevelImmunities);
                }
                void PatchPowerOfLaw() {
                    if (ModSettings.Fixes.Aeon.IsDisabled("PowerOfLaw")) { return; }
                    var AeonPowerOfLawGazeAllyBuff = Resources.GetBlueprint<BlueprintBuff>("1bf6049b6068400e8ac4e98e2e07b4f2");
                    var AeonPowerOfLawGazeEnemyBuff = Resources.GetBlueprint<BlueprintBuff>("648edba195d548f496c9367ddb4e2719");

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
