using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Utilities {
    public static class BloodlineTools {

        public static void AddActionIfTrue(this Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional conditional, GameAction game_action) {
            if (conditional.IfTrue != null) {
                conditional.IfTrue = Helpers.CreateActionList(conditional.IfTrue.Actions);
                conditional.IfTrue.Actions = conditional.IfTrue.Actions.AddToArray(game_action);
            }
            else {
                conditional.IfTrue = Helpers.CreateActionList(game_action);
            }
        }
        public static void AddActionIfFalse(this Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional conditional, GameAction game_action) {
            if (conditional.IfFalse != null) {
                conditional.IfFalse = Helpers.CreateActionList(conditional.IfFalse.Actions);
                conditional.IfFalse.Actions = conditional.IfFalse.Actions.AddToArray(game_action);
            }
            else {
                conditional.IfFalse = Helpers.CreateActionList(game_action);
            }
        }
        public static void AddActionActivated(this AddFactContextActions component, GameAction game_action) {
            if (component.Activated == null) {
                component.Activated = new ActionList();
            }
            component.Activated.Actions = component.Activated.Actions.AddToArray(game_action);
        }
        public static void AddActionDeactivated(this AddFactContextActions component, GameAction game_action) {
            if (component.Deactivated == null) {
                component.Deactivated = new ActionList();
            }
            component.Deactivated.Actions = component.Deactivated.Actions.AddToArray(game_action);
        }
        public static void AddConditionalBuff(this BlueprintBuff parent, BlueprintFeature hasFeature, BlueprintBuff buff) {
            var AddfactContext = parent.GetComponent<AddFactContextActions>();
            if (!AddfactContext) {
                parent.AddComponent(Helpers.Create<AddFactContextActions>());
                AddfactContext = parent.GetComponent<AddFactContextActions>();
            }
            AddfactContext.AddActionActivated(Helpers.Create<Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional>(c => {
                c.ConditionsChecker = new ConditionsChecker();
                c.ConditionsChecker.Conditions = new Condition[] { Helpers.Create<ContextConditionHasFact>(condition => {
                        condition.m_Fact = hasFeature.ToReference<BlueprintUnitFactReference>();
                    })
                };
                c.AddActionIfTrue(Helpers.Create<ContextActionApplyBuff>(context => {
                    context.m_Buff = buff.ToReference<BlueprintBuffReference>();
                    context.AsChild = true;
                    context.Permanent = true;
                }));
            }));
            AddfactContext.AddActionDeactivated(Helpers.Create<Kingmaker.Designers.EventConditionActionSystem.Actions.Conditional>(c => {
                c.ConditionsChecker = new ConditionsChecker();
                c.ConditionsChecker.Conditions = new Condition[] { Helpers.Create<ContextConditionHasFact>(condition => {
                        condition.m_Fact = hasFeature.ToReference<BlueprintUnitFactReference>();
                    })
                };
                c.AddActionIfTrue(Helpers.Create<ContextActionRemoveBuff>(context => {
                    context.m_Buff = buff.ToReference<BlueprintBuffReference>();
                }));
            }));
        }
        public static void ApplyPrimalistException(BlueprintFeature power, int level, BlueprintProgression bloodline) {
            BlueprintFeature PrimalistProgression = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("d8b8d1dd83393484cbacf6c8830080ae");
            BlueprintFeature PrimalistTakePower4 = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("2140040bf367e8b4a9c6a632820becbe");
            BlueprintFeature PrimalistTakePower8 = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("c5aaccc685a37ed4b97869398cdd3ebb");
            BlueprintFeature PrimalistTakePower12 = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("57bb4dc36611c7444817c13135ec58b4");
            BlueprintFeature PrimalistTakePower16 = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("a56a288b9b6097f4eb67be43404321f2");
            BlueprintFeature PrimalistTakePower20 = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("b264a03d036248544acfddbcad709345");
            SelectedPrimalistLevel().AddComponent(
                Helpers.Create<AddFeatureIfHasFact>(c => {
                    c.m_Feature = power.ToReference<BlueprintUnitFactReference>();
                    c.m_CheckedFact = bloodline.ToReference<BlueprintUnitFactReference>();
                })
            );
            power.AddComponent(Helpers.Create<PrerequisiteNoFeature>(p => {
                p.CheckInProgression = true;
                p.m_Feature = PrimalistProgression.ToReference<BlueprintFeatureReference>();

            }));
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
        public static void RegisterBloodragerBloodline(BlueprintProgression bloodline) {
            BlueprintFeatureSelection BloodragerBloodlineSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("62b33ac8ceb18dd47ad4c8f06849bc01");
            BlueprintFeatureSelection SecondBloodragerBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("b7f62628915bdb14d8888c25da3fac56");

            SecondBloodragerBloodline.m_Features = BloodragerBloodlineSelection.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());
            SecondBloodragerBloodline.m_AllFeatures = BloodragerBloodlineSelection.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());
            BloodragerBloodlineSelection.m_AllFeatures = BloodragerBloodlineSelection.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());
        }
        public static void RegisterSorcererBloodline(BlueprintProgression bloodline) {
            BlueprintFeatureSelection SorcererBloodlineSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("24bef8d1bee12274686f6da6ccbc8914");
            BlueprintFeatureSelection SecondBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("3cf2ab2c320b73347a7c21cf0d0995bd");
            BlueprintFeatureSelection BloodlineAscendance = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19");

            SorcererBloodlineSelection.m_AllFeatures = SorcererBloodlineSelection.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());
            SecondBloodline.m_AllFeatures = SecondBloodline.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());

            var capstone = bloodline.LevelEntries.Where(entry => entry.Level == 20).First().Features[0];
            capstone.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                c.m_Feature = bloodline.ToReference<BlueprintFeatureReference>();
                c.Group = Prerequisite.GroupType.All;
            }));
            BloodlineAscendance.m_Features = BloodlineAscendance.m_AllFeatures.AddToArray(capstone.ToReference<BlueprintFeatureReference>());
            BloodlineAscendance.m_AllFeatures = BloodlineAscendance.m_AllFeatures.AddToArray(capstone.ToReference<BlueprintFeatureReference>());
        }
        public static void RegisterSorcererFeatSelection (BlueprintFeatureSelection selection, BlueprintProgression bloodline) {
            BlueprintFeatureSelection SorcererFeatSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("3a60f0c0442acfb419b0c03b584e1394");
            SorcererFeatSelection.m_Features = SorcererFeatSelection.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());
            SorcererFeatSelection.m_AllFeatures = SorcererFeatSelection.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());
        }
        public static void RegisterCrossbloodedBloodline(BlueprintProgression bloodline) {
            BlueprintFeatureSelection CrossbloodedSecondaryBloodlineSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("60c99d78a70e0b44f87ba01d02d909a6");
            CrossbloodedSecondaryBloodlineSelection.m_Features = CrossbloodedSecondaryBloodlineSelection.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());
            CrossbloodedSecondaryBloodlineSelection.m_AllFeatures = CrossbloodedSecondaryBloodlineSelection.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());
        }
        public static void RegisterSeekerBloodline(BlueprintProgression bloodline) {
            BlueprintFeatureSelection SeekerBloodlineSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("7bda7cdb0ccda664c9eb8978cf512dbc");
            SeekerBloodlineSelection.m_Features = SeekerBloodlineSelection.m_Features.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());
            SeekerBloodlineSelection.m_AllFeatures = SeekerBloodlineSelection.m_AllFeatures.AddToArray(bloodline.ToReference<BlueprintFeatureReference>());

            var capstone = bloodline.LevelEntries.Where(entry => entry.Level == 20).First().Features[0];
            capstone.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                c.m_Feature = bloodline.ToReference<BlueprintFeatureReference>();
                c.Group = Prerequisite.GroupType.All;
            }));
        }
    }
}
