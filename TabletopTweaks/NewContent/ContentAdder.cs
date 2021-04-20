using HarmonyLib;
using Kingmaker.Blueprints;

namespace TabletopTweaks.NewContent {
    class ContentAdder {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Loading New Content");
                Features.BloodlineRequisiteFeature.AddBloodlineRequisiteFeature();
                Features.PrimalistRagePowerSelection.AddPrimalistRagePowerSelection();
                Bloodlines.AberrantBloodline.AddBloodragerAberrantBloodline();
                Bloodlines.AberrantBloodline.AddSorcererAberrantBloodline();
                Archetypes.CauldronWitch.AddCauldrenWitch();
                Archetypes.ElementalMaster.AddElementalMaster();
            }
        }
    }
}
