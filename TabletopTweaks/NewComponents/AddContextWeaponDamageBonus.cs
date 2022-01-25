using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;

namespace TabletopTweaks.NewComponents {
    [TypeId("fc8175648c844fbda84dc1cf7f0f670b")]
    public class AddContextWeaponDamageBonus : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
        IRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IInitiatorRulebookSubscriber {
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt) {
            evt.AddDamageModifier(Value.Calculate(base.Fact.MaybeContext), base.Fact, Descriptor);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {
        }

        public ModifierDescriptor Descriptor;
        public ContextValue Value = 0;
    }
}
