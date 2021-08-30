using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
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
                PatchSlayerStudiedTarget();
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
                void PatchSlayerStudiedTarget() {
                    if (ModSettings.Fixes.Slayer.Base.IsDisabled("StudiedTarget")) { return; }
                    BlueprintBuff SlayerStudiedTargetBuff = Resources.GetBlueprint<BlueprintBuff>("45548967b714e254aa83f23354f174b0");
                    SlayerStudiedTargetBuff.GetComponent<ContextRankConfig>().m_Progression = ContextRankProgression.OnePlusDivStep;
                    Main.LogPatch("Patched", SlayerStudiedTargetBuff);
                }
            }
        }
    }
}
