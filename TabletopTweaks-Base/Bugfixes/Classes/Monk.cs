using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Monk {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class Monk_AlternateCapstone_Patch {
            static bool Initialized;
            [HarmonyPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Monk")) { return; }

                var KiPerfectSelfFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("3854f693180168a4980646aee9494c72");
                var MonkAlternateCapstone = NewContent.AlternateCapstones.Monk.MonkAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                KiPerfectSelfFeature.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.MonkClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == KiPerfectSelfFeature.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(MonkAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(MonkAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == KiPerfectSelfFeature.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(MonkAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
        }
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Monk");

                PatchBase();
                PatchZenArcher();
                PatchScaledFist();
            }
            static void PatchBase() {
                PatchStunningFistVarriants();
                PatchStunningFistDescriptors();

                void PatchStunningFistVarriants() {
                    if (TTTContext.Fixes.Monk.Base.IsDisabled("StunningFistVarriants")) { return; }
                    var MonkProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a91753b978e3b34b9425419179aafd6");

                    var StunningFist = BlueprintTools.GetBlueprint<BlueprintFeature>("a29a582c3daa4c24bb0e991c596ccb28");
                    var StunningFistSickenedFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d256ab3837538cc489d4b571e3a813eb");
                    var StunningFistFatigueFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("819645da2e446f84d9b168ed1676ec29");
                    var StunningFistStaggeredFeature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureBaseReference>(TTTContext, "StunningFistStaggeredFeature");
                    var StunningFistBlindFeature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureBaseReference>(TTTContext, "StunningFistBlindFeature");
                    var StunningFistParalyzeFeature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureBaseReference>(TTTContext, "StunningFistParalyzeFeature");

                    var StunningFistAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("732ae7773baf15447a6737ae6547fc1e");
                    var StunningFistFatigueAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("32f92fea1ab81c843a436a49f522bfa1");
                    var StunningFistSickenedAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("c81906c75821cbe4c897fa11bdaeee01");
                    var StunningFistOwnerFatigueBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("696b29374599d4141be64e46a91bd09b");

                    StunningFistAbility.RemoveComponents<SpellDescriptorComponent>();
                    StunningFistFatigueAbility.RemoveComponents<SpellDescriptorComponent>();
                    StunningFistSickenedAbility.RemoveComponents<SpellDescriptorComponent>();
                    StunningFistOwnerFatigueBuff
                        .GetComponents<AddInitiatorAttackWithWeaponTrigger>(c => c.ActionsOnInitiator)
                        .ForEach(c => c.OnlyHit = false);

                    MonkProgression.LevelEntries
                        .Where(entry => entry.Level == 12)
                        .ForEach(entry => entry.m_Features.Add(StunningFistStaggeredFeature));
                    MonkProgression.LevelEntries
                        .Where(entry => entry.Level == 16)
                        .ForEach(entry => entry.m_Features.Add(StunningFistBlindFeature));
                    MonkProgression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(StunningFistParalyzeFeature));
                    MonkProgression.UIGroups
                        .Where(group => group.Features.Contains(StunningFist))
                        .ForEach(group => {
                            group.m_Features.Add(StunningFistStaggeredFeature);
                            group.m_Features.Add(StunningFistBlindFeature);
                            group.m_Features.Add(StunningFistParalyzeFeature);
                        });
                    SaveGameFix.AddUnitPatch((unit) => {
                        var progressionData = unit.Progression;
                        var classData = unit.Progression.GetClassData(ClassTools.Classes.MonkClass);
                        if (classData == null) { return; }
                        var levelEntries = progressionData.SureProgressionData(classData.CharacterClass.Progression).LevelEntries;
                        foreach (LevelEntry entry in levelEntries.Where(e => e.Level <= classData.Level)) {
                            foreach (BlueprintFeatureBase feature in entry.Features) {
                                if (feature.AssetGuid == StunningFistStaggeredFeature.deserializedGuid) {
                                    if (progressionData.Features.HasFact(StunningFistStaggeredFeature)) { continue; }
                                    var addedFeature = progressionData.Features.AddFeature((BlueprintFeature)feature, null);
                                    var characterClass = classData.CharacterClass;
                                    addedFeature.SetSource(characterClass.Progression, entry.Level);
                                    TTTContext.Logger.Log($"{unit.CharacterName}: Applied StunningFistStaggeredFeature");
                                }
                                if (feature.AssetGuid == StunningFistBlindFeature.deserializedGuid) {
                                    if (progressionData.Features.HasFact(StunningFistBlindFeature)) { continue; }
                                    var addedFeature = progressionData.Features.AddFeature((BlueprintFeature)feature, null);
                                    var characterClass = classData.CharacterClass;
                                    addedFeature.SetSource(characterClass.Progression, entry.Level);
                                    TTTContext.Logger.Log($"{unit.CharacterName}: Applied StunningFistBlindFeature");
                                }
                                if (feature.AssetGuid == StunningFistParalyzeFeature.deserializedGuid) {
                                    if (progressionData.Features.HasFact(StunningFistParalyzeFeature)) { continue; }
                                    var addedFeature = progressionData.Features.AddFeature((BlueprintFeature)feature, null);
                                    var characterClass = classData.CharacterClass;
                                    addedFeature.SetSource(characterClass.Progression, entry.Level);
                                    TTTContext.Logger.Log($"{unit.CharacterName}: Applied StunningFistParalyzeFeature");
                                }
                            }
                        }
                    });
                }
                void PatchStunningFistDescriptors() {
                    if (TTTContext.Fixes.Monk.Base.IsDisabled("StunningFistDescriptors")) { return; }

                    var StunningFistAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("732ae7773baf15447a6737ae6547fc1e");
                    var StunningFistFatigueAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("32f92fea1ab81c843a436a49f522bfa1");
                    var StunningFistSickenedAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("c81906c75821cbe4c897fa11bdaeee01");
                    var StunningFistOwnerFatigueBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("696b29374599d4141be64e46a91bd09b");

                    StunningFistAbility.RemoveComponents<SpellDescriptorComponent>();
                    StunningFistFatigueAbility.RemoveComponents<SpellDescriptorComponent>();
                    StunningFistSickenedAbility.RemoveComponents<SpellDescriptorComponent>();
                    StunningFistOwnerFatigueBuff
                        .GetComponents<AddInitiatorAttackWithWeaponTrigger>(c => c.ActionsOnInitiator)
                        .ForEach(c => c.OnlyHit = false);
                }
            }

            static void PatchZenArcher() {
                PatchPerfectStrike();

                void PatchPerfectStrike() {
                    if (TTTContext.Fixes.Monk.Archetypes["ZenArcher"].IsDisabled("PerfectStrike")) { return; }
                    var PerfectStrikeAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("bc656f51e407aad40bc8d974f3d5b04a");
                    var PerfectStrikeOwnerBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("9a41e6d073b42564b9f00ad83b7d3b52");
                    var PerfectStrikeZenArcherBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(TTTContext, "PerfectStrikeZenArcherBuff");
                    var PerfectStrikeFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("7477e2e0b72f4ce4fb674f4b21d5e81d");
                    var PerfectStrikeZenArcherUpgrade = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "PerfectStrikeZenArcherUpgrade");
                    var ZenArcherArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("2b1a58a7917084f49b097e86271df21c");
                    var MonkProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a91753b978e3b34b9425419179aafd6");
                    var MonkClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("e8f21e5b58e0569468e420ebea456124");

                    PerfectStrikeAbility.RemoveComponents<AbilityEffectRunAction>();
                    PerfectStrikeAbility.AddComponent<AbilityEffectRunAction>();
                    PerfectStrikeAbility.GetComponent<AbilityEffectRunAction>()
                        .AddAction<Conditional>(a => {
                            a.ConditionsChecker = new ConditionsChecker() {
                                Conditions = new Condition[] {
                                    Helpers.Create<ContextConditionHasFact>( c => {
                                        c.m_Fact = PerfectStrikeZenArcherUpgrade.ToReference<BlueprintUnitFactReference>();
                                        c.Not = true;
                                    })
                                }
                            };
                            a.IfTrue = new ActionList() {
                                Actions = new GameAction[] {
                                    Helpers.Create<ContextActionApplyBuff>(c => {
                                        c.m_Buff = PerfectStrikeOwnerBuff.ToReference<BlueprintBuffReference>();
                                        c.Permanent = true;
                                        c.DurationValue = new ContextDurationValue() {
                                            m_IsExtendable = true,
                                            DiceCountValue = new ContextValue(),
                                            BonusValue = new ContextValue()
                                        };
                                        c.AsChild = true;
                                    })
                                }
                            };
                            a.IfFalse = new ActionList() {
                                Actions = new GameAction[] {
                                    Helpers.Create<ContextActionApplyBuff>(c => {
                                        c.m_Buff = PerfectStrikeZenArcherBuff.ToReference<BlueprintBuffReference>();
                                        c.Permanent = true;
                                        c.DurationValue = new ContextDurationValue() {
                                            m_IsExtendable = true,
                                            DiceCountValue = new ContextValue(),
                                            BonusValue = new ContextValue()
                                        };
                                        c.AsChild = true;
                                    })
                                }
                            };
                        });
                    PerfectStrikeAbility.GetComponent<AbilityCasterHasNoFacts>()
                        .m_Facts = new BlueprintUnitFactReference[] {
                            PerfectStrikeOwnerBuff.ToReference<BlueprintUnitFactReference>(),
                            PerfectStrikeZenArcherBuff.ToReference<BlueprintUnitFactReference>()
                        };
                    ZenArcherArchetype.AddFeatures
                        .Where(entry => entry.Level == 10)
                        .First().m_Features.Add(PerfectStrikeZenArcherUpgrade.ToReference<BlueprintFeatureBaseReference>());
                    MonkProgression.UIGroups = MonkProgression.UIGroups.AppendToArray(new UIGroup {
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            PerfectStrikeFeature.ToReference<BlueprintFeatureBaseReference>(),
                            PerfectStrikeZenArcherUpgrade.ToReference<BlueprintFeatureBaseReference>()
                        }
                    });
                    SaveGameFix.AddUnitPatch((unit) => {
                        var progressionData = unit.Progression;
                        var classData = unit.Progression.GetClassData(ClassTools.Classes.MonkClass);
                        if (classData == null) { return; }
                        var levelEntries = progressionData.SureProgressionData(classData.CharacterClass.Progression).LevelEntries;
                        foreach (LevelEntry entry in levelEntries.Where(e => e.Level <= classData.Level)) {
                            foreach (BlueprintFeatureBase feature in entry.Features) {
                                if (feature.AssetGuid == PerfectStrikeZenArcherUpgrade.AssetGuid) {
                                    if (progressionData.Features.HasFact(PerfectStrikeZenArcherUpgrade)) { continue; }
                                    var addedFeature = progressionData.Features.AddFeature((BlueprintFeature)feature, null);
                                    var characterClass = classData.CharacterClass;
                                    addedFeature.SetSource(characterClass.Progression, entry.Level);
                                    TTTContext.Logger.Log($"{unit.CharacterName}: Applied PerfectStrikeZenArcherUpgrade");
                                }
                            }
                        }
                    });
                    TTTContext.Logger.LogPatch("Patched", PerfectStrikeAbility);
                    TTTContext.Logger.LogPatch("Patched", ZenArcherArchetype);
                    TTTContext.Logger.LogPatch("Patched", MonkProgression);
                }
            }
            static void PatchScaledFist() {
                PatchStunningFist();
                PatchDraconicFury();

                void PatchStunningFist() {
                    if (TTTContext.Fixes.Monk.Archetypes["ScaledFist"].IsDisabled("FixStunningStrike")) { return; }

                    var ScaledFistArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("5868fc82eb11a4244926363983897279");
                    var MonkClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("e8f21e5b58e0569468e420ebea456124");
                    var MonkProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a91753b978e3b34b9425419179aafd6");

                    var StunningFist = BlueprintTools.GetBlueprint<BlueprintFeature>("a29a582c3daa4c24bb0e991c596ccb28");
                    var StunningFistResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("d2bae584db4bf4f4f86dd9d15ae56558");
                    var StunningFistAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("732ae7773baf15447a6737ae6547fc1e");
                    var StunningFistSickenedFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("d256ab3837538cc489d4b571e3a813eb");
                    var StunningFistFatigueFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("819645da2e446f84d9b168ed1676ec29");
                    var ScaledFistStunningFistFatigueFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("abcf396b95e3dbc4686c8547783a719c");
                    var ScaledFistStunningFistSickenedFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("e754ea837ee7a6e438ff7ebf6da40b79");

                    var StunningFistFatigueAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("32f92fea1ab81c843a436a49f522bfa1");
                    var StunningFistSickenedAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c81906c75821cbe4c897fa11bdaeee01");

                    StunningFistFatigueFeature.Get().TemporaryContext(bp => {
                        bp.RemoveComponents<ReplaceAbilityDC>();
                        bp.AddComponent<MonkReplaceAbilityDC>(c => {
                            c.m_Ability = StunningFistFatigueAbility;
                            c.m_Monk = MonkClass;
                            c.m_ScaledFist = ScaledFistArchetype.ToReference<BlueprintArchetypeReference>();
                        });
                    });
                    StunningFistSickenedFeature.Get().TemporaryContext(bp => {
                        bp.RemoveComponents<ReplaceAbilityDC>();
                        bp.AddComponent<MonkReplaceAbilityDC>(c => {
                            c.m_Ability = StunningFistSickenedAbility;
                            c.m_Monk = MonkClass;
                            c.m_ScaledFist = ScaledFistArchetype.ToReference<BlueprintArchetypeReference>();
                        });
                    });

                    ScaledFistStunningFistFatigueFeature.Get().TemporaryContext(bp => {
                        bp.RemoveComponents<ReplaceAbilityDC>();
                        bp.AddComponent<MonkReplaceAbilityDC>(c => {
                            c.m_Ability = StunningFistFatigueAbility;
                            c.m_Monk = MonkClass;
                            c.m_ScaledFist = ScaledFistArchetype.ToReference<BlueprintArchetypeReference>();
                        });
                    });
                    ScaledFistStunningFistFatigueFeature.Get().TemporaryContext(bp => {
                        bp.AddComponent<SavesFixerReplaceInProgression>(c => {
                            c.m_OldFeature = ScaledFistStunningFistFatigueFeature.Get().ToReference<BlueprintFeatureReference>();
                            c.m_NewFeature = StunningFistFatigueFeature.Get().ToReference<BlueprintFeatureReference>();
                        });
                    });
                    ScaledFistStunningFistSickenedFeature.Get().TemporaryContext(bp => {
                        bp.AddComponent<SavesFixerReplaceInProgression>(c => {
                            c.m_OldFeature = ScaledFistStunningFistSickenedFeature.Get().ToReference<BlueprintFeatureReference>();
                            c.m_NewFeature = StunningFistSickenedFeature.Get().ToReference<BlueprintFeatureReference>();
                        });
                    });

                    ScaledFistArchetype.RemoveFeatures
                        .Where(entry => entry.Level == 4)
                        .ForEach(entry => entry.m_Features.Remove(StunningFistFatigueFeature));
                    ScaledFistArchetype.RemoveFeatures
                        .Where(entry => entry.Level == 8)
                        .ForEach(entry => entry.m_Features.Remove(StunningFistSickenedFeature));
                    ScaledFistArchetype.AddFeatures
                        .Where(entry => entry.Level == 4)
                        .ForEach(entry => entry.m_Features.Remove(ScaledFistStunningFistFatigueFeature));
                    ScaledFistArchetype.AddFeatures
                        .Where(entry => entry.Level == 8)
                        .ForEach(entry => entry.m_Features.Remove(ScaledFistStunningFistSickenedFeature));
                }
                void PatchDraconicFury() {
                    if (TTTContext.Fixes.Monk.Archetypes["ScaledFist"].IsDisabled("DraconicFury")) { return; }
                    var ScaledFistArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("5868fc82eb11a4244926363983897279");
                    var ScaledFistKiPowerSelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("4694f6ac27eaed34abb7d09ab67b4541");
                    var ScaledFistDragonSelection = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("f9042eed12dac2745a2eb7a9a936906b");

                    var ScaledFistBlackProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("0f61e0b868fd60b4181c85f37e6dd542");
                    var ScaledFistBlueProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("6fdf53dc566ba0d4c9be60aab0a3cdc3");
                    var ScaledFistBrassProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("0a3fadecdc9bb70469503888519d2d11");
                    var ScaledFistBronzeProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("5b4333f6d578d47439931a258c6df9f8");
                    var ScaledFistCopperProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("55a672e8a1dbaf940991ca54cbd45579");
                    var ScaledFistGoldProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("d2945f4278becb142917787b89f39c58");
                    var ScaledFistGreenProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("b978de37326b01440b2d9fd2a739f1ea");
                    var ScaledFistRedProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("6bbc4b6a4a6007c429ee7a87dd5e306f");
                    var ScaledFistSilverProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("80c8d99fb9c1daf4f98cfa7f9e186ce4");
                    var ScaledFistWhiteProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("3506251410c7b2d48a5afc36670e95d0");

                    FixDragonProgression(ScaledFistBlackProgression);
                    FixDragonProgression(ScaledFistBlueProgression);
                    FixDragonProgression(ScaledFistBrassProgression);
                    FixDragonProgression(ScaledFistBronzeProgression);
                    FixDragonProgression(ScaledFistCopperProgression);
                    FixDragonProgression(ScaledFistGoldProgression);
                    FixDragonProgression(ScaledFistGreenProgression);
                    FixDragonProgression(ScaledFistRedProgression);
                    FixDragonProgression(ScaledFistSilverProgression);
                    FixDragonProgression(ScaledFistWhiteProgression);

                    ScaledFistArchetype.AddFeatures
                        .Where(entry => entry.Level == 3)
                        .ForEach(entry => entry.m_Features.Remove(ScaledFistDragonSelection));
                    ScaledFistArchetype.AddFeatures
                        .Where(entry => entry.Level == 4)
                        .ForEach(entry => {
                            entry.m_Features.Remove(ScaledFistKiPowerSelection);
                            entry.m_Features.Add(ScaledFistDragonSelection);
                        });

                    void FixDragonProgression(BlueprintProgression progression) {
                        progression.TemporaryContext(bp => {
                            bp.GiveFeaturesForPreviousLevels = true;
                            bp.LevelEntries
                                .Where(entry => entry.Level == 15)
                                .ForEach(entry => entry.Level = 12);
                        });
                    }
                }
            }
        }
    }
}
