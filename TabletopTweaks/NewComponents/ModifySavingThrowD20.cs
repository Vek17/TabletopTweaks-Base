using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("e8651c2865ee48f4a877df9d376bc37c")]
    public class ModifySavingThrowD20 : UnitFactComponentDelegate, 
        IInitiatorRulebookHandler<RuleSavingThrow>, 
        IRulebookHandler<RuleSavingThrow>,
        IInitiatorRulebookHandler<RuleRollD20>, 
        IRulebookHandler<RuleRollD20>,
        ISubscriber, IInitiatorRulebookSubscriber {

		public void OnEventAboutToTrigger(RuleSavingThrow evt) {
            bool flag = evt.Reason.Caster != null && evt.Reason.Caster.Descriptor.Alignment.ValueRaw.HasComponent(this.Alignment);
            if ((evt.Reason.Context != null && evt.Reason.Context.SpellDescriptor.HasAnyFlag(this.Alignment.GetAlignmentDescriptor())) || flag) {
                
            }
        }
        public void OnEventDidTrigger(RuleSavingThrow evt) {
        }

        public void OnEventAboutToTrigger(RuleRollD20 evt) {
            RulebookEvent previousEvent = Rulebook.CurrentContext.PreviousEvent as RuleSavingThrow;
            if (previousEvent == null) { return; }
            var casterHasAlignment = previousEvent.Reason.Caster != null 
                && previousEvent.Reason.Caster.Descriptor.Alignment.ValueRaw.HasComponent(this.Alignment);
            var sourceHasDescriptor = SpellDescriptor.HasAnyFlag(previousEvent.Reason.Context.SpellDescriptor);
            if (AgainstAlignment && !casterHasAlignment) { return; }
            if (SpecificDescriptor && !sourceHasDescriptor) { return; }
            evt.Override(Roll);
        }

        public void OnEventDidTrigger(RuleRollD20 evt) {
        }

        public bool AgainstAlignment;
        public AlignmentComponent Alignment;
        public bool SpecificDescriptor;
        public SpellDescriptorWrapper SpellDescriptor;
        public bool Replace;
        public int Roll;
    }
}
