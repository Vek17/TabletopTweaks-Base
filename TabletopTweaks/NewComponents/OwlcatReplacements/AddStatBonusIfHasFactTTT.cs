using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Owlcat.QA.Validation;
using UnityEngine;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [AllowMultipleComponents]
    [TypeId("7601b8133d844f04956d9bc9a1cce210")]
    public class AddStatBonusIfHasFactTTT : UnitBuffComponentDelegate, IUnitGainFactHandler, IUnitSubscriber, ISubscriber, IUnitLostFactHandler {
        public ReferenceArrayProxy<BlueprintUnitFact, BlueprintUnitFactReference> CheckedFacts {
            get {
                return this.m_CheckedFacts;
            }
        }

        public ReferenceArrayProxy<BlueprintUnitFact, BlueprintUnitFactReference> BlockedFacts {
            get {
                return this.m_BlockedFacts;
            }
        }

        public override void OnTurnOn() {
            this.Update();
        }

        public override void OnTurnOff() {
            this.Cancel();
        }

        private bool ShouldApplyBonus() {

            foreach (BlueprintUnitFact blueprint in this.BlockedFacts) {
                if (base.Owner.HasFact(blueprint)) {
                    return false;
                }
            }
            bool allFacts = this.RequireAllFacts || this.m_CheckedFacts.Length == 1;
            foreach (BlueprintUnitFact blueprint in this.CheckedFacts) {
                bool hasFact = base.Owner.HasFact(blueprint);
                if (hasFact && !allFacts) {
                    return !this.InvertCondition;
                }
                if (!hasFact && allFacts) {
                    return this.InvertCondition;
                }
            }
            return !this.InvertCondition;
        }

        private void Update() {
            ModifiableValue stat = base.Owner.Stats.GetStat(this.Stat);
            if (stat == null) { return; }
            if (this.ShouldApplyBonus()) {
                int value = this.Value.Calculate(base.Context);
                stat.AddModifierUnique(value, base.Runtime, this.Descriptor);
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
            if (bp != null && (this.CheckedFacts.HasReference(bp) || this.BlockedFacts.HasReference(bp))) {
                this.Update();
            }
        }

        public void HandleUnitLostFact(EntityFact fact) {
            BlueprintUnitFact bp = fact.Blueprint as BlueprintUnitFact;
            if (bp != null && (this.CheckedFacts.HasReference(bp) || this.BlockedFacts.HasReference(bp))) {
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
        public BlueprintUnitFactReference[] m_CheckedFacts;
        public BlueprintUnitFactReference[] m_BlockedFacts;
    }
}
