using Kingmaker.Blueprints;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using UnityEngine;

namespace TabletopTweaks.NewUI {
    public class MechanicActionBarSlotPseudoActivatableAbility : 
        MechanicActionBarSlotAbility,
        IPseudoActivatableMechanicsBarSlot {


        public BlueprintBuffReference BuffToWatch { get; set; }
        public bool ShouldBeActive { get; set; }

        public AbilityData PseudoActivatableAbility => this.Ability;

        public override bool IsActive() => ShouldBeActive;
    }
}
