using Kingmaker.Blueprints.Items.Ecnchantments;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.WeaponEnchantments {
    class TwoHandedDamageMultiplier {
        public static void AddTwoHandedDamageMultiplierEnchantment() {
            var TwoHandedDamageMultiplierEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"TwoHandedDamageMultiplierEnchantment", bp => {
                bp.m_Description = Helpers.CreateString($"{bp.name}.description", $"Attacks are made with a 1.5 damage multipler.");
                bp.m_EnchantName = Helpers.CreateString($"{bp.name}.name", $"Increased Damage Multiplier");
                bp.m_Prefix = Helpers.CreateString($"{bp.name}.prefix", $"");
                bp.m_Suffix = Helpers.CreateString($"{bp.name}.suffix", $"");
                bp.m_EnchantmentCost = 1;
                bp.AddComponent<WeaponDamageMultiplierReplacement>(c => {
                    c.Multiplier = 1.5f;
                });
            });
        }
    }
}
