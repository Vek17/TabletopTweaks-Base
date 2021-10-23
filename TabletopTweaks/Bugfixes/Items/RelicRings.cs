using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;

namespace TabletopTweaks.Bugfixes.Items {
    static class RelicRings {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;

                if (ModSettings.Fixes.Items.Weapons.IsDisabled("GhostlyPathwaysRings")) { return; }

                Main.LogHeader("Patching Relic Rings");

                var BrokenSoulAbility = Resources.GetBlueprint<BlueprintAbility>("79858b03ea88801449c95ff9ed813333");

                var SpiderRing = Resources.GetBlueprint<BlueprintItemEquipmentRing>("b2374fa6162e1f54c83b3f02af487a76");
                if (SpiderRing.Ability != null && SpiderRing.Ability.ToReference<BlueprintAbilityReference>().Equals(BrokenSoulAbility.ToReference<BlueprintAbilityReference>()))
                    SpiderRing.m_Ability = null;
                Main.LogPatch("Patched", SpiderRing);
                var PlanarRing = Resources.GetBlueprint<BlueprintItemEquipmentRing>("031e6d000ff5b4849b84f15707a625ec");
                if (PlanarRing.Ability != null && SpiderRing.Ability.ToReference<BlueprintAbilityReference>().Equals(BrokenSoulAbility.ToReference<BlueprintAbilityReference>()))
                    PlanarRing.m_Ability = null;
                Main.LogPatch("Patched", PlanarRing);
            }
        }
    }
}
