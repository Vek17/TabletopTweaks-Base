using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Validation;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using UnityEngine;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    class AddStatBonusIfHasFactTTT : UnitBuffComponentDelegate, IUnitGainFactHandler, IUnitSubscriber, ISubscriber, IUnitLostFactHandler {
        public ReferenceArrayProxy<BlueprintUnitFact, BlueprintUnitFactReference> CheckedFacts {
            get {
                return this.m_CheckedFacts;
            }
        }

        public override void OnTurnOn() {
            this.Update();
        }

        public override void OnTurnOff() {
            this.Cancel();
        }

        private bool ShouldApplyBonus() {
            bool flag = this.RequireAllFacts || this.m_CheckedFacts.Length == 1;
            foreach (BlueprintUnitFact blueprint in this.CheckedFacts) {
                bool flag2 = base.Owner.HasFact(blueprint);
                if (flag2 && !flag) {
                    return !this.InvertCondition;
                }
                if (!flag2 && flag) {
                    return this.InvertCondition;
                }
            }
            return !this.InvertCondition;
        }

        private void Update() {
            if (this.ShouldApplyBonus()) {
                int value = this.Value.Calculate(base.Context);
                base.Owner.Stats.GetStat(this.Stat).AddModifierUnique(value, base.Runtime, this.Descriptor);
                return;
            }
            ModifiableValue stat = base.Owner.Stats.GetStat(this.Stat);
            if (stat == null) {
                return;
            }
            stat.RemoveModifiersFrom(base.Runtime);
        }

        private void Cancel() {
            ModifiableValue stat = base.Owner.Stats.GetStat(this.Stat);
            if (stat == null) {
                return;
            }
            stat.RemoveModifiersFrom(base.Runtime);
        }

        public void HandleUnitGainFact(EntityFact fact) {
            BlueprintUnitFact bp = fact.Blueprint as BlueprintUnitFact;
            if (bp != null && this.CheckedFacts.HasReference(bp)) {
                this.Update();
            }
        }

        public void HandleUnitLostFact(EntityFact fact) {
            BlueprintUnitFact bp = fact.Blueprint as BlueprintUnitFact;
            if (bp != null && this.CheckedFacts.HasReference(bp)) {
                this.Update();
            }
        }

        public ModifierDescriptor Descriptor;
        public StatType Stat;
        public ContextValue Value;
        public bool InvertCondition;
        public bool RequireAllFacts;
        [SerializeField]
        [ValidateNotEmpty]
        public  BlueprintUnitFactReference[] m_CheckedFacts;
    }
}
