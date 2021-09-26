using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    class MountedManiac {
        public static void AddMountedManiac() {
            var MythicAbilitySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var ExtraMythicAbilityMythicFeat = Resources.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");
            var MountedBuff = Resources.GetBlueprint<BlueprintBuff>("b2d13e8f3bb0f1d4c891d71b4d983cf7");
            var DazzlingDisplayAction = Resources.GetBlueprint<BlueprintAbility>("5f3126d4120b2b244a95cb2ec23d69fb");
            var icon = AssetLoader.LoadInternal("Feats", "Icon_MountedManiac.png");

            var MountedManiacDCBuff = Helpers.CreateBuff("MountedManiacDCBuff", bp => {
                bp.m_Icon = icon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.SetName("Mounted Maniac");
                bp.SetDescription("");
                bp.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                    c.Stat = StatType.CheckIntimidate;
                }));
                bp.AddComponent(Helpers.Create<ContextRankConfig>(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_StartLevel = 1;
                    c.m_StepLevel = 1;
                    c.m_Max = 20;
                    c.m_Min = 1;
                }));
                bp.AddComponent(Helpers.Create<RemoveBuffOnAttack>());
            });
            var MountedManiacAbility = Helpers.CreateCopy(DazzlingDisplayAction, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("MountedManiacAbility");
                bp.name = "MountedManiacAbility";
                bp.m_Icon = icon;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.CanTargetEnemies = true;
                bp.Range = AbilityRange.Close;
                bp.SetName("Mounted Maniac");
                bp.SetDescription("Your unstoppable momentum while mounted is terrifying. Whenever you charge a creature while mounted, you can attempt an " +
                    "Intimidate check to demoralize all enemies within 30 feet of your target, adding your mythic rank to the result of the check.");
                bp.GetComponent<AbilityEffectRunAction>().Actions.Actions.OfType<Demoralize>().First().DazzlingDisplay = false;
                Resources.AddBlueprint(bp);
            });
            var MountedManiacBuff = Helpers.CreateBuff("MountedManiacBuff", bp => {

                bp.m_Icon = icon;
                bp.SetName("Mounted Maniac");
                bp.SetDescription("Your unstoppable momentum while mounted is terrifying. Whenever you charge a creature while mounted, you can attempt an " +
                    "Intimidate check to demoralize all enemies within 30 feet of your target, adding your mythic rank to the result of the check.");
                bp.AddComponent(Helpers.Create<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyOnFirstAttack = true;
                    c.OnCharge = true;
                    c.OnlyHit = false;
                    c.Action = new ActionList() {
                        Actions = new GameAction[] {
                            new Conditional() {
                                ConditionsChecker = new ConditionsChecker {
                                    Conditions = new Condition[] {
                                        new ContextConditionCasterHasFact(){
                                            m_Fact = MountedBuff.ToReference<BlueprintUnitFactReference>()
                                        }
                                    },
                                },
                                IfTrue = new ActionList() {
                                    Actions = new GameAction[] {
                                        new ContextActionApplyBuff(){
                                            ToCaster = true,
                                            m_Buff = MountedManiacDCBuff.ToReference<BlueprintBuffReference>(),
                                            DurationValue = new ContextDurationValue() {
                                                Rate = DurationRate.Rounds,
                                                DiceType = DiceType.One,
                                                DiceCountValue = new ContextValue() {
                                                    ValueType = ContextValueType.Simple,
                                                    Value = 1
                                                },
                                                BonusValue = new ContextValue() {
                                                    ValueType = ContextValueType.Simple,
                                                    Value = 0
                                                }
                                            }
                                        },
                                        new ContextActionCastSpell(){
                                            m_Spell = MountedManiacAbility.ToReference<BlueprintAbilityReference>(),
                                            SpellLevel = new ContextValue(),
                                            DC = new ContextValue(),
                                        }
                                    }
                                },
                                IfFalse = new ActionList()
                            }
                        }
                    };
                }));
            });
            var MountedManiacActivatableAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>("MountedManiacActivatableAbility", bp => {
                bp.m_Icon = icon;
                bp.SetName("Mounted Maniac");
                bp.SetDescription("Your unstoppable momentum while mounted is terrifying. Whenever you charge a creature while mounted, you can attempt an " +
                    "Intimidate check to demoralize all enemies within 30 feet of your target, adding your mythic rank to the result of the check.");
                bp.m_Buff = MountedManiacBuff.ToReference<BlueprintBuffReference>();
                bp.IsOnByDefault = true;
            });
            var MountedManiacFeature = Helpers.CreateBlueprint<BlueprintFeature>("MountedManiacFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName("Mounted Maniac");
                bp.SetDescription("Your unstoppable momentum while mounted is terrifying. Whenever you charge a creature while mounted, you can attempt an " +
                    "Intimidate check to demoralize all enemies within 30 feet of your target, adding your mythic rank to the result of the check.");
                bp.AddComponent(Helpers.Create<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        MountedManiacActivatableAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                }));
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("MountedManiac")) { return; }
            FeatTools.AddAsMythicAbility(MountedManiacFeature);
        }
    }
}
