using Kingmaker.Blueprints;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Newtonsoft.Json;
using UnityEngine;

namespace TabletopTweaks.NewUI {
    public class MechanicActionBarSlotPseudoActivatableAbility : 
        MechanicActionBarSlotAbility,
        IPseudoActivatableMechanicsBarSlot {

        private BlueprintBuffReference m_BuffToWatch;

        public BlueprintBuffReference BuffToWatch {
            get => m_BuffToWatch;
            set => m_BuffToWatch = value; 
        }
        public bool ShouldBeActive { get; set; }
        public AbilityData PseudoActivatableAbility => this.Ability;

        public override bool IsActive() => ShouldBeActive;

        public Sprite ForeIconOverride { get; set; }
        public override Sprite GetForeIcon() => ForeIconOverride;
        public bool ShouldUpdateForeIcon { get; set; }
    }
}
