using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.Utilities;
using static Kingmaker.Designers.Mechanics.Facts.AutoMetamagic;

namespace TabletopTweaks.NewContent.MythicAbilities {
    static class EnhancedBlessings {
        public static void AddEnhancedBlessings() {
            var DomainMastery = Resources.GetBlueprint<BlueprintFeature>("2de64f6a1f2baee4f9b7e52e3f046ec5");
            var BlessingSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("6d9dcc2a59210a14891aeedb09d406aa");
            var blessingAbilities = new BlueprintAbilityReference[] {
                Resources.GetBlueprintReference<BlueprintAbilityReference>("006cfb97660cd38438386bd46488cc23"),    // AirBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("364e1e0e4af5b004caabe5f005cee7ca"),    // AirBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("93f0098fe08b94f41a351a4fbb00518a"),    // AnimalBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("56a2584706b8faf4094acfa142747f70"),    // AnimalBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("6fb60e6cb81138941a07e6861df839c6"),    // ChaosBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("39a1a4085aabd154e8aa565cb1579e5e"),    // ChaosBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("d58214dd4e8bd7242b9129eed67d1b61"),    // CharmBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("5aab5321b19e82545a65f203181c3470"),    // CharmBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("c65a2844952e3a94293cd7c42067eeef"),    // CommunityBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("7b9d788a9f7f6b943a5bfa42aa0febed"),    // CommunityBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("1d225432d2bfccd4da91959a9b9378bd"),    // DarknessBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("a91bb1ca61938b44bb236b0a52fe0188"),    // DarknessBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("04dd71ac77051bc46aab114d200e65dd"),    // DeathBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("d3dba848088e1a64582f76108b778fd0"),    // DeathBlessingMajorAbilitySwift,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("542a9661ed997ff418322ff7376bab8c"),    // DeathBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("b4bf40373f371074a87fc6ce554287be"),    // DestructionBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("15559878dd3ac2943b17a5d52ffe6b8a"),    // DestructionBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("a9858668656a56f41a87652d2cd235c0"),    // DivineHuntersBlessingAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("3313addb536171f4d86bb133ec51b008"),    // EarthBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("4cfa41e8557d3c54d9af27af6964bd7e"),    // EarthBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("3ef665bb337d96946bcf98a11103f32f"),    // EcclesitheurgeBlessingLongAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("db0f61cd985ca09498cafde9a4b27a16"),    // EcclesitheurgeBlessingShortAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("c9bbd75a934c6b44fbd2b2a0da9b687f"),    // EvilBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("f5039f03c9a49aa4098248222d3ce451"),    // EvilBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("3a8dd416ce28c414da0439e3bc20f445"),    // FireBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("e36a450cb94ecee4c99047fe598907a5"),    // FireBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("d301ae123b51fe349be81cda5de6758e"),    // GoodBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("05fe64491e635f646b794d572111caa5"),    // GoodBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("012329d3dfb76ef428ccb299b80c7648"),    // HealingBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("3fea9eba5180a32479b1e8e212ed3923"),    // HealingBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("2485e731fecacff459941fb88940c46b"),    // LawBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("10095e0eb96644a4eb5eebdcd21a9b5e"),    // LawBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("2c482a6d6f94bdc4da862d83e9c50c33"),    // LiberationBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("f653839633eb7be48aec20af21aa47c3"),    // LiberationBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("49fa2b54589c34a42b8f06b8de1a6639"),    // LuckBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("22259412e17c7c74c8c2abb68b4ac827"),    // LuckBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("e3d67686b75c49829d279b0a5b4d950a"),    // MadnessBlessingMajorAbilityActNormally,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("445e480535614b99b6814415c1f1f23d"),    // MadnessBlessingMajorAbilityAttackNearestCreature,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("8cc988fc15454f4ba3dc87f7efdcc524"),    // MadnessBlessingMajorAbilityDealDamageToSelf,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("8e66b90b48754155be52c67347dadc4d"),    // MadnessBlessingMajorAbilityDoNothing,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("43b768746ce946b28fb5eaad72734cbe"),    // MadnessBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("f67e58b9bb7003a4aaea0deba52b3fd9"),    // MagicBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("7e4f42cfb91d9ed409b866aeceae308d"),    // MagicBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("e024bf90eedd8034e83886f6caba4136"),    // NobilityBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("862b46568f3c9864688afaba4e6e6ba4"),    // NobilityBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("6b2626e14b75f4a4d815be141ce3c3b7"),    // ProtectionBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("9c23340b6eed989459a648874910230a"),    // ProtectionBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("73acbf947a0bfc84b804b1605d6eeee2"),    // ReposeBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("df99fdc53432031458deeaa21121a847"),    // ReposeBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("0d46406cfc7197944bd75ab76d6abc04"),    // StrengthBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("5bf8156358e448a4c8b9ebc347471228"),    // StrengthBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("fb2ff8221d4c1834c893507d0d4e1fa1"),    // SunBlessingMajorAbilityBoth,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("6aa7160535c9dd74ca707b72c68dac89"),    // SunBlessingMajorAbilityFlaming,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("6eb4da544d0e87346a4dc403f9382f89"),    // SunBlessingMajorAbilityUndeadBane,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("acd8d2886e0989c4096ff5a8146f8106"),    // SunBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("17b3102eb33b8d14ba882ef8ba74fadd"),    // TravelBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("ab261014b90b1de44b4f749f5d6167d2"),    // TravelBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("443789a335451bc42b8281b2d1f4ca37"),    // TrickeryBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("c630aa6a5c6566d4995585b94a5d3749"),    // TrickeryBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("a65af29466c7be343bc831f76d1e7dd4"),    // WaterBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("e572966d72a7f7449aa9236ec7f357d8"),    // WaterBlessingMinorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("f9e01aec7f35ec94ea7f2b3f73f1e98f"),    // WeatherBlessingMajorAbility,
                Resources.GetBlueprintReference<BlueprintAbilityReference>("df8099d3937d52440a5bbec98751ecde"),    // WeatherBlessingMinorAbility,
            };
            var EnhancedBlessingsFeature = Helpers.CreateBlueprint<BlueprintFeature>("EnhancedBlessingsFeature", bp => {
                bp.m_Icon = DomainMastery.m_Icon;
                bp.SetName("Enhanced Blessings");
                bp.SetDescription("You have moved closer to your deity and to the power it has over its blessings.\n" +
                    "The effects from your blessings now last twice as long.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicFeat };
                bp.AddComponent<AutoMetamagic>(c => {
                    c.m_AllowedAbilities = AllowedType.Any;
                    c.m_Spellbook = BlueprintReferenceBase.CreateTyped<BlueprintSpellbookReference>(null);
                    c.Metamagic = Metamagic.Extend;
                    c.Abilities = blessingAbilities.ToList();
                });
                bp.AddPrerequisiteFeature(BlessingSelection);
            });
            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("EnhancedBlessings")) { return; }
            FeatTools.AddAsMythicAbility(EnhancedBlessingsFeature);
        }
    }
}
