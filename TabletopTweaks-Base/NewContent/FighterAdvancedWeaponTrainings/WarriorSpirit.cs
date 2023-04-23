using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.FighterAdvancedWeaponTrainings {
    static class WarriorSpirit {
        public static void AddWarriorSpirit() {
            var FighterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var WeaponBondSwitchAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("7ff088ab58c69854b82ea95c2b0e35b4"); //For fx

            var Icon_WarriorSpirit_Activation = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Activation.png");
            var Icon_WarriorSpirit_Anarchic = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Anarchic.png");
            var Icon_WarriorSpirit_Axiomatic = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Axiomatic.png");
            var Icon_WarriorSpirit_Bane = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Bane.png");
            var Icon_WarriorSpirit_BrilliantEnergy = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_BrilliantEnergy.png");
            var Icon_WarriorSpirit_Corrosive = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Corrosive.png");
            var Icon_WarriorSpirit_CorrosiveBurst = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_CorrosiveBurst.png");
            var Icon_WarriorSpirit_Flaming = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Flaming.png");
            var Icon_WarriorSpirit_FlamingBurst = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_FlamingBurst.png");
            var Icon_WarriorSpirit_Frost = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Frost.png");
            var Icon_WarriorSpirit_GhostTouch = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_GhostTouch.png");
            var Icon_WarriorSpirit_Holy = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Holy.png");
            var Icon_WarriorSpirit_IcyBurst = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_IcyBurst.png");
            var Icon_WarriorSpirit_Shock = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Shock.png");
            var Icon_WarriorSpirit_ShockingBurst = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_ShockingBurst.png");
            var Icon_WarriorSpirit_Thundering = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Thundering.png");
            var Icon_WarriorSpirit_ThunderingBurst = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_ThunderingBurst.png");
            var Icon_WarriorSpirit_Speed = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Speed.png");
            var Icon_WarriorSpirit_Unholy = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Unholy.png");
            var Icon_WarriorSpirit_Keen = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Keen.png");
            var Icon_WarriorSpirit_Cruel = AssetLoader.LoadInternal(TTTContext, folder: "Abilities", file: "Icon_WarriorSpirit_Cruel.png");

            var Anarchic = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("57315bc1e1f62a741be0efde688087e9");
            var Axiomatic = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("0ca43051edefcad4b9b2240aa36dc8d4");
            var Holy = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("28a9964d81fedae44bae3ca45710c140");
            var Unholy = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("d05753b8df780fc4bb55b318f06af453");

            var Corrosive = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("633b38ff1d11de64a91d490c683ab1c8");
            var CorrosiveBurst = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("0cf34703e67e37b40905845ca14b1380");
            var Flaming = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("30f90becaaac51f41bf56641966c4121");
            var FlamingBurst = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("3f032a3cd54e57649a0cdad0434bf221");
            var Frost = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("421e54078b7719d40915ce0672511d0b");
            var IcyBurst = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("564a6924b246d254c920a7c44bf2a58b");
            var Shock = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("7bda5277d36ad114f9f9fd21d0dab658");
            var ShockingBurst = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("914d7ee77fb09d846924ca08bccee0ff");
            var Thundering = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("690e762f7704e1f4aa1ac69ef0ce6a96");
            var ThunderingBurst = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("83bd616525288b34a8f34976b2759ea1");

            var CruelEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("629c383ffb407224398bb71d1bd95d14");
            var Keen = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("102a9c8c9b7a75e4fb5844e79deaf4c0");
            var GhostTouch = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("47857e1a5a3ec1a46adf6491b1423b4f");
            var BaneEverything = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("1a93ab9c46e48f3488178733be29342a");

            var BrilliantEnergy = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("66e9e299c9002ea4bb65b6f300e43770");
            var Speed = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("f1c0c50108025d546b2554674ea1c006");

            var TemporaryEnhancement1 = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("d704f90f54f813043a525f304f6c0050");
            var TemporaryEnhancement2 = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("9e9bab3020ec5f64499e007880b37e52");
            var TemporaryEnhancement3 = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("d072b841ba0668846adeb007f623bd6c");
            var TemporaryEnhancement4 = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("6a6a0901d799ceb49b33d4851ff72132");
            var TemporaryEnhancement5 = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("746ee366e50611146821d61e391edf16");

            var WarriorSpiritResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(TTTContext, "WarriorSpiritResource", bp => {
                bp.m_Icon = Icon_WarriorSpirit_Activation;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount() {
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    BaseValue = 1
                };
            });

            var WarriorSpiritResourceIncrease = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WarriorSpiritResourceIncrease", bp => {
                bp.SetName(TTTContext, "");
                bp.SetDescription(TTTContext, "");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[0];
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.AddComponent<IncreaseResourceAmountByWeaponTraining>(c => {
                    c.m_Resource = WarriorSpiritResource.ToReference<BlueprintAbilityResourceReference>();
                });
            });

            var WarriorSpiritDurationBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "WarriorSpiritDurationBuff", bp => {
                bp.IsClassFeature = true;
                bp.SetName(TTTContext, "Warrior Spirit");
                bp.SetDescription(TTTContext, "");
                bp.Ranks = 1;
                bp.m_Icon = Icon_WarriorSpirit_Activation;
            });

            var WarriorSpiritToggleAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, "WarriorSpiritToggleAbility", bp => {
                var effect1 = WeaponBondSwitchAbility.GetComponents<AbilitySpawnFx>().ToArray()[0];
                var effect2 = WeaponBondSwitchAbility.GetComponents<AbilitySpawnFx>().ToArray()[1];
                bp.SetName(TTTContext, "Warrior Spirit");
                bp.SetDescription(TTTContext, "The fighter can forge a spiritual bond with a weapon that belongs to the associated weapon group, allowing him to " +
                    "unlock the weapon’s potential. Each day he gains a number of points of spiritual energy equal to " +
                    "1 + his maximum weapon training bonus. While wielding a weapon he has weapon training with, he can spend 1 point of spiritual " +
                    "energy to grant the weapon an enhancement bonus equal to his weapon training bonus. " +
                    "Enhancement bonuses gained by this advanced weapon training option stack with those of the weapon, to a maximum of +5. " +
                    "The fighter can also imbue the weapon with any one weapon special " +
                    "ability with an equivalent enhancement bonus less than or equal to his maximum bonus by reducing the granted enhancement " +
                    "bonus by the amount of the equivalent enhancement bonus. These bonuses last for 1 minute.");
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.LocalizedDuration", "1 Minute");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.m_Icon = Icon_WarriorSpirit_Activation;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.NeedEquipWeapons = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon;
                bp.ResourceAssetIds = WeaponBondSwitchAbility.ResourceAssetIds;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionWarriorSpiritEnhance() {
                            DefaultEnchantments = new BlueprintItemEnchantmentReference[] {
                                TemporaryEnhancement1.ToReference<BlueprintItemEnchantmentReference>(),
                                TemporaryEnhancement2.ToReference<BlueprintItemEnchantmentReference>(),
                                TemporaryEnhancement3.ToReference<BlueprintItemEnchantmentReference>(),
                                TemporaryEnhancement4.ToReference<BlueprintItemEnchantmentReference>(),
                                TemporaryEnhancement5.ToReference<BlueprintItemEnchantmentReference>()
                            },
                            DurationValue = new ContextDurationValue() {
                                m_IsExtendable = true,
                                Rate = DurationRate.Minutes,
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            }
                        },
                        new ContextActionApplyBuff() {
                            IsNotDispelable = true,
                            DurationValue = new ContextDurationValue() {
                                m_IsExtendable = true,
                                Rate = DurationRate.Minutes,
                                DiceCountValue = new ContextValue(),
                                BonusValue = 1
                            },
                            m_Buff = WarriorSpiritDurationBuff.ToReference<BlueprintBuffReference>()
                        }
                    );
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = effect1.PrefabLink;
                    c.Time = effect1.Time;
                    c.WeaponTarget = effect1.WeaponTarget;
                    c.Delay = effect1.Delay;
                    c.PositionAnchor = effect1.PositionAnchor;
                    c.OrientationAnchor = effect1.PositionAnchor;
                });
                bp.AddComponent<AbilitySpawnFx>(c => {
                    c.PrefabLink = effect2.PrefabLink;
                    c.Time = effect2.Time;
                    c.WeaponTarget = effect2.WeaponTarget;
                    c.Delay = effect2.Delay;
                    c.PositionAnchor = effect2.PositionAnchor;
                    c.OrientationAnchor = effect2.PositionAnchor;
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = WarriorSpiritResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                });
                bp.AddComponent<NestedPseudoActivatableAbilities>(c => {
                    c.m_Variants = new BlueprintAbilityReference[0];
                });
                bp.AddComponent<UpdateSlotsOnEquipmentChange>();
                bp.AddComponent<AbilityRequirementHasWeaponTrainingWithWeapon>();
                bp.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.VariantsBase;
                    c.m_ActiveWhenVariantActive = false;
                });
            });

            CreateWarriorSpiritToggle("WarriorSpiritAnarchic", Icon_WarriorSpirit_Anarchic, WarriorSpiritToggleAbility, 2, Anarchic);
            CreateWarriorSpiritToggle("WarriorSpiritAxiomatic", Icon_WarriorSpirit_Axiomatic, WarriorSpiritToggleAbility, 2, Axiomatic);
            CreateWarriorSpiritToggle("WarriorSpiritHoly", Icon_WarriorSpirit_Holy, WarriorSpiritToggleAbility, 2, Holy);
            CreateWarriorSpiritToggle("WarriorSpiritUnholy", Icon_WarriorSpirit_Unholy, WarriorSpiritToggleAbility, 2, Unholy);

            CreateWarriorSpiritToggle("WarriorSpiritCorrosive", Icon_WarriorSpirit_Corrosive, WarriorSpiritToggleAbility, 1, Corrosive);
            CreateWarriorSpiritToggle("WarriorSpiritCorrosiveBurst", Icon_WarriorSpirit_CorrosiveBurst, WarriorSpiritToggleAbility, 2, CorrosiveBurst, Corrosive);
            CreateWarriorSpiritToggle("WarriorSpiritFlaming", Icon_WarriorSpirit_Flaming, WarriorSpiritToggleAbility, 1, Flaming);
            CreateWarriorSpiritToggle("WarriorSpiritFlamingBurst", Icon_WarriorSpirit_FlamingBurst, WarriorSpiritToggleAbility, 2, FlamingBurst, Flaming);
            CreateWarriorSpiritToggle("WarriorSpiritFrost", Icon_WarriorSpirit_Frost, WarriorSpiritToggleAbility, 1, Frost);
            CreateWarriorSpiritToggle("WarriorSpiritIcyBurst", Icon_WarriorSpirit_IcyBurst, WarriorSpiritToggleAbility, 2, IcyBurst, Frost);
            CreateWarriorSpiritToggle("WarriorSpiritShock", Icon_WarriorSpirit_Shock, WarriorSpiritToggleAbility, 1, Shock);
            CreateWarriorSpiritToggle("WarriorSpiritShockingBurst", Icon_WarriorSpirit_ShockingBurst, WarriorSpiritToggleAbility, 2, ShockingBurst, Shock);
            CreateWarriorSpiritToggle("WarriorSpiritThundering", Icon_WarriorSpirit_Thundering, WarriorSpiritToggleAbility, 1, Thundering);
            CreateWarriorSpiritToggle("WarriorSpiritThunderingBurst", Icon_WarriorSpirit_ThunderingBurst, WarriorSpiritToggleAbility, 2, ThunderingBurst, Thundering);

            CreateWarriorSpiritToggle("WarriorSpiritCruel", Icon_WarriorSpirit_Cruel, WarriorSpiritToggleAbility, 1, CruelEnchantment);
            CreateWarriorSpiritToggle("WarriorSpiritKeen", Icon_WarriorSpirit_Keen, WarriorSpiritToggleAbility, 1, Keen);
            CreateWarriorSpiritToggle("WarriorSpiritGhostTouch", Icon_WarriorSpirit_GhostTouch, WarriorSpiritToggleAbility, 1, GhostTouch);

            CreateWarriorSpiritToggle("WarriorSpiritBane", Icon_WarriorSpirit_Bane, WarriorSpiritToggleAbility, 2, BaneEverything);
            CreateWarriorSpiritToggle("WarriorSpiritSpeed", Icon_WarriorSpirit_Speed, WarriorSpiritToggleAbility, 3, Speed);
            CreateWarriorSpiritToggle("WarriorSpiritBrilliantEnergy", Icon_WarriorSpirit_BrilliantEnergy, WarriorSpiritToggleAbility, 4, BrilliantEnergy);

            var WarriorSpiritFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "WarriorSpiritFeature", bp => {
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.WeaponTraining };
                bp.m_DisplayName = WarriorSpiritToggleAbility.m_DisplayName;
                bp.m_Description = WarriorSpiritToggleAbility.m_Description;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        WarriorSpiritToggleAbility.ToReference<BlueprintUnitFactReference>(),
                        WarriorSpiritResourceIncrease.ToReference<BlueprintUnitFactReference>()
                    };
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = WarriorSpiritResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = FighterClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 9;
                });
            });

            if (TTTContext.AddedContent.FighterAdvancedWeaponTraining.IsDisabled("WarriorSpirit")) { return; }
            AdvancedWeapontrainingSelection.AddToAdvancedWeaponTrainingSelection(WarriorSpiritFeature);
        }

        private static BlueprintBuff CreateWarriorSpiritWeaponBuff(string blueprintName, Sprite icon, int cost, params BlueprintWeaponEnchantment[] enchants) {

            var weaponBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, blueprintName, bp => {
                bp.m_DisplayName = enchants.First().m_EnchantName;
                bp.m_Description = enchants.First().m_Description;
                bp.m_Flags = BlueprintBuff.Flags.StayOnDeath | BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.m_Icon = icon;
                bp.AddComponent<AddWarriorSpiritEnchantment>(c => {
                    c.Enchants = enchants.Select(e => e.ToReference<BlueprintWeaponEnchantmentReference>()).ToArray();
                    c.Cost = cost;
                });
            });
            return weaponBuff;
        }

        public static BlueprintAbility CreateWarriorSpiritToggle(string baseName, Sprite icon, BlueprintAbility parent, int cost, params BlueprintWeaponEnchantment[] enchants) {
            var weaponBuff = CreateWarriorSpiritWeaponBuff($"{baseName}Buff", icon, cost, enchants);
            var toggleAbility = Helpers.CreateBlueprint<BlueprintAbility>(TTTContext, $"{baseName}Toggle", bp => {
                bp.m_DisplayName = weaponBuff.m_DisplayName;
                bp.m_Description = weaponBuff.m_Description;
                bp.m_DescriptionShort = weaponBuff.m_DescriptionShort;
                bp.LocalizedDuration = Helpers.CreateString(TTTContext, $"{bp.name}.LocalizedDuration", "1 Minute");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.m_Icon = weaponBuff.m_Icon;
                bp.m_Parent = parent.ToReference<BlueprintAbilityReference>();
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Special;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
                bp.DisableLog = true;
                bp.AddComponent<AbilityShowIfCasterWeaponTrainingRank>(c => {
                    c.Rank = cost;
                });
                bp.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.BuffToggle;
                    c.m_Buff = weaponBuff.ToReference<BlueprintBuffReference>();
                    c.m_GroupName = "WarriorSpirit";
                });
                bp.AddComponent<AbilityEffectToggleBuff>(c => {
                    c.m_Buff = weaponBuff.ToReference<BlueprintBuffReference>();
                });
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = parent.GetComponent<AbilityResourceLogic>().m_RequiredResource;
                    c.m_IsSpendResource = false;
                    c.Amount = 1;
                    c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
                    c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
                });
            });
            var abilityVariants = parent.GetComponent<NestedPseudoActivatableAbilities>();
            abilityVariants.m_Variants = abilityVariants.m_Variants.AppendToArray(toggleAbility.ToReference<BlueprintAbilityReference>());
            return toggleAbility;
        }
    }
}
