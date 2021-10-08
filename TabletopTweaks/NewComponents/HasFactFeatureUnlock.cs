using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace TabletopTweaks.NewComponents {
    class HasFactFeatureUnlock : UnitFactComponentDelegate<AddFeatureIfHasFactData>, IUnitGainFactHandler, IUnitLostFactHandler, IUnitSubscriber, ISubscriber {
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
            OnActivate();
        }

        public override void OnTurnOff() {
            OnDeactivate();
        }

        public override void OnActivate() {
            Apply();
        }

        public override void OnDeactivate() {
            RemoveFact();
        }

        private void Apply() {
            RemoveFact();
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

        public void HandleUnitGainFact(EntityFact fact) {
            Apply();
        }

        public void HandleUnitLostFact(EntityFact fact) {
            Apply();
        }

        [SerializeField]
        public BlueprintUnitFactReference m_CheckedFact;

        [SerializeField]
        public BlueprintUnitFactReference m_Feature;

        public bool Not;
    }
}
