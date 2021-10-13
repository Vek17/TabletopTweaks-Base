using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;

namespace TabletopTweaks.Utilities {
    public static class BloodlineTools {

        public static void AddActionIfTrue(this Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional conditional, GameAction game_action) {
            if (conditional.IfTrue == null) {
                conditional.IfTrue = new ActionList();
            }
            conditional.IfTrue.Actions = conditional.IfTrue.Actions.AppendToArray(game_action);
        }
        public static void AddActionIfFalse(this Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional conditional, GameAction game_action) {
            if (conditional.IfFalse == null) {
                conditional.IfFalse = new ActionList();
            }
            conditional.IfFalse.Actions = conditional.IfTrue.Actions.AppendToArray(game_action);
        }
        public static void AddActionActivated(this AddFactContextActions component, GameAction game_action) {
            if (component.Activated == null) {
                component.Activated = new ActionList();
            }
            component.Activated.Actions = component.Activated.Actions.AppendToArray(game_action);
        }
        public static void AddActionDeactivated(this AddFactContextActions component, GameAction game_action) {
            if (component.Deactivated == null) {
                component.Deactivated = new ActionList();
            }
            component.Deactivated.Actions = component.Deactivated.Actions.AppendToArray(game_action);
        }
        public static void AddConditionalBuff(this BlueprintBuff parent, BlueprintFeature hasFeature, BlueprintBuff buff) {
            var AddfactContext = parent.GetComponent<AddFactContextActions>();
            if (!AddfactContext) {
                parent.AddComponent(new AddFactContextActions());
                AddfactContext = parent.GetComponent<AddFactContextActions>();
                AddfactContext.NewRound = new ActionList();
            }
            var actionActivated = new Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional() {
                name = parent.name,
                Comment = buff.name,
                ConditionsChecker = new ConditionsChecker {
                    Conditions = new Condition[] { new ContextConditionHasFact() {
                            m_Fact = hasFeature.ToReference<BlueprintUnitFactReference>()
                        }
                    }
                },
                IfFalse = new ActionList()
            };
            actionActivated.AddActionIfTrue(new ContextActionApplyBuff() {
                m_Buff = buff.ToReference<BlueprintBuffReference>(),
                AsChild = true,
                Permanent = true
            });
            AddfactContext.AddActionActivated(actionActivated);

            var actionDeactivated = new Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional() {
                name = parent.name,
                Comment = buff.name,
                ConditionsChecker = new ConditionsChecker {
                    Conditions = new Condition[] { new ContextConditionHasFact() {
                            m_Fact = hasFeature.ToReference<BlueprintUnitFactReference>()
                        }
                    }
                },
                IfFalse = new ActionList()
            };
            actionDeactivated.AddActionIfTrue(new ContextActionRemoveBuff() {
                m_Buff = buff.ToReference<BlueprintBuffReference>()
            });
            AddfactContext.AddActionDeactivated(actionDeactivated);
        }
        public static void RemoveBuffAfterRage(this BlueprintBuff parent, BlueprintBuff buff) {
            var AddfactContext = parent.GetComponent<AddFactContextActions>();
            if (!AddfactContext) {
                parent.AddComponent(new AddFactContextActions());
                AddfactContext = parent.GetComponent<AddFactContextActions>();
            }
            var actionDeactivated = new Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional() {
                name = parent.name,
                Comment = buff.name,
                ConditionsChecker = new ConditionsChecker {
                    Conditions = new Condition[] { new ContextConditionHasFact() {
                            m_Fact = buff.ToReference<BlueprintUnitFactReference>()
                        }
                    }
                },
                IfFalse = new ActionList()
            };
            actionDeactivated.AddActionIfTrue(new ContextActionRemoveBuff() {
                m_Buff = buff.ToReference<BlueprintBuffReference>()
            });
            AddfactContext.AddActionDeactivated(actionDeactivated);
        }
        public static void ApplyPrimalistException(BlueprintFeature power, int level, BlueprintProgression bloodline) {
            BlueprintFeature PrimalistProgression = Resources.GetBlueprint<BlueprintFeature>("d8b8d1dd83393484cbacf6c8830080ae");
            BlueprintFeature PrimalistTakePower4 = Resources.GetBlueprint<BlueprintFeature>("2140040bf367e8b4a9c6a632820becbe");
            BlueprintFeature PrimalistTakePower8 = Resources.GetBlueprint<BlueprintFeature>("c5aaccc685a37ed4b97869398cdd3ebb");
            BlueprintFeature PrimalistTakePower12 = Resources.GetBlueprint<BlueprintFeature>("57bb4dc36611c7444817c13135ec58b4");
            BlueprintFeature PrimalistTakePower16 = Resources.GetBlueprint<BlueprintFeature>("a56a288b9b6097f4eb67be43404321f2");
            BlueprintFeature PrimalistTakePower20 = Resources.GetBlueprint<BlueprintFeature>("b264a03d036248544acfddbcad709345");
            SelectedPrimalistLevel().AddComponent(
                new AddFeatureIfHasFact() {
                    m_Feature = power.ToReference<BlueprintUnitFactReference>(),
                    m_CheckedFact = bloodline.ToReference<BlueprintUnitFactReference>()
                }
            );
            power.AddComponent(new PrerequisiteNoFeature() {
                CheckInProgression = true,
                Group = Prerequisite.GroupType.Any,
                m_Feature = PrimalistProgression.ToReference<BlueprintFeatureReference>()
            });
            power.AddComponent(new PrerequisiteFeature() {
                CheckInProgression = true,
                Group = Prerequisite.GroupType.Any,
                m_Feature = power.ToReference<BlueprintFeatureReference>()
            });
            BlueprintFeature SelectedPrimalistLevel() {
                switch (level) {
                    case 4: return PrimalistTakePower4;
                    case 8: return PrimalistTakePower8;
                    case 12: return PrimalistTakePower12;
                    case 16: return PrimalistTakePower16;
                    case 20: return PrimalistTakePower20;
                    default: return null;
                }
            }
        }
        public static void ApplyBloodrageRestriction(this BlueprintBuff bloodrage, BlueprintAbility ability) {
            ability.AddComponent(new RestrictHasBuff() {
                RequiredBuff = bloodrage.ToReference<BlueprintBuffReference>()
            });
        }
        public static void RegisterBloodragerBloodline(BlueprintProgression bloodline) {
            BlueprintFeatureSelection BloodragerBloodlineSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("62b33ac8ceb18dd47ad4c8f06849bc01");
            BlueprintFeatureSelection SecondBloodragerBloodline = Resources.GetBlueprint<BlueprintFeatureSelection>("b7f62628915bdb14d8888c25da3fac56");
            BlueprintAbility MixedBloodlineAbility = Resources.GetBlueprint<BlueprintAbility>("352b4e8bb5ca4301b6e6084304a86546");
            BlueprintAbility MixedBloodlineAbility2 = Resources.GetBlueprint<BlueprintAbility>("291fa8cf38fa401397dd3c9b7515b153");

            SecondBloodragerBloodline.m_Features = BloodragerBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());
            SecondBloodragerBloodline.m_AllFeatures = BloodragerBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());
            BloodragerBloodlineSelection.m_AllFeatures = BloodragerBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());

            AddFactToApply(MixedBloodlineAbility, bloodline);
            AddFactToApply(MixedBloodlineAbility2, bloodline);

            void AddFactToApply(BlueprintAbility ability, BlueprintUnitFact fact) {
                var component = ability.GetComponent<AbilityApplyFact>();
                component.m_Facts = component.m_Facts.AppendToArray(fact.ToReference<BlueprintUnitFactReference>());
            }
        }
        public static void RegisterSorcererBloodline(BlueprintProgression bloodline) {
            BlueprintFeatureSelection SorcererBloodlineSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("24bef8d1bee12274686f6da6ccbc8914");
            BlueprintFeatureSelection SecondBloodline = Resources.GetBlueprint<BlueprintFeatureSelection>("3cf2ab2c320b73347a7c21cf0d0995bd");
            BlueprintFeatureSelection BloodlineAscendance = Resources.GetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19");

            SorcererBloodlineSelection.m_AllFeatures = SorcererBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());
            SecondBloodline.m_AllFeatures = SecondBloodline.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());

            var capstone = bloodline.LevelEntries.Where(entry => entry.Level == 20).First().Features[0];
            capstone.AddComponent(new PrerequisiteFeature() {
                m_Feature = bloodline.ToReference<BlueprintFeatureReference>(),
                Group = Prerequisite.GroupType.Any
            });
            BloodlineAscendance.m_Features = BloodlineAscendance.m_AllFeatures.AppendToArray(capstone.ToReference<BlueprintFeatureReference>());
            BloodlineAscendance.m_AllFeatures = BloodlineAscendance.m_AllFeatures.AppendToArray(capstone.ToReference<BlueprintFeatureReference>());
        }
        public static void RegisterSorcererFeatSelection(BlueprintFeatureSelection selection, BlueprintProgression bloodline) {
            BlueprintFeatureSelection SorcererFeatSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("3a60f0c0442acfb419b0c03b584e1394");
            selection.AddComponent(new PrerequisiteFeature() {
                m_Feature = bloodline.ToReference<BlueprintFeatureReference>(),
                Group = Prerequisite.GroupType.All
            });
            SorcererFeatSelection.m_Features = SorcererFeatSelection.m_AllFeatures.AppendToArray(selection.ToReference<BlueprintFeatureReference>());
            SorcererFeatSelection.m_AllFeatures = SorcererFeatSelection.m_AllFeatures.AppendToArray(selection.ToReference<BlueprintFeatureReference>());
        }
        public static void RegisterCrossbloodedBloodline(BlueprintProgression bloodline) {
            BlueprintFeatureSelection CrossbloodedSecondaryBloodlineSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("60c99d78a70e0b44f87ba01d02d909a6");
            CrossbloodedSecondaryBloodlineSelection.m_Features = CrossbloodedSecondaryBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());
            CrossbloodedSecondaryBloodlineSelection.m_AllFeatures = CrossbloodedSecondaryBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());
        }
        public static void RegisterSeekerBloodline(BlueprintProgression bloodline) {
            BlueprintFeatureSelection SeekerBloodlineSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("7bda7cdb0ccda664c9eb8978cf512dbc");
            SeekerBloodlineSelection.m_Features = SeekerBloodlineSelection.m_Features.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());
            SeekerBloodlineSelection.m_AllFeatures = SeekerBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());

            var capstone = bloodline.LevelEntries.Where(entry => entry.Level == 20).First().Features[0];
            capstone.AddComponent(new PrerequisiteFeature() {
                m_Feature = bloodline.ToReference<BlueprintFeatureReference>(),
                Group = Prerequisite.GroupType.Any
            });
        }

        public static BlueprintBuff CreateArcaneBloodrageSwitchBuff(
                string blueprintName,
                string displayName,
                BlueprintAbility bloodragerArcaneSpellAbility,
                BlueprintBuff rageBuff,
                BlueprintBuff spellBuff
                ) {

            return Helpers.CreateBuff(blueprintName, bp => {
                bp.m_Flags = BlueprintBuff.Flags.StayOnDeath | BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName(displayName);
                bp.m_Description = bloodragerArcaneSpellAbility.m_Description;
                bp.m_DescriptionShort = bloodragerArcaneSpellAbility.m_DescriptionShort;
                bp.m_Icon = spellBuff.m_Icon;
                bp.AddComponent<BuffExtraEffects>(c => {
                    c.m_CheckedBuff = rageBuff.ToReference<BlueprintBuffReference>();
                    c.m_ExtraEffectBuff = spellBuff.ToReference<BlueprintBuffReference>();
                });
            });
        }

        public static BlueprintAbility CreateArcaneBloodrageToggle(
            string blueprintName,
            BlueprintAbility abilityToImitate,
            BlueprintAbility bloodragerArcaneSpellAbility,
            BlueprintBuff switchBuff,
            List<BlueprintBuff> allToggleBuffsInGroup
            ) {
            var BloodragerStandartRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
            var BloodragerClass = Resources.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
            return Helpers.CreateBlueprint<BlueprintAbility>(blueprintName, bp => {
                bp.m_DisplayName = abilityToImitate.m_DisplayName;
                bp.m_Description = abilityToImitate.m_Description;
                bp.m_DescriptionShort = abilityToImitate.m_DescriptionShort;
                bp.LocalizedDuration = Helpers.CreateString($"{blueprintName}.LocalizedDuration", "While Raging");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.m_Icon = abilityToImitate.m_Icon;
                bp.m_Parent = bloodragerArcaneSpellAbility.ToReference<BlueprintAbilityReference>();
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Special;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList() {
                        Actions = new GameAction[] {
                                    new Conditional() {
                                        name = bloodragerArcaneSpellAbility.name,
                                        Comment = abilityToImitate.name,
                                        ConditionsChecker = new ConditionsChecker() {
                                            Conditions = new Condition[] {
                                                new ContextConditionHasFact() {
                                                    m_Fact = switchBuff.ToReference<BlueprintUnitFactReference>()
                                                }
                                            }
                                        },
                                        IfTrue = new ActionList() {
                                            Actions = new GameAction[] {
                                                new ContextActionRemoveBuff() {
                                                    m_Buff = switchBuff.ToReference<BlueprintBuffReference>()
                                                }
                                            }
                                        },
                                        IfFalse = new ActionList() {
                                            Actions = new GameAction[] {
                                                new ContextActionApplyBuff() {
                                                    m_Buff = switchBuff.ToReference<BlueprintBuffReference>(),
                                                    Permanent = true,
                                                    IsNotDispelable = true,
                                                    DurationValue = new Kingmaker.UnitLogic.Mechanics.ContextDurationValue(),
                                                    AsChild = true
                                                }
                                            }.AppendToArray(
                                                allToggleBuffsInGroup
                                                    .Where(b => b != switchBuff)
                                                    .Select(buff =>
                                                        new ContextActionRemoveBuff() {
                                                            m_Buff = buff.ToReference<BlueprintBuffReference>()
                                                        })
                                                    ).ToArray()
                                        }
                                    }
                                }
                    };
                });
                bp.AddComponent<PseudoActivatable>(c => {
                    c.m_BuffToWatch = switchBuff.ToReference<BlueprintBuffReference>();
                });
                bp.AddComponent<RestrictHasBuff>(c => {
                    c.Inverted = true;
                    c.RequiredBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                });
                bp.AddComponent<ContextSetAbilityParams>(c => {
                    c.DC = new ContextValue();
                    c.Concentration = new ContextValue();
                    c.SpellLevel = new ContextValue();
                    c.CasterLevel = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = Kingmaker.Enums.AbilityRankType.StatBonus
                    };
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = Kingmaker.Enums.AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { BloodragerClass.ToReference<BlueprintCharacterClassReference>() };
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                });
            });
        }

        public static BlueprintBuff CreateBloodragerTrueArcaneSpellRagePolymorphActivationBuff(
                string blueprintName,
                string displayName,
                BlueprintBuff polymorphBuff) {
            return Helpers.CreateBuff(blueprintName, bp => {
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.SetName(displayName);
                bp.m_Description = polymorphBuff.m_Description;
                bp.AddComponent<AddFactContextActions>(c => {
                    c.Activated = new ActionList() {
                        Actions = new GameAction[] {
                                new ContextActionRemoveBuffsByDescriptor() {
                                    NotSelf = true,
                                    SpellDescriptor = SpellDescriptor.Polymorph
                                },
                                new ContextActionApplyBuff() {
                                    m_Buff = polymorphBuff.ToReference<BlueprintBuffReference>(),
                                    Permanent = true,
                                    AsChild = true,
                                    DurationValue = new ContextDurationValue(),
                                    IsFromSpell = false
                                }
                            }
                    };
                });
            });
        }

        public static class Bloodline {
            public static BlueprintProgression BloodragerAberrantBloodline => Resources.GetModBlueprint<BlueprintProgression>("BloodragerAberrantBloodline");
            public static BlueprintProgression BloodragerAbyssalBloodline => Resources.GetBlueprint<BlueprintProgression>("55d5bbf4b5ae1744ab26c71be98067f9");
            public static BlueprintProgression BloodragerArcaneBloodline => Resources.GetBlueprint<BlueprintProgression>("aeff0a749e20ffe4b9e2846eae29c386");
            public static BlueprintProgression BloodragerCelestialBloodline => Resources.GetBlueprint<BlueprintProgression>("05a141717bbce594a8a763c227f4ee2f");
            public static BlueprintProgression BloodragerDestinedBloodline => Resources.GetModBlueprint<BlueprintProgression>("BloodragerDestinedBloodline");
            public static BlueprintProgression BloodragerDragonBlackBloodline => Resources.GetBlueprint<BlueprintProgression>("3d030a2fed2b5cf45919fc1e40629a9e");
            public static BlueprintProgression BloodragerDragonBlueBloodline => Resources.GetBlueprint<BlueprintProgression>("17bbb6790ca500d4190b978cab5c4dfc");
            public static BlueprintProgression BloodragerDragonBrassBloodline => Resources.GetBlueprint<BlueprintProgression>("56e22cb1dde3f5a4297d45744ca19043");
            public static BlueprintProgression BloodragerDragonBronzeBloodline => Resources.GetBlueprint<BlueprintProgression>("0cf41c61c8ac463478e5ba733fd26b40");
            public static BlueprintProgression BloodragerDragonCopperBloodline => Resources.GetBlueprint<BlueprintProgression>("53f189dce67466c4f9e60610e5d1c4ba");
            public static BlueprintProgression BloodragerDragonGoldBloodline => Resources.GetBlueprint<BlueprintProgression>("ffcb3d0a1a45d8048a691eda9f0219b9");
            public static BlueprintProgression BloodragerDragonGreenBloodline => Resources.GetBlueprint<BlueprintProgression>("69b27eb6bd71ac747a2fac8399c27c3a");
            public static BlueprintProgression BloodragerDragonRedBloodline => Resources.GetBlueprint<BlueprintProgression>("34209f220733fc444a039df1b1076b0b");
            public static BlueprintProgression BloodragerDragonSilverBloodline => Resources.GetBlueprint<BlueprintProgression>("df182ceef2330d74b9bd7bfdb23d144b");
            public static BlueprintProgression BloodragerDragonWhiteBloodline => Resources.GetBlueprint<BlueprintProgression>("c97cf6524b89d474989378d841c7cf5c");
            public static BlueprintProgression BloodragerElementalAcidBloodline => Resources.GetBlueprint<BlueprintProgression>("dafe58c4f0785e94e93c0f07901f1343");
            public static BlueprintProgression BloodragerElementalColdBloodline => Resources.GetBlueprint<BlueprintProgression>("5286db4f19f31eb44af99fb881c99517");
            public static BlueprintProgression BloodragerElementalElectricityBloodline => Resources.GetBlueprint<BlueprintProgression>("777bc77d3dc652c488a20d1a7b0b95e5");
            public static BlueprintProgression BloodragerElementalFireBloodline => Resources.GetBlueprint<BlueprintProgression>("12f7b4c5d603f3744b2b1def28c0a4fa");
            public static BlueprintProgression BloodragerFeyBloodline => Resources.GetBlueprint<BlueprintProgression>("a6e8fae8a6d6e374a9af2893840be4ac");
            public static BlueprintProgression BloodragerInfernalBloodline => Resources.GetBlueprint<BlueprintProgression>("9aef64b53406f114cb43f898a3aec01e");
            public static BlueprintProgression BloodragerSerpentineBloodline => Resources.GetBlueprint<BlueprintProgression>("f5b06b67f04949f4c8d88fd3bbc0771e");
            public static BlueprintProgression BloodragerUndeadBloodline => Resources.GetBlueprint<BlueprintProgression>("9f4ea90e9b9c27c48b541dbef184b3b7");
            // Sorceror Bloodlines
            public static BlueprintProgression BloodlineAbyssalProgression => Resources.GetBlueprint<BlueprintProgression>("d3a4cb7be97a6694290f0dcfbd147113");
            public static BlueprintProgression BloodlineArcaneProgression => Resources.GetBlueprint<BlueprintProgression>("4d491cf9631f7e9429444f4aed629791");
            public static BlueprintProgression BloodlineCelestialProgression => Resources.GetBlueprint<BlueprintProgression>("aa79c65fa0e11464d9d100b038c50796");
            public static BlueprintProgression BloodlineDraconicBlackProgression => Resources.GetBlueprint<BlueprintProgression>("7bd143ead2d6c3a409aad6ee22effe34");
            public static BlueprintProgression BloodlineDraconicBlueProgression => Resources.GetBlueprint<BlueprintProgression>("8a7f100c02d0b254d8f5f3affc8ef386");
            public static BlueprintProgression BloodlineDraconicBrassProgression => Resources.GetBlueprint<BlueprintProgression>("5f9ecbee67db8364985e9d0500eb25f1");
            public static BlueprintProgression BloodlineDraconicBronzeProgression => Resources.GetBlueprint<BlueprintProgression>("7e0f57d8d00464441974e303b84238ac");
            public static BlueprintProgression BloodlineDraconicCopperProgression => Resources.GetBlueprint<BlueprintProgression>("b522759a265897b4f8f7a1a180a692e4");
            public static BlueprintProgression BloodlineDraconicGoldProgression => Resources.GetBlueprint<BlueprintProgression>("6c67ef823db8d7d45bb0ef82f959743d");
            public static BlueprintProgression BloodlineDraconicGreenProgression => Resources.GetBlueprint<BlueprintProgression>("7181be57d1cc3bc40bc4b552e4e4ce24");
            public static BlueprintProgression BloodlineDraconicRedProgression => Resources.GetBlueprint<BlueprintProgression>("8c6e5b3cf12f71e43949f52c41ae70a8");
            public static BlueprintProgression BloodlineDraconicSilverProgression => Resources.GetBlueprint<BlueprintProgression>("c7d2f393e6574874bb3fc728a69cc73a");
            public static BlueprintProgression BloodlineDraconicWhiteProgression => Resources.GetBlueprint<BlueprintProgression>("b0f79497a0d1f4f4b8293e82c8f8fa0c");
            public static BlueprintProgression BloodlineElementalAirProgression => Resources.GetBlueprint<BlueprintProgression>("cd788df497c6f10439c7025e87864ee4");
            public static BlueprintProgression BloodlineElementalEarthProgression => Resources.GetBlueprint<BlueprintProgression>("32393034410fb2f4d9c8beaa5c8c8ab7");
            public static BlueprintProgression BloodlineElementalFireProgression => Resources.GetBlueprint<BlueprintProgression>("17cc794d47408bc4986c55265475c06f");
            public static BlueprintProgression BloodlineElementalWaterProgression => Resources.GetBlueprint<BlueprintProgression>("7c692e90592257a4e901d12ae6ec1e41");
            public static BlueprintProgression BloodlineFeyProgression => Resources.GetBlueprint<BlueprintProgression>("e8445256abbdc45488c2d90373f7dae8");
            public static BlueprintProgression BloodlineInfernalProgression => Resources.GetBlueprint<BlueprintProgression>("e76a774cacfb092498177e6ca706064d");
            public static BlueprintProgression BloodlineSerpentineProgression => Resources.GetBlueprint<BlueprintProgression>("739c1e842bf77994baf963f4ad964379");
            public static BlueprintProgression BloodlineUndeadProgression => Resources.GetBlueprint<BlueprintProgression>("a1a8bf61cadaa4143b2d4966f2d1142e");
            public static BlueprintProgression SorcererAberrantBloodline => Resources.GetModBlueprint<BlueprintProgression>("SorcererAberrantBloodline");
            public static BlueprintProgression SorcererDestinedBloodline => Resources.GetModBlueprint<BlueprintProgression>("SorcererDestinedBloodline");
            //Seeker Bloodlines
            public static BlueprintProgression SeekerBloodlineAbyssalProgression => Resources.GetBlueprint<BlueprintProgression>("17b752be1e0f4a34e8914df52eebeb75");
            public static BlueprintProgression SeekerBloodlineArcaneProgression => Resources.GetBlueprint<BlueprintProgression>("562c5e4031d268244a39e01cc4b834bb");
            public static BlueprintProgression SeekerBloodlineCelestialProgression => Resources.GetBlueprint<BlueprintProgression>("17ac9d771a944194a92ac15b5ff861c9");
            public static BlueprintProgression SeekerBloodlineDraconicBlackProgression => Resources.GetBlueprint<BlueprintProgression>("cf448fafcd8452d4b830bcc9ca074189");
            public static BlueprintProgression SeekerBloodlineDraconicBlueProgression => Resources.GetBlueprint<BlueprintProgression>("82f76646a7f96ed4cafa18480adc0b8c");
            public static BlueprintProgression SeekerBloodlineDraconicBrassProgression => Resources.GetBlueprint<BlueprintProgression>("c355b8777a3bda7429d863367bda3851");
            public static BlueprintProgression SeekerBloodlineDraconicBronzeProgression => Resources.GetBlueprint<BlueprintProgression>("468ecbdd58fbd6045a0a1888308031fe");
            public static BlueprintProgression SeekerBloodlineDraconicCopperProgression => Resources.GetBlueprint<BlueprintProgression>("2ad98f60b29ae604da0297037054080c");
            public static BlueprintProgression SeekerBloodlineDraconicGoldProgression => Resources.GetBlueprint<BlueprintProgression>("63c9d62a56e6921409a58de1ab9a9f9b");
            public static BlueprintProgression SeekerBloodlineDraconicGreenProgression => Resources.GetBlueprint<BlueprintProgression>("6de526eeee72852448c5595f7a44a39d");
            public static BlueprintProgression SeekerBloodlineDraconicRedProgression => Resources.GetBlueprint<BlueprintProgression>("d69a7785de1959c4497e4ff1e9490509");
            public static BlueprintProgression SeekerBloodlineDraconicSilverProgression => Resources.GetBlueprint<BlueprintProgression>("efabab987569bf54abf23848c250e4d5");
            public static BlueprintProgression SeekerBloodlineDraconicWhiteProgression => Resources.GetBlueprint<BlueprintProgression>("7572e8c020a6b8a46bde3ab3ad8c6f70");
            public static BlueprintProgression SeekerBloodlineElementalAirProgression => Resources.GetBlueprint<BlueprintProgression>("940d34be432a1e543b0c0cbecd4ffc1d");
            public static BlueprintProgression SeekerBloodlineElementalEarthProgression => Resources.GetBlueprint<BlueprintProgression>("1c1cdb13caa111d49bd82a7e1f320803");
            public static BlueprintProgression SeekerBloodlineElementalFireProgression => Resources.GetBlueprint<BlueprintProgression>("b19f95964e4a18f4cb3e4e3101593f22");
            public static BlueprintProgression SeekerBloodlineElementalWaterProgression => Resources.GetBlueprint<BlueprintProgression>("179a53407d1141142a91baace7e43325");
            public static BlueprintProgression SeekerBloodlineFeyProgression => Resources.GetBlueprint<BlueprintProgression>("8e6dcc9095dacd042a644dd8c04ffac0");
            public static BlueprintProgression SeekerBloodlineInfernalProgression => Resources.GetBlueprint<BlueprintProgression>("71f9b9d63f3683b4eb57e0025771932e");
            public static BlueprintProgression SeekerBloodlineSerpentineProgression => Resources.GetBlueprint<BlueprintProgression>("59904bf6cc50a52489ebc648fb35f36f");
            public static BlueprintProgression SeekerBloodlineUndeadProgression => Resources.GetBlueprint<BlueprintProgression>("5bc63fdb68b539f4fa500cfb2d0fe0f6");
            public static BlueprintProgression SeekerAberrantBloodline => Resources.GetModBlueprint<BlueprintProgression>("SeekerAberrantBloodline");
            public static BlueprintProgression SeekerDestinedBloodline => Resources.GetModBlueprint<BlueprintProgression>("SeekerDestinedBloodline");
            // Mutated Bloodlines
            public static BlueprintProgression EmpyrealBloodlineProgression => Resources.GetBlueprint<BlueprintProgression>("8a95d80a3162d274896d50c2f18bb6b1");
            public static BlueprintProgression SageBloodlineProgression => Resources.GetBlueprint<BlueprintProgression>("7d990675841a7354c957689a6707c6c2");
            public static BlueprintProgression SylvanBloodlineProgression => Resources.GetBlueprint<BlueprintProgression>("a46d4bd93601427409d034a997673ece");
        }
    }
}
