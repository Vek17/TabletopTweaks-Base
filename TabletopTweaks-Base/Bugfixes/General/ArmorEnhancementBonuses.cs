using HarmonyLib;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers;
using Kingmaker.Items;
using System;

namespace TabletopTweaks.Base.Bugfixes.General {
    class ArmorEnhancementBonuses {
        [HarmonyPatch(typeof(GameHelper), nameof(GameHelper.GetItemEnhancementBonus), new Type[] { typeof(ItemEntity) })]
        class GameHelper_GetItemEnhancementBonus_Patch {
            static void Postfix(ref int __result, ItemEntity item) {
                ItemEntityWeapon itemEntityweapon = item as ItemEntityWeapon;
                if (itemEntityweapon != null) {
                    __result = GameHelper.GetItemEnhancementBonus(itemEntityweapon);
                    return;
                }
                ItemEntityArmor itemEntityArmor = null;
                ItemEntityShield itemEntityShield = item as ItemEntityShield;
                if (itemEntityShield != null) {
                    itemEntityArmor = itemEntityShield.ArmorComponent;
                }
                itemEntityArmor ??= item as ItemEntityArmor;
                if (itemEntityArmor == null) {
                    __result = 0;
                    return;
                }
                int enchantBonus = 0;
                foreach (ItemEnchantment itemEnchantment in itemEntityArmor.Enchantments) {
                    ArmorEnhancementBonus component = itemEnchantment.GetComponent<ArmorEnhancementBonus>();
                    if (component != null) {
                        int enhancementValue = component.EnhancementValue;
                        if (enhancementValue < enchantBonus) {
                            continue;
                        }
                        enchantBonus = enhancementValue;
                    }
                }
                __result = enchantBonus;
            }
        }
    }
}
