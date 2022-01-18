using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Clases {
    class Rogue {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Rogue");

                PatchBase();
                PatchEldritchScoundrel();
            }
            static void PatchBase() {
                PatchTrapfinding();
                PatchRogueTalentSelection();
                PatchSlipperyMind();

                void PatchTrapfinding() {
                    if (ModSettings.Fixes.Rogue.Base.IsDisabled("Trapfinding")) { return; }
                    var Trapfinding = Resources.GetBlueprint<BlueprintFeature>("dbb6b3bffe6db3547b31c3711653838e");
                    Trapfinding.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                        c.Stat = StatType.SkillThievery;
                        c.Multiplier = 1;
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    }));
                    Trapfinding.SetDescription("A rogue adds 1/2 her level on Perception checks and Trickery checks.");
                    Main.LogPatch("Patched", Trapfinding);
                }
                void PatchRogueTalentSelection() {
                    if (ModSettings.Fixes.Rogue.Base.IsDisabled("RogueTalentSelection")) { return; }
                    var RogueTalentSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
                    RogueTalentSelection.Mode = SelectionMode.OnlyNew;
                    Main.LogPatch("Patched", RogueTalentSelection);
                }
                void PatchSlipperyMind() {
                    if (ModSettings.Fixes.Rogue.Base.IsDisabled("SlipperyMind")) { return; }
                    var AdvanceTalents = Resources.GetBlueprint<BlueprintFeature>("a33b99f95322d6741af83e9381b2391c");
                    var SlipperyMind = Resources.GetBlueprint<BlueprintFeature>("a14e8c1801911334f96d410f10eab7bf");
                    SlipperyMind.AddComponent(Helpers.Create<RecalculateOnStatChange>(c => {
                        c.Stat = StatType.Dexterity;
                    }));
                    SlipperyMind.AddPrerequisiteFeature(AdvanceTalents);
                    Main.LogPatch("Patched", SlipperyMind);
                }
            }
            static void PatchEldritchScoundrel() {
                PatchSneakAttackProgression();
                PatchRogueTalentProgression();

                void PatchSneakAttackProgression() {
                    if (ModSettings.Fixes.Rogue.Archetypes["EldritchScoundrel"].IsDisabled("SneakAttackProgression")) { return; }
                    var EldritchScoundrelArchetype = Resources.GetBlueprint<BlueprintArchetype>("57f93dd8423c97c49989501281296c4a");
                    var SneakAttack = Resources.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
                    EldritchScoundrelArchetype.RemoveFeatures = EldritchScoundrelArchetype.RemoveFeatures.AppendToArray(Helpers.LevelEntry(1, SneakAttack));

                    Main.LogPatch("Patched", EldritchScoundrelArchetype);
                }
                void PatchRogueTalentProgression() {
                    if (ModSettings.Fixes.Rogue.Archetypes["EldritchScoundrel"].IsDisabled("RogueTalentProgression")) { return; }
                    var EldritchScoundrelArchetype = Resources.GetBlueprint<BlueprintArchetype>("57f93dd8423c97c49989501281296c4a");
                    var SneakAttack = Resources.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
                    var RogueTalentSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
                    var UncannyDodgeChecker = Resources.GetBlueprint<BlueprintFeature>("8f800ed6ce8c42e8a01fd8f3e990c459");

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
                    Main.LogPatch("Patched", EldritchScoundrelArchetype);
                }
            }
        }
    }
}
