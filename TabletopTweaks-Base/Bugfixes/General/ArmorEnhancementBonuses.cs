using HarmonyLib;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers;
using Kingmaker.Items;
using System.Linq;
using System;

namespace TabletopTweaks.Base.Bugfixes.General {
    class ArmorEnhancementBonuses {
        [HarmonyPatch(typeof(GameHelper), nameof(GameHelper.GetItemEnhancementBonus), new Type[]{ typeof(ItemEntity) })]
        class GameHelper_GetItemEnhancementBonus_Patch {
            static void Postfix(ref int __result, ItemEntity item) {
                ItemEntityWeapon weapon;
                if ((weapon = (item as ItemEntityWeapon)) != null) {
                    __result = GameHelper.GetItemEnhancementBonus(weapon);
                    return;
                }
                ItemEntityArmor itemEntityArmor;
                if ((itemEntityArmor = (item as ItemEntityArmor)) == null) {
                    ItemEntityShield itemEntityShield = item as ItemEntityShield;
                    itemEntityArmor = ((itemEntityShield != null) ? itemEntityShield.ArmorComponent : null);
                }
                ItemEntityArmor itemEntityArmor2 = itemEntityArmor;
                if (itemEntityArmor2 == null) {
                    __result = 0;
                    return;
                }
                int? num = null;
                int? num2;
                foreach (ItemEnchantment itemEnchantment in itemEntityArmor2.Enchantments.Where(e => e.IsActive)) {
                    ArmorEnhancementBonus component = itemEnchantment.GetComponent<ArmorEnhancementBonus>();
                    if (component != null) {
                        if (num != null) {
                            int enhancementValue = component.EnhancementValue;
                            num2 = num;
                            if (!(enhancementValue > num2.GetValueOrDefault() & num2 != null)) {
                                continue;
                            }
                        }
                        num = new int?(component.EnhancementValue);
                    }
                }
                num2 = num;
                if (num2 == null) {
                    __result = 0;
                    return;
                }
                __result = num2.GetValueOrDefault();
            }
        }
    }
}
