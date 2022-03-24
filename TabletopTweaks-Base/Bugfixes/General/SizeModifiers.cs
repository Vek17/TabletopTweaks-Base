using HarmonyLib;
using Kingmaker.EntitySystem.Entities;

namespace TabletopTweaks.Base.Bugfixes.General {
    static class SizeModifiers {
        [HarmonyPatch(typeof(UnitEntityData), "OnAreaDidLoad")]
        class UnitDescriptor_FixSizeModifiers_Patch {
            static void Postfix(UnitEntityData __instance) {
                __instance?.Descriptor.UpdateSizeModifiers();
            }
        }
    }
}
