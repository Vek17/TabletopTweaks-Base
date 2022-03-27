using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats.ShieldMastery {
    static class TopplingBash {
        internal static void AddTopplingBash() {
            var FighterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var ArmorTraining = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
            var ShieldFocus = BlueprintTools.GetBlueprint<BlueprintFeature>("ac57069b6bf8c904086171683992a92a");
            var ShieldBashFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("121811173a614534e8720d7550aae253");
            var ShieldBashBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("5566971fdebf7fe468a497bbee0d3ed5");
            var StumblingBashFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "StumblingBashFeature");

            var TopplingBashPenalty = Helpers.CreateBuff(TTTContext, "TopplingBashPenalty", bp => {
                bp.SetName(TTTContext, "Toppling Bash");
                bp.SetDescription(TTTContext, "");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<ManeuverBonus>(c => {
                    c.Type = CombatManeuver.Trip;
                    c.Bonus = -5;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });
            var TopplingBashEffectBuff = Helpers.CreateBuff(TTTContext, "TopplingBashEffectBuff", bp => {
                bp.SetName(TTTContext, "Toppling Bash");
                bp.SetDescription(TTTContext, "Your shield bash throws your enemies off balance.\n" +
                    "Benefit: As a swift action when you hit a creature with a shield bash, " +
                    "you can attempt a trip combat maneuver against that creature at a –5 penalty. " +
                    "This does not provoke an attack of opportunity.");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.m_Icon = ShieldBashFeature.Icon;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.WeaponLightShield;
                    c.Action = CreateTopplingBashActions();
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.SpikedLightShield;
                    c.Action = CreateTopplingBashActions();
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.WeaponHeavyShield;
                    c.Action = CreateTopplingBashActions();
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.SpikedHeavyShield;
                    c.Action = CreateTopplingBashActions();
                });
                ActionList CreateTopplingBashActions() {
                    return Helpers.CreateActionList(
                        Helpers.Create<Conditional>(condition => {
                            condition.ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                        new ContextConditionCasterHasFact(){
                                            m_Fact = ShieldBashBuff
                                        }
                                }
                            };
                            condition.IfFalse = Helpers.CreateActionList();
                            condition.IfTrue = Helpers.CreateActionList(
                                Helpers.Create<ContextActionApplyBuff>(a => {
                                    a.m_Buff = TopplingBashPenalty.ToReference<BlueprintBuffReference>();
                                    a.ToCaster = true;
                                    a.AsChild = true;
                                    a.IsNotDispelable = true;
                                    a.DurationValue = new ContextDurationValue() {
                                        DiceCountValue = new ContextValue(),
                                        BonusValue = 1
                                    };
                                }),
                                Helpers.Create<ContextActionCombatManeuver>(a => {
                                    a.Type = CombatManeuver.Trip;
                                    a.IgnoreConcealment = true;
                                }),
                                Helpers.Create<ContextActionRemoveBuff>(a => {
                                    a.ToCaster = true;
                                    a.m_Buff = TopplingBashPenalty.ToReference<BlueprintBuffReference>();
                                }),
                                Helpers.Create<ContextActionRemoveSelf>()
                            );
                        }));
                }
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });
            var TopplingBashBuff = Helpers.CreateBuff(TTTContext, "TopplingBashBuff", bp => {
                bp.SetName(TopplingBashEffectBuff.m_DisplayName);
                bp.SetDescription(TopplingBashEffectBuff.m_Description);
                bp.m_Icon = ShieldBashFeature.Icon;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = TopplingBashEffectBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.Permanent = true;
                            a.DurationValue = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            };
                        })
                    );
                    c.Deactivated = Helpers.CreateActionList();
                    c.NewRound = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = TopplingBashEffectBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.Permanent = true;
                            a.DurationValue = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            };
                        })
                    );
                });
            });
            var TopplingBashAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "TopplingBashAbility", bp => {
                bp.m_Icon = TopplingBashEffectBuff.Icon;
                bp.SetName(TopplingBashEffectBuff.m_DisplayName);
                bp.SetDescription(TopplingBashEffectBuff.m_Description);
                bp.m_Buff = TopplingBashBuff.ToReference<BlueprintBuffReference>();
                bp.IsOnByDefault = true;
                bp.DoNotTurnOffOnRest = true;
                bp.DeactivateImmediately = true;
                bp.ActivationType = AbilityActivationType.WithUnitCommand;
                bp.m_ActivateWithUnitCommand = UnitCommand.CommandType.Swift;
                bp.AddComponent<ActivatableAbilityUnitCommand>(c => {
                    c.Type = UnitCommand.CommandType.Swift;
                });
            });
            var TopplingBashEffect = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TopplingBashEffect", bp => {
                bp.SetName(TopplingBashEffectBuff.m_DisplayName);
                bp.SetDescription(TopplingBashEffectBuff.m_Description);
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { TopplingBashAbility.ToReference<BlueprintUnitFactReference>() };
                });
            });
            var TopplingBashFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "TopplingBashFeature", bp => {
                bp.SetName(TopplingBashEffect.m_DisplayName);
                bp.SetDescription(TopplingBashEffect.m_Description);
                bp.m_Icon = TopplingBashEffectBuff.Icon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = TopplingBashEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] {
                        ArmorProficiencyGroup.Buckler,
                        ArmorProficiencyGroup.LightShield,
                        ArmorProficiencyGroup.HeavyShield,
                        ArmorProficiencyGroup.TowerShield,
                    };
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass;
                    c.Level = 8;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.BaseAttackBonus;
                    c.Value = 11;
                    c.Group = GroupType.Any;
                });
                bp.AddPrerequisiteFeature(ShieldBashFeature);
                bp.AddPrerequisiteFeature(StumblingBashFeature);
                bp.AddPrerequisiteFeaturesFromList(1, ArmorTraining, ShieldFocus);
            });

            if (TTTContext.AddedContent.ShieldMasteryFeats.IsDisabled("TopplingBash")) { return; }
            ShieldMastery.AddToShieldMasterySelection(TopplingBashFeature);
        }
    }
}
