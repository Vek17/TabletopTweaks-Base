using Kingmaker;
using Kingmaker.Blueprints.Items;
using Kingmaker.Designers;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
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

        public void AddBlackBlade(BlueprintItem ItemToGive) {
            var valid = Game.Instance.Player.PartyCharacters.AsEnumerable().Append(Game.Instance.Player.MainCharacter.Value).Select(u => u.Value).ToList();
            Main.Log($"Owner: {Owner.Unit.UniqueId}:{Owner.CharacterName} - {valid.Contains(Owner)}");
            if (!BlackBlade.IsEmpty || !valid.Contains(Owner)) {
                return; 
            }
            ItemsCollection inventory = GameHelper.GetPlayerCharacter().Inventory;
            ItemEntity itemEntity = ItemToGive.CreateEntity();
            itemEntity.Identify();
            BlackBlade = inventory.Add(itemEntity);
        }

        public bool IsBlackBlade(ItemEntity item) {
            return item == BlackBlade;
        }

        [JsonProperty]
        private EntityRef<ItemEntity> BlackBlade = new EntityRef<ItemEntity>();
    }
}
