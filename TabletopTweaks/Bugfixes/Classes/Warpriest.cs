using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Classes {
    class Warpriest {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Warpriest");
                PatchBase();
            }
            static void PatchBase() {
                PatchLuckBlessing();

                void PatchLuckBlessing() {
                    if (ModSettings.Fixes.Warpriest.Base.IsDisabled("LuckBlessing")) { return; }

                    var LuckBlessingMajorFeature = Resources.GetBlueprint<BlueprintFeature>("0b59acd4d1fffa34da9fc91da05dd398");
                    var LuckBlessingMajorAbility = Resources.GetBlueprintReference<BlueprintUnitFactReference>("49fa2b54589c34a42b8f06b8de1a6639");
                    LuckBlessingMajorFeature.GetComponent<AddFacts>().m_Facts = new BlueprintUnitFactReference[] { LuckBlessingMajorAbility };
                    Main.LogPatch("Patched", LuckBlessingMajorFeature);
                }
            }

            static void PatchArchetypes() {
            }
        }
    }
}
