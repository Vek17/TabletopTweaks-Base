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
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.General {
    static class ItemEntityPatches {
        [HarmonyPatch(typeof(ItemEntity), nameof(ItemEntity.Name), MethodType.Getter)]
        static class ItemEntity_Names_Patch {
            static bool Prefix(ItemEntity __instance, ref string __result) {
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
            if (enchants == null || enchants.Empty()) {
                return "";
            }
            string text = "";
            foreach (BlueprintItemEnchantment blueprintEnchantment in enchants.Where(e => !e.IsTemporary).Select(e => e.Blueprint)) {
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
            if (enchants == null || enchants.Empty()) {
                return "";
            }
            string text = "";
            foreach (BlueprintItemEnchantment blueprintEnchantment in enchants.Where(e => !e.IsTemporary).Select(e => e.Blueprint)) {
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
                Main.LogHeader("Patching Enchant Prefixes/Suffixes");

                PatchWeaponEnchants();
                PatchArmorEnchants();

                void PatchWeaponEnchants() {
                    WeaponEnchants.Agile.UpdatePrefixSuffix("13103c339c5647fe828b03e8fa8797a3", "2ed4ab7e9576477bb1f844120c0d86d4", "Agile", "");
                    WeaponEnchants.Anarchic.UpdatePrefixSuffix("791d2774aedd44b3a34279e4d5f04910", "9696f205f0cc429cab2e9c4ac1ab9fdb", "Anarchic", "");
                    WeaponEnchants.Axiomatic.UpdatePrefixSuffix("5bb7e714546c4d898782c5d892ab8cee", "1dd4c96efd2d43728b3cd8ecd5bf21b6", "Axiomatic", "");
                    WeaponEnchants.BaneAberration.UpdatePrefixSuffix("e80ed9ee942c4b1ea269483d1a7b4dfd", "b21e9153a4a54462a1713f9e45e6b799", "Aberration bane", "");
                    WeaponEnchants.BaneAnimal.UpdatePrefixSuffix("ddf9ee09496b4c2da8cdab1ee664526e", "4b25ea085a98499bafcb8c08e34bc10d", "Animal Bane", "");
                    WeaponEnchants.BaneConstruct.UpdatePrefixSuffix("30dba5711f9b4a0badeff8861711daae", "205ca13470ee4650ac43f03b2708a5ff", "Construct Bane", "");
                    WeaponEnchants.BaneDragon.UpdatePrefixSuffix("483d4a0d8e614304a57120592afa79d6", "cc48f0aa41a24ab4afb0222b109f7887", "Dragon Bane", "");
                    WeaponEnchants.BaneEverything.UpdatePrefixSuffix("34c415c7fc8c4f78806142a6df2ae24e", "fc40d7a3df2547eb8736f319a75539c1", "Bane", "");
                    WeaponEnchants.BaneFey.UpdatePrefixSuffix("327896460edf4662951a9f9f324fea53", "e905131e837d42d99e5cb7d6b9823bdc", "Fey Bane", "");
                    WeaponEnchants.BaneHumanoidGiant.UpdatePrefixSuffix("058cf7a695d949db9b8f96e46f37047f", "f024192b9cc1448badf2077ac9ee9e47", "Giant Bane", "");
                    WeaponEnchants.BaneHumanoidGiant2d6.UpdatePrefixSuffix("cf87e2da882c4312b2bb0e0d9695bee7", "83f1d0c36ec64a5caf3522592a6d3fbe", "Giant Bane", "");
                    WeaponEnchants.BaneHumanoidReptilian.UpdatePrefixSuffix("7ba4f160743849b89211f0e20294679b", "a1e83b53bc934deb860df5cc378532f7", "Reptilian Humanoid Bane", "");
                    WeaponEnchants.BaneLiving.UpdatePrefixSuffix("267a95f318974726ab89f0fdcd5cddfb", "5e7b8dd22e724b45b2da4907b06a610d", "Living Bane", "");
                    WeaponEnchants.BaneLycanthrope.UpdatePrefixSuffix("a26db876046a42769ec7df9b74ea0093", "c04fb9b6585d495a983f2adc066eef04", "Lycanthrope Bane", "");
                    WeaponEnchants.BaneMagicalBeast.UpdatePrefixSuffix("197c5922dcd8477c9ff739c4f188c17f", "58a8040a5c464e419d5124001247af15", "Magical Beast Bane", "");
                    WeaponEnchants.BaneMonstrousHumanoid.UpdatePrefixSuffix("c02182ba93f94654a408207bc91b581c", "c7224dbeef2a427c82b3c99414aba28a", "Monstrous Humanoid Bane", "");
                    WeaponEnchants.BaneOrcGoblin.UpdatePrefixSuffix("648b37b0617645eca611dc8ba360d220", "0ddcb82f20414b12b4b37d4a594a4114", "Orc Bane", "");
                    WeaponEnchants.BaneOrcGoblin1d6.UpdatePrefixSuffix("328c932c55d54e16b32a2ecd24ec14e7", "acaff627c1d84142b21ea5ec5508c576", "Goblin Bane", "");
                    WeaponEnchants.BaneOutsiderChaotic.UpdatePrefixSuffix("7efd7928bf644aacb3168ce7bce379b6", "113ac23f3d7c497bb409cb8271b1e06a", "Chaotic Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderEvil.UpdatePrefixSuffix("5cb989f2ca274d52a201bec476997544", "97059a708cd34a47bd3130c344515060", "Evil Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderGood.UpdatePrefixSuffix("aa8e1c48ad3342b49f30748828925199", "a21ed16bc3ef494b9cad6efff5296ca6", "Good Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderLawful.UpdatePrefixSuffix("9227b68c4c414ac599f683b9592c4508", "8aa12028266e4160968cfab7ef8f4e15", "Lawful Outsider Bane", "");
                    WeaponEnchants.BaneOutsiderNeutral.UpdatePrefixSuffix("f140a8b22db44c4cbce651b2aaf49d66", "0aa5af23589c437e857468f39ecbaca7", "Neutral Outsider Bane", "");
                    WeaponEnchants.BanePlant.UpdatePrefixSuffix("2b5c2e9a4dba43b58e5547fdb6b52e30", "a551fecab31d4d8ea6155768d5e66d89", "Plant Bane", "");
                    WeaponEnchants.BaneUndead.UpdatePrefixSuffix("4f1cfd445c1b486dbcff0e9a278d1a25", "2e6f9fd691464b46bfe22a02daad9655", "Undead Bane", "");
                    WeaponEnchants.BaneVermin.UpdatePrefixSuffix("873645317d324800b3d3eadaa64d6f61", "bffc3c02d1874277b9a7a13b9ae89548", "Vermin Bane", "");
                    WeaponEnchants.BaneVermin1d8.UpdatePrefixSuffix("aec89e5697564fc293006350e40e046d", "03d17e3ea5d241e4ba0b731a6015d936", "Vermin Bane", "");
                    WeaponEnchants.Bleed.UpdatePrefixSuffix("72928fd58e63469b9dcbf3c0eb7b56b2", "431c7c02d61846efb6fc19e7bdea15a8", "Bleed", "");
                    WeaponEnchants.Brass.UpdatePrefixSuffix("37df8bdd9bcf476e82186546ba976884", "35412efa3cd44a6db6633bf8f0ccaa42", "Flaming", "");
                    WeaponEnchants.Brass2d8.UpdatePrefixSuffix("1a4efcc5257d463f87d26807037d27d4", "b98e9624ce874f8f875a27b5a0e07d9b", "Greater Flaming", "");
                    WeaponEnchants.BrilliantEnergy.UpdatePrefixSuffix("1f4be1d6fc2e43f19c7fd2f7efea0801", "8021f3a92eb843edb7083c8320351654", "Brilliant Energy", "");
                    WeaponEnchants.Corrosive.UpdatePrefixSuffix("2e696bf572ef4208b5b917976fccf3aa", "72390e4ef89b4807967e2f8f70265758", "Corrosive", "");
                    WeaponEnchants.Corrosive2d6.UpdatePrefixSuffix("f33ea993d9a746988ab208ea50cb2b95", "3a0e2462fc924e83a728de86dad37548", "Caustic", "");
                    WeaponEnchants.CorrosiveBurst.UpdatePrefixSuffix("41dc209dad7e4059b7bd81ec98074388", "1b94bf4b429d4529acf6da83bb4156f8", "Burst", "");
                    WeaponEnchants.Deteriorative.UpdatePrefixSuffix("98c714ec174648969a35f38323b8e95c", "cd48af5a077f40178cc19c69ea34c1fd", "Deteriorative", "");
                    WeaponEnchants.DisruptingWeapon.UpdatePrefixSuffix("7a4188c4c76b4d7da7590cee361a7b8e", "656e8f61be474472917a4f8ffcbdf702", "Disruption", "");
                    WeaponEnchants.Disruption.UpdatePrefixSuffix("c4886dac10214eb395fe68648ffe49c6", "18aa3d6a7ff1477092658c522a4a7df5", "Disruption", "");
                    WeaponEnchants.DragonEssenceBaneDragon.UpdatePrefixSuffix("9a33a81e11904c6f921cb52581d94ad7", "8309cab1cba14a748e975eb4e9f5d76d", "Dragon Bane", "");
                    WeaponEnchants.ElderBrass.UpdatePrefixSuffix("c2ec8024aee6474aafdb5b47544bb5d0", "a3a78f047fb14134b57c5be050a35788", "Elder Flaming", "");
                    WeaponEnchants.ElderCorrosive.UpdatePrefixSuffix("843210f1c3d54f7a88e8fadb2bcbfb89", "8c850e117c29495fb432ac3b98e25fc9", "Elder Greater Corrosive", "");
                    WeaponEnchants.ElderCorrosive2d6.UpdatePrefixSuffix("3a71bca89b1e462192704e7068812358", "ef0031fbaed44486830993e73dc2c3a4", "Elder Caustic", "");
                    WeaponEnchants.ElderCorrosiveBurst.UpdatePrefixSuffix("4e26034109534bb1afa9b3def2d69afd", "0f3edb4389af46f594385829e3fdc026", "Burst", "");
                    WeaponEnchants.ElderFlaming.UpdatePrefixSuffix("4a1c1db1394a4398bf4156953397bb76", "424a0173eeba470b861a28909114fca8", "Elder Flaming", "");
                    WeaponEnchants.ElderFlamingBurst.UpdatePrefixSuffix("c4c252911e404da491fb1216cbb32cde", "c96ee078235b4b65855c11aa17661474", "Burst", "");
                    WeaponEnchants.ElderFrost.UpdatePrefixSuffix("209e77a4b9854a8cbe9d3aaeb8c216d5", "ade5d2d1dcf34563b8b86bcf5171a8ca", "Elder Frost", "");
                    WeaponEnchants.ElderIce2d6.UpdatePrefixSuffix("5aada22466d64282959a8a176d9b408f", "bfa88472d9a442ee8528da7d0e45799b", "Elder Freezing", "");
                    WeaponEnchants.ElderIcyBurst.UpdatePrefixSuffix("59a919327aba45e3af16a68c80d2fae0", "ba05ed2a40104c52815a1f9d82d13f9f", "Burst", "");
                    WeaponEnchants.ElderShock.UpdatePrefixSuffix("e1ee5ee7111a401c9663da5a39b33c6d", "86ede9ec6d0d467e9cd9b6d875af1ac9", "Elder Shocking", "");
                    WeaponEnchants.ElderShock2d6.UpdatePrefixSuffix("5d35b8d5a74a42ffab8f82210baae00f", "3a0954fcd61f46babe50203f2bfec0c9", "Elder Greater Shocking", "");
                    WeaponEnchants.ElderShockingBurst.UpdatePrefixSuffix("88a4d278485d4ddf954542cdf9f8bc7c", "b47846f568ee4775b161ea437339ec07", "Burst", "");
                    WeaponEnchants.Enhancement1.UpdatePrefixSuffix("7d9b2b52d81c473bb542d83d84562115", "b454ca42904140ee9e67d07caa166114", "", "+1");
                    WeaponEnchants.Enhancement2.UpdatePrefixSuffix("68a31b09bbf1424b813f21a9768a7829", "c47663610e4f49d6941281d48e159408", "", "+2");
                    WeaponEnchants.Enhancement3.UpdatePrefixSuffix("da84c958592e4f18854c15d5f0985569", "bc4ee85ad48e41da86cac0427205c89b", "", "+3");
                    WeaponEnchants.Enhancement4.UpdatePrefixSuffix("d6fc81752d5e46cd884e3533dda3718e", "d3304617392e45ea894880957d4a15f2", "", "+4");
                    WeaponEnchants.Enhancement5.UpdatePrefixSuffix("d99cc681a0074b7e8c83cc3edd7be453", "3239fdb19a0944f1aef429535c14580e", "", "+5");
                    WeaponEnchants.Enhancement6.UpdatePrefixSuffix("c9104c678a4447618afa7a508d5a8e2a", "5de504527a0140dfad9ccd8365ad7268", "", "+6");
                    WeaponEnchants.Flaming.UpdatePrefixSuffix("19f6995b8c2f41578f5266d24a95ecaf", "66ea662f3482487d99b8de4b21e5137d", "Flaming", "");
                    WeaponEnchants.FlamingBurst.UpdatePrefixSuffix("1d0dcb894d7940978713643798e21fc5", "4e127d8248d44e67ad7032331d3b01b0", "Burst", "");
                    WeaponEnchants.Frost.UpdatePrefixSuffix("dcd3a1bbec7743f2aebaf0ef9425975e", "e74bb40112264346b205de40b0fa38f9", "Frost", "");
                    WeaponEnchants.Frost2d8.UpdatePrefixSuffix("9249c9342dff452fa2b4639abcd9b900", "906dd57b17b940c9855ce0a4eabeca4d", "Greater Frost", "");
                    WeaponEnchants.Furious.UpdatePrefixSuffix("8cb1b6ddb61d4eb09d0428c31c88806a", "9159de92e28347bf8fa336fd58c31d86", "Furious", "");
                    WeaponEnchants.Furyborn.UpdatePrefixSuffix("f398dd6790174008ac7625f7dc7a991a", "467837be60e44e1993ec838c407d830d", "Furyborn", "");
                    WeaponEnchants.GhostTouch.UpdatePrefixSuffix("c4f7c820b5f648a78adc714743496d98", "68b8204772db4008906f990ef6cc8bbb", "Ghost Touch", "");
                    WeaponEnchants.Heartseeker.UpdatePrefixSuffix("c1ca1c19c1944060b1e4842b6d07f647", "9d2ad34ba7a54e4aab94a863e7f560fc", "Heartseeker", "");
                    WeaponEnchants.Holy.UpdatePrefixSuffix("ebae158ba0504408a0cf176d9ef08174", "efcb32acac7940f2aaf6163792fe59e2", "Holy", "");
                    WeaponEnchants.Ice2d6.UpdatePrefixSuffix("997adce5e6ef47d2a8c335a11a6229ee", "39543fe2a0fb4bf68dc12923459bfbdd", "Freezing", "");
                    WeaponEnchants.IcyBurst.UpdatePrefixSuffix("88a0305e711649cbb8362db9f68c288d", "19b8f088e1524d54aab78430c48b0615", "Burst", "");
                    WeaponEnchants.Keen.UpdatePrefixSuffix("3050e4c02fff47a8a50ce2c997b812d2", "0075ad8d586a4353bff6d28fde76f936", "Keen", "");
                    WeaponEnchants.MagicWeapon.UpdatePrefixSuffix("924e4b28350c47c7bf9d2e83cc444324", "a1859b6828cc441d9ddd0d11765dd528", "Magic", "");
                    WeaponEnchants.Masterwork.UpdatePrefixSuffix("bbe1528a347b4206a9c7badb03da06e4", "4f6f229237ef4791a9c9f4ee5f1854df", "Masterwork", "");
                    WeaponEnchants.Necrotic.UpdatePrefixSuffix("5f1b984a389043baab41f1a8e5f17da6", "79f0163524ad4b569665293a1ce28666", "Necrotic", "");
                    WeaponEnchants.Oversized.UpdatePrefixSuffix("7c6ffcfe8f31499aa39417e5aebdd29b", "fa8c9396f2f84861af4f960932dba31a", "Oversized", "");
                    WeaponEnchants.Radiant.UpdatePrefixSuffix("cf4155b50e1e402497a88a30aa503bc8", "488452ae11d8453aae09cbb0fb46abd4", "Radiant", "");
                    WeaponEnchants.Sacrificial.UpdatePrefixSuffix("2775f5f07fef4b73a62c50d49e0a76ae", "fa5c3314f8b44bda901bb03c2f06b7ea", "Sacrificial", "");
                    WeaponEnchants.Shock.UpdatePrefixSuffix("dc2e7f82782b41b0b101828d018b3513", "b68237b611a24f1391db61305653208e", "Shocking", "");
                    WeaponEnchants.Shock2d6.UpdatePrefixSuffix("81e81fab24384a189222388e841ebfc9", "9f68dce6e1fb44e38cb3b9effe20da5c", "Shocking", "");
                    WeaponEnchants.ShockingBurst.UpdatePrefixSuffix("87438d13296e408aa4a4bd39916836a8", "f3c0d9f2d9ea46cd81159aed2d5d85b3", "Burst", "");
                    WeaponEnchants.Speed.UpdatePrefixSuffix("5db0055999d24b0e99e84df7fa5f7c2c", "968391a401ce4f89b6c5dd5e70ebe6d5", "Speed", "");
                    WeaponEnchants.TemporaryEnhancement1.UpdatePrefixSuffix("b888dc37248b43688b9f87b0464d20b6", "c2d39383f6914d7e8ecbaf0dffbd7f97", "", "+1");
                    WeaponEnchants.TemporaryEnhancement2.UpdatePrefixSuffix("7f0539d203d040b5b26772bf923bdecd", "43f1d74c70d94498b63e6e1c9b158ad7", "", "+2");
                    WeaponEnchants.TemporaryEnhancement3.UpdatePrefixSuffix("2172cecc60604e76bcd5f514e2eb1a39", "dfde6099b0204741a1f72f258042138a", "", "+3");
                    WeaponEnchants.TemporaryEnhancement4.UpdatePrefixSuffix("a6210285657944ae9efa54740c270cf4", "f8c418da0a764f59a9c37730326cac33", "", "+4");
                    WeaponEnchants.TemporaryEnhancement5.UpdatePrefixSuffix("d27c0ae1d39441aa837fc1535b3b1960", "3b6abbb03161466ab5e7922ded6dde24", "", "+5");
                    WeaponEnchants.Thundering.UpdatePrefixSuffix("e5b38696fc1f4856bfc5c32b1331a40e", "2b5a99f0a9bc4c15ae7aaea3ab41b7bf", "Thundering", "");
                    WeaponEnchants.ThunderingBurst.UpdatePrefixSuffix("f14ee89dd3104a959e2d69dd7cad4899", "f75acfd2be6b4dd7917f87622e49e9a7", "Burst", "");
                    WeaponEnchants.Ultrasound.UpdatePrefixSuffix("b5d307bd0a4c4849bfc0d8681032b182", "0b37bf5ad5174580bc7098ad949c5b64", "Ultrasound", "");
                    WeaponEnchants.Unholy.UpdatePrefixSuffix("73bf1a7e23c140e8ac6f5f0bb4716f72", "3e3444704d3a4c30be05748435c89f4a", "Unholy", "");
                    WeaponEnchants.ViciousEnchantment.UpdatePrefixSuffix("d37d5761b951426586eb01c77f1c5c40", "137371a9bf16401292ba73448ea5dc26", "Vicious", "");
                    WeaponEnchants.Vorpal.UpdatePrefixSuffix("458fb279ad304c36bb2b4aa38ffd2edc", "216d60d23c754d62845c987f9ccf31ba", "Vorpal", "");
                }
                void PatchArmorEnchants() {
                    ArmorEnchants.AcidResistance10Enchant.UpdatePrefixSuffix("fda6a12008ac40dc9d42cf138af26f20", "6f3cca28f70d46639f0170dfbc6f0e0b", "Acid Resistant", "");
                    ArmorEnchants.AcidResistance15Enchant.UpdatePrefixSuffix("e7935d309ddb49bf83cf2a7912234e6f", "fbb1807132bb4a4390255155746d8863", "Acid Resistant", "");
                    ArmorEnchants.AcidResistance20Enchant.UpdatePrefixSuffix("803aefc6453942b2ba4cacd3cc2023c7", "6985b28b439b46cc88351eea8788aa17", "Acid Resistant", "");
                    ArmorEnchants.AcidResistance30Enchant.UpdatePrefixSuffix("819e074f6dbe4303bb0d8a130b215ad1", "d2a92538b0c940f0b886632888604423", "Acid Resistant", "");
                    ArmorEnchants.AdamantineArmorHeavyEnchant.UpdatePrefixSuffix("63d9d3f9e9ec448e94976f7aedac6cf0", "4baea6dbb7a9420eb28609d03c189ff9", "Adamantine", "");
                    ArmorEnchants.AdamantineArmorLightEnchant.UpdatePrefixSuffix("66a56349dbb14d5f9149f6e5c88c5d1f", "8712187ad02e452dbf7f5e6fdb9957aa", "Adamantine", "");
                    ArmorEnchants.AdamantineArmorMediumEnchant.UpdatePrefixSuffix("1ee44f3b093c41d8903294a083ab4e6b", "7d002a9347ea4d88a150a06a7cc2071f", "Adamantine", "");
                    ArmorEnchants.ArcaneArmorBalancedEnchant.UpdatePrefixSuffix("927c1232c66c454191c3ad2dfc504ba9", "6168ccb5390f4cf9a8192e5833efa2e9", "Balanced", "");
                    ArmorEnchants.ArcaneArmorInvulnerabilityEnchant.UpdatePrefixSuffix("e9423ddde54f48c79857fceb6cf38eb9", "ee40da918a264a8dbfdf4fa142adf9f3", "Invulnerability", "");
                    ArmorEnchants.ArcaneArmorShadowEnchant.UpdatePrefixSuffix("2e640669072b45ef93958a23e2e27bed", "ab522fffa216494d9b624ebbe97f7197", "Shadow", "");
                    ArmorEnchants.ArcaneArmorShadowGreaterEnchant.UpdatePrefixSuffix("845ca47c40854a71818692c19e3b7cd3", "72e1964a7d6c42e1b2596bc9d9b702dd", "Greater Shadow", "");
                    ArmorEnchants.ArmorEnhancementBonus1.UpdatePrefixSuffix("b6e8074d140c424583a0186b48be5436", "1ee242b6dc0e435a8c4af337d11a5208", "", "+1");
                    ArmorEnchants.ArmorEnhancementBonus2.UpdatePrefixSuffix("63738ab5e0bc4782b9fd17c6c3a41aeb", "d46cb43ae9354950abe6475b87c396d5", "", "+2");
                    ArmorEnchants.ArmorEnhancementBonus3.UpdatePrefixSuffix("7c3e247deb054ff3a900b1c7404d2f30", "2cd448ba87bc4e50bc81dcf9d55f235f", "", "+3");
                    ArmorEnchants.ArmorEnhancementBonus4.UpdatePrefixSuffix("ece2df167943438cb78c03d0c1ee7590", "28e2f29433e144d5afcd005f1c4d4257", "", "+4");
                    ArmorEnchants.ArmorEnhancementBonus5.UpdatePrefixSuffix("a6520dfbb1f14624b74e1298b9f3f267", "4cf24bfc6ff740c7afc9888698a5d932", "", "+5");
                    ArmorEnchants.ArmorEnhancementBonus6.UpdatePrefixSuffix("ade22255865148cea2e99108fc5bf1c4", "d07b3f22f22e45ff8c7e4cde3732c54e", "", "+6");
                    ArmorEnchants.ColdResistance10Enchant.UpdatePrefixSuffix("12d79d753e164726bfffdaad2434e7a9", "d9ed7311c4554d5ab2f5f3bf5008adc9", "Cold Resistant", "");
                    ArmorEnchants.ColdResistance15Enchant.UpdatePrefixSuffix("a9c88163b97b4d79b03d3469e040cf1d", "b0459a8ea96a45b3b7ca616a85550795", "Cold Resistant", "");
                    ArmorEnchants.ColdResistance20Enchant.UpdatePrefixSuffix("209710530b4e46adaa0062bf3b246f1c", "6eb8c03966094ece918a6bb8d6f948b8", "Cold Resistant", "");
                    ArmorEnchants.ColdResistance30Enchant.UpdatePrefixSuffix("387db70ba0034a789a708c5bb424ac22", "ce0845da5cf14476be4e38a47e6e81f2", "Cold Resistant", "");
                    ArmorEnchants.ElectricityResistance10Enchant.UpdatePrefixSuffix("e8f4c27bc18d48e298392713f5dac3db", "cca39ae87304460aaebfbb5b091d586f", "Shock Resistant", "");
                    ArmorEnchants.ElectricityResistance15Enchant.UpdatePrefixSuffix("06227e7b8e7b478789c9c5002d3f0d3d", "c8d9f07ecadf48dc91f7973b942c82f0", "Shock Resistant", "");
                    ArmorEnchants.ElectricityResistance20Enchant.UpdatePrefixSuffix("865ed2b71c7141eaa2f6d789a77abe7d", "0dab8a298f9a4d8e866c626ca1294120", "Shock Resistant", "");
                    ArmorEnchants.ElectricityResistance30Enchant.UpdatePrefixSuffix("98d674a48d094e0584d278cb0b373e64", "ffa5866cb3834d8391f9cff5e396ded5", "Shock Resistant", "");
                    ArmorEnchants.EnergyResistance10n10Enchant.UpdatePrefixSuffix("a1deb230990e427e9629b3a5f7abe1c5", "5fbc0f9d70004fb3899e85b9fcd1b6f7", "Energy Resistant", "");
                    ArmorEnchants.FireResistance10Enchant.UpdatePrefixSuffix("63ec9c8bd5cd4d80b103ed419ae41a7a", "008f3d4e6034432f8366437a7fc7876f", "Fire Resistant", "");
                    ArmorEnchants.FireResistance15Enchant.UpdatePrefixSuffix("478417907df64ca28b71fc77d34240ef", "9a146d7db25949de967f50057d18ccb8", "Fire Resistant", "");
                    ArmorEnchants.FireResistance20Enchant.UpdatePrefixSuffix("21c70dfeb80f4f82990dbab81371d774", "eb519dcf2b3147e9a41d1bec107b3864", "Fire Resistant", "");
                    ArmorEnchants.FireResistance30Enchant.UpdatePrefixSuffix("885e797f6c054ebb88276ed53b54b70c", "12a6c142dd9642f3b1be2ab3c533418b", "Fire Resistant", "");
                    ArmorEnchants.Fortification25Enchant.UpdatePrefixSuffix("1af3fbd470b6453d97316c2e18b7aa4a", "1971d92b87e94389bf84e84f67e59d9f", "Light Fortification", "");
                    ArmorEnchants.Fortification50Enchant.UpdatePrefixSuffix("98fe93f2945748d180e693bb46eaa2f1", "a47a12a11dd048859e84eca01be5a83a", "Fortification", "");
                    ArmorEnchants.Fortification75Enchant.UpdatePrefixSuffix("520e22cc16b346aab8aeb9b062d8df70", "c901206dffb2448a9020d77b8d66493b", "Heavy Fortification", "");
                    ArmorEnchants.GreaterShadow.UpdatePrefixSuffix("884c6afee5f4401681081a1da1a29057", "daf536c661e541059b03b0cbe6913817", "Greater Shadow", "");
                    ArmorEnchants.MithralArmorEnchant.UpdatePrefixSuffix("681d6e402fff4d1bb913caf75744d2e7", "d7393e300e3f4fe9bcebea649408f8d8", "Mithral", "");
                    ArmorEnchants.NegativeEnergyImmunty15PerEnchantment.UpdatePrefixSuffix("a86877febf5c4c5b8653c9bb564f331f", "338e87c9c91344eda24e58941a50ffcc", "Negative Energy Blocking", "");
                    ArmorEnchants.NegativeEnergyResistance10Enchant.UpdatePrefixSuffix("954d6b828e5b48da8d5a78dc1939343a", "b0d85fb211c442cbb43ca4da00445e64", "Negative Energy Resistant", "");
                    ArmorEnchants.NegativeEnergyResistance20Enchant.UpdatePrefixSuffix("fc1d514b6e8f4dc09f7b9a549f364caa", "5d6a909bfa654dbc825f2052c3acf442", "Negative Energy Resistant", "");
                    ArmorEnchants.NegativeEnergyResistance30Enchant.UpdatePrefixSuffix("5bdc87b59e1a49efae912d24fc3132ae", "1a7ee320f2434ea986734dbd204e11fb", "Negative Energy Resistant", "");
                    ArmorEnchants.PositiveEnergyResistance30Enchant.UpdatePrefixSuffix("92df002aeb88439bb71699fdb0c8831b", "d59213a7b51f436db7471a4648ff0fc3", "Positive Energy Resistant", "");
                    ArmorEnchants.ShadowArmor.UpdatePrefixSuffix("b93ee3e3ec2c4e519e53077c99b86d8b", "db810bdf5fb64ab494f30bab45dfe58d", "Shadow", "");
                    ArmorEnchants.ShieldEnhancementBonus1.UpdatePrefixSuffix("48fed8662b3444de822d89ec73f04228", "b1ea7ca42cb7446ca4ad4b46574545da", "", "+1");
                    ArmorEnchants.ShieldEnhancementBonus2.UpdatePrefixSuffix("dadee960e4f6484da80fb46d7dcd63a2", "56c4520916944dd5bcb88c82b52d3d61", "", "+2");
                    ArmorEnchants.ShieldEnhancementBonus3.UpdatePrefixSuffix("a0dba1c0c5a34618a93e8f8edc939f30", "0320e5e8092547f6aaa8ae7408d72202", "", "+3");
                    ArmorEnchants.ShieldEnhancementBonus4.UpdatePrefixSuffix("cfbe0c1f023342509a71fc7e625946db", "9f8923053e8846f389c7c2115a42e2da", "", "+4");
                    ArmorEnchants.ShieldEnhancementBonus5.UpdatePrefixSuffix("155d103e6f464e4988876758091f6b94", "a598cafc4d5143d1b3da1cfb59a63377", "", "+4");
                    ArmorEnchants.ShieldEnhancementBonus6.UpdatePrefixSuffix("4e74c77c177545b2853348837832b573", "aa8f47407ed445c789467e3a99394a6e", "", "+6");
                    ArmorEnchants.SonicResistance10Enchant.UpdatePrefixSuffix("dec943572cf94beba751cd9cd93edb04", "eb489ac0fd034fec8f247172cd3bd464", "Sonic Resistant", "");
                    ArmorEnchants.SonicResistance30Enchant.UpdatePrefixSuffix("674695189fcb4bfe8c41d94984164162", "4ba2af41700b4ad4b884e21f240810bf", "Sonic Resistant", "");
                    ArmorEnchants.SpellResistance13Enchant.UpdatePrefixSuffix("23cfda52f52f49caa16d970dc0fb358f", "8d5ceac6cbbf44b699e2835c01fbd5f4", "Spell Resistant", "");
                    ArmorEnchants.SpellResistance15Enchant.UpdatePrefixSuffix("126322ea121242a981ff9e61a636f880", "834e08fd82924c71b53391b4c38a7765", "Spell Resistant", "");
                    ArmorEnchants.SpellResistance17Enchant.UpdatePrefixSuffix("177418865efb403a96fde438c34bcb9b", "25eb4a21576747ba9a5528ea12fc0e57", "Spell Resistant", "");
                    ArmorEnchants.SpellResistance19Enchant.UpdatePrefixSuffix("f790d92c20bc45d79a40b84c0329a0c3", "bf716c6a017e423aa7573cc048e7ee94", "Spell Resistant", "");
                    ArmorEnchants.TemporaryArmorEnhancementBonus1.UpdatePrefixSuffix("61510483d29b4662bd7804c3fa25a308", "895850a2171641a6b68b70168b45dad7", "", "+1");
                    ArmorEnchants.TemporaryArmorEnhancementBonus2.UpdatePrefixSuffix("2c7205dde3ae4d8ea93c4b4fcf7951c5", "de5bb578a40545fd934c00a073fbb195", "", "+2");
                    ArmorEnchants.TemporaryArmorEnhancementBonus3.UpdatePrefixSuffix("2bee847769f843dcbd73c95aa86d30a8","7a9d07e39c094d82a3b0b044b97e5b4c", "", "+3");
                    ArmorEnchants.TemporaryArmorEnhancementBonus4.UpdatePrefixSuffix("0fcacfd4ffac4fa2bab74932654969cd", "ea4d5bb74b544632bf3540b371261231", "", "+4");
                    ArmorEnchants.TemporaryArmorEnhancementBonus5.UpdatePrefixSuffix("0ed27ef5589a4aab9490d16857487c85", "fbfcff8928ca4deeab08d1398290dbc3", "", "+5");
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
