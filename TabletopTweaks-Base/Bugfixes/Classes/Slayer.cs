using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Classes {
    class Slayer {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Slayer");

                PatchBaseClass();
            }
            static void PatchBaseClass() {
                PatchSlayerTrapfinding();

                void PatchSlayerTrapfinding() {
                    if (TTTContext.Fixes.Slayer.Base.IsDisabled("Trapfinding")) { return; }
                    var SlayerTrapfinding = BlueprintTools.GetBlueprint<BlueprintFeature>("e3c12938c2f93544da89824fbe0933a5");
                    SlayerTrapfinding.AddComponent(Helpers.Create<AddContextStatBonus>(c => {
                        c.Stat = StatType.SkillThievery;
                        c.Multiplier = 1;
                        c.Value = new ContextValue() {
                            ValueType = ContextValueType.Rank
                        };
                    }));
                    TTTContext.Logger.LogPatch("Patched", SlayerTrapfinding);
                }
            }
        }
    }
}
