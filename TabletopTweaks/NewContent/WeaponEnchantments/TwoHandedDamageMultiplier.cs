using Kingmaker.Blueprints.Items.Ecnchantments;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.WeaponEnchantments {
    class TwoHandedDamageMultiplier {
        public static void AddTwoHandedDamageMultiplierEnchantment() {
            var TwoHandedDamageMultiplierEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"TwoHandedDamageMultiplierEnchantment", bp => {
                bp.SetName("4069dced9e064e7dbf1b88c1676a554e", "Increased Damage Multiplier");
                bp.SetDescription("2f046fabf52b4a39a09a2259c5f4a7c7", "Attacks are made with a 1.5 damage multipler.");
                bp.SetPrefix("ec67f572fd66409289bd6a5eb59eb007", "");
                bp.SetSuffix("dc8f4b0235e74be2ad6bbec41fc0f8d2", "");
                bp.m_EnchantmentCost = 1;
                bp.AddComponent<WeaponDamageMultiplierReplacement>(c => {
                    c.Multiplier = 1.5f;
                });
            });
        }
    }
}
