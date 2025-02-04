using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Slayer {
        [PatchBlueprintsCacheInit]
        static class Slayer_AlternateCapstone_Patch {
            static bool Initialized;
            [PatchBlueprintsCacheInitPriority(Priority.Last)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                PatchAlternateCapstone();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Skald")) { return; }

                var MasterSlayerFeature = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("a26c0279a423fc94cabeea898f4d9f8a");
                var SlayerAlternateCapstone = NewContent.AlternateCapstones.Slayer.SlayerAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                MasterSlayerFeature.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.SlayerClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == MasterSlayerFeature.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(SlayerAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(SlayerAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == MasterSlayerFeature.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(SlayerAlternateCapstone));
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
                TTTContext.Logger.LogHeader("Patching Slayer");

                PatchBase();
            }
            static void PatchBase() {
                PatchSlayerTrapfinding();

                void PatchSlayerTrapfinding() {
                    if (TTTContext.Fixes.Slayer.Base.IsDisabled("Trapfinding")) { return; }
                    var SlayerTrapfinding = BlueprintTools.GetBlueprint<BlueprintFeature>("e3c12938c2f93544da89824fbe0933a5");
                    SlayerTrapfinding.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                        c.Stat = StatType.SkillThievery;
                        c.Multiplier = 1;
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    }));
                    TTTContext.Logger.LogPatch("Patched", SlayerTrapfinding);
                }
            }
        }
    }
}
