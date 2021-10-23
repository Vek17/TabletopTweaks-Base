using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace TabletopTweaks.NewComponents {
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("708754fe955c4c83922dddf4e89c86a7")]
    class ArmorFeatureUnlock : UnitFactComponentDelegate<ArmorFeatureUnlock.ArmorFeatureUnlockData>,
        IUnitActiveEquipmentSetHandler,
        IUnitLevelUpHandler,
        IGlobalSubscriber, ISubscriber,
        IUnitEquipmentHandler {
        public override void OnTurnOn() {
            base.OnTurnOn();
            Update();
        }

        public override void OnTurnOff() {
            base.OnTurnOff();
            RemoveFact();
        }

        public override void OnActivate() {
            Update();
        }

        public override void OnDeactivate() {
            RemoveFact();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            Update();
        }
        /*
        private void CheckEligibility() {
            RemoveFact();
            if (Owner.Body.IsPolymorphed) { return; }
            var Armor = Owner.Body?.Armor?.MaybeArmor;
            var Shield = Owner.Body?.SecondaryHand?.MaybeShield?.ArmorComponent;
            if (Armor != null
                && (RequiredArmor.Contains(Armor.Blueprint.ProficiencyGroup) == !Invert)) {
                AddFact();
                return;
            };
            if (Shield != null
                && (RequiredArmor.Contains(Shield.Blueprint.ProficiencyGroup) == !Invert)) {
                AddFact();
                return;
            };
        }
        */
        private bool ShouldApply() {
            if (Owner.Body.IsPolymorphed) { return false; }
            var Armor = Owner.Body?.Armor?.MaybeArmor;
            var Shield = Owner.Body?.SecondaryHand?.MaybeShield?.ArmorComponent;
            return (Armor != null && (RequiredArmor.Contains(Armor.Blueprint.ProficiencyGroup) == !Invert))
                || (Shield != null && (RequiredArmor.Contains(Shield.Blueprint.ProficiencyGroup) == !Invert));
        }

        private void AddFact() {
            if (Data.AppliedFact == null) {
                Data.AppliedFact = Owner.AddFact(NewFact, null, null);
            }
        }

        private void RemoveFact() {
            if (Data.AppliedFact != null) {
                Owner.RemoveFact(Data.AppliedFact);
                Data.AppliedFact = null;
            }
        }

        private void Update() {
            if (ShouldApply()) {
                AddFact();
            } else {
                RemoveFact();
            }
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != Owner) {
                return;
            }
            if (!slot.Active) {
                return;
            }
            Update();
        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit) {
        }

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller) {
            Update();
        }

        [SerializeField]
        [FormerlySerializedAs("NewFact")]
        public BlueprintUnitFactReference NewFact;
        public ArmorProficiencyGroup[] RequiredArmor = new ArmorProficiencyGroup[0];
        public bool Invert = false;

        [TypeId("ba0c9c406dd448d89e8ec941f8c5ff56")]
        public class ArmorFeatureUnlockData {
            [JsonProperty]
            public EntityFact AppliedFact;
        }
    }
}
