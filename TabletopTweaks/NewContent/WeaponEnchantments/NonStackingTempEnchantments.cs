using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.WeaponEnchantments {
    class NonStackingTempEnchantments {
        public static void AddWeaponEnhancements() {
            CreateNonStackingEnhancement(1);
            CreateNonStackingEnhancement(2);
            CreateNonStackingEnhancement(3);
            CreateNonStackingEnhancement(4);
            CreateNonStackingEnhancement(5);
            CreateNonStackingEnhancement(6);
        }

        private static BlueprintWeaponEnchantment CreateNonStackingEnhancement(int enhancmentBonus) {
            return Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"TemporaryEnhancement{enhancmentBonus}NonStacking", bp => {
                bp.m_EnchantmentCost = enhancmentBonus;
                bp.SetName($"Temporary Enhancement +{enhancmentBonus}");
                bp.SetDescription($"Attacks with this weapon get +{enhancmentBonus} " +
                    $"enhancement bonus on both attack and damage rolls.");
                bp.SetPrefix("");
                bp.SetSuffix($"+{enhancmentBonus}");
                bp.AddComponent<WeaponEnhancementBonus>(c => {
                    c.EnhancementBonus = enhancmentBonus;
                });
            });
        }
    }
}
