using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewContent.MechanicsChanges;
using UnityEngine;

namespace TabletopTweaks.Utilities {
    public static class ItemTools {

        public enum MetamagicRodType: int { 
            Lesser = 3,
            Normal = 6,
            Greater = 9
        }
        private static readonly string LesserMetamagicRodString = "Lesser rods can be used with spells of 3rd level or lower.";
        private static readonly string NormalMetamagicRodString = "Regular rods can be used with spells of 6th level or lower.";
        private static readonly string GreaterMetamagicRodString = "Greater rods can be used with spells of 9th level or lower.";
        private static BlueprintItemEquipmentUsable CreateMetamagicRod(
            string rodName,
            Sprite icon,
            Metamagic metamagic,
            string metamagicName,
            MetamagicRodType type,
            string rodDescriptionStart,
            string metamagicDescription,
            Action<BlueprintItemEquipmentUsable> init = null
        ) {
            var description = $"{rodDescriptionStart}\n{GetRodString(type)}\n{metamagicDescription}";

            var Buff = Helpers.CreateBuff($"MetamagicRod{type}{metamagicName}Buff", bp => {
                bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
                bp.ResourceAssetIds = new string[0];
                bp.SetName(rodName);
                bp.SetDescription(description);
                bp.m_DescriptionShort = Helpers.CreateString($"{bp.name}.Description_Short", "");
                bp.m_Icon = icon;
            });
            var ActivatableAbility = Helpers.CreateBlueprint<BlueprintActivatableAbility>($"MetamagicRod{type}{metamagicName}ToggleAbility", bp => {
                bp.m_Buff = Buff.ToReference<BlueprintBuffReference>();
                bp.m_SelectTargetAbility = new BlueprintAbilityReference();
                bp.Group = ActivatableAbilityGroup.MetamagicRod;
                bp.WeightInGroup = 1;
                bp.DeactivateImmediately = true;
                bp.ResourceAssetIds = new string[0];
                bp.SetName(rodName);
                bp.SetDescription(description);
                bp.m_DescriptionShort = Helpers.CreateString($"{bp.name}.Description_Short", "");
                bp.m_Icon = icon;
                bp.AddComponent<ActivatableAbilityResourceLogic>(c => {
                    c.m_RequiredResource = new BlueprintAbilityResourceReference();
                    c.m_FreeBlueprint = new BlueprintUnitFactReference();
                    c.Categories = new Kingmaker.Enums.WeaponCategory[0];
                });
            });
            Buff.AddComponent<MetamagicRodMechanics>(c => {
                c.m_RodAbility = ActivatableAbility.ToReference<BlueprintActivatableAbilityReference>();
                c.m_AbilitiesWhiteList = new BlueprintAbilityReference[0];
                c.Metamagic = metamagic;
                c.MaxSpellLevel = (int)type;
            });
            var MetamagicRod = Helpers.CreateBlueprint<BlueprintItemEquipmentUsable>($"MetamagicRod{type}{metamagicName}", bp => {
                bp.m_InventoryEquipSound = "WandPut";
                bp.m_BeltItemPrefab = new Kingmaker.ResourceLinks.PrefabLink();
                bp.m_Enchantments = new BlueprintEquipmentEnchantmentReference[0];
                bp.m_Ability = new BlueprintAbilityReference();
                bp.m_ActivatableAbility = ActivatableAbility.ToReference<BlueprintActivatableAbilityReference>();
                bp.m_EquipmentEntity = new KingmakerEquipmentEntityReference();
                bp.m_EquipmentEntityAlternatives = new KingmakerEquipmentEntityReference[0];
                bp.SpendCharges = true;
                bp.Charges = 3;
                bp.RestoreChargesOnRest = true;
                bp.m_DisplayNameText = Helpers.CreateString($"{bp.name}.Name", rodName);
                bp.m_DescriptionText = Helpers.CreateString($"{bp.name}.Description", description, shouldProcess: true);
                bp.m_FlavorText = Helpers.CreateString($"{bp.name}.Flavor", "");
                bp.m_NonIdentifiedNameText = Helpers.CreateString($"{bp.name}.Unidentified_Name", "Rod");
                bp.m_NonIdentifiedDescriptionText = Helpers.CreateString($"{bp.name}.Unidentified_Description", "");
                bp.m_Icon = icon;
                bp.m_Cost = GetRodCost(metamagic, type);
                bp.m_Weight = 1;
                bp.m_Destructible = true;
                bp.m_ShardItem = Resources.GetBlueprintReference<BlueprintItemReference>("e6820e62423d4c81a2ba20d236251b67"); //MetalShardItem
                bp.m_InventoryPutSound = "WandPut";
                bp.m_InventoryTakeSound = "WandTake";
                bp.TrashLootTypes = new Kingmaker.Enums.TrashLootType[0];
                bp.m_Overrides = new List<string>();
            });
            return MetamagicRod;

            string GetRodString(MetamagicRodType type) {
                switch (type) {
                    case MetamagicRodType.Lesser:
                        return LesserMetamagicRodString;
                    case MetamagicRodType.Normal:
                        return NormalMetamagicRodString;
                    case MetamagicRodType.Greater:
                        return GreaterMetamagicRodString;
                    default:
                        return string.Empty;
                }
            }

            int GetRodCost(Metamagic metamagic, MetamagicRodType type) {
                switch (type) {
                    case MetamagicRodType.Lesser:
                        switch (metamagic.DefaultCost()) {
                            case 0:
                                return 3000;
                            case 1:
                                return 3000;
                            case 2:
                                return 9000;
                            case 3:
                                return 14000;
                            case 4:
                                return 35000;
                            default:
                                return 70000;
                        }
                    case MetamagicRodType.Normal:
                        switch (metamagic.DefaultCost()) {
                            case 0:
                                return 11000;
                            case 1:
                                return 11000;
                            case 2:
                                return 32500;
                            case 3:
                                return 54000;
                            case 4:
                                return 75500;
                            default:
                                return 15000;
                        }
                    case MetamagicRodType.Greater:
                        switch (metamagic.DefaultCost()) {
                            case 0:
                                return 24500;
                            case 1:
                                return 24500;
                            case 2:
                                return 73000;
                            case 3:
                                return 121500;
                            case 4:
                                return 170000;
                            default:
                                return 300000;
                        }
                    default:
                        return 0;
                }
            }
        }
        public static BlueprintItemEquipmentUsable CreateMetamagicRod(
            string rodName,
            Sprite icon,
            Metamagic metamagic,
            MetamagicRodType type,
            string rodDescriptionStart,
            string metamagicDescription,
            Action<BlueprintItemEquipmentUsable> init = null
        ) {
            if (metamagic.IsNewMetamagic()) {
                return CreateMetamagicRod(
                    rodName,
                    icon,
                    metamagic,
                    ((MetamagicExtention.CustomMetamagic)metamagic).ToString(),
                    type,
                    rodDescriptionStart,
                    metamagicDescription
                );
            }
            return CreateMetamagicRod(
                rodName,
                icon,
                metamagic,
                metamagic.ToString(),
                type,
                rodDescriptionStart,
                metamagicDescription
            );
        }
        public static BlueprintItemEquipmentUsable[] CreateAllMetamagicRods(
            string rodName,
            Sprite lesserIcon,
            Sprite normalIcon,
            Sprite greaterIcon,
            Metamagic metamagic,
            string rodDescriptionStart,
            string metamagicDescription) {

            return new BlueprintItemEquipmentUsable[] {
                CreateMetamagicRod(
                    $"Lesser {rodName}",
                    lesserIcon,
                    metamagic,
                    type: MetamagicRodType.Lesser,
                    rodDescriptionStart,
                    metamagicDescription
                ),
                CreateMetamagicRod(
                    rodName,
                    normalIcon,
                    metamagic,
                    type: MetamagicRodType.Normal,
                    rodDescriptionStart,
                    metamagicDescription
                ),
                CreateMetamagicRod(
                    $"Greater {rodName}",
                    greaterIcon,
                    metamagic,
                    type: MetamagicRodType.Greater,
                    rodDescriptionStart,
                    metamagicDescription
                )
            };
        }
    }
}
