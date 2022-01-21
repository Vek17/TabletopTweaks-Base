using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;

namespace TabletopTweaks.Bugfixes.Classes {
    static class Cleric {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Cleric");

                PatchBaseClass();
            }
            static void PatchBaseClass() {
                PatchGloryDomain();

                void PatchGloryDomain() {
                    if (ModSettings.Fixes.Cleric.Base.IsDisabled("GloryDomain")) { return; }

                    var GloryDomainBaseBuff = Resources.GetBlueprint<BlueprintBuff>("55edcfff497a1e04a963f72c485da5cb");
                    GloryDomainBaseBuff.RemoveComponents<AddContextStatBonus>(component => component.Stat == StatType.Charisma);
                    Main.LogPatch("Patched", GloryDomainBaseBuff);
                }
            }
        }
    }
}
