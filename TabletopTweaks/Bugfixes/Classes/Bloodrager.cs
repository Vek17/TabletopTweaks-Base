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
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Bloodrager {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Bloodrager");

                PatchBaseClass();
                PatchPrimalist();
                PatchReformedFiend();
            }
            static void PatchBaseClass() {
                PatchSpellbook();
                PatchAbysalBulk();
                PatchLimitlessRage();
                PatchArcaneBloodrage();

                void PatchArcaneBloodrage() {
                    if(ModSettings.Fixes.Bloodrager.Base.IsDisabled("ArcaneBloodrage")) { return; }

                    var BloodragerArcaneSpellBlur = Resources.GetBlueprint<BlueprintAbility>("1cca16d1f03462b4697b39937a5aa726");
                    BloodragerArcaneSpellBlur.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneSpellProtectionFromArrows = Resources.GetBlueprint<BlueprintAbility>("033428f0ab03df047ac3920e1c5f6152");
                    BloodragerArcaneSpellProtectionFromArrows.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneSpellResistAcid = Resources.GetBlueprint<BlueprintAbility>("f3968083c2b20014ab58fb2232594f9e");
                    BloodragerArcaneSpellResistAcid.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneSpellResistCold = Resources.GetBlueprint<BlueprintAbility>("6ba9f942e25d9fa41a54980b36b40698");
                    BloodragerArcaneSpellResistCold.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneSpellResistElectricity = Resources.GetBlueprint<BlueprintAbility>("c627dc55d27b77044885634c07d8ab0d");
                    BloodragerArcaneSpellResistElectricity.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneSpellResistFire = Resources.GetBlueprint<BlueprintAbility>("ebac7215bdf025443b85643a3096e221");
                    BloodragerArcaneSpellResistFire.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneSpellResistSonic = Resources.GetBlueprint<BlueprintAbility>("eb0bfb36fb5d5454e8cae0d62985cad4");
                    BloodragerArcaneSpellResistSonic.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneGreaterSpellHaste = Resources.GetBlueprint<BlueprintAbility>("54209a58537e1a34e99c2e28a0341f25");
                    BloodragerArcaneGreaterSpellHaste.Range = AbilityRange.Personal;
                    BloodragerArcaneGreaterSpellHaste.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneGreaterSpellDisplacement = Resources.GetBlueprint<BlueprintAbility>("65eb78967e14aaf48a4b8baa5f40dc11");
                    BloodragerArcaneGreaterSpellDisplacement.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellBeastShapeIVShamblingMound = Resources.GetBlueprint<BlueprintAbility>("67fc979bc5c426e41bc88c1c0df964a7");
                    BloodragerArcaneTrueSpellBeastShapeIVShamblingMound.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellBeastShapeIVSmilodon = Resources.GetBlueprint<BlueprintAbility>("a0644a79ff01dda4190be4be0ac5d1f4");
                    BloodragerArcaneTrueSpellBeastShapeIVSmilodon.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellBeastShapeIVWyvern = Resources.GetBlueprint<BlueprintAbility>("bfd0f517657ba8a46992ca532b441b5d");
                    BloodragerArcaneTrueSpellBeastShapeIVWyvern.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellFormOfTheDragonIBlack = Resources.GetBlueprint<BlueprintAbility>("d259038503ee8c94ebc2f0d6f2ff69e1");
                    BloodragerArcaneTrueSpellFormOfTheDragonIBlack.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellFormOfTheDragonIBlue = Resources.GetBlueprint<BlueprintAbility>("d5322b7198a7b8d408252001f4f07c0b");
                    BloodragerArcaneTrueSpellFormOfTheDragonIBlue.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellFormOfTheDragonIBrass = Resources.GetBlueprint<BlueprintAbility>("2577b5e0ccee59e44abbbcda8bcbaea6");
                    BloodragerArcaneTrueSpellFormOfTheDragonIBrass.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellFormOfTheDragonIBronze = Resources.GetBlueprint<BlueprintAbility>("05266e35a7aaf5046aaf41d1fc151ede");
                    BloodragerArcaneTrueSpellFormOfTheDragonIBronze.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellFormOfTheDragonICopper = Resources.GetBlueprint<BlueprintAbility>("2014f063206d6cc429be52ccf3bd65d5");
                    BloodragerArcaneTrueSpellFormOfTheDragonICopper.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellFormOfTheDragonIGold = Resources.GetBlueprint<BlueprintAbility>("bb864728d4e63e14b9bb02cbf9e46280");
                    BloodragerArcaneTrueSpellFormOfTheDragonIGold.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellFormOfTheDragonIGreen = Resources.GetBlueprint<BlueprintAbility>("ccc14864e9ca6ed4da878c1c34b832d9");
                    BloodragerArcaneTrueSpellFormOfTheDragonIGreen.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;

                    var BloodragerArcaneTrueSpellTransformation = Resources.GetBlueprint<BlueprintAbility>("dc8f5f7658743ff4ba99e1d2fba6cf9c");
                    BloodragerArcaneTrueSpellTransformation.GetComponent<ContextCalculateAbilityParams>().ReplaceCasterLevel = false;
                }

                void PatchAbysalBulk() {
                    if (ModSettings.Fixes.Bloodrager.Base.IsDisabled("AbysalBulk")) { return; }
                    var BloodragerAbyssalBloodlineBaseBuff = Resources.GetBlueprint<BlueprintBuff>("2ba7b4b3b87156543b43d0686404655a");
                    var BloodragerAbyssalDemonicBulkBuff = Resources.GetBlueprint<BlueprintBuff>("031a8053a7c02ab42ad53f50dd2e9437");
                    var BloodragerAbyssalDemonicBulkEnlargeBuff = Resources.GetModBlueprint<BlueprintBuff>("BloodragerAbyssalDemonicBulkEnlargeBuff");

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
                }
                void PatchSpellbook() {
                    if (ModSettings.Fixes.Bloodrager.Base.IsDisabled("Spellbook")) { return; }
                    BlueprintSpellbook BloodragerSpellbook = Resources.GetBlueprint<BlueprintSpellbook>("e19484252c2f80e4a9439b3681b20f00");
                    var BloodragerSpellKnownTable = BloodragerSpellbook.SpellsKnown;
                    var BloodragerSpellPerDayTable = BloodragerSpellbook.SpellsPerDay;
                    BloodragerSpellbook.CasterLevelModifier = 0;
                    BloodragerSpellKnownTable.Levels = new SpellsLevelEntry[] {
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0,2),
                        CreateSpellLevelEntry(0,3),
                        CreateSpellLevelEntry(0,4),
                        CreateSpellLevelEntry(0,4,2),
                        CreateSpellLevelEntry(0,4,3),
                        CreateSpellLevelEntry(0,5,4),
                        CreateSpellLevelEntry(0,5,4,2),
                        CreateSpellLevelEntry(0,5,4,3),
                        CreateSpellLevelEntry(0,6,5,4),
                        CreateSpellLevelEntry(0,6,5,4,2),
                        CreateSpellLevelEntry(0,6,5,4,3),
                        CreateSpellLevelEntry(0,6,6,5,4),
                        CreateSpellLevelEntry(0,6,6,5,4),
                        CreateSpellLevelEntry(0,6,6,5,4),
                        CreateSpellLevelEntry(0,6,6,6,5),
                        CreateSpellLevelEntry(0,6,6,6,5),
                        CreateSpellLevelEntry(0,6,6,6,5)
                    };
                    BloodragerSpellPerDayTable.Levels = new SpellsLevelEntry[] {
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0),
                        CreateSpellLevelEntry(0,1),
                        CreateSpellLevelEntry(0,1),
                        CreateSpellLevelEntry(0,1),
                        CreateSpellLevelEntry(0,1,1),
                        CreateSpellLevelEntry(0,1,1),
                        CreateSpellLevelEntry(0,2,1),
                        CreateSpellLevelEntry(0,2,1,1),
                        CreateSpellLevelEntry(0,2,1,1),
                        CreateSpellLevelEntry(0,2,2,1),
                        CreateSpellLevelEntry(0,3,2,1,1),
                        CreateSpellLevelEntry(0,3,2,1,1),
                        CreateSpellLevelEntry(0,3,2,2,1),
                        CreateSpellLevelEntry(0,3,3,2,1),
                        CreateSpellLevelEntry(0,4,3,2,1),
                        CreateSpellLevelEntry(0,4,3,2,2),
                        CreateSpellLevelEntry(0,4,3,3,2),
                        CreateSpellLevelEntry(0,4,4,3,2)
                    };
                    Main.LogPatch("Patched", BloodragerSpellPerDayTable);
                    SpellsLevelEntry CreateSpellLevelEntry(params int[] count) {
                        var entry = new SpellsLevelEntry();
                        entry.Count = count;
                        return entry;
                    }
                }
            }
            static void PatchPrimalist() {
                PatchRagePowerFeatQualifications();
                PatchPrimalistRageBuffs();

                void PatchRagePowerFeatQualifications() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["Primalist"].IsDisabled("RagePowerFeatQualifications")) { return; }
                    var PrimalistTakeRagePowers4 = Resources.GetBlueprint<BlueprintProgression>("8eb5c34bb8471a0438e7eb3994de3b92");
                    var PrimalistTakeRagePowers8 = Resources.GetBlueprint<BlueprintProgression>("db2710cd915bbcf4193fa54083e56b27");
                    var PrimalistTakeRagePowers12 = Resources.GetBlueprint<BlueprintProgression>("e43a7bfd5c90a514cab1c11b41c550b1");
                    var PrimalistTakeRagePowers16 = Resources.GetBlueprint<BlueprintProgression>("b6412ff44f3a82f499d0dd6748a123bc");
                    var PrimalistTakeRagePowers20 = Resources.GetBlueprint<BlueprintProgression>("5905a80d5934248439e83612d9101b4b");

                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers4, 4);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers8, 8);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers12, 12);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers16, 16);
                    PatchPrimalistTakeRagePowers(PrimalistTakeRagePowers20, 20);

                    void PatchPrimalistTakeRagePowers(BlueprintProgression PrimalistTakeRagePowers, int level) {
                        var PrimalistRagePowerSelection = Resources.GetModBlueprint<BlueprintFeatureSelection>("PrimalistRagePowerSelection");
                        PrimalistTakeRagePowers.LevelEntries = new LevelEntry[] {
                            new LevelEntry {
                                Level = level,
                                Features = {
                                    PrimalistRagePowerSelection,
                                    PrimalistRagePowerSelection
                                }
                            }
                        };
                        Main.LogPatch("Patched", PrimalistTakeRagePowers);
                    }
                }
                static void PatchPrimalistRageBuffs() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["Primalist"].IsDisabled("FixBrokenRagePowers")) { return; }
                    var BloodragerStandartRageBuff = Resources.GetBlueprint<BlueprintBuff>("5eac31e457999334b98f98b60fc73b2f");
                    PatchCelestialTotem();
                    PatchDaemonTotem();
                    PatchFiendTotem();
                    PatchPowerfulStance();
                    PatchScentRagePower();

                    void PatchCelestialTotem() {

                        var CelestialTotemLesserFeature = Resources.GetBlueprint<BlueprintFeature>("aba61e0b0e66bf3439cc247ee89fddae");
                        var CelestialTotemLesserBuff = Resources.GetBlueprint<BlueprintBuff>("fe27c0d9b9dc6a74aa88887b561ad5f3");

                        CelestialTotemLesserFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = CelestialTotemLesserBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", CelestialTotemLesserFeature);

                        var CelestialTotemFeature = Resources.GetBlueprint<BlueprintFeature>("5156331dc888e9347ae6fc81ad3f3cec");
                        var CelestialTotemAreaBuff = Resources.GetBlueprint<BlueprintBuff>("7bf740b33eaa2534e91def3cef142e00");

                        CelestialTotemFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = CelestialTotemAreaBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", CelestialTotemFeature);

                        var CelestialTotemGreaterFeature = Resources.GetBlueprint<BlueprintFeature>("774f79845d1683a43aa42ebd2a549497");
                        var CelestialTotemGreaterBuff = Resources.GetBlueprint<BlueprintBuff>("e31276241f875254cb102329c0b55ba7");

                        CelestialTotemGreaterFeature.GetComponent<PrerequisiteArchetypeLevel>().Group = Prerequisite.GroupType.Any;

                        CelestialTotemGreaterFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = CelestialTotemGreaterBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", CelestialTotemGreaterFeature);
                    }

                    void PatchDaemonTotem() {
                        var DaemonTotemLesserFeature = Resources.GetBlueprint<BlueprintFeature>("45102fd7aab96f94d81ec80768549e12");
                        var DaemonTotemLesserBaseBuff = Resources.GetBlueprint<BlueprintBuff>("a8957a1c6f212244d969645bc2fa7c25");

                        DaemonTotemLesserFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = DaemonTotemLesserBaseBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", DaemonTotemLesserFeature);

                        var DaemonTotemFeature = Resources.GetBlueprint<BlueprintFeature>("d673c30720e8e7c4bb0903dc3c9ab649");
                        var DaemonTotemBuff = Resources.GetBlueprint<BlueprintBuff>("a4195deeb13eb9c4b93b6987839b60c7");

                        DaemonTotemFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = DaemonTotemBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", DaemonTotemFeature);

                        var DaemonTotemGreaterFeature = Resources.GetBlueprint<BlueprintFeature>("9a2f0ffe517d221459640a4ad85710d7");
                        var DaemonTotemGreaterBuff = Resources.GetBlueprint<BlueprintBuff>("2cf21dce5ecc791449f3106fcd0b60c3");

                        DaemonTotemGreaterFeature.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = DaemonTotemGreaterBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", DaemonTotemGreaterFeature);
                    }

                    void PatchFiendTotem() {
                        var PrimalistRagePowersBuff = Resources.GetBlueprint<BlueprintBuff>("ecc22ca1eea1bf6488a0d7c6ee2527d8");

                        var FiendTotemLesserFeature = Resources.GetBlueprint<BlueprintFeature>("76437492f801f054ba536473ad2fde79");
                        var FiendTotemLesserRageBuff = Resources.GetBlueprint<BlueprintBuff>("d0649010d93907745a44034ad6eeeb5e");
                        PrimalistRagePowersBuff.AddConditionalBuff(FiendTotemLesserFeature, FiendTotemLesserRageBuff);
                        Main.LogPatch("Patched", FiendTotemLesserFeature);

                        var FiendTotemFeature = Resources.GetBlueprint<BlueprintFeature>("ce449404eeb4a7c499fbe0248056174f");
                        var FiendTotemRageBuff = Resources.GetBlueprint<BlueprintBuff>("4f524a75bb13f7c40806b0c19dc06fe4");
                        PrimalistRagePowersBuff.AddConditionalBuff(FiendTotemFeature, FiendTotemRageBuff);
                        Main.LogPatch("Patched", FiendTotemFeature);

                        var FiendTotemGreaterFeature = Resources.GetBlueprint<BlueprintFeature>("1105632657d94d940a43707a3a57b006");
                        var FiendTotemGreaterRageBuff = Resources.GetBlueprint<BlueprintBuff>("c84ca8f21f63c8249a192f34195f8787");
                        PrimalistRagePowersBuff.AddConditionalBuff(FiendTotemGreaterFeature, FiendTotemGreaterRageBuff);
                        Main.LogPatch("Patched", FiendTotemGreaterFeature);
                    }

                    void PatchPowerfulStance() {
                        var PowerfulStanceSwitchBuff = Resources.GetBlueprint<BlueprintBuff>("539e480bcfe6d6f48bdd90418240b50f");
                        var PowerfulStanceEffectBuff = Resources.GetBlueprint<BlueprintBuff>("aabad91034e5c7943986fe3e83bfc78e");

                        PowerfulStanceSwitchBuff.AddComponent<BuffExtraEffects>(c => {
                            c.m_CheckedBuff = BloodragerStandartRageBuff.ToReference<BlueprintBuffReference>();
                            c.m_ExtraEffectBuff = PowerfulStanceEffectBuff.ToReference<BlueprintBuffReference>();
                        });
                        Main.LogPatch("Patched", PowerfulStanceSwitchBuff);
                    }

                    void PatchScentRagePower() {
                        var PrimalistRagePowersBuff = Resources.GetBlueprint<BlueprintBuff>("ecc22ca1eea1bf6488a0d7c6ee2527d8");

                        var ScentFeature = Resources.GetBlueprint<BlueprintFeature>("6e5d57a733d1eea46a9022a304f2c728");
                        var ScentRageBuff = Resources.GetBlueprint<BlueprintBuff>("879e6a7ed8101404d8e4f1bc25c0d34f");
                        PrimalistRagePowersBuff.AddConditionalBuff(ScentFeature, ScentRageBuff);
                        Main.LogPatch("Patched", ScentFeature);
                    }
                }
            }
            static void PatchReformedFiend() {
                PatchHatredAgainstEvil();
                PatchDamageReduction();

                void PatchHatredAgainstEvil() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["ReformedFiend"].IsDisabled("HatredAgainstEvil")) { return; }
                    var BloodragerClass = Resources.GetBlueprint<BlueprintCharacterClass>("d77e67a814d686842802c9cfd8ef8499");
                    var ReformedFiendBloodrageBuff = Resources.GetBlueprint<BlueprintBuff>("72a679f712bd4f69a07bf03d5800900b");
                    var rankConfig = ReformedFiendBloodrageBuff.GetComponent<ContextRankConfig>();

                    rankConfig.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    rankConfig.m_Class = new BlueprintCharacterClassReference[] { BloodragerClass.ToReference<BlueprintCharacterClassReference>() };
                    rankConfig.m_UseMin = true;
                }
                void PatchDamageReduction() {
                    if (ModSettings.Fixes.Bloodrager.Archetypes["ReformedFiend"].IsDisabled("DamageReduction")) { return; }
                    var ReformedFiendDamageReductionFeature = Resources.GetBlueprint<BlueprintFeature>("2a3243ad1ccf43d5a5d69de3f9d0420e");
                    ReformedFiendDamageReductionFeature.GetComponent<AddDamageResistancePhysical>().BypassedByAlignment = true;
                }
            }

            
        }
    }
}
