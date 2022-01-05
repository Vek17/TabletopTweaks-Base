using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Alchemist {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Alchemist Resources");

                PatchBase();
                PatchGrenadier();
            }

            static void PatchBase() {
                PatchMutagens();

                void PatchMutagens() {
                    if (ModSettings.Fixes.Alchemist.Base.IsDisabled("MutagenStacking")) { return; }

                    var mutagenBuffs = new BlueprintBuff[] {
                        Resources.GetBlueprint<BlueprintBuff>("32f2bc843effd9b45a0952a3cffbbe9f"),    // CognatogenCharismaBuff,
                        Resources.GetBlueprint<BlueprintBuff>("20e740104092b5e49bfb167f1670a9de"),    // CognatogenIntelligenceBuff,
                        Resources.GetBlueprint<BlueprintBuff>("6871149a90e278f479aa171ee8bb563e"),    // CognatogenWisdomBuff,
                        Resources.GetBlueprint<BlueprintBuff>("a5a6f915d13fd994fb109473032d7440"),    // GrandCognatogenCharismaIntelligenceBuff,
                        Resources.GetBlueprint<BlueprintBuff>("608dd115b3b0fba4ab511f448bc798f8"),    // GrandCognatogenCharismaWisdomBuff,
                        Resources.GetBlueprint<BlueprintBuff>("8de52f7aa6052a0498875e0d834330af"),    // GrandCognatogenIntelligenceCharismaBuff,
                        Resources.GetBlueprint<BlueprintBuff>("ac7753d72b0b7264982c2b6670fa2a2e"),    // GrandCognatogenIntelligenceWisdomBuff,
                        Resources.GetBlueprint<BlueprintBuff>("232fe914c22744c4ea3e050901bda424"),    // GrandCognatogenWisdomCharismaBuff,
                        Resources.GetBlueprint<BlueprintBuff>("98a46e8da1dca9f47b41b9d71d579628"),    // GrandCognatogenWisdomIntelligenceBuff,
                        Resources.GetBlueprint<BlueprintBuff>("0d51a2ff0a6ce85458309affbc00b933"),    // GrandMutagenConstitutionDexterityBuff,
                        Resources.GetBlueprint<BlueprintBuff>("9c3761b9f48f69849ad78873c5a12147"),    // GrandMutagenConstitutionStrengthBuff,
                        Resources.GetBlueprint<BlueprintBuff>("8d4357118c75a5746802a3582a937376"),    // GrandMutagenDexterityConstitutionBuff,
                        Resources.GetBlueprint<BlueprintBuff>("bf73a2b70b6fac54e891431cf6c7d8eb"),    // GrandMutagenDexterityStrengthBuff,
                        Resources.GetBlueprint<BlueprintBuff>("204a74affae72d54984fb533704caf72"),    // GrandMutagenStrengthConstitutionBuff,
                        Resources.GetBlueprint<BlueprintBuff>("3b7cf6307d3e61545a977c9f4156e12e"),    // GrandMutagenStrengthDexterityBuff,
                        Resources.GetBlueprint<BlueprintBuff>("b60f8b93d3d1d26439c1bb48fd461a3a"),    // GreaterCognatogenCharismaIntelligenceBuff,
                        Resources.GetBlueprint<BlueprintBuff>("61271a59038390c488c313f7a0aee6ea"),    // GreaterCognatogenCharismaWisdomBuff,
                        Resources.GetBlueprint<BlueprintBuff>("1c2fdba3b33dacd41afd5b74d84c7332"),    // GreaterCognatogenIntelligenceCharismaBuff,
                        Resources.GetBlueprint<BlueprintBuff>("34fde71198d30094aa133546e8cf8733"),    // GreaterCognatogenIntelligenceWisdomBuff,
                        Resources.GetBlueprint<BlueprintBuff>("60eb20b9d1077ed4f8f8a9df5490a208"),    // GreaterCognatogenWisdomCharismaBuff,
                        Resources.GetBlueprint<BlueprintBuff>("bc0890817bb28fe4a86094fe57cd40fb"),    // GreaterCognatogenWisdomIntelligenceBuff,
                        Resources.GetBlueprint<BlueprintBuff>("a8e7ca242395c3b49af5a3dbc9dee683"),    // GreaterMutagenConstitutionDexterityBuff,
                        Resources.GetBlueprint<BlueprintBuff>("84c42fea967a2a8499ceeaef3a6416b8"),    // GreaterMutagenConstitutionStrengthBuff,
                        Resources.GetBlueprint<BlueprintBuff>("84ae955af09809b4ea31a2c719c68377"),    // GreaterMutagenDexterityConstitutionBuff,
                        Resources.GetBlueprint<BlueprintBuff>("d0a5cedfd497f3b4f9581b6066d9043b"),    // GreaterMutagenDexterityStrengthBuff,
                        Resources.GetBlueprint<BlueprintBuff>("83ed8d5c1e4ed9045874494c0fe2b682"),    // GreaterMutagenStrengthConstitutionBuff,
                        Resources.GetBlueprint<BlueprintBuff>("a42c49fcb081bd1469679e4f515732c8"),    // GreaterMutagenStrengthDexterityBuff,
                        Resources.GetBlueprint<BlueprintBuff>("bd48322a4e258b8418106dcc6459e024"),    // MutagenConstitutionBuff,
                        Resources.GetBlueprint<BlueprintBuff>("f2be3d538b5d75c409289d35399723c4"),    // MutagenDexterityBuff,
                        Resources.GetBlueprint<BlueprintBuff>("b84abc3531ed5674284ef0ba4aafcd3b"),    // MutagenStrengthBuff,
                        Resources.GetBlueprint<BlueprintBuff>("3fb9e9a6408589343bc8bfc3fd1610e5"),    // TrueMutagenBuff,
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
                        Main.LogPatch("Patched", mutagen);
                    }
                }
            }

            static void PatchGrenadier() {
                PatchBrewPotions();
                PatchPoisonResistance();

                void PatchBrewPotions() {
                    if (ModSettings.Fixes.Alchemist.Archetypes["Grenadier"].IsDisabled("BrewPotions")) { return; }

                    var GrenadierArchetype = Resources.GetBlueprint<BlueprintArchetype>("6af888a7800b3e949a40f558ff204aae");
                    var BrewPotions = Resources.GetBlueprint<BlueprintFeature>("c0f8c4e513eb493408b8070a1de93fc0");

                    GrenadierArchetype.RemoveFeatures = GrenadierArchetype.RemoveFeatures.AppendToArray(new LevelEntry() {
                        Level = 1,
                        m_Features = new List<BlueprintFeatureBaseReference>() {
                            BrewPotions.ToReference<BlueprintFeatureBaseReference>()
                        }
                    }); ;
                    Main.LogPatch("Patched", GrenadierArchetype);
                }

                void PatchPoisonResistance() {
                    if (ModSettings.Fixes.Alchemist.Archetypes["Grenadier"].IsDisabled("PoisonResistance")) { return; }

                    var GrenadierArchetype = Resources.GetBlueprint<BlueprintArchetype>("6af888a7800b3e949a40f558ff204aae");
                    var PoisonResistance = Resources.GetBlueprint<BlueprintFeature>("c9022272c87bd66429176ce5c597989c");
                    var PoisonImmunity = Resources.GetBlueprint<BlueprintFeature>("202af59b918143a4ab7c33d72c8eb6d5");

                    GrenadierArchetype.RemoveFeatures = GrenadierArchetype.RemoveFeatures
                        .Where(entry => !new int[] { 2, 5, 8, 10 }.Contains(entry.Level))
                        .Concat(new LevelEntry[] {
                            Helpers.CreateLevelEntry(2, PoisonResistance),
                            Helpers.CreateLevelEntry(5, PoisonResistance),
                            Helpers.CreateLevelEntry(8, PoisonResistance),
                            Helpers.CreateLevelEntry(10, PoisonImmunity)
                        }).ToArray();
                    Main.LogPatch("Patched", GrenadierArchetype);
                }
            }
        }
    }
}
