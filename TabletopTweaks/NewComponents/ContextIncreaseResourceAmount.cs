using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.NewComponents {
    [AllowMultipleComponents]
    [TypeId("d4fdb55c61b64258b40be8a6ca49980d")]
    class ContextIncreaseResourceAmount : UnitFactComponentDelegate, IResourceAmountBonusHandler, IUnitSubscriber, ISubscriber {

        public BlueprintAbilityResource Resource {
            get {
                BlueprintAbilityResourceReference resource = this.m_Resource;
                if (resource == null) {
                    return null;
                }
                return resource.Get();
            }
        }

        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus) {
            if (base.Fact.Active && resource == m_Resource.Get()) {
                if (Subtract) {
                    bonus -= Value.Calculate(base.Context);
                } else {
                    bonus += Value.Calculate(base.Context);
                }
            }
        }

        public BlueprintAbilityResourceReference m_Resource;
        public ContextValue Value;
        public bool Subtract;
    }
}
