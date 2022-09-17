using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;
using StatValueGetter = Kingmaker.UnitLogic.Mechanics.Properties.StatValueGetter;

namespace TabletopTweaks.Base.NewContent.RogueTalents {
    internal static class BleedingAttack {
        public static void AddBleedingAttack() {
            var Icon_BleedingAttack = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_BleedingAttack.png");
            var BleedingAttackProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(TTTContext, "EmboldeningStrikeProperty", bp => {
                bp.AddComponent<StatValueGetter>(c => {
                    c.Stat = StatType.SneakAttack;
                });
            });
            var BleedingAttackBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BleedingAttackBuff", bp => {
                bp.SetName(TTTContext, "Bleeding Attack");
                bp.SetDescription(TTTContext, "A rogue with this ability can cause living opponents to bleed by hitting them with a sneak attack. " +
                    "This attack causes the target to take 1 additional point of damage each round for each die of the rogue’s sneak attack (e.g., 4d6 equals 4 points of bleed). " +
                    "Bleeding creatures take that amount of damage every round at the start of each of their turns. The bleeding can be stopped by the application of any effect that heals hit point damage. " +
                    "Bleed damage from this ability does not stack with itself. Bleed damage bypasses any damage reduction the creature might possess.");
                bp.m_Icon = Icon_BleedingAttack;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddHealTrigger>(c => {
                    c.Action = Helpers.CreateActionList(
                        new ContextActionRemoveSelf()
                    );
                    c.HealerAction = Helpers.CreateActionList();
                    c.OnHealDamage = true;
                    c.AllowZeroHealDamage = true;
                });
                bp.AddComponent<AddFactContextActions>(c => { 
                    c.Activated = Helpers.CreateActionList();
                    c.Deactivated = Helpers.CreateActionList();
                    c.NewRound = Helpers.CreateActionList(
                        new ContextActionDealDamage() { 
                            DamageType = new DamageTypeDescription() { 
                                Type = DamageType.Direct
                            },
                            Duration = new ContextDurationValue() { 
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            },
                            Value = new ContextDiceValue() { 
                                DiceCountValue = 0,
                                BonusValue = new ContextValue() { 
                                    ValueType = ContextValueType.Rank
                                }
                            }
                        }    
                    );
                });
                bp.AddComponent<SpellDescriptorComponent>(c => {
                    c.Descriptor = SpellDescriptor.Bleed;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_CustomProperty = RogueTalentProperties.SneakAttackDiceProperty.ToReference<BlueprintUnitPropertyReference>();
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMin = true;
                    c.m_Min = 1;
                });
            });
            var BleedingAttackFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BleedingAttackFeature", bp => {
                bp.SetName(TTTContext, "Bleeding Attack");
                bp.SetDescription(TTTContext, "A rogue with this ability can cause living opponents to bleed by hitting them with a sneak attack. " +
                    "This attack causes the target to take 1 additional point of damage each round for each die of the rogue’s sneak attack (e.g., 4d6 equals 4 points of bleed). " +
                    "Bleeding creatures take that amount of damage every round at the start of each of their turns. The bleeding can be stopped by the application of any effect that heals hit point damage. " +
                    "Bleed damage from this ability does not stack with itself. Bleed damage bypasses any damage reduction the creature might possess.");
                bp.m_Icon = Icon_BleedingAttack;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.OnlySneakAttack = true;
                    c.Action = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = BleedingAttackBuff.ToReference<BlueprintBuffReference>(),
                            Permanent = true,
                            DurationValue = new ContextDurationValue() {
                                m_IsExtendable = false,
                                Rate = DurationRate.Rounds,
                                DiceCountValue = 0,
                                BonusValue = 1
                            }
                        }
                    );
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_CustomProperty = RogueTalentProperties.RougeLevelProperty.ToReference<BlueprintUnitPropertyReference>();
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMin = true;
                    c.m_Min = 1;
                });
            });

            if (TTTContext.AddedContent.RogueTalents.IsDisabled("BleedingAttack")) { return; }
            FeatTools.AddAsRogueTalent(BleedingAttackFeature);
        }
    }
}
