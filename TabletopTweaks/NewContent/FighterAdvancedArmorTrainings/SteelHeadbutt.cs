using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
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

            var SteelHeadbutt1d3 = Helpers.CreateCopy(Gore1d6, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("SteelHeadbutt1d3");
                bp.name = "SteelHeadbutt1d3";
                bp.m_DisplayNameText = Helpers.CreateString($"{bp.name}.Description", "Steel Headbutt");
                bp.m_OverrideDamageType = true;
                bp.m_DamageType.Physical.Form = PhysicalDamageForm.Bludgeoning;
                bp.m_OverrideDamageDice = true;
                bp.m_DamageDice = new DiceFormula(1, DiceType.D3);
            });
            var SteelHeadbutt1d4 = Helpers.CreateCopy(Gore1d6, bp => {
                bp.AssetGuid = ModSettings.Blueprints.GetGUID("SteelHeadbutt1d4");
                bp.name = "SteelHeadbutt1d4";
                bp.m_DisplayNameText = Helpers.CreateString($"{bp.name}.Description", "Steel Headbutt");
                bp.m_OverrideDamageType = true;
                bp.m_DamageType.Physical.Form = PhysicalDamageForm.Bludgeoning;
                bp.m_OverrideDamageDice = true;
                bp.m_DamageDice = new DiceFormula(1, DiceType.D4);
            });
            var SteelHeadbuttMediumEffect = Helpers.CreateBlueprint<BlueprintFeature>("SteelHeadbuttMediumEffect", bp => {
                bp.SetName("Steel Headbutt Effect");
                bp.SetDescription("Steel Headbutt");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent<AddSecondaryAttacks>(c => {
                    c.m_Weapon = new BlueprintItemWeaponReference[] { SteelHeadbutt1d3.ToReference<BlueprintItemWeaponReference>() };
                });
                bp.AddComponent<ApplyArmorEnchantsToWeapon>(c => {
                    c.Weapon = SteelHeadbutt1d3.ToReference<BlueprintItemWeaponReference>();
                });
            });
            var SteelHeadbuttHeavyEffect = Helpers.CreateBlueprint<BlueprintFeature>("SteelHeadbuttHeavyEffect", bp => {
                bp.SetName("Steel Headbutt Effect");
                bp.SetDescription("Steel Headbutt");
                bp.IsClassFeature = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
                bp.AddComponent<AddSecondaryAttacks>(c => {
                    c.m_Weapon = new BlueprintItemWeaponReference[] { SteelHeadbutt1d4.ToReference<BlueprintItemWeaponReference>() };
                });
                bp.AddComponent<ApplyArmorEnchantsToWeapon>(c => {
                    c.Weapon = SteelHeadbutt1d4.ToReference<BlueprintItemWeaponReference>();
                });
            });
            var SteelHeadbuttFeature = Helpers.CreateBlueprint<BlueprintFeature>("SteelHeadbuttFeature", bp => {
                bp.SetName("Steel Headbutt");
                bp.SetDescription("While wearing medium or heavy armor, a fighter can deliver a headbutt with his helm as part of a full attack action. " +
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
