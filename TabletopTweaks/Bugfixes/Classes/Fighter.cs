using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Classes {
    class Fighter {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (ModSettings.Fixes.Rogue.DisableAll) { return; }
                Main.LogHeader("Patching Fighter");
                PatchBase();
                PatchTwoHandedFighter();
            }
            static void PatchBase() {
                if (ModSettings.Fixes.Fighter.Base.DisableAll) { return; }
                PatchAdvancedWeaponTraining();
                void PatchAdvancedWeaponTraining() {
                    if (!ModSettings.Fixes.Fighter.Base.Enabled["AdvancedWeaponTraining"]) { return; }
                    var WeaponTrainingSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");
                    var AdvancedWeaponTraining1 = Resources.GetBlueprint<BlueprintFeatureSelection>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
                    var AdvancedWeaponTraining2 = Resources.GetBlueprint<BlueprintFeatureSelection>("70a139f0a4c6c534eaa34feea0d08622");
                    var AdvancedWeaponTraining3 = Resources.GetBlueprint<BlueprintFeatureSelection>("ee9ab0117ca06b84f9c66469f4428c61");
                    var AdvancedWeaponTraining4 = Resources.GetBlueprint<BlueprintFeatureSelection>("0b55d725ded1ae549bb858fba1d84114");
                    var AdvancedWeapontrainingSelection = Resources.GetBlueprint<BlueprintFeatureSelection>(ModSettings.Blueprints.NewBlueprints["AdvancedWeaponTrainingSelection"]);

                    WeaponTrainingSelection.m_AllFeatures = WeaponTrainingSelection.m_AllFeatures.Where(feature => !AdvancedWeapontrainingSelection.m_AllFeatures.Contains(feature)).ToArray();
                    WeaponTrainingSelection.m_AllFeatures = WeaponTrainingSelection.m_AllFeatures.AppendToArray(AdvancedWeapontrainingSelection.ToReference<BlueprintFeatureReference>());
                    Main.LogPatch("Patched", WeaponTrainingSelection);
                    AdvancedWeapontrainingSelection.m_AllFeatures.ForEach(feature => {
                        feature.Get().RemoveComponents<PrerequisiteClassLevel>();
                        Main.LogPatch("Patched", feature.Get());
                    });
                    AdvancedWeaponTraining1.IgnorePrerequisites = false;
                    AdvancedWeaponTraining2.IgnorePrerequisites = false;
                    AdvancedWeaponTraining3.IgnorePrerequisites = false;
                    AdvancedWeaponTraining4.IgnorePrerequisites = false;
                    Main.LogPatch("Patched", AdvancedWeaponTraining1);
                    Main.LogPatch("Patched", AdvancedWeaponTraining2);
                    Main.LogPatch("Patched", AdvancedWeaponTraining3);
                    Main.LogPatch("Patched", AdvancedWeaponTraining4);
                }
            }
            static void PatchTwoHandedFighter() {
                if (ModSettings.Fixes.Fighter.Archetypes["TwoHandedFighter"].DisableAll) { return; }
                PatchAdvancedWeaponTraining();

                void PatchAdvancedWeaponTraining() {
                    if (!ModSettings.Fixes.Fighter.Archetypes["TwoHandedFighter"].Enabled["AdvancedWeaponTraining"]) { return; }

                    var TwoHandedFighterWeaponTraining = Resources.GetBlueprint<BlueprintFeature>("88da2a5dfc505054f933bb81014e864f");
                    var WeaponTrainingSelection = Resources.GetBlueprint<BlueprintFeature>("b8cecf4e5e464ad41b79d5b42b76b399");

                    var AdvancedWeaponTraining1 = Resources.GetBlueprint<BlueprintFeature>("3aa4cbdd4af5ba54888b0dc7f07f80c4");
                    var AdvancedWeaponTraining2 = Resources.GetBlueprint<BlueprintFeature>("70a139f0a4c6c534eaa34feea0d08622");
                    var AdvancedWeaponTraining3 = Resources.GetBlueprint<BlueprintFeature>("ee9ab0117ca06b84f9c66469f4428c61");
                    var AdvancedWeaponTraining4 = Resources.GetBlueprint<BlueprintFeature>("0b55d725ded1ae549bb858fba1d84114");

                    PatchPrerequisites(AdvancedWeaponTraining1);
                    PatchPrerequisites(AdvancedWeaponTraining2);
                    PatchPrerequisites(AdvancedWeaponTraining3);
                    PatchPrerequisites(AdvancedWeaponTraining4);

                    void PatchPrerequisites(BlueprintFeature AdvancedWeaponTraining) {
                        AdvancedWeaponTraining.GetComponent<PrerequisiteFeature>().Group = Prerequisite.GroupType.Any;
                        AdvancedWeaponTraining.AddComponent(Helpers.Create<PrerequisiteFeature>(c => {
                            c.m_Feature = TwoHandedFighterWeaponTraining.ToReference<BlueprintFeatureReference>();
                            c.Group = Prerequisite.GroupType.Any;
                        }));
                        Main.LogPatch("Patched", AdvancedWeaponTraining);
                    }
                }
            }
        }
    }
}
