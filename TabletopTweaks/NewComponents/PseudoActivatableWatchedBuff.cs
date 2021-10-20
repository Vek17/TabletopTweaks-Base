using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintBuff))]
    [TypeId("1ab7dd64fc4b4018bbc16bb484dd81a4")]
    public class PseudoActivatableWatchedBuff :
        UnitFactComponentDelegate,
        ISubscriber,
        IUnitSubscriber,
        IUnitBuffHandler {

        public void HandleBuffDidAdded(Buff buff) {
            if (!buff.Blueprint.Equals(Fact.Blueprint) || buff.Owner.Unit != this.Owner)
                return;
            this.Owner.Ensure<UnitPartPseudoActivatableAbilities>().BuffActivated(buff.Blueprint);
        }

        public void HandleBuffDidRemoved(Buff buff) {
            if (!buff.Blueprint.Equals(Fact.Blueprint) || buff.Owner.Unit != this.Owner)
                return;
            this.Owner.Get<UnitPartPseudoActivatableAbilities>()?.BuffDeactivated(buff.Blueprint);
        }

    }
}
