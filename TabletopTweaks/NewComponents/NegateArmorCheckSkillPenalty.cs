using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using System;

namespace TabletopTweaks.NewComponents {
    [TypeId("4cc6ea51fc1b40dd965e44aa645751ff")]
    class NegateArmorCheckSkillPenalty: UnitFactComponentDelegate,
        IUnitActiveEquipmentSetHandler,
        IGlobalSubscriber,
        ISubscriber,
        IUnitEquipmentHandler,
        IUnitBuffHandler {
        public override void OnTurnOn() {
            ApplyStatBonus();
        }

        public override void OnTurnOff() {
            base.Owner.Stats.GetStat(Stat).RemoveModifiersFrom(base.Runtime);
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            ApplyStatBonus();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != Owner) {
                return;
            }
            ApplyStatBonus();
        }

        public void HandleBuffDidAdded(Buff buff) {
            ApplyStatBonus();
        }

        public void HandleBuffDidRemoved(Buff buff) {
            ApplyStatBonus();
        }

        private void ApplyStatBonus() {
            ModifiableValue stat = base.Owner.Stats.GetStat(Stat);
            stat.RemoveModifiersFrom(base.Runtime);
            int num = stat.GetDescriptorBonus(ModifierDescriptor.Armor)
                + stat.GetDescriptorBonus(ModifierDescriptor.ArmorFocus)
                + stat.GetDescriptorBonus(ModifierDescriptor.Shield)
                + stat.GetDescriptorBonus(ModifierDescriptor.ShieldFocus);
            if (stat != null) {
                stat.AddModifierUnique(Math.Max(-num, 0), base.Runtime, Descriptor);
            }
        }

        public ModifierDescriptor Descriptor;
        public StatType Stat;
    }
}
