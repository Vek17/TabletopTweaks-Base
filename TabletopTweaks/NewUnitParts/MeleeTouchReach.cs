using Kingmaker.UnitLogic;
using System;

namespace TabletopTweaks.NewUnitParts {
    // Marked Obsolete on 2021-11-11
    [Obsolete("use UnitPartCustomStats instead", true)]
    public class MeleeTouchReach : UnitPart {
        public override void OnTurnOn() {
            base.RemoveSelf();
        }
    }
}