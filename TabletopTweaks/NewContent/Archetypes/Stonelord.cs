using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using System;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;

namespace TabletopTweaks.NewContent.Archetypes {
    internal class Stonelord {

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

                static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                Main.LogHeader("Patching Stonelord");
                PatchStonelordArchetype();

                static void PatchStonelordArchetype() {
                    BlueprintArchetype StonelordArchetype = Resources.GetBlueprint<BlueprintArchetype>("cf0f99b3cd7444a48681b1c1c4aa2a41");
                    StonelordArchetype.RemoveComponents<PrerequisiteFeature>();

                    Main.LogPatch("Patched", StonelordArchetype);
                }

            }
        }

        internal static void PatchStonelordArchetype() {
            throw new NotImplementedException();
        }
    }
}