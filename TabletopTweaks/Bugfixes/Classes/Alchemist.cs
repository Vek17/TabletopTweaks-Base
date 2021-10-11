using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Alchemist {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                Main.LogHeader("Patching Alchemist Resources");

                PatchGrenadier();
            }

            static void PatchGrenadier() {
                PatchBrewPotions();
                PatchPoisonResistance();

                void PatchBrewPotions() {
                    if (ModSettings.Fixes.Alchemist.Archetypes["Grenadier"].IsDisabled("BrewPotions")) { return; }

                    var GrenadierArchetype = Resources.GetBlueprint<BlueprintArchetype>("6af888a7800b3e949a40f558ff204aae");
                    var BrewPotions = Resources.GetBlueprint<BlueprintFeature>("c0f8c4e513eb493408b8070a1de93fc0");

                    GrenadierArchetype.RemoveFeatures = GrenadierArchetype.RemoveFeatures.AppendToArray(new LevelEntry() {
                        Level = 1,
                        m_Features = new List<BlueprintFeatureBaseReference>() {
                            BrewPotions.ToReference<BlueprintFeatureBaseReference>()
                        }
                    }); ;
                    Main.LogPatch("Patched", GrenadierArchetype);
                }

                void PatchPoisonResistance() {
                    if (ModSettings.Fixes.Alchemist.Archetypes["Grenadier"].IsDisabled("PoisonResistance")) { return; }

                    var GrenadierArchetype = Resources.GetBlueprint<BlueprintArchetype>("6af888a7800b3e949a40f558ff204aae");
                    var PoisonResistance = Resources.GetBlueprint<BlueprintFeature>("c9022272c87bd66429176ce5c597989c");
                    var PoisonImmunity = Resources.GetBlueprint<BlueprintFeature>("202af59b918143a4ab7c33d72c8eb6d5");

                    GrenadierArchetype.RemoveFeatures = GrenadierArchetype.RemoveFeatures
                        .Where(entry => !new int[] { 2, 5, 8, 10 }.Contains(entry.Level))
                        .Concat(new LevelEntry[] {
                            Helpers.CreateLevelEntry(2, PoisonResistance),
                            Helpers.CreateLevelEntry(5, PoisonResistance),
                            Helpers.CreateLevelEntry(8, PoisonResistance),
                            Helpers.CreateLevelEntry(10, PoisonImmunity)
                        }).ToArray();
                    Main.LogPatch("Patched", GrenadierArchetype);
                }
            }
        }
    }
}
