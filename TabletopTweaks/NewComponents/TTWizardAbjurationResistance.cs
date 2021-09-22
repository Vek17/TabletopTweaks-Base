using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents
{
    [ComponentName("Resist Energy")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [TypeId("3d883633ca184f348ce77cd5376c78cb")]
    public class TTWizardAbjurationResistance : TTAddDamageResistanceEnergy
    {
        [SerializeField]
        [FormerlySerializedAs("Wizard")]
        private BlueprintCharacterClassReference m_Wizard;

        public BlueprintCharacterClass Wizard => this.m_Wizard?.Get();

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other)
        {
            return other is TTWizardAbjurationResistance otherWizardResist && this.m_Wizard == otherWizardResist.m_Wizard && base.IsSameDRTypeAs(other);
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance)
        {
            base.AdditionalInitFromVanillaDamageResistance(vanillaResistance);
            if (vanillaResistance is Kingmaker.Designers.Mechanics.Buffs.WizardAbjurationResistance vanillaWizardResistance)
            {
                m_Wizard = vanillaWizardResistance.m_Wizard;
            }
        }

        public override EntityFactComponent CreateRuntimeFactComponent() => (EntityFactComponent)new TTWizardAbjurationResistance.WizardAbjurationResistanceRuntime();

        public class WizardAbjurationResistanceRuntime :
          TTAddDamageResistanceBase.ComponentRuntime,
          ITargetRulebookHandler<RuleCalculateDamage>,
          IRulebookHandler<RuleCalculateDamage>,
          ISubscriber,
          ITargetRulebookSubscriber
        {
            private new TTWizardAbjurationResistance Settings => (TTWizardAbjurationResistance)base.Settings;

            public void OnEventAboutToTrigger(RuleCalculateDamage evt)
            {
                this.Settings.Immunity = this.Owner.Progression.GetClassLevel(this.Settings.Wizard) >= 20;                  
            }

            public void OnEventDidTrigger(RuleCalculateDamage evt)
            {
            }
        }
    }
}
