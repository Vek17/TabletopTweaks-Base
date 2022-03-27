using Kingmaker.Blueprints.Items.Ecnchantments;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.WeaponEnchantments {
    class TwoHandedDamageMultiplier {
        public static void AddTwoHandedDamageMultiplierEnchantment() {
            var TwoHandedDamageMultiplierEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>(TTTContext, $"TwoHandedDamageMultiplierEnchantment", bp => {
                bp.SetName(TTTContext, "Increased Damage Multiplier");
                bp.SetDescription(TTTContext, "Attacks are made with a 1.5 damage multipler.");
                bp.SetPrefix(TTTContext, "");
                bp.SetSuffix(TTTContext, "");
                bp.m_EnchantmentCost = 1;
                bp.AddComponent<WeaponDamageMultiplierReplacement>(c => {
                    c.Multiplier = 1.5f;
                });
            });
        }
    }
}
