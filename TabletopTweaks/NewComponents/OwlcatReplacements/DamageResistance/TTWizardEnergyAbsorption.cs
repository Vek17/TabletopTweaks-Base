using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.OwlcatReplacements.DamageResistance {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("38d9082bfa624e3ab039a6d810cf99db")]
    public class TTWizardEnergyAbsorption : TTAddDamageResistanceEnergy {
        [SerializeField]
        [FormerlySerializedAs("Resource")]
        private BlueprintAbilityResourceReference m_Resource;

        public BlueprintAbilityResource Resource => m_Resource?.Get();

        protected override bool ShouldBeRemoved(ComponentRuntime runtime) => false;

        protected override int CalculateValue(ComponentRuntime runtime) => CalculateRemainingPool(runtime);

        protected override void OnSpendPool(
          ComponentRuntime runtime,
          int damage) {
            runtime.Owner.Resources.Spend(Resource, Math.Min(CalculateRemainingPool(runtime), damage));
        }

        protected override int CalculateRemainingPool(ComponentRuntime runtime) => runtime.Owner.Resources.GetResourceAmount(Resource);

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other) {
            return other is TTWizardEnergyAbsorption otherWizardAbsorb && m_Resource == otherWizardAbsorb.m_Resource && base.IsSameDRTypeAs(other);
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance) {
            base.AdditionalInitFromVanillaDamageResistance(vanillaResistance);
            if (vanillaResistance is Kingmaker.Designers.Mechanics.Buffs.WizardEnergyAbsorption vanillaWizardAbsorb) {
                m_Resource = vanillaWizardAbsorb.m_Resource;
                Immunity = true;
                Priority = DRPriority.Low;
            }
        }
    }
}
