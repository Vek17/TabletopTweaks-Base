using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace TabletopTweaks.NewComponents {
    [TypeId("11729578161344d99990125910019bfd")]
    class IncreaseResourceAmountByWeaponTraining : UnitFactComponentDelegate, IResourceAmountBonusHandler, IUnitSubscriber, ISubscriber {

        public BlueprintAbilityResource Resource {
            get {
                if (m_Resource == null) {
                    return null;
                }
                return m_Resource.Get();
            }
        }

        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus) {
            var weaponTraining = Owner.Get<UnitPartWeaponTraining>();
            if (base.Fact.Active && weaponTraining  != null && resource == this.Resource) {
                bonus += weaponTraining.GetMaxWeaponRank();
            }
        }
        public BlueprintAbilityResourceReference m_Resource;
    }
}
