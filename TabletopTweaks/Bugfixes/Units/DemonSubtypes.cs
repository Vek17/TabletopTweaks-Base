using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Units {
    static class DemonSubtypes {

        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Demon Subtypes");
                patchDemonSubtypes();
                Main.LogHeader("Demon Subtype Patch Complete");
            }
        }

        public static void patchDemonSubtypes() {
            if (!ModSettings.Fixes.FixDemonSubtypes) { return; }
            BlueprintFeature subtypeDemon = Resources.GetBlueprint<BlueprintFeature>("dc960a234d365cb4f905bdc5937e623a");
            BlueprintFeature subtypeEvil = Resources.GetBlueprint<BlueprintFeature>("5279fc8380dd9ba419b4471018ffadd1");
            BlueprintFeature subtypeChaotic = Resources.GetBlueprint<BlueprintFeature>("1dd712e7f147ab84bad6ffccd21a878d");

            var addFacts = subtypeDemon.GetComponent<AddFacts>();
            addFacts.m_Facts = addFacts.m_Facts.AddToArray(subtypeEvil.ToReference<BlueprintUnitFactReference>());
            addFacts.m_Facts = addFacts.m_Facts.AddToArray(subtypeChaotic.ToReference<BlueprintUnitFactReference>());
            Main.LogPatch("Patched", subtypeDemon);
        }
    }
}
