using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Linq;
using System.Text.RegularExpressions;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewActions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.NewComponents.Properties;
using TabletopTweaks.Utilities;
using UnityEngine;
using static TabletopTweaks.NewUnitParts.CustomStatTypes;

namespace TabletopTweaks.NewContent.Archetypes {
    static class BladeBound {
        public static void AddBlackBlade() {
            //var MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var MagusClass = Resources.GetBlueprintReference<BlueprintCharacterClassReference>("45a4607686d96a1498891b3286121780");
            var BladeBoundArchetype = Resources.GetModBlueprintReference<BlueprintArchetypeReference>("BladeBoundArchetype");
            var ArcanistClass = Resources.GetBlueprintReference<BlueprintCharacterClassReference>("52dbfd8505e22f84fad8d702611f60b7");
            var BladeAdeptArchetype = Resources.GetModBlueprintReference<BlueprintArchetypeReference>("BladeAdeptArchetype");

            var ArcanePoolFeature = Resources.GetBlueprint<BlueprintFeature>("3ce9bb90749c21249adc639031d5eed1");
            var ArcanePoolResourse = Resources.GetBlueprint<BlueprintAbilityResource>("effc3e386331f864e9e06d19dc218b37");
            var ArcanistArcaneReservoirResource = Resources.GetBlueprint<BlueprintAbilityResource>("cac948cbbe79b55459459dd6a8fe44ce");
            var Alertness = Resources.GetBlueprint<BlueprintFeature>("1c04fe9a13a22bc499ffac03e6f79153");
            var Unholy = Resources.GetBlueprint<BlueprintWeaponEnchantment>("d05753b8df780fc4bb55b318f06af453");
            var SpellResistanceBuff = Resources.GetBlueprint<BlueprintBuff>("50a77710a7c4914499d0254e76a808e5");
            var Fatigued = Resources.GetBlueprint<BlueprintBuff>("e6f2fc5d73d88064583cb828801212f4");
            var Exhausted = Resources.GetBlueprint<BlueprintBuff>("46d1b9cc3d0fd36469a471b047d773a2");

            var BastardSwordPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("91a4b3f6b4b53ae4fb3095cba86a38ca");
            var BattleAxPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("bf20773f9c880144d989e4a6f41071c7");
            var DuelingSwordPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("c23265c7960b5c144a200eafda0e7cf1");
            var DwarvenWarAx = Resources.GetBlueprint<BlueprintItemWeapon>("c3b25150bbf1bea42887176bbe2306b2");
            var FalcataPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("3e14db6284db73d40b4a4b99943e2018");
            var HandAxePlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("506a2d43fbbbe7041ad57f05900478db");
            var KamaPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("5b8394d717f0789418146692d561cd36");
            var kukriPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("86cae2531ed5df641aa57e5fb24a88c0");
            var LongSwordPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("7453fb8aa1cd7f3428a14eeadc2022d7");
            var RapierPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("0e2b2a13f286c10499921633a557388c");
            var ScimitarPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("af2a9b2b3a6905f49a44e4676a39cea8");
            var ShortSwordPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("b5f6e218fb193a24cb00bdec435732ff");
            var SicklePlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("5733378292a9fd547aeb7eccb7e79c60");

            var Icon_BlackBlade = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade.png");
            var Icon_BlackBlade_BlackBladeStrike = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_BlackBladeStrike.png");
            var Icon_BlackBlade_ElementalAttunment_Base = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_ElementalAttunment_Base.png");
            var Icon_BlackBlade_ElementalAttunment_Cold = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_ElementalAttunment_Cold.png");
            var Icon_BlackBlade_ElementalAttunment_Electricity = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_ElementalAttunment_Electricity.png");
            var Icon_BlackBlade_ElementalAttunment_Fire = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_ElementalAttunment_Fire.png");
            var Icon_BlackBlade_ElementalAttunment_Sonic = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_ElementalAttunment_Sonic.png");
            var Icon_BlackBlade_ElementalAttunment_Force = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_ElementalAttunment_Force.png");
            var Icon_BlackBlade_TransferArcana = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_TransferArcana.png");
            var Icon_BlackBlade_SpellDefense = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_SpellDefense.png");
            var Icon_BlackBlade_LifeDrinkerBase = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_LifeDrinkerBase.png");
            var Icon_BlackBlade_LifeDrinkerBlade = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_LifeDrinkerBlade.png");
            var Icon_BlackBlade_LifeDrinkerSelf = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_LifeDrinkerSelf.png");
            var Icon_BlackBlade_LifeDrinkerShared = AssetLoader.LoadInternal("Abilities", "Icon_BlackBlade_LifeDrinkerShared.png");

            var BlackBladeEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnchantment", bp => {
                bp.SetName("");
                bp.SetDescription("");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.m_IdentifyDC = 0;
            });

            var BlackBladeBastardSword = CreateBlackBlade(BastardSwordPlus5, BlackBladeEnchantment);
            var BlackBladeBattleAx = CreateBlackBlade(BattleAxPlus5, BlackBladeEnchantment);
            var BlackBladeDuelingSword = CreateBlackBlade(DuelingSwordPlus5, BlackBladeEnchantment);
            var BlackBladeDwarvenWarAx = CreateBlackBlade(DwarvenWarAx, BlackBladeEnchantment);
            var BlackBladeFalcata = CreateBlackBlade(FalcataPlus5, BlackBladeEnchantment);
            var BlackBladeHandAxe = CreateBlackBlade(HandAxePlus5, BlackBladeEnchantment);
            var BlackBladeKama = CreateBlackBlade(KamaPlus5, BlackBladeEnchantment);
            var BlackBladekukri = CreateBlackBlade(kukriPlus5, BlackBladeEnchantment);
            var BlackBladeLongSword = CreateBlackBlade(LongSwordPlus5, BlackBladeEnchantment);
            var BlackBladeRapier = CreateBlackBlade(RapierPlus5, BlackBladeEnchantment);
            var BlackBladeScimitar = CreateBlackBlade(ScimitarPlus5, BlackBladeEnchantment);
            var BlackBladeShortSword = CreateBlackBlade(ShortSwordPlus5, BlackBladeEnchantment);
            var BlackBladeSickle = CreateBlackBlade(SicklePlus5, BlackBladeEnchantment);

            var BlackBladeSelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>("BlackBladeSelection", bp => {
                bp.SetName("Black Blade Selection");
                bp.SetDescription("At 3rd level, the bladebound magus’ gains a powerful sentient weapon called a black blade, " +
                    "whose weapon type is chosen by the magus. A magus with this class feature cannot take the familiar magus arcana, " +
                    "and cannot have a familiar of any kind, even from another class.");
                bp.m_Icon = Icon_BlackBlade;
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddFeatures(
                    BlackBladeBastardSword,
                    BlackBladeBattleAx,
                    BlackBladeDuelingSword,
                    BlackBladeDwarvenWarAx,
                    BlackBladeFalcata,
                    BlackBladeHandAxe,
                    BlackBladeKama,
                    BlackBladekukri,
                    BlackBladeLongSword,
                    BlackBladeRapier,
                    BlackBladeScimitar,
                    BlackBladeShortSword,
                    BlackBladeSickle
                );
                bp.AddComponent<NoSelectionIfAlreadyHasFeature>(c => {
                    c.m_Features = new BlueprintFeatureReference[0];
                    c.AnyFeatureFromSelection = true;
                });
            });
            var BlackBladeArcanePool = Helpers.CreateBlueprint<BlueprintAbilityResource>("BlackBladeArcanePool", bp => {
                bp.m_Min = 1;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1,
                    IncreasedByStat = true,
                    ResourceBonusStat = CustomStatType.BlackBladeIntelligence.Stat(),
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                };
            });
            var BlackBladeArcanePoolFeature = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeArcanePoolFeature", bp => {
                bp.SetName("Black Blade Arcane Pool");
                bp.SetDescription("A black blade has an arcane pool with a number of points equal to 1 + its Intelligence bonus.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = ArcanePoolFeature.Icon;
            });
            var BlackBladeProgression = Helpers.CreateBlueprint<BlueprintProgression>("BlackBladeProgression", bp => {
                bp.SetName("Black Blade");
                bp.SetDescription("At level 3 the you will gain a black blade that will grow stronger along side you.");
                bp.Ranks = 1;
                bp.m_Icon = Icon_BlackBlade;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeatureRankIncrease = new BlueprintFeatureReference();
            });
            var BlackBladeProgressionProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("BlackBladeProgressionProperty", bp => {
                bp.AddComponent<ProgressionRankGetter>(c => {
                    c.Progression = BlackBladeProgression.ToReference<BlueprintProgressionReference>();
                    c.UseMax = true;
                    c.Max = 20;
                });
            });

            var BlackBladeEgoProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("BlackBladeEgoProperty", bp => {
                bp.AddComponent<StatValueGetter>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                });
            });

            var BlackBladeStrikeEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeStrikeEnchantment", bp => {
                bp.SetName("Black Blade Strike");
                bp.SetDescription("The Black Blade gains a +1 bonus on damage rolls. For every four levels beyond 1st, " +
                    "this bonus increases by 1.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeStrike>(c => {
                    c.WeilderProperty = BlackBladeProgressionProperty.ToReference<BlueprintUnitPropertyReference>();
                });
            });
            var BlackBladeStrikeBuff = Helpers.CreateBuff("BlackBladeStrikeBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Black Blade Strike");
                bp.SetDescription("As a free action, the magus can spend a point from the black blade’s arcane pool " +
                    "to grant the black blade a +1 bonus on damage rolls for 1 minute. For every four levels beyond 1st, " +
                    "this ability gives the black blade another +1 on damage rolls.");
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_BlackBlade_BlackBladeStrike;
                bp.AddComponent<BlackBladeEffect>(c => {
                    c.BlackBladeStrike = true;
                    c.Enchantment = BlackBladeStrikeEnchantment.ToReference<BlueprintWeaponEnchantmentReference>();
                });
            });
            var BlackBladeStrikeAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeStrikeAbility", bp => {
                bp.SetName(BlackBladeStrikeBuff.m_DisplayName);
                bp.SetDescription(BlackBladeStrikeBuff.Description);
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 minute");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.Type = AbilityType.Special;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = BlackBladeStrikeBuff.Icon;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                });
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<AbilityRequirementHasBuff>(c => {
                    c.RequiredBuff = BlackBladeStrikeBuff.ToReference<BlueprintBuffReference>();
                    c.Not = true;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] {
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = BlackBladeStrikeBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        })
                    };
                });
            });
            var BlackBladeStrike = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeStrike", bp => {
                bp.SetName(BlackBladeStrikeAbility.m_DisplayName);
                bp.SetDescription(BlackBladeStrikeAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlackBladeStrikeAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BlackBladeStrikeBuff.Icon;
            });

            var BlackBladeEnergyAttunementCold = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementCold", bp => {
                bp.SetName("Energy Attunement");
                bp.SetDescription("The Black Blade's damage is converted into cold damage.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData(),
                        Energy = DamageEnergyType.Cold
                    };
                });
            });
            var BlackBladeEnergyAttunementElectricity = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementElectricity", bp => {
                bp.SetName("Energy Attunement");
                bp.SetDescription("The Black Blade's damage is converted into electricity damage.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData(),
                        Energy = DamageEnergyType.Electricity
                    };
                });
            });
            var BlackBladeEnergyAttunementFire = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementFire", bp => {
                bp.SetName("Energy Attunement");
                bp.SetDescription("The Black Blade's damage is converted into fire damage.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData(),
                        Energy = DamageEnergyType.Fire
                    };
                });
            });
            var BlackBladeEnergyAttunementSonic = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementSonic", bp => {
                bp.SetName("Energy Attunement");
                bp.SetDescription("The Black Blade's damage is converted into sonic damage.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData(),
                        Energy = DamageEnergyType.Sonic
                    };
                });
            });
            var BlackBladeEnergyAttunementForce = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementForce", bp => {
                bp.SetName("Energy Attunement");
                bp.SetDescription("The Black Blade's damage is converted into force damage.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Force,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData()
                    };
                });
            });
            var BlackBladeEnergyAttunementBuff = Helpers.CreateBuff("BlackBladeEnergyAttunementBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Energy Attunement");
                bp.SetDescription("At 5th level, as a free action, the wielder can spend a point of his black blade’s arcane pool to have it deal " +
                    "one of the following types of damage instead of weapon damage: cold, electricity, or fire. He can spend 2 points from the " +
                    "black blade’s arcane pool to deal sonic or force damage instead of weapon damage. This effect lasts until the start of the wielder’s next turn.");
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_BlackBlade_ElementalAttunment_Base;
            });
            var BlackBladeEnergyAttunementColdAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementColdAbility",
                "Energy Attunement — Cold",
                BlackBladeArcanePool, 1,
                BlackBladeEnergyAttunementCold,
                BlackBladeEnergyAttunementBuff,
                Icon_BlackBlade_ElementalAttunment_Cold
            );
            var BlackBladeEnergyAttunementElectricityAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementElectricityAbility",
                "Energy Attunement — Electricity",
                BlackBladeArcanePool, 1,
                BlackBladeEnergyAttunementElectricity,
                BlackBladeEnergyAttunementBuff,
                Icon_BlackBlade_ElementalAttunment_Electricity
            );
            var BlackBladeEnergyAttunementFireAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementFireAbility",
                "Energy Attunement — Fire",
                BlackBladeArcanePool, 1,
                BlackBladeEnergyAttunementFire,
                BlackBladeEnergyAttunementBuff,
                Icon_BlackBlade_ElementalAttunment_Fire
            );
            var BlackBladeEnergyAttunementSonicAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementSonicAbility",
                "Energy Attunement — Sonic",
                BlackBladeArcanePool, 2,
                BlackBladeEnergyAttunementSonic,
                BlackBladeEnergyAttunementBuff,
                Icon_BlackBlade_ElementalAttunment_Sonic
            );
            var BlackBladeEnergyAttunementForceAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementForceAbility",
                "Energy Attunement — Force",
                BlackBladeArcanePool, 2,
                BlackBladeEnergyAttunementForce,
                BlackBladeEnergyAttunementBuff,
                Icon_BlackBlade_ElementalAttunment_Force
            );
            var BlackBladeEnergyAttunementBaseAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeEnergyAttunementBaseAbility", bp => {
                bp.SetName("Energy Attunement");
                bp.SetDescription("At 5th level, as a free action, a wielder can spend a point of his black blade’s arcane pool to have it deal one " +
                    "of the following types of damage instead of weapon damage: cold, electricity, or fire. He can spend 2 points from the black " +
                    "blade’s arcane pool to deal sonic or force damage instead of weapon damage. This effect lasts until the start of the wielder’s next turn.");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.LocalizedDuration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.m_Icon = Icon_BlackBlade_ElementalAttunment_Base;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.NeedEquipWeapons = true;
                bp.AddComponent<AbilityVariants>(c => {
                    c.m_Variants = new BlueprintAbilityReference[] {
                        BlackBladeEnergyAttunementColdAbility.ToReference<BlueprintAbilityReference>(),
                        BlackBladeEnergyAttunementElectricityAbility.ToReference<BlueprintAbilityReference>(),
                        BlackBladeEnergyAttunementFireAbility.ToReference<BlueprintAbilityReference>(),
                        BlackBladeEnergyAttunementSonicAbility.ToReference<BlueprintAbilityReference>(),
                        BlackBladeEnergyAttunementForceAbility.ToReference<BlueprintAbilityReference>(),
                    };
                });
            });
            var BlackBladeEnergyAttunement = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeEnergyAttunement", bp => {
                bp.SetName(BlackBladeEnergyAttunementBaseAbility.m_DisplayName);
                bp.SetDescription(BlackBladeEnergyAttunementBaseAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlackBladeEnergyAttunementBaseAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BlackBladeEnergyAttunementBaseAbility.Icon;
            });

            
            var BlackBladeTransferArcanaAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeTransferArcanaAbility", bp => {
                bp.SetName("Transfer Arcana — Magus");
                bp.SetDescription("At 13th level a magus can attempt to siphon points from his black blade’s arcane pool " +
                    "into his own arcane pool. Doing so takes a full-round action and the magus must succeed at a Will saving throw with " +
                    "a DC equal to the black blade’s ego. If the magus succeeds, he regains 1 point to his arcane pool for every 2 points " +
                    "he saps from his black blade. If he fails the saving throw, the magus becomes fatigued. If he is " +
                    "fatigued, he becomes exhausted instead. He cannot use this ability if he is exhausted.");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 round");
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.Save", "");
                bp.m_Icon = Icon_BlackBlade_TransferArcana;
                bp.Range = AbilityRange.Personal;
                bp.Type = AbilityType.Supernatural;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = true;
                bp.AddComponent<ContextSetAbilityParams>(c => {
                    c.DC = new ContextValue();
                    c.Concentration = new ContextValue();
                    c.SpellLevel = new ContextValue();
                    c.CasterLevel = new ContextValue();
                    c.DC = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = BlackBladeEgoProperty.ToReference<BlueprintUnitPropertyReference>()
                    };
                    c.Add10ToDC = false;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionTransferArcana>(a => {
                            a.m_sourceResource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                            a.m_sourceAmount = 2;
                            a.m_destinationResource = ArcanePoolResourse.ToReference<BlueprintAbilityResourceReference>();
                            a.m_destinationAmount = 1;
                            a.SaveDC = new ContextValue() {
                                ValueType = ContextValueType.CasterCustomProperty,
                                m_CustomProperty = BlackBladeEgoProperty.ToReference<BlueprintUnitPropertyReference>()
                            };
                            a.FailedActions = Helpers.CreateActionList(
                                new Conditional {
                                    ConditionsChecker = new ConditionsChecker {
                                        Conditions = new Condition[] {
                                            Helpers.Create<ContextConditionHasFact>(c => {
                                                c.m_Fact = Fatigued.ToReference<BlueprintUnitFactReference>();
                                                c.Not = true;
                                            })
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(c => {
                                            c.m_Buff = Fatigued.ToReference<BlueprintBuffReference>();
                                            c.Permanent = true;
                                            c.DurationValue = new ContextDurationValue();
                                        })
                                    ),
                                    IfFalse = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(c => {
                                            c.m_Buff = Exhausted.ToReference<BlueprintBuffReference>();
                                            c.Permanent = true;
                                            c.DurationValue = new ContextDurationValue();
                                        })
                                    )
                                }
                            );
                        })
                    );
                });
                bp.AddComponent<AbilityShowIfCasterHasArchetype>(c => {
                    c.Class = MagusClass;
                    c.Archetype = BladeBoundArchetype;
                });
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<AbilityRequirementHasResource>(c => {
                    c.Amount = 2;
                    c.Resource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                });
            });
            var BlackBladeTransferArcanaArcanistAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeTransferArcanaArcanistAbility", bp => {
                bp.SetName("Transfer Arcana — Arcanist");
                bp.SetDescription("At 13th level a magus can attempt to siphon points from his black blade’s arcane pool " +
                    "into his own arcane reservoir. Doing so takes a full-round action and the arcanist must succeed at a Will saving throw with " +
                    "a DC equal to the black blade’s ego. If the arcanist succeeds, he regains 1 point to his arcane reservoir for every 2 points " +
                    "he saps from his black blade. If he fails the saving throw, the magus becomes fatigued. If he is " +
                    "fatigued, he becomes exhausted instead. He cannot use this ability if he is exhausted.");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 round");
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.Save", "");
                bp.m_Icon = Icon_BlackBlade_TransferArcana;
                bp.Range = AbilityRange.Personal;
                bp.Type = AbilityType.Supernatural;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = true;
                bp.AddComponent<ContextSetAbilityParams>(c => {
                    c.DC = new ContextValue();
                    c.Concentration = new ContextValue();
                    c.SpellLevel = new ContextValue();
                    c.CasterLevel = new ContextValue();
                    c.DC = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = BlackBladeEgoProperty.ToReference<BlueprintUnitPropertyReference>()
                    };
                    c.Add10ToDC = false;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionTransferArcana>(a => {
                            a.m_sourceResource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                            a.m_sourceAmount = 2;
                            a.m_destinationResource = ArcanistArcaneReservoirResource.ToReference<BlueprintAbilityResourceReference>();
                            a.m_destinationAmount = 1;
                            a.SaveDC = new ContextValue() {
                                ValueType = ContextValueType.CasterCustomProperty,
                                m_CustomProperty = BlackBladeEgoProperty.ToReference<BlueprintUnitPropertyReference>()
                            };
                            a.FailedActions = Helpers.CreateActionList(
                                new Conditional {
                                    ConditionsChecker = new ConditionsChecker {
                                        Conditions = new Condition[] {
                                            Helpers.Create<ContextConditionHasFact>(c => {
                                                c.m_Fact = Fatigued.ToReference<BlueprintUnitFactReference>();
                                                c.Not = true;
                                            })
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(c => {
                                            c.m_Buff = Fatigued.ToReference<BlueprintBuffReference>();
                                            c.Permanent = true;
                                            c.DurationValue = new ContextDurationValue();
                                        })
                                    ),
                                    IfFalse = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(c => {
                                            c.m_Buff = Exhausted.ToReference<BlueprintBuffReference>();
                                            c.Permanent = true;
                                            c.DurationValue = new ContextDurationValue();
                                        })
                                    )
                                }
                            );
                        })
                    );
                });
                bp.AddComponent<AbilityShowIfCasterHasArchetype>(c => {
                    c.Class = ArcanistClass;
                    c.Archetype = BladeAdeptArchetype;
                });
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<AbilityRequirementHasResource>(c => {
                    c.Amount = 2;
                    c.Resource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                });
            });
            var BlackBladeTransferArcanaBaseAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeTransferArcanaBaseAbility", bp => {
                bp.SetName("Transfer Arcana");
                bp.SetDescription("At 13th level a magus can attempt to siphon points from his black blade’s arcane pool " +
                    "into his own arcane pool. Doing so takes a full-round action and the magus must succeed at a Will saving throw with " +
                    "a DC equal to the black blade’s ego. If the magus succeeds, he regains 1 point to his arcane pool for every 2 points " +
                    "he saps from his black blade. If he fails the saving throw, the magus becomes fatigued. If he is " +
                    "fatigued, he becomes exhausted instead. He cannot use this ability if he is exhausted.");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 round");
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.Save", "");
                bp.m_Icon = Icon_BlackBlade_TransferArcana;
                bp.Range = AbilityRange.Personal;
                bp.Type = AbilityType.Supernatural;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = true;
                bp.AddComponent<AbilityVariants>(c => {
                    c.m_Variants = new BlueprintAbilityReference[] {
                        BlackBladeTransferArcanaAbility.ToReference<BlueprintAbilityReference>(),
                        BlackBladeTransferArcanaArcanistAbility.ToReference<BlueprintAbilityReference>()
                    };
                });
            });
            var BlackBladeTransferArcana = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeTransferArcana", bp => {
                bp.SetName(BlackBladeTransferArcanaAbility.m_DisplayName);
                bp.SetDescription(BlackBladeTransferArcanaAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlackBladeTransferArcanaBaseAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BlackBladeTransferArcanaAbility.Icon;
            });

            var BlackBladeSpellDefenseBuff = Helpers.CreateBuff("BlackBladeSpellDefenseBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Spell Defense");
                bp.SetDescription("A wielder of a black black of 17th level or higher can expend an arcane point from his weapon’s arcane pool as a free action; " +
                    "he then gains SR equal to his black blade’s ego until the start of his next turn.");
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_BlackBlade_SpellDefense;
                bp.AddComponent<AddSpellResistance>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = BlackBladeEgoProperty.ToReference<BlueprintUnitPropertyReference>()
                    };
                });
            });
            var BlackBladeSpellDefenseAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeSpellDefenseAbility", bp => {
                bp.SetName(BlackBladeSpellDefenseBuff.m_DisplayName);
                bp.SetDescription(BlackBladeSpellDefenseBuff.Description);
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.Type = AbilityType.Special;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = BlackBladeSpellDefenseBuff.Icon;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                });
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] {
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = BlackBladeSpellDefenseBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        })
                    };
                });
            });
            var BlackBladeSpellDefense = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeSpellDefense", bp => {
                bp.SetName(BlackBladeSpellDefenseAbility.m_DisplayName);
                bp.SetDescription(BlackBladeSpellDefenseAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlackBladeSpellDefenseAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BlackBladeSpellDefenseAbility.Icon;
            });

            var BlackBladeLifeDrinkerBase = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeLifeDrinkerBase", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("At 19th level, each time the wielder kills a living creature with the black blade, he can pick one of the " +
                    "following effects: \n" +
                    "Blade - The Black Blade restores 2 points to its arcane pool.\n" +
                    "Shared - The Black Blade restores 1 point to its arcane pool and the magus restores 1 point to his arcane pool\n" +
                    "Wielder - The Wielder gains a number of temporary hit points equal to the black blade’s ego " +
                    "(these temporary hit points last until spent or 1 minute, whichever is shorter).");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.Type = AbilityType.Supernatural;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerBase;
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<AbilityVariants>();
                bp.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.VariantsBase;
                    c.m_ActiveWhenVariantActive = true;
                    c.m_UseActiveVariantForeIcon = true;
                });
            });
            var BlackBladeLifeDrinkerBladeEnchantAbility = Helpers.CreateBlueprint<BlueprintAbility>($"BlackBladeLifeDrinkerBladeEnchantAbility", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("");
                bp.DisableLog = true;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerBlade;
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.Save", $"");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", $"");
                bp.CanTargetSelf = true;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Touch;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextRestoreResource>(a => {
                            a.m_Resource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                            a.ContextValueRestoration = true;
                            a.Value = 2;
                        })
                    );
                });
            });
            var BlackBladeLifeDrinkerBladeEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeLifeDrinkerBladeEnchantment", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("Each time the wielder kills a living creature with the black blade, the black blade restores 2 points of its arcane pool.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.m_EnchantmentCost = 0;
                bp.m_IdentifyDC = 0;
                bp.AddComponent<AddWeaponDamageTrigger>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionCastSpell() {
                            m_Spell = BlackBladeLifeDrinkerBladeEnchantAbility.ToReference<BlueprintAbilityReference>(),
                            DC = new ContextValue(),
                            SpellLevel = new ContextValue()
                        }
                    );
                    c.CastOnSelf = true;
                    c.TargetKilledByThisDamage = true;
                });
            });
            var BlackBladeLifeDrinkerBladeBuff = Helpers.CreateBuff($"BlackBladeLifeDrinkerBladeBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Life Drinker — Blade");
                bp.SetDescription(BlackBladeLifeDrinkerBladeEnchantment.m_Description);
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerBlade;
                bp.AddComponent<BlackBladeEffect>(c => {
                    c.LifeDrinker = true;
                    c.Enchantment = BlackBladeLifeDrinkerBladeEnchantment.ToReference<BlueprintWeaponEnchantmentReference>();
                });
            });
            var BlackBladeLifeDrinkerBladeAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeLifeDrinkerBladeAbility", bp => {
                bp.SetName(BlackBladeLifeDrinkerBladeBuff.m_DisplayName);
                bp.SetDescription(BlackBladeLifeDrinkerBladeEnchantment.m_Description);
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.DisableLog = true;
                bp.m_Parent = BlackBladeLifeDrinkerBase.ToReference<BlueprintAbilityReference>();
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = BlackBladeLifeDrinkerBladeBuff.Icon;
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.BuffToggle;
                    c.m_Buff = BlackBladeLifeDrinkerBladeBuff.ToReference<BlueprintBuffReference>();
                    c.m_GroupName = "BlackBladeLifeDrinker";
                });
                bp.AddComponent<AbilityEffectToggleBuff>(c => {
                    c.m_Buff = BlackBladeLifeDrinkerBladeBuff.ToReference<BlueprintBuffReference>();
                });
            });
            var BlackBladeLifeDrinkerSharedEnchantAbility = Helpers.CreateBlueprint<BlueprintAbility>($"BlackBladeLifeDrinkerSharedEnchantAbility", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("");
                bp.DisableLog = true;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerShared;
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.Save", $"");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", $"");
                bp.CanTargetSelf = true;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Touch;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextRestoreResource>(a => {
                            a.m_Resource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                            a.ContextValueRestoration = true;
                            a.Value = 1;
                        }),
                        Helpers.Create<ContextRestoreResource>(a => {
                            a.m_Resource = ArcanePoolResourse.ToReference<BlueprintAbilityResourceReference>();
                            a.ContextValueRestoration = true;
                            a.Value = 1;
                        })
                    );
                });
            });
            var BlackBladeLifeDrinkerSharedEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeLifeDrinkerSharedEnchantment", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("Each time the wielder kills a living creature with the black blade, " +
                    "the black blade restores 1 point to its arcane pool and the wielder restores 1 point to his arcane pool.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.m_EnchantmentCost = 0;
                bp.m_IdentifyDC = 0;
                bp.AddComponent<AddWeaponDamageTrigger>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionCastSpell() {
                            m_Spell = BlackBladeLifeDrinkerSharedEnchantAbility.ToReference<BlueprintAbilityReference>(),
                            DC = new ContextValue(),
                            SpellLevel = new ContextValue()
                        }
                    );
                    c.CastOnSelf = true;
                    c.TargetKilledByThisDamage = true;
                });
            });
            var BlackBladeLifeDrinkerSharedBuff = Helpers.CreateBuff($"BlackBladeLifeDrinkerSharedBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Life Drinker — Shared (Magus)");
                bp.SetDescription(BlackBladeLifeDrinkerSharedEnchantment.m_Description);
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerShared;
                bp.AddComponent<BlackBladeEffect>(c => {
                    c.LifeDrinker = true;
                    c.Enchantment = BlackBladeLifeDrinkerSharedEnchantment.ToReference<BlueprintWeaponEnchantmentReference>();
                });
            });
            var BlackBladeLifeDrinkerSharedAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeLifeDrinkerSharedAbility", bp => {
                bp.SetName(BlackBladeLifeDrinkerSharedBuff.m_DisplayName);
                bp.SetDescription(BlackBladeLifeDrinkerSharedBuff.m_Description);
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.DisableLog = true;
                bp.m_Parent = BlackBladeLifeDrinkerBase.ToReference<BlueprintAbilityReference>();
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerShared;
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<AbilityShowIfCasterHasArchetype>(c => {
                    c.Class = MagusClass;
                    c.Archetype = BladeBoundArchetype;
                });
                bp.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.BuffToggle;
                    c.m_Buff = BlackBladeLifeDrinkerSharedBuff.ToReference<BlueprintBuffReference>();
                    c.m_GroupName = "BlackBladeLifeDrinker";
                });
                bp.AddComponent<AbilityEffectToggleBuff>(c => {
                    c.m_Buff = BlackBladeLifeDrinkerSharedBuff.ToReference<BlueprintBuffReference>();
                });
            });

            var BlackBladeLifeDrinkerSharedArcanistEnchantAbility = Helpers.CreateBlueprint<BlueprintAbility>($"BlackBladeLifeDrinkerSharedArcanistEnchantAbility", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("");
                bp.DisableLog = true;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerShared;
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.Save", $"");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", $"");
                bp.CanTargetSelf = true;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Touch;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextRestoreResource>(a => {
                            a.m_Resource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                            a.ContextValueRestoration = true;
                            a.Value = 1;
                        }),
                        Helpers.Create<ContextRestoreResource>(a => {
                            a.m_Resource = ArcanistArcaneReservoirResource.ToReference<BlueprintAbilityResourceReference>();
                            a.ContextValueRestoration = true;
                            a.Value = 1;
                        })
                    );
                });
            });
            var BlackBladeLifeDrinkerSharedArcanistEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeLifeDrinkerSharedArcanistEnchantment", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("Each time the wielder kills a living creature with the black blade, " +
                    "the black blade restores 1 point to its arcane pool and the wielder restores 1 point to his arcane reservoir.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.m_EnchantmentCost = 0;
                bp.m_IdentifyDC = 0;
                bp.AddComponent<AddWeaponDamageTrigger>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionCastSpell() {
                            m_Spell = BlackBladeLifeDrinkerSharedEnchantAbility.ToReference<BlueprintAbilityReference>(),
                            DC = new ContextValue(),
                            SpellLevel = new ContextValue()
                        }
                    );
                    c.CastOnSelf = true;
                    c.TargetKilledByThisDamage = true;
                });
            });
            var BlackBladeLifeDrinkerSharedArcanistBuff = Helpers.CreateBuff($"BlackBladeLifeDrinkerSharedArcanistBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Life Drinker — Shared (Arcanist)");
                bp.SetDescription(BlackBladeLifeDrinkerSharedArcanistEnchantment.m_Description);
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerShared;
                bp.AddComponent<BlackBladeEffect>(c => {
                    c.LifeDrinker = true;
                    c.Enchantment = BlackBladeLifeDrinkerSharedEnchantment.ToReference<BlueprintWeaponEnchantmentReference>();
                });
            });
            var BlackBladeLifeDrinkerSharedArcanistAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeLifeDrinkerSharedArcanistAbility", bp => {
                bp.SetName(BlackBladeLifeDrinkerSharedArcanistBuff.m_DisplayName);
                bp.SetDescription(BlackBladeLifeDrinkerSharedArcanistBuff.m_Description);
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.DisableLog = true;
                bp.m_Parent = BlackBladeLifeDrinkerBase.ToReference<BlueprintAbilityReference>();
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerShared;
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<AbilityShowIfCasterHasArchetype>(c => {
                    c.Class = ArcanistClass;
                    c.Archetype = BladeAdeptArchetype;
                });
                bp.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.BuffToggle;
                    c.m_Buff = BlackBladeLifeDrinkerSharedArcanistBuff.ToReference<BlueprintBuffReference>();
                    c.m_GroupName = "BlackBladeLifeDrinker";
                });
                bp.AddComponent<AbilityEffectToggleBuff>(c => {
                    c.m_Buff = BlackBladeLifeDrinkerSharedArcanistBuff.ToReference<BlueprintBuffReference>();
                });
            });

            var BlackBladeLifeDrinkerTempHPBuff = Helpers.CreateBuff($"BlackBladeLifeDrinkerTempHPBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Life Drinker Temp HP");
                bp.SetDescription("");
                bp.IsClassFeature = true;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.AddComponent<TemporaryHitPointsFromAbilityValue>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = BlackBladeEgoProperty.ToReference<BlueprintUnitPropertyReference>()
                    };
                    c.RemoveWhenHitPointsEnd = true;
                });
            });
            var BlackBladeLifeDrinkerWielderEnchantAbility = Helpers.CreateBlueprint<BlueprintAbility>($"BlackBladeLifeDrinkerWielderEnchantAbility", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("");
                bp.DisableLog = true;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerSelf;
                bp.LocalizedSavingThrow = Helpers.CreateString($"{bp.name}.Save", $"");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", $"");
                bp.CanTargetSelf = true;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Touch;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = BlackBladeLifeDrinkerTempHPBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        })
                    );
                });
            });
            var BlackBladeLifeDrinkerWielderEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeLifeDrinkerWielderEnchantment", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("Each time the wielder kills a living creature with the black blade, " +
                    "the wielder gains a number of temporary hit points equal to the black blade’s ego (these temporary hit points last until spent or 1 minute, whichever is shorter).");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.m_EnchantmentCost = 0;
                bp.m_IdentifyDC = 0;
                bp.AddComponent<AddWeaponDamageTrigger>(c => {
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionCastSpell() {
                            m_Spell = BlackBladeLifeDrinkerWielderEnchantAbility.ToReference<BlueprintAbilityReference>(),
                            DC = new ContextValue(),
                            SpellLevel = new ContextValue()
                        }
                    );
                    c.CastOnSelf = true;
                    c.TargetKilledByThisDamage = true;
                });
            });
            var BlackBladeLifeDrinkerWielderBuff = Helpers.CreateBuff($"BlackBladeLifeDrinkerWielderBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Life Drinker — Wielder");
                bp.SetDescription(BlackBladeLifeDrinkerWielderEnchantment.m_Description);
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerSelf;
                bp.AddComponent<BlackBladeEffect>(c => {
                    c.LifeDrinker = true;
                    c.Enchantment = BlackBladeLifeDrinkerWielderEnchantment.ToReference<BlueprintWeaponEnchantmentReference>();
                });
            });
            var BlackBladeLifeDrinkerWielderAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeLifeDrinkerWielderAbility", bp => {
                bp.SetName(BlackBladeLifeDrinkerWielderBuff.m_DisplayName);
                bp.SetDescription(BlackBladeLifeDrinkerWielderEnchantment.m_Description);
                bp.LocalizedDuration = new Kingmaker.Localization.LocalizedString();
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.DisableLog = true;
                bp.m_Parent = BlackBladeLifeDrinkerBase.ToReference<BlueprintAbilityReference>();
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = Icon_BlackBlade_LifeDrinkerSelf;
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.BuffToggle;
                    c.m_Buff = BlackBladeLifeDrinkerWielderBuff.ToReference<BlueprintBuffReference>();
                    c.m_GroupName = "BlackBladeLifeDrinker";
                });
                bp.AddComponent<AbilityEffectToggleBuff>(c => {
                    c.m_Buff = BlackBladeLifeDrinkerWielderBuff.ToReference<BlueprintBuffReference>();
                });
            });
            BlackBladeLifeDrinkerBase.GetComponent<AbilityVariants>().m_Variants = new BlueprintAbilityReference[] {
                BlackBladeLifeDrinkerBladeAbility.ToReference<BlueprintAbilityReference>(),
                BlackBladeLifeDrinkerSharedAbility.ToReference<BlueprintAbilityReference>(),
                BlackBladeLifeDrinkerSharedArcanistAbility.ToReference<BlueprintAbilityReference>(),
                BlackBladeLifeDrinkerWielderAbility.ToReference<BlueprintAbilityReference>(),
            };

            var BlackBladeLifeDrinker = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeLifeDrinker", bp => {
                bp.SetName(BlackBladeLifeDrinkerBase.m_DisplayName);
                bp.SetDescription(BlackBladeLifeDrinkerBase.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlackBladeLifeDrinkerBase.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BlackBladeLifeDrinkerBase.Icon;
            });

            var BlackBladeBaseStats = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeBaseStats", bp => {
                bp.SetName("Black Blade Base Stats");
                bp.SetDescription("The Black Blade starts with the following stats:\n" +
                    "Ego: 5\n" +
                    "Int: 11\n" +
                    "Wis: 7\n" +
                    "Cha: 7");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                    c.Value = 5;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeIntelligence.Stat();
                    c.Value = 11;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeCharisma.Stat();
                    c.Value = 7;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeWisdom.Stat();
                    c.Value = 7;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var BlackBladeStatIncrease = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeMentalIncrease", bp => {
                bp.SetName("Black Blade Stat Increase");
                bp.SetDescription("The Black Blade's mental stats increase by 1.");
                bp.Ranks = 8;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeIntelligence.Stat();
                    c.Value = 1;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeCharisma.Stat();
                    c.Value = 1;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeWisdom.Stat();
                    c.Value = 1;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var BlackBladeEgoIncrease2 = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeEgoIncrease2", bp => {
                bp.SetName("Black Blade Ego Increase");
                bp.SetDescription("The Black Blade's ego increases by 2.");
                bp.Ranks = 6;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                    c.Value = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var BlackBladeEgoIncrease3 = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeEgoIncrease3", bp => {
                bp.SetName("Black Blade Ego Increase");
                bp.SetDescription("The Black Blade's ego increases by 3.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                    c.Value = 3;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var BlackBladeEgoIncrease4 = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeEgoIncrease4", bp => {
                bp.SetName("Black Blade Ego Increase");
                bp.SetDescription("The Black Blade's ego increases by 4.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                    c.Value = 4;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });

            BlackBladeProgression.LevelEntries = new LevelEntry[] {
                Helpers.CreateLevelEntry(3, BlackBladeBaseStats, BlackBladeSelection, Alertness, BlackBladeStrike, BlackBladeArcanePoolFeature),
                Helpers.CreateLevelEntry(5, BlackBladeStatIncrease, BlackBladeEgoIncrease3, BlackBladeEnergyAttunement),
                Helpers.CreateLevelEntry(7, BlackBladeStatIncrease, BlackBladeEgoIncrease2),
                Helpers.CreateLevelEntry(9, BlackBladeStatIncrease, BlackBladeEgoIncrease2),
                Helpers.CreateLevelEntry(11, BlackBladeStatIncrease, BlackBladeEgoIncrease2),
                Helpers.CreateLevelEntry(13, BlackBladeStatIncrease, BlackBladeEgoIncrease2, BlackBladeTransferArcana),
                Helpers.CreateLevelEntry(15, BlackBladeStatIncrease, BlackBladeEgoIncrease2),
                Helpers.CreateLevelEntry(17, BlackBladeStatIncrease, BlackBladeEgoIncrease4, BlackBladeSpellDefense),
                Helpers.CreateLevelEntry(19, BlackBladeStatIncrease, BlackBladeEgoIncrease2, BlackBladeLifeDrinker),
            };
            BlackBladeProgression.UIGroups = new UIGroup[] {
                Helpers.CreateUIGroup(BlackBladeBaseStats, BlackBladeEgoIncrease2, BlackBladeEgoIncrease3, BlackBladeEgoIncrease4),
                Helpers.CreateUIGroup(BlackBladeSelection, BlackBladeStatIncrease),
                Helpers.CreateUIGroup(BlackBladeStrike, BlackBladeEnergyAttunement, BlackBladeTransferArcana, BlackBladeSpellDefense, BlackBladeLifeDrinker)
            };
        }
        public static void AddBladeBound() {
            var MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var MagusArcanaSelection = Resources.GetBlueprint<BlueprintFeature>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");
            var ArcanePoolFeature = Resources.GetBlueprint<BlueprintFeature>("3ce9bb90749c21249adc639031d5eed1");
            var ArcanePoolResourse = Resources.GetBlueprint<BlueprintAbilityResource>("effc3e386331f864e9e06d19dc218b37");
            var BlackBladeProgression = Resources.GetModBlueprint<BlueprintProgression>("BlackBladeProgression");

            var BladeBoundArchetype = Helpers.CreateBlueprint<BlueprintArchetype>("BladeBoundArchetype", bp => {
                bp.SetName("Bladebound");
                bp.SetDescription("A select group of magi are called to carry a black blade, a sentient " +
                    "weapon of often unknown and possibly unknowable purpose. These weapons become " +
                    "valuable tools and allies, as both the magus and weapon typically crave arcane power, " +
                    "but as a black blade becomes more aware, its true motivations manifest, and as does its " +
                    "ability to influence its wielder with its ever-increasing ego.");
            });
            var BladeBoundArcanePool = Helpers.CreateBlueprint<BlueprintFeature>("BladeBoundArcanePool", bp => {
                bp.SetName(ArcanePoolFeature.m_DisplayName);
                bp.SetDescription("Instead of the normal arcane pool amount, the bladebound magus’s arcane pool has a number of points " +
                    "equal to 1/3 his level (minimum 1) plus his Intelligence bonus.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = ArcanePoolFeature.Icon;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        ArcanePoolFeature.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.AddComponent<ContextIncreaseResourceAmount>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus
                    };
                    c.Subtract = true;
                    c.m_Resource = ArcanePoolResourse.ToReference<BlueprintAbilityResourceReference>();
                });
                bp.AddComponent<ContextIncreaseResourceAmount>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.StatBonus
                    };
                    c.m_Resource = ArcanePoolResourse.ToReference<BlueprintAbilityResourceReference>();
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                    c.m_Progression = ContextRankProgression.Div2;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                    c.m_Class = new BlueprintCharacterClassReference[] { MagusClass.ToReference<BlueprintCharacterClassReference>() };
                    c.Archetype = BladeBoundArchetype.ToReference<BlueprintArchetypeReference>();
                    c.m_AdditionalArchetypes = new BlueprintArchetypeReference[0];
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.StatBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                    c.m_Progression = ContextRankProgression.DivStep;
                    c.m_StepLevel = 3;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                    c.m_Class = new BlueprintCharacterClassReference[] { MagusClass.ToReference<BlueprintCharacterClassReference>() };
                    c.Archetype = BladeBoundArchetype.ToReference<BlueprintArchetypeReference>();
                    c.m_AdditionalArchetypes = new BlueprintArchetypeReference[0];
                });
            });
            
            BladeBoundArchetype.RemoveFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, ArcanePoolFeature),
                Helpers.CreateLevelEntry(3, MagusArcanaSelection)
            };
            BladeBoundArchetype.AddFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, BladeBoundArcanePool, BlackBladeProgression)
            };
            BlackBladeProgression.AddArchetype(BladeBoundArchetype);

            if (ModSettings.AddedContent.Archetypes.IsDisabled("BladeBound")) { return; }
            MagusClass.m_Archetypes = MagusClass.m_Archetypes.AppendToArray(BladeBoundArchetype.ToReference<BlueprintArchetypeReference>());
            MagusClass.Progression.UIGroups
                .Where(group => group.Features.Contains(ArcanePoolFeature))
                .ForEach(group => group.m_Features.Add(BladeBoundArcanePool.ToReference<BlueprintFeatureBaseReference>()));
            Main.LogPatch("Added", BladeBoundArchetype);
        }

        private static BlueprintAbility CreateEnergyAttunement(
            string name,
            string DisplayName,
            BlueprintAbilityResource resource, int cost,
            BlueprintWeaponEnchantment enchant,
            BlueprintBuff buff,
            Sprite icon = null) {

            var EnergyAttunementBuff = Helpers.CreateBuff($"{name}Buff", bp => {
                bp.Ranks = 1;
                bp.SetName(DisplayName);
                bp.SetDescription("");
                bp.IsClassFeature = true;
                bp.m_Icon = icon;
                bp.AddComponent<BlackBladeEffect>(c => {
                    c.EnergyAttunement = true;
                    c.Enchantment = enchant.ToReference<BlueprintWeaponEnchantmentReference>();
                });
            });

            var EnergyAttunement = Helpers.CreateBlueprint<BlueprintAbility>(name, bp => {
                bp.SetName(DisplayName);
                bp.SetDescription("");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.Type = AbilityType.Supernatural;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = icon;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = cost;
                });
                bp.AddComponent<AbilityRequirementHasBuff>(c => {
                    c.RequiredBuff = buff.ToReference<BlueprintBuffReference>();
                    c.Not = true;
                });
                bp.AddComponent<AbilityRequirementHasBlackBlade>();
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] {
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = buff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        }),
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = EnergyAttunementBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        })
                    };
                });
            });
            return EnergyAttunement;
        }

        private static BlueprintFeature CreateBlackBlade(BlueprintItemWeapon baseWeapon, BlueprintWeaponEnchantment enchant) {
            //var LexiconAssemble_BE = Resources.GetBlueprint<BlueprintDialog>("9df5b313d792a424392ae64647e36969");
            var CatergorySplit = Regex.Split(baseWeapon.Category.ToString(), @"(?<!^)(?=[A-Z])");
            var CatergoryName = string.Join(" ", CatergorySplit);
            var BlackBlade = Helpers.CreateCopy(baseWeapon, bp => {
                bp.name = $"BlackBlade{baseWeapon.Category}";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_DisplayNameText = Helpers.CreateString($"{bp.name}.Name", "Black Blade");
                bp.m_DescriptionText = Helpers.CreateString($"{bp.name}.Description", "A black blade's enhancement bonus increases as it gains levels. " +
                    "It is +1 at level 3, +2 at level 5, +3 at level 9, +4 at level 13, and +5 at level 17. " +
                    "A black blade cannot be wielded by anyone other than its owner.");
                bp.m_Enchantments = new BlueprintWeaponEnchantmentReference[] { enchant.ToReference<BlueprintWeaponEnchantmentReference>() };
                bp.m_Destructible = false;
                bp.m_IsNotable = true;
                bp.m_Weight = 0;
                bp.m_Cost = -100_000_000;
                bp.AddComponent<ItemEntityRestrictionBlackBlade>();
                /*
                bp.AddComponent<ItemDialog>(c => {
                    c.m_Conditions = new ConditionsChecker() {
                        Conditions = new Condition[0]
                    };
                    c.m_ItemName = new Kingmaker.Localization.LocalizedString();
                    c.m_DialogReference = LexiconAssemble_BE.ToReference<BlueprintDialogReference>();
                });
                */
            });
            Resources.AddBlueprint(BlackBlade);
            var BlackBladeFeature = Helpers.CreateBlueprint<BlueprintFeature>($"{BlackBlade.name}Feature", bp => {
                bp.SetName($"{CatergoryName}");
                bp.SetDescription($"Your Black Blade takes the form of a {CatergoryName}.");
                bp.Ranks = 1;
                bp.m_Icon = BlackBlade.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddBlackBlade>(c => {
                    c.BlackBlade = BlackBlade.ToReference<BlueprintItemWeaponReference>();
                });
                bp.AddPrerequisite<PrerequisiteProficiency>(c => {
                    c.WeaponProficiencies = new WeaponCategory[] { BlackBlade.Category };
                    c.ArmorProficiencies = new ArmorProficiencyGroup[0];
                });
            });
            return BlackBladeFeature;
        }
    }
}
