using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    class AberrantArcanaExtendComponent : UnitFactComponentDelegate, IUnitBuffHandler, IGlobalSubscriber, ISubscriber {

		// Token: 0x0600A132 RID: 41266 RVA: 0x002767A0 File Offset: 0x002749A0
		public void HandleBuffDidAdded(Buff buff) {
			MechanicsContext maybeContext = buff.MaybeContext;
			if (((maybeContext != null) ? maybeContext.MaybeCaster : null) == Owner &&
				buff.Blueprint.GetComponents<SpellDescriptorComponent>().Any(c => c.Descriptor.HasAnyFlag(SpellDescriptor.Polymorph)) &&
				!buff.MaybeContext.HasMetamagic(Metamagic.Extend)) {
				buff.SetDuration(new TimeSpan((long)(buff.TimeLeft.Ticks * 1.5)));
			}
		}

		// Token: 0x0600A133 RID: 41267 RVA: 0x000036D8 File Offset: 0x000018D8
		public void HandleBuffDidRemoved(Buff buff) {
		}
	}
}