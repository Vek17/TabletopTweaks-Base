using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Slayer {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Slayer");

                PatchBaseClass();
            }
            static void PatchBaseClass() {
                PatchSlayerTrapfinding();

                void PatchSlayerTrapfinding() {
                    if (ModSettings.Fixes.Slayer.Base.IsDisabled("Trapfinding")) { return; }
                    var SlayerTrapfinding = Resources.GetBlueprint<BlueprintFeature>("e3c12938c2f93544da89824fbe0933a5");
                    SlayerTrapfinding.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                        c.Stat = StatType.SkillThievery;
                        c.Multiplier = 1;
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    }));
                    Main.LogPatch("Patched", SlayerTrapfinding);
                }
            }
        }
    }
}
