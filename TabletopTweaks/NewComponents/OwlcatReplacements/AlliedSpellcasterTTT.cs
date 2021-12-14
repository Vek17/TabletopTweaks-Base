using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents.OwlcatReplacements {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("8c5941da63e245a5aa9ef7a1cbabbc33")]
    public class AlliedSpellcasterTTT : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleCalculateAbilityParams>, 
        IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber,
        IInitiatorRulebookHandler<RuleSpellResistanceCheck>, 
        IRulebookHandler<RuleSpellResistanceCheck>, IConcentrationBonusProvider {

        public BlueprintUnitFact AlliedSpellcasterFact {
            get {
                BlueprintUnitFactReference alliedSpellcasterFact = this.m_AlliedSpellcasterFact;
                if (alliedSpellcasterFact == null) {
                    return null;
                }
                return alliedSpellcasterFact.Get();
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt) {
            foreach (UnitEntityData unitEntityData in GameHelper.GetTargetsAround(base.Owner.Position, (float)this.Radius, true, false)) {
                if ((unitEntityData.Descriptor.HasFact(this.AlliedSpellcasterFact) || base.Owner.State.Features.SoloTactics) 
                    && unitEntityData != base.Owner 
                    && !unitEntityData.IsEnemy(base.Owner)) {
                    evt.AddBonusConcentration(2);
                    break;
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt) {
        }

        public void OnEventAboutToTrigger(RuleSpellResistanceCheck evt) {
            foreach (UnitEntityData unitEntityData in GameHelper.GetTargetsAround(base.Owner.Position, (float)this.Radius, true, false)) {
                if ((unitEntityData.Descriptor.HasFact(this.AlliedSpellcasterFact) || base.Owner.State.Features.SoloTactics) 
                    && unitEntityData != base.Owner 
                    && !unitEntityData.IsEnemy(base.Owner)) {
                    evt.AddSpellPenetration(2, ModifierDescriptor.UntypedStackable);
                    break;
                }
            }
        }

        public void OnEventDidTrigger(RuleSpellResistanceCheck evt) {
        }

        public int GetStaticConcentrationBonus(EntityFactComponent runtime) {
            return 0;
        }

        [SerializeField]
        [FormerlySerializedAs("AlliedSpellcasterFact")]
        public BlueprintUnitFactReference m_AlliedSpellcasterFact;

        public int Radius;
    }
}
