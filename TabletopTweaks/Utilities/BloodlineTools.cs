using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
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

            SecondBloodragerBloodline.m_Features = BloodragerBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());
            SecondBloodragerBloodline.m_AllFeatures = BloodragerBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());
            BloodragerBloodlineSelection.m_AllFeatures = BloodragerBloodlineSelection.m_AllFeatures.AppendToArray(bloodline.ToReference<BlueprintFeatureReference>());
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
    }
}
