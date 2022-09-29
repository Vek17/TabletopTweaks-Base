using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Clases {
    class Rogue {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class Rogue_AlternateCapstone_Patch {
            static bool Initialized;
            [HarmonyPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Rogue")) { return; }

                var MasterStrike = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("72dcf1fb106d5054a81fd804fdc168d3");
                var RogueAlternateCapstone = NewContent.AlternateCapstones.Rogue.RogueAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                MasterStrike.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.RogueClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == MasterStrike.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(RogueAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(RogueAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == MasterStrike.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(RogueAlternateCapstone));
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
                TTTContext.Logger.LogHeader("Patching Rogue");

                PatchBase();
                PatchEldritchScoundrel();
                PatchSylvanTrickster();
            }
            static void PatchBase() {
                PatchTrapfinding();
                PatchRogueTalentSelection();
                PatchDispellingAttack();
                PatchSlipperyMind();

                void PatchTrapfinding() {
                    if (Main.TTTContext.Fixes.Rogue.Base.IsDisabled("Trapfinding")) { return; }
                    var Trapfinding = BlueprintTools.GetBlueprint<BlueprintFeature>("dbb6b3bffe6db3547b31c3711653838e");
                    Trapfinding.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                        c.Stat = StatType.SkillThievery;
                        c.Multiplier = 1;
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    }));
                    Trapfinding.SetDescription(TTTContext, "A rogue adds 1/2 her level on Perception checks and Trickery checks.");
                    TTTContext.Logger.LogPatch("Patched", Trapfinding);
                }
                void PatchRogueTalentSelection() {
                    if (Main.TTTContext.Fixes.Rogue.Base.IsDisabled("RogueTalentSelection")) { return; }

                    var RogueTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
                    var CombatTrick = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c5158a6622d0b694a99efb1d0025d2c1");
                    RogueTalentSelection.AllFeatures.ForEach(feature => {
                        if (!feature.HasGroup(FeatureGroup.Feat) && feature.HasGroup(FeatureGroup.RogueTalent)) {
                            feature.AddPrerequisite<PrerequisiteNoFeature>(p => {
                                p.m_Feature = feature.ToReference<BlueprintFeatureReference>();
                                p.Group = Prerequisite.GroupType.All;
                            });
                        }
                    });
                    CombatTrick.SetDescription(TTTContext, "A character that selects this talent gains a bonus combat feat.");
                    TTTContext.Logger.LogPatch(RogueTalentSelection);
                    TTTContext.Logger.LogPatch(CombatTrick);
                }
                void PatchDispellingAttack() {
                    if (Main.TTTContext.Fixes.Rogue.Base.IsDisabled("DispellingAttack")) { return; }

                    var DispellingAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("1b92146b8a9830d4bb97ab694335fa7c");
                    DispellingAttack.FlattenAllActions()
                        .OfType<ContextActionDispelMagic>()
                        .ForEach(a => {
                            a.m_CheckType = Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.CasterLevel;
                            a.m_UseMaxCasterLevel = true;
                            a.m_MaxCasterLevel = new ContextValue() {
                                ValueType = ContextValueType.Rank
                            };
                            a.ContextBonus = new ContextValue();
                            a.OnlyTargetEnemyBuffs = true;
                            a.OneRollForAll = true;
                        });
                    TTTContext.Logger.LogPatch(DispellingAttack);
                }
                void PatchSlipperyMind() {
                    if (Main.TTTContext.Fixes.Rogue.Base.IsDisabled("SlipperyMind")) { return; }

                    var SlipperyMind = BlueprintTools.GetBlueprint<BlueprintFeature>("a14e8c1801911334f96d410f10eab7bf");
                    var AdvanceTalents = BlueprintTools.GetBlueprint<BlueprintFeature>("a33b99f95322d6741af83e9381b2391c");

                    SlipperyMind.AddPrerequisiteFeature(AdvanceTalents);
                    TTTContext.Logger.LogPatch(SlipperyMind);
                }
            }
            static void PatchEldritchScoundrel() {
                PatchSneakAttackProgression();
                PatchRogueTalentProgression();

                void PatchSneakAttackProgression() {
                    if (Main.TTTContext.Fixes.Rogue.Archetypes["EldritchScoundrel"].IsDisabled("SneakAttackProgression")) { return; }
                    var EldritchScoundrelArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("57f93dd8423c97c49989501281296c4a");
                    var SneakAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
                    EldritchScoundrelArchetype.RemoveFeatures = EldritchScoundrelArchetype.RemoveFeatures.AppendToArray(Helpers.CreateLevelEntry(1, SneakAttack));

                    TTTContext.Logger.LogPatch("Patched", EldritchScoundrelArchetype);
                }
                void PatchRogueTalentProgression() {
                    if (Main.TTTContext.Fixes.Rogue.Archetypes["EldritchScoundrel"].IsDisabled("RogueTalentProgression")) { return; }
                    var EldritchScoundrelArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("57f93dd8423c97c49989501281296c4a");
                    var SneakAttack = BlueprintTools.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
                    var RogueTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
                    var UncannyDodgeChecker = BlueprintTools.GetBlueprint<BlueprintFeature>("8f800ed6ce8c42e8a01fd8f3e990c459");

                    EldritchScoundrelArchetype.RemoveFeatures = EldritchScoundrelArchetype.RemoveFeatures
                        .Where(entry => entry.Level != 4)
                        .AddItem(new LevelEntry() {
                            m_Features = new List<BlueprintFeatureBaseReference>() { RogueTalentSelection.ToReference<BlueprintFeatureBaseReference>() },
                            Level = 2
                        })
                        .AddItem(new LevelEntry() {
                            m_Features = new List<BlueprintFeatureBaseReference>() { UncannyDodgeChecker.ToReference<BlueprintFeatureBaseReference>() },
                            Level = 4
                        }).ToArray();
                    TTTContext.Logger.LogPatch("Patched", EldritchScoundrelArchetype);
                }
            }
            static void PatchSylvanTrickster() {
                PatchRogueTalentSelection();

                void PatchRogueTalentSelection() {
                    if (Main.TTTContext.Fixes.Rogue.Archetypes["SylvanTrickster"].IsDisabled("FeyTricks")) { return; }
                    var RogueTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
                    var SylvanTricksterTalentSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("290bbcc3c3bb92144b853fd8fb8ff452");
                    SylvanTricksterTalentSelection.AddFeatures(RogueTalentSelection.m_AllFeatures);

                    TTTContext.Logger.LogPatch("Patched", SylvanTricksterTalentSelection);
                }
            }
        }
    }
}
