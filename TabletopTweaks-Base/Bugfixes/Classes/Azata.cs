using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class Azata {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Azata Resources");

                PatchSongOfCourageousDefender();

                void PatchSongOfCourageousDefender() {
                    if (TTTContext.Fixes.Azata.IsDisabled("SongOfCourageousDefender")) { return; }

                    var SongOfCourageousDefenderBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5b583771e65b7dd43b999ecc8783b341");
                    var SongOfCourageousDefenderArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("87e410da87176964093b6e237c299ffb");
                    var SongOfCourageousDefenderEffectBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("2c51835b2057101408b023d10235c969");
                    var SongOfCourageousDefenderEnemyEffectBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("3c3c89f2b79a4eb4b3e0c2ff77a17ea9");
                    var SongOfCourageousDefenderChoseCompanionAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("1e9d632ff09f4b3387467ceb827a6c01");
                    var SongOfCourageousDefenderCompanionBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f4300d92847d489ba394896a41a7ca1b");

                    SongOfCourageousDefenderChoseCompanionAbility.TemporaryContext(bp => {
                        bp.GetComponent<AbilityEffectRunAction>()?.TemporaryContext(c => {
                            c.Actions = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    m_Buff = SongOfCourageousDefenderCompanionBuff.ToReference<BlueprintBuffReference>(),
                                    Permanent = true,
                                    IsNotDispelable = true,
                                    DurationValue = new ContextDurationValue() {
                                        DiceCountValue = new ContextValue(),
                                        BonusValue = new ContextValue()
                                    }
                                }
                            );
                        });
                        bp.AddComponent<BlockSpellDuplicationComponent>();
                    });
                    SongOfCourageousDefenderCompanionBuff.TemporaryContext(bp => {
                        bp.SetName(SongOfCourageousDefenderChoseCompanionAbility.m_DisplayName);
                        bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
                    });
                    SongOfCourageousDefenderArea.TemporaryContext(bp => {
                        bp.SetComponents();
                        bp.AddComponent<AbilityAreaEffectBuff>(c => {
                            c.m_Buff = SongOfCourageousDefenderEffectBuff;
                            c.Condition = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    new ContextConditionIsAlly(),
                                    new ContextConditionHasBuffFromCaster() {
                                        m_Buff = SongOfCourageousDefenderCompanionBuff.ToReference<BlueprintBuffReference>()
                                    }
                                }
                            };
                            c.CheckConditionEveryRound = true;
                        });
                        bp.AddComponent<AbilityAreaEffectRunAction>(c => {
                            c.UnitEnter = Helpers.CreateActionList(
                                new Conditional() {
                                    ConditionsChecker = new ConditionsChecker() {
                                        Conditions = new Condition[] {
                                            new ContextConditionIsEnemy()
                                        }
                                    },
                                    IfFalse = Helpers.CreateActionList(),
                                    IfTrue = Helpers.CreateActionList(
                                        new ContextActionSavingThrow() {
                                            Type = SavingThrowType.Will,
                                            CustomDC = new ContextValue(),
                                            Actions = Helpers.CreateActionList(
                                                new ContextActionConditionalSaved() {
                                                    Succeed = Helpers.CreateActionList(),
                                                    Failed = Helpers.CreateActionList(
                                                        new ContextActionApplyBuff() {
                                                            m_Buff = SongOfCourageousDefenderEnemyEffectBuff,
                                                            Permanent = true,
                                                            IsNotDispelable = true,
                                                            AsChild = true,
                                                            DurationValue = new ContextDurationValue() {
                                                                DiceCountValue = new ContextValue(),
                                                                BonusValue = new ContextValue()
                                                            }
                                                        }
                                                    )
                                                }
                                            )
                                        }
                                    )
                                }
                            );
                            c.UnitExit = Helpers.CreateActionList(
                                new ContextActionRemoveBuff() {
                                    m_Buff = SongOfCourageousDefenderEnemyEffectBuff
                                }
                            );
                            c.Round = Helpers.CreateActionList();
                            c.UnitMove = Helpers.CreateActionList();
                        });
                    });

                    TTTContext.Logger.LogPatch(SongOfCourageousDefenderChoseCompanionAbility);
                    TTTContext.Logger.LogPatch(SongOfCourageousDefenderCompanionBuff);
                    TTTContext.Logger.LogPatch(SongOfCourageousDefenderArea);
                }
            }
        }
    }
}
