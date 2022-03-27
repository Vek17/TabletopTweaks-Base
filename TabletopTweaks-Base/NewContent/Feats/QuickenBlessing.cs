using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Commands.Base;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Feats {
    static class QuickenBlessing {
        public static void AddQuickenBlessing() {
            var SelectiveChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff");
            var ExtraChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("cd9f19775bd9d3343a31a065e93f0c47");
            var WarpriestClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("30b5e47d47a0e37438cc5a80c96cfb99");

            var AirBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e1ff99dc3aeaa064e8eecde51c1c4773");
            var AnimalBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9d991f8374c3def4cb4a6287f370814d");
            var ChaosBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("528e316f9f092954b9e38d3a82b1634a");
            var CharmBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("516bc13e0e76a834bb3a4c3e3d01c0cf");
            var CommunityBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("36fcd6ca7e279874d9197f38501f0e93");
            var DarknessBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("3ed6cd88caecec944b837f57b9be176f");
            var DeathBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("6d11e8b00add90c4f93c2ad6d12885f7");
            var DestructionBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("dd5e75a02e4563e44a0931c6f46fb0a7");
            var EarthBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("73c37a22bc9a523409a47218d507acf6");
            var EvilBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("f38f3abf6ca3a07499a61f96e342bb16");
            var FireBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("2368212fa3856d74589e924d3e2074d8");
            var GoodBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("60a85144ed37e3a45b343d291dc48079");
            var HealingBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("f3881a1a7b44dc74c9d76907c94e49f2");
            var LawBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("9c49504e2e4c66d4aa341348356b47a8");
            var LuckBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("70654ee784fffa74489933a0d2047bbd");
            var MagicBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("1754ff61a0805714fa2b89c8c1bb87ad");
            var NobilityBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("f52af97d05e5de34ea6e0d1b0af740ea");
            var ProtectionBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("c6a3fa9d8d7f942499e4909cd01ca22d");
            var ReposeBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("64a416082927673409deb330af04d6d2");
            var SunBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("ba825e3c77acaec4386e00f691f8f3be");
            var TravelBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("87641a8efec53d64d853ecc436234dce");
            var TrickeryBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("a8e7abcad0cf8384b9f12c3b075b5cae");
            var WaterBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("0f457943bb99f9b48b709c90bfc0467e");
            var WeatherBlessingFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("4172d92c598de1d47aa2c0dd51c05e24");

            var AirBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("006cfb97660cd38438386bd46488cc23");
            var AirBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("364e1e0e4af5b004caabe5f005cee7ca");
            var AnimalBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("93f0098fe08b94f41a351a4fbb00518a");
            var AnimalBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("56a2584706b8faf4094acfa142747f70");
            var ChaosBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("6fb60e6cb81138941a07e6861df839c6");
            var ChaosBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("39a1a4085aabd154e8aa565cb1579e5e");
            var CharmBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("d58214dd4e8bd7242b9129eed67d1b61");
            var CharmBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("5aab5321b19e82545a65f203181c3470");
            var CommunityBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c65a2844952e3a94293cd7c42067eeef");
            var CommunityBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("7b9d788a9f7f6b943a5bfa42aa0febed");
            var DarknessBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("1d225432d2bfccd4da91959a9b9378bd");
            var DarknessBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("a91bb1ca61938b44bb236b0a52fe0188");
            var DeathBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("04dd71ac77051bc46aab114d200e65dd");
            var DeathBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("542a9661ed997ff418322ff7376bab8c");
            var DestructionBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("b4bf40373f371074a87fc6ce554287be");
            var DestructionBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("15559878dd3ac2943b17a5d52ffe6b8a");
            var EarthBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("3313addb536171f4d86bb133ec51b008");
            var EarthBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("4cfa41e8557d3c54d9af27af6964bd7e");
            var EvilBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c9bbd75a934c6b44fbd2b2a0da9b687f");
            var EvilBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("f5039f03c9a49aa4098248222d3ce451");
            var FireBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("3a8dd416ce28c414da0439e3bc20f445");
            var FireBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("e36a450cb94ecee4c99047fe598907a5");
            var GoodBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("05fe64491e635f646b794d572111caa5");
            var HealingBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("012329d3dfb76ef428ccb299b80c7648");
            var LawBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("10095e0eb96644a4eb5eebdcd21a9b5e");
            var LuckBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("49fa2b54589c34a42b8f06b8de1a6639");
            var LuckBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("22259412e17c7c74c8c2abb68b4ac827");
            var MagicBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("7e4f42cfb91d9ed409b866aeceae308d");
            var NobilityBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("e024bf90eedd8034e83886f6caba4136");
            var NobilityBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("862b46568f3c9864688afaba4e6e6ba4");
            var ProtectionBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("6b2626e14b75f4a4d815be141ce3c3b7");
            var ProtectionBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("9c23340b6eed989459a648874910230a");
            var ReposeBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("df99fdc53432031458deeaa21121a847");
            var SunBlessingMajorAbilityBoth = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("fb2ff8221d4c1834c893507d0d4e1fa1");
            var SunBlessingMajorAbilityFlaming = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("6aa7160535c9dd74ca707b72c68dac89");
            var SunBlessingMajorAbilityUndeadBane = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("6eb4da544d0e87346a4dc403f9382f89");
            var SunBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("acd8d2886e0989c4096ff5a8146f8106");
            var TravelBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("17b3102eb33b8d14ba882ef8ba74fadd");
            var TrickeryBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("c630aa6a5c6566d4995585b94a5d3749");
            var WaterBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("a65af29466c7be343bc831f76d1e7dd4");
            var WaterBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("e572966d72a7f7449aa9236ec7f357d8");
            var WeatherBlessingMajorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("f9e01aec7f35ec94ea7f2b3f73f1e98f");
            var WeatherBlessingMinorAbility = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("df8099d3937d52440a5bbec98751ecde");

            var QuickenBlessing = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "QuickenBlessing", bp => {
                bp.SetName(TTTContext, "Quicken Blessing");
                bp.SetDescription(TTTContext, "You can deliver one of your blessings with greater speed.\n" +
                    "Choose one of your blessings that normally requires a standard action to use. " +
                    "You can expend two of your daily uses of blessings to deliver that blessing " +
                    "(regardless of whether it’s a minor or major effect) as a swift action instead.");
                bp.m_AllFeatures = new BlueprintFeatureReference[0];
                bp.m_Features = new BlueprintFeatureReference[0];
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddPrerequisite<PrerequisiteClassLevel>(c => {
                    c.m_CharacterClass = WarpriestClass.ToReference<BlueprintCharacterClassReference>();
                    c.Level = 10;
                });
                bp.AddComponent<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                });
            });
            QuickenBlessing.AddFeatures(
                CreateQuickenedBlessing("Air", AirBlessingFeature, AirBlessingMajorAbility, AirBlessingMajorAbility),
                CreateQuickenedBlessing("Animal", AnimalBlessingFeature, AnimalBlessingMinorAbility, AnimalBlessingMajorAbility),
                CreateQuickenedBlessing("Chaos", ChaosBlessingFeature, ChaosBlessingMinorAbility, ChaosBlessingMajorAbility),
                //CreateQuickenedBlessing("Charm", CharmBlessingFeature, CharmBlessingMinorAbility, CharmBlessingMajorAbility),
                //CreateQuickenedBlessing("Community", CommunityBlessingFeature, CommunityBlessingMinorAbility, CommunityBlessingMajorAbility),
                CreateQuickenedBlessing("Darkness", DarknessBlessingFeature, DarknessBlessingMinorAbility, DarknessBlessingMajorAbility),
                CreateQuickenedBlessing("Death", DeathBlessingFeature, DeathBlessingMinorAbility, DeathBlessingMajorAbility),
                CreateQuickenedBlessing("Destruction", DestructionBlessingFeature, DestructionBlessingMinorAbility, DestructionBlessingMajorAbility),
                CreateQuickenedBlessing("Earth", EarthBlessingFeature, EarthBlessingMinorAbility, EarthBlessingMajorAbility),
                CreateQuickenedBlessing("Evil", EvilBlessingFeature, EvilBlessingMinorAbility, EvilBlessingMajorAbility),
                CreateQuickenedBlessing("Fire", FireBlessingFeature, FireBlessingMinorAbility, FireBlessingMajorAbility),
                CreateQuickenedBlessing("Good", GoodBlessingFeature, GoodBlessingMinorAbility),
                CreateQuickenedBlessing("Healing", HealingBlessingFeature, HealingBlessingMajorAbility),
                CreateQuickenedBlessing("Law", LawBlessingFeature, LawBlessingMinorAbility),
                CreateQuickenedBlessing("Luck", LuckBlessingFeature, LuckBlessingMinorAbility, LuckBlessingMajorAbility),
                CreateQuickenedBlessing("Magic", MagicBlessingFeature, MagicBlessingMinorAbility),
                CreateQuickenedBlessing("Nobility", NobilityBlessingFeature, NobilityBlessingMinorAbility, NobilityBlessingMajorAbility),
                CreateQuickenedBlessing("Protection", ProtectionBlessingFeature, ProtectionBlessingMinorAbility, ProtectionBlessingMinorAbility),
                CreateQuickenedBlessing("Repose", ReposeBlessingFeature, ReposeBlessingMinorAbility),
                CreateQuickenedBlessing("Sun", SunBlessingFeature, SunBlessingMinorAbility, SunBlessingMajorAbilityFlaming, SunBlessingMajorAbilityUndeadBane, SunBlessingMajorAbilityBoth),
                CreateQuickenedBlessing("Travel", TravelBlessingFeature, TravelBlessingMajorAbility),
                CreateQuickenedBlessing("Trickery", TrickeryBlessingFeature, TrickeryBlessingMinorAbility),
                CreateQuickenedBlessing("Water", WaterBlessingFeature, WaterBlessingMinorAbility, WaterBlessingMajorAbility),
                CreateQuickenedBlessing("Weather", WeatherBlessingFeature, WeatherBlessingMinorAbility, WeatherBlessingMajorAbility)
            );

            if (TTTContext.AddedContent.Feats.IsDisabled("QuickenBlessing")) { return; }
            FeatTools.AddAsFeat(QuickenBlessing);
        }

        private static BlueprintFeature CreateQuickenedBlessing(string blessingName, BlueprintFeature blessing, params BlueprintAbilityReference[] powers) {
            var QuickenBlessing = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, $"QuickenBlessing{blessingName}Feature", bp => {
                bp.SetName(TTTContext, $"Quicken Blessing — {blessingName}");
                bp.SetDescription(TTTContext, "Choose one of your blessings that normally requires a standard action to use. " +
                    "You can expend two of your daily uses of blessings to deliver that blessing " +
                    "(regardless of whether it’s a minor or major effect) as a swift action instead.");
                bp.m_Icon = blessing.Icon;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat };
                bp.AddComponent<AbilityActionTypeConversion>(c => {
                    c.m_Abilities = powers;
                    c.ResourceMultiplier = 2;
                    c.ActionType = UnitCommand.CommandType.Swift;
                });
                bp.AddPrerequisiteFeature(blessing);
            });
            return QuickenBlessing;
        }
    }
}
