using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using System.Text.RegularExpressions;
using TabletopTweaks.Core;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.Features {
    class Bloodlines {

        [PatchBlueprintsCacheInit]
        static class BlueprintsCache_Init_Patch {
            static bool Initialized;

            static void Postfix() {
                if (Initialized) return;
                Initialized = true;
                TTTContext.Logger.LogHeader("Patching Bloodlines");
                PatchBloodlineRestrictions();
            }
        }
        static void PatchBloodlineRestrictions() {
            if (TTTContext.Fixes.Bloodlines.IsDisabled("BloodlineRestrictions")) { return; }
            // Bloodline Requisite 
            var BloodlineRequisiteFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "BloodlineRequisiteFeature");
            // Requisite Features
            var AbyssalBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("b09b58c7f8efff244a33269489abeac6");
            var ArcaneBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("60d8632e96739a74dbac23dd078d205d");
            var CelestialBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6906eafd622304841bb45c2601aec7a7");
            var DraconicBlackBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d89fb8ce9152ffa4dacd69390f3d7721");
            var DraconicBlueBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("64e1f27147b642448842ab8dcbaca03f");
            var DraconicBrassBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("12bb1056a5f3f9f4b9facdb78b8d8914");
            var DraconicBronzeBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1d34d95ad4961e343b02db14690eb6d8");
            var DraconicCopperBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("eef664d1e4318f64cb2304d1628d29ae");
            var DraconicGoldBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("bef8d08ee3c20b246b404ce3ef948291");
            var DraconicGreenBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("49115e2147cd32841baa34c305171daa");
            var DraconicRedBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9c5ed34089fedf54ba8d0f43565bcc91");
            var DraconicSilverBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("01e7aab638d6a0b43bc4e9d5b49e68d9");
            var DraconicWhiteBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("3867419bf47841b428333808dfdf4ae0");
            var ElementalAirBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("65bc595a8a27acb48a8758c4d4caa338");
            var ElementalEarthBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("96e15b7b08b65d44b90a3f0c3c7ed72b");
            var ElementalFireBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("363737ca96b4a084b852688fd7430fa5");
            var ElementalWaterBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("cfd48c078a14ee247aa7eabe263ad132");
            var FeyBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("2c82cecdcafb5c741bf4901e689a4844");
            var InfernalBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e71d00a5aa1ed8f4591d021056a6dbe7");
            var SerpentineBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("233d0e5bdd25a7f44a56c09c8b5041e7");
            var UndeadBloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("8a59e4af9f32950418260034c8b477fa");
            // DragonHeirScion Bloodlines
            var DragonheirScionFeatureBlack = BlueprintTools.GetBlueprint<BlueprintFeature>("caee86c49e21d5845959ba5124f290cf");
            var DragonheirScionFeatureBlue = BlueprintTools.GetBlueprint<BlueprintFeature>("b09e72bd3325bf1419287fdbb989fc30");
            var DragonheirScionFeatureBrass = BlueprintTools.GetBlueprint<BlueprintFeature>("a6b1361e50599dd409905b4adaa05ee0");
            var DragonheirScionFeatureBronze = BlueprintTools.GetBlueprint<BlueprintFeature>("18f4e1b699f1e1f409c6bd44ce38aa0e");
            var DragonheirScionFeatureCopper = BlueprintTools.GetBlueprint<BlueprintFeature>("f6e0593ebfd33624f9b5d284b67927f7");
            var DragonheirScionFeatureGold = BlueprintTools.GetBlueprint<BlueprintFeature>("e8da00d4ee18fd246b624b776e1c0b64");
            var DragonheirScionFeatureGreen = BlueprintTools.GetBlueprint<BlueprintFeature>("190b2087470dcab49962af6d285ac6e2");
            var DragonheirScionFeatureRed = BlueprintTools.GetBlueprint<BlueprintFeature>("1a4be101e6761064e9bf4aeaf4863fff");
            var DragonheirScionFeatureSilver = BlueprintTools.GetBlueprint<BlueprintFeature>("be0d94ea05a931345981ac9bfd57840b");
            var DragonheirScionFeatureWhite = BlueprintTools.GetBlueprint<BlueprintFeature>("0db22c3cf99a03d409fb4fd9bd1ec53b");
            // Bloodrager Bloodlines
            var BloodragerAbyssalBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("55d5bbf4b5ae1744ab26c71be98067f9");
            var BloodragerArcaneBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("aeff0a749e20ffe4b9e2846eae29c386");
            var BloodragerCelestialBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("05a141717bbce594a8a763c227f4ee2f");
            var BloodragerDragonBlackBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("3d030a2fed2b5cf45919fc1e40629a9e");
            var BloodragerDragonBlueBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("17bbb6790ca500d4190b978cab5c4dfc");
            var BloodragerDragonBrassBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("56e22cb1dde3f5a4297d45744ca19043");
            var BloodragerDragonBronzeBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("0cf41c61c8ac463478e5ba733fd26b40");
            var BloodragerDragonCopperBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("53f189dce67466c4f9e60610e5d1c4ba");
            var BloodragerDragonGoldBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("ffcb3d0a1a45d8048a691eda9f0219b9");
            var BloodragerDragonGreenBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("69b27eb6bd71ac747a2fac8399c27c3a");
            var BloodragerDragonRedBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("34209f220733fc444a039df1b1076b0b");
            var BloodragerDragonSilverBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("df182ceef2330d74b9bd7bfdb23d144b");
            var BloodragerDragonWhiteBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("c97cf6524b89d474989378d841c7cf5c");
            var BloodragerElementalAcidBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("dafe58c4f0785e94e93c0f07901f1343");
            var BloodragerElementalColdBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("5286db4f19f31eb44af99fb881c99517");
            var BloodragerElementalElectricityBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("777bc77d3dc652c488a20d1a7b0b95e5");
            var BloodragerElementalFireBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("12f7b4c5d603f3744b2b1def28c0a4fa");
            var BloodragerFeyBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("a6e8fae8a6d6e374a9af2893840be4ac");
            var BloodragerInfernalBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("9aef64b53406f114cb43f898a3aec01e");
            var BloodragerSerpentineBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("f5b06b67f04949f4c8d88fd3bbc0771e");
            var BloodragerUndeadBloodline = BlueprintTools.GetBlueprint<BlueprintProgression>("9f4ea90e9b9c27c48b541dbef184b3b7");
            // Sorceror Bloodlines
            var BloodlineAbyssalProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("d3a4cb7be97a6694290f0dcfbd147113");
            var BloodlineArcaneProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("4d491cf9631f7e9429444f4aed629791");
            var BloodlineCelestialProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("aa79c65fa0e11464d9d100b038c50796");
            var BloodlineDraconicBlackProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("7bd143ead2d6c3a409aad6ee22effe34");
            var BloodlineDraconicBlueProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a7f100c02d0b254d8f5f3affc8ef386");
            var BloodlineDraconicBrassProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("5f9ecbee67db8364985e9d0500eb25f1");
            var BloodlineDraconicBronzeProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("7e0f57d8d00464441974e303b84238ac");
            var BloodlineDraconicCopperProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("b522759a265897b4f8f7a1a180a692e4");
            var BloodlineDraconicGoldProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("6c67ef823db8d7d45bb0ef82f959743d");
            var BloodlineDraconicGreenProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("7181be57d1cc3bc40bc4b552e4e4ce24");
            var BloodlineDraconicRedProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8c6e5b3cf12f71e43949f52c41ae70a8");
            var BloodlineDraconicSilverProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("c7d2f393e6574874bb3fc728a69cc73a");
            var BloodlineDraconicWhiteProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("b0f79497a0d1f4f4b8293e82c8f8fa0c");
            var BloodlineElementalAirProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("cd788df497c6f10439c7025e87864ee4");
            var BloodlineElementalEarthProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("32393034410fb2f4d9c8beaa5c8c8ab7");
            var BloodlineElementalFireProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("17cc794d47408bc4986c55265475c06f");
            var BloodlineElementalWaterProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("7c692e90592257a4e901d12ae6ec1e41");
            var BloodlineFeyProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("e8445256abbdc45488c2d90373f7dae8");
            var BloodlineInfernalProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("e76a774cacfb092498177e6ca706064d");
            var BloodlineSerpentineProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("739c1e842bf77994baf963f4ad964379");
            var BloodlineUndeadProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("a1a8bf61cadaa4143b2d4966f2d1142e");
            //Seeker Bloodlines
            var SeekerBloodlineAbyssalProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("17b752be1e0f4a34e8914df52eebeb75");
            var SeekerBloodlineArcaneProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("562c5e4031d268244a39e01cc4b834bb");
            var SeekerBloodlineCelestialProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("17ac9d771a944194a92ac15b5ff861c9");
            var SeekerBloodlineDraconicBlackProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("cf448fafcd8452d4b830bcc9ca074189");
            var SeekerBloodlineDraconicBlueProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("82f76646a7f96ed4cafa18480adc0b8c");
            var SeekerBloodlineDraconicBrassProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("c355b8777a3bda7429d863367bda3851");
            var SeekerBloodlineDraconicBronzeProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("468ecbdd58fbd6045a0a1888308031fe");
            var SeekerBloodlineDraconicCopperProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("2ad98f60b29ae604da0297037054080c");
            var SeekerBloodlineDraconicGoldProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("63c9d62a56e6921409a58de1ab9a9f9b");
            var SeekerBloodlineDraconicGreenProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("6de526eeee72852448c5595f7a44a39d");
            var SeekerBloodlineDraconicRedProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("d69a7785de1959c4497e4ff1e9490509");
            var SeekerBloodlineDraconicSilverProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("efabab987569bf54abf23848c250e4d5");
            var SeekerBloodlineDraconicWhiteProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("7572e8c020a6b8a46bde3ab3ad8c6f70");
            var SeekerBloodlineElementalAirProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("940d34be432a1e543b0c0cbecd4ffc1d");
            var SeekerBloodlineElementalEarthProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("1c1cdb13caa111d49bd82a7e1f320803");
            var SeekerBloodlineElementalFireProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("b19f95964e4a18f4cb3e4e3101593f22");
            var SeekerBloodlineElementalWaterProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("179a53407d1141142a91baace7e43325");
            var SeekerBloodlineFeyProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8e6dcc9095dacd042a644dd8c04ffac0");
            var SeekerBloodlineInfernalProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("71f9b9d63f3683b4eb57e0025771932e");
            var SeekerBloodlineSerpentineProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("59904bf6cc50a52489ebc648fb35f36f");
            var SeekerBloodlineUndeadProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("5bc63fdb68b539f4fa500cfb2d0fe0f6");
            // Mutated Bloodlines
            var EmpyrealBloodlineProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8a95d80a3162d274896d50c2f18bb6b1");
            var SageBloodlineProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("7d990675841a7354c957689a6707c6c2");
            var SylvanBloodlineProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("a46d4bd93601427409d034a997673ece");
            // Bloodline Selections
            var BloodOfDragonsSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("da48f9d7f697ae44ca891bfc50727988");
            var DragonheirDragonSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("729411185291d704696e58316420fe38");
            var BloodragerBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("62b33ac8ceb18dd47ad4c8f06849bc01");
            var SorcererBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("24bef8d1bee12274686f6da6ccbc8914");
            var SeekerBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7bda7cdb0ccda664c9eb8978cf512dbc");
            var NineTailedHeirBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7c813fb495d74246918a690ba86f9c86");
            var EldritchScionBloodlineSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("94c29f69cdc34594a6a4677441ed7375");

            // Fix Names
            BloodOfDragonsSelection.SetName(TTTContext, "Dragon Disciple Bloodline");
            DragonheirDragonSelection.SetName(TTTContext, "Dragonheir Scion Bloodline");
            SorcererBloodlineSelection.SetName(TTTContext, "Sorcerer Bloodline");
            BloodragerBloodlineSelection.SetName(TTTContext, "Bloodrager Bloodline");
            SeekerBloodlineSelection.SetName(TTTContext, "Seeker Bloodline");
            SageBloodlineProgression.SetName(TTTContext, "Sage Bloodline");
            NineTailedHeirBloodlineSelection.SetName(TTTContext, "Nine Tailed Heir Bloodline");
            EldritchScionBloodlineSelection.SetName(TTTContext, "Eldritch Scion Bloodline");
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
            AddRequisiteFeature(EmpyrealBloodlineProgression, BloodlineRequisiteFeature, CelestialBloodlineRequisiteFeature); TTTContext.Logger.LogPatch("Patched", EmpyrealBloodlineProgression);
            AddRequisiteFeature(SageBloodlineProgression, BloodlineRequisiteFeature, ArcaneBloodlineRequisiteFeature); TTTContext.Logger.LogPatch("Patched", SageBloodlineProgression);
            AddRequisiteFeature(SylvanBloodlineProgression, BloodlineRequisiteFeature, FeyBloodlineRequisiteFeature); TTTContext.Logger.LogPatch("Patched", SylvanBloodlineProgression);
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
                var EmpyrealSorcererArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("aa00d945f7cf6c34c909a29a25f2df38");
                var SageSorcererArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("00b990c8be2117e45ae6514ee4ef561c");
                var SylvanSorcererArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("711d5024ecc75f346b9cda609c3a1f83");
                var SeekerSorcererArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("7229db6fc0b07af4180e783eed43c4d9");

                FixMutatedPrerequisites(EmpyrealSorcererArchetype, CelestialBloodlineRequisiteFeature);
                FixMutatedPrerequisites(SageSorcererArchetype, ArcaneBloodlineRequisiteFeature);
                FixMutatedPrerequisites(SylvanSorcererArchetype, FeyBloodlineRequisiteFeature);
                SeekerSorcererArchetype.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.All;
                    c.m_Feature = BloodOfDragonsSelection.ToReference<BlueprintFeatureReference>();
                }));
                TTTContext.Logger.LogPatch("Patched", SeekerSorcererArchetype);

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
                    TTTContext.Logger.LogPatch("Patched", archetype);
                };
            }
            void FixDragonDisciple() {
                var DragonDiscipleClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("72051275b1dbb2d42ba9118237794f7c");
                var DragonDiscipleSpellbookSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8c1ba14c0b6dcdb439c56341385ee474");

                DragonDiscipleClass.SetComponents(DragonDiscipleClass.ComponentsArray
                    .Where(c => !(c is PrerequisiteNoFeature)) // Remove old Bloodline Feature
                    .Where(c => !(c is PrerequisiteNoArchetype)) // Remove Sorcerer Archetype Restrictions
                    .Where(c => !(c is PrerequisiteCondition)) // Remove Sorcerer Archetype Restrictions
                );
                DragonDiscipleClass.AddComponent<PrerequisiteNoFeature>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Feature = BloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>();
                });
                DragonDiscipleClass.AddComponent<PrerequisiteFeaturesFromList>(c => {
                    c.Group = Prerequisite.GroupType.Any;
                    c.m_Features = new BlueprintFeatureReference[] {
                        DraconicBlackBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                        DraconicBlueBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                        DraconicBrassBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                        DraconicBronzeBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                        DraconicCopperBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                        DraconicGoldBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                        DraconicGreenBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                        DraconicRedBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                        DraconicSilverBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                        DraconicWhiteBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>()
                    };
                });
                BloodOfDragonsSelection.GetComponent<NoSelectionIfAlreadyHasFeature>()
                    .m_Features = new BlueprintFeatureReference[] { BloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>() };
                DragonDiscipleSpellbookSelection.AddFeatures(
                    BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "DragonDiscipleSageSorcerer"),
                    BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "DragonDiscipleEmpyrealSorcerer"),
                    BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "DragonDiscipleUnletteredArcanist"),
                    BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "DragonDiscipleNatureMage"),
                    BlueprintTools.GetModBlueprint<BlueprintFeature>(TTTContext, "DragonDiscipleAccursedWitch")
                ); ;
                TTTContext.Logger.LogPatch("Patched", BloodOfDragonsSelection);
                TTTContext.Logger.LogPatch("Patched", DragonDiscipleSpellbookSelection);
                TTTContext.Logger.LogPatch("Patched", DragonDiscipleClass);
            }
            void FixDragonheirScion() {
                var DragonheirScionArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("8dff97413c63c1147be8a5ca229abefc");
                var DragonDiscipleClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("72051275b1dbb2d42ba9118237794f7c");
                var DragonheirAcidProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("f09074860cc87fd4ebf6bf69ddd20d10");
                var DragonheirProgressionCold = BlueprintTools.GetBlueprint<BlueprintProgression>("ff7eb5969525b5b41b2c68328bc9bb7c");
                var DragonheirProgressionElectricity = BlueprintTools.GetBlueprint<BlueprintProgression>("07f6ba0f63d8d414f92b5d0a559455e1");
                var DragonheirProgressionFire = BlueprintTools.GetBlueprint<BlueprintProgression>("8e30b4dab152d4549bf9c0dbf901aadf");

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
                        c.m_Features = new BlueprintFeatureReference[] {
                            DraconicBlackBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                            DraconicBlueBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                            DraconicBrassBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                            DraconicBronzeBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                            DraconicCopperBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                            DraconicGoldBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                            DraconicGreenBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                            DraconicRedBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                            DraconicSilverBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>(),
                            DraconicWhiteBloodlineRequisiteFeature.ToReference<BlueprintFeatureReference>()
                        };
                    });
                    archetype.AddComponents(noBloodline, draconicBloodline);
                }
                void EnableDragonDisicpleAdvancement(BlueprintFeature feature) {
                    feature.GetComponent<AddFeatureOnClassLevel>().m_AdditionalClasses = new BlueprintCharacterClassReference[] {
                        DragonDiscipleClass.ToReference<BlueprintCharacterClassReference>()
                    };
                    TTTContext.Logger.LogPatch("Patched", feature);
                }
                void EnableDragonDisicpleProgression(BlueprintProgression feature) {
                    feature.m_Classes = feature.m_Classes.AddItem(new BlueprintProgression.ClassWithLevel {
                        m_Class = DragonDiscipleClass.ToReference<BlueprintCharacterClassReference>()
                    }).ToArray();
                    TTTContext.Logger.LogPatch("Patched", feature);
                }
            }
            void FixRequisiteName(int length, BlueprintFeature feature) {
                string[] split = Regex.Split(feature.name, @"(?<!^)(?=[A-Z])");
                if (length == 1) {
                    feature.SetName(TTTContext, $"{split[0]} {split[1]}");
                } else {
                    feature.SetName(TTTContext, $"{split[0]} {split[2]} — {split[1]}");
                }
                feature.SetDescription(TTTContext, "Bloodline Requisite Feature");
                TTTContext.Logger.LogPatch("Patched", feature);
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
                TTTContext.Logger.LogPatch("Patched", bloodline);
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
                bloodline.AddPrerequisites(requisiteFeature);
                bloodline.AddComponents(noBloodline, addFacts);
                TTTContext.Logger.LogPatch("Patched", bloodline);
                AddSaveGamePatch(bloodline, requisite);
            }
            void AddSaveGamePatch(BlueprintFeature bloodline, BlueprintFeature requisite) {
                SaveGameFix.AddUnitPatch((unit) => {
                    if (unit.HasFact(bloodline)) {
                        if (!unit.HasFact(BloodlineRequisiteFeature)) {
                            if (unit.AddFact(BloodlineRequisiteFeature) != null) {
                                TTTContext.Logger.Log($"Added: {BloodlineRequisiteFeature} To: {unit.CharacterName}");
                                return;
                            }
                            TTTContext.Logger.Log($"Failed Add: {BloodlineRequisiteFeature} To: {unit.CharacterName}");
                        }
                        if (!unit.HasFact(requisite)) {
                            if (unit.AddFact(BloodlineRequisiteFeature) != null) {
                                TTTContext.Logger.Log($"Added: {BloodlineRequisiteFeature} To: {unit.CharacterName}");
                                return;
                            }
                            TTTContext.Logger.Log($"Failed Add: {BloodlineRequisiteFeature} To: {unit.CharacterName}");
                        }
                    }
                });
            }
        }
    }
}