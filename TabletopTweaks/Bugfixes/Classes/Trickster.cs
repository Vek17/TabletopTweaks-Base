using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using System.Linq;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Trickster {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Trickster.DisableAll) { return; }
                Main.LogHeader("Patching Trickster Resources");
                PatchTricksterTricks();
            }
            static void PatchTricksterTricks() {
                if (!ModSettings.Fixes.Trickster.Enabled["TricksterTricks"]) { return; }
                var TricksterTrickeryTier3Feature = Resources.GetBlueprint<BlueprintFeature>("64131f0ac1e2497a806856461bdcfe4e");
                var TricksterStealthTier3Feature = Resources.GetBlueprint<BlueprintFeature>("74d23b6698554e36974711eacc386290");
                var TricksterPersuasionTier3Feature = Resources.GetBlueprint<BlueprintFeature>("9f677bc314b84cc388044c3786148fd9");
                var TricksterMobilityTier3Feature = Resources.GetBlueprint<BlueprintFeature>("6db3651d9af54f28b5a3a5570f49f349");
                var TricksterKnowledgeArcanaTier3Feature = Resources.GetBlueprint<BlueprintFeature>("5e26c673173e423881e318d2f0ae84f0");
                var TricksterAthleticsTier3Feature = Resources.GetBlueprint<BlueprintFeature>("e45bf795c4f84c3b8a83c011f8580491");
                var TricksterLoreReligionTier3Parametrized = Resources.GetBlueprint<BlueprintFeature>("0466cdfa56f943608760952a6bf2a6fa");
                var TricksterLoreNature3Feature = Resources.GetBlueprint<BlueprintFeature>("b88ca3a5476ebcc4ea63d5c1e92ce222");
                var TricksterKnowledgeWorldTier3Feature = Resources.GetBlueprint<BlueprintFeature>("26691ec239c84568bd27b055a1528912");
                var TricksterRank3Selection = Resources.GetBlueprint<BlueprintFeatureSelection>("446f4a8b32019f5478a8dfeddac74710");

                TricksterRank3Selection.m_AllFeatures = TricksterRank3Selection.m_AllFeatures.Concat(new BlueprintFeatureReference[] {
                    TricksterTrickeryTier3Feature.ToReference<BlueprintFeatureReference>(),
                    TricksterStealthTier3Feature.ToReference<BlueprintFeatureReference>(),
                    TricksterPersuasionTier3Feature.ToReference<BlueprintFeatureReference>(),
                    TricksterMobilityTier3Feature.ToReference<BlueprintFeatureReference>(),
                    TricksterKnowledgeArcanaTier3Feature.ToReference<BlueprintFeatureReference>(),
                    TricksterAthleticsTier3Feature.ToReference<BlueprintFeatureReference>(),
                    TricksterLoreReligionTier3Parametrized.ToReference<BlueprintFeatureReference>()
                }).ToArray();
                Main.LogPatch("Patched", TricksterRank3Selection);
            }
        }
    }
}
