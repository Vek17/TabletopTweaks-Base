using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    internal class Skald {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Skald");
                PatchAlternateCapstone();
                PatchBase();
            }
            static void PatchAlternateCapstone() {
                if (Main.TTTContext.Fixes.AlternateCapstones.IsDisabled("Skald")) { return; }

                var MasterSkald = BlueprintTools.GetBlueprintReference<BlueprintFeatureBaseReference>("ae4d45a39a91dee4fb4200d7a677d9a7");
                var SkaldAlternateCapstone = NewContent.AlternateCapstones.Skald.SkaldAlternateCapstone.ToReference<BlueprintFeatureBaseReference>();

                MasterSkald.Get().TemporaryContext(bp => {
                    bp.AddComponent<PrerequisiteInPlayerParty>(c => {
                        c.CheckInProgression = true;
                        c.HideInUI = true;
                        c.Not = true;
                    });
                    bp.HideNotAvailibleInUI = true;
                    TTTContext.Logger.LogPatch(bp);
                });
                ClassTools.Classes.SkaldClass.TemporaryContext(bp => {
                    bp.Progression.UIGroups
                        .Where(group => group.m_Features.Any(f => f.deserializedGuid == MasterSkald.deserializedGuid))
                        .ForEach(group => group.m_Features.Add(SkaldAlternateCapstone));
                    bp.Progression.LevelEntries
                        .Where(entry => entry.Level == 20)
                        .ForEach(entry => entry.m_Features.Add(SkaldAlternateCapstone));
                    bp.Archetypes.ForEach(a => {
                        a.RemoveFeatures
                            .Where(remove => remove.Level == 20)
                            .Where(remove => remove.m_Features.Any(f => f.deserializedGuid == MasterSkald.deserializedGuid))
                            .ForEach(remove => remove.m_Features.Add(SkaldAlternateCapstone));
                    });
                    TTTContext.Logger.LogPatch("Enabled Alternate Capstones", bp);
                });
            }
            static void PatchBase() {
            }
        }
    }
}
