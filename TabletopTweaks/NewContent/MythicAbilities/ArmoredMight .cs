using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.Utilities;

namespace TabletopTweaks.NewContent.MythicAbilities {
    class ArmoredMight {
        public static void AddArmoredMight() {
            var MythicAbilitySelection = Resources.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var ExtraMythicAbilityMythicFeat = Resources.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");
            var icon = AssetLoader.LoadInternal("Feats", "Icon_ArmoredMight.png");

            var ArmoredMightFeature = Helpers.CreateBlueprint<BlueprintFeature>("ArmoredMightFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName("Armored Might");
                bp.SetDescription("You treat the armor bonus from your armor as 50% higher than normal, to a maximum increase of half your mythic rank plus one.");
                bp.AddComponent(Helpers.Create<ArmoredMightComponent>());
            });

            if (ModSettings.AddedContent.MythicAbilities.IsDisabled("ArmoredMight")) { return; }
            FeatTools.AddAsMythicAbility(ArmoredMightFeature);
        }
    }
}
