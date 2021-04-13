using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Features {
    class Bloodlines {

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
                if (Resources.Fixes.Bloodlines.DisableAllFixes) { return; }
                Main.LogHeader("Patching Bloodline Resources");
                patchBloodlineRestrictions();
                Main.LogHeader("Patching Bloodline Complete");
                //Do Stuff
            }
        }
        static void patchBloodlineRestrictions() {
            if (!Resources.Fixes.Bloodlines.Fixes["BloodlineRestrictions"]) { return; }
            // Sorceror Bloodlines
            BlueprintFeature BloodlineAbyssal = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("d3a4cb7be97a6694290f0dcfbd147113");
            BlueprintFeature BloodlineArcane = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("4d491cf9631f7e9429444f4aed629791");
            BlueprintFeature BloodlineCelestial = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("aa79c65fa0e11464d9d100b038c50796");
            BlueprintFeature BloodlineDraconicBlack = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7bd143ead2d6c3a409aad6ee22effe34");
            BlueprintFeature BloodlineDraconicBlue = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8a7f100c02d0b254d8f5f3affc8ef386");
            BlueprintFeature BloodlineDraconicBrass = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("5f9ecbee67db8364985e9d0500eb25f1");
            BlueprintFeature BloodlineDraconicBronze = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7e0f57d8d00464441974e303b84238ac");
            BlueprintFeature BloodlineDraconicCopper = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("b522759a265897b4f8f7a1a180a692e4");
            BlueprintFeature BloodlineDraconicGold = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("6c67ef823db8d7d45bb0ef82f959743d");
            BlueprintFeature BloodlineDraconicGreen = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7181be57d1cc3bc40bc4b552e4e4ce24");
            BlueprintFeature BloodlineDraconicRed = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8c6e5b3cf12f71e43949f52c41ae70a8");
            BlueprintFeature BloodlineDraconicSilver = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("c7d2f393e6574874bb3fc728a69cc73a");
            BlueprintFeature BloodlineDraconicWhite = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("b0f79497a0d1f4f4b8293e82c8f8fa0c");
            BlueprintFeature BloodlineElementalAir = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("cd788df497c6f10439c7025e87864ee4");
            BlueprintFeature BloodlineElementalEarth = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("32393034410fb2f4d9c8beaa5c8c8ab7");
            BlueprintFeature BloodlineElementalFire = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("17cc794d47408bc4986c55265475c06f");
            BlueprintFeature BloodlineElementalWater = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7c692e90592257a4e901d12ae6ec1e41");
            BlueprintFeature BloodlineFey = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("e8445256abbdc45488c2d90373f7dae8");
            BlueprintFeature BloodlineInfernal = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("e76a774cacfb092498177e6ca706064d");
            BlueprintFeature BloodlineSerpentine = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("739c1e842bf77994baf963f4ad964379");
            BlueprintFeature BloodlineUndead = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("a1a8bf61cadaa4143b2d4966f2d1142e");
            //Seeker Bloodlines
            BlueprintFeature SeekerBloodlineAbyssal = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("17b752be1e0f4a34e8914df52eebeb75");
            BlueprintFeature SeekerBloodlineArcane = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("562c5e4031d268244a39e01cc4b834bb");
            BlueprintFeature SeekerBloodlineCelestial = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("17ac9d771a944194a92ac15b5ff861c9");
            BlueprintFeature SeekerBloodlineDraconicBlack = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("cf448fafcd8452d4b830bcc9ca074189");
            BlueprintFeature SeekerBloodlineDraconicBlue = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("82f76646a7f96ed4cafa18480adc0b8c");
            BlueprintFeature SeekerBloodlineDraconicBrass = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("c355b8777a3bda7429d863367bda3851");
            BlueprintFeature SeekerBloodlineDraconicBronze = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("468ecbdd58fbd6045a0a1888308031fe");
            BlueprintFeature SeekerBloodlineDraconicCopper = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("2ad98f60b29ae604da0297037054080c");
            BlueprintFeature SeekerBloodlineDraconicGold = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("63c9d62a56e6921409a58de1ab9a9f9b");
            BlueprintFeature SeekerBloodlineDraconicGreen = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("6de526eeee72852448c5595f7a44a39d");
            BlueprintFeature SeekerBloodlineDraconicRed = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("d69a7785de1959c4497e4ff1e9490509");
            BlueprintFeature SeekerBloodlineDraconicSilver = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("efabab987569bf54abf23848c250e4d5");
            BlueprintFeature SeekerBloodlineDraconicWhite = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7572e8c020a6b8a46bde3ab3ad8c6f70");
            BlueprintFeature SeekerBloodlineElementalAir = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("940d34be432a1e543b0c0cbecd4ffc1d");
            BlueprintFeature SeekerBloodlineElementalEarth = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1c1cdb13caa111d49bd82a7e1f320803");
            BlueprintFeature SeekerBloodlineElementalFire = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("b19f95964e4a18f4cb3e4e3101593f22");
            BlueprintFeature SeekerBloodlineElementalWater = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("179a53407d1141142a91baace7e43325");
            BlueprintFeature SeekerBloodlineFey = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8e6dcc9095dacd042a644dd8c04ffac0");
            BlueprintFeature SeekerBloodlineInfernal = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("71f9b9d63f3683b4eb57e0025771932e");
            BlueprintFeature SeekerBloodlineSerpentine = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("59904bf6cc50a52489ebc648fb35f36f");
            BlueprintFeature SeekerBloodlineUndead = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("5bc63fdb68b539f4fa500cfb2d0fe0f6");
            // EmpyrealBloodlineProgression
            BlueprintFeature EmpyrealBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8a95d80a3162d274896d50c2f18bb6b1");
            // Sage Bloodlines
            BlueprintFeature SageBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("7d990675841a7354c957689a6707c6c2");
            // SylvanBloodlineProgression
            BlueprintFeature SylvanBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("a46d4bd93601427409d034a997673ece");

            Dictionary<BlueprintFeature, List<BlueprintFeatureReference>> BloodlinesBlockedCombinations = new Dictionary<BlueprintFeature, List<BlueprintFeatureReference>> {
                {BloodlineAbyssal, new List<BlueprintFeatureReference>{
                    SeekerBloodlineAbyssal.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineArcane, new List<BlueprintFeatureReference>{
                    SeekerBloodlineArcane.ToReference<BlueprintFeatureReference>(),
                    SageBloodline.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineCelestial, new List<BlueprintFeatureReference>{
                    SeekerBloodlineCelestial.ToReference<BlueprintFeatureReference>(),
                    EmpyrealBloodline.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineDraconicBlack, new List<BlueprintFeatureReference>{
                    SeekerBloodlineDraconicBlack.ToReference<BlueprintFeatureReference>(),
                }},
                {BloodlineDraconicBlue, new List<BlueprintFeatureReference>{
                    SeekerBloodlineDraconicBlue.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineDraconicBrass, new List<BlueprintFeatureReference>{
                    SeekerBloodlineDraconicBrass.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineDraconicCopper, new List<BlueprintFeatureReference>{
                    SeekerBloodlineDraconicCopper.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineDraconicGold, new List<BlueprintFeatureReference>{
                    SeekerBloodlineDraconicGold.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineDraconicGreen, new List<BlueprintFeatureReference>{
                    SeekerBloodlineDraconicGreen.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineDraconicRed, new List<BlueprintFeatureReference>{
                    SeekerBloodlineDraconicRed.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineDraconicSilver, new List<BlueprintFeatureReference>{
                    SeekerBloodlineDraconicSilver.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineDraconicWhite, new List<BlueprintFeatureReference>{
                    SeekerBloodlineDraconicWhite.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineElementalAir, new List<BlueprintFeatureReference>{
                    SeekerBloodlineElementalAir.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineElementalEarth, new List<BlueprintFeatureReference>{
                    SeekerBloodlineElementalEarth.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineElementalFire, new List<BlueprintFeatureReference>{
                    SeekerBloodlineElementalFire.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineElementalWater, new List<BlueprintFeatureReference>{
                    SeekerBloodlineElementalWater.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineFey, new List<BlueprintFeatureReference>{
                    SeekerBloodlineFey.ToReference<BlueprintFeatureReference>(),
                    SylvanBloodline.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineInfernal, new List<BlueprintFeatureReference>{
                    SeekerBloodlineInfernal.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineSerpentine, new List<BlueprintFeatureReference>{
                    SeekerBloodlineSerpentine.ToReference<BlueprintFeatureReference>()
                }},
                {BloodlineUndead, new List<BlueprintFeatureReference>{
                    SeekerBloodlineUndead.ToReference<BlueprintFeatureReference>()
                }},
            };
            foreach (BlueprintFeature Bloodline in BloodlinesBlockedCombinations.Keys) {
                IEnumerable<PrerequisiteNoFeature> newPrerequisites = BloodlinesBlockedCombinations[Bloodline].Select(blood => 
                    Helpers.Create<PrerequisiteNoFeature>(c => {
                        c.m_Feature = blood;
                        c.Group = Prerequisite.GroupType.All;
                    })
                );
                Bloodline.AddComponents(newPrerequisites);
                Main.LogPatch("Patched", Bloodline);
            }
        }
    }
}