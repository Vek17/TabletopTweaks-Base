using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents
{
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("38d9082bfa624e3ab039a6d810cf99db")]
    public class TTWizardEnergyAbsorption : TTAddDamageResistanceEnergy
    {
        [SerializeField]
        [FormerlySerializedAs("Resource")]
        private BlueprintAbilityResourceReference m_Resource;

        public BlueprintAbilityResource Resource => this.m_Resource?.Get();

        protected override bool ShouldBeRemoved(TTAddDamageResistanceBase.ComponentRuntime runtime) => false;

        protected override int CalculateValue(TTAddDamageResistanceBase.ComponentRuntime runtime) => this.CalculateRemainingPool(runtime);

        protected override void OnSpendPool(
          TTAddDamageResistanceBase.ComponentRuntime runtime,
          int damage)
        {
            runtime.Owner.Resources.Spend((BlueprintScriptableObject)this.Resource, Math.Min(this.CalculateRemainingPool(runtime), damage));
        }

        protected override int CalculateRemainingPool(TTAddDamageResistanceBase.ComponentRuntime runtime) => runtime.Owner.Resources.GetResourceAmount((BlueprintScriptableObject)this.Resource);

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other)
        {
            return other is TTWizardEnergyAbsorption otherWizardAbsorb && this.m_Resource == otherWizardAbsorb.m_Resource && base.IsSameDRTypeAs(other);
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance)
        {
            base.AdditionalInitFromVanillaDamageResistance(vanillaResistance);
            if (vanillaResistance is Kingmaker.Designers.Mechanics.Buffs.WizardEnergyAbsorption vanillaWizardAbsorb)
            {
                this.m_Resource = vanillaWizardAbsorb.m_Resource;
                this.Immunity = true;
                this.Priority = DRPriority.Low;
            }
        }
    }
}
