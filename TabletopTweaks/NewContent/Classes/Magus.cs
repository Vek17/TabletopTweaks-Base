using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.Classes {
    static class Magus {
        public static void AddMagusFeatures() {
            var Icon_WarriorSpirit_FlamingBurst = AssetLoader.LoadInternal("Abilities", "Icon_WarriorSpirit_FlamingBurst.png");
            var Icon_WarriorSpirit_IcyBurst = AssetLoader.LoadInternal("Abilities", "Icon_WarriorSpirit_IcyBurst.png");
            var Icon_WarriorSpirit_ShockingBurst = AssetLoader.LoadInternal("Abilities", "Icon_WarriorSpirit_ShockingBurst.png");

            var FlamingBurst = Resources.GetBlueprint<BlueprintWeaponEnchantment>("3f032a3cd54e57649a0cdad0434bf221");
            var IcyBurst = Resources.GetBlueprint<BlueprintWeaponEnchantment>("564a6924b246d254c920a7c44bf2a58b");
            var ShockingBurst = Resources.GetBlueprint<BlueprintWeaponEnchantment>("914d7ee77fb09d846924ca08bccee0ff");

            var FlamingBurst_ArcaneWeapon_TTT = Helpers.CreateCopy(FlamingBurst, bp => {
                bp.name = "FlamingBurst_ArcaneWeapon_TTT";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_EnchantmentCost = 1;
            });
            Resources.AddBlueprint(FlamingBurst_ArcaneWeapon_TTT);
            var IcyBurst_ArcaneWeapon_TTT = Helpers.CreateCopy(IcyBurst, bp => {
                bp.name = "IcyBurst_ArcaneWeapon_TTT";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_EnchantmentCost = 1;
            });
            Resources.AddBlueprint(IcyBurst_ArcaneWeapon_TTT);
            var ShockingBurst_ArcaneWeapon_TTT = Helpers.CreateCopy(ShockingBurst, bp => {
                bp.name = "ShockingBurst_ArcaneWeapon_TTT";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_EnchantmentCost = 1;
            });
            Resources.AddBlueprint(ShockingBurst_ArcaneWeapon_TTT);

            var ArcaneWeaponFlamingBurstBuff_TTT = Helpers.CreateBuff("ArcaneWeaponFlamingBurstBuff_TTT", bp => {
                bp.SetName("Flaming Burst");
                bp.SetDescription("");
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_WarriorSpirit_FlamingBurst;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.AddComponent<AddBondProperty>(c => {
                    c.m_Enchant = FlamingBurst_ArcaneWeapon_TTT.ToReference<BlueprintItemEnchantmentReference>();
                    c.EnchantPool = EnchantPoolType.ArcanePool;
                });
            });
            var ArcaneWeaponFlamingBurstChoice_TTT = Helpers.CreateBlueprint<BlueprintActivatableAbility>("ArcaneWeaponFlamingBurstChoice_TTT", bp => {
                bp.SetName("Arcane Weapon — Flaming Burst");
                bp.SetDescription("A flaming burst weapon explodes with flame upon striking a successful critical hit. " +
                    "The fire does not harm the wielder. A flaming burst weapon deals an extra 1d10 points of fire damage" +
                    " on a successful critical hit. Add an extra 1d10 points of fire damage for every critical multiplier beyond 2.");
                bp.m_Icon = Icon_WarriorSpirit_FlamingBurst;
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.Group = ActivatableAbilityGroup.ArcaneWeaponProperty;
                bp.m_Buff = ArcaneWeaponFlamingBurstBuff_TTT.ToReference<BlueprintBuffReference>();
            });

            var ArcaneWeaponIcyBurstBuff_TTT = Helpers.CreateBuff("ArcaneWeaponIcyBurstBuff_TTT", bp => {
                bp.SetName("Icy Burst");
                bp.SetDescription("");
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_WarriorSpirit_IcyBurst;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.AddComponent<AddBondProperty>(c => {
                    c.m_Enchant = IcyBurst_ArcaneWeapon_TTT.ToReference<BlueprintItemEnchantmentReference>();
                    c.EnchantPool = EnchantPoolType.ArcanePool;
                });
            });
            var ArcaneWeaponIcyBurstChoice_TTT = Helpers.CreateBlueprint<BlueprintActivatableAbility>("ArcaneWeaponIcyBurstChoice_TTT", bp => {
                bp.SetName("Arcane Weapon — Icy Burst");
                bp.SetDescription("A icy burst weapon explodes with ice upon striking a successful critical hit. " +
                    "The cold does not harm the wielder. An icy burst weapon deals an extra 1d10 points of cold damage" +
                    " on a successful critical hit. Add an extra 1d10 points of cold damage for every critical multiplier beyond 2.");
                bp.m_Icon = Icon_WarriorSpirit_IcyBurst;
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.Group = ActivatableAbilityGroup.ArcaneWeaponProperty;
                bp.m_Buff = ArcaneWeaponIcyBurstBuff_TTT.ToReference<BlueprintBuffReference>();
            });

            var ArcaneWeaponShockingBurstBuff_TTT = Helpers.CreateBuff("ArcaneWeaponShockingBurstBuff_TTT", bp => {
                bp.SetName("Shocking Burst");
                bp.SetDescription("");
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_WarriorSpirit_ShockingBurst;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.AddComponent<AddBondProperty>(c => {
                    c.m_Enchant = ShockingBurst_ArcaneWeapon_TTT.ToReference<BlueprintItemEnchantmentReference>();
                    c.EnchantPool = EnchantPoolType.ArcanePool;
                });
            });
            var ArcaneWeaponShockingBurstChoice_TTT = Helpers.CreateBlueprint<BlueprintActivatableAbility>("ArcaneWeaponShockingBurstChoice_TTT", bp => {
                bp.SetName("Arcane Weapon — Shocking Burst");
                bp.SetDescription("A shocking burst weapon explodes with electricity upon striking a successful critical hit. " +
                    "The electricity does not harm the wielder. A shocking burst weapon deals an extra 1d10 points of electricity damage" +
                    " on a successful critical hit. Add an extra 1d10 points of electricity damage for every critical multiplier beyond 2.");
                bp.m_Icon = Icon_WarriorSpirit_ShockingBurst;
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.Group = ActivatableAbilityGroup.ArcaneWeaponProperty;
                bp.m_Buff = ArcaneWeaponShockingBurstBuff_TTT.ToReference<BlueprintBuffReference>();
            });
        }
    }
}
