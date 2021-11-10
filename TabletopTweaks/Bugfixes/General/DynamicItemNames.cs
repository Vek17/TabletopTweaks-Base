using HarmonyLib;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Items;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.General {
    static class DynamicItemNames {
        [HarmonyPatch(typeof(ItemEntity), nameof(ItemEntity.Name), MethodType.Getter)]
        static class ItemEntity_Names_Patch {
            static bool Prefix(ItemEntity __instance, ref string __result) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("DynamicItemNaming")) { return true; }
                if (!__instance.IsIdentified) { return true; }
                string UniqueName = __instance.Blueprint.m_DisplayNameText;
                string DefaultName = "";
                switch (__instance.Blueprint) {
                    case BlueprintItemWeapon blueprint:
                        DefaultName = blueprint.Type.DefaultName;
                        break;
                    case BlueprintItemArmor blueprint:
                        DefaultName = blueprint.Type.DefaultName;
                        break;
                    default:
                        return true;
                }
                string name = UniqueName.IsNullOrEmpty() ?
                            __instance.GetEnchantmentPrefixes() + DefaultName + __instance.GetEnchantmentSuffixes() :
                            __instance.GetCustomEnchantmentPrefixes() + UniqueName + __instance.GetCustomEnchantmentSuffixes();
                if (!name.IsNullOrEmpty()) {
                    __result = name;
                }
                return false;
            }
        }
        private static string GetEnchantmentPrefixes(this IEnumerable<ItemEnchantment> enchants) {
            var includeTemporary = ModSettings.Fixes.BaseFixes.IsEnabled("DynamicItemNamingTemporary");
            if (enchants == null || enchants.Empty()) {
                return "";
            }
            string text = "";
            foreach (BlueprintItemEnchantment blueprintEnchantment in enchants.Where(e => includeTemporary ? true : !e.IsTemporary).Select(e => e.Blueprint)) {
                if (!blueprintEnchantment.Prefix.IsNullOrEmpty()) {
                    text += blueprintEnchantment.Prefix + " ";
                }
            }
            return text;
        }
        private static string GetEnchantmentPrefixes(this ItemEntity item) {
            return item.Enchantments
                .GetEnchantmentPrefixes();
        }
        private static string GetCustomEnchantmentPrefixes(this ItemEntity item) {
            return item.Enchantments
                .Where(e => !item.Blueprint.Enchantments.Contains(e.Blueprint))
                .GetEnchantmentPrefixes();
        }
        private static string GetEnchantmentSuffixes(this IEnumerable<ItemEnchantment> enchants) {
            var includeTemporary = ModSettings.Fixes.BaseFixes.IsEnabled("DynamicItemNamingTemporary");
            if (enchants == null || enchants.Empty()) {
                return "";
            }
            string text = "";
            foreach (BlueprintItemEnchantment blueprintEnchantment in enchants.Where(e => includeTemporary ? true : !e.IsTemporary).Select(e => e.Blueprint)) {
                if (!blueprintEnchantment.Suffix.IsNullOrEmpty()) {
                    text += " " + blueprintEnchantment.Suffix;
                }
            }
            return text;
        }
        private static string GetEnchantmentSuffixes(this ItemEntity item) {
            string text = item.Enchantments
                .Where(e => e.GetComponent<WeaponEnhancementBonus>() == null)
                .GetEnchantmentSuffixes();
            int totalEnhancment = item.GetItemEnhancementBonus();
            if (totalEnhancment > 0) {
                text += $" +{totalEnhancment}";
            }
            return text;
        }
        private static string GetCustomEnchantmentSuffixes(this ItemEntity item) {
            string text = item.Enchantments
                .Where(e => e.GetComponent<WeaponEnhancementBonus>() == null)
                .Where(e => !item.Blueprint.Enchantments.Contains(e.Blueprint))
                .GetEnchantmentSuffixes();
            int totalEnhancment = item.GetItemEnhancementBonus();
            int baseEnhancment = item.GetWeaponBlueprintEnhancementBonus();
            if (totalEnhancment > baseEnhancment) {
                text += $" +{totalEnhancment}";
            }
            return text;
        }
        private static int GetItemEnhancementBonus(this ItemEntity item) {
            ItemEntityWeapon weapon = item as ItemEntityWeapon;
            if (weapon != null) { return weapon.GetWeaponEnhancementBonus(); }
            int bonus = 0;
            return bonus;
        }
        private static int GetWeaponEnhancementBonus(this ItemEntityWeapon item) {
            return GameHelper.GetItemEnhancementBonus(item);
        }
        private static int GetWeaponBlueprintEnhancementBonus(this ItemEntity item) {
            ItemEntityWeapon weapon = item as ItemEntityWeapon;
            if (weapon != null) { return GameHelper.GetWeaponEnhancementBonus(weapon.Blueprint); }
            return 0;
        }

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.BaseFixes.IsDisabled("DynamicItemNaming")) { return; }
                Main.LogHeader("Patching Enchant Prefixes/Suffixes");

                PatchWeaponEnchants();
                PatchArmorEnchants();

                void PatchWeaponEnchants() {
                    WeaponEnchants.Agile.UpdatePrefixSuffix("Agile", "");
                    WeaponEnchants.Anarchic.UpdatePrefixSuffix("Anarchic", "");
                    WeaponEnchants.Axiomatic.UpdatePrefixSuffix("Axiomatic", "");
                    WeaponEnchants.BaneAberration.UpdatePrefixSuffix("Aberration bane", "");
                    WeaponEnchants.BaneAnimal.UpdatePrefixSuffix("Animal Bane", "");
                    WeaponEnchants.BaneConstruct.UpdatePrefixSuffix("Construct Bane", "");
                    WeaponEnchants.BaneDragon.UpdatePrefixSuffix("Dragon Bane", "");
                    WeaponEnchants.BaneEverything.UpdatePrefixSuffix("Bane", "");
                    WeaponEnchants.BaneFey.UpdatePrefixSuffix("Fey Bane", "");
                    WeaponEnchants.BaneHumanoidGiant.UpdatePrefixSuffix("Giant Bane", "");
                    WeaponEnchants.BaneHumanoidGiant2d6.UpdatePrefixSuffix("Giant Bane", "");
                    WeaponEnchants.BaneHumanoidReptilian.UpdatePrefixSuffix("Reptilian Humanoid Bane", "");
                    WeaponEnchants.BaneLiving.UpdatePrefixSuffix("Living Bane", "");
                    WeaponEnchants.BaneLycanthrope.UpdatePrefixSuffix("Lycanthrope Bane", "");
                    WeaponEnchants.BaneMagicalBeast.UpdatePrefixSuffix("Magical Beast Bane", "");
                    WeaponEnchants.BaneMonstrousHumanoid.UpdatePrefixSuffix("Monstrous Humanoid Bane", "");
                    WeaponEnchants.BaneOrcGoblin.UpdatePrefixSuffix("Orc Bane", "");
                    WeaponEnchants.BaneOrcGoblin1d6.UpdatePrefixSuffix("Goblin Bane", "");
                    WeaponEnchants.BaneOutsiderChaotic.UpdatePrefixSuffix("Chaotic Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderEvil.UpdatePrefixSuffix("Evil Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderGood.UpdatePrefixSuffix("Good Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderLawful.UpdatePrefixSuffix("Lawful Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderNeutral.UpdatePrefixSuffix("Neutral Outsider Bane", "");
                    WeaponEnchants.BanePlant.UpdatePrefixSuffix("Plant Bane", "");
                    WeaponEnchants.BaneUndead.UpdatePrefixSuffix("Undead Bane", "");
                    WeaponEnchants.BaneVermin.UpdatePrefixSuffix("Vermin Bane", "");
                    WeaponEnchants.BaneVermin1d8.UpdatePrefixSuffix("Vermin Bane", "");
                    WeaponEnchants.Bleed.UpdatePrefixSuffix("Bleed", "");
                    WeaponEnchants.Brass.UpdatePrefixSuffix("Flaming", "");
                    WeaponEnchants.Brass2d8.UpdatePrefixSuffix("Greater Flaming", "");
                    WeaponEnchants.BrilliantEnergy.UpdatePrefixSuffix("Brilliant Energy", "");
                    WeaponEnchants.Corrosive.UpdatePrefixSuffix("Corrosive", "");
                    WeaponEnchants.Corrosive2d6.UpdatePrefixSuffix("Caustic", "");
                    WeaponEnchants.CorrosiveBurst.UpdatePrefixSuffix("Burst", "");
                    WeaponEnchants.Deteriorative.UpdatePrefixSuffix("Deteriorative", "");
                    WeaponEnchants.DisruptingWeapon.UpdatePrefixSuffix("Disruption", "");
                    WeaponEnchants.Disruption.UpdatePrefixSuffix("Disruption", "");
                    WeaponEnchants.DragonEssenceBaneDragon.UpdatePrefixSuffix("Dragon Bane", "");
                    WeaponEnchants.ElderBrass.UpdatePrefixSuffix("Elder Flaming", "");
                    WeaponEnchants.ElderCorrosive.UpdatePrefixSuffix("Elder Greater Corrosive", "");
                    WeaponEnchants.ElderCorrosive2d6.UpdatePrefixSuffix("Elder Caustic", "");
                    WeaponEnchants.ElderCorrosiveBurst.UpdatePrefixSuffix("Burst", "");
                    WeaponEnchants.ElderFlaming.UpdatePrefixSuffix("Elder Flaming", "");
                    WeaponEnchants.ElderFlamingBurst.UpdatePrefixSuffix("Burst", "");
                    WeaponEnchants.ElderFrost.UpdatePrefixSuffix("Elder Frost", "");
                    WeaponEnchants.ElderIce2d6.UpdatePrefixSuffix("Elder Freezing", "");
                    WeaponEnchants.ElderIcyBurst.UpdatePrefixSuffix("Burst", "");
                    WeaponEnchants.ElderShock.UpdatePrefixSuffix("Elder Shocking", "");
                    WeaponEnchants.ElderShock2d6.UpdatePrefixSuffix("Elder Greater Shocking", "");
                    WeaponEnchants.ElderShockingBurst.UpdatePrefixSuffix("Burst", "");
                    WeaponEnchants.Enhancement1.UpdatePrefixSuffix("", "+1");
                    WeaponEnchants.Enhancement2.UpdatePrefixSuffix("", "+2");
                    WeaponEnchants.Enhancement3.UpdatePrefixSuffix("", "+3");
                    WeaponEnchants.Enhancement4.UpdatePrefixSuffix("", "+4");
                    WeaponEnchants.Enhancement5.UpdatePrefixSuffix("", "+5");
                    WeaponEnchants.Enhancement6.UpdatePrefixSuffix("", "+6");
                    WeaponEnchants.Flaming.UpdatePrefixSuffix("Flaming", "");
                    WeaponEnchants.FlamingBurst.UpdatePrefixSuffix("Burst", "");
                    WeaponEnchants.Frost.UpdatePrefixSuffix("Frost", "");
                    WeaponEnchants.Frost2d8.UpdatePrefixSuffix("Greater Frost", "");
                    WeaponEnchants.Furious.UpdatePrefixSuffix("Furious", "");
                    WeaponEnchants.Furyborn.UpdatePrefixSuffix("Furyborn", "");
                    WeaponEnchants.GhostTouch.UpdatePrefixSuffix("Ghost Touch", "");
                    WeaponEnchants.Heartseeker.UpdatePrefixSuffix("Heartseeker", "");
                    WeaponEnchants.Holy.UpdatePrefixSuffix("Holy", "");
                    WeaponEnchants.Ice2d6.UpdatePrefixSuffix("Freezing", "");
                    WeaponEnchants.IcyBurst.UpdatePrefixSuffix("Burst", "");
                    WeaponEnchants.Keen.UpdatePrefixSuffix("Keen", "");
                    WeaponEnchants.MagicWeapon.UpdatePrefixSuffix("Magic", "");
                    WeaponEnchants.Masterwork.UpdatePrefixSuffix("Masterwork", "");
                    WeaponEnchants.Necrotic.UpdatePrefixSuffix("Necrotic", "");
                    WeaponEnchants.Oversized.UpdatePrefixSuffix("Oversized", "");
                    WeaponEnchants.Radiant.UpdatePrefixSuffix("Radiant", "");
                    WeaponEnchants.Sacrificial.UpdatePrefixSuffix("Sacrificial", "");
                    WeaponEnchants.Shock.UpdatePrefixSuffix("Shocking", "");
                    WeaponEnchants.Shock2d6.UpdatePrefixSuffix("Shocking", "");
                    WeaponEnchants.ShockingBurst.UpdatePrefixSuffix("Burst", "");
                    WeaponEnchants.Speed.UpdatePrefixSuffix("Speed", "");
                    WeaponEnchants.TemporaryEnhancement1.UpdatePrefixSuffix("", "+1");
                    WeaponEnchants.TemporaryEnhancement2.UpdatePrefixSuffix("", "+2");
                    WeaponEnchants.TemporaryEnhancement3.UpdatePrefixSuffix("", "+3");
                    WeaponEnchants.TemporaryEnhancement4.UpdatePrefixSuffix("", "+4");
                    WeaponEnchants.TemporaryEnhancement5.UpdatePrefixSuffix("", "+5");
                    WeaponEnchants.Thundering.UpdatePrefixSuffix("Thundering", "");
                    WeaponEnchants.ThunderingBurst.UpdatePrefixSuffix("Burst", "");
                    WeaponEnchants.Ultrasound.UpdatePrefixSuffix("Ultrasound", "");
                    WeaponEnchants.Unholy.UpdatePrefixSuffix("Unholy", "");
                    WeaponEnchants.ViciousEnchantment.UpdatePrefixSuffix("Vicious", "");
                    WeaponEnchants.Vorpal.UpdatePrefixSuffix("Vorpal", "");
                }
                void PatchArmorEnchants() {
                    ArmorEnchants.AcidResistance10Enchant.UpdatePrefixSuffix("Acid Resistant", "");
                    ArmorEnchants.AcidResistance15Enchant.UpdatePrefixSuffix("Acid Resistant", "");
                    ArmorEnchants.AcidResistance20Enchant.UpdatePrefixSuffix("Acid Resistant", "");
                    ArmorEnchants.AcidResistance30Enchant.UpdatePrefixSuffix("Acid Resistant", "");
                    ArmorEnchants.AdamantineArmorHeavyEnchant.UpdatePrefixSuffix("Adamantine", "");
                    ArmorEnchants.AdamantineArmorLightEnchant.UpdatePrefixSuffix("Adamantine", "");
                    ArmorEnchants.AdamantineArmorMediumEnchant.UpdatePrefixSuffix("Adamantine", "");
                    ArmorEnchants.ArcaneArmorBalancedEnchant.UpdatePrefixSuffix("Balanced", "");
                    ArmorEnchants.ArcaneArmorInvulnerabilityEnchant.UpdatePrefixSuffix("Invulnerability", "");
                    ArmorEnchants.ArcaneArmorShadowEnchant.UpdatePrefixSuffix("Shadow", "");
                    ArmorEnchants.ArcaneArmorShadowGreaterEnchant.UpdatePrefixSuffix("Greater Shadow", "");
                    ArmorEnchants.ArmorEnhancementBonus1.UpdatePrefixSuffix("", "+1");
                    ArmorEnchants.ArmorEnhancementBonus2.UpdatePrefixSuffix("", "+2");
                    ArmorEnchants.ArmorEnhancementBonus3.UpdatePrefixSuffix("", "+3");
                    ArmorEnchants.ArmorEnhancementBonus4.UpdatePrefixSuffix("", "+4");
                    ArmorEnchants.ArmorEnhancementBonus5.UpdatePrefixSuffix("", "+5");
                    ArmorEnchants.ArmorEnhancementBonus6.UpdatePrefixSuffix("", "+6");
                    ArmorEnchants.ColdResistance10Enchant.UpdatePrefixSuffix("Cold Resistant", "");
                    ArmorEnchants.ColdResistance15Enchant.UpdatePrefixSuffix("Cold Resistant", "");
                    ArmorEnchants.ColdResistance20Enchant.UpdatePrefixSuffix("Cold Resistant", "");
                    ArmorEnchants.ColdResistance30Enchant.UpdatePrefixSuffix("Cold Resistant", "");
                    ArmorEnchants.ElectricityResistance10Enchant.UpdatePrefixSuffix("Shock Resistant", "");
                    ArmorEnchants.ElectricityResistance15Enchant.UpdatePrefixSuffix("Shock Resistant", "");
                    ArmorEnchants.ElectricityResistance20Enchant.UpdatePrefixSuffix("Shock Resistant", "");
                    ArmorEnchants.ElectricityResistance30Enchant.UpdatePrefixSuffix("Shock Resistant", "");
                    ArmorEnchants.EnergyResistance10n10Enchant.UpdatePrefixSuffix("Energy Resistant", "");
                    ArmorEnchants.FireResistance10Enchant.UpdatePrefixSuffix("Fire Resistant", "");
                    ArmorEnchants.FireResistance15Enchant.UpdatePrefixSuffix("Fire Resistant", "");
                    ArmorEnchants.FireResistance20Enchant.UpdatePrefixSuffix("Fire Resistant", "");
                    ArmorEnchants.FireResistance30Enchant.UpdatePrefixSuffix("Fire Resistant", "");
                    ArmorEnchants.Fortification25Enchant.UpdatePrefixSuffix("Light Fortification", "");
                    ArmorEnchants.Fortification50Enchant.UpdatePrefixSuffix("Fortification", "");
                    ArmorEnchants.Fortification75Enchant.UpdatePrefixSuffix("Heavy Fortification", "");
                    ArmorEnchants.GreaterShadow.UpdatePrefixSuffix("Greater Shadow", "");
                    ArmorEnchants.MithralArmorEnchant.UpdatePrefixSuffix("Mithral", "");
                    ArmorEnchants.NegativeEnergyImmunty15PerEnchantment.UpdatePrefixSuffix("Negative Energy Blocking", "");
                    ArmorEnchants.NegativeEnergyResistance10Enchant.UpdatePrefixSuffix("Negative Energy Resistant", "");
                    ArmorEnchants.NegativeEnergyResistance20Enchant.UpdatePrefixSuffix("Negative Energy Resistant", "");
                    ArmorEnchants.NegativeEnergyResistance30Enchant.UpdatePrefixSuffix("Negative Energy Resistant", "");
                    ArmorEnchants.PositiveEnergyResistance30Enchant.UpdatePrefixSuffix("Positive Energy Resistant", "");
                    ArmorEnchants.ShadowArmor.UpdatePrefixSuffix("Shadow", "");
                    ArmorEnchants.ShieldEnhancementBonus1.UpdatePrefixSuffix("", "+1");
                    ArmorEnchants.ShieldEnhancementBonus2.UpdatePrefixSuffix("", "+2");
                    ArmorEnchants.ShieldEnhancementBonus3.UpdatePrefixSuffix("", "+3");
                    ArmorEnchants.ShieldEnhancementBonus4.UpdatePrefixSuffix("", "+4");
                    ArmorEnchants.ShieldEnhancementBonus5.UpdatePrefixSuffix("", "+4");
                    ArmorEnchants.ShieldEnhancementBonus6.UpdatePrefixSuffix("", "+6");
                    ArmorEnchants.SonicResistance10Enchant.UpdatePrefixSuffix("Sonic Resistant", "");
                    ArmorEnchants.SonicResistance30Enchant.UpdatePrefixSuffix("Sonic Resistant", "");
                    ArmorEnchants.SpellResistance13Enchant.UpdatePrefixSuffix("Spell Resistant", "");
                    ArmorEnchants.SpellResistance15Enchant.UpdatePrefixSuffix("Spell Resistant", "");
                    ArmorEnchants.SpellResistance17Enchant.UpdatePrefixSuffix("Spell Resistant", "");
                    ArmorEnchants.SpellResistance19Enchant.UpdatePrefixSuffix("Spell Resistant", "");
                    ArmorEnchants.TemporaryArmorEnhancementBonus1.UpdatePrefixSuffix("", "+1");
                    ArmorEnchants.TemporaryArmorEnhancementBonus2.UpdatePrefixSuffix("", "+2");
                    ArmorEnchants.TemporaryArmorEnhancementBonus3.UpdatePrefixSuffix("", "+3");
                    ArmorEnchants.TemporaryArmorEnhancementBonus4.UpdatePrefixSuffix("", "+4");
                    ArmorEnchants.TemporaryArmorEnhancementBonus5.UpdatePrefixSuffix("", "+5");
                }
            }
        }

        private static class WeaponEnchants {
            public static BlueprintWeaponEnchantment Agile => Resources.GetBlueprint<BlueprintWeaponEnchantment>("a36ad92c51789b44fa8a1c5c116a1328");
            public static BlueprintWeaponEnchantment Anarchic => Resources.GetBlueprint<BlueprintWeaponEnchantment>("57315bc1e1f62a741be0efde688087e9");
            public static BlueprintWeaponEnchantment Axiomatic => Resources.GetBlueprint<BlueprintWeaponEnchantment>("0ca43051edefcad4b9b2240aa36dc8d4");
            public static BlueprintWeaponEnchantment BaneAberration => Resources.GetBlueprint<BlueprintWeaponEnchantment>("ee71cc8848219c24b8418a628cc3e2fa");
            public static BlueprintWeaponEnchantment BaneAnimal => Resources.GetBlueprint<BlueprintWeaponEnchantment>("78cf9fabe95d3934688ea898c154d904");
            public static BlueprintWeaponEnchantment BaneConstruct => Resources.GetBlueprint<BlueprintWeaponEnchantment>("73d30862f33cc754bb5a5f3240162ae6");
            public static BlueprintWeaponEnchantment BaneDragon => Resources.GetBlueprint<BlueprintWeaponEnchantment>("e5cb46a0a658b0a41854447bea32d2ee");
            public static BlueprintWeaponEnchantment BaneEverything => Resources.GetBlueprint<BlueprintWeaponEnchantment>("1a93ab9c46e48f3488178733be29342a");
            public static BlueprintWeaponEnchantment BaneFey => Resources.GetBlueprint<BlueprintWeaponEnchantment>("b6948040cdb601242884744a543050d4");
            public static BlueprintWeaponEnchantment BaneHumanoidGiant => Resources.GetBlueprint<BlueprintWeaponEnchantment>("dcecb5f2ffacfd44ead0ed4f8846445d");
            public static BlueprintWeaponEnchantment BaneHumanoidGiant2d6 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("1e25d1f515c867d40b9c0642e0b40ec2");
            public static BlueprintWeaponEnchantment BaneHumanoidReptilian => Resources.GetBlueprint<BlueprintWeaponEnchantment>("c4b9cce255d1d6641a6105a255934e2e");
            public static BlueprintWeaponEnchantment BaneLiving => Resources.GetBlueprint<BlueprintWeaponEnchantment>("e1d6f5e3cd3855b43a0cb42f6c747e1c");
            public static BlueprintWeaponEnchantment BaneLycanthrope => Resources.GetBlueprint<BlueprintWeaponEnchantment>("188efcfcd9938d44e9561c87794d17a8");
            public static BlueprintWeaponEnchantment BaneMagicalBeast => Resources.GetBlueprint<BlueprintWeaponEnchantment>("97d477424832c5144a9413c64d818659");
            public static BlueprintWeaponEnchantment BaneMonstrousHumanoid => Resources.GetBlueprint<BlueprintWeaponEnchantment>("c5f84a79ad154c84e8d2e9fe0dd49350");
            public static BlueprintWeaponEnchantment BaneOrcGoblin => Resources.GetBlueprint<BlueprintWeaponEnchantment>("0391d8eae25f39a48bcc6c2fc8bf4e12");
            public static BlueprintWeaponEnchantment BaneOrcGoblin1d6 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("ab0108b67cfc2a849926a79ece0fdddc");
            public static BlueprintWeaponEnchantment BaneOutsiderChaotic => Resources.GetBlueprint<BlueprintWeaponEnchantment>("234177d5807909f44b8c91ed3c9bf7ac");
            public static BlueprintWeaponEnchantment BaneOutsiderEvil => Resources.GetBlueprint<BlueprintWeaponEnchantment>("20ba9055c6ae1e44ca270c03feacc53b");
            public static BlueprintWeaponEnchantment BaneOutsiderGood => Resources.GetBlueprint<BlueprintWeaponEnchantment>("a876de94b916b7249a77d090cb9be4f3");
            public static BlueprintWeaponEnchantment BaneOutsiderLawful => Resources.GetBlueprint<BlueprintWeaponEnchantment>("3a6f564c8ea2d1941a45b19fa16e59f5");
            public static BlueprintWeaponEnchantment BaneOutsiderNeutral => Resources.GetBlueprint<BlueprintWeaponEnchantment>("4e30e79c500e5af4b86a205cc20436f2");
            public static BlueprintWeaponEnchantment BanePlant => Resources.GetBlueprint<BlueprintWeaponEnchantment>("0b761b6ed6375114d8d01525d44be5a9");
            public static BlueprintWeaponEnchantment BaneUndead => Resources.GetBlueprint<BlueprintWeaponEnchantment>("eebb4d3f20b8caa43af1fed8f2773328");
            public static BlueprintWeaponEnchantment BaneVermin => Resources.GetBlueprint<BlueprintWeaponEnchantment>("c3428441c00354c4fabe27629c6c64dd");
            public static BlueprintWeaponEnchantment BaneVermin1d8 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("e535fc007d0d7e74da45021d4607e607");
            public static BlueprintWeaponEnchantment Bleed => Resources.GetBlueprint<BlueprintWeaponEnchantment>("ac0108944bfaa7e48aa74f407e3944e3");
            public static BlueprintWeaponEnchantment Brass => Resources.GetBlueprint<BlueprintWeaponEnchantment>("5e0e5de297c229f42b00c5b1738b50fa");
            public static BlueprintWeaponEnchantment Brass2d8 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("a6064c14629de6d48bec8e8e0460f661");
            public static BlueprintWeaponEnchantment BrilliantEnergy => Resources.GetBlueprint<BlueprintWeaponEnchantment>("66e9e299c9002ea4bb65b6f300e43770");
            public static BlueprintWeaponEnchantment Corrosive => Resources.GetBlueprint<BlueprintWeaponEnchantment>("633b38ff1d11de64a91d490c683ab1c8");
            public static BlueprintWeaponEnchantment Corrosive2d6 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("2becfef47bec13940b9ee71f1b14d2dd");
            public static BlueprintWeaponEnchantment CorrosiveBurst => Resources.GetBlueprint<BlueprintWeaponEnchantment>("0cf34703e67e37b40905845ca14b1380");
            public static BlueprintWeaponEnchantment Deteriorative => Resources.GetBlueprint<BlueprintWeaponEnchantment>("bbe55d6e76b973d41bf5abeed643861d");
            public static BlueprintWeaponEnchantment DisruptingWeapon => Resources.GetBlueprint<BlueprintWeaponEnchantment>("7a41c4df836a0e34daa6e82c2bad8a85");
            public static BlueprintWeaponEnchantment Disruption => Resources.GetBlueprint<BlueprintWeaponEnchantment>("0f20d79b7049c0f4ca54ca3d1ea44baa");
            public static BlueprintWeaponEnchantment DragonEssenceBaneDragon => Resources.GetBlueprint<BlueprintWeaponEnchantment>("dd4285c1c3a8d834f888cac9cb0f980a");
            public static BlueprintWeaponEnchantment ElderBrass => Resources.GetBlueprint<BlueprintWeaponEnchantment>("0a8f3559cfcc4d38961bd9658d026cc8");
            public static BlueprintWeaponEnchantment ElderCorrosive => Resources.GetBlueprint<BlueprintWeaponEnchantment>("c7fa5c82d5bb4baf8458dd30981908d1");
            public static BlueprintWeaponEnchantment ElderCorrosive2d6 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("55eece8fead0448aac01c44f37ea065a");
            public static BlueprintWeaponEnchantment ElderCorrosiveBurst => Resources.GetBlueprint<BlueprintWeaponEnchantment>("ea30c02bc5814d8fb600e66dc9d3d520");
            public static BlueprintWeaponEnchantment ElderFlaming => Resources.GetBlueprint<BlueprintWeaponEnchantment>("1735064e8f614d8ca0a065b5e051dbc1");
            public static BlueprintWeaponEnchantment ElderFlamingBurst => Resources.GetBlueprint<BlueprintWeaponEnchantment>("9ab28e024ad2448f8c76c9f60b398e6a");
            public static BlueprintWeaponEnchantment ElderFrost => Resources.GetBlueprint<BlueprintWeaponEnchantment>("c9c2580b9b6c43e992acae157615deb5");
            public static BlueprintWeaponEnchantment ElderIce2d6 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("c4c701aee76742188477a6f24505c222");
            public static BlueprintWeaponEnchantment ElderIcyBurst => Resources.GetBlueprint<BlueprintWeaponEnchantment>("3011dd343b1b41c4b49734b65ca26c5d");
            public static BlueprintWeaponEnchantment ElderShock => Resources.GetBlueprint<BlueprintWeaponEnchantment>("27e28279803f4ef58fdd3ab76e68c376");
            public static BlueprintWeaponEnchantment ElderShock2d6 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("5573c979b9dc403684166fe6e1c31c15");
            public static BlueprintWeaponEnchantment ElderShockingBurst => Resources.GetBlueprint<BlueprintWeaponEnchantment>("a30a16ee048e4d1fb186c5cf4a0984b0");
            public static BlueprintWeaponEnchantment Enhancement1 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("d42fc23b92c640846ac137dc26e000d4");
            public static BlueprintWeaponEnchantment Enhancement2 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("eb2faccc4c9487d43b3575d7e77ff3f5");
            public static BlueprintWeaponEnchantment Enhancement3 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("80bb8a737579e35498177e1e3c75899b");
            public static BlueprintWeaponEnchantment Enhancement4 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("783d7d496da6ac44f9511011fc5f1979");
            public static BlueprintWeaponEnchantment Enhancement5 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("bdba267e951851449af552aa9f9e3992");
            public static BlueprintWeaponEnchantment Enhancement6 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("0326d02d2e24d254a9ef626cc7a3850f");
            public static BlueprintWeaponEnchantment Firebrand => Resources.GetBlueprint<BlueprintWeaponEnchantment>("7b314775d394a2846a9b64651e84a9c6");
            public static BlueprintWeaponEnchantment Flaming => Resources.GetBlueprint<BlueprintWeaponEnchantment>("30f90becaaac51f41bf56641966c4121");
            public static BlueprintWeaponEnchantment FlamingBurst => Resources.GetBlueprint<BlueprintWeaponEnchantment>("3f032a3cd54e57649a0cdad0434bf221");
            public static BlueprintWeaponEnchantment Frost => Resources.GetBlueprint<BlueprintWeaponEnchantment>("421e54078b7719d40915ce0672511d0b");
            public static BlueprintWeaponEnchantment Frost2d8 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("83e7559124cb78a4c9d61360d3a4c3c2");
            public static BlueprintWeaponEnchantment Furious => Resources.GetBlueprint<BlueprintWeaponEnchantment>("b606a3f5daa76cc40add055613970d2a");
            public static BlueprintWeaponEnchantment Furyborn => Resources.GetBlueprint<BlueprintWeaponEnchantment>("091e2f6b2fad84a45ae76b8aac3c55c3");
            public static BlueprintWeaponEnchantment GhostTouch => Resources.GetBlueprint<BlueprintWeaponEnchantment>("47857e1a5a3ec1a46adf6491b1423b4f");
            public static BlueprintWeaponEnchantment GreaterBaneEverything => Resources.GetBlueprint<BlueprintWeaponEnchantment>("bb434647a70ca7e4f9c8050c55a7d235");
            public static BlueprintWeaponEnchantment Heartseeker => Resources.GetBlueprint<BlueprintWeaponEnchantment>("e252b26686ab66241afdf33f2adaead6");
            public static BlueprintWeaponEnchantment Holy => Resources.GetBlueprint<BlueprintWeaponEnchantment>("28a9964d81fedae44bae3ca45710c140");
            public static BlueprintWeaponEnchantment Ice2d6 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("00049f6046b20394091b29702c6e9617");
            public static BlueprintWeaponEnchantment IcyBurst => Resources.GetBlueprint<BlueprintWeaponEnchantment>("564a6924b246d254c920a7c44bf2a58b");
            public static BlueprintWeaponEnchantment Keen => Resources.GetBlueprint<BlueprintWeaponEnchantment>("102a9c8c9b7a75e4fb5844e79deaf4c0");
            public static BlueprintWeaponEnchantment MagicWeapon => Resources.GetBlueprint<BlueprintWeaponEnchantment>("631cb72d11015374987c161a2451a1cf");
            public static BlueprintWeaponEnchantment Masterwork => Resources.GetBlueprint<BlueprintWeaponEnchantment>("6b38844e2bffbac48b63036b66e735be");
            public static BlueprintWeaponEnchantment Necrotic => Resources.GetBlueprint<BlueprintWeaponEnchantment>("bad4134798e182c4487819dce9b43003");
            public static BlueprintWeaponEnchantment Oversized => Resources.GetBlueprint<BlueprintWeaponEnchantment>("d8e1ebc1062d8cc42abff78783856b0d");
            public static BlueprintWeaponEnchantment Radiant => Resources.GetBlueprint<BlueprintWeaponEnchantment>("5ac5c88157f7dde48a2a5b24caf40131");
            public static BlueprintWeaponEnchantment Sacrificial => Resources.GetBlueprint<BlueprintWeaponEnchantment>("b7f029a31452b26408bc75d715227993");
            public static BlueprintWeaponEnchantment Shock => Resources.GetBlueprint<BlueprintWeaponEnchantment>("7bda5277d36ad114f9f9fd21d0dab658");
            public static BlueprintWeaponEnchantment Shock2d6 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("b1de8528121b80844bd7cf09d9e1cf00");
            public static BlueprintWeaponEnchantment ShockingBurst => Resources.GetBlueprint<BlueprintWeaponEnchantment>("914d7ee77fb09d846924ca08bccee0ff");
            public static BlueprintWeaponEnchantment Speed => Resources.GetBlueprint<BlueprintWeaponEnchantment>("f1c0c50108025d546b2554674ea1c006");
            public static BlueprintWeaponEnchantment TemporaryEnhancement1 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("d704f90f54f813043a525f304f6c0050");
            public static BlueprintWeaponEnchantment TemporaryEnhancement2 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("9e9bab3020ec5f64499e007880b37e52");
            public static BlueprintWeaponEnchantment TemporaryEnhancement3 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("d072b841ba0668846adeb007f623bd6c");
            public static BlueprintWeaponEnchantment TemporaryEnhancement4 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("6a6a0901d799ceb49b33d4851ff72132");
            public static BlueprintWeaponEnchantment TemporaryEnhancement5 => Resources.GetBlueprint<BlueprintWeaponEnchantment>("746ee366e50611146821d61e391edf16");
            public static BlueprintWeaponEnchantment Thundering => Resources.GetBlueprint<BlueprintWeaponEnchantment>("690e762f7704e1f4aa1ac69ef0ce6a96");
            public static BlueprintWeaponEnchantment ThunderingBurst => Resources.GetBlueprint<BlueprintWeaponEnchantment>("83bd616525288b34a8f34976b2759ea1");
            public static BlueprintWeaponEnchantment Ultrasound => Resources.GetBlueprint<BlueprintWeaponEnchantment>("582849db96824254ebcc68f0b7484e51");
            public static BlueprintWeaponEnchantment Unholy => Resources.GetBlueprint<BlueprintWeaponEnchantment>("d05753b8df780fc4bb55b318f06af453");
            public static BlueprintWeaponEnchantment ViciousEnchantment => Resources.GetBlueprint<BlueprintWeaponEnchantment>("a1455a289da208144981e4b1ef92cc56");
            public static BlueprintWeaponEnchantment Vorpal => Resources.GetBlueprint<BlueprintWeaponEnchantment>("2f60bfcba52e48a479e4a69868e24ebc");
        }

        private static class ArmorEnchants {
            public static BlueprintArmorEnchantment AcidResistance10Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("dd0e096412423d646929d9b945fd6d4c");
            public static BlueprintArmorEnchantment AcidResistance15Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("09e0be00530efec4693a913d6a7efe23");
            public static BlueprintArmorEnchantment AcidResistance20Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("1346633e0ff138148a9a925e330314b5");
            public static BlueprintArmorEnchantment AcidResistance30Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("e6fa2f59c7f1bb14ebfc429f17d0a4c6");
            public static BlueprintArmorEnchantment AdamantineArmorHeavyEnchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("933456ff83c454146a8bf434e39b1f93");
            public static BlueprintArmorEnchantment AdamantineArmorLightEnchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("5faa3aaee432ac444b101de2b7b0faf7");
            public static BlueprintArmorEnchantment AdamantineArmorMediumEnchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("aa25531ab5bb58941945662aa47b73e7");
            public static BlueprintArmorEnchantment ArcaneArmorBalancedEnchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("53fba8eec3abd214b98a57b12d7ad0a7");
            public static BlueprintArmorEnchantment ArcaneArmorInvulnerabilityEnchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("4ffa3c3d5f6cdfb4eaf15f11d8e55bd1");
            public static BlueprintArmorEnchantment ArcaneArmorShadowEnchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("4e916cbeced676f4e83e02ac65dc562c");
            public static BlueprintArmorEnchantment ArcaneArmorShadowGreaterEnchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("dd8f2032f05d72740961fc95201a5b15");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus1 => Resources.GetBlueprint<BlueprintArmorEnchantment>("a9ea95c5e02f9b7468447bc1010fe152");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus2 => Resources.GetBlueprint<BlueprintArmorEnchantment>("758b77a97640fd747abf149f5bf538d0");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus3 => Resources.GetBlueprint<BlueprintArmorEnchantment>("9448d3026111d6d49b31fc85e7f3745a");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus4 => Resources.GetBlueprint<BlueprintArmorEnchantment>("eaeb89df5be2b784c96181552414ae5a");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus5 => Resources.GetBlueprint<BlueprintArmorEnchantment>("6628f9d77fd07b54c911cd8930c0d531");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus6 => Resources.GetBlueprint<BlueprintArmorEnchantment>("de15272d1f4eb7244aa3af47dbb754ef");
            public static BlueprintArmorEnchantment ColdResistance10Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("c872314ecfab32949ad2e0eebd834919");
            public static BlueprintArmorEnchantment ColdResistance15Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("581c22e55f03e4e4f9f9ea619d89af5f");
            public static BlueprintArmorEnchantment ColdResistance20Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("510d87d2a949587469882061ee186522");
            public static BlueprintArmorEnchantment ColdResistance30Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("7ef70c319ca74fe4cb5eddea792bb353");
            public static BlueprintArmorEnchantment ElectricityResistance10Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("1e4dcaf8ffa56c24788e392dae886166");
            public static BlueprintArmorEnchantment ElectricityResistance15Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("2ed92b92b5381ef488282eb506170322");
            public static BlueprintArmorEnchantment ElectricityResistance20Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("fcfd9515adbd07a43b490280c06203f9");
            public static BlueprintArmorEnchantment ElectricityResistance30Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("26b91513989a653458986fabce24ba95");
            public static BlueprintArmorEnchantment EnergyResistance10n10Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("bc67a01b94164ea4a843028edfcbab01");
            public static BlueprintArmorEnchantment FireResistance10Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("47f45701cc9545049b3745ef949d7446");
            public static BlueprintArmorEnchantment FireResistance15Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("85c2f44721922e4409130791f913d4b4");
            public static BlueprintArmorEnchantment FireResistance20Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("e7af6912cc308df4e9ee63c8824f2738");
            public static BlueprintArmorEnchantment FireResistance30Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("0e98403449de8ce4c846361c6df30d1f");
            public static BlueprintArmorEnchantment Fortification25Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("1e69e9029c627914eb06608dad707b36");
            public static BlueprintArmorEnchantment Fortification50Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("62ec0b22425fb424c82fd52d7f4c02a5");
            public static BlueprintArmorEnchantment Fortification75Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("9b1538c732e06544bbd955fee570a2be");
            public static BlueprintArmorEnchantment GreaterShadow => Resources.GetBlueprint<BlueprintArmorEnchantment>("6b090a291c473984baa5b5bb07a1e300");
            public static BlueprintArmorEnchantment MithralArmorEnchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("7b95a819181574a4799d93939aa99aff");
            public static BlueprintArmorEnchantment NegativeEnergyImmunty15PerEnchantment => Resources.GetBlueprint<BlueprintArmorEnchantment>("3d832dc727c722d49bd02f9f04d0c872");
            public static BlueprintArmorEnchantment NegativeEnergyResistance10Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("34504fb2cecda144aaff34929ba10202");
            public static BlueprintArmorEnchantment NegativeEnergyResistance20Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("1bd448c554f14fc44878bbc983605710");
            public static BlueprintArmorEnchantment NegativeEnergyResistance30Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("27e95849860301b4ab257f72df627149");
            public static BlueprintArmorEnchantment PositiveEnergyResistance30Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("80453601b93f0ef43b215087a484d517");
            public static BlueprintArmorEnchantment ShadowArmor => Resources.GetBlueprint<BlueprintArmorEnchantment>("d64d7aa52626bc24da3906dce17dbc7d");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus1 => Resources.GetBlueprint<BlueprintArmorEnchantment>("e90c252e08035294eba39bafce76c119");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus2 => Resources.GetBlueprint<BlueprintArmorEnchantment>("7b9f2f78a83577d49927c78be0f7fbc1");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus3 => Resources.GetBlueprint<BlueprintArmorEnchantment>("ac2e3a582b5faa74aab66e0a31c935a9");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus4 => Resources.GetBlueprint<BlueprintArmorEnchantment>("a5d27d73859bd19469a6dde3b49750ff");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus5 => Resources.GetBlueprint<BlueprintArmorEnchantment>("84d191a748edef84ba30c13b8ab83bd9");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus6 => Resources.GetBlueprint<BlueprintArmorEnchantment>("70c26c66adb96d74baec38fc8d20c139");
            public static BlueprintArmorEnchantment SonicResistance10Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("6e2dfcafe4faf8941b1426a86a76c368");
            public static BlueprintArmorEnchantment SonicResistance30Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("8b940da1e47fb6843aacdeac9410ec41");
            public static BlueprintArmorEnchantment SpellResistance13Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("4bc20fd0e137e1645a18f030b961ef3d");
            public static BlueprintArmorEnchantment SpellResistance15Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("ad0f81f6377180d4292a2316efb950f2");
            public static BlueprintArmorEnchantment SpellResistance17Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("49fe9e1969afd874181ed7613120c250");
            public static BlueprintArmorEnchantment SpellResistance19Enchant => Resources.GetBlueprint<BlueprintArmorEnchantment>("583938eaafc820f49ad94eca1e5a98ca");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus1 => Resources.GetBlueprint<BlueprintArmorEnchantment>("1d9b60d57afb45c4f9bb0a3c21bb3b98");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus2 => Resources.GetBlueprint<BlueprintArmorEnchantment>("d45bfd838c541bb40bde7b0bf0e1b684");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus3 => Resources.GetBlueprint<BlueprintArmorEnchantment>("51c51d841e9f16046a169729c13c4d4f");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus4 => Resources.GetBlueprint<BlueprintArmorEnchantment>("a23bcee56c9fcf64d863dafedb369387");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus5 => Resources.GetBlueprint<BlueprintArmorEnchantment>("15d7d6cbbf56bd744b37bbf9225ea83b");
        }
    }
}
