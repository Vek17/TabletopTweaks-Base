using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Linq;

namespace TabletopTweaks.NewComponents.AbilitySpecific {
    [TypeId("e9cfd3c8b2ee49caada37795d9925af3")]
    class AberrantArcanaExtendComponent : UnitFactComponentDelegate, IUnitBuffHandler, IGlobalSubscriber, ISubscriber {

        public void HandleBuffDidAdded(Buff buff) {
            MechanicsContext maybeContext = buff.MaybeContext;
            if (((maybeContext != null) ? maybeContext.MaybeCaster : null) == Owner &&
                buff.Blueprint.GetComponents<SpellDescriptorComponent>().Any(c => c.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph)) &&
                !buff.MaybeContext.HasMetamagic(Metamagic.Extend)) {
                buff.SetDuration(new TimeSpan((long)(buff.TimeLeft.Ticks * 1.5)));
            }
        }

        public void HandleBuffDidRemoved(Buff buff) {
        }
    }
}