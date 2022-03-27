using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.Features {
    static class MartialWeaponProficencySelection {
        public static void AddMartialWeaponProficencySelection() {
            var ExoticWeaponProficiencySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9a01b6815d6c3684cb25f30b8bf20932");

            var HandaxeProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("c59205a5d18930d4b88b74d2acda2f49");
            var KukriProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("a7e822a8507e44b0a981ca55586dfad9");
            var LightHammerProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("8a43b4c0a59f7bb479d2af66d9a43ac5");
            var LightPickProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("9aca88a627afc724bb167b05aba2f6a4");
            var ShortswordProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("9e828934974f0fc4bbf7542eb0446e45");
            var StarknifeProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("7818ba3db79ac064e88fa14a2478b24b");
            var BattleaxeProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("5d1fb7b0c7a8b634b9d7903d9264895d");
            var FlailProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("6d273f46bce2e0f47a0958810dc4c7d9");
            var HeavyPickProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("aeac272bf357c1247a51e9c56af7193b");
            var LongswordProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("62e27ffd9d53e14479f73da29760f64e");
            var RapierProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("292d51f3d6a331644a8c29be0614f671");
            var ScimitarProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("75146ee0b32e5424ab77902bf86f91ee");
            var WarhammerProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("aba1be1d113ea4049b99ea92165e91dc");
            var LongbowProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("0978f630fc5d6a6409ac641137bf6659");
            var ShortbowProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("e8096942d950c8843857c2545f8dc18f");
            var FalchionProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("caff2f50d06e4069ab18dc05cc97a966");
            var GlaiveProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("38d4d143e7f293249b72694ddb1e0a32");
            var GreataxeProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("70ab8880eaf6c0640887ae586556a652");
            var GreatswordProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("f35e15b1fdff0c54087746c2da80a053");
            var HeavyFlailProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("a22e30bd35fbb704cab2d7e3c00717c1");
            var ScytheProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("96c174b0ebca7b246b82d4bc4aac4574");
            var TridentProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("f9565a97342ac594e9b6a495368c1a57");
            var ThrowingAxeProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("579ab5b0c5bbce445a5a9bee1b1fe057");

            var EarthBreakerProficiency = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "EarthBreakerProficiency", bp => {
                bp.SetName(TTTContext, "Weapon Proficiency (Earth Breaker)");
                bp.SetDescription(TTTContext, "You become proficient with earth breakers and can use them as a weapon.");
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.m_Icon = ExoticWeaponProficiencySelection.Icon;
                bp.AddComponent<AddProficiencies>(c => {
                    c.m_RaceRestriction = new Kingmaker.Blueprints.BlueprintRaceReference();
                    c.ArmorProficiencies = new ArmorProficiencyGroup[0];
                    c.WeaponProficiencies = new WeaponCategory[] { WeaponCategory.EarthBreaker };
                });
            });
            var BardicheProficiency = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "BardicheProficiency", bp => {
                bp.SetName(TTTContext, "Weapon Proficiency (Bardiche)");
                bp.SetDescription(TTTContext, "You become proficient with bardiches and can use them as a weapon.");
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.m_Icon = ExoticWeaponProficiencySelection.Icon;
                bp.AddComponent<AddProficiencies>(c => {
                    c.m_RaceRestriction = new Kingmaker.Blueprints.BlueprintRaceReference();
                    c.ArmorProficiencies = new ArmorProficiencyGroup[0];
                    c.WeaponProficiencies = new WeaponCategory[] { WeaponCategory.Bardiche };
                });
            });
            var MartialWeaponProficencySelection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(TTTContext, "MartialWeaponProficencySelection", bp => {
                bp.SetName(TTTContext, "Martial Weapon Proficency Selection");
                bp.SetDescription(TTTContext, "");
                bp.Groups = new FeatureGroup[0];
                bp.IsClassFeature = true;
                bp.AddFeatures(
                    HandaxeProficiency,
                    KukriProficiency,
                    LightHammerProficiency,
                    LightPickProficiency,
                    ShortswordProficiency,
                    StarknifeProficiency,
                    BattleaxeProficiency,
                    FlailProficiency,
                    HeavyPickProficiency,
                    LongswordProficiency,
                    RapierProficiency,
                    ScimitarProficiency,
                    WarhammerProficiency,
                    LongbowProficiency,
                    ShortbowProficiency,
                    FalchionProficiency,
                    GlaiveProficiency,
                    GreataxeProficiency,
                    GreatswordProficiency,
                    HeavyFlailProficiency,
                    ScytheProficiency,
                    TridentProficiency,
                    ThrowingAxeProficiency,
                    EarthBreakerProficiency,
                    BardicheProficiency
                );
            });
        }
    }
}
