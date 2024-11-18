using HarmonyLib;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance;
using static TabletopTweaks.Base.Main;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Base.Bugfixes.General {
    class BuffEnchantAnyWeaponFix {
        [HarmonyPatch(typeof(BlueprintFact), nameof(BlueprintFact.CollectComponents))]
        static class BlueprintFact_CollectComponents_Patch {
            static void Postfix(ref List<BlueprintComponent> __result) {
                if (Main.TTTContext.Fixes.BaseFixes.IsDisabled("InfiniteStackingWeaponEffects")) { return; }

                for (int i = 0; i < __result.Count; i++) {
                    BlueprintComponent component = __result[i];
                    if (component is BuffEnchantAnyWeapon enchantComponent) {
                        BuffEnchantAnyWeaponTTT replacementComponent = Helpers.Create<BuffEnchantAnyWeaponTTT>(c => {
                            c.name = enchantComponent.name;
                            c.m_EnchantmentBlueprint = enchantComponent.m_EnchantmentBlueprint;
                            c.Slot = enchantComponent.Slot;
                        });
                        // https://c.tenor.com/eqLNYv0A9TQAAAAC/swap-indiana-jones.gif
                        __result[i] = replacementComponent;
                        TTTContext.Logger.LogVerbose("Replaced " + enchantComponent.GetType().ToString() + " with " + replacementComponent.GetType().ToString());
                    }
                }
            }
        }
    }
}
