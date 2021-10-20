using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.NewComponents;
using UnityEngine;

namespace TabletopTweaks.NewUI {
    public class MechanicActionBarSlotPseudoActivatableAbility : 
        MechanicActionBarSlotAbility,
        IPseudoActivatableMechanicsBarSlot {

        public BlueprintBuffReference BuffToWatch { get; set; } = BlueprintReferenceBase.CreateTyped<BlueprintBuffReference>(null);
        public bool ShouldBeActive { get; set; }
        public AbilityData PseudoActivatableAbility => Ability;
        public List<BlueprintBuffReference> VariantBuffsToWatch = new List<BlueprintBuffReference>();
        public HashSet<BlueprintBuff> ActiveVariantBuffs = new HashSet<BlueprintBuff>();

        public override int GetResource() {
            return -1;
        }


        public override bool IsActive() => ShouldBeActive;

        /*public override Sprite GetForeIcon() {
            var variants = Ability.Blueprint.GetComponent<AbilityVariants>();
            if (variants == null) {
                return base.GetForeIcon();
            }

            int numActiveVariants = 0;
            BlueprintAbility activeVariant = null;
            foreach (var variant in variants.Variants) {
                var pseudoActivatableComponent = variant.GetComponent<PseudoActivatable>();
                if (pseudoActivatableComponent != null && Unit.Descriptor.HasFact(pseudoActivatableComponent.BuffToWatch)) {
                    activeVariant = variant;
                    numActiveVariants++;
                }
            }
            if (numActiveVariants == 1) {
                return activeVariant.Icon;
            }
            return base.GetForeIcon();
        }*/
    }
}
