using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Alchemist {
        [PatchBlueprintsCacheInit]
        static class Alchemist_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Alchemist")) { return; }

                var DiscoverySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("cd86c437488386f438dcc9ae727ea2a6");
                var VivsectionistDiscoverySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("67f499218a0e22944abab6fe1c9eaeee");
                var VivisectionistArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("68cbcd9fbf1fb1d489562f829bb97e38");
                var GrandDiscoverySelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("2729af328ab46274394cedc3582d6e98");
                var AlchemistAlternateCapstone = NewContent.AlternateCapstones.Alchemist.AlchemistAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();
                var GrandDiscoveryProgression = NewContent.AlternateCapstones.Alchemist.GrandDiscoveryProgression.ToReference<BlueprintUnitFactReference>();

                DiscoverySelection.TemporaryContext(bp => {
                    bp.HideNotAvailibleInUI = true;
                    bp.AddPrerequisite<PrerequisiteNoArchetype>(c => {
                        c.m_Archetype = VivisectionistArchetype.ToReference<BlueprintArchetypeReference>();
                        c.m_CharacterClass = ClassTools.Classes.AlchemistClass.ToReference<BlueprintCharacterClassReference>();
                        c.HideInUI = true;
                        c.CheckInProgression = true;
                    });
                });
                VivsectionistDiscoverySelection.TemporaryContext(bp => {
                    bp.HideNotAvailibleInUI = true;
                    bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                        c.m_Archetype = VivisectionistArchetype.ToReference<BlueprintArchetypeReference>();
                        c.m_CharacterClass = ClassTools.Classes.AlchemistClass.ToReference<BlueprintCharacterClassReference>();
                        c.HideInUI = true;
                        c.CheckInProgression = true;
                    });
                });

                ClassTools.Classes.AlchemistClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups = bp.Progression.UIGroups.AppendToArray(Helpers.CreateUIGroup(DiscoverySelection, VivsectionistDiscoverySelection, AlchemistAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => {
                            entry.m_Features.RemoveAll(f => f.deserializedGuid == DiscoverySelection.AssetGuid);
                            entry.m_Features.RemoveAll(f => f.deserializedGuid == GrandDiscoverySelection.deserializedGuid);
                            entry.m_Features.Add(AlchemistAlternateCapstone);
                        });
                    bp.Archetypes
                        .ForEach(a => {
                            a.RemoveFeatures
                                .Where(remove => remove.Level == 20)
                                .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == GrandDiscoverySelection.deserializedGuid))
                                .ForEach(remove => {
                                    remove.m_Features.RemoveAll(f => f.deserializedGuid == DiscoverySelection.AssetGuid);
                                    remove.m_Features.RemoveAll(f => f.deserializedGuid == GrandDiscoverySelection.deserializedGuid);
                                    remove.m_Features.Add(AlchemistAlternateCapstone);
                                });
                            a.RemoveFeatures
                                .Where(remove => remove.Level == 20)
                                .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == DiscoverySelection.AssetGuid))
                                .ForEach(remove => {
                                    remove.m_Features.RemoveAll(f => f.deserializedGuid == DiscoverySelection.AssetGuid);
                                });
                        });
                    VivisectionistArchetype.TemporaryContext(bp => {
                        bp.AddFeatures
                            .Where(add => add.Level == 20)
                            .ForEach(add => {
                                add.m_Features.RemoveAll(f => f.deserializedGuid == VivsectionistDiscoverySelection.AssetGuid);
                            });
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
                TTTContext.Logger.LogHeader("Patching Alchemist Resources");

                PatchBase();
                PatchGrenadier();
                PatchIncenseSynthesizer();
                PatchVivisectionist();
            }
            static void PatchBase() {
                PatchDispelingBombs();
                PatchMutagens();

                void PatchDispelingBombs() {
                    if (TTTContext.Fixes.Alchemist.Base.IsDisabled("DispelingBombs")) { return; }

                    var DispelingBomb = BlueprintTools.GetBlueprint<BlueprintAbility>("f80896af0e10d7c4f9454cf1ce50ada4");

                    DispelingBomb.TemporaryContext(bp => {
                        bp.FlattenAllActions()
                            .OfType<ContextActionDispelMagic>()
                            .ForEach(a => {
                                a.m_StopAfterCountRemoved = true;
                                a.m_CountToRemove = 1;
                                a.OneRollForAll = true;
                            });
                    });

                    TTTContext.Logger.LogPatch(DispelingBomb);
                }
                void PatchMutagens() {
                    if (TTTContext.Fixes.Alchemist.Base.IsDisabled("MutagenStacking")) { return; }

                    var mutagenBuffs = new BlueprintBuff[] {
                        BlueprintTools.GetBlueprint<BlueprintBuff>("32f2bc843effd9b45a0952a3cffbbe9f"),    // CognatogenCharismaBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("20e740104092b5e49bfb167f1670a9de"),    // CognatogenIntelligenceBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("6871149a90e278f479aa171ee8bb563e"),    // CognatogenWisdomBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("a5a6f915d13fd994fb109473032d7440"),    // GrandCognatogenCharismaIntelligenceBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("608dd115b3b0fba4ab511f448bc798f8"),    // GrandCognatogenCharismaWisdomBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("8de52f7aa6052a0498875e0d834330af"),    // GrandCognatogenIntelligenceCharismaBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("ac7753d72b0b7264982c2b6670fa2a2e"),    // GrandCognatogenIntelligenceWisdomBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("232fe914c22744c4ea3e050901bda424"),    // GrandCognatogenWisdomCharismaBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("98a46e8da1dca9f47b41b9d71d579628"),    // GrandCognatogenWisdomIntelligenceBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("0d51a2ff0a6ce85458309affbc00b933"),    // GrandMutagenConstitutionDexterityBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("9c3761b9f48f69849ad78873c5a12147"),    // GrandMutagenConstitutionStrengthBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("8d4357118c75a5746802a3582a937376"),    // GrandMutagenDexterityConstitutionBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("bf73a2b70b6fac54e891431cf6c7d8eb"),    // GrandMutagenDexterityStrengthBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("204a74affae72d54984fb533704caf72"),    // GrandMutagenStrengthConstitutionBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("3b7cf6307d3e61545a977c9f4156e12e"),    // GrandMutagenStrengthDexterityBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("b60f8b93d3d1d26439c1bb48fd461a3a"),    // GreaterCognatogenCharismaIntelligenceBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("61271a59038390c488c313f7a0aee6ea"),    // GreaterCognatogenCharismaWisdomBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("1c2fdba3b33dacd41afd5b74d84c7332"),    // GreaterCognatogenIntelligenceCharismaBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("34fde71198d30094aa133546e8cf8733"),    // GreaterCognatogenIntelligenceWisdomBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("60eb20b9d1077ed4f8f8a9df5490a208"),    // GreaterCognatogenWisdomCharismaBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("bc0890817bb28fe4a86094fe57cd40fb"),    // GreaterCognatogenWisdomIntelligenceBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("a8e7ca242395c3b49af5a3dbc9dee683"),    // GreaterMutagenConstitutionDexterityBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("84c42fea967a2a8499ceeaef3a6416b8"),    // GreaterMutagenConstitutionStrengthBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("84ae955af09809b4ea31a2c719c68377"),    // GreaterMutagenDexterityConstitutionBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("d0a5cedfd497f3b4f9581b6066d9043b"),    // GreaterMutagenDexterityStrengthBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("83ed8d5c1e4ed9045874494c0fe2b682"),    // GreaterMutagenStrengthConstitutionBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("a42c49fcb081bd1469679e4f515732c8"),    // GreaterMutagenStrengthDexterityBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("bd48322a4e258b8418106dcc6459e024"),    // MutagenConstitutionBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("f2be3d538b5d75c409289d35399723c4"),    // MutagenDexterityBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("b84abc3531ed5674284ef0ba4aafcd3b"),    // MutagenStrengthBuff,
                        BlueprintTools.GetBlueprint<BlueprintBuff>("3fb9e9a6408589343bc8bfc3fd1610e5"),    // TrueMutagenBuff,
                    };

                    foreach (var mutagen in mutagenBuffs) {
                        AddBuffRemoval(mutagen, mutagenBuffs);
                    }

                    void AddBuffRemoval(BlueprintBuff mutagen, BlueprintBuff[] mutagenBuffs) {
                        var addFactContextActions = mutagen.GetComponent<AddFactContextActions>();
                        foreach (var buff in mutagenBuffs.Where(b => b != mutagen)) {
                            var removeBuff = new ContextActionRemoveBuff() {
                                m_Buff = buff.ToReference<BlueprintBuffReference>(),
                            };
                            var conditional = new Conditional() {
                                ConditionsChecker = new ConditionsChecker() {
                                    Conditions = new Condition[] {
                                        new ContextConditionHasBuff() {
                                            m_Buff = buff.ToReference<BlueprintBuffReference>(),
                                        }
                                    }
                                },
                                IfTrue = Helpers.CreateActionList(removeBuff),
                                IfFalse = Helpers.CreateActionList()
                            };
                            addFactContextActions.Activated.Actions = addFactContextActions.Activated.Actions.AppendToArray(conditional);
                        }
                        TTTContext.Logger.LogPatch("Patched", mutagen);
                    }
                }
            }
            static void PatchGrenadier() {
                PatchBrewPotions();
                PatchPoisonResistance();

                void PatchBrewPotions() {
                    if (TTTContext.Fixes.Alchemist.Archetypes["Grenadier"].IsDisabled("BrewPotions")) { return; }

                    var GrenadierArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("6af888a7800b3e949a40f558ff204aae");
                    var BrewPotions = BlueprintTools.GetBlueprint<BlueprintFeature>("c0f8c4e513eb493408b8070a1de93fc0");

                    GrenadierArchetype.RemoveFeatures = GrenadierArchetype.RemoveFeatures.AppendToArray(new LevelEntry() {
                        Level = 1,
                        m_Features = new List<BlueprintFeatureBaseReference>() {
                            BrewPotions.ToReference<BlueprintFeatureBaseReference>()
                        }
                    }); ;
                    TTTContext.Logger.LogPatch("Patched", GrenadierArchetype);
                }

                void PatchPoisonResistance() {
                    if (TTTContext.Fixes.Alchemist.Archetypes["Grenadier"].IsDisabled("PoisonResistance")) { return; }

                    var GrenadierArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("6af888a7800b3e949a40f558ff204aae");
                    var PoisonResistance = BlueprintTools.GetBlueprint<BlueprintFeature>("c9022272c87bd66429176ce5c597989c");
                    var PoisonImmunity = BlueprintTools.GetBlueprint<BlueprintFeature>("202af59b918143a4ab7c33d72c8eb6d5");

                    GrenadierArchetype.RemoveFeatures = GrenadierArchetype.RemoveFeatures
                        .Where(entry => !new int[] { 2, 5, 8, 10 }.Contains(entry.Level))
                        .Concat(new LevelEntry[] {
                            Helpers.CreateLevelEntry(2, PoisonResistance),
                            Helpers.CreateLevelEntry(5, PoisonResistance),
                            Helpers.CreateLevelEntry(8, PoisonResistance),
                            Helpers.CreateLevelEntry(10, PoisonImmunity)
                        }).ToArray();
                    TTTContext.Logger.LogPatch("Patched", GrenadierArchetype);
                }
            }
            static void PatchIncenseSynthesizer() {
                PatchImprovedIncense();
                PatchThickFog();
                PatchSacredIncense();

                void PatchImprovedIncense() {
                    if (TTTContext.Fixes.Alchemist.Archetypes["IncenseSynthesizer"].IsDisabled("ImprovedIncense")) { return; }

                    var IncenseFogImprovedIncenseFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("ccea52d8cc5f8d34d95196d0a885be06");
                    var IncenseFogEffectBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("d2facca5b5801234b95f0cd75ebac3c1");
                    IncenseFogEffectBuff.GetComponent<ContextRankConfig>().TemporaryContext(c => {
                        c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                        c.m_Feature = IncenseFogImprovedIncenseFeature;
                        c.m_Progression = ContextRankProgression.OnePlusDivStep;
                        c.m_StepLevel = 1;
                    });

                    TTTContext.Logger.LogPatch("Patched", IncenseFogEffectBuff);
                }
                void PatchSacredIncense() {
                    if (TTTContext.Fixes.Alchemist.Archetypes["IncenseSynthesizer"].IsDisabled("SacredIncense")) { return; }

                    var IncenseFogArea = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("4aeb5ae7923dac74d91069f13a7f0a95");
                    var IncenseFog30Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("ffbbe6a2390245649ae5c7f2854d9cc2");
                    var IncenseFogSickenedBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("e905c55936cb67a48b8adf36e0d71de9");
                    var IncenseFogNauseatedBuff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("580df469d7a9460429265fafb231fc9a");
                    var Sickened = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("4e42460798665fd4cb9173ffa7ada323");
                    var Nauseated = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("956331dba5125ef48afe41875a00ca0e");
                    IncenseFogArea.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .ForEach(c => {
                            if (c.m_Buff.deserializedGuid == IncenseFogSickenedBuff.deserializedGuid) {
                                c.m_Buff = Sickened;
                            }
                            if (c.m_Buff.deserializedGuid == IncenseFogNauseatedBuff.deserializedGuid) {
                                c.m_Buff = Nauseated;
                            }
                        });
                    IncenseFog30Area.FlattenAllActions()
                        .OfType<ContextActionApplyBuff>()
                        .ForEach(c => {
                            if (c.m_Buff.deserializedGuid == IncenseFogSickenedBuff.deserializedGuid) {
                                c.m_Buff = Sickened;
                            }
                            if (c.m_Buff.deserializedGuid == IncenseFogNauseatedBuff.deserializedGuid) {
                                c.m_Buff = Nauseated;
                            }
                        });

                    TTTContext.Logger.LogPatch(IncenseFogArea);
                    TTTContext.Logger.LogPatch(IncenseFog30Area);
                }
                void PatchThickFog() {
                    if (TTTContext.Fixes.Alchemist.Archetypes["IncenseSynthesizer"].IsDisabled("ThickFog")) { return; }

                    var IncenseFogThickFogBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f3716df1416d128469c2fc2fcd8dd85d");
                    IncenseFogThickFogBuff.GetComponent<AddConcealment>().TemporaryContext(c => {
                        c.Descriptor = ConcealmentDescriptor.Fog;
                    });

                    TTTContext.Logger.LogPatch("Patched", IncenseFogThickFogBuff);
                }
            }
            static void PatchVivisectionist() {
                PatchMedicalDiscovery();

                void PatchMedicalDiscovery() {
                    if (TTTContext.Fixes.Alchemist.Archetypes["Vivisectionist"].IsDisabled("Discoveries")) { return; }

                    var VivisectionistArchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("68cbcd9fbf1fb1d489562f829bb97e38");
                    var VivsectionistDiscoverySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("67f499218a0e22944abab6fe1c9eaeee");
                    var ExtraDiscoveryVivsectionist = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(TTTContext, "ExtraDiscoveryVivsectionist");
                    var AdvanceTalents = BlueprintTools.GetBlueprint<BlueprintFeature>("a33b99f95322d6741af83e9381b2391c");

                    var CombatTrick = BlueprintTools.GetBlueprint<BlueprintFeature>("c5158a6622d0b694a99efb1d0025d2c1");
                    var FastStealth = BlueprintTools.GetBlueprint<BlueprintFeature>("97a6aa2b64dd21a4fac67658a91067d7");
                    var FocusingAttackConfused = BlueprintTools.GetBlueprint<BlueprintFeature>("955ff81c596c1c3489406d03e81e6087");
                    var FocusingAttackShaken = BlueprintTools.GetBlueprint<BlueprintFeature>("791f50e199d069d4f8e933996a2ce054");
                    var FocusingAttackSickened = BlueprintTools.GetBlueprint<BlueprintFeature>("79475c263e538c94f8e23907bd570a35");
                    var IronGuts = BlueprintTools.GetBlueprint<BlueprintFeature>("6087e0c9801b5eb48bf48d6e75116aad");
                    var SlowReactions = BlueprintTools.GetBlueprint<BlueprintFeature>("7787030571e87704d9177401c595408e");
                    var WeakeningWoundFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("2b61127f29ba97942868e8677b7633e6");
                    var ConfoundingBlades = BlueprintTools.GetBlueprint<BlueprintFeature>("ce72662a812b1f242849417b2c784b5e");
                    var CripplingStrike = BlueprintTools.GetBlueprint<BlueprintFeature>("b696bd7cb38da194fa3404032483d1db");
                    var DispellingAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("1b92146b8a9830d4bb97ab694335fa7c");

                    RemoveVivisectionistGroup(CombatTrick);
                    RemoveVivisectionistGroup(FastStealth);
                    RemoveVivisectionistGroup(FocusingAttackConfused);
                    RemoveVivisectionistGroup(FocusingAttackShaken);
                    RemoveVivisectionistGroup(FocusingAttackSickened);
                    RemoveVivisectionistGroup(IronGuts);
                    RemoveVivisectionistGroup(SlowReactions);
                    RemoveVivisectionistGroup(WeakeningWoundFeature);
                    RemoveVivisectionistGroup(ConfoundingBlades);
                    RemoveVivisectionistGroup(DispellingAttack);

                    CripplingStrike.TemporaryContext(bp => {
                        bp.GetComponents<PrerequisiteFeaturesFromList>(c => c.Features.Contains(AdvanceTalents)).ForEach(c => {
                            c.Group = Prerequisite.GroupType.Any;
                        });
                        bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                            c.Group = Prerequisite.GroupType.Any;
                            c.Level = 10;
                            c.m_Archetype = VivisectionistArchetype;
                            c.m_CharacterClass = ClassTools.ClassReferences.AlchemistClass;
                        });
                    });

                    VivsectionistDiscoverySelection.TemporaryContext(bp => {
                        bp.RemoveFeatures(f => f.HasGroup(FeatureGroup.RogueTalent) && !f.HasGroup(FeatureGroup.VivisectionistDiscovery));
                    });
                    ExtraDiscoveryVivsectionist.TemporaryContext(bp => {
                        bp.RemoveFeatures(f => f.HasGroup(FeatureGroup.RogueTalent) && !f.HasGroup(FeatureGroup.VivisectionistDiscovery));
                    });
                    VivisectionistArchetype.Get()?.TemporaryContext(bp => {
                        bp.AddFeatures = bp.AddFeatures.Where(entry => !entry.Features.Any(f => f.AssetGuid == AdvanceTalents.AssetGuid)).ToArray();
                    });

                    TTTContext.Logger.LogPatch(CripplingStrike);
                    TTTContext.Logger.LogPatch(VivsectionistDiscoverySelection);
                    TTTContext.Logger.LogPatch(ExtraDiscoveryVivsectionist);
                    TTTContext.Logger.LogPatch(VivisectionistArchetype.Get());

                    void RemoveVivisectionistGroup(BlueprintFeature feature) {
                        feature.Groups = feature.Groups.Where(g => g != FeatureGroup.VivisectionistDiscovery).ToArray();
                    }
                }
            }
        }
    }
}
