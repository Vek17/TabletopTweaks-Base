using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using System.Linq;
using UnityEngine;

namespace TabletopTweaks.Bugfixes.Features {
    class MythicAbilities {
        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;
            static bool Prefix() {
                if (Initialized) {
                    // When wrath first loads into the main menu InitializeLibrary is called by Kingmaker.GameStarter.
                    // When loading into maps, Kingmaker.Runner.Start will call InitializeLibrary which will
                    // clear the ResourcesLibrary.s_LoadedBlueprints cache which causes loaded blueprints to be garbage collected.
                    // Return false here to prevent ResourcesLibrary.InitializeLibrary from being called twice 
                    // to prevent blueprints from being garbage collected.
                    return false;
                }
                else {
                    return true;
                }
            }
            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Resources.Settings.MythicAbilities.DisableAllFixes) { return; }
                Main.LogHeader("Patching Mythic Ability Resources");
                patchBloodlineAscendance();
                patchSecondBloodline();
                Main.LogHeader("Patching Mythic Ability Resources Complete");
                //Do Stuff
            }
            static void patchBloodlineAscendance() {
                if (!Resources.Settings.MythicAbilities.Fixes["BloodlineAscendance"]) { return; }
                BlueprintFeatureSelection BloodlineAscendance = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("ce85aee1726900641ab53ede61ac5c19");
                PrerequisiteFeaturesFromList newPrerequisites = ScriptableObject.CreateInstance<PrerequisiteFeaturesFromList>();

                newPrerequisites.m_Features = new BlueprintFeatureReference[] {
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("24bef8d1bee12274686f6da6ccbc8914").ToReference<BlueprintFeatureReference>(),    // SorcererBloodlineSelection
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7bda7cdb0ccda664c9eb8978cf512dbc").ToReference<BlueprintFeatureReference>(),    // SeekerBloodlineSelection
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("a46d4bd93601427409d034a997673ece").ToReference<BlueprintFeatureReference>(),    // SylvanBloodlineProgression
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7d990675841a7354c957689a6707c6c2").ToReference<BlueprintFeatureReference>(),    // SageBloodlineProgression
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8a95d80a3162d274896d50c2f18bb6b1").ToReference<BlueprintFeatureReference>(),    // EmpyrealBloodlineProgression
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("da48f9d7f697ae44ca891bfc50727988").ToReference<BlueprintFeatureReference>()     // BloodOfDragonsSelection - Dragon Disciple
                };
                newPrerequisites.Amount = 1;
                BloodlineAscendance.ComponentsArray = BloodlineAscendance.ComponentsArray
                    .Where(c => c.GetType() != typeof(PrerequisiteFeature))
                    .Append(newPrerequisites)
                    .ToArray();
                Main.LogPatch("Patched", BloodlineAscendance);
            }
            static void patchSecondBloodline() {
                if (!Resources.Settings.MythicAbilities.Fixes["SecondBloodline"]) { return; }
                BlueprintFeatureSelection SecondBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("3cf2ab2c320b73347a7c21cf0d0995bd");
                PrerequisiteFeaturesFromList NewPrerequisites = ScriptableObject.CreateInstance<PrerequisiteFeaturesFromList>();

                NewPrerequisites.m_Features = new BlueprintFeatureReference[] {
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("24bef8d1bee12274686f6da6ccbc8914").ToReference<BlueprintFeatureReference>(),    // SorcererBloodlineSelection
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7bda7cdb0ccda664c9eb8978cf512dbc").ToReference<BlueprintFeatureReference>(),    // SeekerBloodlineSelection
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("a46d4bd93601427409d034a997673ece").ToReference<BlueprintFeatureReference>(),    // SylvanBloodlineProgression
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7d990675841a7354c957689a6707c6c2").ToReference<BlueprintFeatureReference>(),    // SageBloodlineProgression
                    ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8a95d80a3162d274896d50c2f18bb6b1").ToReference<BlueprintFeatureReference>()     // EmpyrealBloodlineProgression
                };
                NewPrerequisites.Amount = 1;
                SecondBloodline.ComponentsArray = SecondBloodline.ComponentsArray
                    .Where(c => c.GetType() != typeof(PrerequisiteFeature))
                    .Append(NewPrerequisites)
                    .ToArray();
                SecondBloodline.IgnorePrerequisites = false;
                Main.LogPatch("Patched", SecondBloodline);
            }
        }
    }
}
