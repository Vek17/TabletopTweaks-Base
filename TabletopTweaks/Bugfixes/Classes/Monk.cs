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
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Monk {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Monk");

                PatchBase();
                PatchZenArcher();
            }

            static void PatchBase() {
            }

            static void PatchZenArcher() {
                PatchPerfectStrike();

                void PatchPerfectStrike() {
                    if (ModSettings.Fixes.Monk.Archetypes["ZenArcher"].IsDisabled("PerfectStrike")) { return; }
                    var PerfectStrikeAbility = Resources.GetBlueprint<BlueprintAbility>("bc656f51e407aad40bc8d974f3d5b04a");
                    var PerfectStrikeOwnerBuff = Resources.GetBlueprint<BlueprintBuff>("9a41e6d073b42564b9f00ad83b7d3b52");
                    var PerfectStrikeZenArcherBuff = Resources.GetModBlueprint<BlueprintBuff>("PerfectStrikeZenArcherBuff");
                    var PerfectStrikeFeature = Resources.GetBlueprint<BlueprintFeature>("7477e2e0b72f4ce4fb674f4b21d5e81d");
                    var PerfectStrikeZenArcherUpgrade = Resources.GetModBlueprint<BlueprintFeature>("PerfectStrikeZenArcherUpgrade");
                    var ZenArcherArchetype = Resources.GetBlueprint<BlueprintArchetype>("2b1a58a7917084f49b097e86271df21c");
                    var MonkProgression = Resources.GetBlueprint<BlueprintProgression>("8a91753b978e3b34b9425419179aafd6");
                    var MonkClass = Resources.GetBlueprint<BlueprintCharacterClass>("e8f21e5b58e0569468e420ebea456124");

                    PerfectStrikeAbility.RemoveComponents<AbilityEffectRunAction>();
                    PerfectStrikeAbility.AddComponent<AbilityEffectRunAction>();
                    PerfectStrikeAbility.GetComponent<AbilityEffectRunAction>()
                        .AddAction(Helpers.Create<Conditional>(a => {
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
                        }));
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
                        if (unit.Progression.GetClassLevel(MonkClass) >= 10 && unit.Progression.IsArchetype(ZenArcherArchetype)) {
                            if (!unit.HasFact(PerfectStrikeZenArcherUpgrade)) {
                                if (unit.AddFact(PerfectStrikeZenArcherUpgrade) != null) {
                                    Main.Log($"Added: {PerfectStrikeZenArcherUpgrade.name} To: {unit.CharacterName}");
                                    return;
                                }
                                Main.Log($"Failed Add: {PerfectStrikeZenArcherUpgrade.name} To: {unit.CharacterName}");
                            }
                        }
                    });
                    Main.LogPatch("Patched", PerfectStrikeAbility);
                    Main.LogPatch("Patched", ZenArcherArchetype);
                    Main.LogPatch("Patched", MonkProgression);
                }
            }
        }
    }
}
