using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.MechanicsChanges;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Classes {
    internal class Barbarian {
        public static void AddBarbarianFeatures() {
            var CunningElusionUnlockFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("91c8b2e3abdb4e2e807fddb668b619f8");

            var InstinctualWarriorACBonusBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "InstinctualWarriorACBonusBuff", bp => {
                bp.SetName(CunningElusionUnlockFeature.m_DisplayName);
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Descriptor = (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.Wisdom;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                    c.Multiplier = 1;
                });
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Descriptor = (ModifierDescriptor)AdditionalModifierDescriptors.Untyped.Monk;
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    c.Multiplier = 1;
                });
                bp.AddComponent<RecalculateOnStatChange>();
                bp.AddComponent<RecalculateOnFactsChange>();
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.StatBonus;
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_Stat = StatType.Wisdom;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMin = true;
                    c.m_Min = 0;
                });
                bp.AddContextRankConfig(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Type = AbilityRankType.Default;
                    c.m_Progression = ContextRankProgression.DivStep;
                    c.m_StepLevel = 4;
                    c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.ShifterClass };
                });
            });
        }
    }
}
