using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Validation;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.NewEvents;

namespace TabletopTweaks.NewComponents {
    [AllowedOn(typeof(BlueprintAbility), false)]
    [TypeId("a9892b3a72f349fe8acb2e7565d18f93")]
    class NestedPsudoActivatableAbilities : UnitFactComponentDelegate, ISpontaneousConversionHandler, IUnitEquipmentHandler {
        public ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference> Variants {
            get {
                return this.m_Variants;
            }
        }

        public override void ApplyValidation(ValidationContext context) {
            base.ApplyValidation(context);
            if (this.Variants.Length < 2) {
                context.AddError("Variants count less than 2", Array.Empty<object>());
            }
        }

        public void HandleGetConversions(AbilityData ability, ref IEnumerable<AbilityData> conversions) {
            if (ability.Blueprint != OwnerBlueprint) { return; }
            var conversionList = conversions.ToList();
            foreach (BlueprintAbility replaceBlueprint in Variants) {
                AbilityData abilityData = new AbilityData(ability, replaceBlueprint);
                AbilityData.AddAbilityUnique(ref conversionList, abilityData);
            }
            conversions = conversionList;
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem) {
            if (slot.Owner != Owner) { return; }
            Game.Instance?.RootUiContext?.InGameVM?.StaticPartVM?.ActionBarVM?.OnUnitChanged(Owner);
        }

        public BlueprintAbilityReference[] m_Variants;
    }
}
