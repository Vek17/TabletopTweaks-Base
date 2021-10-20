using Kingmaker.Blueprints;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace TabletopTweaks.NewUI {
    public class MechanicActionBarSlotPseudoActivatableAbility : MechanicActionBarSlotSpontaneusConvertedSpell {

        public BlueprintBuffReference BuffToWatch;

        public override bool IsActive() { 
            return Unit.Descriptor.HasFact(BuffToWatch); 
        }

        public override int GetResource() {
            return -1;
        }

        public override Sprite GetIcon() {
            return base.GetForeIcon();
        }

        public override Sprite GetForeIcon() {
            return null;
        }
    }
}
