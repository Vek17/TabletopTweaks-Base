using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
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
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Monk");

                PatchAlternateCapstone();
                PatchBase();
                PatchZenArcher();
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
            static void PatchBase() {
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
        }
    }
}
