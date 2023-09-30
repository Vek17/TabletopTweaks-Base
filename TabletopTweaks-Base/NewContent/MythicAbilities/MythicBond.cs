using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    internal class MythicBond {
        public static void AddMythicBond() {
            var HuntersBondFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6dddf5ba2291f41498df2df7f8fa2b35");
            var HuntersBondAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("cd80ea8a7a07a9d4cb1a54e67a9390a5");
            var HuntersBondBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2f93cad6b132aac4e80728d7fa03a8aa");

            var MythicBondBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "MythicBondBuff", bp => {
                bp.SetName(HuntersBondBuff.m_DisplayName);
                bp.SetDescription(HuntersBondBuff.m_Description);
                bp.m_Icon = HuntersBondBuff.Icon;
                bp.m_Flags = HuntersBondBuff.m_Flags;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<ShareFavoredEnemies>(c => {
                    c.Half = false;
                });
            });
            var MythicBondFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "MythicBondFeature", bp => {
                bp.SetName(TTTContext, "Mythic Bond");
                bp.SetDescription(TTTContext, "Your ranger’s bond ability is more powerful than most.\n" +
                    "If your ranger’s bond is with your companions, you can activate the bond as a swift action, " +
                    "granting your allies your full favored enemy bonus instead of just half.");
                bp.m_Icon = HuntersBondAbility.Icon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.AddComponent<AutoMetamagic>(c => {
                    c.m_AllowedAbilities = AutoMetamagic.AllowedType.Any;
                    c.m_Spellbook = new BlueprintSpellbookReference();
                    c.m_IncludeClasses = new BlueprintCharacterClassReference[0];
                    c.m_ExcludeClasses = new BlueprintCharacterClassReference[0];
                    c.Metamagic = Metamagic.Quicken;
                    c.Abilities = new List<BlueprintAbilityReference>() {
                        HuntersBondAbility.ToReference<BlueprintAbilityReference>()
                    };
                });
                bp.AddPrerequisiteFeature(HuntersBondFeature);
            });
            HuntersBondAbility.TemporaryContext(bp => {
                bp.RemoveComponents<AbilityEffectRunAction>();
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new Conditional() {
                            ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    new ContextConditionCasterHasFact() {
                                        m_Fact = MythicBondFeature.ToReference<BlueprintUnitFactReference>()
                                    }
                                }
                            },
                            IfTrue = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    m_Buff = MythicBondBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        DiceCountValue = 0,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank
                                        }
                                    },
                                    AsChild = true
                                }
                            ),
                            IfFalse = Helpers.CreateActionList(
                                new ContextActionApplyBuff() {
                                    m_Buff = HuntersBondBuff.ToReference<BlueprintBuffReference>(),
                                    DurationValue = new ContextDurationValue() {
                                        DiceCountValue = 0,
                                        BonusValue = new ContextValue() {
                                            ValueType = ContextValueType.Rank
                                        }
                                    },
                                    AsChild = true
                                }
                            ),
                        }
                    );
                });
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("MythicBond")) { return; }
            FeatTools.AddAsMythicAbility(MythicBondFeature);
        }
    }
}
