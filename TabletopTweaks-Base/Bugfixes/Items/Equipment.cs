using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewActions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Items {
    static class Equipment {
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                TTTContext.Logger.LogHeader("Patching Equipment");
                PatchAmuletOfQuickDraw();
                PatchApprenticeRobe();
                PatchAspectOfTheAsp();
                PatchBoundOfPossibilityAeon();
                PatchBracersOfArchery();
                PatchMagiciansRing();
                PatchManglingFrenzy();
                PatchMetamagicRods();
                PatchHolySymbolofIomedae();
                PatchHalfOfThePair();
                PatchShapeshiftersHelm();
                PatchStormlordsResolve();
                PatchFlawlessBeltOfPhysicalPerfection8CritIncrease();
                PatchQuiverOfRosesThorns();

                void PatchAmuletOfQuickDraw() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("AmuletOfQuickDraw")) { return; }

                    var AmuletOfQuickDrawItem = BlueprintTools.GetBlueprint<BlueprintItemEquipmentNeck>("eb22f2919c30a9e4fa4e8cc3160b2432");
                    var AmuletOfQuickDrawFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("60c9144d13674a445aa303fa272aae0a");
                    var RayType = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("1d39a22f206840e40b2255fc0175b8d0");

                    AmuletOfQuickDrawFeature.TemporaryContext(bp => {
                        bp.m_DisplayName = AmuletOfQuickDrawItem.m_DisplayNameText;
                        bp.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                        bp.AddComponent<IncreaseSpellDescriptorDC>(c => {
                            c.Descriptor = SpellDescriptor.Poison;
                            c.BonusDC = 2;
                        });
                        bp.AddComponent<AddConditionalWeaponDamageBonus>(c => {
                            c.TargetConditions = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    new ContextConditionSize(){
                                        Size = Size.Large
                                    }
                                }
                            };
                            c.Descriptor = ModifierDescriptor.Insight;
                            c.Value = 2;
                            c.CheckWeaponRangeType = true;
                            c.RangeType = WeaponRangeType.Ranged;
                        });
                    });

                    TTTContext.Logger.LogPatch(AmuletOfQuickDrawFeature);
                }
                void PatchApprenticeRobe() {
                    if (TTTContext.Fixes.Items.Equipment.IsDisabled("ApprenticeRobe")) { return; }

                    var ApprenticeRobeItem = BlueprintTools.GetBlueprint<BlueprintItemEquipmentShirt>("fedef0913b7d598478918b81eed9fada");
                    var ApprenticeRobeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("3443f54b269d30540a0ab2e97005e416");
                    var ApprenticeRobeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("ec41863fa404a6149a0f0a3f435896e0");
                    var ArchmageArmor = BlueprintTools.GetBlueprint<BlueprintFeature>("c3ef5076c0feb3c4f90c229714e62cd0");
                    var MageArmor = BlueprintTools.GetBlueprint<BlueprintAbility>("9e1ad5d6f87d19e4d8883d63a6e35568");
                    var MageArmorBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a92acdf18049d784eaa8f2004f5d2304");
                    var MageArmorBuffMythic = BlueprintTools.GetBlueprint<BlueprintBuff>("355be0688dabc21409f37942d637cdab");
                    var MageArmorBuffPermanent = BlueprintTools.GetBlueprint<BlueprintBuff>("3410dd9986662684e8debdcf272e2cdc");

                    ApprenticeRobeFeature.TemporaryContext(bp => {
                        bp.SetName(ApprenticeRobeItem.m_DisplayNameText);
                        bp.SetComponents();
                        bp.AddComponent<AddStatBonusIfHasFactTTT>(c => {
                            c.Stat = StatType.AC;
                            c.Descriptor = ModifierDescriptor.ArmorFocus;
                            c.Value = 1;
                            c.m_CheckedFacts = new BlueprintUnitFactReference[] {
                                MageArmorBuff.ToReference<BlueprintUnitFactReference>(),
                                MageArmorBuffMythic.ToReference<BlueprintUnitFactReference>(),
                                MageArmorBuffPermanent.ToReference<BlueprintUnitFactReference>()
                            };
                        });
                    });
                    ApprenticeRobeBuff.TemporaryContext(bp => {
                        bp.SetName(ApprenticeRobeItem.m_DisplayNameText);
                        bp.SetComponents();
                        bp.AddComponent<AddFactContextActions>(c => {
                            c.Activated = Helpers.CreateActionList(
                                new ContextActionRemoveSelf()
                            );
                            c.Deactivated = Helpers.CreateActionList();
                            c.NewRound = Helpers.CreateActionList(
                                new ContextActionRemoveSelf()
                            );
                        });
                    });
                    MageArmorBuff.TemporaryContext(bp => {
                        bp.SetName(MageArmor.m_DisplayName);
                        bp.RemoveComponents<AddFactContextActions>();
                    });
                    MageArmorBuffMythic.TemporaryContext(bp => {
                        bp.SetName(ArchmageArmor.m_DisplayName);
                        bp.RemoveComponents<AddFactContextActions>();
                    });
                    MageArmorBuffPermanent.TemporaryContext(bp => {
                        bp.SetName(MageArmor.m_DisplayName);
                        bp.RemoveComponents<AddFactContextActions>();
                    });

                    TTTContext.Logger.LogPatch(ApprenticeRobeItem);
                    TTTContext.Logger.LogPatch(ApprenticeRobeFeature);
                    TTTContext.Logger.LogPatch(MageArmorBuff);
                    TTTContext.Logger.LogPatch(MageArmorBuffMythic);
                    TTTContext.Logger.LogPatch(MageArmorBuffPermanent);
                }
                void PatchAspectOfTheAsp() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("AspectOfTheAsp")) { return; }

                    var AspectOfTheAspItem = BlueprintTools.GetBlueprint<BlueprintItemEquipmentNeck>("7d55f6615f884bc45b85fdaa45cd7672");
                    var AspectOfTheAspFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9b3f6877efdf29a4e821c33ec830f312");
                    var RayType = BlueprintTools.GetBlueprintReference<BlueprintWeaponTypeReference>("1d39a22f206840e40b2255fc0175b8d0");

                    AspectOfTheAspFeature.TemporaryContext(bp => {
                        bp.m_DisplayName = AspectOfTheAspItem.m_DisplayNameText;
                        bp.SetComponents();
                        bp.AddComponent<IncreaseSpellDescriptorDC>(c => {
                            c.Descriptor = SpellDescriptor.Poison;
                            c.BonusDC = 2;
                        });
                        bp.AddComponent<AddOutgoingDamageTriggerTTT>(c => {
                            c.IgnoreDamageFromThisFact = true;
                            c.CheckAbilityType = true;
                            c.m_AbilityType = AbilityType.Spell;
                            c.CheckWeaponType = true;
                            c.m_WeaponType = RayType;
                            c.OncePerAttackRoll = true;
                            c.Actions = Helpers.CreateActionList(
                                Helpers.Create<ContextActionDealDamageTTT>(a => {
                                    a.DamageType = new DamageTypeDescription() {
                                        Type = DamageType.Energy,
                                        Energy = DamageEnergyType.Acid
                                    };
                                    a.Duration = new ContextDurationValue() {
                                        DiceCountValue = new ContextValue(),
                                        BonusValue = new ContextValue()
                                    };
                                    a.Value = new ContextDiceValue() {
                                        DiceType = DiceType.D6,
                                        DiceCountValue = 1,
                                        BonusValue = 5
                                    };
                                    a.IgnoreCritical = true;
                                    a.SetFactAsReason = true;
                                    a.IgnoreWeapon = true;
                                })
                            );
                        });
                    });

                    TTTContext.Logger.LogPatch(AspectOfTheAspFeature);
                }
                void PatchBoundOfPossibilityAeon() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("BoundOfPossibilityAeon")) { return; }

                    var Artifact_AeonCloakItem = BlueprintTools.GetBlueprint<BlueprintItemEquipment>("b24d6185acea1f949b026c3b58e47947");
                    var Artifact_AeonCloakArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("1e3f209f70fc4ca696a40d474945ddd1");
                    var Artifact_AeonCloakAreaBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5ed06badb95540fe855c638213b0a60b");
                    var Artifact_AeonCloakAllyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("437fd1832d75433e93675babbf9da7f0");
                    var Artifact_AeonCloakDCProperty = BlueprintTools.GetModBlueprintReference<BlueprintUnitPropertyReference>(TTTContext, "Artifact_AeonCloakDCProperty");

                    Artifact_AeonCloakArea.TemporaryContext(bp => {
                        bp.FlattenAllActions().OfType<ContextActionSavingThrow>().ForEach(save => {
                            save.HasCustomDC = true;
                            save.CustomDC = new ContextValue() {
                                ValueType = ContextValueType.CasterCustomProperty,
                                m_CustomProperty = Artifact_AeonCloakDCProperty
                            };
                        });
                    });
                    Artifact_AeonCloakAreaBuff.TemporaryContext(bp => {
                        bp.SetName(Artifact_AeonCloakItem.m_DisplayNameText);
                    });
                    Artifact_AeonCloakAllyBuff.TemporaryContext(bp => {
                        bp.SetName(Artifact_AeonCloakItem.m_DisplayNameText);
                    });

                    TTTContext.Logger.LogPatch(Artifact_AeonCloakArea);
                    TTTContext.Logger.LogPatch(Artifact_AeonCloakItem);
                }
                void PatchBracersOfArchery() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("BracersOfArchery")) { return; }

                    var BracersOfArcher = BlueprintTools.GetBlueprint<BlueprintItemEquipment>("0e7a0b96f67660c4ca74786c187a02d2");
                    var BracersOfArcheryLesser = BlueprintTools.GetBlueprint<BlueprintItemEquipment>("01d51ff5f3db2164b88aaa662a9b0f2e");
                    var ArcheryEnchantment = BlueprintTools.GetBlueprint<BlueprintEquipmentEnchantment>("366f3ce5832e547489a13ce6101d411e");
                    var ArcheryLesserEnchantment = BlueprintTools.GetBlueprint<BlueprintEquipmentEnchantment>("d695146c6c6dcfd48980406a280faca1");
                    var ArcheryBonuses = BlueprintTools.GetBlueprint<BlueprintFeature>("136adc54467964446b2e790c1698d93a");
                    var ArcheryBonusesLesser = BlueprintTools.GetBlueprint<BlueprintFeature>("1440703181bc6e54487b4673e73af34e");
                    var ShortbowProficiencyBracersOfArchery = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("b55f72cc64808a34fba65c5e0636ab18");
                    var LongbowProficiencyBracersOfArchery = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("3d14fa9e7620b134aba7e0f06aa107d6");

                    ArcheryBonusesLesser.TemporaryContext(bp => {
                        bp.SetName(BracersOfArcheryLesser.m_DisplayNameText);
                        bp.SetComponents();
                        bp.AddComponent<WeaponGroupAttackBonusTTT>(c => {
                            c.WeaponGroup = WeaponFighterGroup.Bows;
                            c.AttackBonus = 1;
                            c.Descriptor = ModifierDescriptor.Competence;
                        });
                        bp.AddComponent<AddFacts>(c => {
                            c.m_Facts = new BlueprintUnitFactReference[] {
                                ShortbowProficiencyBracersOfArchery,
                                LongbowProficiencyBracersOfArchery
                            };
                        });
                    });
                    ArcheryBonuses.TemporaryContext(bp => {
                        bp.SetName(BracersOfArcher.m_DisplayNameText);
                        bp.SetComponents();
                        bp.AddComponent<WeaponGroupAttackBonusTTT>(c => {
                            c.WeaponGroup = WeaponFighterGroup.Bows;
                            c.AttackBonus = 2;
                            c.Descriptor = ModifierDescriptor.Competence;
                        });
                        bp.AddComponent<WeaponGroupDamageBonusTTT>(c => {
                            c.WeaponGroup = WeaponFighterGroup.Bows;
                            c.DamageBonus = 1;
                            c.Descriptor = ModifierDescriptor.Competence;
                        });
                        bp.AddComponent<AddFacts>(c => {
                            c.m_Facts = new BlueprintUnitFactReference[] {
                                ShortbowProficiencyBracersOfArchery,
                                LongbowProficiencyBracersOfArchery
                            };
                        });
                    });

                    TTTContext.Logger.LogPatch(BracersOfArcheryLesser);
                    TTTContext.Logger.LogPatch(BracersOfArcher);
                }
                void PatchFlawlessBeltOfPhysicalPerfection8CritIncrease() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("FlawlessBeltOfPhysicalPerfection8CritIncrease")) { return; }

                    var BeltOfPerfection8ExtraFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e1f419f50b5a45158080bb4cb4ff6858");

                    if (BeltOfPerfection8ExtraFeature != null) {
                        BeltOfPerfection8ExtraFeature.SetComponents();
                        BeltOfPerfection8ExtraFeature.AddComponent<AddFlatCriticalRangeIncrease>(c => {
                            c.CriticalRangeIncrease = 1;
                            c.AllWeapons = true;
                        });
                        TTTContext.Logger.LogPatch(BeltOfPerfection8ExtraFeature);
                    }
                }
                void PatchMagiciansRing() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("MagiciansRing")) { return; }

                    var RingOfTheSneakyWizardFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d848f1f1b31b3e143ba4aeeecddb17f4");
                    RingOfTheSneakyWizardFeature.GetComponent<IncreaseSpellSchoolDC>().BonusDC = 2;
                    TTTContext.Logger.LogPatch(RingOfTheSneakyWizardFeature);
                }
                void PatchHalfOfThePair() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("HalfOfThePair")) { return; }

                    var HalfOfPairedPendantArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("8187fd9306b8c4f46824fbba9808f458");
                    var HalfOfPairedPendantBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("066229a41ae97d6439fea81ebf141528");
                    var HalfOfPairedPendantPersonalBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("71a14bfc21b64ad4bbb916a7ad58effb");
                    HalfOfPairedPendantArea
                        .GetComponent<AbilityAreaEffectRunAction>()
                        .Round = Helpers.CreateActionList();
                    HalfOfPairedPendantArea
                        .GetComponent<AbilityAreaEffectRunAction>()
                        .UnitEnter
                        .Actions
                        .OfType<Conditional>()
                        .FirstOrDefault()
                        .ConditionsChecker
                        .Conditions = new Condition[] {
                            new ContextConditionHasFact() {
                                m_Fact = HalfOfPairedPendantBuff.ToReference<BlueprintUnitFactReference>(),
                            },
                            new ContextConditionIsCaster() {
                                Not = true
                            }
                        };
                    HalfOfPairedPendantArea.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .ForEach(c => {
                            c.ToCaster = false;
                            c.AsChild = false;
                        });
                    HalfOfPairedPendantArea.FlattenAllActions()
                        .OfType<ContextActionRemoveBuff>()
                        .ForEach(c => c.ToCaster = false);
                    TTTContext.Logger.LogPatch(HalfOfPairedPendantArea);
                }
                void PatchHolySymbolofIomedae() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("HolySymbolofIomedae")) { return; }

                    var Artifact_HolySymbolOfIomedaeArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("e6dff35442f00ab4fa2468804c15efc0");
                    var Artifact_HolySymbolOfIomedaeBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c8b1c0f5cd21f1d4e892f7440ec28e24");
                    Artifact_HolySymbolOfIomedaeArea
                        .GetComponent<AbilityAreaEffectRunAction>()
                        .UnitExit = Helpers.CreateActionList(
                            new ContextActionRemoveBuff() {
                                m_Buff = Artifact_HolySymbolOfIomedaeBuff.ToReference<BlueprintBuffReference>()
                            }
                    );
                    Artifact_HolySymbolOfIomedaeBuff.AddComponent<AddOutgoingPhysicalDamageProperty>(c => {
                        c.m_WeaponType = new BlueprintWeaponTypeReference();
                        c.m_UnitFact = new BlueprintUnitFactReference();
                        c.Material = PhysicalDamageMaterial.ColdIron;
                        c.Alignment = DamageAlignment.Good;
                        c.AddAlignment = true;
                        c.AddMaterial = true;
                        c.AffectAnyPhysicalDamage = true;
                    });
                    TTTContext.Logger.LogPatch(Artifact_HolySymbolOfIomedaeArea);
                    TTTContext.Logger.LogPatch(Artifact_HolySymbolOfIomedaeBuff);
                }
                // Fix Mangling Frenzy does not apply to Bloodrager's Rage
                void PatchManglingFrenzy() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("ManglingFrenzy")) { return; }

                    var ManglingFrenzyFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("29e2f51e6dd7427099b015de88718990");
                    var ManglingFrenzyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("1581c5ceea24418cadc9f26ce4d391a9");
                    var BloodragerStandartRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");

                    ManglingFrenzyFeature.AddComponent(Helpers.Create<BuffExtraEffects>(c => {
                        c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                        c.m_ExtraEffectBuff = ManglingFrenzyBuff.ToReference<BlueprintBuffReference>();
                    }));

                    TTTContext.Logger.LogPatch(ManglingFrenzyFeature);
                }
                void PatchShapeshiftersHelm() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("ShapeshiftersHelm")) { return; }

                    var ShapeshiftersHelmFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("accf16e898671054ca98761247fb6d5e");
                    var ShapeshiftersHelmBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4a149c41ce0c70a40826051a53ebdc32");
                    var ShapeshiftersHelmItem = BlueprintTools.GetBlueprint<BlueprintItemEquipmentHead>("6b658e1f12d7a624ab4911873f78c694");

                    ShapeshiftersHelmFeature.TemporaryContext(bp => {
                        bp.RemoveComponents<BuffExtraEffects>();
                        WildShapeTools.WildShapeBuffs.AllReferences.ForEach(buff => {
                            bp.AddComponent<BuffExtraEffects>(c => {
                                c.m_CheckedBuff = buff;
                                c.m_ExtraEffectBuff = ShapeshiftersHelmBuff.ToReference<BlueprintBuffReference>();
                                c.m_ExceptionFact = new BlueprintUnitFactReference();
                            });
                        });
                    });
                    ShapeshiftersHelmBuff.TemporaryContext(bp => {
                        bp.m_DisplayName = ShapeshiftersHelmItem.m_DisplayNameText;
                    });
                    TTTContext.Logger.LogPatch(ShapeshiftersHelmBuff);
                    TTTContext.Logger.LogPatch(ShapeshiftersHelmFeature);
                }
                void PatchStormlordsResolve() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("StormlordsResolve")) { return; }

                    var StormlordsResolveActivatableAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("ae908f59269c54c4d83ca51a63be8db4");
                    StormlordsResolveActivatableAbility.DeactivateImmediately = true;

                    TTTContext.Logger.LogPatch(StormlordsResolveActivatableAbility);
                }
                void PatchMetamagicRods() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("MetamagicRods")) { return; }

                    BlueprintActivatableAbility[] MetamagicRodAbilities = new BlueprintActivatableAbility[] {
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("ccffef1193d04ad1a9430a8009365e81"), //MetamagicRodGreaterBolsterToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("cc266cfb106a5a3449b383a25ab364f0"), //MetamagicRodGreaterEmpowerToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("c137a17a798334c4280e1eb811a14a70"), //MetamagicRodGreaterExtendToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("78b5971c7a0b7f94db5b4d22c2224189"), //MetamagicRodGreaterMaximizeToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("5016f110e5c742768afa08224d6cde56"), //MetamagicRodGreaterPersistentToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("fca35196b3b23c346a7d1b1ce20c6f1c"), //MetamagicRodGreaterQuickenToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("cc116b4dbb96375429107ed2d88943a1"), //MetamagicRodGreaterReachToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("f0d798f5139440a8b2e72fe445678d29"), //MetamagicRodGreaterSelectiveToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("056b9f1aa5c54a7996ca8c4a00a88f88"), //MetamagicRodLesserBolsterToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("ed10ddd385a528944bccbdc4254f8392"), //MetamagicRodLesserEmpowerToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("605e64c0b4586a34494fc3471525a2e5"), //MetamagicRodLesserExtendToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("868673cd023f96945a2ee61355740a96"), //MetamagicRodLesserKineticToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("485ffd3bd7877fb4d81409b120a41076"), //MetamagicRodLesserMaximizeToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("5a87350fcc6b46328a2b345f23bbda44"), //MetamagicRodLesserPersistentToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("b8b79d4c37981194fa91771fc5376c5e"), //MetamagicRodLesserQuickenToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("7dc276169f3edd54093bf63cec5701ff"), //MetamagicRodLesserReachToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("66e68fd0b661413790e3000ede141f16"), //MetamagicRodLesserSelectiveToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("afb2e1f96933c22469168222f7dab8fb"), //MetamagicRodMasterpieceToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("6cc31148ae2d48359c02712308cb4167"), //MetamagicRodNormalBolsterToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("077ec9f9394b8b347ba2b9ec45c74739"), //MetamagicRodNormalEmpowerToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("69de70b88ca056440b44acb029a76cd7"), //MetamagicRodNormalExtendToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("3b5184a55f98f264f8b39bddd3fe0e88"), //MetamagicRodNormalMaximizeToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("9ae2e56b24404144bd911378fe541597"), //MetamagicRodNormalPersistentToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("1f390e6f38d3d5247aacb25ab3a2a6d2"), //MetamagicRodNormalQuickenToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("f0b05e39b82c3be408009e26be40bc91"), //MetamagicRodNormalReachToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("04f768c59bb947e3948ce2e7e72feecb"), //MetamagicRodNormalSelectiveToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("afb2e1f96933c22469168222f7dab8fb"), //MetamagicRodMasterpieceToggleAbility - Grandmaster's Rod
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("c43746bc20a151b4eaf1836cc6eb9a92"), //AmberLoveRodToggleAbility - Passion's Sweet Poison
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("afca601615e344446a24433202567c39"), //RodOfMysticismToggleAbility - Rod of Mysticism
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("1c70e3d9e1f74c42904c044cf001fc0b"), //SithhudsRodToggleAbility - Rod of Mortiferous Blizzard
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("741e33a3e283482387974348c0d9a4a9"), //MetamagicRodLesserIntensifiedToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("e131ba0e736a484a94173587307b646e"), //MetamagicRodNormalIntensifiedToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("950d773f2bdc45f7b5572fd82456d5d1"), //MetamagicRodGreaterIntensifiedToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("fc7bd8b05d6147aab2d8b4378801db05"), //MetamagicRodLesserPiercingToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("8911333355f9425dbbb5119f79eb58d2"), //MetamagicRodNormalPiercingToggleAbility
                        BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("ae022fc0b1aa474093067dc7a0cc170b"), //MetamagicRodGreaterPiercingToggleAbility
                    };
                    MetamagicRodAbilities.ForEach(ability => {
                        ability.IsOnByDefault = false;
                        ability.DoNotTurnOffOnRest = false;
                        TTTContext.Logger.LogPatch("Patched", ability);
                    });
                }
                void PatchQuiverOfRosesThorns() {
                    if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("QuiverOfRosesThorns")) { return; }

                    var WeakenArrowsQuiverItem = BlueprintTools.GetBlueprint<BlueprintItemEquipmentUsable>("911d0f8cad672454a94286a9feafc360");
                    var WeakenArrowsQuiverBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("784043a11f7404c42a97ba4d55417d23");
                    var WeakenArrowsQuiverEnchantment = BlueprintTools.GetBlueprint<BlueprintWeaponEnchantment>("c4fe1891ba41da346ab553a94107fd96");

                    WeakenArrowsQuiverBuff.m_Icon = WeakenArrowsQuiverItem.Icon;
                    WeakenArrowsQuiverEnchantment.AddComponent<WeaponExtraAttack>(c => {
                        c.Number = 1;
                        c.Haste = true;
                    });

                    TTTContext.Logger.LogPatch(WeakenArrowsQuiverEnchantment);
                }
            }
        }
        [HarmonyPatch(typeof(ItemStatHelper), nameof(ItemStatHelper.GetUseMagicDeviceDC))]
        static class ItemStatHelper_GetUseMagicDeviceDC_Patch {
            static void Postfix(ItemEntity item, UnitEntityData caster, ref int? __result) {
                if (Main.TTTContext.Fixes.Items.Equipment.IsDisabled("FixScrollUMDDCs")) { return; }
                BlueprintItemEquipmentUsable blueprintItemEquipmentUsable = item.Blueprint as BlueprintItemEquipmentUsable;
                if (blueprintItemEquipmentUsable == null) {
                    __result = null;
                    return;
                }
                if (caster != null && !blueprintItemEquipmentUsable.IsUnitNeedUMDForUse(caster)) {
                    __result = null;
                    return;
                }
                __result = blueprintItemEquipmentUsable.Type == UsableItemType.Scroll ? 20 + item.GetCasterLevel() : 20;
            }
        }
    }
}
