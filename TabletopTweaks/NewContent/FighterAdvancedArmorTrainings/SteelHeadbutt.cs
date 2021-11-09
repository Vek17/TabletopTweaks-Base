using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.FighterAdvancedArmorTrainings {
    class SteelHeadbutt {
        public static void AddSteelHeadbutt() {
            var FighterClass = Resources.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var Gore1d6 = Resources.GetBlueprint<BlueprintItemWeapon>("daf4ab765feba8548b244e174e7af5be");

            var SteelHeadbuttEnchant = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"SteelHeadbuttEnchant", bp => {
                bp.SetName("c15adc6e074442b0a5c982a10602dfed", "Steel Headbutt");
                bp.SetDescription("b334af7fd3224124993ba1d2e8cc0136", "Weapon uses armor enchants and material.");
                bp.SetPrefix("b75208f524de4c63b2bd275c70f27c13", "");
                bp.SetSuffix("4e672d9e78b64cac9c210972c83afe83", "");
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<ArmorEnchantsToWeapon>();
            });
            var SteelHeadbutt1d3 = Helpers.CreateCopy(Gore1d6, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("SteelHeadbutt1d3");
                bp.name = "SteelHeadbutt1d3";
                bp.m_DisplayNameText = Helpers.CreateString("69c372480c5a44dcb6dfd4929b339e3c", $"{bp.name}.Description", "Steel Headbutt");
                bp.m_OverrideDamageType = true;
                bp.m_DamageType.Physical.Form = PhysicalDamageForm.Bludgeoning;
                bp.m_OverrideDamageDice = true;
                bp.m_DamageDice = new DiceFormula(1, DiceType.D3);
                bp.m_Enchantments = new BlueprintWeaponEnchantmentReference[] { SteelHeadbuttEnchant.ToReference<BlueprintWeaponEnchantmentReference>() };
            });
            var SteelHeadbutt1d4 = Helpers.CreateCopy(Gore1d6, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("SteelHeadbutt1d4");
                bp.name = "SteelHeadbutt1d4";
                bp.m_DisplayNameText = Helpers.CreateString("880b88c78e0b466281d040cb92af6d4e", $"{bp.name}.Description", "Steel Headbutt");
                bp.m_OverrideDamageType = true;
                bp.m_DamageType.Physical.Form = PhysicalDamageForm.Bludgeoning;
                bp.m_OverrideDamageDice = true;
                bp.m_DamageDice = new DiceFormula(1, DiceType.D4);
                bp.m_Enchantments = new BlueprintWeaponEnchantmentReference[] { SteelHeadbuttEnchant.ToReference<BlueprintWeaponEnchantmentReference>() };
            });
            var SteelHeadbuttMediumEffect = Helpers.CreateBlueprint<BlueprintFeature>("SteelHeadbuttMediumEffect", bp => {
                bp.SetName("c1d4efb32180421e9fe24660d0800a1e", "Steel Headbutt Effect");
                bp.SetDescription("d29c9ba6e806422ca88aee237d81c6db", "Steel Headbutt");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent<AddSecondaryAttacks>(c => {
                    c.m_Weapon = new BlueprintItemWeaponReference[] { SteelHeadbutt1d3.ToReference<BlueprintItemWeaponReference>() };
                });
            });
            var SteelHeadbuttHeavyEffect = Helpers.CreateBlueprint<BlueprintFeature>("SteelHeadbuttHeavyEffect", bp => {
                bp.SetName("b107fd4a5f684e1289de80d52b2d32db", "Steel Headbutt Effect");
                bp.SetDescription("e525ac95a7a94e29ac81d8573a893791", "Steel Headbutt");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent<AddSecondaryAttacks>(c => {
                    c.m_Weapon = new BlueprintItemWeaponReference[] { SteelHeadbutt1d4.ToReference<BlueprintItemWeaponReference>() };
                });
            });
            var SteelHeadbuttFeature = Helpers.CreateBlueprint<BlueprintFeature>("SteelHeadbuttFeature", bp => {
                bp.SetName("d84806b451af4a06840bd8ed2734ded4", "Steel Headbutt");
                bp.SetDescription("19310cd2f3e547f588dc8c4bf040f7ff", "While wearing medium or heavy armor, a fighter can deliver a headbutt with his helm as part of a full attack action. " +
                    "This headbutt is in addition to his normal attacks, and is made using the fighter’s base attack bonus – 5. A helmet headbutt deals " +
                    "1d3 points of damage if the fighter is wearing medium armor, or 1d4 points of damage if he is wearing heavy armor (1d2 and 1d3, " +
                    "respectively, for Small creatures), plus an amount of damage equal to 1/2 the fighter’s Strength modifier. Treat this attack as a " +
                    "weapon attack made using the same special material and echantment bonus (if any) as the armor.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = SteelHeadbuttMediumEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Medium };
                });
                bp.AddComponent<ArmorFeatureUnlock>(c => {
                    c.NewFact = SteelHeadbuttHeavyEffect.ToReference<BlueprintUnitFactReference>();
                    c.RequiredArmor = new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy };
                });
            });

            Resources.AddBlueprint(SteelHeadbutt1d3);
            Resources.AddBlueprint(SteelHeadbutt1d4);
            if (ModSettings.AddedContent.FighterAdvancedArmorTraining.IsDisabled("SteelHeadbutt")) { return; }
            AdvancedArmorTraining.AddToAdvancedArmorTrainingSelection(SteelHeadbuttFeature);
        }
    }
}
