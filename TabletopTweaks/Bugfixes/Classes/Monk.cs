using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Config;
using static TabletopTweaks.MechanicsChanges.AdditionalModifierDescriptors;

namespace TabletopTweaks.Bugfixes.Classes {
    class Monk {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Monk.DisableAllFixes) { return; }
                Main.LogHeader("Patching Monk");
                PatchBase();
                PatchScaledFist();
            }
            static void PatchBase() {
                if (ModSettings.Fixes.Monk.Base.DisableAllFixes) { return; }
                PatchMonkACBonus();

                void PatchMonkACBonus() {
                    if (!ModSettings.Fixes.Monk.Base.Fixes["MonkACBonus"]) { return; }
                    var MonkACBonus = Resources.GetBlueprint<BlueprintFeature>("e241bdfd6333b9843a7bfd674d607ac4");
                    MonkACBonus.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Value.ValueRank == AbilityRankType.DamageDice)
                        .First().Descriptor = (ModifierDescriptor)Untyped.Wisdom;
                    Main.LogPatch("Patched", MonkACBonus);
                }
            }
            static void PatchScaledFist() {
                if (ModSettings.Fixes.Monk.Archetypes["ScaledFist"].DisableAllFixes) { return; }
                PatchMonkACBonus();

                void PatchMonkACBonus() {
                    if (!ModSettings.Fixes.Monk.Archetypes["ScaledFist"].Fixes["ScaledFistACBonus"]) { return; }
                    var ScaledFistACBonus = Resources.GetBlueprint<BlueprintFeature>("3929bfd1beeeed243970c9fc0cf333f8");
                    ScaledFistACBonus.GetComponents<AddContextStatBonus>()
                        .Where(c => c.Value.ValueRank == AbilityRankType.DamageDice)
                        .First().Descriptor = (ModifierDescriptor)Untyped.Charisma;
                    Main.LogPatch("Patched", ScaledFistACBonus);
                }
            }
        }
    }
}
