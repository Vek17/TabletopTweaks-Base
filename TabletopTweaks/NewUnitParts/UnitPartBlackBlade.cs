using Kingmaker;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.NewUnitParts {
    class UnitPartBlackBlade : OldStyleUnitPart {
        public UnitPartBlackBlade() {
        }

        public void AddBlackBlade(BlueprintItem ItemToGive, MechanicsContext context, EntityFact fact) {
            TryAddToInventory(ItemToGive);
            UpdateEnhancementBonus(context, fact);
        }

        public bool TryAddToInventory(BlueprintItem ItemToGive) {
            var valid = Game.Instance.Player.PartyCharacters.AsEnumerable().Append(Game.Instance.Player.MainCharacter.Value).Select(u => u.Value).ToList();
            Main.Log($"Owner: {Owner.Unit.UniqueId}:{Owner.CharacterName} - {valid.Contains(Owner)}");
            if (!BlackBlade.IsEmpty || !valid.Contains(Owner)) {
                return false;
            }
            ItemsCollection inventory = GameHelper.GetPlayerCharacter().Inventory;
            ItemEntity itemEntity = ItemToGive.CreateEntity();
            itemEntity.Identify();
            BlackBlade = inventory.Add(itemEntity);
            return true;
        }

        public bool IsBlackBlade(ItemEntity item) {
            return item == BlackBlade;
        }

        private void ClearEnchantment(ref string enchant) {
            try {
                ItemEntity entity = BlackBlade.Entity;
                EntityFact enchantFact = (entity != null) ? entity.Facts.FindById(enchant) : null;
                if (enchantFact != null) {
                    entity.Facts.Remove(enchantFact, true);
                }
                enchant = null;
            } catch {
                Main.Error("Failed to clear Black Blade LifeDrinker");
            }
        }

        private void UpdateEnhancementBonus(MechanicsContext parentContext, EntityFact fact) {
            if (BlackBlade.IsEmpty) { return; }
            ClearEnchantment(ref EnhancementBonus);
            var enhancement = GetEnhancement();
            if (enhancement) {
                EnhancementBonus = BlackBlade.Entity.AddEnchantment(enhancement, parentContext).UniqueId;
            }
        }

        public ItemEnchantment ApplyEnchantment(BlueprintItemEnchantment enchantment, MechanicsContext parentContext) {
            return BlackBlade.Entity.AddEnchantment(enchantment, parentContext);
        }

        public void RemoveEnchantment(string enchantID) {
            ClearEnchantment(ref enchantID);
        }

        private BlueprintWeaponEnchantment GetEnhancement() {
            var bonus = ProgressionProperty.GetInt(Owner) switch {
                >= 3 and <= 4 => Enhancements[0],
                >= 5 and <= 8 => Enhancements[1],
                >= 9 and <= 12 => Enhancements[2],
                >= 13 and <= 16 => Enhancements[3],
                >= 17 and <= 20 => Enhancements[4],
                _ => null
            };
            return bonus;
        }

        [JsonProperty]
        private EntityRef<ItemEntity> BlackBlade = new EntityRef<ItemEntity>();
        [JsonProperty]
        private string EnhancementBonus;

        private static readonly BlueprintUnitProperty ProgressionProperty = Resources.GetModBlueprint<BlueprintUnitProperty>("BlackBladeProgressionProperty");
        private static readonly BlueprintWeaponEnchantment[] Enhancements = new BlueprintWeaponEnchantment[] {
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("d42fc23b92c640846ac137dc26e000d4"), //+1
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("eb2faccc4c9487d43b3575d7e77ff3f5"), //+2
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("80bb8a737579e35498177e1e3c75899b"), //+3
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("783d7d496da6ac44f9511011fc5f1979"), //+4
            Resources.GetBlueprint<BlueprintWeaponEnchantment>("bdba267e951851449af552aa9f9e3992")  //+5
        };
    }
}
