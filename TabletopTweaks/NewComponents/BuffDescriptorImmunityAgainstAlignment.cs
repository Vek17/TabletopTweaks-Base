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
	public class BuffDescriptorImmunityAgainstAlignment : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCanApplyBuff>, IRulebookHandler<RuleCanApplyBuff>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow> {
		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x060085C5 RID: 34245 RVA: 0x00213AC2 File Offset: 0x00211CC2
		public BlueprintUnitFact IgnoreFeature {
			get {
				BlueprintUnitFactReference ignoreFeature = this.m_IgnoreFeature;
				if (ignoreFeature == null) {
					return null;
				}
				return ignoreFeature.Get();
			}
		}

		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x060085C6 RID: 34246 RVA: 0x00213AD5 File Offset: 0x00211CD5
		public BlueprintUnitFact FactToCheck {
			get {
				BlueprintUnitFactReference factToCheck = this.m_FactToCheck;
				if (factToCheck == null) {
					return null;
				}
				return factToCheck.Get();
			}
		}

		// Token: 0x060085C7 RID: 34247 RVA: 0x00213AE8 File Offset: 0x00211CE8
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

		// Token: 0x060085C8 RID: 34248 RVA: 0x00213B91 File Offset: 0x00211D91
		public void OnEventAboutToTrigger(RuleCanApplyBuff evt) {
			if (this.IsImmune(evt.Context)) {
				evt.Immunity = true;
			}
		}

		// Token: 0x060085C9 RID: 34249 RVA: 0x000036D8 File Offset: 0x000018D8
		public void OnEventDidTrigger(RuleCanApplyBuff evt) {
		}

		// Token: 0x060085CA RID: 34250 RVA: 0x00213BA8 File Offset: 0x00211DA8
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
		// Token: 0x060085CB RID: 34251 RVA: 0x000036D8 File Offset: 0x000018D8
		public void OnEventDidTrigger(RuleSavingThrow evt) {
		}

		public AlignmentComponent Alignment;
		public SpellDescriptorWrapper Descriptor;

		// Token: 0x04005B50 RID: 23376
		[SerializeField]
		[FormerlySerializedAs("IgnoreFeature")]
		private BlueprintUnitFactReference m_IgnoreFeature = null;

		// Token: 0x04005B51 RID: 23377
		public bool CheckFact;

		// Token: 0x04005B52 RID: 23378
		[ShowIf("CheckFact")]
		[SerializeField]
		[FormerlySerializedAs("FactToCheck")]
		private BlueprintUnitFactReference m_FactToCheck = null;
#pragma warning restore IDE0044 // Add readonly modifier
	}
}
