using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewComponents {

    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("3bc123b6862c42d987955e4350a7fd94")]
    public class RecalculateOnEquipmentChange :
        UnitFactComponentDelegate,
        IUnitActiveEquipmentSetHandler,
        IUnitSubscriber,
        ISubscriber,
        IUnitEquipmentHandler {


        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if ((UnitEntityData)slot.Owner != this.Owner)
                return;
            this.Fact.Reapply();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            this.Fact.Reapply();
        }
    }
}
