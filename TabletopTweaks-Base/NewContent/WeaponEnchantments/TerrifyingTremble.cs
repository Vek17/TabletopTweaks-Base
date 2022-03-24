using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.WeaponEnchantments {
    static class TerrifyingTremble {
        public static void AddTerrifyingTrembleEnchant() {
            var TerrifyingTrembleItem = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("8c31891423c4405393741e829aebec85");
            var ThunderingBlowsAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("a9c0ab0293b0c3245881b27ea4e8f95d");

            var TerrifyingTrembleAbility_TTT = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, $"TerrifyingTrembleAbility_TTT", bp => {
                var effects = ThunderingBlowsAbility.GetComponent<AbilitySpawnFx>();
                bp.SetName(TTTContext, "Terrifying Tremble");
                bp.SetDescription(TTTContext, "Whenever the wielder of this weapon lands a killing blow, " +
                    "he deals sonic damage equal to his ranks in the Athletics skill to all enemies within 10 feet. " +
                    "Successful Reflex save (DC 30) halves the damage.");
                bp.m_Icon = TerrifyingTrembleItem.Icon;
                bp.ResourceAssetIds = ThunderingBlowsAbility.ResourceAssetIds;
                bp.LocalizedSavingThrow = Helpers.CreateString(TTTContext, $"{bp.name}.save", $"");
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.duration", $"");
                bp.CanTargetEnemies = true;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Special;
                bp.AddComponent<AbilityTargetsAround>(c => {
                    c.m_Radius = 10.Feet();
                    c.m_TargetType = TargetType.Enemy;
                    c.m_Condition = new ConditionsChecker();
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    new ContextConditionIsEnemy(),
                                    new ContextConditionAlive(),
                                    new ContextConditionIsMainTarget(){
                                        Not = true
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new ContextActionSavingThrow() {
                                    m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
                                    Type = SavingThrowType.Reflex,
                                    HasCustomDC = true,
                                    CustomDC = 30,
                                    Actions = Helpers.CreateActionList(
                                        new ContextActionDealDamage() {
                                            DamageType = new DamageTypeDescription() {
                                                Type = DamageType.Energy,
                                                Common = new DamageTypeDescription.CommomData(),
                                                Physical = new DamageTypeDescription.PhysicalData(),
                                                Energy = DamageEnergyType.Sonic
                                            },
                                            Duration = new ContextDurationValue() {
                                                m_IsExtendable = true,
                                                DiceCountValue = new ContextValue(),
                                                BonusValue = new ContextValue()
                                            },
                                            Value = new ContextDiceValue() {
                                                DiceCountValue = new ContextValue(),
                                                BonusValue = new ContextValue() {
                                                    ValueType = ContextValueType.Rank
                                                }
                                            },
                                            HalfIfSaved = true,
                                            IsAoE = true
                                        }
                                    )
                                }
                            ),
                            IfFalse = Helpers.CreateActionList()
                        }
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.BaseStat;
                    c.m_Stat = StatType.SkillAthletics;
                });
                bp.AddComponent<ContextSetAbilityParams>(c => {
                    c.DC = 30;
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = effects.PrefabLink;
                    c.PositionAnchor = AbilitySpawnFxAnchor.ClickedTarget;
                    c.OrientationAnchor = effects.OrientationAnchor;
                });
            });
            var TerrifyingTrembleEnchant_TTT = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>(TTTContext, $"TerrifyingTrembleEnchant_TTT", bp => {
                bp.SetName(TTTContext, "");
                bp.SetDescription(TTTContext, "");
                bp.SetPrefix(TTTContext, "");
                bp.SetSuffix(TTTContext, "");
                bp.m_EnchantmentCost = 1;
                bp.m_IdentifyDC = 5;
                bp.AddComponent<AddWeaponDamageTrigger>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionCastSpell() {
                            m_Spell = TerrifyingTrembleAbility_TTT.ToReference<BlueprintAbilityReference>(),
                            DC = new ContextValue(),
                            SpellLevel = new ContextValue()
                        }
                    );
                    c.TargetKilledByThisDamage = true;
                });
            });
        }
    }
}
