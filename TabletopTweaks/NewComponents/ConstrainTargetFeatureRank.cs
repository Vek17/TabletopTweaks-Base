using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents {
    [TypeId("c1e7344a9870459a8c9fe2f8f4cf5b5e")]
    [AllowedOn(typeof(BlueprintFeature))]
    class ConstrainTargetFeatureRank : UnitFactComponentDelegate<CompanionBoonData>, ILevelUpCompleteUIHandler, IGlobalSubscriber, ISubscriber {

        public override void OnActivate() {
            Apply();
        }

        public override void OnDeactivate() {
            while (base.Data.AppliedRank > 0) {
                base.Owner.RemoveFact(this.TargetFeature);
                base.Data.AppliedRank--;
            }
        }

        private void Apply() {
            EntityFact baseFact = base.Owner.GetFact(base.Fact.Blueprint);
            EntityFact targetFact = base.Owner.GetFact(TargetFeature);
            int baseRank = baseFact?.GetRank() ?? 0;
            int targetRank = targetFact?.GetRank() ?? 0;
            while (baseRank > targetRank) {
                EntityFact targetFact2 = base.Owner.AddFact(this.TargetFeature);
                base.Data.AppliedRank++;
                targetRank = targetFact2?.GetRank() ?? 0;
            }
        }

        public void HandleLevelUpComplete(UnitEntityData unit, bool isChargen) {
            Apply();
        }

        [SerializeField]
        [FormerlySerializedAs("RankFeature")]
        public BlueprintFeatureReference TargetFeature;
    }
}
