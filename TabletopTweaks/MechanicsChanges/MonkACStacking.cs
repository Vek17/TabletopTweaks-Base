using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Config;
using static TabletopTweaks.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.MechanicsChanges {
    class MonkACStacking {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (!ModSettings.Fixes.DisableMonkACStacking) { return; }
                Main.LogHeader("Patching Monk AC Effects");

                var CunningElusionFeature = Resources.GetBlueprint<BlueprintFeature>("a71103ce28964f39b38442baa32a3031");
                var MonkACBonus = Resources.GetBlueprint<BlueprintFeature>("e241bdfd6333b9843a7bfd674d607ac4");
                var ScaledFistACBonus = Resources.GetBlueprint<BlueprintFeature>("3929bfd1beeeed243970c9fc0cf333f8");

                CunningElusionFeature.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Value.ValueRank == AbilityRankType.DamageDice)
                        .First().Descriptor = (ModifierDescriptor)Untyped.Wisdom;
                Main.LogPatch("Patched", MonkACBonus);
                MonkACBonus.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Value.ValueRank == AbilityRankType.DamageDice)
                        .First().Descriptor = (ModifierDescriptor)Untyped.Wisdom;
                Main.LogPatch("Patched", MonkACBonus);
                ScaledFistACBonus.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Value.ValueRank == AbilityRankType.DamageDice)
                        .First().Descriptor = (ModifierDescriptor)Untyped.Charisma;
                Main.LogPatch("Patched", MonkACBonus);
            }
        }
    }
}
