using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    internal class MantisStyle {
        public static void AddMantisStyle() {
            var Icon_MantisStyle = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_MantisStyle.png");
            var Icon_MantisTormentAbility = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_MantisTormentAbility.png");
            var ImprovedUnarmedStrike = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167");
            var StunningFistMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("a29a582c3daa4c24bb0e991c596ccb28");
            var StunningFist = BlueprintTools.GetBlueprint<BlueprintFeature>("a29a582c3daa4c24bb0e991c596ccb28");
            var StunningFistResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("d2bae584db4bf4f4f86dd9d15ae56558");
            var StunningFistSickenedFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d256ab3837538cc489d4b571e3a813eb");
            var StunningFistFatigueFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("819645da2e446f84d9b168ed1676ec29");

            var StunningFistAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("732ae7773baf15447a6737ae6547fc1e");
            var StunningFistFatigueAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("32f92fea1ab81c843a436a49f522bfa1");
            var StunningFistSickenedAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c81906c75821cbe4c897fa11bdaeee01");
            var StunningFistStaggeredAbility = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(TTTContext, "StunningFistStaggeredAbility");
            var StunningFistBlindAbility = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(TTTContext, "StunningFistBlindAbility");
            var StunningFistParalyzeAbility = BlueprintTools.GetModBlueprintReference<BlueprintAbilityReference>(TTTContext, "StunningFistParalyzeAbility");

            var StunningFistOwnerBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("d9eaeba5690a7704da8bbf626456a50e");
            var StunningFistOwnerFatigueBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("696b29374599d4141be64e46a91bd09b");
            var StunningFistOwnerSickenedBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("4d7da6df5cb3b3940a9d96311a2dc311");
            var StunningFistStaggeredBuff = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(TTTContext, "StunningFistStaggeredBuff");
            var StunningFistBlindBuff = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(TTTContext, "StunningFistBlindBuff");
            var StunningFistParalyzeBuff = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(TTTContext, "StunningFistParalyzeBuff");

            var DragonFerocityBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("8709a00782de26d4a8524732879000fa");
            var Shaken = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("25ec6cb6ab1845c48a95f9c20b034220");
            var Staggered = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("df3950af5a783bd4d91ab73eb8fa0fd3");
            var DazzledBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("df6d1025da07524429afbae248845ecc");
            var Fatigued = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e6f2fc5d73d88064583cb828801212f4");

            var MantisStyleFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MantisStyleFeature", bp => {
                bp.SetName(TTTContext, "Mantis Style");
                bp.SetDescription(TTTContext, "You have learned to target vital areas with crippling accuracy.\n" +
                    "You gain one additional Stunning Fist attempt per day. While using this style, you gain a +2 bonus to the DC of effects you deliver with your Stunning Fist.");
                bp.m_Icon = Icon_MantisStyle;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat, FeatureGroup.StyleFeat };
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = StunningFistResource;
                    c.Value = 1;
                });
                bp.AddPrerequisiteFeature(ImprovedUnarmedStrike);
                bp.AddPrerequisiteFeature(StunningFist);
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillLoreReligion;
                    c.Value = 3;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.ClassSpecific;
                });
            });
            var MantisWisdomFeature = Helpers.CreateBlueprint<BlueprintProgression>(TTTContext, "MantisWisdomFeature", bp => {
                bp.SetName(TTTContext, "Mantis Wisdom");
                bp.SetDescription(TTTContext, "Your knowledge of vital areas allows you to land debilitating strikes with precision.\n" +
                    "Treat half your levels in classes other than monk as monk levels for determining effects you can apply to a target of your Stunning Fist per the Stunning Fist monk class feature. " +
                    "While using Mantis Style, you gain a +2 bonus on unarmed attack rolls with which you are using Stunning Fist attempts.");
                bp.m_Icon = Icon_MantisStyle;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddClass(ClassTools.Classes.MonkClass);
                bp.AlternateProgressionType = AlternateProgressionType.Div2;
                bp.ForAllOtherClasses = true;
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(4, StunningFistSickenedFeature),
                    Helpers.CreateLevelEntry(8, StunningFistFatigueFeature),
                };
                bp.AddPrerequisiteFeature(ImprovedUnarmedStrike);
                bp.AddPrerequisiteFeature(StunningFist);
                bp.AddPrerequisiteFeature(MantisStyleFeature);
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillLoreReligion;
                    c.Value = 6;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.ClassSpecific;
                });
            });
            var MantisTormentBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "MantisTormentBuff", bp => {
                bp.SetName(TTTContext, "Mantis Torment");
                bp.SetDescription(TTTContext, "You make an unarmed attack that expends two daily attempts of your Stunning Fist. " +
                    "If you hit, your opponent must succeed at a saving throw against your Stunning Fist or become dazzled and staggered with " +
                    "crippling pain until the start of your next turn, and at that point the opponent becomes fatigued.");
                bp.m_Icon = Icon_MantisTormentAbility;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                            applyBuff.m_Buff = Staggered;
                            applyBuff.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            };
                        }),
                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                            applyBuff.m_Buff = DazzledBuff;
                            applyBuff.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            };
                        })
                    );
                    c.NewRound = Helpers.CreateActionList();
                    c.Deactivated = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                            applyBuff.m_Buff = Fatigued;
                            applyBuff.Permanent = true;
                            applyBuff.DurationValue = new ContextDurationValue() {
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            };
                        })
                    );
                });
            });
            var MantisTormentOwnerBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "MantisTormentOwnerBuff", bp => {
                bp.SetName(TTTContext, "Mantis Torment");
                bp.SetDescription(TTTContext, "You make an unarmed attack that expends two daily attempts of your Stunning Fist. " +
                    "If you hit, your opponent must succeed at a saving throw against your Stunning Fist or become dazzled and staggered with " +
                    "crippling pain until the start of your next turn, and at that point the opponent becomes fatigued.");
                bp.m_Icon = Icon_MantisTormentAbility;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.UnarmedStrike;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionSavingThrow>(savingThrow => {
                            savingThrow.Type = SavingThrowType.Fortitude;
                            savingThrow.CustomDC = new ContextValue();
                            savingThrow.Actions = Helpers.CreateActionList(
                                Helpers.Create<ContextActionConditionalSaved>(saved => {
                                    saved.Succeed = Helpers.CreateActionList();
                                    saved.Failed = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                                            applyBuff.m_Buff = MantisTormentBuff.ToReference<BlueprintBuffReference>();
                                            applyBuff.DurationValue = new ContextDurationValue() {
                                                Rate = DurationRate.Rounds,
                                                DiceCountValue = new ContextValue(),
                                                BonusValue = 1
                                            };
                                        }),
                                        Helpers.Create<Conditional>(conditional => {
                                            conditional.ConditionsChecker = new ConditionsChecker() {
                                                Conditions = new Condition[] {
                                                    new ContextConditionCasterHasFact(){
                                                        m_Fact = DragonFerocityBuff
                                                    }
                                                }
                                            };
                                            conditional.IfFalse = Helpers.CreateActionList();
                                            conditional.IfTrue = Helpers.CreateActionList(
                                                Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                                                    applyBuff.m_Buff = Shaken;
                                                    applyBuff.DurationValue = new ContextDurationValue() {
                                                        m_IsExtendable = true,
                                                        Rate = DurationRate.Rounds,
                                                        DiceType = DiceType.D4,
                                                        DiceCountValue = 1,
                                                        BonusValue = new ContextValue() {
                                                            ValueType = ContextValueType.CasterProperty,
                                                            Property = UnitProperty.StatBonusStrength
                                                        }
                                                    };
                                                })
                                            );
                                        })
                                    );
                                })
                            );
                        })
                    );
                });
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = false;
                    c.ActionsOnInitiator = true;
                    c.CheckWeaponCategory = true;
                    c.Category = WeaponCategory.UnarmedStrike;
                    c.Action = Helpers.CreateActionList(
                        Helpers.Create<ContextActionRemoveSelf>()
                    );
                });
                StunningFistAbility.Get().AddComponent<AbilityCasterHasNoFacts>(c => {
                     c.m_Facts = new BlueprintUnitFactReference[] {
                        bp.ToReference<BlueprintUnitFactReference>()
                    };
                 });
                StunningFistFatigueAbility.Get().AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        bp.ToReference<BlueprintUnitFactReference>()
                    };
                });
                StunningFistSickenedAbility.Get().AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        bp.ToReference<BlueprintUnitFactReference>()
                    };
                });
                StunningFistStaggeredAbility.Get().AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        bp.ToReference<BlueprintUnitFactReference>()
                    };
                });
                StunningFistBlindAbility.Get().AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        bp.ToReference<BlueprintUnitFactReference>()
                    };
                });
                StunningFistParalyzeAbility.Get().AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        bp.ToReference<BlueprintUnitFactReference>()
                    };
                });
            });
            var MantisTormentAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "MantisTormentAbility", bp => {
                bp.SetName(TTTContext, "Mantis Torment");
                bp.SetDescription(TTTContext, "You make an unarmed attack that expends two daily attempts of your Stunning Fist. " +
                    "If you hit, your opponent must succeed at a saving throw against your Stunning Fist or become dazzled and staggered with " +
                    "crippling pain until the start of your next turn, and at that point the opponent becomes fatigued.");
                bp.m_Icon = Icon_MantisTormentAbility;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Special;
                bp.Range = AbilityRange.Personal;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.Duration", "");
                bp.LocalizedSavingThrow = Helpers.CreateString(TTTContext, $"{bp.name}.SavingThrow", "");
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(applyBuff => {
                            applyBuff.m_Buff = MantisTormentOwnerBuff.ToReference<BlueprintBuffReference>();
                            applyBuff.Permanent = true;
                            applyBuff.DurationValue = new ContextDurationValue() {
                                m_IsExtendable = true,
                                DiceCountValue = new ContextValue(),
                                BonusValue = new ContextValue()
                            };
                        })
                    );
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = StunningFistResource;
                    c.m_IsSpendResource = true;
                    c.Amount = 2;
                    c.ResourceCostIncreasingFacts = new();
                    c.ResourceCostDecreasingFacts = new();
                });
                bp.AddComponent<AbilityCasterMainWeaponCheck>(c => {
                    c.Category = new WeaponCategory[] { WeaponCategory.UnarmedStrike };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { 
                    };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        StunningFistOwnerBuff
                    };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        StunningFistOwnerFatigueBuff
                    };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        StunningFistOwnerSickenedBuff
                    };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        StunningFistStaggeredBuff
                    };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        StunningFistBlindBuff
                    };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        StunningFistParalyzeBuff
                    };
                });
                bp.AddComponent<AbilityCasterHasNoFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        MantisTormentOwnerBuff.ToReference<BlueprintUnitFactReference>()
                    };
                });
                StunningFistMythicFeat.AddComponent<IncreaseSpellDC>(c => {
                    c.m_Spell = bp.ToReference<BlueprintAbilityReference>();
                    c.HalfMythicRank = true;
                    c.Value = new ContextValue();
                });
            });
            var MantisTormentFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MantisTormentFeature", bp => {
                bp.SetName(TTTContext, "Mantis Torment");
                bp.SetDescription(TTTContext, "Your knowledge of the mysteries of anatomy allows you to cause debilitating pain with a simple touch.\n" +
                    "You gain one additional Stunning Fist attempt per day. " +
                    "While using Mantis Style, you make an unarmed attack that expends two daily attempts of your Stunning Fist. " +
                    "If you hit, your opponent must succeed at a saving throw against your Stunning Fist or become dazzled and staggered with " +
                    "crippling pain until the start of your next turn, and at that point the opponent becomes fatigued.");
                bp.m_Icon = Icon_MantisStyle;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = StunningFistResource;
                    c.Value = 1;
                });
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { MantisTormentAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddPrerequisiteFeature(ImprovedUnarmedStrike);
                bp.AddPrerequisiteFeature(StunningFist);
                bp.AddPrerequisiteFeature(MantisStyleFeature);
                bp.AddPrerequisiteFeature(MantisWisdomFeature);
                bp.AddPrerequisite<PrerequisiteStatValue>(c => {
                    c.Stat = StatType.SkillLoreReligion;
                    c.Value = 9;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Attack | FeatureTag.ClassSpecific;
                });
            });
            var MantisStyleBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "MantisStyleBuff", bp => {
                bp.SetName(TTTContext, "Mantis Style");
                bp.SetDescription(TTTContext, "While using this style, you gain a +2 bonus to the DC of effects you deliver with your Stunning Fist.");
                bp.m_Icon = Icon_MantisStyle;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistFatigueAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistSickenedAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistStaggeredAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistBlindAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = StunningFistParalyzeAbility;
                });
                bp.AddComponent<IncreaseSpellDC>(c => {
                    c.BonusDC = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.m_Spell = MantisTormentAbility.ToReference<BlueprintAbilityReference>();
                });
                MantisTormentAbility.AddComponent<AbilityCasterHasFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { bp.ToReference<BlueprintUnitFactReference>() };
                });
            });
            var MantisWisdomBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "MantisWisdomBuff", bp => {
                bp.SetName(TTTContext, "Mantis Wisdom");
                bp.SetDescription(TTTContext, "");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.m_Icon = Icon_MantisStyle;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                    c.Stat = StatType.AdditionalAttackBonus;
                    c.Value = 2;
                    c.RequireAllFacts = false;
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        StunningFistOwnerBuff,
                        StunningFistOwnerFatigueBuff,
                        StunningFistOwnerSickenedBuff,
                        StunningFistStaggeredBuff,
                        StunningFistBlindBuff,
                        StunningFistParalyzeBuff,
                        MantisTormentOwnerBuff.ToReference<BlueprintUnitFactReference>()
                    };
                });
                MantisWisdomFeature.AddComponent<BuffExtraEffects>(c => {
                    c.m_CheckedBuff = MantisStyleBuff.ToReference<BlueprintBuffReference>();
                    c.m_ExtraEffectBuff = bp.ToReference<BlueprintBuffReference>();
                });
            });
            var MantisStyleToggle = Helpers.CreateBlueprint<BlueprintActivatableAbility>(TTTContext, "MantisStyleToggle", bp => {
                bp.SetName(TTTContext, "Mantis Style");
                bp.SetDescription(TTTContext, "While using this style, you gain a +2 bonus to the DC of effects you deliver with your Stunning Fist.");
                bp.m_Icon = Icon_MantisStyle;
                bp.Group = ActivatableAbilityGroup.CombatStyle;
                bp.m_Buff = MantisStyleBuff.ToReference<BlueprintBuffReference>();
                bp.IsOnByDefault = true;
                bp.DoNotTurnOffOnRest = true;
                bp.DeactivateImmediately = true;
                bp.ActivationType = AbilityActivationType.WithUnitCommand;
                bp.m_ActivateWithUnitCommand = UnitCommand.CommandType.Swift;
            });
            MantisStyleFeature.AddComponent<AddFacts>(c => {
                c.m_Facts = new BlueprintUnitFactReference[] { MantisStyleToggle.ToReference<BlueprintUnitFactReference>()};
            });

            if (TTTContext.AddedContent.Feats.IsDisabled("MantisStyle")) { return; }
            FeatTools.AddAsFeat(MantisStyleFeature);
            if (TTTContext.AddedContent.Feats.IsDisabled("MantisWisdom")) { return; }
            FeatTools.AddAsFeat(MantisWisdomFeature);
            if (TTTContext.AddedContent.Feats.IsDisabled("MantisTorment")) { return; }
            FeatTools.AddAsFeat(MantisTormentFeature);
        }
    }
}