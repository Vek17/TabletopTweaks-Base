using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.Kineticist.Properties;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;

namespace TabletopTweaks.NewComponents {

    [TypeId("157d986776584a2da0729efd762e5598")]
    public class StalwartDRProperty : PropertyValueGetter {
        private BlueprintUnitFactReference m_CombatExpertiseBuff;

        private BlueprintUnitFactReference m_FightingDefensivelyBuff;
        private BlueprintUnitFactReference m_CraneStyleBuff;
        private BlueprintUnitFactReference m_CautiousFighter;

        private BlueprintUnitFactReference m_SwordLordSteelNetFeature;

        private BlueprintUnitFactReference m_StalwartImprovedFeature;

        public StalwartDRProperty(
            BlueprintUnitFactReference combatExpertiseBuff,
            BlueprintUnitFactReference fightingDefensivelyBuff,
            BlueprintUnitFactReference craneStyleBuff,
            BlueprintUnitFactReference cautiousFighter,
            BlueprintUnitFactReference swordLordSteelNetFeature,
            BlueprintUnitFactReference stalwartImprovedFeature) {
            m_CombatExpertiseBuff = combatExpertiseBuff;
            m_FightingDefensivelyBuff = fightingDefensivelyBuff;
            m_CraneStyleBuff = craneStyleBuff;
            m_CautiousFighter = cautiousFighter;
            m_SwordLordSteelNetFeature = swordLordSteelNetFeature;
            m_StalwartImprovedFeature = stalwartImprovedFeature;
        }

        public override int GetBaseValue(UnitEntityData unit) {
            int num = 0;
            EntityFact fightingDefensivelyFact = unit.Descriptor.GetFact(m_FightingDefensivelyBuff);
            if (fightingDefensivelyFact != null) {
                num = unit.Stats.SkillMobility.BaseValue >= 3 ? 3 : 2;
                num += unit.Descriptor.GetFact(m_CraneStyleBuff) == null ? 0 : 1;
                num += unit.Descriptor.GetFact(m_CautiousFighter) == null ? 0 : 2;
                num += unit.Descriptor.GetFact(m_SwordLordSteelNetFeature) == null || !FightingDefensivelyAttackPenaltyProperty.CheckDuelingSword(unit) ? 0 : 2;
            }
            EntityFact combatExpertiseFact = unit.Descriptor.GetFact(m_CombatExpertiseBuff);
            if (combatExpertiseFact != null) {
                num += 1 + unit.Stats.BaseAttackBonus.ModifiedValue / 4;
            }

            EntityFact stalwartImprovedFeature = unit.Descriptor.GetFact(m_StalwartImprovedFeature);
            num *= stalwartImprovedFeature == null ? 1 : 2;

            int max = stalwartImprovedFeature == null ? 5 : 10;

            return Math.Min(num, max);
        }
    }
}
