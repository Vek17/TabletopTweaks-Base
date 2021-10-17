using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System.Linq;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintAbility), false)]
    [TypeId("60689f3632da4205b776a2c4a02485a7")]
    class UpdateSlotsOnEquipmentChange : UnitFactComponentDelegate, IUnitEquipmentHandler {
        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != Owner) { return; }
            var slots = Game.Instance?.RootUiContext?.InGameVM?.StaticPartVM?.ActionBarVM?.Slots;
            if (slots == null) { return; }
            slots
                .Where(slot => {
                    switch (slot.MechanicActionBarSlot) {
                        case MechanicActionBarSlotAbility abilitySlot:
                            return abilitySlot.Ability.Blueprint == OwnerBlueprint;
                        case MechanicActionBarSlotSpell spellSlot:
                            return spellSlot.Spell.Blueprint == OwnerBlueprint;
                        default:
                            return false;
                    }
                })
                .ForEach(slot => slot.SetMechanicSlot(slot.MechanicActionBarSlot));
        }
    }
}
