using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;

namespace TabletopTweaks.NewContent {
    class ContentAdder {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Loading New Content");
                Features.AeonBaneIncreaseResourceFeature.AddAeonBaneIncreaseResourceFeature();
                Features.BloodlineRequisiteFeature.AddBloodlineRequisiteFeature();
                Features.InstinctualWarriorACBonusUnlock.AddInstinctualWarriorACBonusUnlock();
                Features.CavalierMounts.AddCavalierMountFeatureWolf();
                Features.PrimalistRagePowerSelection.AddPrimalistRagePowerSelection();
                Bloodlines.AberrantBloodline.AddBloodragerAberrantBloodline();
                Bloodlines.AberrantBloodline.AddSorcererAberrantBloodline();
                Bloodlines.DestinedBloodline.AddBloodragerDestinedBloodline();
                Bloodlines.DestinedBloodline.AddSorcererDestinedBloodline();
                Bloodlines.AbyssalBloodline.AddBloodragerAbyssalDemonicBulkEnlargeBuff();
                Spells.LongArms.AddLongArms();
                Archetypes.CauldronWitch.AddCauldrenWitch();
                Archetypes.ElementalMaster.AddElementalMaster();
            }
        }
    }
}
