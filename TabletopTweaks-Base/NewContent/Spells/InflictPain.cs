using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Spells {
    internal class InflictPain {
        public static void AddInflictPain() {
            var Icon_InflictPain = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_InflictPain.png");
            var Icon_InflictPainMass = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_InflictPainMass.png");
            var Icon_ScrollOfInflictPain = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfInflictPain.png");
            var Icon_ScrollOfInflictPainMass = AssetLoader.LoadInternal(TTTContext, folder: "Equipment", file: "Icon_ScrollOfInflictPainMass.png");
            var HarmEffect = new PrefabLink() {
                AssetId = "2f9b7cd8912ad7d4484a5055eece0d47"
            };

            var InflictPainBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "InflictPainBuff", bp => {
                bp.SetName(TTTContext, "Inflict Pain");
                bp.SetDescription(TTTContext, "You telepathically wrack the target’s mind and body with agonizing pain that imposes a –4 penalty on attack rolls, skill checks, and ability checks. A successful Will save reduces the duration to 1 round.");
                bp.m_Icon = Icon_InflictPain;
                bp.m_Flags = BlueprintBuff.Flags.IsFromSpell;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.AdditionalAttackBonus;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                });
                bp.AddComponent<BuffAllSkillsBonus>(c => {
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -4;
                    c.Multiplier = 1;
                });
            });
            var InflictPainAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "InflictPainAbility", bp => {
                bp.SetName(TTTContext, "Inflict Pain");
                bp.SetDescription(TTTContext, "You telepathically wrack the target’s mind and body with agonizing pain that imposes a –4 penalty on attack rolls, skill checks, and ability checks. A successful Will save reduces the duration to 1 round.");
                bp.SetLocalizedDuration(TTTContext, "1 round/level");
                bp.SetLocalizedSavingThrow(TTTContext, "Will partial");
                bp.AvailableMetamagic = Metamagic.Extend
                    | Metamagic.Heighten
                    | Metamagic.Quicken
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent
                    | Metamagic.Reach;
                bp.Range = AbilityRange.Close;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.CanTargetEnemies = true;
                bp.SpellResistance = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_InflictPain;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Will;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionConditionalSaved() {
                            Succeed = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    m_Buff = InflictPainBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Rounds,
                                        BonusValue = 1,
                                        DiceCountValue = 0,
                                        m_IsExtendable = false
                                    }
                                }
                            ),
                            Failed = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    m_Buff = InflictPainBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Rounds,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank
                                        },
                                        DiceCountValue = 0
                                    }
                                }
                            )
                        }
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = HarmEffect;
                    c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Enchantment;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.MindAffecting;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Debuff;
                    c.SavingThrow = CraftSavingThrow.Will;
                    c.AOEType = CraftAOE.None;
                });
            });
            var InflictPainMassAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "InflictPainMassAbility", bp => {
                bp.SetName(TTTContext, "Inflict Pain, Mass");
                bp.SetDescription(TTTContext, "You telepathically wrack the target’s mind and body with agonizing pain that imposes a –4 penalty on attack rolls, skill checks, and ability checks. A successful Will save reduces the duration to 1 round.");
                bp.SetLocalizedDuration(TTTContext, "1 round/level");
                bp.SetLocalizedSavingThrow(TTTContext, "Will partial");
                bp.AvailableMetamagic = Metamagic.Extend
                    | Metamagic.Heighten
                    | Metamagic.Quicken
                    | Metamagic.CompletelyNormal
                    | Metamagic.Persistent
                    | Metamagic.Reach
                    | Metamagic.Selective;
                bp.Range = AbilityRange.Close;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetPoint = true;
                bp.SpellResistance = true;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.m_Icon = Icon_InflictPainMass;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Will;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionConditionalSaved() {
                            Succeed = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    m_Buff = InflictPainBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Rounds,
                                        BonusValue = 1,
                                        DiceCountValue = 0,
                                        m_IsExtendable = false
                                    }
                                }
                            ),
                            Failed = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    IsFromSpell = true,
                                    m_Buff = InflictPainBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        Rate = DurationRate.Rounds,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank
                                        },
                                        DiceCountValue = 0
                                    }
                                }
                            )
                        }
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent<AbilityTargetsAround>(c => {
                    c.m_Radius = 30.Feet();
                    c.m_TargetType = TargetType.Any;
                    c.m_Condition = new ConditionsChecker() {
                        Conditions = new Condition[0]
                    };
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = HarmEffect;
                    c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
                });
                bp.AddComponent<SpellComponent>(c => {
                    c.School = SpellSchool.Enchantment;
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.MindAffecting;
                });
                bp.AddComponent<CraftInfoComponent>(c => {
                    c.OwnerBlueprint = bp;
                    c.SpellType = CraftSpellType.Debuff;
                    c.SavingThrow = CraftSavingThrow.Will;
                    c.AOEType = CraftAOE.AOE;
                });
            });
            var ScrollOfInflictPain = ItemTools.CreateScroll(TTTContext, "ScrollOfInflictPain", Icon_ScrollOfInflictPain, InflictPainAbility, 3, 5);
            var ScrollOfInflictPainMass = ItemTools.CreateScroll(TTTContext, "ScrollOfInflictPainMass", Icon_ScrollOfInflictPainMass, InflictPainMassAbility, 7, 13);

            if (TTTContext.AddedContent.Spells.IsDisabled("InflictPain")) { return; }

            VenderTools.AddScrollToLeveledVenders(ScrollOfInflictPain);
            VenderTools.AddScrollToLeveledVenders(ScrollOfInflictPainMass);

            InflictPainAbility.AddToSpellList(SpellTools.SpellList.InquisitorSpellList, 2);
            InflictPainAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 3);
            InflictPainAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 3);

            InflictPainMassAbility.AddToSpellList(SpellTools.SpellList.InquisitorSpellList, 5);
            InflictPainMassAbility.AddToSpellList(SpellTools.SpellList.WizardSpellList, 7);
            InflictPainMassAbility.AddToSpellList(SpellTools.SpellList.WitchSpellList, 7);
        }
    }
}
