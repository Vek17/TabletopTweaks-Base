using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.WeaponEnchantments {
    class NonStackingTempEnchantments {
        public static void AddWeaponEnhancements() {
            CreateNonStackingWeaponEnhancement(1);
            CreateNonStackingWeaponEnhancement(2);
            CreateNonStackingWeaponEnhancement(3);
            CreateNonStackingWeaponEnhancement(4);
            CreateNonStackingWeaponEnhancement(5);
            CreateNonStackingWeaponEnhancement(6);
        }

        public static void AddArmorEnhancements() {
            CreateNonStackingArmorEnhancement(1);
            CreateNonStackingArmorEnhancement(2);
            CreateNonStackingArmorEnhancement(3);
            CreateNonStackingArmorEnhancement(4);
            CreateNonStackingArmorEnhancement(5);
            CreateNonStackingArmorEnhancement(6);
        }

        private static BlueprintWeaponEnchantment CreateNonStackingWeaponEnhancement(int enhancmentBonus) {
            return Helpers.CreateBlueprint<BlueprintWeaponEnchantment>(TTTContext, $"TemporaryEnhancement{enhancmentBonus}NonStacking", bp => {
                bp.m_EnchantmentCost = enhancmentBonus;
                bp.SetName(TTTContext, $"Temporary Enhancement +{enhancmentBonus}");
                bp.SetDescription(TTTContext, $"Attacks with this weapon get +{enhancmentBonus} " +
                    $"enhancement bonus on both attack and damage rolls.");
                bp.SetPrefix(TTTContext, "");
                bp.SetSuffix(TTTContext, $"+{enhancmentBonus}");
                bp.AddComponent<WeaponEnhancementBonus>(c => {
                    c.EnhancementBonus = enhancmentBonus;
                });
            });
        }
        private static BlueprintArmorEnchantment CreateNonStackingArmorEnhancement(int enhancmentBonus) {
            return Helpers.CreateBlueprint<BlueprintArmorEnchantment>(TTTContext, $"TemporaryEnhancementArmor{enhancmentBonus}TTT", bp => {
                bp.m_EnchantmentCost = enhancmentBonus;
                bp.SetName(TTTContext, $"Temporary Enhancement +{enhancmentBonus}");
                bp.SetDescription(TTTContext, $"This armor provides a +{enhancmentBonus} " +
                    $"enhancement bonus to Armor Class. The armor check penalty of magic armor " +
                    $"is lessened by {enhancmentBonus} compared to ordinary armor of its type.");
                bp.SetPrefix(TTTContext, "");
                bp.SetSuffix(TTTContext, $"+{enhancmentBonus}");
                bp.AddComponent<AdvanceArmorStats>(c => {
                    c.ArmorCheckPenaltyShift = enhancmentBonus;
                });
                bp.AddComponent<ArmorEnhancementBonus>(c => {
                    c.EnhancementValue = enhancmentBonus;
                });
            });
        }
    }
}
