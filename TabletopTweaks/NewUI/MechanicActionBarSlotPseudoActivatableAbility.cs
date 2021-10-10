using Kingmaker.Blueprints;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewUI {
    public class MechanicActionBarSlotPseudoActivatableAbility : MechanicActionBarSlotSpontaneusConvertedSpell {

        public BlueprintBuffReference BuffToWatch;

        public override bool IsActive() => Unit.Descriptor.HasFact(BuffToWatch);
    }
}
