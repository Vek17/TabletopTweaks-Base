using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    static class Oracle {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Oracle");
                PatchBase();
            }
            static void PatchBase() {
                PatchNaturesWhisper();
                PatchFlameMystery();

                void PatchNaturesWhisper() {
                    if (TTTContext.Fixes.Oracle.Base.IsDisabled("NaturesWhisperMonkStacking")) { return; }

                    var OracleRevelationNatureWhispers = BlueprintTools.GetBlueprint<BlueprintFeature>("3d2cd23869f0d98458169b88738f3c32");
                    var NaturesWhispersACConversion = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "NaturesWhispersACConversion");
                    var ScaledFistACBonus = BlueprintTools.GetBlueprint<BlueprintFeature>("3929bfd1beeeed243970c9fc0cf333f8");

                    OracleRevelationNatureWhispers.RemoveComponents<ReplaceStatBaseAttribute>();
                    OracleRevelationNatureWhispers.RemoveComponents<ReplaceCMDDexterityStat>();
                    OracleRevelationNatureWhispers.AddComponent<HasFactFeatureUnlock>(c => {
                        c.m_CheckedFact = ScaledFistACBonus.ToReference<BlueprintUnitFactReference>();
                        c.m_Feature = NaturesWhispersACConversion.ToReference<BlueprintUnitFactReference>();
                        c.Not = true;
                    });
                    TTTContext.Logger.LogPatch("Patched", OracleRevelationNatureWhispers);
                }

                void PatchFlameMystery() {
                    PatchRevelationBurningMagic();

                    void PatchRevelationBurningMagic() {
                        if (TTTContext.Fixes.Oracle.Base.IsDisabled("RevelationBurningMagic")) { return; }

                        var OracleRevelationBurningMagicBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4ae27ae7c3d758041b25e9a3aff73592");
                        OracleRevelationBurningMagicBuff.GetComponent<ContextRankConfig>().m_Progression = ContextRankProgression.AsIs;
                        TTTContext.Logger.LogPatch("Patched", OracleRevelationBurningMagicBuff);
                    }
                }
            }
            static void PatchArchetypes() {
            }
        }
    }
}
