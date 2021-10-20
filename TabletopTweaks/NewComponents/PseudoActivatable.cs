using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewComponents {

    [AllowedOn(typeof(BlueprintAbility))]
    [TypeId("9b6e1ed4932a4932827c7fecb2c57427")]
    public class PseudoActivatable : 
        UnitFactComponentDelegate,
        ISubscriber,
        IUnitSubscriber,
        IUnitBuffHandler {
        public BlueprintBuffReference m_BuffToWatch;

        public BlueprintBuffReference BuffToWatch => m_BuffToWatch ?? BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);

        public void HandleBuffDidAdded(Buff buff) {
            if (!buff.Blueprint.ToReference<BlueprintBuffReference>().Equals(BuffToWatch) || buff.Owner != this.Owner)
                return;
            this.Owner.Ensure<UnitPartPseudoActivatableAbilities>().BuffActivated(buff.Blueprint);
        }

        public void HandleBuffDidRemoved(Buff buff) {
            if (!buff.Blueprint.ToReference<BlueprintBuffReference>().Equals(BuffToWatch) || buff.Owner != this.Owner)
                return;
            this.Owner.Get<UnitPartPseudoActivatableAbilities>()?.BuffDeactivated(buff.Blueprint);
        }
    }
}
