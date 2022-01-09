using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.OwlcatReplacements;

namespace TabletopTweaks.Utilities {
    static class QuickFixTools {
        public static void ReplaceSuppression(BlueprintBuff buff) {
            var suppressBuffComponent = buff.GetComponent<SuppressBuffs>();
            if (suppressBuffComponent == null) { return; }
            buff.RemoveComponents<SuppressBuffs>();
            buff.AddComponent<SuppressBuffsTTT>(c => {
                c.m_Buffs = suppressBuffComponent.m_Buffs;
                c.Descriptor = suppressBuffComponent.Descriptor;
                c.Schools = suppressBuffComponent.Schools;
            });

            Main.LogPatch("Patched", buff);
        }
    }
}
