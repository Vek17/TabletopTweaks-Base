using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabletopTweaks.Bugfixes.General {
    static class HeightenSpellFix {

        [HarmonyPatch(typeof(MetamagicBuilder), "SetHeightenLevel")]
        class MetamagicBuilder_SetHeightenLevel_Patch {
            static bool Prefix(MetamagicBuilder __instance, int level) {
                if (ModSettings.Fixes.BaseFixes.IsDisabled("HeightenMetamagicMaxSpellLevel")) { return true; }

                var apply = level >= 0;

                // original has off-by-one (e.g. can't heighten a base-level 8+ spell)
                // it also ignores other metamagic (e.g. completely normal spell)
                apply = apply && (__instance.ResultSpellLevel - __instance.HeightenLevel) + level <= __instance.Spellbook.MaxSpellLevel;

                apply = apply && __instance.SpellMetamagicFeatures.Any((Feature feature) => feature.GetComponent<AddMetamagicFeat>().Metamagic == Metamagic.Heighten);
                apply = apply && __instance.AppliedMetamagicFeatures.Any((Feature feature) => feature.GetComponent<AddMetamagicFeat>().Metamagic == Metamagic.Heighten);
                apply = apply && __instance.AppliedMetamagics.Any((Metamagic metamagic) => (metamagic & Metamagic.Heighten) != 0);

                if (apply) {
                    __instance.HeightenLevel = level;
                    __instance.Apply();
                }

                return false;
            }
        }
    }
}
