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
                bp.SetName(Helpers.DeriveId("e74c34eebd664e8b81d261df1a9de4e2", enhancmentBonus), $"Temporary Enhancement +{enhancmentBonus}");
                bp.SetDescription(Helpers.DeriveId("87c767465a324c9cad24d56cd425974e", enhancmentBonus), $"{{g|Encyclopedia:Attack}}Attacks{{/g}} with this weapon get +{enhancmentBonus} " +
                    $"enhancement {{g|Encyclopedia:Bonus}}bonus{{/g}} on both attack and {{g|Encyclopedia:Damage}}damage rolls.");
                bp.SetPrefix(Helpers.DeriveId("8de6ba29d79a4326a28136f6e0291f7e", enhancmentBonus), "");
                bp.SetSuffix(Helpers.DeriveId("2bee04557fb74f3ea481449e7806e421", enhancmentBonus), $"+{enhancmentBonus}");
                bp.AddComponent<WeaponEnhancementBonus>(c => {
                    c.EnhancementBonus = enhancmentBonus;
                });
            });
        }
    }
}
