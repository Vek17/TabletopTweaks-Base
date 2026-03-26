using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Bloodrager {
        [PatchBlueprintsCacheInit]
        static class Bloodrager_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Bloodrager")) { return; }

                var BloodragerMightyBloodrage = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("a6cd3eca05ee24840ab159ca47b4cd88");
                var BloodragerAlternateCapstone = NewContent.AlternateCapstones.Bloodrager.BloodragerAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                BloodragerMightyBloodrage.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.BloodragerClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == BloodragerMightyBloodrage.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(BloodragerAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(BloodragerAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == BloodragerMightyBloodrage.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(BloodragerAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Bloodrager");

                PatchBaseClass();
                PatchPrimalist();
                PatchReformedFiend();
                PatchArcaneBloodrage();
                PatchGreaterArcaneBloodrage();
                PatchTrueArcaneBloodrage();
                PatchDisruptiveBloodrage();
                PatchCastersBane();
            }
            static void PatchBaseClass() {
                PatchSpellbook();
                PatchAbyssalBulk();
                PatchAbyssalBloodrage();
                PatchTempHP();

                void PatchAbyssalBulk() {
                    if (TTTContext.Fixes.Bloodrager.Base.IsDisabled("AbyssalBulk")) { return; }
                    var BloodragerAbyssalBloodlineBaseBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2ba7b4b3b87156543b43d0686404655a");
                    var BloodragerAbyssalDemonicBulkBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("031a8053a7c02ab42ad53f50dd2e9437");
                    var BloodragerAbyssalDemonicBulkEnlargeBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(TTTContext, "BloodragerAbyssalDemonicBulkEnlargeBuff");

                    var ApplyBuff = new ContextActionApplyBuff() {
                        m_Buff = BloodragerAbyssalDemonicBulkEnlargeBuff.ToReference<BlueprintBuffReference>(),
                        AsChild = true,
                        Permanent = true
                    };
                    var RemoveBuff = new ContextActionRemoveBuff() {
                        m_Buff = BloodragerAbyssalDemonicBulkEnlargeBuff.ToReference<BlueprintBuffReference>()
                    };
                    var AddFactContext = BloodragerAbyssalBloodlineBaseBuff.GetComponent<AddFactContextActions>();

                    AddFactContext.Activated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().AddActionIfTrue(ApplyBuff);
                    AddFactContext.Deactivated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().IfTrue = null;
                    AddFactContext.Deactivated.Actions.OfType<Conditional>().Where(a => a.Comment.Equals("Demonic Bulk")).First().AddActionIfTrue(RemoveBuff);

                    TTTContext.Logger.LogPatch(BloodragerAbyssalBloodlineBaseBuff);
                }
                void PatchAbyssalBloodrage() {
                    if (TTTContext.Fixes.Bloodrager.Base.IsDisabled("AbyssalBloodrage")) { return; }

                    var BloodragerAbyssalBloodrageBonusProperty = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("415f71e5a47f4cccb3dbd10bd7a0f8f8");

                    BloodragerAbyssalBloodrageBonusProperty.TemporaryContext(bp => {
                        bp.BaseValue = 1;
                    });

                    TTTContext.Logger.LogPatch(BloodragerAbyssalBloodrageBonusProperty);
                }
                void PatchSpellbook() {
                    if (TTTContext.Fixes.Bloodrager.Base.IsDisabled("Spellbook")) { return; }
                    BlueprintSpellbook BloodragerSpellbook = BlueprintTools.GetBlueprint<BlueprintSpellbook>("e19484252c2f80e4a9439b3681b20f00");
                    var BloodragerSpellKnownTable = BloodragerSpellbook.SpellsKnown;
                    var BloodragerSpellPerDayTable = BloodragerSpellbook.SpellsPerDay;
                    BloodragerSpellbook.CasterLevelModifier = 0;
                    BloodragerSpellKnownTable.Levels = new SpellsLevelEntry[] {
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0,2),
                        SpellTools.CreateSpellLevelEntry(0,3),
                        SpellTools.CreateSpellLevelEntry(0,4),
                        SpellTools.CreateSpellLevelEntry(0,4,2),
                        SpellTools.CreateSpellLevelEntry(0,4,3),
                        SpellTools.CreateSpellLevelEntry(0,5,4),
                        SpellTools.CreateSpellLevelEntry(0,5,4,2),
                        SpellTools.CreateSpellLevelEntry(0,5,4,3),
                        SpellTools.CreateSpellLevelEntry(0,6,5,4),
                        SpellTools.CreateSpellLevelEntry(0,6,5,4,2),
                        SpellTools.CreateSpellLevelEntry(0,6,5,4,3),
                        SpellTools.CreateSpellLevelEntry(0,6,6,5,4),
                        SpellTools.CreateSpellLevelEntry(0,6,6,5,4),
                        SpellTools.CreateSpellLevelEntry(0,6,6,5,4),
                        SpellTools.CreateSpellLevelEntry(0,6,6,6,5),
                        SpellTools.CreateSpellLevelEntry(0,6,6,6,5),
                        SpellTools.CreateSpellLevelEntry(0,6,6,6,5)
                    };
                    BloodragerSpellPerDayTable.Levels = new SpellsLevelEntry[] {
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0),
                        SpellTools.CreateSpellLevelEntry(0,1),
                        SpellTools.CreateSpellLevelEntry(0,1),
                        SpellTools.CreateSpellLevelEntry(0,1),
                        SpellTools.CreateSpellLevelEntry(0,1,1),
                        SpellTools.CreateSpellLevelEntry(0,1,1),
                        SpellTools.CreateSpellLevelEntry(0,2,1),
                        SpellTools.CreateSpellLevelEntry(0,2,1,1),
                        SpellTools.CreateSpellLevelEntry(0,2,1,1),
                        SpellTools.CreateSpellLevelEntry(0,2,2,1),
                        SpellTools.CreateSpellLevelEntry(0,3,2,1,1),
                        SpellTools.CreateSpellLevelEntry(0,3,2,1,1),
                        SpellTools.CreateSpellLevelEntry(0,3,2,2,1),
                        SpellTools.CreateSpellLevelEntry(0,3,3,2,1),
                        SpellTools.CreateSpellLevelEntry(0,4,3,2,1),
                        SpellTools.CreateSpellLevelEntry(0,4,3,2,2),
                        SpellTools.CreateSpellLevelEntry(0,4,3,3,2),
                        SpellTools.CreateSpellLevelEntry(0,4,4,3,2)
                    };
                    TTTContext.Logger.LogPatch("Patched", BloodragerSpellPerDayTable);
                }
                void PatchTempHP() {
                    if (TTTContext.Fixes.Bloodrager.Base.IsDisabled("TemporaryHitPoints")) { return; }
                    var BloodragerStandartRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
                    var BloodragerRageResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("4aec9ec9d9cd5e24a95da90e56c72e37");

                    var tempHP = BloodragerStandartRageBuff.GetComponent<TemporaryHitPointsPerLevel>();
                    tempHP.m_LimitlessRageResource = BloodragerRageResource.ToReference<BlueprintAbilityResourceReference>();

                    TTTContext.Logger.LogPatch("Patched", BloodragerStandartRageBuff);
                }
            }
            static void PatchPrimalist() {
                PatchRagePowerFeatQualifications();
                PatchPrimalistRageBuffs();

                void PatchRagePowerFeatQualifications() {
                    if (TTTContext.Fixes.Bloodrager.Archetypes["Primalist"].IsDisabled("RagePowerFeatQualifications")) { return; }

                    var PrimalistTakeRagePowers4 = BlueprintTools.GetBlueprint<BlueprintProgression>("8eb5c34bb8471a0438e7eb3994de3b92");
                    var PrimalistTakeRagePowers8 = BlueprintTools.GetBlueprint<BlueprintProgression>("db2710cd915bbcf4193fa54083e56b27");
                    var PrimalistTakeRagePowers12 = BlueprintTools.GetBlueprint<BlueprintProgression>("e43a7bfd5c90a514cab1c11b41c550b1");
                    var PrimalistTakeRagePowers16 = BlueprintTools.GetBlueprint<BlueprintProgression>("b6412ff44f3a82f499d0dd6748a123bc");
                    var PrimalistTakeRagePowers20 = BlueprintTools.GetBlueprint<BlueprintProgression>("5905a80d5934248439e83612d9101b4b");

                    var PrimalistSecondBloodlineTakeRagePowers4 = BlueprintTools.GetBlueprint<BlueprintProgression>("39fd1f34c8ab45d997192773281291e3");
                    var PrimalistSecondBloodlineTakeRagePowers8 = BlueprintTools.GetBlueprint<BlueprintProgression>("cc0ed1ccd6664dd881922321fec7a8ba");
                    var PrimalistSecondBloodlineTakeRagePowers12 = BlueprintTools.GetBlueprint<BlueprintProgression>("c7254db3798e4e68baa85a084ca3d73b");
                    var PrimalistSecondBloodlineTakeRagePowers16 = BlueprintTools.GetBlueprint<BlueprintProgression>("531d1dd9dd7b4cdf9849ae027b2caf6d");
                    var PrimalistSecondBloodlineTakeRagePowers20 = BlueprintTools.GetBlueprint<BlueprintProgression>("32525ed3aea0480ebd7ad3654dca96cc");

                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers4, 4);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers8, 8);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers12, 12);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers16, 16);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers20, 20);

                    PatchPrimalistTakeRagePowers(PrimalistSecondBloodlineTakeRagePowers4, 4);
                    PatchPrimalistTakeRagePowers(PrimalistSecondBloodlineTakeRagePowers8, 8);
                    PatchPrimalistTakeRagePowers(PrimalistSecondBloodlineTakeRagePowers12, 12);
                    PatchPrimalistTakeRagePowers(PrimalistSecondBloodlineTakeRagePowers16, 16);
                    PatchPrimalistTakeRagePowers(PrimalistSecondBloodlineTakeRagePowers20, 20);

                    void PatchPrimalistTakeRagePowers(BlueprintProgression PrimalistTakeRagePowers, int level) {
                        var PrimalistRagePowerSelection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "PrimalistRagePowerSelection");
                        PrimalistTakeRagePowers.LevelEntries = new LevelEntry[] {
                            new LevelEntry {
                                Level = level,
                                Features = {
                                    PrimalistRagePowerSelection,
                                    PrimalistRagePowerSelection
                                }
                            }
                        };
                        TTTContext.Logger.LogPatch("Patched", PrimalistTakeRagePowers);
                    }
                }
                static void PatchPrimalistRageBuffs() {
                    if (TTTContext.Fixes.Bloodrager.Archetypes["Primalist"].IsDisabled("FixBrokenRagePowers")) { return; }
                    var BloodragerStandartRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
                    PatchCelestialTotem();
                    PatchDaemonTotem();
                    PatchFiendTotem();
                    PatchPowerfulStance();
                    PatchScentRagePower();

                    void PatchCelestialTotem() {

                        var CelestialTotemLesserFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("aba61e0b0e66bf3439cc247ee89fddae");
                        var CelestialTotemLesserBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("fe27c0d9b9dc6a74aa88887b561ad5f3");

                        CelestialTotemLesserFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = CelestialTotemLesserBuff.ToReference<BlueprintBuffReference>();
                        });
                        TTTContext.Logger.LogPatch("Patched", CelestialTotemLesserFeature);

                        var CelestialTotemFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("5156331dc888e9347ae6fc81ad3f3cec");
                        var CelestialTotemAreaBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("7bf740b33eaa2534e91def3cef142e00");

                        CelestialTotemFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = CelestialTotemAreaBuff.ToReference<BlueprintBuffReference>();
                        });
                        TTTContext.Logger.LogPatch("Patched", CelestialTotemFeature);

                        var CelestialTotemGreaterFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("774f79845d1683a43aa42ebd2a549497");
                        var CelestialTotemGreaterBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("e31276241f875254cb102329c0b55ba7");

                        CelestialTotemGreaterFeature.GetComponent<PrerequisiteArchetypeLevel>().Group = Prerequisite.GroupType.Any;

                        CelestialTotemGreaterFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = CelestialTotemGreaterBuff.ToReference<BlueprintBuffReference>();
                        });
                        TTTContext.Logger.LogPatch("Patched", CelestialTotemGreaterFeature);
                    }

                    void PatchDaemonTotem() {
                        var DaemonTotemLesserFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("45102fd7aab96f94d81ec80768549e12");
                        var DaemonTotemLesserBaseBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a8957a1c6f212244d969645bc2fa7c25");

                        DaemonTotemLesserFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = DaemonTotemLesserBaseBuff.ToReference<BlueprintBuffReference>();
                        });
                        TTTContext.Logger.LogPatch("Patched", DaemonTotemLesserFeature);

                        var DaemonTotemFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d673c30720e8e7c4bb0903dc3c9ab649");
                        var DaemonTotemBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("a4195deeb13eb9c4b93b6987839b60c7");

                        DaemonTotemFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = DaemonTotemBuff.ToReference<BlueprintBuffReference>();
                        });
                        TTTContext.Logger.LogPatch("Patched", DaemonTotemFeature);

                        var DaemonTotemGreaterFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9a2f0ffe517d221459640a4ad85710d7");
                        var DaemonTotemGreaterBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("2cf21dce5ecc791449f3106fcd0b60c3");

                        DaemonTotemGreaterFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = DaemonTotemGreaterBuff.ToReference<BlueprintBuffReference>();
                        });
                        TTTContext.Logger.LogPatch("Patched", DaemonTotemGreaterFeature);
                    }

                    void PatchFiendTotem() {
                        var PrimalistRagePowersBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("ecc22ca1eea1bf6488a0d7c6ee2527d8");

                        var FiendTotemLesserFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("76437492f801f054ba536473ad2fde79");
                        var FiendTotemLesserRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("d0649010d93907745a44034ad6eeeb5e");
                        PrimalistRagePowersBuff.AddConditionalBuff(FiendTotemLesserFeature, FiendTotemLesserRageBuff);
                        TTTContext.Logger.LogPatch("Patched", FiendTotemLesserFeature);

                        var FiendTotemFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("ce449404eeb4a7c499fbe0248056174f");
                        var FiendTotemRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4f524a75bb13f7c40806b0c19dc06fe4");
                        PrimalistRagePowersBuff.AddConditionalBuff(FiendTotemFeature, FiendTotemRageBuff);
                        TTTContext.Logger.LogPatch("Patched", FiendTotemFeature);

                        var FiendTotemGreaterFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1105632657d94d940a43707a3a57b006");
                        var FiendTotemGreaterRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c84ca8f21f63c8249a192f34195f8787");
                        PrimalistRagePowersBuff.AddConditionalBuff(FiendTotemGreaterFeature, FiendTotemGreaterRageBuff);
                        TTTContext.Logger.LogPatch("Patched", FiendTotemGreaterFeature);
                    }

                    void PatchPowerfulStance() {
                        var PowerfulStanceSwitchBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("539e480bcfe6d6f48bdd90418240b50f");
                        var PowerfulStanceEffectBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("aabad91034e5c7943986fe3e83bfc78e");

                        PowerfulStanceSwitchBuff.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = PowerfulStanceEffectBuff.ToReference<BlueprintBuffReference>();
                        });
                        TTTContext.Logger.LogPatch("Patched", PowerfulStanceSwitchBuff);
                    }

                    void PatchScentRagePower() {
                        var PrimalistRagePowersBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("ecc22ca1eea1bf6488a0d7c6ee2527d8");

                        var ScentFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6e5d57a733d1eea46a9022a304f2c728");
                        var ScentRageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("879e6a7ed8101404d8e4f1bc25c0d34f");
                        PrimalistRagePowersBuff.AddConditionalBuff(ScentFeature, ScentRageBuff);
                        TTTContext.Logger.LogPatch("Patched", ScentFeature);
                    }
                }
            }
            static void PatchReformedFiend() {
                PatchHatredAgainstEvil();
                PatchDamageReduction();

                void PatchHatredAgainstEvil() {
                    if (TTTContext.Fixes.Bloodrager.Archetypes["ReformedFiend"].IsDisabled("HatredAgainstEvil")) { return; }
                    var BloodragerClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
                    var ReformedFiendBloodrageBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("72a679f712bd4f69a07bf03d5800900b");
                    var rankConfig = ReformedFiendBloodrageBuff.GetComponent<ContextRankConfig>();

                    rankConfig.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    rankConfig.m_Class = new BlueprintCharacterClassReference[] { BloodragerClass.ToReference<BlueprintCharacterClassReference>() };
                    rankConfig.m_UseMin = true;
                }
                void PatchDamageReduction() {
                    if (TTTContext.Fixes.Bloodrager.Archetypes["ReformedFiend"].IsDisabled("DamageReduction")) { return; }
                    var ReformedFiendDamageReductionFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("2a3243ad1ccf43d5a5d69de3f9d0420e");
                    ReformedFiendDamageReductionFeature.GetComponent<AddDamageResistancePhysical>().BypassedByAlignment = true;
                }
            }
            static void PatchArcaneBloodrage() {
                if (TTTContext.Fixes.Bloodrager.Base.IsDisabled("ArcaneBloodrage")) { return; }
                var BloodragerArcaneSpellAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("3151dfeeb202e38448d1fea1e8bc237e");
                BloodragerArcaneSpellAbility.GetComponent<AbilityVariants>().m_Variants = new BlueprintAbilityReference[] {
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellBlurToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellProtectionFromArrowsToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellResistFireToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellResistColdToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellResistElectricityToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellResistAcidToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellResistSonicToggle").ToReference<BlueprintAbilityReference>()
                };
                BloodragerArcaneSpellAbility.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.VariantsBase;
                });

                TTTContext.Logger.LogPatch("Patched", BloodragerArcaneSpellAbility);
            }
            static void PatchGreaterArcaneBloodrage() {
                if (TTTContext.Fixes.Bloodrager.Base.IsDisabled("ArcaneBloodrage")) { return; }
                var BloodragerArcaneGreaterSpell = BlueprintTools.GetBlueprint<BlueprintAbility>("31dbadf586920494b87e8e95452af998");
                BloodragerArcaneGreaterSpell.GetComponent<AbilityVariants>().m_Variants = new BlueprintAbilityReference[] {
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellGreaterDisplacementToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellGreaterHasteToggle").ToReference<BlueprintAbilityReference>()
                };
                BloodragerArcaneGreaterSpell.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.VariantsBase;
                });

                TTTContext.Logger.LogPatch("Patched", BloodragerArcaneGreaterSpell);
            }
            static void PatchTrueArcaneBloodrage() {
                if (TTTContext.Fixes.Bloodrager.Base.IsDisabled("ArcaneBloodrage")) { return; }
                var BloodragerArcaneTrueSpellAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("9d4d7f56d2d87f643b5ef990ef481094");
                BloodragerArcaneTrueSpellAbility.GetComponent<AbilityVariants>().m_Variants = new BlueprintAbilityReference[] {
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueBeastShapeIVShamblingMoundToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueBeastShapeIVSmilodonToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueBeastShapeIVWyvernToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueFormOfTheDragonIBlackToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueFormOfTheDragonIBlueToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueFormOfTheDragonIBrassToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueFormOfTheDragonIBronzeToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueFormOfTheDragonICopperToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueFormOfTheDragonIGoldToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueFormOfTheDragonIGreenToggle").ToReference<BlueprintAbilityReference>(),
                    BlueprintTools.GetModBlueprint<BlueprintAbility>(TTTContext, "BloodragerArcaneSpellTrueTransformationToggle").ToReference<BlueprintAbilityReference>(),
                };
                BloodragerArcaneTrueSpellAbility.AddComponent<PseudoActivatable>(c => {
                    c.m_Type = PseudoActivatable.PseudoActivatableType.VariantsBase;
                });

                TTTContext.Logger.LogPatch("Patched", BloodragerArcaneTrueSpellAbility);
            }
            static void PatchDisruptiveBloodrage() {
                if (TTTContext.Fixes.Bloodrager.Base.IsDisabled("DisruptiveBloodrage")) { return; }
                var BloodragerArcaneBloodlineBaseBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b500dec368206eb41808fb73238b8769");
                var BloodragerArcaneDisruptive = BlueprintTools.GetBlueprint<BlueprintFeature>("d8cb753a508c6fd4090a9c67ad0c1e58");
                var BloodragerArcaneDisruptiveBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("0e8083dbeec00a84dafa6e5be0408d1f");

                BloodragerArcaneBloodlineBaseBuff.AddConditionalBuff(BloodragerArcaneDisruptive, BloodragerArcaneDisruptiveBuff);
                TTTContext.Logger.LogPatch("Patched", BloodragerArcaneDisruptive);
            }
            static void PatchCastersBane() {
                if (TTTContext.Fixes.Bloodrager.Base.IsDisabled("CastersBane")) { return; }
                var BloodragerArcaneBloodlineBaseBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b500dec368206eb41808fb73238b8769");
                var BloodragerArcaneCastersBane = BlueprintTools.GetBlueprint<BlueprintFeature>("7327c5f0d02a4eb99949da59603263a3");
                var BloodragerArcaneCastersBaneBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("b0dd4cb1813340c3b724062b0f6842b0");

                BloodragerArcaneBloodlineBaseBuff.AddConditionalBuff(BloodragerArcaneCastersBane, BloodragerArcaneCastersBaneBuff);
                TTTContext.Logger.LogPatch("Patched", BloodragerArcaneCastersBane);
            }
        }
    }
}
