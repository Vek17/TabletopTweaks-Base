using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
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
    [TypeId("891df14e8a9b4f72a28dd2b7a8c2de08")]
    class ContextActionApplyWeaponEnchant : ContextAction {
        public override string GetCaption() {
            return string.Format("Add selected enchantments");
        }

        public override void RunAction() {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null) {
                LogChannel.Default.Error(this, "ContextActionApplyWeaponEnchant: caster is null", Array.Empty<object>());
                return;
            }
            ItemEntityWeapon weapon = (!maybeCaster.Body.PrimaryHand.HasWeapon) ? maybeCaster.Body.EmptyHandWeapon : maybeCaster.Body.PrimaryHand.MaybeWeapon;
            if (weapon == null) {
                return;
            }
            Rounds duration = DurationValue.Calculate(base.Context);
            foreach (var enchantment in Enchantments) {
                weapon.AddEnchantment(enchantment, base.Context, new Rounds?(duration));
            }
        }

        public BlueprintItemEnchantmentReference[] Enchantments = new BlueprintItemEnchantmentReference[5];
        public ContextDurationValue DurationValue;
    }
}
