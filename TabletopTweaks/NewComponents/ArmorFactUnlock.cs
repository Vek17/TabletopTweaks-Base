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
    class ArmorFactUnlock: UnitFactComponentDelegate<ArmorFactUnlock.ArmorFeatureUnlockData>,
        IUnitActiveEquipmentSetHandler,
        IUnitLevelUpHandler,
        IGlobalSubscriber, ISubscriber,
        IUnitEquipmentHandler {
        public override void OnTurnOn() {
            CheckEligibility();
        }

        public override void OnTurnOff() {
            RemoveFact();
        }

        public override void OnActivate() {
            CheckEligibility();
        }

        public override void OnDeactivate() {
            RemoveFact();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) {
            CheckEligibility();
        }

        private void CheckEligibility() {
            RemoveFact();
            if (Owner.Body.IsPolymorphed) { return; }
            var Armor = Owner.Body?.Armor?.MaybeArmor;
            var Shield = Owner.Body?.SecondaryHand?.MaybeShield?.ArmorComponent;
            if (Armor != null
                && RequiredArmor.Contains(Armor.Blueprint.ProficiencyGroup)
                && !ForbiddenArmor.Contains(Armor.Blueprint.ProficiencyGroup)) {
                AddFact();
                return;
            };
            if (Shield != null
                && RequiredArmor.Contains(Shield.Blueprint.ProficiencyGroup)
                && !ForbiddenArmor.Contains(Shield.Blueprint.ProficiencyGroup)) {
                AddFact();
                return;
            };
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

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != Owner) {
                return;
            }
            if (!slot.Active) {
                return;
            }
            CheckEligibility();
        }

        public void HandleUnitBeforeLevelUp(UnitEntityData unit) {
        }

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller) {
            CheckEligibility();
        }

        [SerializeField]
        [FormerlySerializedAs("NewFact")]
        public BlueprintUnitFactReference NewFact;
        public ArmorProficiencyGroup[] RequiredArmor = new ArmorProficiencyGroup[0];
        public ArmorProficiencyGroup[] ForbiddenArmor = new ArmorProficiencyGroup[0];

        public class ArmorFeatureUnlockData {
            [JsonProperty]
            public EntityFact AppliedFact;
        }
    }
}
