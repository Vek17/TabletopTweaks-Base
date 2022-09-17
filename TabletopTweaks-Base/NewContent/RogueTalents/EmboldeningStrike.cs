using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.RogueTalents {
    internal static class EmboldeningStrike {
        public static void AddEmboldeningStrike() {

            var EmboldeningStrikeBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "EmboldeningStrikeBuff", bp => {
                bp.SetName(TTTContext, "Emboldening Strike");
                bp.SetDescription(TTTContext, "When a rogue with this talent hits a creature with a melee attack that deals sneak attack damage, " +
                    "she gains a +1 circumstance bonus on saving throws for every 2 sneak attack dice rolled (minimum +1) for 1 round.");
                //bp.m_Icon = Icon_StunningFistStagger;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SaveFortitude;
                    c.Descriptor = ModifierDescriptor.Circumstance;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SaveReflex;
                    c.Descriptor = ModifierDescriptor.Circumstance;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SaveWill;
                    c.Descriptor = ModifierDescriptor.Circumstance;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank
                    };
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                    c.m_CustomProperty = RogueTalentProperties.SneakAttackDiceProperty.ToReference<BlueprintUnitPropertyReference>();
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_UseMin = true;
                    c.m_Min = 1;
                });
            });
            var EmboldeningStrikeFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "EmboldeningStrikeFeature", bp => {
                bp.SetName(TTTContext, "Emboldening Strike");
                bp.SetDescription(TTTContext, "When a rogue with this talent hits a creature with a melee attack that deals sneak attack damage, " +
                    "she gains a +1 circumstance bonus on saving throws for every 2 sneak attack dice rolled (minimum +1) for 1 round.");
                //bp.m_Icon = Icon_StunningFistStagger;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => {
                    c.OnlyHit = true;
                    c.OnlySneakAttack = true;
                    c.CheckWeaponRangeType = true;
                    c.RangeType = WeaponRangeType.Melee;
                    c.Action = Helpers.CreateActionList(
                        new ContextActionApplyBuff() {
                            m_Buff = EmboldeningStrikeBuff.ToReference<BlueprintBuffReference>(),
                            ToCaster = true,
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
                    c.m_CustomProperty = RogueTalentProperties.SneakAttackDiceProperty.ToReference<BlueprintUnitPropertyReference>();
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_UseMin = true;
                    c.m_Min = 1;
                });
            });

            if (TTTContext.AddedContent.RogueTalents.IsDisabled("EmboldeningStrike")) { return; }
            FeatTools.AddAsRogueTalent(EmboldeningStrikeFeature);
        }
    }
}
