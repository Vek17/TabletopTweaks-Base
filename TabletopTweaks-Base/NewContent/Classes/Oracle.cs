using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Classes {
    class Oracle {
        public static void AddOracleFeatures() {
            var NaturesWhispersACConversion = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "NaturesWhispersACConversion", bp => {
                bp.SetName(TTTContext, "Natures Whispers AC Conversion");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.AddComponent<ReplaceStatBaseAttribute>(c => {
                    c.TargetStat = StatType.AC;
                    c.BaseAttributeReplacement = StatType.Charisma;
                });
                bp.AddComponent<ReplaceCMDDexterityStat>(c => {
                    c.NewStat = StatType.Charisma;
                });
                bp.AddComponent<ForceACUpdate>();
            });

            var HasteBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("03464790f40c3c24aa684b57155f3280");
            var SlowBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("0bc608c3f2b548b44b7146b7530613ac");
            var OracleRevelationFreezingSpellsBuff1 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "OracleRevelationFreezingSpellsBuff1", bp => {
                bp.SetName(TTTContext, "Freezing Spells Effect");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] { 
                                    new ContextConditionHasBuff() {
                                        m_Buff = HasteBuff
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new ContextActionRemoveBuffSingleStack() { 
                                    m_TargetBuff = HasteBuff
                                }
                            ),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    m_Buff = SlowBuff,
                                    DurationValue = new ContextDurationValue() { 
                                        DiceCountValue = 0,
                                        BonusValue = 1
                                    },
                                    IsFromSpell = true,
                                    AsChild = false
                                }
                            )
                        },
                        new ContextActionRemoveSelf()
                    );
                    c.Deactivated = Helpers.CreateActionList();
                    c.NewRound = Helpers.CreateActionList();
                });
            });
            var OracleRevelationFreezingSpellsBuff11 = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "OracleRevelationFreezingSpellsBuff11", bp => {
                bp.SetName(TTTContext, "Freezing Spells Effect");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    new ContextConditionHasBuff() {
                                        m_Buff = HasteBuff
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new ContextActionRemoveBuffSingleStack() {
                                    m_TargetBuff = HasteBuff
                                }
                            ),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    m_Buff = SlowBuff,
                                    DurationValue = new ContextDurationValue() {
                                        DiceType = DiceType.D4,
                                        DiceCountValue = 1,
                                        BonusValue = 0
                                    },
                                    IsFromSpell = true,
                                    AsChild = false
                                }
                            )
                        },
                        new ContextActionRemoveSelf()
                    );
                    c.Deactivated = Helpers.CreateActionList();
                    c.NewRound = Helpers.CreateActionList();
                });
            });
        }
    }
}
