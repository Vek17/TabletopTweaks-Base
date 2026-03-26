using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.UI.GenericSlot.EquipSlotBase;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Units {
    static class Enemies {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Bosses");
                PatchAnomaly();
                PatchBalors();
                PatchCarnivorousCrystals();
                PatchVescavors();
            }
        }
        static void PatchAnomaly() {
            if (TTTContext.Fixes.Units.Enemies.IsDisabled("Anomaly")) { return; }

            PatchAnomalyChaoticMind();

            void PatchAnomalyChaoticMind() {
                var AnomalyTemplateDefensive_ChaoticMindBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2159f35f1dfb4ee78da818f443a086ee");
                var AnomalyDistortionNormalDCProperty = BlueprintTools.GetBlueprintReference<BlueprintUnitPropertyReference>("0cc67f363c944539bb09217f2ba3e149");

                AnomalyTemplateDefensive_ChaoticMindBuff.TemporaryContext(bp => {
                    var OriginalTrigger = bp.GetComponent<AddAbilityUseTargetTrigger>();
                    bp.RemoveComponents<AddAbilityUseTargetTrigger>();
                    bp.AddComponent<AddAbilityUseTargetTriggerTTT>(c => {
                        c.ToCaster = true;
                        c.DontCheckType = true;
                        c.CheckDescriptor = true;
                        c.SpellDescriptor = SpellDescriptor.MindAffecting;
                        c.TriggerOnEffectApply = true;
                        c.TriggerEvenIfNoEffect = true;
                        c.Action = OriginalTrigger.Action;
                    });
                    bp.AddComponent<AddSpellImmunity>(c => {
                        c.Type = SpellImmunityType.SpellDescriptor;
                        c.SpellDescriptor = SpellDescriptor.MindAffecting;
                    });
                    bp.FlattenAllActions().OfType<ContextActionSavingThrow>()?.ForEach(a => {
                        a.HasCustomDC = true;
                        a.CustomDC = new ContextValue() {
                            ValueType = ContextValueType.CasterCustomProperty,
                            m_CustomProperty = AnomalyDistortionNormalDCProperty
                        };
                    });
                });

            }
        }
        static void PatchBalors() {
            if (TTTContext.Fixes.Units.Enemies.IsDisabled("Balors")) { return; }

            var BalorVorpalStrikeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("acc4a16c4088f2546b4237dcbb774f14");
            var BalorVorpalStrikeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5220bc4386bf3e147b1beb93b0b8b5e7");
            var Vorpal = BlueprintTools.GetBlueprintReference<BlueprintItemEnchantmentReference>("2f60bfcba52e48a479e4a69868e24ebc");

            BalorVorpalStrikeBuff.SetComponents();
            BalorVorpalStrikeBuff.AddComponent<BuffEnchantWornItem>(c => {
                c.m_EnchantmentBlueprint = Vorpal;
                c.Slot = SlotType.PrimaryHand;
            });
            BalorVorpalStrikeBuff.AddComponent<BuffEnchantWornItem>(c => {
                c.m_EnchantmentBlueprint = Vorpal;
                c.Slot = SlotType.SecondaryHand;
            });
            BalorVorpalStrikeFeature.AddComponent<RecalculateOnEquipmentChange>();

            TTTContext.Logger.LogPatch(BalorVorpalStrikeFeature);
            TTTContext.Logger.LogPatch(BalorVorpalStrikeBuff);
        }
        static void PatchCarnivorousCrystals() {
            if (TTTContext.Fixes.Units.Enemies.IsDisabled("CarnivorousCrystals")) { return; }

            var CarnivorousCrystalAreaEffectSubsonicHum = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("a89a5b1edba9c614b92a7ba7ab3f5a1d");
            var CarnivorousCrystalBuffSubsonicHum = BlueprintTools.GetBlueprint<BlueprintBuff>("8b981244028551c4e8b35c9d50352527");
            var CarnivorousCrystalBuffSubsonicHumImmunity = BlueprintTools.GetBlueprint<BlueprintBuff>("88e345f3233c8024e9d191a807c40223");
            var CarnivorousCrystalApplyImmunityAbility = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(TTTContext, "CarnivorousCrystalApplyImmunityAbility");
            var Stunned = BlueprintTools.GetBlueprint<BlueprintBuff>("09d39b38bb7c6014394b6daced9bacd3");

            CarnivorousCrystalAreaEffectSubsonicHum.SetComponents();
            CarnivorousCrystalAreaEffectSubsonicHum.TemporaryContext(bp => {
                bp.AddComponent<AbilityAreaEffectRunAction>(c => {
                    c.UnitEnter = Helpers.CreateActionList();
                    c.UnitExit = Helpers.CreateActionList();
                    c.UnitMove = Helpers.CreateActionList();
                    c.Round = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Operation = Operation.Or,
                                Conditions = new Condition[] {
                                    new ContextConditionHasBuffFromCaster(){
                                        m_Buff = CarnivorousCrystalBuffSubsonicHumImmunity.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionHasBuff(){
                                        m_Buff = CarnivorousCrystalBuffSubsonicHum.ToReference<BlueprintBuffReference>()
                                    },
                                }
                            },
                            IfTrue = Helpers.CreateActionList(),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionSavingThrow() {
                                    Type = SavingThrowType.Fortitude,
                                    CustomDC = new ContextValue(),
                                    Actions = Helpers.CreateActionList(
                                        new ContextActionConditionalSaved() {
                                            Succeed = Helpers.CreateActionList(
                                                new ContextActionCastSpell() {
                                                    m_Spell = CarnivorousCrystalApplyImmunityAbility,
                                                    DC = new ContextValue(),
                                                    SpellLevel = new ContextValue(),
                                                }
                                            ),
                                            Failed = Helpers.CreateActionList(
                                                new ContextActionApplyBuff() {
                                                    m_Buff = Stunned.ToReference<BlueprintBuffReference>(),
                                                    DurationValue = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        Rate = DurationRate.Rounds,
                                                        DiceType = DiceType.One,
                                                        DiceCountValue = 0,
                                                        BonusValue = 1
                                                    },
                                                    AsChild = true,
                                                    NotLinkToAreaEffect = true
                                                }
                                            )
                                        }
                                    )
                                }
                            )
                        }
                    );
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Sonic;
                });
                bp.AddComponent<ContextCalculateAbilityParams>(c => {
                    c.m_CustomProperty = new BlueprintUnitPropertyReference();
                    c.StatType = StatType.Constitution;
                    c.CasterLevel = 0;
                    c.SpellLevel = 0;
                });
            });

            TTTContext.Logger.LogPatch(CarnivorousCrystalAreaEffectSubsonicHum);
            TTTContext.Logger.LogPatch(CarnivorousCrystalBuffSubsonicHumImmunity);
        }
        static void PatchVescavors() {
            if (TTTContext.Fixes.Units.Enemies.IsDisabled("Vescavors")) { return; }

            var VescavorQueenGibberAreaEffect = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("acbb8f87c5d98164dbdc1aee0f9eda2b");
            var VescavorSwarmGibberAreaEffect = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("a80c90f3223d8324ea0c1d75c45bd331");

            var VescavorQueenGibberBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("6434f71780431f541bc104ea37a8570b");
            var VescavorQueenGibberImmunityBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("15957616e7b46b34d9a4a92daf3c3ac8");
            var VescavorSwarmGibberBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2b3ade70324be58439c77235fd827d1b");
            var VescavorSwarmGibberImmunityBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("91d3d7a6a409c32418c98859bcd58844");

            var VescavorSwarmApplyGibberImmunityAbility = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(TTTContext, "VescavorSwarmApplyGibberImmunityAbility");
            var VescavorQueenApplyGibberImmunityAbility = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(TTTContext, "VescavorQueenApplyGibberImmunityAbility");

            var Confusion = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("886c7407dc629dc499b9f1465ff382df");
            var DifficultyPreset = VescavorSwarmGibberAreaEffect
                .FlattenAllActions()
                .OfType<Conditional>()
                .First()
                .ConditionsChecker
                .Conditions
                .OfType<ContextConditionDifficultyHigherThan>()
                .First()
                .m_Difficulty;
            VescavorQueenGibberImmunityBuff.TemporaryContext(bp => {
                bp.AddComponent<CombatStateTrigger>(c => {
                    c.CombatStartActions = Helpers.CreateActionList();
                    c.CombatEndActions = Helpers.CreateActionList(
                        new ContextActionRemoveSelf()
                    );
                });
            });
            VescavorSwarmGibberImmunityBuff.TemporaryContext(bp => {
                bp.AddComponent<CombatStateTrigger>(c => {
                    c.CombatStartActions = Helpers.CreateActionList();
                    c.CombatEndActions = Helpers.CreateActionList(
                        new ContextActionRemoveSelf()
                    );
                });
            });

            //VescavorQueenGibberAreaEffect.SetComponents();
            VescavorQueenGibberAreaEffect.Components.ForEach(c => c.Disabled = true);
            VescavorQueenGibberAreaEffect.TemporaryContext(bp => {
                bp.AddComponent<AbilityAreaEffectRunAction>(c => {
                    c.UnitEnter = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Operation = Operation.Or,
                                Conditions = new Condition[] {
                                    new ContextConditionHasBuffFromCaster(){
                                        m_Buff = VescavorQueenGibberImmunityBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionHasBuff(){
                                        m_Buff = VescavorSwarmGibberBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionHasBuff(){
                                        m_Buff = VescavorQueenGibberBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionDifficultyHigherThan(){
                                        m_Difficulty = DifficultyPreset,
                                        CheckOnlyForMonster = true
                                    },
                                }
                            },
                            IfTrue = Helpers.CreateActionList(),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionSavingThrow() {
                                    Type = SavingThrowType.Will,
                                    CustomDC = new ContextValue(),
                                    Actions = Helpers.CreateActionList(
                                        new ContextActionConditionalSaved() {
                                            Succeed = Helpers.CreateActionList(
                                                new ContextActionCastSpell() {
                                                    m_Spell = VescavorQueenApplyGibberImmunityAbility,
                                                    DC = new ContextValue(),
                                                    SpellLevel = new ContextValue(),
                                                }
                                            ),
                                            Failed = Helpers.CreateActionList(
                                                new ContextActionApplyBuff() {
                                                    m_Buff = Confusion,
                                                    DurationValue = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        Rate = DurationRate.Rounds,
                                                        DiceType = DiceType.One,
                                                        DiceCountValue = 0,
                                                        BonusValue = 1
                                                    },
                                                    AsChild = true,
                                                    NotLinkToAreaEffect = true
                                                }
                                            )
                                        }
                                    )
                                }
                            )
                        }
                    );
                    c.UnitExit = Helpers.CreateActionList();
                    c.UnitMove = Helpers.CreateActionList();
                    c.Round = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Operation = Operation.Or,
                                Conditions = new Condition[] {
                                    new ContextConditionHasBuffFromCaster(){
                                        m_Buff = VescavorQueenGibberImmunityBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionHasBuff(){
                                        m_Buff = VescavorSwarmGibberBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionHasBuff(){
                                        m_Buff = VescavorQueenGibberBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionDifficultyHigherThan(){
                                        m_Difficulty = DifficultyPreset,
                                        CheckOnlyForMonster = true
                                    },
                                }
                            },
                            IfTrue = Helpers.CreateActionList(),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionSavingThrow() {
                                    Type = SavingThrowType.Will,
                                    CustomDC = new ContextValue(),
                                    Actions = Helpers.CreateActionList(
                                        new ContextActionConditionalSaved() {
                                            Succeed = Helpers.CreateActionList(
                                                new ContextActionCastSpell() {
                                                    m_Spell = VescavorQueenApplyGibberImmunityAbility,
                                                    DC = new ContextValue(),
                                                    SpellLevel = new ContextValue(),
                                                }
                                            ),
                                            Failed = Helpers.CreateActionList(
                                                new ContextActionApplyBuff() {
                                                    m_Buff = Confusion,
                                                    DurationValue = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        Rate = DurationRate.Rounds,
                                                        DiceType = DiceType.One,
                                                        DiceCountValue = 0,
                                                        BonusValue = 1
                                                    },
                                                    AsChild = true,
                                                    NotLinkToAreaEffect = true
                                                }
                                            )
                                        }
                                    )
                                }
                            )
                        }
                    );
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.Sonic;
                });
                bp.AddComponent<ContextCalculateAbilityParams>(c => {
                    c.m_CustomProperty = new BlueprintUnitPropertyReference();
                    c.StatType = StatType.Constitution;
                    c.CasterLevel = 0;
                    c.SpellLevel = 0;
                });
            });
            //VescavorSwarmGibberAreaEffect.SetComponents();
            VescavorSwarmGibberAreaEffect.Components.ForEach(c => c.Disabled = true);
            VescavorSwarmGibberAreaEffect.TemporaryContext(bp => {
                bp.AddComponent<AbilityAreaEffectRunAction>(c => {
                    c.UnitEnter = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Operation = Operation.Or,
                                Conditions = new Condition[] {
                                    new ContextConditionHasBuffFromCaster(){
                                        m_Buff = VescavorSwarmGibberImmunityBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionHasBuff(){
                                        m_Buff = VescavorSwarmGibberBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionHasBuff(){
                                        m_Buff = VescavorQueenGibberBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionDifficultyHigherThan(){
                                        m_Difficulty = DifficultyPreset,
                                        CheckOnlyForMonster = true
                                    },
                                }
                            },
                            IfTrue = Helpers.CreateActionList(),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionSavingThrow() {
                                    Type = SavingThrowType.Will,
                                    CustomDC = new ContextValue(),
                                    Actions = Helpers.CreateActionList(
                                        new ContextActionConditionalSaved() {
                                            Succeed = Helpers.CreateActionList(
                                                new ContextActionCastSpell() {
                                                    m_Spell = VescavorSwarmApplyGibberImmunityAbility,
                                                    DC = new ContextValue(),
                                                    SpellLevel = new ContextValue(),
                                                }
                                            ),
                                            Failed = Helpers.CreateActionList(
                                                new ContextActionApplyBuff() {
                                                    m_Buff = Confusion,
                                                    DurationValue = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        Rate = DurationRate.Rounds,
                                                        DiceType = DiceType.One,
                                                        DiceCountValue = 0,
                                                        BonusValue = 1
                                                    },
                                                    AsChild = true,
                                                    NotLinkToAreaEffect = true
                                                }
                                            )
                                        }
                                    )
                                }
                            )
                        }
                    );
                    c.UnitExit = Helpers.CreateActionList();
                    c.UnitMove = Helpers.CreateActionList();
                    c.Round = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Operation = Operation.Or,
                                Conditions = new Condition[] {
                                    new ContextConditionHasBuffFromCaster(){
                                        m_Buff = VescavorSwarmGibberImmunityBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionHasBuff(){
                                        m_Buff = VescavorSwarmGibberBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionHasBuff(){
                                        m_Buff = VescavorQueenGibberBuff.ToReference<BlueprintBuffReference>()
                                    },
                                    new ContextConditionDifficultyHigherThan(){
                                        m_Difficulty = DifficultyPreset,
                                        CheckOnlyForMonster = true
                                    },
                                }
                            },
                            IfTrue = Helpers.CreateActionList(),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionSavingThrow() {
                                    Type = SavingThrowType.Will,
                                    CustomDC = new ContextValue(),
                                    Actions = Helpers.CreateActionList(
                                        new ContextActionConditionalSaved() {
                                            Succeed = Helpers.CreateActionList(
                                                new ContextActionCastSpell() {
                                                    m_Spell = VescavorSwarmApplyGibberImmunityAbility,
                                                    DC = new ContextValue(),
                                                    SpellLevel = new ContextValue(),
                                                }
                                            ),
                                            Failed = Helpers.CreateActionList(
                                                new ContextActionApplyBuff() {
                                                    m_Buff = Confusion,
                                                    DurationValue = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        Rate = DurationRate.Rounds,
                                                        DiceType = DiceType.One,
                                                        DiceCountValue = 0,
                                                        BonusValue = 1
                                                    },
                                                    AsChild = true,
                                                    NotLinkToAreaEffect = true
                                                }
                                            )
                                        }
                                    )
                                }
                            )
                        }
                    );
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.Sonic;
                });
                bp.AddComponent<ContextCalculateAbilityParams>(c => {
                    c.m_CustomProperty = new BlueprintUnitPropertyReference();
                    c.StatType = StatType.Constitution;
                    c.CasterLevel = 0;
                    c.SpellLevel = 0;
                });
            });

            TTTContext.Logger.LogPatch(VescavorSwarmGibberAreaEffect);
            TTTContext.Logger.LogPatch(VescavorSwarmGibberImmunityBuff);
            TTTContext.Logger.LogPatch(VescavorQueenGibberAreaEffect);
            TTTContext.Logger.LogPatch(VescavorQueenGibberImmunityBuff);
        }
    }
}
