using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class MythicBanner {
        public static void AddMythicBanner() {
            var CavalierBanner = BlueprintTools.GetBlueprint<BlueprintFeature>("2d957edad0adb3d49991cfcd3ac4cbf8");
            var StandardBearerAwesomePennonFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("638549b003555e948bd67fa6b55ebad6");

            var MythicBannerBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "MythicBannerBuff", bp => {
                bp.SetName(TTTContext, "Mythic Banner");
                bp.SetDescription(TTTContext, "");
                bp.m_Icon = StandardBearerAwesomePennonFeature.Icon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Value = 2;
                    c.Descriptor = ModifierDescriptor.Morale;
                });
                bp.AddComponent<CriticalConfirmationACBonus>(c => {
                    c.Value = new ContextValue() { 
                        ValueType = ContextValueType.Rank
                    };
                    c.Bonus = 0;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.MythicLevel;
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                });
            });
            var MythicBannerArea = Helpers.CreateBlueprint<BlueprintAbilityAreaEffect>(TTTContext, "MythicBannerArea", bp => {
                bp.Size = 60.Feet();
                bp.Shape = AreaEffectShape.Cylinder;
                bp.Fx = new PrefabLink();
                bp.AddComponent<AbilityAreaEffectRunAction>(c => {
                    c.UnitEnter = Helpers.CreateActionList(
                        Helpers.Create<Conditional>(condition => {
                            condition.ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] { new ContextConditionIsAlly() }
                            };
                            condition.IfTrue = Helpers.CreateActionList(
                                new ContextActionApplyBuff() { 
                                    m_Buff = MythicBannerBuff.ToReference<BlueprintBuffReference>(),
                                    Permanent = true,
                                    DurationValue = new ContextDurationValue() { 
                                        DiceCountValue = 0,
                                        BonusValue = 0
                                    }
                                }
                            );
                            condition.IfFalse = Helpers.CreateActionList();
                        })    
                    );
                    c.UnitExit = Helpers.CreateActionList(
                        Helpers.Create<Conditional>(condition => {
                            condition.ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] { new ContextConditionIsAlly() }
                            };
                            condition.IfTrue = Helpers.CreateActionList(
                                new ContextActionRemoveBuff() {
                                    m_Buff = MythicBannerBuff.ToReference<BlueprintBuffReference>()
                                }
                            );
                            condition.IfFalse = Helpers.CreateActionList();
                        })
                    );
                    c.Round = Helpers.CreateActionList();
                    c.UnitMove = Helpers.CreateActionList();
                });
            });
            var MythicBannerAreaBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "MythicBannerAreaBuff", bp => {
                bp.SetName(TTTContext, "Mythic Banner Area");
                bp.SetDescription(TTTContext, "");
                bp.m_Icon = StandardBearerAwesomePennonFeature.Icon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddAreaEffect>(c => {
                    c.m_AreaEffect = MythicBannerArea.ToReference<BlueprintAbilityAreaEffectReference>();
                });
            });
            var MythicBannerFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MythicBannerFeature", bp => {
                bp.SetName(TTTContext, "Mythic Banner");
                bp.SetDescription(TTTContext, "Your banner has been touched by mythic power and now protects those nearby.\n" +
                    "All allies within 60 feet gain a +2 morale bonus to AC. " +
                    "They gain an additional bonus to AC against attack rolls made to confirm a critical hit equal to half your mythic rank.");
                bp.m_Icon = StandardBearerAwesomePennonFeature.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { MythicBannerAreaBuff.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent<AuraFeatureComponent>(c => {
                    c.m_Buff = MythicBannerAreaBuff.ToReference<BlueprintBuffReference>();
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = ClassTools.Classes.CavalierClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 5;
                });
                bp.AddPrerequisiteFeature(CavalierBanner);
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("MythicBanner")) { return; }
            FeatTools.AddAsMythicAbility(MythicBannerFeature);
        }
    }
}
