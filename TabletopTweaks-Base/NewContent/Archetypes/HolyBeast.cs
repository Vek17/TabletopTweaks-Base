using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Linq;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Archetypes {
    internal class HolyBeast {
        public static void AddHolyBeast() {

            var FavoriteEnemyOutsider = BlueprintTools.GetBlueprint<BlueprintFeature>("f643b38acc23e8e42a3ed577daeb6949");
            var OutsiderType = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("9054d3988d491d944ac144e27b6bc318");
            var DemonOfMagicFeature = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("1b466705276e3124ab43f865e282c6e8");
            var DemonOfSlaughterFeature = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("2fe24d538bbcea34cbe9e8600d92bdd2");
            var DemonOfStrengthFeature = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("561041cdb5887464883c55c75219a9dc");
            var InstantEnemyBuff = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("82574f7d14a28e64fab8867fbaa17715");
            var MasterHunterResourceDemonOfMagic = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("280ee87570be4c0aa7444a0194e030b6");
            var MasterHunterResourceDemonOfSlaughter = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("b38fd06f03ff48e5b859a9583f5b0cd8");
            var MasterHunterResourceDemonOfStrength = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("7f57ac3202f2418180535c98dd432ec0");
            var MasterHunterResourceOutsider = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("75abc6860d1146299ff802b5b0120ca5");

            var AbadarFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("6122dacf418611540a3c91e67197ee4e");
            var AreshkagalFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d714ecb5d5bb89a42957de0304e459c9");
            var AsmodeusFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("a3a5ccc9c670e6f4ca4a686d23b89900");
            var AtheismFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("92c0d2da0a836ce418a267093c09ca54");
            var BaphometFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("bd72ca8ffcfec5745899ac56c93f12c5");
            var CalistriaFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("c7531715a3f046d4da129619be63f44c");
            var CaydenCaileanFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("300e212868bca984687c92bcb66d381b");
            var DeskariFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("ddf913858bdf43b4da3b731e082fbcc0");
            var DesnaFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("2c0a3b9971327ba4d9d85354d16998c1");
            var ErastilFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("afc775188deb7a44aa4cbde03512c671");
            var GodclawFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("583a26e88031d0a4a94c8180105692a5");
            var GorumFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("8f49a5d8528a82c44b8c117a89f6b68c");
            var GozrehFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("4af983eec2d821b40a3065eb5e8c3a72");
            var GreenFaithFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("99a7a8f13c1300c42878558fa9471e2f");
            var GyronnaFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("8b535b6842e063d48a571a042c3c6e8f");
            var IomedaeFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("88d5da04361b16746bf5b65795e0c38c");
            var IroriFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("23a77a5985de08349820429ce1b5a234");
            var KabririFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("f12c1ccc9d600c04f8887cd28a8f45a5");
            var LamashtuFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("f86bc8fbf13221f4f9041608a1fb8585");
            var LichDeityFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("b4153b422d02d4f48b3f8f0ceb6a10eb");
            var NethysFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("6262cfce7c31626458325ca0909de997");
            var NorgorberFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("805b6bdc8c96f4749afc687a003f9628");
            var PharasmaFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("458750bc214ab2e44abdeae404ab22e9");
            var PuluraFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("ebb0b46f95dbac74681c78aae895dbd0");
            var RovagugFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("04bc2b62273ab744092d992ed72bff41");
            var SarenraeFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("c1c4f7f64842e7e48849e5e67be11a1b");
            var ShelynFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("b382afa31e4287644b77a8b30ed4aa0b");
            var ToragFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d2d5c5a58885a6b489727467e13c3337");
            var UrgathoaFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("812f6c07148088e41a9ac94b56ac2fc8");
            var ZonKuthonFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("f7eed400baa66a744ad361d4df0e6f1b");

            var ShifterTrackFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("1fd8085a71c94a2a94266cbeba71a562");
            var ShifterWoodlandGraceFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("0217aaa0c3f74dc39643c3e841636625");
            var ShifterClaw1d10x3 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("9788b10519734a07ac83ebe65f0dd7a4");
            var ShifterClawAbilityLevel1 = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("854127ffc3644660b1c047d0a1967a66");
            var ShifterClawAbilityLevel11 = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("297eb5e400e747c785cf98523f46db5d");
            var ShifterClawAbilityLevel13 = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("db4a89bdcc6044d6b538dc2c19cac745");
            var ShifterClawAbilityLevel17 = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("b88411e69af245c08dfbbb88d9b24112");
            var ShifterClawAbilityLevel19 = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("469e70bdd542407293ba0f00d9c672d4");
            var ShifterClawAbilityLevel3 = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("79d7262325ab48be89b2dd8194683508");
            var ShifterClawAbilityLevel7 = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("ab97ac6b88eb4a96a375a47971a994e1");
            var ShifterClawBuffLevel1 = BlueprintTools.GetBlueprint<BlueprintBuff>("02070af90de345c6a82a8cf469a65080");
            var ShifterClawBuffLevel11 = BlueprintTools.GetBlueprint<BlueprintBuff>("13243d59d212463d9ab3f36e646aa40c");
            var ShifterClawBuffLevel13 = BlueprintTools.GetBlueprint<BlueprintBuff>("6e31c78ce801444aad398248b66a22b8");
            var ShifterClawBuffLevel17 = BlueprintTools.GetBlueprint<BlueprintBuff>("cb51194e75ca45bc9fedf9a09c50b827");
            var ShifterClawBuffLevel19 = BlueprintTools.GetBlueprint<BlueprintBuff>("494d127890c3498fb3dbf3a53dcb4fe6");
            var ShifterClawBuffLevel3 = BlueprintTools.GetBlueprint<BlueprintBuff>("1bb67316c37e400888e0489ee8d64067");
            var ShifterClawBuffLevel7 = BlueprintTools.GetBlueprint<BlueprintBuff>("c9441167a3b84fb48729e55f29a9df64");
            var ShifterClawsFeatureAddLevel = BlueprintTools.GetBlueprint<BlueprintFeature>("08a8cfba6ae34505a64d6ba00225c4d2");
            var ShifterClawsFeatureAddLevel1 = BlueprintTools.GetBlueprint<BlueprintFeature>("f7996c5b51e348fc9277480d9cc0a88c");
            var ShifterClawsFeatureAddLevel2 = BlueprintTools.GetBlueprint<BlueprintFeature>("19b7335626b3434cbe2af01fb33582ff");
            var ShifterClawsFeatureAddLevel3 = BlueprintTools.GetBlueprint<BlueprintFeature>("8d6b338131764a4fb68eaf5b5c6cfe47");
            var ShifterClawsFeatureAddLevel4 = BlueprintTools.GetBlueprint<BlueprintFeature>("024b8248f85d412cb7c520a9f746c547");
            var ShifterClawsFeatureAddLevel5 = BlueprintTools.GetBlueprint<BlueprintFeature>("76b8314a83ff4825a145ac8f7b59d6e4");
            var ShifterClawsFeatureAddLevel6 = BlueprintTools.GetBlueprint<BlueprintFeature>("28991899db1948d9bdd5f958b4add2d8");
            var ShifterClawsFeatureLevel1 = BlueprintTools.GetBlueprint<BlueprintFeature>("512f845e29514539b1943b74633dba24");
            var ShifterClawsFeatureLevel11 = BlueprintTools.GetBlueprint<BlueprintFeature>("634b57e9dd184ad7aebdf435099f78c5");
            var ShifterClawsFeatureLevel13 = BlueprintTools.GetBlueprint<BlueprintFeature>("22e1ef0d336f42d0bc3132af882b8bd6");
            var ShifterClawsFeatureLevel17 = BlueprintTools.GetBlueprint<BlueprintFeature>("ae6f3511e5df49e4a9555853c47ab737");
            var ShifterClawsFeatureLevel19 = BlueprintTools.GetBlueprint<BlueprintFeature>("b8d5234c11534ab8aaa37e23039b35e6");
            var ShifterClawsFeatureLevel3 = BlueprintTools.GetBlueprint<BlueprintFeature>("43b4b8191678405894c4ce0c7176dcb7");
            var ShifterClawsFeatureLevel7 = BlueprintTools.GetBlueprint<BlueprintFeature>("3f3f9bb5d1ec40f2a487b02e599eb8f1");
            var ShifterClawVisualBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b09147e9b63b49b89c90361fbad90a68");
            var ClawType = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("d4f7aee36efe0b54e810c9d3407b6ab3");

            Sprite BlessedClawsFeatureIcon = ShifterClawsFeatureAddLevel.Icon;

            var HolyBeastArchetype = Helpers.CreateBlueprint<BlueprintArchetype>(TTTContext, "HolyBeastArchetype", bp => {
                bp.SetName(TTTContext, "Holy Beast");
                bp.SetDescription(TTTContext, "Thousands of gods are venerated in Vudra, " +
                    "and devoted followers might have personal relationships with their chosen deities. " +
                    "Holy beast shifters pledge to hunt down their deity’s enemies to earn that deity’s blessing. " +
                    "These shifters tend to take on the aspect of their deity’s sacred animal, and many of them seek to destroy rakshasas.");
                bp.AddComponent<PrerequisiteNoFeature>(c => {
                    c.m_Feature = AtheismFeature;
                });
                bp.AddComponent<PrerequisiteNoFeature>(c => {
                    c.m_Feature = GreenFaithFeature;
                });
                bp.AddComponent<PrerequisiteNoFeature>(c => {
                    c.m_Feature = GodclawFeature;
                });
            });
            var HolyBeastDivineFury = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "HolyBeastDivineFury", bp => {
                bp.SetName(TTTContext, "Divine Fury");
                bp.SetDescription(TTTContext, "A holy beast shifter focuses entirely on hunting down specific outsiders on behalf of her deity. " +
                    "She gains the ranger’s favored enemy class feature against outsiders except the bonus starts at 1 instead of 2. " +
                    "The bonus against her favored enemy automatically increases by 1 at 5th level and every 5 levels thereafter.");
                bp.m_Icon = FavoriteEnemyOutsider.m_Icon;
                bp.AddComponent<FavoredEnemyTTT>(c => {
                    c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                        OutsiderType,
                        DemonOfMagicFeature,
                        DemonOfSlaughterFeature,
                        DemonOfStrengthFeature,
                        InstantEnemyBuff
                    };
                    c.ValuePerRank = 1;
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = MasterHunterResourceDemonOfMagic;
                    c.RestoreAmount = true;
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = MasterHunterResourceDemonOfSlaughter;
                    c.RestoreAmount = true;
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = MasterHunterResourceDemonOfStrength;
                    c.RestoreAmount = true;
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = MasterHunterResourceOutsider;
                    c.RestoreAmount = true;
                });
            });
            var BlessedClawsGoodFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BlessedClawsGoodFeature", bp => {
                bp.SetName(TTTContext, "Blessed Claws — Good");
                bp.SetDescription(TTTContext, "At 3rd level, her claws are treated as one type of aligned weapon " +
                    "(chaotic, evil, good, or lawful) within one step of her deity’s alignment in addition to ignoring DR/cold iron, DR/magic, and DR/silver.");
                bp.m_Icon = BlessedClawsFeatureIcon;
                bp.AddComponent<PrerequisiteFeaturesFromList>(c => {
                    c.HideInUI = true;
                    c.m_Features = new BlueprintFeatureReference[] {
                        AbadarFeature,
                        CalistriaFeature,
                        CaydenCaileanFeature,
                        DesnaFeature,
                        GorumFeature,
                        GozrehFeature,
                        IroriFeature,
                        NethysFeature,
                        PharasmaFeature,
                        SarenraeFeature,
                        ShelynFeature,
                        ErastilFeature,
                        IomedaeFeature,
                        ToragFeature
                    };
                });
            });
            var BlessedClawsEvilFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BlessedClawsEvilFeature", bp => {
                bp.SetName(TTTContext, "Blessed Claws — Evil");
                bp.SetDescription(TTTContext, "At 3rd level, her claws are treated as one type of aligned weapon " +
                    "(chaotic, evil, good, or lawful) within one step of her deity’s alignment in addition to ignoring DR/cold iron, DR/magic, and DR/silver.");
                bp.AddComponent<PrerequisiteFeaturesFromList>(c => {
                    c.HideInUI = true;
                    c.m_Features = new BlueprintFeatureReference[] { 
                        AbadarFeature,
                        CalistriaFeature,
                        GorumFeature,
                        GozrehFeature,
                        GyronnaFeature,
                        IroriFeature,
                        LamashtuFeature,
                        NethysFeature,
                        NorgorberFeature,
                        PharasmaFeature,
                        RovagugFeature,
                        UrgathoaFeature,
                        AsmodeusFeature,
                        ZonKuthonFeature
                    };
                });
                bp.m_Icon = BlessedClawsFeatureIcon;
            });
            var BlessedClawsLawFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BlessedClawsLawFeature", bp => {
                bp.SetName(TTTContext, "Blessed Claws — Lawful");
                bp.SetDescription(TTTContext, "At 3rd level, her claws are treated as one type of aligned weapon " +
                    "(chaotic, evil, good, or lawful) within one step of her deity’s alignment in addition to ignoring DR/cold iron, DR/magic, and DR/silver.");
                bp.m_Icon = BlessedClawsFeatureIcon;
                bp.AddComponent<PrerequisiteFeaturesFromList>(c => {
                    c.HideInUI = true;
                    c.m_Features = new BlueprintFeatureReference[] {
                        AbadarFeature,
                        IroriFeature,
                        AsmodeusFeature,
                        ErastilFeature,
                        IomedaeFeature,
                        ToragFeature,
                        ZonKuthonFeature,
                        GozrehFeature,
                        NethysFeature,
                        PharasmaFeature,
                        SarenraeFeature,
                        ShelynFeature,
                        UrgathoaFeature,

                    };
                });
            });
            var BlessedClawsChaosFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BlessedClawsChaosFeature", bp => {
                bp.SetName(TTTContext, "Blessed Claws — Chaotic");
                bp.SetDescription(TTTContext, "At 3rd level, her claws are treated as one type of aligned weapon " +
                    "(chaotic, evil, good, or lawful) within one step of her deity’s alignment in addition to ignoring DR/cold iron, DR/magic, and DR/silver.");
                bp.m_Icon = BlessedClawsFeatureIcon;
                bp.AddComponent<PrerequisiteFeaturesFromList>(c => {
                    c.HideInUI = true;
                    c.m_Features = new BlueprintFeatureReference[] {
                        CalistriaFeature,
                        CaydenCaileanFeature,
                        DesnaFeature,
                        GorumFeature,
                        GozrehFeature,
                        GyronnaFeature,
                        LamashtuFeature,
                        NethysFeature,
                        NorgorberFeature,
                        PharasmaFeature,
                        RovagugFeature,
                        SarenraeFeature,
                        ShelynFeature,
                        UrgathoaFeature
                    };
                });
            });
            var BlessedClawsGoodBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BlessedClawsGoodBuff", bp => {
                bp.SetName(TTTContext, "Blessed Claws — Good");
                bp.SetDescription(TTTContext, "At 3rd level, her claws are treated as one type of aligned weapon " +
                    "(chaotic, evil, good, or lawful) within one step of her deity’s alignment in addition to ignoring DR/cold iron, DR/magic, and DR/silver.");
                bp.m_Icon = BlessedClawsFeatureIcon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.AddComponent<AddOutgoingPhysicalDamageProperty>(c => {
                    c.CheckWeaponType = true;
                    c.m_WeaponType = ClawType;
                    c.AddAlignment = true;
                    c.Alignment = DamageAlignment.Good;
                });
            });
            var BlessedClawsEvilBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BlessedClawsEvilBuff", bp => {
                bp.SetName(TTTContext, "Blessed Claws — Evil");
                bp.SetDescription(TTTContext, "At 3rd level, her claws are treated as one type of aligned weapon " +
                    "(chaotic, evil, good, or lawful) within one step of her deity’s alignment in addition to ignoring DR/cold iron, DR/magic, and DR/silver.");
                bp.m_Icon = BlessedClawsFeatureIcon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.AddComponent<AddOutgoingPhysicalDamageProperty>(c => {
                    c.CheckWeaponType = true;
                    c.m_WeaponType = ClawType;
                    c.AddAlignment = true;
                    c.Alignment = DamageAlignment.Evil;
                });
            });
            var BlessedClawsLawBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BlessedClawsLawBuff", bp => {
                bp.SetName(TTTContext, "Blessed Claws — Lawful");
                bp.SetDescription(TTTContext, "At 3rd level, her claws are treated as one type of aligned weapon " +
                    "(chaotic, evil, good, or lawful) within one step of her deity’s alignment in addition to ignoring DR/cold iron, DR/magic, and DR/silver.");
                bp.m_Icon = BlessedClawsFeatureIcon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.AddComponent<AddOutgoingPhysicalDamageProperty>(c => {
                    c.CheckWeaponType = true;
                    c.m_WeaponType = ClawType;
                    c.AddAlignment = true;
                    c.Alignment = DamageAlignment.Lawful;
                });
            });
            var BlessedClawsChaosBuff = Helpers.CreateBlueprint<BlueprintBuff>(TTTContext, "BlessedClawsChaosBuff", bp => {
                bp.SetName(TTTContext, "Blessed Claws — Chaotic");
                bp.SetDescription(TTTContext, "At 3rd level, her claws are treated as one type of aligned weapon " +
                    "(chaotic, evil, good, or lawful) within one step of her deity’s alignment in addition to ignoring DR/cold iron, DR/magic, and DR/silver.");
                bp.m_Icon = BlessedClawsFeatureIcon;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
                bp.AddComponent<AddOutgoingPhysicalDamageProperty>(c => {
                    c.CheckWeaponType = true;
                    c.m_WeaponType = ClawType;
                    c.AddAlignment = true;
                    c.Alignment = DamageAlignment.Chaotic;
                });
            });

            var BlessedClawsFeatureAddLevel = CreateBlessedClawsFeature(
                ShifterClawsFeatureAddLevel,
                ShifterClawsFeatureLevel1,
                ShifterClawBuffLevel1,
                ShifterClawAbilityLevel1,
                DiceType.D4,
                append: "-Base"
            );
            var BlessedClawsFeatureAddLevel1 = CreateBlessedClawsFeatureSelection(
                ShifterClawsFeatureAddLevel1,
                ShifterClawsFeatureLevel3,
                ShifterClawBuffLevel3,
                ShifterClawAbilityLevel3,
                DiceType.D4
            );
            var BlessedClawsFeatureAddLevel2 = CreateBlessedClawsFeature(
                ShifterClawsFeatureAddLevel2,
                ShifterClawsFeatureLevel7,
                ShifterClawBuffLevel7,
                ShifterClawAbilityLevel7,
                DiceType.D6
            );
            var BlessedClawsFeatureAddLevel3 = CreateBlessedClawsFeature(
                ShifterClawsFeatureAddLevel3,
                ShifterClawsFeatureLevel11,
                ShifterClawBuffLevel11,
                ShifterClawAbilityLevel11,
                DiceType.D8
            );
            var BlessedClawsFeatureAddLevel4 = CreateBlessedClawsFeature(
                ShifterClawsFeatureAddLevel4,
                ShifterClawsFeatureLevel13,
                ShifterClawBuffLevel13,
                ShifterClawAbilityLevel13,
                DiceType.D10
            );
            var BlessedClawsFeatureAddLevel5 = CreateBlessedClawsFeature(
                ShifterClawsFeatureAddLevel5,
                ShifterClawsFeatureLevel17,
                ShifterClawBuffLevel17,
                ShifterClawAbilityLevel17,
                DiceType.D10,
                true
            );
            var BlessedClawsFeatureAddLevel6 = CreateBlessedClawsFeature(
                ShifterClawsFeatureAddLevel6,
                ShifterClawsFeatureLevel19,
                ShifterClawBuffLevel19,
                ShifterClawAbilityLevel19,
                DiceType.D10,
                true,
                "-Duplicate"
            );
            // CREATE ITEM BOND
            HolyBeastArchetype.RemoveFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, ShifterClawsFeatureAddLevel.ToReference<BlueprintFeatureBaseReference>()),
                Helpers.CreateLevelEntry(2, ShifterTrackFeature),
                Helpers.CreateLevelEntry(3, ShifterClawsFeatureAddLevel1.ToReference<BlueprintFeatureBaseReference>(), ShifterWoodlandGraceFeature),
                Helpers.CreateLevelEntry(7, ShifterClawsFeatureAddLevel2.ToReference<BlueprintFeatureBaseReference>()),
                Helpers.CreateLevelEntry(11, ShifterClawsFeatureAddLevel3.ToReference<BlueprintFeatureBaseReference>()),
                Helpers.CreateLevelEntry(13, ShifterClawsFeatureAddLevel4.ToReference<BlueprintFeatureBaseReference>()),
                Helpers.CreateLevelEntry(17, ShifterClawsFeatureAddLevel5.ToReference<BlueprintFeatureBaseReference>()),
                Helpers.CreateLevelEntry(19, ShifterClawsFeatureAddLevel6.ToReference<BlueprintFeatureBaseReference>()),
            };
            HolyBeastArchetype.AddFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, BlessedClawsFeatureAddLevel, HolyBeastDivineFury),
                Helpers.CreateLevelEntry(3, BlessedClawsFeatureAddLevel1),
                Helpers.CreateLevelEntry(5, HolyBeastDivineFury),
                Helpers.CreateLevelEntry(7, BlessedClawsFeatureAddLevel2),
                Helpers.CreateLevelEntry(10, HolyBeastDivineFury),
                Helpers.CreateLevelEntry(11, BlessedClawsFeatureAddLevel3),
                Helpers.CreateLevelEntry(13, BlessedClawsFeatureAddLevel4),
                Helpers.CreateLevelEntry(15, HolyBeastDivineFury),
                Helpers.CreateLevelEntry(17, BlessedClawsFeatureAddLevel5),
                Helpers.CreateLevelEntry(19, BlessedClawsFeatureAddLevel6),
                Helpers.CreateLevelEntry(20, HolyBeastDivineFury)
            };

            BlueprintFeature CreateBlessedClawsFeature(
                BlueprintFeature AddLevelFeature,
                BlueprintFeature Feature,
                BlueprintBuff Buff,
                BlueprintActivatableAbility Ability,
                DiceType Dice,
                bool ImprovedCritical = false,
                string append = ""
            ) {
                var BlessedClawsWeapon = ShifterClaw1d10x3.CreateCopy(TTTContext, $"BlessedClaw1{Dice}{(ImprovedCritical ? "x3" : "")}{append}", bp => {
                    bp.m_OverrideDamageDice = true;
                    bp.m_DamageDice = new DiceFormula(1, Dice);
                    bp.m_OverrideDamageType = true;
                    bp.m_DamageType = new DamageTypeDescription() {
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData() {
                            Form = PhysicalDamageForm.Piercing
                        }
                    };
                    if (!ImprovedCritical) {
                        bp.m_Enchantments = new BlueprintWeaponEnchantmentReference[0];
                    }
                });
                var BlessedClawsBuff = Buff.CreateCopy(TTTContext, $"{Buff.name.Replace("ShifterClaw", "BlessedClaw")}", bp => {
                    bp.SetName(TTTContext, "Blessed Claws");
                    bp.GetComponent<EmptyHandWeaponOverride>()?.TemporaryContext(c => { 
                        c.m_Weapon = BlessedClawsWeapon.ToReference<BlueprintItemWeaponReference>();
                    });
                    bp.GetComponent<AddFactContextActions>()?.TemporaryContext(c => {
                        c.Activated.AddAction(
                            Helpers.Create<Conditional>(condition => {
                                condition.ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasFact(){
                                            m_Fact = BlessedClawsGoodFeature.ToReference<BlueprintUnitFactReference>()
                                        }
                                    }
                                };
                                condition.IfTrue = Helpers.CreateActionList(
                                    new ContextActionApplyBuff() { 
                                        m_Buff = BlessedClawsGoodBuff.ToReference<BlueprintBuffReference>(),
                                        DurationValue = new ContextDurationValue() { 
                                            DiceCountValue = 0,
                                            BonusValue = 1
                                        },
                                        SameDuration = true,
                                        AsChild = true
                                    }    
                                );
                                condition.IfFalse = Helpers.CreateActionList();
                            }),
                            Helpers.Create<Conditional>(condition => {
                                condition.ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasFact(){
                                            m_Fact = BlessedClawsEvilFeature.ToReference<BlueprintUnitFactReference>()
                                        }
                                    }
                                };
                                condition.IfTrue = Helpers.CreateActionList(
                                    new ContextActionApplyBuff() {
                                        m_Buff = BlessedClawsEvilBuff.ToReference<BlueprintBuffReference>(),
                                        DurationValue = new ContextDurationValue() {
                                            DiceCountValue = 0,
                                            BonusValue = 1
                                        },
                                        SameDuration = true,
                                        AsChild = true
                                    }
                                );
                                condition.IfFalse = Helpers.CreateActionList();
                            }),
                            Helpers.Create<Conditional>(condition => {
                                condition.ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasFact(){
                                            m_Fact = BlessedClawsLawFeature.ToReference<BlueprintUnitFactReference>()
                                        }
                                    }
                                };
                                condition.IfTrue = Helpers.CreateActionList(
                                    new ContextActionApplyBuff() {
                                        m_Buff = BlessedClawsLawBuff.ToReference<BlueprintBuffReference>(),
                                        DurationValue = new ContextDurationValue() {
                                            DiceCountValue = 0,
                                            BonusValue = 1
                                        },
                                        SameDuration = true,
                                        AsChild = true
                                    }
                                );
                                condition.IfFalse = Helpers.CreateActionList();
                            }),
                            Helpers.Create<Conditional>(condition => {
                                condition.ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasFact(){
                                            m_Fact = BlessedClawsChaosFeature.ToReference<BlueprintUnitFactReference>()
                                        }
                                    }
                                };
                                condition.IfTrue = Helpers.CreateActionList(
                                    new ContextActionApplyBuff() {
                                        m_Buff = BlessedClawsChaosBuff.ToReference<BlueprintBuffReference>(),
                                        DurationValue = new ContextDurationValue() {
                                            DiceCountValue = 0,
                                            BonusValue = 1
                                        },
                                        SameDuration = true,
                                        AsChild = true
                                    }
                                );
                                condition.IfFalse = Helpers.CreateActionList();
                            })
                        );
                        c.Deactivated.AddAction(
                            Helpers.Create<ContextActionRemoveBuff>(a => {
                                a.m_Buff = BlessedClawsGoodBuff.ToReference<BlueprintBuffReference>();
                            }),
                            Helpers.Create<ContextActionRemoveBuff>(a => {
                                a.m_Buff = BlessedClawsEvilBuff.ToReference<BlueprintBuffReference>();
                            }),
                            Helpers.Create<ContextActionRemoveBuff>(a => {
                                a.m_Buff = BlessedClawsLawBuff.ToReference<BlueprintBuffReference>();
                            }), 
                            Helpers.Create<ContextActionRemoveBuff>(a => {
                                a.m_Buff = BlessedClawsChaosBuff.ToReference<BlueprintBuffReference>();
                            })
                        );
                    });
                });
                var BlessedClawsAbility = Ability.CreateCopy(TTTContext, $"{Ability.name.Replace("ShifterClaw", "BlessedClaw")}", bp => {
                    bp.SetName(TTTContext, "Blessed Claws");
                    bp.m_Buff = BlessedClawsBuff.ToReference<BlueprintBuffReference>();
                });
                var BlessedClawsFeature = Feature.CreateCopy(TTTContext, $"{Feature.name.Replace("ShifterClaw", "BlessedClaw")}", bp => {
                    bp.SetName(TTTContext, "Blessed Claws");
                    bp.GetComponent<AddFacts>()?.TemporaryContext(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] {
                            BlessedClawsAbility.ToReference<BlueprintUnitFactReference>()
                        };
                    });
                });
                return AddLevelFeature.CreateCopy(TTTContext, $"{AddLevelFeature.name.Replace("ShifterClaw", "BlessedClaw")}", bp => {
                    bp.SetName(TTTContext, "Blessed Claws");
                    int level = bp.GetComponents<AddFeatureOnClassLevel>()?.First().Level ?? 1;
                    bp.RemoveComponents<AddFeatureOnClassLevel>();
                    bp.AddComponent<AddFeatureOnClassLevel>(c => {
                        c.m_Class = ClassTools.ClassReferences.ShifterClass;
                        c.m_Feature = BlessedClawsFeature.ToReference<BlueprintFeatureReference>();
                        c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
                        c.m_Archetypes = new BlueprintArchetypeReference[0];
                        c.Level = level;
                        c.BeforeThisLevel = true;
                    });
                });
            }
            BlueprintFeatureSelection CreateBlessedClawsFeatureSelection(
                BlueprintFeature AddLevelFeature,
                BlueprintFeature Feature,
                BlueprintBuff Buff,
                BlueprintActivatableAbility Ability,
                DiceType Dice,
                bool ImprovedCritical = false
            ) {
                var BlessedClawsWeapon = ShifterClaw1d10x3.CreateCopy(TTTContext, $"BlessedClaw1{Dice}", bp => {
                    bp.m_DamageDice = new DiceFormula(1, Dice);
                    bp.m_DamageType = new DamageTypeDescription() { 
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData() { 
                            Form = PhysicalDamageForm.Piercing
                        }
                    };
                    if (!ImprovedCritical) {
                        bp.m_Enchantments = new BlueprintWeaponEnchantmentReference[0];
                    }
                });
                var BlessedClawsBuff = Buff.CreateCopy(TTTContext, $"{Buff.name.Replace("ShifterClaw", "BlessedClaw")}", bp => {
                    bp.SetName(TTTContext, "Blessed Claws");
                    bp.RemoveComponents<EmptyHandWeaponOverride>();
                    bp.AddComponent<EmptyHandWeaponOverride>(c => {
                        c.m_Weapon = BlessedClawsWeapon.ToReference<BlueprintItemWeaponReference>();
                    });
                    bp.GetComponent<AddFactContextActions>()?.TemporaryContext(c => {
                        c.Activated.AddAction(
                            Helpers.Create<Conditional>(condition => {
                                condition.ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasFact(){
                                            m_Fact = BlessedClawsGoodFeature.ToReference<BlueprintUnitFactReference>()
                                        }
                                    }
                                };
                                condition.IfTrue = Helpers.CreateActionList(
                                    new ContextActionApplyBuff() {
                                        m_Buff = BlessedClawsGoodBuff.ToReference<BlueprintBuffReference>(),
                                        DurationValue = new ContextDurationValue() {
                                            DiceCountValue = 0,
                                            BonusValue = 1
                                        },
                                        SameDuration = true,
                                        AsChild = true
                                    }
                                );
                                condition.IfFalse = Helpers.CreateActionList();
                            }),
                            Helpers.Create<Conditional>(condition => {
                                condition.ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasFact(){
                                            m_Fact = BlessedClawsEvilFeature.ToReference<BlueprintUnitFactReference>()
                                        }
                                    }
                                };
                                condition.IfTrue = Helpers.CreateActionList(
                                    new ContextActionApplyBuff() {
                                        m_Buff = BlessedClawsEvilBuff.ToReference<BlueprintBuffReference>(),
                                        DurationValue = new ContextDurationValue() {
                                            DiceCountValue = 0,
                                            BonusValue = 1
                                        },
                                        SameDuration = true,
                                        AsChild = true
                                    }
                                );
                                condition.IfFalse = Helpers.CreateActionList();
                            }),
                            Helpers.Create<Conditional>(condition => {
                                condition.ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasFact(){
                                            m_Fact = BlessedClawsLawFeature.ToReference<BlueprintUnitFactReference>()
                                        }
                                    }
                                };
                                condition.IfTrue = Helpers.CreateActionList(
                                    new ContextActionApplyBuff() {
                                        m_Buff = BlessedClawsLawBuff.ToReference<BlueprintBuffReference>(),
                                        DurationValue = new ContextDurationValue() {
                                            DiceCountValue = 0,
                                            BonusValue = 1
                                        },
                                        SameDuration = true,
                                        AsChild = true
                                    }
                                );
                                condition.IfFalse = Helpers.CreateActionList();
                            }),
                            Helpers.Create<Conditional>(condition => {
                                condition.ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasFact(){
                                            m_Fact = BlessedClawsChaosFeature.ToReference<BlueprintUnitFactReference>()
                                        }
                                    }
                                };
                                condition.IfTrue = Helpers.CreateActionList(
                                    new ContextActionApplyBuff() {
                                        m_Buff = BlessedClawsChaosBuff.ToReference<BlueprintBuffReference>(),
                                        DurationValue = new ContextDurationValue() {
                                            DiceCountValue = 0,
                                            BonusValue = 1
                                        },
                                        SameDuration = true,
                                        AsChild = true
                                    }
                                );
                                condition.IfFalse = Helpers.CreateActionList();
                            })
                        );
                        c.Deactivated.AddAction(
                            Helpers.Create<ContextActionRemoveBuff>(a => {
                                a.m_Buff = BlessedClawsGoodBuff.ToReference<BlueprintBuffReference>();
                            }),
                            Helpers.Create<ContextActionRemoveBuff>(a => {
                                a.m_Buff = BlessedClawsEvilBuff.ToReference<BlueprintBuffReference>();
                            }),
                            Helpers.Create<ContextActionRemoveBuff>(a => {
                                a.m_Buff = BlessedClawsLawBuff.ToReference<BlueprintBuffReference>();
                            }),
                            Helpers.Create<ContextActionRemoveBuff>(a => {
                                a.m_Buff = BlessedClawsChaosBuff.ToReference<BlueprintBuffReference>();
                            })
                        );
                    });
                });
                var BlessedClawsAbility = Ability.CreateCopy(TTTContext, $"{Ability.name.Replace("ShifterClaw", "BlessedClaw")}", bp => {
                    bp.SetName(TTTContext, "Blessed Claws");
                    bp.m_Buff = BlessedClawsBuff.ToReference<BlueprintBuffReference>();
                });
                var BlessedClawsFeature = Feature.CreateCopy(TTTContext, $"{Feature.name.Replace("ShifterClaw", "BlessedClaw")}", bp => {
                    bp.SetName(TTTContext, "Blessed Claws");
                    bp.RemoveComponents<AddFacts>();
                    bp.AddComponent<AddFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] {
                            BlessedClawsAbility.ToReference<BlueprintUnitFactReference>()
                        };
                    });
                });
                return Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "BlessedClawsSelection", bp => {
                    bp.SetName(TTTContext, "Blessed Claws");
                    bp.SetDescription(TTTContext, "At 3rd level, her claws are treated as one type of aligned weapon " +
                    "(chaotic, evil, good, or lawful) within one step of her deity’s alignment in addition to ignoring DR/cold iron, DR/magic, and DR/silver.");
                    bp.m_Icon = BlessedClawsFeatureIcon;
                    bp.AddFeatures(
                        BlessedClawsGoodFeature,
                        BlessedClawsEvilFeature,
                        BlessedClawsLawFeature,
                        BlessedClawsChaosFeature
                    );
                    int level = AddLevelFeature.GetComponents<AddFeatureOnClassLevel>()?.First().Level ?? 7;
                    bp.AddComponent<AddFeatureOnClassLevel>(c => {
                        c.m_Class = ClassTools.ClassReferences.ShifterClass;
                        c.m_Feature = BlessedClawsFeature.ToReference<BlueprintFeatureReference>();
                        c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
                        c.m_Archetypes = new BlueprintArchetypeReference[0];
                        c.Level = level;
                        c.BeforeThisLevel = true;
                    });
                });
            }

            if (TTTContext.AddedContent.Archetypes.IsDisabled("HolyBeast")) { return; }
            ClassTools.Classes.ShifterClass.m_Archetypes = ClassTools.Classes.ShifterClass.m_Archetypes.AppendToArray(HolyBeastArchetype.ToReference<BlueprintArchetypeReference>());
            ClassTools.Classes.ShifterClass.Progression.UIGroups = ClassTools.Classes.ShifterClass.Progression.UIGroups.AppendToArray(
                Helpers.CreateUIGroup(HolyBeastDivineFury)
            );
            ClassTools.Classes.ShifterClass.Progression.UIGroups = ClassTools.Classes.ShifterClass.Progression.UIGroups.AppendToArray(
                Helpers.CreateUIGroup(
                    BlessedClawsFeatureAddLevel,
                    BlessedClawsFeatureAddLevel1,
                    BlessedClawsGoodFeature,
                    BlessedClawsEvilFeature,
                    BlessedClawsLawFeature,
                    BlessedClawsChaosFeature,
                    BlessedClawsFeatureAddLevel2,
                    BlessedClawsFeatureAddLevel3,
                    BlessedClawsFeatureAddLevel4,
                    BlessedClawsFeatureAddLevel5,
                    BlessedClawsFeatureAddLevel6
                )
            );
            AtheismFeature.Get().AddPrerequisite<PrerequisiteNoArchetype>(c => {
                c.m_CharacterClass = ClassTools.ClassReferences.ShifterClass;
                c.m_Archetype = HolyBeastArchetype.ToReference<BlueprintArchetypeReference>();
            });
            GodclawFeature.Get().AddPrerequisite<PrerequisiteNoArchetype>(c => {
                c.m_CharacterClass = ClassTools.ClassReferences.ShifterClass;
                c.m_Archetype = HolyBeastArchetype.ToReference<BlueprintArchetypeReference>();
            });
            GreenFaithFeature.Get().AddPrerequisite<PrerequisiteNoArchetype>(c => {
                c.m_CharacterClass = ClassTools.ClassReferences.ShifterClass;
                c.m_Archetype = HolyBeastArchetype.ToReference<BlueprintArchetypeReference>();
            });
            TTTContext.Logger.LogPatch("Added", HolyBeastArchetype);
        }
    }
}
