using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;

namespace TabletopTweaks {
    static class SaveGameFix {
        static private List<Action<UnitEntityData>> save_game_actions = new List<Action<UnitEntityData>>();

        static public void AddUnitPatch(Action<UnitEntityData> patch) {
            save_game_actions.Add(patch);
        }

        [HarmonyPatch(typeof(UnitEntityData), "OnAreaDidLoad")]
        class UnitDescriptor__PostLoad__Patch {
            static void Postfix(UnitEntityData __instance) {
                foreach (var action in SaveGameFix.save_game_actions) {
                    action(__instance);
                }
            }
        }
    }
}
