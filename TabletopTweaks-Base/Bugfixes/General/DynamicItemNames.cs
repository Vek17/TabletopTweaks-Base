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
using System.Text.RegularExpressions;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.General {
    static class DynamicItemNames {
        [HarmonyPatch(typeof(ItemEntity), nameof(ItemEntity.Name), MethodType.Getter)]
        static class ItemEntity_Names_Patch {
            static Regex EnhancementPatten = new Regex(@"\+\d");
            static void Postfix(ItemEntity __instance, ref string __result) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("DynamicItemNaming")) { return; }
                var itemEntity = __instance switch {
                    ItemEntityWeapon weapon => weapon.IsShield ? weapon.Shield : weapon,
                    _ => __instance
                };
                if (!itemEntity.IsIdentified) { return; }
                string UniqueName = itemEntity.Blueprint.m_DisplayNameText;
                string DefaultName = "";
                switch (itemEntity.Blueprint) {
                    case BlueprintItemWeapon blueprint:
                        DefaultName = blueprint.Type.DefaultName;
                        break;
                    case BlueprintItemArmor blueprint:
                        DefaultName = blueprint.Type.DefaultName;
                        break;
                    default:
                        return;
                }
                /*
                string name = UniqueName.IsNullOrEmpty() ?
                            __instance.GetEnchantmentPrefixes() + DefaultName + __instance.GetEnchantmentSuffixes() :
                            __instance.GetCustomEnchantmentPrefixes() + UniqueName + __instance.GetCustomEnchantmentSuffixes();
                */
                string name = null;
                if (UniqueName.IsNullOrEmpty()) {
                    name = itemEntity.GetEnchantmentPrefixes() + DefaultName + itemEntity.GetEnchantmentSuffixes();
                } else {
                    var suffixes = itemEntity.GetCustomEnchantmentSuffixes();
                    if (EnhancementPatten.Match(suffixes).Success) {
                        name = itemEntity.GetCustomEnchantmentPrefixes() + Regex.Replace(UniqueName, @"\+\d", "") + suffixes;
                    } else {
                        name = itemEntity.GetCustomEnchantmentPrefixes() + UniqueName + suffixes;
                    }
                }
                if (!name.IsNullOrEmpty()) {
                    __result = name;
                }
                //return false;
            }
        }
        private static string GetEnchantmentPrefixes(this IEnumerable<ItemEnchantment> enchants) {
            var includeTemporary = Main.TTTContext.Fixes.BaseFixes.IsEnabled("DynamicItemNamingTemporary");
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
            var includeTemporary = Main.TTTContext.Fixes.BaseFixes.IsEnabled("DynamicItemNamingTemporary");
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
                .Where(e => e.GetComponent<ArmorEnhancementBonus>() == null)
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
                .Where(e => e.GetComponent<ArmorEnhancementBonus>() == null)
                .Where(e => !item.Blueprint.Enchantments.Contains(e.Blueprint))
                .GetEnchantmentSuffixes();
            int totalEnhancment = item.GetItemEnhancementBonus();
            int baseEnhancment = item.GetBlueprintEnhancementBonus();
            if (totalEnhancment > baseEnhancment) {
                text += $" +{totalEnhancment}";
            }
            return text;
        }
        private static int GetItemEnhancementBonus(this ItemEntity item) {
            ItemEntityWeapon weapon = item as ItemEntityWeapon;
            if (weapon != null) { return weapon.GetWeaponEnhancementBonus(); }
            int bonus = GameHelper.GetItemEnhancementBonus(item);
            return bonus;
        }
        private static int GetWeaponEnhancementBonus(this ItemEntityWeapon item) {
            return GameHelper.GetItemEnhancementBonus(item);
        }

        private static int GetBlueprintEnhancementBonus(this ItemEntity item) {
            ItemEntityWeapon weapon = item as ItemEntityWeapon;
            ItemEntityArmor armor = item as ItemEntityArmor;
            if (weapon != null) { return item.GetWeaponBlueprintEnhancementBonus(); }
            if (armor != null) { return item.GetArmorBlueprintEnhancementBonus(); }
            return 0;
        }

        private static int GetWeaponBlueprintEnhancementBonus(this ItemEntity item) {
            ItemEntityWeapon weapon = item as ItemEntityWeapon;
            if (weapon != null) { return GameHelper.GetWeaponEnhancementBonus(weapon.Blueprint); }
            return 0;
        }

        private static int GetArmorBlueprintEnhancementBonus(this ItemEntity item) {
            ItemEntityArmor armor = item as ItemEntityArmor;
            if (armor != null) { return GameHelper.GetArmorEnhancementBonus(armor.Blueprint); }
            return 0;
        }

        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("DynamicItemNaming")) { return; }
                TTTContext.Logger.LogHeader("Patching Enchant Prefixes/Suffixes");

                PatchWeaponEnchants();
                PatchArmorEnchants();

                void PatchWeaponEnchants() {
                    WeaponEnchants.Agile.UpdatePrefixSuffix(TTTContext, "Agile", "");
                    WeaponEnchants.Anarchic.UpdatePrefixSuffix(TTTContext, "Anarchic", "");
                    WeaponEnchants.Axiomatic.UpdatePrefixSuffix(TTTContext, "Axiomatic", "");
                    WeaponEnchants.BaneAberration.UpdatePrefixSuffix(TTTContext, "Aberration bane", "");
                    WeaponEnchants.BaneAnimal.UpdatePrefixSuffix(TTTContext, "Animal Bane", "");
                    WeaponEnchants.BaneConstruct.UpdatePrefixSuffix(TTTContext, "Construct Bane", "");
                    WeaponEnchants.BaneDragon.UpdatePrefixSuffix(TTTContext, "Dragon Bane", "");
                    WeaponEnchants.BaneEverything.UpdatePrefixSuffix(TTTContext, "Bane", "");
                    WeaponEnchants.BaneFey.UpdatePrefixSuffix(TTTContext, "Fey Bane", "");
                    WeaponEnchants.BaneHumanoidGiant.UpdatePrefixSuffix(TTTContext, "Giant Bane", "");
                    WeaponEnchants.BaneHumanoidGiant2d6.UpdatePrefixSuffix(TTTContext, "Giant Bane", "");
                    WeaponEnchants.BaneHumanoidReptilian.UpdatePrefixSuffix(TTTContext, "Reptilian Humanoid Bane", "");
                    WeaponEnchants.BaneLiving.UpdatePrefixSuffix(TTTContext, "Living Bane", "");
                    WeaponEnchants.BaneLycanthrope.UpdatePrefixSuffix(TTTContext, "Lycanthrope Bane", "");
                    WeaponEnchants.BaneMagicalBeast.UpdatePrefixSuffix(TTTContext, "Magical Beast Bane", "");
                    WeaponEnchants.BaneMonstrousHumanoid.UpdatePrefixSuffix(TTTContext, "Monstrous Humanoid Bane", "");
                    WeaponEnchants.BaneOrcGoblin.UpdatePrefixSuffix(TTTContext, "Orc Bane", "");
                    WeaponEnchants.BaneOrcGoblin1d6.UpdatePrefixSuffix(TTTContext, "Goblin Bane", "");
                    WeaponEnchants.BaneOutsiderChaotic.UpdatePrefixSuffix(TTTContext, "Chaotic Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderEvil.UpdatePrefixSuffix(TTTContext, "Evil Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderGood.UpdatePrefixSuffix(TTTContext, "Good Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderLawful.UpdatePrefixSuffix(TTTContext, "Lawful Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderNeutral.UpdatePrefixSuffix(TTTContext, "Neutral Outsider Bane", "");
                    WeaponEnchants.BanePlant.UpdatePrefixSuffix(TTTContext, "Plant Bane", "");
                    WeaponEnchants.BaneUndead.UpdatePrefixSuffix(TTTContext, "Undead Bane", "");
                    WeaponEnchants.BaneVermin.UpdatePrefixSuffix(TTTContext, "Vermin Bane", "");
                    WeaponEnchants.BaneVermin1d8.UpdatePrefixSuffix(TTTContext, "Vermin Bane", "");
                    WeaponEnchants.Bleed.UpdatePrefixSuffix(TTTContext, "Bleed", "");
                    WeaponEnchants.Brass.UpdatePrefixSuffix(TTTContext, "Incinerating", "");
                    WeaponEnchants.Brass2d8.UpdatePrefixSuffix(TTTContext, "Greater Flaming", "");
                    WeaponEnchants.BrilliantEnergy.UpdatePrefixSuffix(TTTContext, "Brilliant Energy", "");
                    WeaponEnchants.Corrosive.UpdatePrefixSuffix(TTTContext, "Corrosive", "");
                    WeaponEnchants.Corrosive2d6.UpdatePrefixSuffix(TTTContext, "Caustic", "");
                    WeaponEnchants.CorrosiveBurst.UpdatePrefixSuffix(TTTContext, "Burst", "");
                    WeaponEnchants.CruelEnchantment.UpdatePrefixSuffix(TTTContext, "Cruel", "");
                    WeaponEnchants.Deteriorative.UpdatePrefixSuffix(TTTContext, "Deteriorative", "");
                    WeaponEnchants.DisruptingWeapon.UpdatePrefixSuffix(TTTContext, "Disruption", "");
                    WeaponEnchants.Disruption.UpdatePrefixSuffix(TTTContext, "Disruption", "");
                    WeaponEnchants.DragonEssenceBaneDragon.UpdatePrefixSuffix(TTTContext, "Dragon Bane", "");
                    WeaponEnchants.ElderBrass.UpdatePrefixSuffix(TTTContext, "Elder Incinerating", "");
                    WeaponEnchants.ElderCorrosive.UpdatePrefixSuffix(TTTContext, "Elder Greater Corrosive", "");
                    WeaponEnchants.ElderCorrosive2d6.UpdatePrefixSuffix(TTTContext, "Elder Caustic", "");
                    WeaponEnchants.ElderCorrosiveBurst.UpdatePrefixSuffix(TTTContext, "Burst", "");
                    WeaponEnchants.ElderFlaming.UpdatePrefixSuffix(TTTContext, "Elder Flaming", "");
                    WeaponEnchants.ElderFlamingBurst.UpdatePrefixSuffix(TTTContext, "Burst", "");
                    WeaponEnchants.ElderFrost.UpdatePrefixSuffix(TTTContext, "Elder Frost", "");
                    WeaponEnchants.ElderIce2d6.UpdatePrefixSuffix(TTTContext, "Elder Freezing", "");
                    WeaponEnchants.ElderIcyBurst.UpdatePrefixSuffix(TTTContext, "Burst", "");
                    WeaponEnchants.ElderShock.UpdatePrefixSuffix(TTTContext, "Elder Shocking", "");
                    WeaponEnchants.ElderShock2d6.UpdatePrefixSuffix(TTTContext, "Elder Greater Shocking", "");
                    WeaponEnchants.ElderShockingBurst.UpdatePrefixSuffix(TTTContext, "Burst", "");
                    WeaponEnchants.Enhancement1.UpdatePrefixSuffix(TTTContext, "", "+1");
                    WeaponEnchants.Enhancement2.UpdatePrefixSuffix(TTTContext, "", "+2");
                    WeaponEnchants.Enhancement3.UpdatePrefixSuffix(TTTContext, "", "+3");
                    WeaponEnchants.Enhancement4.UpdatePrefixSuffix(TTTContext, "", "+4");
                    WeaponEnchants.Enhancement5.UpdatePrefixSuffix(TTTContext, "", "+5");
                    WeaponEnchants.Enhancement6.UpdatePrefixSuffix(TTTContext, "", "+6");
                    WeaponEnchants.Flaming.UpdatePrefixSuffix(TTTContext, "Flaming", "");
                    WeaponEnchants.FlamingBurst.UpdatePrefixSuffix(TTTContext, "Burst", "");
                    WeaponEnchants.Frost.UpdatePrefixSuffix(TTTContext, "Frost", "");
                    WeaponEnchants.Frost2d8.UpdatePrefixSuffix(TTTContext, "Greater Frost", "");
                    WeaponEnchants.Furious.UpdatePrefixSuffix(TTTContext, "Furious", "");
                    WeaponEnchants.Furyborn.UpdatePrefixSuffix(TTTContext, "Furyborn", "");
                    WeaponEnchants.GhostTouch.UpdatePrefixSuffix(TTTContext, "Ghost Touch", "");
                    WeaponEnchants.Heartseeker.UpdatePrefixSuffix(TTTContext, "Heartseeker", "");
                    WeaponEnchants.Holy.UpdatePrefixSuffix(TTTContext, "Holy", "");
                    WeaponEnchants.Ice2d6.UpdatePrefixSuffix(TTTContext, "Freezing", "");
                    WeaponEnchants.IcyBurst.UpdatePrefixSuffix(TTTContext, "Burst", "");
                    WeaponEnchants.Keen.UpdatePrefixSuffix(TTTContext, "Keen", "");
                    WeaponEnchants.MagicWeapon.UpdatePrefixSuffix(TTTContext, "Magic", "");
                    WeaponEnchants.Masterwork.UpdatePrefixSuffix(TTTContext, "Masterwork", "");
                    WeaponEnchants.Necrotic.UpdatePrefixSuffix(TTTContext, "Necrotic", "");
                    WeaponEnchants.NullifyingEnchantment.UpdatePrefixSuffix(TTTContext, "Nullifying", "");
                    WeaponEnchants.Oversized.UpdatePrefixSuffix(TTTContext, "Oversized", "");
                    WeaponEnchants.Radiant.UpdatePrefixSuffix(TTTContext, "Radiant", "");
                    WeaponEnchants.Sacrificial.UpdatePrefixSuffix(TTTContext, "Sacrificial", "");
                    WeaponEnchants.Shock.UpdatePrefixSuffix(TTTContext, "Shocking", "");
                    WeaponEnchants.Shock2d6.UpdatePrefixSuffix(TTTContext, "Shocking", "");
                    WeaponEnchants.ShockingBurst.UpdatePrefixSuffix(TTTContext, "Burst", "");
                    WeaponEnchants.Speed.UpdatePrefixSuffix(TTTContext, "Speed", "");
                    WeaponEnchants.TemporaryEnhancement1.UpdatePrefixSuffix(TTTContext, "", "+1");
                    WeaponEnchants.TemporaryEnhancement2.UpdatePrefixSuffix(TTTContext, "", "+2");
                    WeaponEnchants.TemporaryEnhancement3.UpdatePrefixSuffix(TTTContext, "", "+3");
                    WeaponEnchants.TemporaryEnhancement4.UpdatePrefixSuffix(TTTContext, "", "+4");
                    WeaponEnchants.TemporaryEnhancement5.UpdatePrefixSuffix(TTTContext, "", "+5");
                    WeaponEnchants.Thundering.UpdatePrefixSuffix(TTTContext, "Thundering", "");
                    WeaponEnchants.ThunderingBurst.UpdatePrefixSuffix(TTTContext, "Burst", "");
                    WeaponEnchants.Ultrasound.UpdatePrefixSuffix(TTTContext, "Ultrasound", "");
                    WeaponEnchants.Unholy.UpdatePrefixSuffix(TTTContext, "Unholy", "");
                    WeaponEnchants.ViciousEnchantment.UpdatePrefixSuffix(TTTContext, "Vicious", "");
                    WeaponEnchants.Vorpal.UpdatePrefixSuffix(TTTContext, "Vorpal", "");
                }
                void PatchArmorEnchants() {
                    ArmorEnchants.AcidResistance10Enchant.UpdatePrefixSuffix(TTTContext, "Acid Resistant", "");
                    ArmorEnchants.AcidResistance15Enchant.UpdatePrefixSuffix(TTTContext, "Acid Resistant", "");
                    ArmorEnchants.AcidResistance20Enchant.UpdatePrefixSuffix(TTTContext, "Acid Resistant", "");
                    ArmorEnchants.AcidResistance30Enchant.UpdatePrefixSuffix(TTTContext, "Acid Resistant", "");
                    ArmorEnchants.AdamantineArmorHeavyEnchant.UpdatePrefixSuffix(TTTContext, "Adamantine", "");
                    ArmorEnchants.AdamantineArmorLightEnchant.UpdatePrefixSuffix(TTTContext, "Adamantine", "");
                    ArmorEnchants.AdamantineArmorMediumEnchant.UpdatePrefixSuffix(TTTContext, "Adamantine", "");
                    ArmorEnchants.ArcaneArmorBalancedEnchant.UpdatePrefixSuffix(TTTContext, "Balanced", "");
                    ArmorEnchants.ArcaneArmorInvulnerabilityEnchant.UpdatePrefixSuffix(TTTContext, "Invulnerability", "");
                    ArmorEnchants.ArcaneArmorShadowEnchant.UpdatePrefixSuffix(TTTContext, "Shadow", "");
                    ArmorEnchants.ArcaneArmorShadowGreaterEnchant.UpdatePrefixSuffix(TTTContext, "Greater Shadow", "");
                    ArmorEnchants.ArmorEnhancementBonus1.UpdatePrefixSuffix(TTTContext, "", "+1");
                    ArmorEnchants.ArmorEnhancementBonus2.UpdatePrefixSuffix(TTTContext, "", "+2");
                    ArmorEnchants.ArmorEnhancementBonus3.UpdatePrefixSuffix(TTTContext, "", "+3");
                    ArmorEnchants.ArmorEnhancementBonus4.UpdatePrefixSuffix(TTTContext, "", "+4");
                    ArmorEnchants.ArmorEnhancementBonus5.UpdatePrefixSuffix(TTTContext, "", "+5");
                    ArmorEnchants.ArmorEnhancementBonus6.UpdatePrefixSuffix(TTTContext, "", "+6");
                    ArmorEnchants.ColdResistance10Enchant.UpdatePrefixSuffix(TTTContext, "Cold Resistant", "");
                    ArmorEnchants.ColdResistance15Enchant.UpdatePrefixSuffix(TTTContext, "Cold Resistant", "");
                    ArmorEnchants.ColdResistance20Enchant.UpdatePrefixSuffix(TTTContext, "Cold Resistant", "");
                    ArmorEnchants.ColdResistance30Enchant.UpdatePrefixSuffix(TTTContext, "Cold Resistant", "");
                    ArmorEnchants.ElectricityResistance10Enchant.UpdatePrefixSuffix(TTTContext, "Shock Resistant", "");
                    ArmorEnchants.ElectricityResistance15Enchant.UpdatePrefixSuffix(TTTContext, "Shock Resistant", "");
                    ArmorEnchants.ElectricityResistance20Enchant.UpdatePrefixSuffix(TTTContext, "Shock Resistant", "");
                    ArmorEnchants.ElectricityResistance30Enchant.UpdatePrefixSuffix(TTTContext, "Shock Resistant", "");
                    ArmorEnchants.EnergyResistance10n10Enchant.UpdatePrefixSuffix(TTTContext, "Energy Resistant", "");
                    ArmorEnchants.FireResistance10Enchant.UpdatePrefixSuffix(TTTContext, "Fire Resistant", "");
                    ArmorEnchants.FireResistance15Enchant.UpdatePrefixSuffix(TTTContext, "Fire Resistant", "");
                    ArmorEnchants.FireResistance20Enchant.UpdatePrefixSuffix(TTTContext, "Fire Resistant", "");
                    ArmorEnchants.FireResistance30Enchant.UpdatePrefixSuffix(TTTContext, "Fire Resistant", "");
                    ArmorEnchants.Fortification25Enchant.UpdatePrefixSuffix(TTTContext, "Light Fortification", "");
                    ArmorEnchants.Fortification50Enchant.UpdatePrefixSuffix(TTTContext, "Fortification", "");
                    ArmorEnchants.Fortification75Enchant.UpdatePrefixSuffix(TTTContext, "Heavy Fortification", "");
                    ArmorEnchants.GreaterShadow.UpdatePrefixSuffix(TTTContext, "Greater Shadow", "");
                    ArmorEnchants.MithralArmorEnchant.UpdatePrefixSuffix(TTTContext, "Mithral", "");
                    ArmorEnchants.NegativeEnergyImmunty15PerEnchantment.UpdatePrefixSuffix(TTTContext, "Negative Energy Blocking", "");
                    ArmorEnchants.NegativeEnergyResistance10Enchant.UpdatePrefixSuffix(TTTContext, "Negative Energy Resistant", "");
                    ArmorEnchants.NegativeEnergyResistance20Enchant.UpdatePrefixSuffix(TTTContext, "Negative Energy Resistant", "");
                    ArmorEnchants.NegativeEnergyResistance30Enchant.UpdatePrefixSuffix(TTTContext, "Negative Energy Resistant", "");
                    ArmorEnchants.PositiveEnergyResistance30Enchant.UpdatePrefixSuffix(TTTContext, "Positive Energy Resistant", "");
                    ArmorEnchants.ShadowArmor.UpdatePrefixSuffix(TTTContext, "Shadow", "");
                    ArmorEnchants.ShieldEnhancementBonus1.UpdatePrefixSuffix(TTTContext, "", "+1");
                    ArmorEnchants.ShieldEnhancementBonus2.UpdatePrefixSuffix(TTTContext, "", "+2");
                    ArmorEnchants.ShieldEnhancementBonus3.UpdatePrefixSuffix(TTTContext, "", "+3");
                    ArmorEnchants.ShieldEnhancementBonus4.UpdatePrefixSuffix(TTTContext, "", "+4");
                    ArmorEnchants.ShieldEnhancementBonus5.UpdatePrefixSuffix(TTTContext, "", "+4");
                    ArmorEnchants.ShieldEnhancementBonus6.UpdatePrefixSuffix(TTTContext, "", "+6");
                    ArmorEnchants.SonicResistance10Enchant.UpdatePrefixSuffix(TTTContext, "Sonic Resistant", "");
                    ArmorEnchants.SonicResistance30Enchant.UpdatePrefixSuffix(TTTContext, "Sonic Resistant", "");
                    ArmorEnchants.SpellResistance13Enchant.UpdatePrefixSuffix(TTTContext, "Spell Resistant", "");
                    ArmorEnchants.SpellResistance15Enchant.UpdatePrefixSuffix(TTTContext, "Spell Resistant", "");
                    ArmorEnchants.SpellResistance17Enchant.UpdatePrefixSuffix(TTTContext, "Spell Resistant", "");
                    ArmorEnchants.SpellResistance19Enchant.UpdatePrefixSuffix(TTTContext, "Spell Resistant", "");
                    ArmorEnchants.TemporaryArmorEnhancementBonus1.UpdatePrefixSuffix(TTTContext, "", "+1");
                    ArmorEnchants.TemporaryArmorEnhancementBonus2.UpdatePrefixSuffix(TTTContext, "", "+2");
                    ArmorEnchants.TemporaryArmorEnhancementBonus3.UpdatePrefixSuffix(TTTContext, "", "+3");
                    ArmorEnchants.TemporaryArmorEnhancementBonus4.UpdatePrefixSuffix(TTTContext, "", "+4");
                    ArmorEnchants.TemporaryArmorEnhancementBonus5.UpdatePrefixSuffix(TTTContext, "", "+5");
                }
            }
        }

        private static class WeaponEnchants {
            public static BlueprintWeaponEnchantment Agile => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("a36ad92c51789b44fa8a1c5c116a1328");
            public static BlueprintWeaponEnchantment Anarchic => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("57315bc1e1f62a741be0efde688087e9");
            public static BlueprintWeaponEnchantment Axiomatic => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("0ca43051edefcad4b9b2240aa36dc8d4");
            public static BlueprintWeaponEnchantment BaneAberration => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("ee71cc8848219c24b8418a628cc3e2fa");
            public static BlueprintWeaponEnchantment BaneAnimal => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("78cf9fabe95d3934688ea898c154d904");
            public static BlueprintWeaponEnchantment BaneConstruct => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("73d30862f33cc754bb5a5f3240162ae6");
            public static BlueprintWeaponEnchantment BaneDragon => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("e5cb46a0a658b0a41854447bea32d2ee");
            public static BlueprintWeaponEnchantment BaneEverything => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("1a93ab9c46e48f3488178733be29342a");
            public static BlueprintWeaponEnchantment BaneFey => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("b6948040cdb601242884744a543050d4");
            public static BlueprintWeaponEnchantment BaneHumanoidGiant => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("dcecb5f2ffacfd44ead0ed4f8846445d");
            public static BlueprintWeaponEnchantment BaneHumanoidGiant2d6 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("1e25d1f515c867d40b9c0642e0b40ec2");
            public static BlueprintWeaponEnchantment BaneHumanoidReptilian => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("c4b9cce255d1d6641a6105a255934e2e");
            public static BlueprintWeaponEnchantment BaneLiving => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("e1d6f5e3cd3855b43a0cb42f6c747e1c");
            public static BlueprintWeaponEnchantment BaneLycanthrope => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("188efcfcd9938d44e9561c87794d17a8");
            public static BlueprintWeaponEnchantment BaneMagicalBeast => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("97d477424832c5144a9413c64d818659");
            public static BlueprintWeaponEnchantment BaneMonstrousHumanoid => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("c5f84a79ad154c84e8d2e9fe0dd49350");
            public static BlueprintWeaponEnchantment BaneOrcGoblin => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("0391d8eae25f39a48bcc6c2fc8bf4e12");
            public static BlueprintWeaponEnchantment BaneOrcGoblin1d6 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("ab0108b67cfc2a849926a79ece0fdddc");
            public static BlueprintWeaponEnchantment BaneOutsiderChaotic => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("234177d5807909f44b8c91ed3c9bf7ac");
            public static BlueprintWeaponEnchantment BaneOutsiderEvil => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("20ba9055c6ae1e44ca270c03feacc53b");
            public static BlueprintWeaponEnchantment BaneOutsiderGood => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("a876de94b916b7249a77d090cb9be4f3");
            public static BlueprintWeaponEnchantment BaneOutsiderLawful => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("3a6f564c8ea2d1941a45b19fa16e59f5");
            public static BlueprintWeaponEnchantment BaneOutsiderNeutral => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("4e30e79c500e5af4b86a205cc20436f2");
            public static BlueprintWeaponEnchantment BanePlant => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("0b761b6ed6375114d8d01525d44be5a9");
            public static BlueprintWeaponEnchantment BaneUndead => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("eebb4d3f20b8caa43af1fed8f2773328");
            public static BlueprintWeaponEnchantment BaneVermin => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("c3428441c00354c4fabe27629c6c64dd");
            public static BlueprintWeaponEnchantment BaneVermin1d8 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("e535fc007d0d7e74da45021d4607e607");
            public static BlueprintWeaponEnchantment Bleed => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("ac0108944bfaa7e48aa74f407e3944e3");
            public static BlueprintWeaponEnchantment Brass => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("5e0e5de297c229f42b00c5b1738b50fa");
            public static BlueprintWeaponEnchantment Brass2d8 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("a6064c14629de6d48bec8e8e0460f661");
            public static BlueprintWeaponEnchantment BrilliantEnergy => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("66e9e299c9002ea4bb65b6f300e43770");
            public static BlueprintWeaponEnchantment Corrosive => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("633b38ff1d11de64a91d490c683ab1c8");
            public static BlueprintWeaponEnchantment Corrosive2d6 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("2becfef47bec13940b9ee71f1b14d2dd");
            public static BlueprintWeaponEnchantment CorrosiveBurst => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("0cf34703e67e37b40905845ca14b1380");
            public static BlueprintWeaponEnchantment CruelEnchantment => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("629c383ffb407224398bb71d1bd95d14");
            public static BlueprintWeaponEnchantment Deteriorative => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("bbe55d6e76b973d41bf5abeed643861d");
            public static BlueprintWeaponEnchantment DisruptingWeapon => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("7a41c4df836a0e34daa6e82c2bad8a85");
            public static BlueprintWeaponEnchantment Disruption => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("0f20d79b7049c0f4ca54ca3d1ea44baa");
            public static BlueprintWeaponEnchantment DragonEssenceBaneDragon => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("dd4285c1c3a8d834f888cac9cb0f980a");
            public static BlueprintWeaponEnchantment ElderBrass => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("0a8f3559cfcc4d38961bd9658d026cc8");
            public static BlueprintWeaponEnchantment ElderCorrosive => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("c7fa5c82d5bb4baf8458dd30981908d1");
            public static BlueprintWeaponEnchantment ElderCorrosive2d6 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("55eece8fead0448aac01c44f37ea065a");
            public static BlueprintWeaponEnchantment ElderCorrosiveBurst => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("ea30c02bc5814d8fb600e66dc9d3d520");
            public static BlueprintWeaponEnchantment ElderFlaming => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("1735064e8f614d8ca0a065b5e051dbc1");
            public static BlueprintWeaponEnchantment ElderFlamingBurst => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("9ab28e024ad2448f8c76c9f60b398e6a");
            public static BlueprintWeaponEnchantment ElderFrost => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("c9c2580b9b6c43e992acae157615deb5");
            public static BlueprintWeaponEnchantment ElderIce2d6 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("c4c701aee76742188477a6f24505c222");
            public static BlueprintWeaponEnchantment ElderIcyBurst => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("3011dd343b1b41c4b49734b65ca26c5d");
            public static BlueprintWeaponEnchantment ElderShock => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("27e28279803f4ef58fdd3ab76e68c376");
            public static BlueprintWeaponEnchantment ElderShock2d6 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("5573c979b9dc403684166fe6e1c31c15");
            public static BlueprintWeaponEnchantment ElderShockingBurst => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("a30a16ee048e4d1fb186c5cf4a0984b0");
            public static BlueprintWeaponEnchantment Enhancement1 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("d42fc23b92c640846ac137dc26e000d4");
            public static BlueprintWeaponEnchantment Enhancement2 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("eb2faccc4c9487d43b3575d7e77ff3f5");
            public static BlueprintWeaponEnchantment Enhancement3 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("80bb8a737579e35498177e1e3c75899b");
            public static BlueprintWeaponEnchantment Enhancement4 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("783d7d496da6ac44f9511011fc5f1979");
            public static BlueprintWeaponEnchantment Enhancement5 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("bdba267e951851449af552aa9f9e3992");
            public static BlueprintWeaponEnchantment Enhancement6 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("0326d02d2e24d254a9ef626cc7a3850f");
            public static BlueprintWeaponEnchantment Firebrand => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("7b314775d394a2846a9b64651e84a9c6");
            public static BlueprintWeaponEnchantment Flaming => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("30f90becaaac51f41bf56641966c4121");
            public static BlueprintWeaponEnchantment FlamingBurst => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("3f032a3cd54e57649a0cdad0434bf221");
            public static BlueprintWeaponEnchantment Frost => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("421e54078b7719d40915ce0672511d0b");
            public static BlueprintWeaponEnchantment Frost2d8 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("83e7559124cb78a4c9d61360d3a4c3c2");
            public static BlueprintWeaponEnchantment Furious => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("b606a3f5daa76cc40add055613970d2a");
            public static BlueprintWeaponEnchantment Furyborn => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("091e2f6b2fad84a45ae76b8aac3c55c3");
            public static BlueprintWeaponEnchantment GhostTouch => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("47857e1a5a3ec1a46adf6491b1423b4f");
            public static BlueprintWeaponEnchantment GreaterBaneEverything => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("bb434647a70ca7e4f9c8050c55a7d235");
            public static BlueprintWeaponEnchantment Heartseeker => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("e252b26686ab66241afdf33f2adaead6");
            public static BlueprintWeaponEnchantment Holy => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("28a9964d81fedae44bae3ca45710c140");
            public static BlueprintWeaponEnchantment Ice2d6 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("00049f6046b20394091b29702c6e9617");
            public static BlueprintWeaponEnchantment IcyBurst => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("564a6924b246d254c920a7c44bf2a58b");
            public static BlueprintWeaponEnchantment Keen => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("102a9c8c9b7a75e4fb5844e79deaf4c0");
            public static BlueprintWeaponEnchantment MagicWeapon => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("631cb72d11015374987c161a2451a1cf");
            public static BlueprintWeaponEnchantment Masterwork => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("6b38844e2bffbac48b63036b66e735be");
            public static BlueprintWeaponEnchantment Necrotic => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("bad4134798e182c4487819dce9b43003");
            public static BlueprintWeaponEnchantment NullifyingEnchantment => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("efbe3a35fc7349845ac9f96b4c63312e");
            public static BlueprintWeaponEnchantment Oversized => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("d8e1ebc1062d8cc42abff78783856b0d");
            public static BlueprintWeaponEnchantment Radiant => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("5ac5c88157f7dde48a2a5b24caf40131");
            public static BlueprintWeaponEnchantment Sacrificial => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("b7f029a31452b26408bc75d715227993");
            public static BlueprintWeaponEnchantment Shock => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("7bda5277d36ad114f9f9fd21d0dab658");
            public static BlueprintWeaponEnchantment Shock2d6 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("b1de8528121b80844bd7cf09d9e1cf00");
            public static BlueprintWeaponEnchantment ShockingBurst => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("914d7ee77fb09d846924ca08bccee0ff");
            public static BlueprintWeaponEnchantment Speed => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("f1c0c50108025d546b2554674ea1c006");
            public static BlueprintWeaponEnchantment TemporaryEnhancement1 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("d704f90f54f813043a525f304f6c0050");
            public static BlueprintWeaponEnchantment TemporaryEnhancement2 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("9e9bab3020ec5f64499e007880b37e52");
            public static BlueprintWeaponEnchantment TemporaryEnhancement3 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("d072b841ba0668846adeb007f623bd6c");
            public static BlueprintWeaponEnchantment TemporaryEnhancement4 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("6a6a0901d799ceb49b33d4851ff72132");
            public static BlueprintWeaponEnchantment TemporaryEnhancement5 => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("746ee366e50611146821d61e391edf16");
            public static BlueprintWeaponEnchantment Thundering => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("690e762f7704e1f4aa1ac69ef0ce6a96");
            public static BlueprintWeaponEnchantment ThunderingBurst => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("83bd616525288b34a8f34976b2759ea1");
            public static BlueprintWeaponEnchantment Ultrasound => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("582849db96824254ebcc68f0b7484e51");
            public static BlueprintWeaponEnchantment Unholy => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("d05753b8df780fc4bb55b318f06af453");
            public static BlueprintWeaponEnchantment ViciousEnchantment => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("a1455a289da208144981e4b1ef92cc56");
            public static BlueprintWeaponEnchantment Vorpal => BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("2f60bfcba52e48a479e4a69868e24ebc");
        }

        private static class ArmorEnchants {
            public static BlueprintArmorEnchantment AcidResistance10Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("dd0e096412423d646929d9b945fd6d4c");
            public static BlueprintArmorEnchantment AcidResistance15Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("09e0be00530efec4693a913d6a7efe23");
            public static BlueprintArmorEnchantment AcidResistance20Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("1346633e0ff138148a9a925e330314b5");
            public static BlueprintArmorEnchantment AcidResistance30Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("e6fa2f59c7f1bb14ebfc429f17d0a4c6");
            public static BlueprintArmorEnchantment AdamantineArmorHeavyEnchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("933456ff83c454146a8bf434e39b1f93");
            public static BlueprintArmorEnchantment AdamantineArmorLightEnchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("5faa3aaee432ac444b101de2b7b0faf7");
            public static BlueprintArmorEnchantment AdamantineArmorMediumEnchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("aa25531ab5bb58941945662aa47b73e7");
            public static BlueprintArmorEnchantment ArcaneArmorBalancedEnchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("53fba8eec3abd214b98a57b12d7ad0a7");
            public static BlueprintArmorEnchantment ArcaneArmorInvulnerabilityEnchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("4ffa3c3d5f6cdfb4eaf15f11d8e55bd1");
            public static BlueprintArmorEnchantment ArcaneArmorShadowEnchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("4e916cbeced676f4e83e02ac65dc562c");
            public static BlueprintArmorEnchantment ArcaneArmorShadowGreaterEnchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("dd8f2032f05d72740961fc95201a5b15");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus1 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("a9ea95c5e02f9b7468447bc1010fe152");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus2 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("758b77a97640fd747abf149f5bf538d0");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus3 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("9448d3026111d6d49b31fc85e7f3745a");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus4 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("eaeb89df5be2b784c96181552414ae5a");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus5 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("6628f9d77fd07b54c911cd8930c0d531");
            public static BlueprintArmorEnchantment ArmorEnhancementBonus6 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("de15272d1f4eb7244aa3af47dbb754ef");
            public static BlueprintArmorEnchantment ColdResistance10Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("c872314ecfab32949ad2e0eebd834919");
            public static BlueprintArmorEnchantment ColdResistance15Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("581c22e55f03e4e4f9f9ea619d89af5f");
            public static BlueprintArmorEnchantment ColdResistance20Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("510d87d2a949587469882061ee186522");
            public static BlueprintArmorEnchantment ColdResistance30Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("7ef70c319ca74fe4cb5eddea792bb353");
            public static BlueprintArmorEnchantment ElectricityResistance10Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("1e4dcaf8ffa56c24788e392dae886166");
            public static BlueprintArmorEnchantment ElectricityResistance15Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("2ed92b92b5381ef488282eb506170322");
            public static BlueprintArmorEnchantment ElectricityResistance20Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("fcfd9515adbd07a43b490280c06203f9");
            public static BlueprintArmorEnchantment ElectricityResistance30Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("26b91513989a653458986fabce24ba95");
            public static BlueprintArmorEnchantment EnergyResistance10n10Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("bc67a01b94164ea4a843028edfcbab01");
            public static BlueprintArmorEnchantment FireResistance10Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("47f45701cc9545049b3745ef949d7446");
            public static BlueprintArmorEnchantment FireResistance15Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("85c2f44721922e4409130791f913d4b4");
            public static BlueprintArmorEnchantment FireResistance20Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("e7af6912cc308df4e9ee63c8824f2738");
            public static BlueprintArmorEnchantment FireResistance30Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("0e98403449de8ce4c846361c6df30d1f");
            public static BlueprintArmorEnchantment Fortification25Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("1e69e9029c627914eb06608dad707b36");
            public static BlueprintArmorEnchantment Fortification50Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("62ec0b22425fb424c82fd52d7f4c02a5");
            public static BlueprintArmorEnchantment Fortification75Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("9b1538c732e06544bbd955fee570a2be");
            public static BlueprintArmorEnchantment GreaterShadow => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("6b090a291c473984baa5b5bb07a1e300");
            public static BlueprintArmorEnchantment MithralArmorEnchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("7b95a819181574a4799d93939aa99aff");
            public static BlueprintArmorEnchantment NegativeEnergyImmunty15PerEnchantment => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("3d832dc727c722d49bd02f9f04d0c872");
            public static BlueprintArmorEnchantment NegativeEnergyResistance10Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("34504fb2cecda144aaff34929ba10202");
            public static BlueprintArmorEnchantment NegativeEnergyResistance20Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("1bd448c554f14fc44878bbc983605710");
            public static BlueprintArmorEnchantment NegativeEnergyResistance30Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("27e95849860301b4ab257f72df627149");
            public static BlueprintArmorEnchantment PositiveEnergyResistance30Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("80453601b93f0ef43b215087a484d517");
            public static BlueprintArmorEnchantment ShadowArmor => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("d64d7aa52626bc24da3906dce17dbc7d");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus1 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("e90c252e08035294eba39bafce76c119");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus2 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("7b9f2f78a83577d49927c78be0f7fbc1");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus3 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("ac2e3a582b5faa74aab66e0a31c935a9");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus4 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("a5d27d73859bd19469a6dde3b49750ff");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus5 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("84d191a748edef84ba30c13b8ab83bd9");
            public static BlueprintArmorEnchantment ShieldEnhancementBonus6 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("70c26c66adb96d74baec38fc8d20c139");
            public static BlueprintArmorEnchantment SonicResistance10Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("6e2dfcafe4faf8941b1426a86a76c368");
            public static BlueprintArmorEnchantment SonicResistance30Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("8b940da1e47fb6843aacdeac9410ec41");
            public static BlueprintArmorEnchantment SpellResistance13Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("4bc20fd0e137e1645a18f030b961ef3d");
            public static BlueprintArmorEnchantment SpellResistance15Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("ad0f81f6377180d4292a2316efb950f2");
            public static BlueprintArmorEnchantment SpellResistance17Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("49fe9e1969afd874181ed7613120c250");
            public static BlueprintArmorEnchantment SpellResistance19Enchant => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("583938eaafc820f49ad94eca1e5a98ca");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus1 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("1d9b60d57afb45c4f9bb0a3c21bb3b98");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus2 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("d45bfd838c541bb40bde7b0bf0e1b684");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus3 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("51c51d841e9f16046a169729c13c4d4f");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus4 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("a23bcee56c9fcf64d863dafedb369387");
            public static BlueprintArmorEnchantment TemporaryArmorEnhancementBonus5 => BlueprintTools.GetBlueprint<BlueprintArmorEnchantment>("15d7d6cbbf56bd744b37bbf9225ea83b");
        }
    }
}
