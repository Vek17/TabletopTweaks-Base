using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Oracle {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Oracle");
                PatchBase();
            }
            static void PatchBase() {
                PatchNaturesWhisper();
                PatchFlameMystery();

                void PatchNaturesWhisper() {
                    if (ModSettings.Fixes.Oracle.Base.IsDisabled("NaturesWhisperMonkStacking")) { return; }

                    var OracleRevelationNatureWhispers = Resources.GetBlueprint<BlueprintFeature>("3d2cd23869f0d98458169b88738f3c32");
                    var NaturesWhispersACConversion = Resources.GetModBlueprint<BlueprintFeature>("NaturesWhispersACConversion");
                    var ScaledFistACBonus = Resources.GetBlueprint<BlueprintFeature>("3929bfd1beeeed243970c9fc0cf333f8");

                    OracleRevelationNatureWhispers.RemoveComponents<ReplaceStatBaseAttribute>();
                    OracleRevelationNatureWhispers.RemoveComponents<ReplaceCMDDexterityStat>();
                    OracleRevelationNatureWhispers.AddComponent<HasFactFeatureUnlock>(c => {
                        c.m_CheckedFact = ScaledFistACBonus.ToReference<BlueprintUnitFactReference>();
                        c.m_Feature = NaturesWhispersACConversion.ToReference<BlueprintUnitFactReference>();
                        c.Not = true;
                    });
                    Main.LogPatch("Patched", OracleRevelationNatureWhispers);
                }

                void PatchFlameMystery() {
                    PatchRevelationBurningMagic();

                    void PatchRevelationBurningMagic() {
                        if (ModSettings.Fixes.Oracle.Base.IsDisabled("RevelationBurningMagic")) { return; }

                        var OracleRevelationBurningMagicBuff = Resources.GetBlueprint<BlueprintBuff>("4ae27ae7c3d758041b25e9a3aff73592");
                        OracleRevelationBurningMagicBuff.GetComponent<ContextRankConfig>().m_Progression = ContextRankProgression.AsIs;
                        Main.LogPatch("Patched", OracleRevelationBurningMagicBuff);
                    }
                }
            }
            static void PatchArchetypes() {
            }
        }
    }
}
