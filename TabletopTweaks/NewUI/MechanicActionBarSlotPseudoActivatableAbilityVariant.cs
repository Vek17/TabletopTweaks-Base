using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using UnityEngine;

namespace TabletopTweaks.NewUI {
    public class MechanicActionBarSlotPseudoActivatableAbilityVariant : 
        MechanicActionBarSlotSpontaneusConvertedSpell,
        IPseudoActivatableMechanicsBarSlot {

        public BlueprintBuffReference BuffToWatch { get; set; }
        public bool ShouldBeActive { get; set; }
        public AbilityData PseudoActivatableAbility => this.Spell;

        public override bool IsActive() => ShouldBeActive;

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
