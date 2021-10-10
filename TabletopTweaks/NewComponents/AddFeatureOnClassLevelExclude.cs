using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    class AddFeatureOnClassLevelExclude : UnitFactComponentDelegate<AddFeatureOnClassLevelData>, IOwnerGainLevelHandler, IUnitSubscriber, ISubscriber {

        public override void OnActivate() {
            Apply();
        }

        public override void OnDeactivate() {
            base.Owner.RemoveFact(base.Data.AppliedFact);
            base.Data.AppliedFact = null;
        }

        public void HandleUnitGainLevel() {
            Apply();
        }

        private void Apply() {
            if (IsFeatureShouldBeApplied()) {
                if (base.Data.AppliedFact == null) {
                    base.Data.AppliedFact = base.Owner.AddFact(m_Feature, null, null);
                    return;
                }
            } else if (base.Data.AppliedFact != null) {
                base.Owner.RemoveFact(base.Data.AppliedFact);
                base.Data.AppliedFact = null;
            }
        }

        private bool IsFeatureShouldBeApplied() {
            int num = ReplaceCasterLevelOfAbility.CalculateClassLevel(m_Class, m_AdditionalClasses.Select(c => c.Get()).ToArray(), base.Owner, m_Archetypes.Select(c => c.Get()).ToArray());
            num -= ReplaceCasterLevelOfAbility.CalculateClassLevel(m_Class, m_AdditionalClasses.Select(c => c.Get()).ToArray(), base.Owner, m_ExcludeArchetypes.Select(c => c.Get()).ToArray());
            return (!BeforeThisLevel || num < Level) && ((num < Level && BeforeThisLevel) || (num >= Level && !BeforeThisLevel));
        }


        public override void OnPostLoad() {
            base.OnPostLoad();
            bool flag = base.Data.AppliedFact != null && !base.Owner.HasFact(base.Data.AppliedFact);
            if (flag) {
                base.Data.AppliedFact.Dispose();
                base.Data.AppliedFact = null;
            }
            if (flag && BlueprintRoot.Instance.PlayerUpgradeActions.AllowedForRestoreFeatures.HasReference(m_Feature)) {
                this.Apply();
            }
        }

        public BlueprintCharacterClassReference m_Class;
        public int Level;
        public BlueprintFeatureReference m_Feature;
        public bool BeforeThisLevel;
        public BlueprintCharacterClassReference[] m_AdditionalClasses;
        public BlueprintArchetypeReference[] m_Archetypes;
        public BlueprintArchetypeReference[] m_ExcludeArchetypes;
    }
}
