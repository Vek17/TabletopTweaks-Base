using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace TabletopTweaks.NewComponents {
    public class HasFactFeatureUnlock : UnitFactComponentDelegate<AddFeatureIfHasFactData>, IUnitGainFactHandler, IUnitLostFactHandler, IUnitSubscriber, ISubscriber {
        public BlueprintUnitFact CheckedFact {
            get {
                BlueprintUnitFactReference checkedFact = m_CheckedFact;
                if (checkedFact == null) {
                    return null;
                }
                return checkedFact.Get();
            }
        }

        public BlueprintUnitFact Feature {
            get {
                BlueprintUnitFactReference feature = m_Feature;
                if (feature == null) {
                    return null;
                }
                return feature.Get();
            }
        }

        public override void OnTurnOn() {
            Update();
        }

        public override void OnTurnOff() {
            RemoveFact();
        }

        public override void OnActivate() {
            Update();
        }

        public override void OnDeactivate() {
            RemoveFact();
        }

        public override void OnPostLoad() {
            Update();
        }

        private void Apply() {
            if (base.Data.AppliedFact != null) {
                return;
            }
            if ((Owner.HasFact(CheckedFact) && !Not) || (!Owner.HasFact(CheckedFact) && Not)) {
                base.Data.AppliedFact = Owner.AddFact(Feature, null, null);
            }
        }

        private void RemoveFact() {
            if (Data.AppliedFact != null) {
                Owner.RemoveFact(Data.AppliedFact);
                Data.AppliedFact = null;
            }
        }

        private void Update() {
            if (ShouldApply()) {
                Apply();
            } else {
                RemoveFact();
            }
        }

        private bool ShouldApply() {
            return base.Data.AppliedFact == null && (Owner.HasFact(CheckedFact) && !Not) || (!Owner.HasFact(CheckedFact) && Not);
        }

        public void HandleUnitGainFact(EntityFact fact) {
            Update();
        }

        public void HandleUnitLostFact(EntityFact fact) {
            Update();
        }

        [SerializeField]
        public BlueprintUnitFactReference m_CheckedFact;

        [SerializeField]
        public BlueprintUnitFactReference m_Feature;

        public bool Not;
    }
}
