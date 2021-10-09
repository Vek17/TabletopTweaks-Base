using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
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

                }
            }
            static void PatchArchetypes() {
            }
        }
    }
}
