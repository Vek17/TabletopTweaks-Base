using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Classes {
    class Sorcerer {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Sorcerer");

                PatchBase();
                PatchCrossblooded();
            }

            static void PatchBase() {
            }

            static void PatchCrossblooded() {
                PatchDrawbacks();

                void PatchDrawbacks() {
                    if (ModSettings.Fixes.Sorcerer.Archetypes["Crossblooded"].IsDisabled("Drawbacks")) { return; }

                    var CrossbloodedDrawbacks = Resources.GetBlueprint<BlueprintFeature>("f02fd748fecb4cc2a4d7d282c6b3de46");
                    CrossbloodedDrawbacks.SetName("Crossblooded Drawbacks");
                    CrossbloodedDrawbacks.SetDescription("A crossblooded sorcerer has one fewer spell known at each level than regular sorcerer.\n" +
                        "Furthermore, the conflicting urges created by the divergent nature of the crossblooded sorcerer’s dual heritage forces " +
                        "her to constantly take some mental effort just to remain focused on her current situation and needs. This leaves her " +
                        "with less mental resolve to deal with external threats. A crossblooded sorcerer always takes a -2 penalty on Will saves.");
                    CrossbloodedDrawbacks.AddComponent<AddStatBonus>(c => {
                        c.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveWill;
                        c.Value = -2;
                    });

                    Main.LogPatch("Patched", CrossbloodedDrawbacks);
                }
            }
        }
    }
}
