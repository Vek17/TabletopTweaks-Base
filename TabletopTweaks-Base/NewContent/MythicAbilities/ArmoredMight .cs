using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.Wrappers;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.NewContent.MythicAbilities {
    static class ArmoredMight {
        public static void AddArmoredMight() {
            var MythicAbilitySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var ExtraMythicAbilityMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8a6a511c55e67d04db328cc49aaad2b8");
            var icon = AssetLoader.LoadInternal(TTTContext, folder: "Feats", file: "Icon_ArmoredMight.png");

            var ArmoredMightFeature = Helpers.CreateBlueprint<BlueprintFeature>(TTTContext, "ArmoredMightFeature", bp => {
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MythicAbility };
                bp.Ranks = 1;
                bp.m_Icon = icon;
                bp.SetName(TTTContext, "Armored Might");
                bp.SetDescription(TTTContext, "You treat the armor bonus from your armor as 50% higher than normal, to a maximum increase of half your mythic rank plus one.");
                bp.AddComponent(Helpers.Create<ArmoredMightComponent>());
            });

            if (TTTContext.AddedContent.MythicAbilities.IsDisabled("ArmoredMight")) { return; }
            FeatTools.AddAsMythicAbility(ArmoredMightFeature);
        }
    }
}
