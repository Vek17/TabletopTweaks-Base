using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using System.Text.RegularExpressions;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.Bugfixes.Features {
    class Bloodlines {

        [HarmonyPatch(typeof(ResourcesLibrary), "InitializeLibrary")]
        static class ResourcesLibrary_InitializeLibrary_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                if (Settings.Fixes.Bloodlines.DisableAllFixes) { return; }
                Main.LogHeader("Patching Bloodline Resources");
                PatchBloodlineRestrictions();
                Main.LogHeader("Patching Bloodline Complete");
            }
        }
        static void PatchBloodlineRestrictions() {
            if (!Settings.Fixes.Bloodlines.Fixes["BloodlineRestrictions"]) { return; }
            // Bloodline Requisite 
            var BloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(Settings.Blueprints.NewBlueprints["BloodlineRequisiteFeature"]);
            // Requisite Features
            var AbyssalBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("b09b58c7f8efff244a33269489abeac6");
            var ArcaneBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("60d8632e96739a74dbac23dd078d205d");
            var CelestialBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("6906eafd622304841bb45c2601aec7a7");
            var DraconicBlackBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("d89fb8ce9152ffa4dacd69390f3d7721");
            var DraconicBlueBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("64e1f27147b642448842ab8dcbaca03f");
            var DraconicBrassBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("12bb1056a5f3f9f4b9facdb78b8d8914");
            var DraconicBronzeBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1d34d95ad4961e343b02db14690eb6d8");
            var DraconicCopperBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("eef664d1e4318f64cb2304d1628d29ae");
            var DraconicGoldBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("bef8d08ee3c20b246b404ce3ef948291");
            var DraconicGreenBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("49115e2147cd32841baa34c305171daa");
            var DraconicRedBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("9c5ed34089fedf54ba8d0f43565bcc91");
            var DraconicSilverBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("01e7aab638d6a0b43bc4e9d5b49e68d9");
            var DraconicWhiteBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("3867419bf47841b428333808dfdf4ae0");
            var ElementalAirBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("65bc595a8a27acb48a8758c4d4caa338");
            var ElementalEarthBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("96e15b7b08b65d44b90a3f0c3c7ed72b");
            var ElementalFireBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("363737ca96b4a084b852688fd7430fa5");
            var ElementalWaterBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("cfd48c078a14ee247aa7eabe263ad132");
            var FeyBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("2c82cecdcafb5c741bf4901e689a4844");
            var InfernalBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("e71d00a5aa1ed8f4591d021056a6dbe7");
            var SerpentineBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("233d0e5bdd25a7f44a56c09c8b5041e7");
            var UndeadBloodlineRequisiteFeature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("8a59e4af9f32950418260034c8b477fa");
            // DragonHeirScion Bloodlines
            var DragonheirScionFeatureBlack = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("caee86c49e21d5845959ba5124f290cf");
            var DragonheirScionFeatureBlue = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("b09e72bd3325bf1419287fdbb989fc30");
            var DragonheirScionFeatureBrass = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("a6b1361e50599dd409905b4adaa05ee0");
            var DragonheirScionFeatureBronze = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("18f4e1b699f1e1f409c6bd44ce38aa0e");
            var DragonheirScionFeatureCopper = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("f6e0593ebfd33624f9b5d284b67927f7");
            var DragonheirScionFeatureGold = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("e8da00d4ee18fd246b624b776e1c0b64");
            var DragonheirScionFeatureGreen = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("190b2087470dcab49962af6d285ac6e2");
            var DragonheirScionFeatureRed = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("1a4be101e6761064e9bf4aeaf4863fff");
            var DragonheirScionFeatureSilver = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("be0d94ea05a931345981ac9bfd57840b");
            var DragonheirScionFeatureWhite = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>("0db22c3cf99a03d409fb4fd9bd1ec53b");
            // Bloodrager Bloodlines
            var BloodragerAbyssalBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("55d5bbf4b5ae1744ab26c71be98067f9");
            var BloodragerArcaneBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("aeff0a749e20ffe4b9e2846eae29c386");
            var BloodragerCelestialBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("05a141717bbce594a8a763c227f4ee2f");
            var BloodragerDragonBlackBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("3d030a2fed2b5cf45919fc1e40629a9e");
            var BloodragerDragonBlueBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("17bbb6790ca500d4190b978cab5c4dfc");
            var BloodragerDragonBrassBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("56e22cb1dde3f5a4297d45744ca19043");
            var BloodragerDragonBronzeBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("0cf41c61c8ac463478e5ba733fd26b40");
            var BloodragerDragonCopperBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("53f189dce67466c4f9e60610e5d1c4ba");
            var BloodragerDragonGoldBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("ffcb3d0a1a45d8048a691eda9f0219b9");
            var BloodragerDragonGreenBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("69b27eb6bd71ac747a2fac8399c27c3a");
            var BloodragerDragonRedBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("34209f220733fc444a039df1b1076b0b");
            var BloodragerDragonSilverBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("df182ceef2330d74b9bd7bfdb23d144b");
            var BloodragerDragonWhiteBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("c97cf6524b89d474989378d841c7cf5c");
            var BloodragerElementalAcidBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("dafe58c4f0785e94e93c0f07901f1343");
            var BloodragerElementalColdBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("5286db4f19f31eb44af99fb881c99517");
            var BloodragerElementalElectricityBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("777bc77d3dc652c488a20d1a7b0b95e5");
            var BloodragerElementalFireBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("12f7b4c5d603f3744b2b1def28c0a4fa");
            var BloodragerFeyBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("a6e8fae8a6d6e374a9af2893840be4ac");
            var BloodragerInfernalBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("9aef64b53406f114cb43f898a3aec01e");
            var BloodragerSerpentineBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("f5b06b67f04949f4c8d88fd3bbc0771e");
            var BloodragerUndeadBloodline = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("9f4ea90e9b9c27c48b541dbef184b3b7");
            // Sorceror Bloodlines
            var BloodlineAbyssalProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("d3a4cb7be97a6694290f0dcfbd147113");
            var BloodlineArcaneProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("4d491cf9631f7e9429444f4aed629791");
            var BloodlineCelestialProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("aa79c65fa0e11464d9d100b038c50796");
            var BloodlineDraconicBlackProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("7bd143ead2d6c3a409aad6ee22effe34");
            var BloodlineDraconicBlueProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("8a7f100c02d0b254d8f5f3affc8ef386");
            var BloodlineDraconicBrassProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("5f9ecbee67db8364985e9d0500eb25f1");
            var BloodlineDraconicBronzeProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("7e0f57d8d00464441974e303b84238ac");
            var BloodlineDraconicCopperProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("b522759a265897b4f8f7a1a180a692e4");
            var BloodlineDraconicGoldProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("6c67ef823db8d7d45bb0ef82f959743d");
            var BloodlineDraconicGreenProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("7181be57d1cc3bc40bc4b552e4e4ce24");
            var BloodlineDraconicRedProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("8c6e5b3cf12f71e43949f52c41ae70a8");
            var BloodlineDraconicSilverProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("c7d2f393e6574874bb3fc728a69cc73a");
            var BloodlineDraconicWhiteProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("b0f79497a0d1f4f4b8293e82c8f8fa0c");
            var BloodlineElementalAirProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("cd788df497c6f10439c7025e87864ee4");
            var BloodlineElementalEarthProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("32393034410fb2f4d9c8beaa5c8c8ab7");
            var BloodlineElementalFireProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("17cc794d47408bc4986c55265475c06f");
            var BloodlineElementalWaterProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("7c692e90592257a4e901d12ae6ec1e41");
            var BloodlineFeyProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("e8445256abbdc45488c2d90373f7dae8");
            var BloodlineInfernalProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("e76a774cacfb092498177e6ca706064d");
            var BloodlineSerpentineProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("739c1e842bf77994baf963f4ad964379");
            var BloodlineUndeadProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("a1a8bf61cadaa4143b2d4966f2d1142e");
            //Seeker Bloodlines
            var SeekerBloodlineAbyssalProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("17b752be1e0f4a34e8914df52eebeb75");
            var SeekerBloodlineArcaneProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("562c5e4031d268244a39e01cc4b834bb");
            var SeekerBloodlineCelestialProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("17ac9d771a944194a92ac15b5ff861c9");
            var SeekerBloodlineDraconicBlackProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("cf448fafcd8452d4b830bcc9ca074189");
            var SeekerBloodlineDraconicBlueProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("82f76646a7f96ed4cafa18480adc0b8c");
            var SeekerBloodlineDraconicBrassProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("c355b8777a3bda7429d863367bda3851");
            var SeekerBloodlineDraconicBronzeProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("468ecbdd58fbd6045a0a1888308031fe");
            var SeekerBloodlineDraconicCopperProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("2ad98f60b29ae604da0297037054080c");
            var SeekerBloodlineDraconicGoldProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("63c9d62a56e6921409a58de1ab9a9f9b");
            var SeekerBloodlineDraconicGreenProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("6de526eeee72852448c5595f7a44a39d");
            var SeekerBloodlineDraconicRedProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("d69a7785de1959c4497e4ff1e9490509");
            var SeekerBloodlineDraconicSilverProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("efabab987569bf54abf23848c250e4d5");
            var SeekerBloodlineDraconicWhiteProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("7572e8c020a6b8a46bde3ab3ad8c6f70");
            var SeekerBloodlineElementalAirProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("940d34be432a1e543b0c0cbecd4ffc1d");
            var SeekerBloodlineElementalEarthProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("1c1cdb13caa111d49bd82a7e1f320803");
            var SeekerBloodlineElementalFireProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("b19f95964e4a18f4cb3e4e3101593f22");
            var SeekerBloodlineElementalWaterProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("179a53407d1141142a91baace7e43325");
            var SeekerBloodlineFeyProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("8e6dcc9095dacd042a644dd8c04ffac0");
            var SeekerBloodlineInfernalProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("71f9b9d63f3683b4eb57e0025771932e");
            var SeekerBloodlineSerpentineProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("59904bf6cc50a52489ebc648fb35f36f");
            var SeekerBloodlineUndeadProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("5bc63fdb68b539f4fa500cfb2d0fe0f6");
            // Mutated Bloodlines
            var EmpyrealBloodlineProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("8a95d80a3162d274896d50c2f18bb6b1");
            var SageBloodlineProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("7d990675841a7354c957689a6707c6c2");
            var SylvanBloodlineProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("a46d4bd93601427409d034a997673ece");
            // Bloodline Selections
            var BloodOfDragonsSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("da48f9d7f697ae44ca891bfc50727988");
            var DragonheirDragonSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("729411185291d704696e58316420fe38");
            var BloodragerBloodlineSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("62b33ac8ceb18dd47ad4c8f06849bc01");
            var SorcererBloodlineSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("24bef8d1bee12274686f6da6ccbc8914");
            var SeekerBloodlineSelection = ResourcesLibrary.TryGetBlueprint<BlueprintFeatureSelection>("7bda7cdb0ccda664c9eb8978cf512dbc");

            // Fix Names
            BloodOfDragonsSelection.SetName("Dragon Disciple Bloodline");
            DragonheirDragonSelection.SetName("Dragonheir Scion Bloodline");
            SorcererBloodlineSelection.SetName("Sorcerer Bloodline");
            BloodragerBloodlineSelection.SetName("Bloodrager Bloodline");
            SeekerBloodlineSelection.SetName("Seeker Bloodline");
            SageBloodlineProgression.SetName("Sage Bloodline");
            FixRequisiteName(1, AbyssalBloodlineRequisiteFeature);
            FixRequisiteName(1, ArcaneBloodlineRequisiteFeature);
            FixRequisiteName(1, CelestialBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicBlackBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicBlueBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicBrassBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicBronzeBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicCopperBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicGoldBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicGreenBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicRedBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicSilverBloodlineRequisiteFeature);
            FixRequisiteName(2, DraconicWhiteBloodlineRequisiteFeature);
            FixRequisiteName(2, ElementalAirBloodlineRequisiteFeature);
            FixRequisiteName(2, ElementalEarthBloodlineRequisiteFeature);
            FixRequisiteName(2, ElementalFireBloodlineRequisiteFeature);
            FixRequisiteName(2, ElementalWaterBloodlineRequisiteFeature);
            FixRequisiteName(1, FeyBloodlineRequisiteFeature);
            FixRequisiteName(1, InfernalBloodlineRequisiteFeature);
            FixRequisiteName(1, SerpentineBloodlineRequisiteFeature);
            FixRequisiteName(1, UndeadBloodlineRequisiteFeature);
            // Fix Mutated Bloodlines
            AddRequisiteFeature(EmpyrealBloodlineProgression, BloodlineRequisiteFeature, CelestialBloodlineRequisiteFeature); Main.LogPatch("Patched", EmpyrealBloodlineProgression);
            AddRequisiteFeature(SageBloodlineProgression, BloodlineRequisiteFeature, ArcaneBloodlineRequisiteFeature); Main.LogPatch("Patched", SageBloodlineProgression);
            AddRequisiteFeature(SylvanBloodlineProgression, BloodlineRequisiteFeature, FeyBloodlineRequisiteFeature); Main.LogPatch("Patched", SylvanBloodlineProgression);
            // Fix Sorcerer Bloodlines
            FixBloodlineProgressionPrerequisites(BloodlineAbyssalProgression, AbyssalBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineArcaneProgression, ArcaneBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineCelestialProgression, CelestialBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicBlackProgression, DraconicBlackBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicBlueProgression, DraconicBlueBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicBrassProgression, DraconicBrassBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicBronzeProgression, DraconicBronzeBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicCopperProgression, DraconicCopperBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicGoldProgression, DraconicGoldBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicGreenProgression, DraconicGreenBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicRedProgression, DraconicRedBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicSilverProgression, DraconicSilverBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineDraconicWhiteProgression, DraconicWhiteBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineElementalAirProgression, ElementalAirBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineElementalEarthProgression, ElementalEarthBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineElementalFireProgression, ElementalFireBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineElementalWaterProgression, ElementalWaterBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineFeyProgression, FeyBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineInfernalProgression, InfernalBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineSerpentineProgression, SerpentineBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodlineUndeadProgression, UndeadBloodlineRequisiteFeature);
            // Fix Seeker Bloodlines
            FixBloodlineProgressionPrerequisites(SeekerBloodlineAbyssalProgression, AbyssalBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineArcaneProgression, ArcaneBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineCelestialProgression, CelestialBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicBlackProgression, DraconicBlackBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicBlueProgression, DraconicBlueBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicBrassProgression, DraconicBrassBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicBronzeProgression, DraconicBronzeBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicCopperProgression, DraconicCopperBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicGoldProgression, DraconicGoldBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicGreenProgression, DraconicGreenBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicRedProgression, DraconicRedBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicSilverProgression, DraconicSilverBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineDraconicWhiteProgression, DraconicWhiteBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineElementalAirProgression, ElementalAirBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineElementalEarthProgression, ElementalEarthBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineElementalFireProgression, ElementalFireBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineElementalWaterProgression, ElementalWaterBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineFeyProgression, FeyBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineInfernalProgression, InfernalBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineSerpentineProgression, SerpentineBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(SeekerBloodlineUndeadProgression, UndeadBloodlineRequisiteFeature);
            // Fix Bloodrager Bloodlines
            FixBloodlineProgressionPrerequisites(BloodragerAbyssalBloodline, AbyssalBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerArcaneBloodline, ArcaneBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerCelestialBloodline, CelestialBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonBlackBloodline, DraconicBlackBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonBlueBloodline, DraconicBlueBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonBrassBloodline, DraconicBrassBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonBronzeBloodline, DraconicBronzeBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonCopperBloodline, DraconicCopperBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonGoldBloodline, DraconicGoldBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonGreenBloodline, DraconicGreenBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonRedBloodline, DraconicRedBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonSilverBloodline, DraconicSilverBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerDragonWhiteBloodline, DraconicWhiteBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerElementalElectricityBloodline, ElementalAirBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerElementalAcidBloodline, ElementalEarthBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerElementalFireBloodline, ElementalFireBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerElementalColdBloodline, ElementalWaterBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerFeyBloodline, FeyBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerInfernalBloodline, InfernalBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerSerpentineBloodline, SerpentineBloodlineRequisiteFeature);
            FixBloodlineProgressionPrerequisites(BloodragerUndeadBloodline, UndeadBloodlineRequisiteFeature);
            // Fix Dragonheir Scion Bloodlines
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureBlack, DraconicBlackBloodlineRequisiteFeature);
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureBlue, DraconicBlueBloodlineRequisiteFeature);
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureBrass, DraconicBrassBloodlineRequisiteFeature);
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureBronze, DraconicBronzeBloodlineRequisiteFeature);
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureCopper, DraconicCopperBloodlineRequisiteFeature);
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureGold, DraconicGoldBloodlineRequisiteFeature);
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureGreen, DraconicGreenBloodlineRequisiteFeature);
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureRed, DraconicRedBloodlineRequisiteFeature);
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureSilver, DraconicSilverBloodlineRequisiteFeature);
            FixBloodlineFeaturePrerequisites(DragonheirScionFeatureWhite, DraconicWhiteBloodlineRequisiteFeature);
            // Patch Bloodline Class Prerequisites
            FixSorcererArchetypes();
            FixDragonDisciple();
            FixDragonheirScion();

            void FixSorcererArchetypes() {
                var EmpyrealSorcererArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("aa00d945f7cf6c34c909a29a25f2df38");
                var SageSorcererArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("00b990c8be2117e45ae6514ee4ef561c");
                var SylvanSorcererArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("711d5024ecc75f346b9cda609c3a1f83");
                var SeekerSorcererArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("7229db6fc0b07af4180e783eed43c4d9");

                FixMutatedPrerequisites(EmpyrealSorcererArchetype, CelestialBloodlineRequisiteFeature);
                FixMutatedPrerequisites(SageSorcererArchetype, ArcaneBloodlineRequisiteFeature);
                FixMutatedPrerequisites(SylvanSorcererArchetype, FeyBloodlineRequisiteFeature);
                SeekerSorcererArchetype.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.All;
                    c.m_Feature = BloodOfDragonsSelection.ToReference<BlueprintFeatureReference>();
                }));
                Main.LogPatch("Patched", SeekerSorcererArchetype);

                void FixMutatedPrerequisites(BlueprintArchetype archetype, BlueprintFeature requisite) {
                    var noBloodline = Helpers.Create<PrerequisiteNoFeature>(c => {
                        c.Group = Prerequisite.GroupType.Any;
                        c.m_Feature = BloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
                    });
                    var requisiteFeature = Helpers.Create<PrerequisiteFeature>(c => {
                        c.Group = Prerequisite.GroupType.Any;
                        c.m_Feature = requisite.ToReference<BlueprintFeatureReference>();
                    });
                    archetype.AddComponents(noBloodline, requisiteFeature);
                    Main.LogPatch("Patched", archetype);
                };
            }
            void FixDragonDisciple() {
                var DragonDiscipleClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("72051275b1dbb2d42ba9118237794f7c");
                var noBloodline = Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
                });

                DragonDiscipleClass.SetComponents(DragonDiscipleClass.ComponentsArray
                    .Where(c => !(c is PrerequisiteNoFeature)) // Remove old Bloodline Feature
                    .Where(c => !(c is PrerequisiteNoArchetype)) // Remove Sorcerer Archetype Restrictions
                    .Append(noBloodline));
                BloodOfDragonsSelection.GetComponent<NoSelectionIfAlreadyHasFeature>().m_Features = new BlueprintFeatureReference[] { BloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>() };
                Main.LogPatch("Patched", BloodOfDragonsSelection);
                Main.LogPatch("Patched", DragonDiscipleClass);
            }
            void FixDragonheirScion() {
                var DragonheirScionArchetype = ResourcesLibrary.TryGetBlueprint<BlueprintArchetype>("8dff97413c63c1147be8a5ca229abefc");
                var DragonDiscipleClass = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("72051275b1dbb2d42ba9118237794f7c");
                var DragonheirAcidProgression = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("f09074860cc87fd4ebf6bf69ddd20d10");
                var DragonheirProgressionCold = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("ff7eb5969525b5b41b2c68328bc9bb7c");
                var DragonheirProgressionElectricity = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("07f6ba0f63d8d414f92b5d0a559455e1");
                var DragonheirProgressionFire = ResourcesLibrary.TryGetBlueprint<BlueprintProgression>("8e30b4dab152d4549bf9c0dbf901aadf");

                AddDragonheirScionPrerequisites(DragonheirScionArchetype);

                EnableDragonDisicpleAdvancement(DragonheirScionFeatureBlack);
                EnableDragonDisicpleAdvancement(DragonheirScionFeatureBlue);
                EnableDragonDisicpleAdvancement(DragonheirScionFeatureBrass);
                EnableDragonDisicpleAdvancement(DragonheirScionFeatureBronze);
                EnableDragonDisicpleAdvancement(DragonheirScionFeatureCopper);
                EnableDragonDisicpleAdvancement(DragonheirScionFeatureGold);
                EnableDragonDisicpleAdvancement(DragonheirScionFeatureGreen);
                EnableDragonDisicpleAdvancement(DragonheirScionFeatureRed);
                EnableDragonDisicpleAdvancement(DragonheirScionFeatureSilver);
                EnableDragonDisicpleAdvancement(DragonheirScionFeatureWhite);

                EnableDragonDisicpleProgression(DragonheirAcidProgression);
                EnableDragonDisicpleProgression(DragonheirProgressionCold);
                EnableDragonDisicpleProgression(DragonheirProgressionElectricity);
                EnableDragonDisicpleProgression(DragonheirProgressionFire);

                void AddDragonheirScionPrerequisites(BlueprintArchetype archetype) {
                    var noBloodline = Helpers.Create<PrerequisiteNoFeature>(c => {
                        c.Group = Prerequisite.GroupType.Any;
                        c.m_Feature = BloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
                    });
                    var draconicBloodline = Helpers.Create<PrerequisiteFeaturesFromList>(c => {
                        c.Group = Prerequisite.GroupType.Any;
                        c.m_Features = DragonDiscipleClass.GetComponent<PrerequisiteFeaturesFromList>().m_Features;
                    });
                    archetype.AddComponents(noBloodline, draconicBloodline);
                }
                void EnableDragonDisicpleAdvancement(BlueprintFeature feature) {
                    feature.GetComponent<AddFeatureOnClassLevel>().m_AdditionalClasses = new BlueprintCharacterClassReference[] {
                        DragonDiscipleClass.ToReference<BlueprintCharacterClassReference>()
                    };
                    Main.LogPatch("Patched", feature);
                }
                void EnableDragonDisicpleProgression(BlueprintProgression feature) {
                    feature.m_Classes = feature.m_Classes.AddItem(new BlueprintProgression.ClassWithLevel {
                        m_Class = DragonDiscipleClass.ToReference<BlueprintCharacterClassReference>()
                    }).ToArray();
                    Main.LogPatch("Patched", feature);
                }
            }
            void FixRequisiteName(int length, BlueprintFeature feature) {
                string[] split = Regex.Split(feature.name, @"(?<!^)(?=[A-Z])");
                if (length == 1) {
                    feature.SetName($"{split[0]} {split[1]}");
                }
                else {
                    feature.SetName($"{split[0]} {split[2]} — {split[1]}");
                }
                feature.SetDescription("Bloodline Requisite Feature");
                Main.LogPatch("Patched", feature);
            }
            void AddRequisiteFeature(BlueprintProgression bloodline, params BlueprintFeature[] requisites) {
                var levelOne = bloodline.LevelEntries.Where(entry => entry.Level == 1).First();
                foreach (var requisite in requisites) {
                    if (!levelOne.m_Features.Contains(requisite.ToReference<BlueprintFeatureBaseReference>())) {
                        levelOne.m_Features.Add(requisite.ToReference<BlueprintFeatureBaseReference>());
                    }
                }
            }
            void FixBloodlineProgressionPrerequisites(BlueprintProgression bloodline, BlueprintFeature requisite) {
                var noBloodline = Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
                });
                var requisiteFeature = Helpers.Create<PrerequisiteFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = requisite.ToReference<BlueprintFeatureReference>();
                });
                AddRequisiteFeature(bloodline, BloodlineRequisiteFeature, requisite);
                bloodline.RemoveComponents<Prerequisite>();
                bloodline.AddComponents(noBloodline, requisiteFeature);
                Main.LogPatch("Patched", bloodline);
                AddSaveGamePatch(bloodline, requisite);
            }
            void FixBloodlineFeaturePrerequisites(BlueprintFeature bloodline, BlueprintFeature requisite) {
                var noBloodline = Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
                });
                var requisiteFeature = Helpers.Create<PrerequisiteFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = requisite.ToReference<BlueprintFeatureReference>();
                });
                var addFacts = Helpers.Create<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] { 
                        BloodlineRequisiteFeature.ToReference<BlueprintUnitFactReference>(),
                        requisite.ToReference<BlueprintUnitFactReference>()
                    };
                });
                bloodline.RemoveComponents<Prerequisite>();
                bloodline.AddComponents(noBloodline, requisiteFeature, addFacts);
                Main.LogPatch("Patched", bloodline);
                AddSaveGamePatch(bloodline, requisite);
            }
            void AddSaveGamePatch(BlueprintFeature bloodline, BlueprintFeature requisite) {
                SaveGameFix.AddUnitPatch((unit) => {
                    if (unit.HasFact(bloodline)) {
                        if (!unit.HasFact(BloodlineRequisiteFeature)) {
                            if (unit.AddFact(BloodlineRequisiteFeature) != null) {
                                Main.Log($"Added: {BloodlineRequisiteFeature} To: {unit.CharacterName}");
                                return;
                            }
                            Main.Log($"Failed Add: {BloodlineRequisiteFeature} To: {unit.CharacterName}");
                        }
                        if (!unit.HasFact(requisite)) {
                            if (unit.AddFact(BloodlineRequisiteFeature) != null) {
                                Main.Log($"Added: {BloodlineRequisiteFeature} To: {unit.CharacterName}");
                                return;
                            }
                                Main.Log($"Failed Add: {BloodlineRequisiteFeature} To: {unit.CharacterName}");
                        }
                    }
                });
            }
        }
    }
}