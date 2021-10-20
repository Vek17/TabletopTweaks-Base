using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Owlcat.Runtime.Core.Logging;
using System;
using TabletopTweaks.NewUnitParts;

namespace TabletopTweaks.NewActions {
    class ContextActionWarriorSpiritEnhance : ContextAction {
        private BlueprintItemEnchantmentReference[] m_DefaultEnchantments = new BlueprintItemEnchantmentReference[5];

        public override string GetCaption() {
            return string.Format("Add enchants from warrior spirit to caster's weapon 1 minute");
        }

        public override void RunAction() {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null) {
                LogChannel.Default.Error(this, "ContextActionWarriorSpiritEnhance: target is null", Array.Empty<object>());
                return;
            }
            UnitPartWarriorSpirit unitPartWarriorSpirit = maybeCaster.Ensure<UnitPartWarriorSpirit>();
            UnitPartWeaponTraining unitPartWeaponTraining = maybeCaster.Ensure<UnitPartWeaponTraining>();

            ItemEntityWeapon weapon = (!maybeCaster.Body.PrimaryHand.HasWeapon) ? maybeCaster.Body.EmptyHandWeapon : maybeCaster.Body.PrimaryHand.MaybeWeapon;
            if (weapon == null) {
                return;
            }

            int maxEnhancement = unitPartWeaponTraining.GetMaxWeaponRank();
            Rounds duration = DurationValue.Calculate(base.Context);
            unitPartWarriorSpirit.ClearActiveEnchants();
            if (unitPartWarriorSpirit.HasSelectedEnchant()) {
                var enchantmentData = unitPartWarriorSpirit.GetSelectedEnchant();
                maxEnhancement -= enchantmentData.Cost;
                foreach (var enchant in enchantmentData.Enchants) {
                    unitPartWarriorSpirit.ActivateEnchant(weapon, enchant, Context, duration);
                }
            }
            int itemEnhancementBonus = GameHelper.GetItemEnhancementBonus(weapon);
            int remainingEnhancement = Math.Min(Math.Max(0, 5 - itemEnhancementBonus), Math.Min(5, maxEnhancement));
            if (remainingEnhancement > 0) {
                BlueprintItemEnchantment enchantment = this.DefaultEnchantments[remainingEnhancement - 1];
                unitPartWarriorSpirit.ActivateEnchant(weapon, enchantment, Context, duration);
            }
        }

        public BlueprintItemEnchantmentReference[] DefaultEnchantments = new BlueprintItemEnchantmentReference[5];
        public ContextDurationValue DurationValue;
    }
}
