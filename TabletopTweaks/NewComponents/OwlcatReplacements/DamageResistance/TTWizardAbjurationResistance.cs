using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.OwlcatReplacements.DamageResistance {
    [ComponentName("Resist Energy")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowedOn(typeof(BlueprintUnit), false)]
    [TypeId("3d883633ca184f348ce77cd5376c78cb")]
    public class TTWizardAbjurationResistance : TTAddDamageResistanceEnergy {
        [SerializeField]
        [FormerlySerializedAs("Wizard")]
        private BlueprintCharacterClassReference m_Wizard;

        public BlueprintCharacterClass Wizard => m_Wizard?.Get();

        public override bool IsSameDRTypeAs(TTAddDamageResistanceBase other) {
            return other is TTWizardAbjurationResistance otherWizardResist && m_Wizard == otherWizardResist.m_Wizard && base.IsSameDRTypeAs(other);
        }

        protected override void AdditionalInitFromVanillaDamageResistance(Kingmaker.UnitLogic.FactLogic.AddDamageResistanceBase vanillaResistance) {
            base.AdditionalInitFromVanillaDamageResistance(vanillaResistance);
            if (vanillaResistance is Kingmaker.Designers.Mechanics.Buffs.WizardAbjurationResistance vanillaWizardResistance) {
                m_Wizard = vanillaWizardResistance.m_Wizard;
            }
        }

        public override EntityFactComponent CreateRuntimeFactComponent() => new WizardAbjurationResistanceRuntime();

        public class WizardAbjurationResistanceRuntime :
          ComponentRuntime,
          ITargetRulebookHandler<RuleCalculateDamage>,
          IRulebookHandler<RuleCalculateDamage>,
          ISubscriber,
          ITargetRulebookSubscriber {
            private new TTWizardAbjurationResistance Settings => (TTWizardAbjurationResistance)base.Settings;

            public void OnEventAboutToTrigger(RuleCalculateDamage evt) {
                Settings.Immunity = Owner.Progression.GetClassLevel(Settings.Wizard) >= 20;
            }

            public void OnEventDidTrigger(RuleCalculateDamage evt) {
            }
        }
    }
}
