using Kingmaker;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.Enums;

namespace TabletopTweaks.NewComponents {
    [AllowMultipleComponents]
	[AllowedOn(typeof(BlueprintUnitFact))]
	public class BuffDescriptorImmunityAgainstAlignment : UnitFactComponentDelegate, 
		IInitiatorRulebookHandler<RuleCanApplyBuff>, 
		IRulebookHandler<RuleCanApplyBuff>, 
		ISubscriber, 
		IInitiatorRulebookSubscriber, 
		IInitiatorRulebookHandler<RuleSavingThrow>, 
		IRulebookHandler<RuleSavingThrow> {

		public BlueprintUnitFact IgnoreFeature {
			get {
				BlueprintUnitFactReference ignoreFeature = this.m_IgnoreFeature;
				if (ignoreFeature == null) {
					return null;
				}
				return ignoreFeature.Get();
			}
		}

		public BlueprintUnitFact FactToCheck {
			get {
				BlueprintUnitFactReference factToCheck = this.m_FactToCheck;
				if (factToCheck == null) {
					return null;
				}
				return factToCheck.Get();
			}
		}

		private bool IsImmune(MechanicsContext context) {
			UnitEntityData maybeCaster = context.MaybeCaster;
			if ((maybeCaster != null) ? maybeCaster.State.Features.MythicReduceResistances : null) {
				return false;
			}
			bool buffHasDescriptor = this.Descriptor.HasAnyFlag(context.SpellDescriptor);
			bool noCaster = context.MaybeCaster == null;
			bool noImmunityBypassFeature = this.IgnoreFeature == null || noCaster || !context.MaybeCaster.Descriptor.HasFact(this.IgnoreFeature);
			bool noImmunityFact = !this.CheckFact || (!noCaster && context.MaybeCaster.Descriptor.HasFact(this.FactToCheck));
			bool casterHasAlignment = !noCaster && context.MaybeCaster.Descriptor.Alignment.Value.HasComponent(this.Alignment);
			bool spellHasAlignment = context.SpellDescriptor.HasAnyFlag(this.Alignment.GetAlignmentDescriptor());
			return buffHasDescriptor && noImmunityBypassFeature && noImmunityFact && (casterHasAlignment || spellHasAlignment);
		}

		public void OnEventAboutToTrigger(RuleCanApplyBuff evt) {
			if (this.IsImmune(evt.Context)) {
				evt.Immunity = true;
			}
		}

		public void OnEventDidTrigger(RuleCanApplyBuff evt) {
		}

		public void OnEventAboutToTrigger(RuleSavingThrow evt) {
			if (evt.Buff != null) {
				MechanicsContext context = evt.Reason.Context;
				CountableFlag flag;
				if (context == null) {
					flag = null;
				}
				else {
					UnitEntityData maybeCaster = context.MaybeCaster;
					flag = ((maybeCaster != null) ? maybeCaster.State.Features.MythicReduceResistances : null);
				}
				if (flag && this.Descriptor.HasAnyFlag(evt.Buff.SpellDescriptor)) {
					evt.D20.AddReroll(1, true, base.Fact);
				}
			}
		}
#pragma warning disable IDE0044 // Add readonly modifier
		public void OnEventDidTrigger(RuleSavingThrow evt) {
		}

		public AlignmentComponent Alignment;
		public SpellDescriptorWrapper Descriptor;

		[SerializeField]
		[FormerlySerializedAs("IgnoreFeature")]
		private BlueprintUnitFactReference m_IgnoreFeature = null;

		public bool CheckFact;

		[ShowIf("CheckFact")]
		[SerializeField]
		[FormerlySerializedAs("FactToCheck")]
		private BlueprintUnitFactReference m_FactToCheck = null;
#pragma warning restore IDE0044 // Add readonly modifier
	}
}
