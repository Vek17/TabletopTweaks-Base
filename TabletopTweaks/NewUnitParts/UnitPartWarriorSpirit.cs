using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartWarriorSpirit : UnitPart {
        public void AddEntry(EntityFact source, int cost, params BlueprintWeaponEnchantmentReference[] enchants) {
            ClearSelectedEnchants(source);
            WarriorSpiritSelectedEntry item = new WarriorSpiritSelectedEntry(enchants, cost, source);
            this.SelectedEnchants.Add(item);
        }

        public void ActivateEnchant(ItemEntity item, BlueprintItemEnchantment enchantment, MechanicsContext parentContext, Rounds duration) {
            ActiveEnchants.Add(new WarriorSpiritActiveEntry() {
                Item = item,
                EnchantmentID = item.AddEnchantment(enchantment, parentContext, new Rounds?(duration)).UniqueId
            });
        }

        public WarriorSpiritSelectedEntry GetSelectedEnchant() {
            return SelectedEnchants.FirstOrDefault();
        }

        public bool HasSelectedEnchant() {
            return SelectedEnchants.Any();
        }

        public void ClearSelectedEnchants(EntityFact except) {
            SelectedEnchants
                .ToArray()
                .Where(entry => entry.Source != except)
                .ForEach(entry => RemoveEntry(entry.Source, true));
        }

        public void ClearActiveEnchants() {
            foreach (var Entry in ActiveEnchants) {
                try {
                    ItemEntity entity = Entry.Item.Entity;
                    EntityFact fact = (entity != null) ? entity.Facts.FindById(Entry.EnchantmentID) : null;
                    if (entity != null) {
                        entity.Facts.Remove(fact, true);
                    }
                } catch {
                    Main.Error("Failed to clear Warrior Spirit Enchants");
                }
            }
            ActiveEnchants.Clear();
        }

        public void RemoveEntry(EntityFact source, bool removeBuff = false) {
            if (removeBuff) {
                Owner.RemoveFact(source);
            }
            this.SelectedEnchants.RemoveAll((WarriorSpiritSelectedEntry entry) => entry.Source == source);
        }

        private void TryRemove() {
            if (!SelectedEnchants.Any()) { this.RemoveSelf(); }
        }

        private readonly List<WarriorSpiritSelectedEntry> SelectedEnchants = new List<WarriorSpiritSelectedEntry>();
        private readonly List<WarriorSpiritActiveEntry> ActiveEnchants = new List<WarriorSpiritActiveEntry>();

        public class WarriorSpiritActiveEntry {
            public string EnchantmentID;
            public EntityRef<ItemEntity> Item;
        }
        public class WarriorSpiritSelectedEntry {
            public WarriorSpiritSelectedEntry(BlueprintWeaponEnchantmentReference[] enchants, int cost, EntityFact source) {
                Enchants = enchants;
                Cost = cost;
                Source = source;
            }
            public readonly BlueprintWeaponEnchantmentReference[] Enchants;
            public readonly int Cost;
            public readonly EntityFact Source;
        }
    }
}
