using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace TabletopTweaks.Bugfixes.Classes {
    class Barbarian {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Barbarian");

                PatchBase();
                PatchWreckingBlows();
                PatchCripplingBlows();
            }
            static void PatchBase() {
            }

            static void PatchWreckingBlows() {
                var WreckingBlowsFeature = Resources.GetBlueprint<BlueprintFeature>("5bccc86dd1f187a4a99f092dc054c755");
                var PowerfulStanceEffectBuff = Resources.GetBlueprint<BlueprintBuff>("aabad91034e5c7943986fe3e83bfc78e");
                WreckingBlowsFeature.GetComponent<BuffExtraEffects>().m_CheckedBuff = PowerfulStanceEffectBuff.ToReference<BlueprintBuffReference>();
                Main.LogPatch("Patched", WreckingBlowsFeature);
            }

            static void PatchCripplingBlows() {
                var CripplingBlowsFeature = Resources.GetBlueprint<BlueprintFeature>("0eec6efbb7f66e148817c9f51b804f08");
                var PowerfulStanceEffectBuff = Resources.GetBlueprint<BlueprintBuff>("aabad91034e5c7943986fe3e83bfc78e");
                CripplingBlowsFeature.GetComponent<BuffExtraEffects>().m_CheckedBuff = PowerfulStanceEffectBuff.ToReference<BlueprintBuffReference>();
                Main.LogPatch("Patched", CripplingBlowsFeature);
            }
        }
    }
}
