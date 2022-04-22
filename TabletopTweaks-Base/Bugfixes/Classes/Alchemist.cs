using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Alchemist {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Alchemist Resources");

                PatchAlternateCapstone();
                PatchBase();
                PatchGrenadier();
                PatchIncenseSynthesizer();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Alchemist")) { return; }

                var DiscoverySelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("cd86c437488386f438dcc9ae727ea2a6");
                var GrandDiscoverySelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("2729af328ab46274394cedc3582d6e98");
                var AlchemistAlternateCapstone = NewContent.AlternateCapstones.Alchemist.AlchemistAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();
                var GrandDiscoveryProgression = NewContent.AlternateCapstones.Alchemist.AlchemistAlternateCapstone.ToReference<BlueprintUnitFactReference>();

                GrandDiscoverySelection.Get().TemporaryContext(bp => {
                    bp.SetDescription(TTTContext, "At 20th level, the alchemist makes a grand discovery. He immediately learns two normal discoveries, " +
                        "but also learns a third discovery chosen from the list below, representing a truly astounding alchemical breakthrough of significant import. " +
                        "For many alchemists, the promise of one of these grand discoveries is the primary goal of their experiments and hard work.");
                    bp.AddComponent<AddFacts>(c => {
                        c.m_Facts = new BlueprintUnitFactReference[] { GrandDiscoveryProgression };
                    });
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.AlchemistClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == GrandDiscoverySelection.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(AlchemistAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => {
                            entry.m_Features.RemoveAll(f => f.deserializedGuid == DiscoverySelection.deserializedGuid);
                            entry.m_Features.Add(AlchemistAlternateCapstone);
                        });
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == GrandDiscoverySelection.deserializedGuid))
                            .ForEach(remove => {
                                remove.m_Features.RemoveAll(f => f.deserializedGuid == DiscoverySelection.deserializedGuid);
                                remove.m_Features.Add(AlchemistAlternateCapstone);
                            });
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
            static void PatchBase() {
                PatchMutagens();

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
        }
    }
}
